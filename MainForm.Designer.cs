namespace ParseGet
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        	this.components = new System.ComponentModel.Container();
        	System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        	this.ImageList = new System.Windows.Forms.ImageList(this.components);
        	this.btnSavePath = new System.Windows.Forms.ToolStripButton();
        	this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
        	this.URL = new System.Windows.Forms.ToolStripTextBox();
        	this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
        	this.lbTask = new System.Windows.Forms.ToolStripLabel();
        	this.MaxDownloaders = new System.Windows.Forms.ToolStripComboBox();
        	this.FBD = new System.Windows.Forms.FolderBrowserDialog();
        	this.Tray = new System.Windows.Forms.NotifyIcon(this.components);
        	this.Toolbar = new System.Windows.Forms.ToolStrip();
        	this.lbSplit = new System.Windows.Forms.ToolStripLabel();
        	this.MaxConnections = new System.Windows.Forms.ToolStripComboBox();
        	this.btnOption = new System.Windows.Forms.ToolStripDropDownButton();
        	this.miShutdown = new System.Windows.Forms.ToolStripMenuItem();
        	this.miEditINI = new System.Windows.Forms.ToolStripMenuItem();
        	this.miTrace = new System.Windows.Forms.ToolStripMenuItem();
        	this.debug = new System.Windows.Forms.ToolStripMenuItem();
        	this.info = new System.Windows.Forms.ToolStripMenuItem();
        	this.notice = new System.Windows.Forms.ToolStripMenuItem();
        	this.warn = new System.Windows.Forms.ToolStripMenuItem();
        	this.error = new System.Windows.Forms.ToolStripMenuItem();
        	this.miLog = new System.Windows.Forms.ToolStripMenuItem();
        	this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
        	this.miProxy = new System.Windows.Forms.ToolStripMenuItem();
        	this.miConnection = new System.Windows.Forms.ToolStripMenuItem();
        	this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
        	this.miClipboard = new System.Windows.Forms.ToolStripMenuItem();
        	this.miParseOnly = new System.Windows.Forms.ToolStripMenuItem();
        	this.miCopyURL = new System.Windows.Forms.ToolStripMenuItem();
        	this.miRunDownloader = new System.Windows.Forms.ToolStripMenuItem();
        	this.miSeparator0 = new System.Windows.Forms.ToolStripSeparator();
        	this.miStayTop = new System.Windows.Forms.ToolStripMenuItem();
        	this.miLanguage = new System.Windows.Forms.ToolStripMenuItem();
        	this.miLangAuto = new System.Windows.Forms.ToolStripMenuItem();
        	this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
        	this.miLangChs = new System.Windows.Forms.ToolStripMenuItem();
        	this.miLangEn = new System.Windows.Forms.ToolStripMenuItem();
        	this.btnReload = new System.Windows.Forms.ToolStripButton();
        	this.cmTaskList = new System.Windows.Forms.ContextMenuStrip(this.components);
        	this.miOpen = new System.Windows.Forms.ToolStripMenuItem();
        	this.miOpenDir = new System.Windows.Forms.ToolStripMenuItem();
        	this.miSeparator1 = new System.Windows.Forms.ToolStripSeparator();
        	this.miMoveTop = new System.Windows.Forms.ToolStripMenuItem();
        	this.miMoveUp = new System.Windows.Forms.ToolStripMenuItem();
        	this.miMoveDown = new System.Windows.Forms.ToolStripMenuItem();
        	this.miMoveBottom = new System.Windows.Forms.ToolStripMenuItem();
        	this.miSeparator2 = new System.Windows.Forms.ToolStripSeparator();
        	this.miDelete = new System.Windows.Forms.ToolStripMenuItem();
        	this.miSeparator3 = new System.Windows.Forms.ToolStripSeparator();
        	this.miSelectAll = new System.Windows.Forms.ToolStripMenuItem();
        	this.OFD = new System.Windows.Forms.OpenFileDialog();
        	this.MainSplit = new System.Windows.Forms.SplitContainer();
        	this.TaskList = new KK.ListViewEX();
        	this.colURL = new System.Windows.Forms.ColumnHeader();
        	this.colName = new System.Windows.Forms.ColumnHeader();
        	this.colSize = new System.Windows.Forms.ColumnHeader();
        	this.colProgress = new System.Windows.Forms.ColumnHeader();
        	this.colSpeed = new System.Windows.Forms.ColumnHeader();
        	this.colETA = new System.Windows.Forms.ColumnHeader();
        	this.Memo = new System.Windows.Forms.RichTextBox();
        	this.miEditLog = new System.Windows.Forms.ToolStripMenuItem();
        	this.Toolbar.SuspendLayout();
        	this.cmTaskList.SuspendLayout();
        	((System.ComponentModel.ISupportInitialize)(this.MainSplit)).BeginInit();
        	this.MainSplit.Panel1.SuspendLayout();
        	this.MainSplit.Panel2.SuspendLayout();
        	this.MainSplit.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// ImageList
        	// 
        	this.ImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList.ImageStream")));
        	this.ImageList.TransparentColor = System.Drawing.Color.Magenta;
        	this.ImageList.Images.SetKeyName(0, "");
        	this.ImageList.Images.SetKeyName(1, "");
        	// 
        	// btnSavePath
        	// 
        	this.btnSavePath.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        	resources.ApplyResources(this.btnSavePath, "btnSavePath");
        	this.btnSavePath.Name = "btnSavePath";
        	this.btnSavePath.Click += new System.EventHandler(this.btnSavePath_Click);
        	// 
        	// toolStripSeparator1
        	// 
        	this.toolStripSeparator1.Name = "toolStripSeparator1";
        	resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
        	// 
        	// URL
        	// 
        	resources.ApplyResources(this.URL, "URL");
        	this.URL.Name = "URL";
        	this.URL.KeyDown += new System.Windows.Forms.KeyEventHandler(this.URL_KeyDown);
        	this.URL.DoubleClick += new System.EventHandler(this.URL_DoubleClick);
        	this.URL.MouseEnter += new System.EventHandler(this.URL_MouseEnter);
        	// 
        	// toolStripSeparator2
        	// 
        	this.toolStripSeparator2.Name = "toolStripSeparator2";
        	resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
        	// 
        	// lbTask
        	// 
        	this.lbTask.Name = "lbTask";
        	resources.ApplyResources(this.lbTask, "lbTask");
        	// 
        	// MaxDownloaders
        	// 
        	this.MaxDownloaders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        	resources.ApplyResources(this.MaxDownloaders, "MaxDownloaders");
        	this.MaxDownloaders.Items.AddRange(new object[] {
			resources.GetString("MaxDownloaders.Items"),
			resources.GetString("MaxDownloaders.Items1"),
			resources.GetString("MaxDownloaders.Items2"),
			resources.GetString("MaxDownloaders.Items3"),
			resources.GetString("MaxDownloaders.Items4"),
			resources.GetString("MaxDownloaders.Items5"),
			resources.GetString("MaxDownloaders.Items6"),
			resources.GetString("MaxDownloaders.Items7"),
			resources.GetString("MaxDownloaders.Items8")});
        	this.MaxDownloaders.Name = "MaxDownloaders";
        	this.MaxDownloaders.DropDownClosed += new System.EventHandler(this.MaxDownloaders_DropDownClosed);
        	this.MaxDownloaders.SelectedIndexChanged += new System.EventHandler(this.MaxDownloaders_SelectedIndexChanged);
        	// 
        	// FBD
        	// 
        	resources.ApplyResources(this.FBD, "FBD");
        	// 
        	// Tray
        	// 
        	this.Tray.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Tray_MouseClick);
        	// 
        	// Toolbar
        	// 
        	resources.ApplyResources(this.Toolbar, "Toolbar");
        	this.Toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.btnSavePath,
			this.toolStripSeparator1,
			this.URL,
			this.toolStripSeparator2,
			this.lbTask,
			this.MaxDownloaders,
			this.lbSplit,
			this.MaxConnections,
			this.btnOption,
			this.btnReload});
        	this.Toolbar.Name = "Toolbar";
        	this.Toolbar.ShowItemToolTips = false;
        	this.Toolbar.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Toolbar_MouseDoubleClick);
        	// 
        	// lbSplit
        	// 
        	this.lbSplit.Name = "lbSplit";
        	resources.ApplyResources(this.lbSplit, "lbSplit");
        	// 
        	// MaxConnections
        	// 
        	this.MaxConnections.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        	resources.ApplyResources(this.MaxConnections, "MaxConnections");
        	this.MaxConnections.Items.AddRange(new object[] {
			resources.GetString("MaxConnections.Items"),
			resources.GetString("MaxConnections.Items1"),
			resources.GetString("MaxConnections.Items2"),
			resources.GetString("MaxConnections.Items3"),
			resources.GetString("MaxConnections.Items4"),
			resources.GetString("MaxConnections.Items5"),
			resources.GetString("MaxConnections.Items6"),
			resources.GetString("MaxConnections.Items7"),
			resources.GetString("MaxConnections.Items8")});
        	this.MaxConnections.Name = "MaxConnections";
        	this.MaxConnections.DropDownClosed += new System.EventHandler(this.MaxDownloaders_DropDownClosed);
        	this.MaxConnections.SelectedIndexChanged += new System.EventHandler(this.MaxConnections_SelectedIndexChanged);
        	// 
        	// btnOption
        	// 
        	this.btnOption.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        	this.btnOption.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.miShutdown,
			this.miTrace,
			this.miLog,
			this.miEditLog,
			this.miEditINI,
			this.toolStripSeparator3,
			this.miProxy,
			this.miConnection,
			this.toolStripMenuItem2,
			this.miClipboard,
			this.miParseOnly,
			this.miSeparator0,
			this.miStayTop,
			this.miLanguage});
        	resources.ApplyResources(this.btnOption, "btnOption");
        	this.btnOption.Name = "btnOption";
        	// 
        	// miShutdown
        	// 
        	this.miShutdown.CheckOnClick = true;
        	this.miShutdown.Name = "miShutdown";
        	resources.ApplyResources(this.miShutdown, "miShutdown");
        	// 
        	// miEditINI
        	// 
        	this.miEditINI.Name = "miEditINI";
        	resources.ApplyResources(this.miEditINI, "miEditINI");
        	this.miEditINI.Click += new System.EventHandler(this.miEditINI_Click);
        	// 
        	// miTrace
        	// 
        	this.miTrace.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.debug,
			this.info,
			this.notice,
			this.warn,
			this.error});
        	this.miTrace.Name = "miTrace";
        	resources.ApplyResources(this.miTrace, "miTrace");
        	this.miTrace.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.miTrace_DropDownItemClicked);
        	this.miTrace.Click += new System.EventHandler(this.miTrace_Click);
        	// 
        	// debug
        	// 
        	this.debug.Name = "debug";
        	resources.ApplyResources(this.debug, "debug");
        	// 
        	// info
        	// 
        	this.info.Name = "info";
        	resources.ApplyResources(this.info, "info");
        	// 
        	// notice
        	// 
        	this.notice.Name = "notice";
        	resources.ApplyResources(this.notice, "notice");
        	// 
        	// warn
        	// 
        	this.warn.Name = "warn";
        	resources.ApplyResources(this.warn, "warn");
        	// 
        	// error
        	// 
        	this.error.Name = "error";
        	resources.ApplyResources(this.error, "error");
        	// 
        	// miLog
        	// 
        	this.miLog.Name = "miLog";
        	resources.ApplyResources(this.miLog, "miLog");
        	this.miLog.Click += new System.EventHandler(this.miLog_Click);
        	// 
        	// toolStripSeparator3
        	// 
        	this.toolStripSeparator3.Name = "toolStripSeparator3";
        	resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
        	// 
        	// miProxy
        	// 
        	this.miProxy.Name = "miProxy";
        	resources.ApplyResources(this.miProxy, "miProxy");
        	this.miProxy.Click += new System.EventHandler(this.miProxy_Click);
        	// 
        	// miConnection
        	// 
        	this.miConnection.Name = "miConnection";
        	resources.ApplyResources(this.miConnection, "miConnection");
        	this.miConnection.Click += new System.EventHandler(this.miConnection_Click);
        	// 
        	// toolStripMenuItem2
        	// 
        	this.toolStripMenuItem2.Name = "toolStripMenuItem2";
        	resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
        	// 
        	// miClipboard
        	// 
        	this.miClipboard.Name = "miClipboard";
        	resources.ApplyResources(this.miClipboard, "miClipboard");
        	this.miClipboard.Click += new System.EventHandler(this.miClipboard_Click);
        	// 
        	// miParseOnly
        	// 
        	this.miParseOnly.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.miCopyURL,
			this.miRunDownloader});
        	this.miParseOnly.Name = "miParseOnly";
        	resources.ApplyResources(this.miParseOnly, "miParseOnly");
        	this.miParseOnly.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.miParseOnly_DropDownItemClicked);
        	this.miParseOnly.Click += new System.EventHandler(this.miParseOnly_Click);
        	// 
        	// miCopyURL
        	// 
        	this.miCopyURL.Name = "miCopyURL";
        	resources.ApplyResources(this.miCopyURL, "miCopyURL");
        	// 
        	// miRunDownloader
        	// 
        	this.miRunDownloader.Name = "miRunDownloader";
        	resources.ApplyResources(this.miRunDownloader, "miRunDownloader");
        	// 
        	// miSeparator0
        	// 
        	this.miSeparator0.Name = "miSeparator0";
        	resources.ApplyResources(this.miSeparator0, "miSeparator0");
        	// 
        	// miStayTop
        	// 
        	this.miStayTop.CheckOnClick = true;
        	this.miStayTop.Name = "miStayTop";
        	resources.ApplyResources(this.miStayTop, "miStayTop");
        	this.miStayTop.CheckedChanged += new System.EventHandler(this.miStayTop_CheckedChanged);
        	// 
        	// miLanguage
        	// 
        	this.miLanguage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.miLangAuto,
			this.toolStripMenuItem1,
			this.miLangChs,
			this.miLangEn});
        	this.miLanguage.Name = "miLanguage";
        	resources.ApplyResources(this.miLanguage, "miLanguage");
        	this.miLanguage.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.miLanguage_DropDownItemClicked);
        	// 
        	// miLangAuto
        	// 
        	this.miLangAuto.Name = "miLangAuto";
        	resources.ApplyResources(this.miLangAuto, "miLangAuto");
        	// 
        	// toolStripMenuItem1
        	// 
        	this.toolStripMenuItem1.Name = "toolStripMenuItem1";
        	resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
        	// 
        	// miLangChs
        	// 
        	this.miLangChs.Name = "miLangChs";
        	resources.ApplyResources(this.miLangChs, "miLangChs");
        	// 
        	// miLangEn
        	// 
        	this.miLangEn.Name = "miLangEn";
        	resources.ApplyResources(this.miLangEn, "miLangEn");
        	// 
        	// btnReload
        	// 
        	this.btnReload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
        	resources.ApplyResources(this.btnReload, "btnReload");
        	this.btnReload.Name = "btnReload";
        	this.btnReload.Click += new System.EventHandler(this.BtnReloadClick);
        	// 
        	// cmTaskList
        	// 
        	resources.ApplyResources(this.cmTaskList, "cmTaskList");
        	this.cmTaskList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.miOpen,
			this.miOpenDir,
			this.miSeparator1,
			this.miMoveTop,
			this.miMoveUp,
			this.miMoveDown,
			this.miMoveBottom,
			this.miSeparator2,
			this.miDelete,
			this.miSeparator3,
			this.miSelectAll});
        	this.cmTaskList.Name = "cmTaskList";
        	this.cmTaskList.Opening += new System.ComponentModel.CancelEventHandler(this.cmTaskList_Opening);
        	// 
        	// miOpen
        	// 
        	this.miOpen.Name = "miOpen";
        	resources.ApplyResources(this.miOpen, "miOpen");
        	this.miOpen.Click += new System.EventHandler(this.miOpen_Click);
        	// 
        	// miOpenDir
        	// 
        	this.miOpenDir.Name = "miOpenDir";
        	resources.ApplyResources(this.miOpenDir, "miOpenDir");
        	this.miOpenDir.Click += new System.EventHandler(this.miOpenDir_Click);
        	// 
        	// miSeparator1
        	// 
        	this.miSeparator1.Name = "miSeparator1";
        	resources.ApplyResources(this.miSeparator1, "miSeparator1");
        	// 
        	// miMoveTop
        	// 
        	this.miMoveTop.Name = "miMoveTop";
        	resources.ApplyResources(this.miMoveTop, "miMoveTop");
        	this.miMoveTop.Click += new System.EventHandler(this.miMoveTop_Click);
        	// 
        	// miMoveUp
        	// 
        	this.miMoveUp.Name = "miMoveUp";
        	resources.ApplyResources(this.miMoveUp, "miMoveUp");
        	this.miMoveUp.Click += new System.EventHandler(this.miMoveUp_Click);
        	// 
        	// miMoveDown
        	// 
        	this.miMoveDown.Name = "miMoveDown";
        	resources.ApplyResources(this.miMoveDown, "miMoveDown");
        	this.miMoveDown.Click += new System.EventHandler(this.miMoveDown_Click);
        	// 
        	// miMoveBottom
        	// 
        	this.miMoveBottom.Name = "miMoveBottom";
        	resources.ApplyResources(this.miMoveBottom, "miMoveBottom");
        	this.miMoveBottom.Click += new System.EventHandler(this.miMoveBottom_Click);
        	// 
        	// miSeparator2
        	// 
        	this.miSeparator2.Name = "miSeparator2";
        	resources.ApplyResources(this.miSeparator2, "miSeparator2");
        	// 
        	// miDelete
        	// 
        	this.miDelete.Name = "miDelete";
        	resources.ApplyResources(this.miDelete, "miDelete");
        	this.miDelete.Click += new System.EventHandler(this.miDelete_Click);
        	// 
        	// miSeparator3
        	// 
        	this.miSeparator3.Name = "miSeparator3";
        	resources.ApplyResources(this.miSeparator3, "miSeparator3");
        	// 
        	// miSelectAll
        	// 
        	this.miSelectAll.Name = "miSelectAll";
        	resources.ApplyResources(this.miSelectAll, "miSelectAll");
        	this.miSelectAll.Click += new System.EventHandler(this.miSelectAll_Click);
        	// 
        	// OFD
        	// 
        	resources.ApplyResources(this.OFD, "OFD");
        	this.OFD.SupportMultiDottedExtensions = true;
        	// 
        	// MainSplit
        	// 
        	resources.ApplyResources(this.MainSplit, "MainSplit");
        	this.MainSplit.Name = "MainSplit";
        	// 
        	// MainSplit.Panel1
        	// 
        	this.MainSplit.Panel1.Controls.Add(this.TaskList);
        	// 
        	// MainSplit.Panel2
        	// 
        	this.MainSplit.Panel2.Controls.Add(this.Memo);
        	this.MainSplit.TabStop = false;
        	this.MainSplit.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.MainSplit_SplitterMoved);
        	// 
        	// TaskList
        	// 
        	this.TaskList.AllowColumnReorder = true;
        	this.TaskList.AllowDrop = true;
        	this.TaskList.BorderStyle = System.Windows.Forms.BorderStyle.None;
        	this.TaskList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.colURL,
			this.colName,
			this.colSize,
			this.colProgress,
			this.colSpeed,
			this.colETA});
        	this.TaskList.ContextMenuStrip = this.cmTaskList;
        	resources.ApplyResources(this.TaskList, "TaskList");
        	this.TaskList.FullRowSelect = true;
        	this.TaskList.Name = "TaskList";
        	this.TaskList.OwnerDraw = true;
        	this.TaskList.ShowItemToolTips = true;
        	this.TaskList.SmallImageList = this.ImageList;
        	this.TaskList.UseCompatibleStateImageBehavior = false;
        	this.TaskList.View = System.Windows.Forms.View.Details;
        	this.TaskList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.TaskList_ColumnClick);
        	this.TaskList.DragDrop += new System.Windows.Forms.DragEventHandler(this.TaskList_DragDrop);
        	this.TaskList.DragEnter += new System.Windows.Forms.DragEventHandler(this.TaskList_DragEnter);
        	this.TaskList.DoubleClick += new System.EventHandler(this.TaskList_DoubleClick);
        	this.TaskList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TaskList_KeyDown);
        	this.TaskList.MouseEnter += new System.EventHandler(this.MaxDownloaders_DropDownClosed);
        	// 
        	// colURL
        	// 
        	resources.ApplyResources(this.colURL, "colURL");
        	// 
        	// colName
        	// 
        	resources.ApplyResources(this.colName, "colName");
        	// 
        	// colSize
        	// 
        	resources.ApplyResources(this.colSize, "colSize");
        	// 
        	// colProgress
        	// 
        	resources.ApplyResources(this.colProgress, "colProgress");
        	// 
        	// colSpeed
        	// 
        	resources.ApplyResources(this.colSpeed, "colSpeed");
        	// 
        	// colETA
        	// 
        	resources.ApplyResources(this.colETA, "colETA");
        	// 
        	// Memo
        	// 
        	this.Memo.BorderStyle = System.Windows.Forms.BorderStyle.None;
        	resources.ApplyResources(this.Memo, "Memo");
        	this.Memo.Name = "Memo";
        	this.Memo.ReadOnly = true;
        	this.Memo.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.Memo_LinkClicked);
        	this.Memo.DoubleClick += new System.EventHandler(this.Memo_DoubleClick);
        	// 
        	// miEditLog
        	// 
        	this.miEditLog.Name = "miEditLog";
        	resources.ApplyResources(this.miEditLog, "miEditLog");
        	this.miEditLog.Click += new System.EventHandler(this.MiEditLogClick);
        	// 
        	// MainForm
        	// 
        	resources.ApplyResources(this, "$this");
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.Controls.Add(this.MainSplit);
        	this.Controls.Add(this.Toolbar);
        	this.Name = "MainForm";
        	this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
        	this.Shown += new System.EventHandler(this.MainForm_Shown);
        	this.Toolbar.ResumeLayout(false);
        	this.Toolbar.PerformLayout();
        	this.cmTaskList.ResumeLayout(false);
        	this.MainSplit.Panel1.ResumeLayout(false);
        	this.MainSplit.Panel2.ResumeLayout(false);
        	((System.ComponentModel.ISupportInitialize)(this.MainSplit)).EndInit();
        	this.MainSplit.ResumeLayout(false);
        	this.ResumeLayout(false);
        	this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog FBD;
        private System.Windows.Forms.ToolStripButton btnSavePath;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripTextBox URL;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel lbTask;
        private System.Windows.Forms.ToolStripComboBox MaxDownloaders;
        private System.Windows.Forms.ImageList ImageList;
        private System.Windows.Forms.NotifyIcon Tray;
        private System.Windows.Forms.ToolStrip Toolbar;
        private System.Windows.Forms.ToolStripDropDownButton btnOption;
        private System.Windows.Forms.ToolStripMenuItem miLanguage;
        private System.Windows.Forms.ToolStripMenuItem miLangAuto;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem miLangChs;
        private System.Windows.Forms.ToolStripMenuItem miLangEn;
        private System.Windows.Forms.ToolStripSeparator miSeparator0;
        private System.Windows.Forms.ToolStripMenuItem miConnection;
        private System.Windows.Forms.ContextMenuStrip cmTaskList;
        private System.Windows.Forms.ToolStripMenuItem miMoveTop;
        private System.Windows.Forms.ToolStripMenuItem miMoveUp;
        private System.Windows.Forms.ToolStripMenuItem miMoveDown;
        private System.Windows.Forms.ToolStripMenuItem miMoveBottom;
        private System.Windows.Forms.ToolStripSeparator miSeparator1;
        private System.Windows.Forms.ToolStripMenuItem miDelete;
        private System.Windows.Forms.ToolStripMenuItem miSelectAll;
        private System.Windows.Forms.ToolStripSeparator miSeparator2;
        private System.Windows.Forms.ToolStripSeparator miSeparator3;
        private System.Windows.Forms.ToolStripMenuItem miOpen;
        private System.Windows.Forms.ToolStripMenuItem miClipboard;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem miParseOnly;
        private System.Windows.Forms.ToolStripMenuItem miCopyURL;
        private System.Windows.Forms.ToolStripMenuItem miRunDownloader;
        private System.Windows.Forms.OpenFileDialog OFD;
        private System.Windows.Forms.SplitContainer MainSplit;
        private KK.ListViewEX TaskList;
        private System.Windows.Forms.ColumnHeader colURL;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colSize;
        private System.Windows.Forms.ColumnHeader colProgress;
        private System.Windows.Forms.ColumnHeader colSpeed;
        private System.Windows.Forms.ColumnHeader colETA;
        private System.Windows.Forms.RichTextBox Memo;
        private System.Windows.Forms.ToolStripMenuItem miEditINI;
        private System.Windows.Forms.ToolStripMenuItem miOpenDir;
        private System.Windows.Forms.ToolStripMenuItem miProxy;
        private System.Windows.Forms.ToolStripMenuItem miLog;
        private System.Windows.Forms.ToolStripMenuItem miTrace;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel lbSplit;
        private System.Windows.Forms.ToolStripComboBox MaxConnections;
        private System.Windows.Forms.ToolStripMenuItem debug;
        private System.Windows.Forms.ToolStripMenuItem info;
        private System.Windows.Forms.ToolStripMenuItem notice;
        private System.Windows.Forms.ToolStripMenuItem warn;
        private System.Windows.Forms.ToolStripMenuItem error;
        private System.Windows.Forms.ToolStripMenuItem miStayTop;
        private System.Windows.Forms.ToolStripMenuItem miShutdown;
        private System.Windows.Forms.ToolStripButton btnReload;
        private System.Windows.Forms.ToolStripMenuItem miEditLog;
    }
}

