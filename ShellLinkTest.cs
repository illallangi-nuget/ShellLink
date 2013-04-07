/**************************************************************************
*
* Filename:     ShellLinkTest.cs
* Project:      ShellLinkSample
* Author:       Mattias Sjögren (mattias@mvps.org)
*               http://www.msjogren.net/dotnet/
*
* Description:  Simple shortcut editor to demonstrate how to use
*               the ShellShortcut class.
*
* Public types: class ShellLinkTest
*
*
* Dependencies: ShellShortcut.cs
*
*
* Copyright ©2001-2002, Mattias Sjögren
* 
**************************************************************************/

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using MSjogren.Samples.ShellLink;


public class ShellLinkTest : System.Windows.Forms.Form
{
  private ShellShortcut m_Shortcut;

	public ShellLinkTest()
	{
		//
		// Required for Windows Form Designer support
		//
		InitializeComponent();

    cboWinStyle.Items.Add( ProcessWindowStyle.Normal );
    cboWinStyle.Items.Add( ProcessWindowStyle.Minimized );
    cboWinStyle.Items.Add( ProcessWindowStyle.Maximized );
  }

	/// <summary>
	///   Clean up any resources being used.
	/// </summary>
	protected override void Dispose(bool disposing)
	{
	  if ( disposing ) {
	    if ( components != null ) 
	      components.Dispose();
    }	    
		base.Dispose( disposing );
	}

	#region Windows Form Designer generated code

  private System.Windows.Forms.TextBox txtDescription;
  private System.Windows.Forms.TextBox txtWorkingDir;
  private System.Windows.Forms.TextBox txtArgs;
  private System.Windows.Forms.TextBox txtIconFile;
  private System.Windows.Forms.TextBox txtTarget;
  private System.Windows.Forms.Label lblCurrentFile;
  private System.Windows.Forms.PictureBox picIcon;
  private System.Windows.Forms.Label Label7;
  private System.Windows.Forms.Label Label1;
  private System.Windows.Forms.Label Label2;
  private System.Windows.Forms.Label Label3;
  private System.Windows.Forms.Label Label9;
  private System.Windows.Forms.Label Label4;
  private System.Windows.Forms.Label Label5;
  private System.Windows.Forms.Label lblHotkey;
  private System.Windows.Forms.Label Label6;
  private System.Windows.Forms.TextBox txtIconIdx;
  private System.Windows.Forms.ComboBox cboWinStyle;
  private System.Windows.Forms.Button btnBrowseTarget;
  private System.Windows.Forms.Button btnOpen;
  private System.Windows.Forms.Button btnBrowseIcon;
  private System.Windows.Forms.Button btnSave;
	/// <summary>
	/// Required designer variable.
	/// </summary>
	private System.ComponentModel.Container components;

	/// <summary>
	/// Required method for Designer support - do not modify
	/// the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent()
	{
    this.txtDescription = new System.Windows.Forms.TextBox();
    this.cboWinStyle = new System.Windows.Forms.ComboBox();
    this.txtIconIdx = new System.Windows.Forms.TextBox();
    this.Label9 = new System.Windows.Forms.Label();
    this.btnOpen = new System.Windows.Forms.Button();
    this.Label5 = new System.Windows.Forms.Label();
    this.btnBrowseTarget = new System.Windows.Forms.Button();
    this.Label7 = new System.Windows.Forms.Label();
    this.txtIconFile = new System.Windows.Forms.TextBox();
    this.Label1 = new System.Windows.Forms.Label();
    this.Label2 = new System.Windows.Forms.Label();
    this.Label3 = new System.Windows.Forms.Label();
    this.lblHotkey = new System.Windows.Forms.Label();
    this.Label4 = new System.Windows.Forms.Label();
    this.Label6 = new System.Windows.Forms.Label();
    this.btnSave = new System.Windows.Forms.Button();
    this.txtArgs = new System.Windows.Forms.TextBox();
    this.txtTarget = new System.Windows.Forms.TextBox();
    this.picIcon = new System.Windows.Forms.PictureBox();
    this.btnBrowseIcon = new System.Windows.Forms.Button();
    this.lblCurrentFile = new System.Windows.Forms.Label();
    this.txtWorkingDir = new System.Windows.Forms.TextBox();
    this.SuspendLayout();
    // 
    // txtDescription
    // 
    this.txtDescription.Location = new System.Drawing.Point(80, 120);
    this.txtDescription.Name = "txtDescription";
    this.txtDescription.Size = new System.Drawing.Size(328, 20);
    this.txtDescription.TabIndex = 4;
    this.txtDescription.Text = "";
    // 
    // cboWinStyle
    // 
    this.cboWinStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
    this.cboWinStyle.DropDownWidth = 160;
    this.cboWinStyle.Location = new System.Drawing.Point(248, 168);
    this.cboWinStyle.Name = "cboWinStyle";
    this.cboWinStyle.Size = new System.Drawing.Size(160, 21);
    this.cboWinStyle.TabIndex = 8;
    // 
    // txtIconIdx
    // 
    this.txtIconIdx.Location = new System.Drawing.Point(80, 168);
    this.txtIconIdx.Name = "txtIconIdx";
    this.txtIconIdx.Size = new System.Drawing.Size(64, 20);
    this.txtIconIdx.TabIndex = 7;
    this.txtIconIdx.Text = "";
    // 
    // Label9
    // 
    this.Label9.Location = new System.Drawing.Point(8, 124);
    this.Label9.Name = "Label9";
    this.Label9.TabIndex = 11;
    this.Label9.Text = "Description:";
    // 
    // btnOpen
    // 
    this.btnOpen.Location = new System.Drawing.Point(208, 240);
    this.btnOpen.Name = "btnOpen";
    this.btnOpen.Size = new System.Drawing.Size(96, 23);
    this.btnOpen.TabIndex = 9;
    this.btnOpen.Text = "&Open/Create";
    this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
    // 
    // Label5
    // 
    this.Label5.Location = new System.Drawing.Point(8, 172);
    this.Label5.Name = "Label5";
    this.Label5.TabIndex = 12;
    this.Label5.Text = "Icon index:";
    // 
    // btnBrowseTarget
    // 
    this.btnBrowseTarget.Enabled = false;
    this.btnBrowseTarget.Location = new System.Drawing.Point(384, 48);
    this.btnBrowseTarget.Name = "btnBrowseTarget";
    this.btnBrowseTarget.Size = new System.Drawing.Size(24, 20);
    this.btnBrowseTarget.TabIndex = 1;
    this.btnBrowseTarget.Text = "...";
    this.btnBrowseTarget.Click += new System.EventHandler(this.BrowseForFile);
    // 
    // Label7
    // 
    this.Label7.Location = new System.Drawing.Point(8, 196);
    this.Label7.Name = "Label7";
    this.Label7.Size = new System.Drawing.Size(64, 23);
    this.Label7.TabIndex = 15;
    this.Label7.Text = "Hotkey:";
    // 
    // txtIconFile
    // 
    this.txtIconFile.Location = new System.Drawing.Point(80, 144);
    this.txtIconFile.Name = "txtIconFile";
    this.txtIconFile.Size = new System.Drawing.Size(296, 20);
    this.txtIconFile.TabIndex = 5;
    this.txtIconFile.Text = "";
    // 
    // Label1
    // 
    this.Label1.Location = new System.Drawing.Point(8, 52);
    this.Label1.Name = "Label1";
    this.Label1.TabIndex = 8;
    this.Label1.Text = "Target:";
    // 
    // Label2
    // 
    this.Label2.Location = new System.Drawing.Point(8, 76);
    this.Label2.Name = "Label2";
    this.Label2.TabIndex = 9;
    this.Label2.Text = "Working dir:";
    // 
    // Label3
    // 
    this.Label3.Location = new System.Drawing.Point(8, 100);
    this.Label3.Name = "Label3";
    this.Label3.TabIndex = 10;
    this.Label3.Text = "Arguments:";
    // 
    // lblHotkey
    // 
    this.lblHotkey.Location = new System.Drawing.Point(80, 196);
    this.lblHotkey.Name = "lblHotkey";
    this.lblHotkey.Size = new System.Drawing.Size(112, 23);
    this.lblHotkey.TabIndex = 17;
    // 
    // Label4
    // 
    this.Label4.Location = new System.Drawing.Point(8, 148);
    this.Label4.Name = "Label4";
    this.Label4.TabIndex = 11;
    this.Label4.Text = "Icon file:";
    // 
    // Label6
    // 
    this.Label6.Location = new System.Drawing.Point(168, 172);
    this.Label6.Name = "Label6";
    this.Label6.TabIndex = 14;
    this.Label6.Text = "Window style:";
    // 
    // btnSave
    // 
    this.btnSave.Enabled = false;
    this.btnSave.Location = new System.Drawing.Point(312, 240);
    this.btnSave.Name = "btnSave";
    this.btnSave.Size = new System.Drawing.Size(96, 23);
    this.btnSave.TabIndex = 10;
    this.btnSave.Text = "&Save";
    this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
    // 
    // txtArgs
    // 
    this.txtArgs.Location = new System.Drawing.Point(80, 96);
    this.txtArgs.Name = "txtArgs";
    this.txtArgs.Size = new System.Drawing.Size(328, 20);
    this.txtArgs.TabIndex = 3;
    this.txtArgs.Text = "";
    // 
    // txtTarget
    // 
    this.txtTarget.Location = new System.Drawing.Point(80, 48);
    this.txtTarget.Name = "txtTarget";
    this.txtTarget.Size = new System.Drawing.Size(296, 20);
    this.txtTarget.TabIndex = 0;
    this.txtTarget.Text = "";
    // 
    // picIcon
    // 
    this.picIcon.Location = new System.Drawing.Point(8, 8);
    this.picIcon.Name = "picIcon";
    this.picIcon.Size = new System.Drawing.Size(32, 32);
    this.picIcon.TabIndex = 18;
    this.picIcon.TabStop = false;
    // 
    // btnBrowseIcon
    // 
    this.btnBrowseIcon.Enabled = false;
    this.btnBrowseIcon.Location = new System.Drawing.Point(384, 144);
    this.btnBrowseIcon.Name = "btnBrowseIcon";
    this.btnBrowseIcon.Size = new System.Drawing.Size(24, 20);
    this.btnBrowseIcon.TabIndex = 6;
    this.btnBrowseIcon.Text = "...";
    this.btnBrowseIcon.Click += new System.EventHandler(this.BrowseForFile);
    // 
    // lblCurrentFile
    // 
    this.lblCurrentFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
    this.lblCurrentFile.ForeColor = System.Drawing.Color.Blue;
    this.lblCurrentFile.Location = new System.Drawing.Point(64, 8);
    this.lblCurrentFile.Name = "lblCurrentFile";
    this.lblCurrentFile.Size = new System.Drawing.Size(344, 23);
    this.lblCurrentFile.TabIndex = 16;
    // 
    // txtWorkingDir
    // 
    this.txtWorkingDir.Location = new System.Drawing.Point(80, 72);
    this.txtWorkingDir.Name = "txtWorkingDir";
    this.txtWorkingDir.Size = new System.Drawing.Size(328, 20);
    this.txtWorkingDir.TabIndex = 2;
    this.txtWorkingDir.Text = "";
    // 
    // ShellLinkTest
    // 
    this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
    this.ClientSize = new System.Drawing.Size(420, 273);
    this.Controls.AddRange(new System.Windows.Forms.Control[] {this.btnSave,
                                                                this.btnBrowseIcon,
                                                                this.btnOpen,
                                                                this.btnBrowseTarget,
                                                                this.cboWinStyle,
                                                                this.txtIconIdx,
                                                                this.Label6,
                                                                this.lblHotkey,
                                                                this.picIcon,
                                                                this.lblCurrentFile,
                                                                this.txtTarget,
                                                                this.txtIconFile,
                                                                this.txtArgs,
                                                                this.txtWorkingDir,
                                                                this.txtDescription,
                                                                this.Label5,
                                                                this.Label4,
                                                                this.Label9,
                                                                this.Label3,
                                                                this.Label2,
                                                                this.Label1,
                                                                this.Label7});
    this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
    this.MaximizeBox = false;
    this.Name = "ShellLinkTest";
    this.Text = "ShellLink sample";
    this.ResumeLayout(false);

  }
	#endregion


  protected void btnOpen_Click(object sender, EventArgs e)
  {
    OpenFileDialog ofd = new OpenFileDialog();

    ofd.CheckFileExists = false;
    ofd.DefaultExt = "lnk";
    ofd.DereferenceLinks = false;
    ofd.Title = "Select a shortcut file";
    ofd.Filter = "Shortcuts (*.lnk)|*.lnk|All files (*.*)|*.*";
    ofd.FilterIndex = 1;

    if ( ofd.ShowDialog() == DialogResult.OK ) {
      m_Shortcut = new ShellShortcut( ofd.FileName );
      lblCurrentFile.Text = ofd.FileName;

      txtTarget.Text = m_Shortcut.Path;
      txtWorkingDir.Text = m_Shortcut.WorkingDirectory;
      txtArgs.Text = m_Shortcut.Arguments;
      txtDescription.Text = m_Shortcut.Description;
      txtIconFile.Text = m_Shortcut.IconPath;
      txtIconIdx.Text = m_Shortcut.IconIndex.ToString();

      cboWinStyle.SelectedIndex = cboWinStyle.Items.IndexOf( m_Shortcut.WindowStyle );
      lblHotkey.Text = TypeDescriptor.GetConverter( typeof(Keys) ).ConvertToString( m_Shortcut.Hotkey );

      Icon ico = m_Shortcut.Icon;
      if ( ico != null ) {
        picIcon.Image = ico.ToBitmap();
        ico.Dispose();
      }
      else
        picIcon.Image = null;

      btnSave.Enabled = true;
      btnBrowseTarget.Enabled = true;
      btnBrowseIcon.Enabled = true;
    }

    ofd.Dispose();
  }

  protected void btnSave_Click(object sender, EventArgs e)
  {
    m_Shortcut.Path = txtTarget.Text;
    m_Shortcut.WorkingDirectory = txtWorkingDir.Text;
    m_Shortcut.Arguments = txtArgs.Text;
    m_Shortcut.Description = txtDescription.Text;
    m_Shortcut.IconPath = txtIconFile.Text;
    m_Shortcut.IconIndex = Convert.ToInt32( txtIconIdx.Text );
    m_Shortcut.WindowStyle = (ProcessWindowStyle) cboWinStyle.SelectedItem;
    m_Shortcut.Save();

    Icon ico = m_Shortcut.Icon;
    if ( ico != null ) {
      picIcon.Image = ico.ToBitmap();
      ico.Dispose();
    }
    else
      picIcon.Image = null;
  }

  /// <summary>
  ///   Handles the Click event of btnBrowseTarget and btnBrowseIcon.
  ///   Used to browse for a target or icon file.
  /// </summary>
  protected void BrowseForFile(object sender, EventArgs e)
  {
    OpenFileDialog ofd = new OpenFileDialog();

    if ( sender.Equals( btnBrowseTarget ) ) {
      ofd.Title = "Select a target file";
      ofd.Filter = "All files (*.*)|*.*";
    }
    else {
      ofd.Title = "Select an icon file or library";
      ofd.Filter = "Icon files|*.ico;*.dll;*.exe|All files (*.*)|*.*";
    }
    ofd.FilterIndex = 1;
    ofd.CheckFileExists = true;
    
    if ( ofd.ShowDialog() == DialogResult.OK ) {
      if ( sender.Equals( btnBrowseTarget ) )
        txtTarget.Text = ofd.FileName;
      else
        txtIconFile.Text = ofd.FileName;
    }
    
    ofd.Dispose();
  }

	/// <summary>
	///   The main entry point for the application.
	/// </summary>
	[STAThread]
	static void Main() 
	{
		Application.Run( new ShellLinkTest() );
	}
}
