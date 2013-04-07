/**************************************************************************
*
* Filename:     ShellShortcut.cs
* Author:       Mattias Sjögren (mattias@mvps.org)
*               http://www.msjogren.net/dotnet/
*
* Description:  Defines a .NET friendly class, ShellShortcut, for reading
*               and writing shortcuts.
*               Define the conditional compilation symbol UNICODE to use
*               IShellLinkW internally.
*
* Public types: class ShellShortcut
*
*
* Dependencies: ShellLinkNative.cs
*
*
* Copyright ©2001-2002, Mattias Sjögren
* 
**************************************************************************/

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Illallangi.ShellLink
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>.NET friendly wrapper for the ShellLink class</summary>
    public class ShellShortcut : IDisposable
    {
        #region Fields

        private const int Infotipsize = 1024;
        private const int MaxPath = 260;
        private const int SwShownormal = 1;
        private const int SwShowminimized = 2;
        private const int SwShowmaximized = 3;
        private const int SwShowminnoactive = 7;

        private IShellLinkA currentShellLink;
        private readonly string currentShellPath;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellShortcut"/> class. 
        /// </summary>
        /// <param name="linkPath">
        /// Path to new or existing shortcut file.
        /// </param>
        public ShellShortcut(string linkPath)
        {
            this.currentShellPath = linkPath;
            this.currentShellLink = (IShellLinkA)new ShellLink();

            if (!File.Exists(linkPath))
            {
                return;
            }

            ((IPersistFile)this.currentShellLink).Load(linkPath, 0);
        }

        #endregion
        
        #region Properties

        /// <summary>
        /// Gets or sets the argument list of the shortcut.
        /// </summary>
        /// <value>
        /// The argument list of the shortcut.
        /// </value>
        public string Arguments
        {
            get
            {
                var sb = new StringBuilder(Infotipsize);
                this.currentShellLink.GetArguments(sb, sb.Capacity);
                return sb.ToString();
            }

            set
            {
                this.currentShellLink.SetArguments(value);
            }
        }

        /// <value>
        ///   Gets or sets a description of the shortcut.
        /// </value>
        public string Description
        {
            get
            {
                var sb = new StringBuilder(Infotipsize);
                this.currentShellLink.GetDescription(sb, sb.Capacity);
                return sb.ToString();
            }

            set
            {
                this.currentShellLink.SetDescription(value);
            }
        }

        /// <summary>
        /// Gets or sets the working directory (aka start in directory) of the shortcut.
        /// </summary>
        /// <value>
        /// The working directory (aka start in directory) of the shortcut.
        /// </value>
        public string WorkingDirectory
        {
            get
            {
                var sb = new StringBuilder(MaxPath);
                this.currentShellLink.GetWorkingDirectory(sb, sb.Capacity);
                return sb.ToString();
            }

            set
            {
                this.currentShellLink.SetWorkingDirectory(value);
            }
        }

        /// <summary>
        /// Gets or sets the target path of the shortcut.
        /// </summary>
        /// <value>
        /// The target path of the shortcut.
        /// </value>
        /// <comment>
        /// If Path returns an empty string, the shortcut is associated with
        /// a PIDL instead, which can be retrieved with IShellLink.GetIDList().
        /// This is beyond the scope of this wrapper class.
        /// </comment>
        public string Path
        {
            get
            {
                WIN32_FIND_DATAA wfd;
                var sb = new StringBuilder(MaxPath);
                this.currentShellLink.GetPath(sb, sb.Capacity, out wfd, SLGP_FLAGS.SLGP_UNCPRIORITY);
                return sb.ToString();
            }

            set
            {
                this.currentShellLink.SetPath(value);
            }
        }

        /// <summary>
        /// Gets or sets the path of the <see cref="Icon"/> assigned to the shortcut. <seealso cref="IconIndex"/>
        /// </summary>
        /// <value>
        /// The path of the <see cref="Icon"/> assigned to the shortcut.
        /// </value>
        public string IconPath
        {
            get
            {
                var sb = new StringBuilder(MaxPath);
                int iconIdx;
                this.currentShellLink.GetIconLocation(sb, sb.Capacity, out iconIdx);
                return sb.ToString();
            }

            set
            {
                this.currentShellLink.SetIconLocation(value, this.IconIndex);
            }
        }

        /// <value>
        /// The index of the <see cref="Icon"/> assigned to the shortcut.
        /// </value>
        /// <summary>
        /// Gets or sets the index of the <see cref="Icon"/> assigned to the shortcut.
        /// Set to zero when the <see cref="IconPath"/> property specifies a .ICO file.
        /// <seealso cref="IconPath"/>
        /// </summary>
        public int IconIndex
        {
            get
            {
                var sb = new StringBuilder(MaxPath);
                int iconIdx;
                this.currentShellLink.GetIconLocation(sb, sb.Capacity, out iconIdx);
                return iconIdx;
            }

            set
            {
                this.currentShellLink.SetIconLocation(this.IconPath, value);
            }
        }

        /// <summary>
        /// Gets the Icon of the shortcut as it will appear in Explorer.
        /// Use the <see cref="IconPath"/> and <see cref="IconIndex"/>
        /// properties to change it.
        /// </summary>
        /// <value>
        /// The Icon of the shortcut as it will appear in Explorer.
        /// </value>
        public Icon Icon
        {
            get
            {
                var sb = new StringBuilder(MaxPath);
                int iconIdx;

                this.currentShellLink.GetIconLocation(sb, sb.Capacity, out iconIdx);

                var inst = Marshal.GetHINSTANCE(this.GetType().Module);
                var icon = Native.ExtractIcon(inst, sb.ToString(), iconIdx);
                if (icon == IntPtr.Zero)
                {
                    return null;
                }

                // Return a cloned Icon, because we have to free the original ourselves.
                var ico = Icon.FromHandle(icon);
                var clone = (Icon)ico.Clone();
                ico.Dispose();
                Native.DestroyIcon(icon);
                return clone;
            }
        }

        /// <summary>
        /// Gets or sets the System.Diagnostics.ProcessWindowStyle value
        /// that decides the initial show state of the shortcut target. Note that
        /// ProcessWindowStyle.Hidden is not a valid property value.
        /// </summary>
        /// <value>
        /// The System.Diagnostics.ProcessWindowStyle value
        /// that decides the initial show state of the shortcut target.
        /// </value>
        public ProcessWindowStyle WindowStyle
        {
            get
            {
                int ws;
                this.currentShellLink.GetShowCmd(out ws);

                switch (ws)
                {
                    case SwShowminimized:
                    case SwShowminnoactive:
                        return ProcessWindowStyle.Minimized;

                    case SwShowmaximized:
                        return ProcessWindowStyle.Maximized;

                    default:
                        return ProcessWindowStyle.Normal;
                }
            }

            set
            {
                int ws;

                switch (value)
                {
                    case ProcessWindowStyle.Normal:
                        ws = SwShownormal;
                        break;

                    case ProcessWindowStyle.Minimized:
                        ws = SwShowminnoactive;
                        break;

                    case ProcessWindowStyle.Maximized:
                        ws = SwShowmaximized;
                        break;

                    default: // ProcessWindowStyle.Hidden
                        throw new ArgumentException("Unsupported ProcessWindowStyle value.");
                }

                this.currentShellLink.SetShowCmd(ws);
            }
        }

        /// <summary>
        /// Gets or sets the hotkey for the shortcut.
        /// </summary>
        /// <value>
        /// The hotkey for the shortcut.
        /// </value>
        public Keys Hotkey
        {
            get
            {
                short hotkey;
                this.currentShellLink.GetHotkey(out hotkey);

                // Convert from IShellLink 16-bit format to Keys enumeration 32-bit value
                // IShellLink: 0xMMVK
                // Keys:  0x00MM00VK        
                //   MM = Modifier (Alt, Control, Shift)
                //   VK = Virtual key code
                return (Keys)(((hotkey & 0xFF00) << 8) | (hotkey & 0xFF));
            }

            set
            {
                if ((value & Keys.Modifiers) == 0)
                {
                    throw new ArgumentException("Hotkey must include a modifier key.");
                }

                // Convert from Keys enumeration 32-bit value to IShellLink 16-bit format
                // IShellLink: 0xMMVK
                // Keys:  0x00MM00VK        
                //   MM = Modifier (Alt, Control, Shift)
                //   VK = Virtual key code
                this.currentShellLink.SetHotkey(unchecked((short)(((int)(value & Keys.Modifiers) >> 8) | (int)(value & Keys.KeyCode))));
            }
        }

        #endregion
        
        #region Methods
        
        /// <summary>
        ///   Saves the shortcut to disk.
        /// </summary>
        public void Save()
        {
            var pf = (IPersistFile)this.currentShellLink;
            pf.Save(this.currentShellPath, true);
        }

        /// <summary>
        /// Implementation of the IDispose interface.
        /// </summary>
        public void Dispose()
        {
            if (this.currentShellLink == null)
            {
                return;
            }

            Marshal.ReleaseComObject(this.currentShellLink);
            this.currentShellLink = null;
        }

        #endregion

        #region Classes

        /// <summary>
        /// Native win32 operations.
        /// </summary>
        private static class Native
        {
            [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Win32 Native operation.")]
            [DllImport("shell32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr ExtractIcon(IntPtr hInst, string lpszExeFileName, int nIconIndex);

            [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Win32 Native operation.")]
            [DllImport("user32.dll")]
            public static extern bool DestroyIcon(IntPtr hIcon);
        }

        #endregion
    }
}
