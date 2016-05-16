namespace ParseGet
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        System.ComponentModel.IContainer components;

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
        void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            ImageList = new System.Windows.Forms.ImageList(components);
            btnSavePath = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            URL = new System.Windows.Forms.ToolStripTextBox();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            lbTask = new System.Windows.Forms.ToolStripLabel();
            MaxDownloaders = new System.Windows.Forms.ToolStripComboBox();
            FBD = new System.Windows.Forms.FolderBrowserDialog();
            Tray = new System.Windows.Forms.NotifyIcon(components);
            Toolbar = new System.Windows.Forms.ToolStrip();
            lbSplit = new System.Windows.Forms.ToolStripLabel();
            MaxConnections = new System.Windows.Forms.ToolStripComboBox();
            btnOption = new System.Windows.Forms.ToolStripDropDownButton();
            miEditINI = new System.Windows.Forms.ToolStripMenuItem();
            miTrace = new System.Windows.Forms.ToolStripMenuItem();
            debug = new System.Windows.Forms.ToolStripMenuItem();
            info = new System.Windows.Forms.ToolStripMenuItem();
            notice = new System.Windows.Forms.ToolStripMenuItem();
            warn = new System.Windows.Forms.ToolStripMenuItem();
            error = new System.Windows.Forms.ToolStripMenuItem();
            miLog = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            miProxy = new System.Windows.Forms.ToolStripMenuItem();
            miConnection = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            miClipboard = new System.Windows.Forms.ToolStripMenuItem();
            miParseOnly = new System.Windows.Forms.ToolStripMenuItem();
            miCopyURL = new System.Windows.Forms.ToolStripMenuItem();
            miRunDownloader = new System.Windows.Forms.ToolStripMenuItem();
            miSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            miStayTop = new System.Windows.Forms.ToolStripMenuItem();
            miLanguage = new System.Windows.Forms.ToolStripMenuItem();
            miLangAuto = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            miLangChs = new System.Windows.Forms.ToolStripMenuItem();
            miLangEn = new System.Windows.Forms.ToolStripMenuItem();
            cmTaskList = new System.Windows.Forms.ContextMenuStrip(components);
            miOpen = new System.Windows.Forms.ToolStripMenuItem();
            miOpenDir = new System.Windows.Forms.ToolStripMenuItem();
            miSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            miMoveTop = new System.Windows.Forms.ToolStripMenuItem();
            miMoveUp = new System.Windows.Forms.ToolStripMenuItem();
            miMoveDown = new System.Windows.Forms.ToolStripMenuItem();
            miMoveBottom = new System.Windows.Forms.ToolStripMenuItem();
            miSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            miDelete = new System.Windows.Forms.ToolStripMenuItem();
            miSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            miSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            OFD = new System.Windows.Forms.OpenFileDialog();
            MainSplit = new System.Windows.Forms.SplitContainer();
            TaskList = new KK.ListViewEX();
            colURL = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            colSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            colProgress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            colSpeed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            colETA = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            Memo = new System.Windows.Forms.RichTextBox();
            miShutdown = new System.Windows.Forms.ToolStripMenuItem();
            Toolbar.SuspendLayout();
            cmTaskList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(MainSplit)).BeginInit();
            MainSplit.Panel1.SuspendLayout();
            MainSplit.Panel2.SuspendLayout();
            MainSplit.SuspendLayout();
            SuspendLayout();
            // 
            // ImageList
            // 
            ImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList.ImageStream")));
            ImageList.TransparentColor = System.Drawing.Color.Magenta;
            ImageList.Images.SetKeyName(0, "");
            ImageList.Images.SetKeyName(1, "");
            // 
            // btnSavePath
            // 
            btnSavePath.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(btnSavePath, "btnSavePath");
            btnSavePath.Name = "btnSavePath";
			btnSavePath.Click += btnSavePath_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(toolStripSeparator1, "toolStripSeparator1");
            // 
            // URL
            // 
            resources.ApplyResources(URL, "URL");
            URL.Name = "URL";
			URL.KeyDown += URL_KeyDown;
			URL.DoubleClick += URL_DoubleClick;
			URL.MouseEnter += URL_MouseEnter;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(toolStripSeparator2, "toolStripSeparator2");
            // 
            // lbTask
            // 
            lbTask.Name = "lbTask";
            resources.ApplyResources(lbTask, "lbTask");
            // 
            // MaxDownloaders
            // 
            MaxDownloaders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(MaxDownloaders, "MaxDownloaders");
            MaxDownloaders.Items.AddRange(new object[] {
            resources.GetString("MaxDownloaders.Items"),
            resources.GetString("MaxDownloaders.Items1"),
            resources.GetString("MaxDownloaders.Items2"),
            resources.GetString("MaxDownloaders.Items3"),
            resources.GetString("MaxDownloaders.Items4"),
            resources.GetString("MaxDownloaders.Items5"),
            resources.GetString("MaxDownloaders.Items6"),
            resources.GetString("MaxDownloaders.Items7"),
            resources.GetString("MaxDownloaders.Items8")});
            MaxDownloaders.Name = "MaxDownloaders";
			MaxDownloaders.DropDownClosed += MaxDownloaders_DropDownClosed;
			MaxDownloaders.SelectedIndexChanged += MaxDownloaders_SelectedIndexChanged;
            // 
            // FBD
            // 
            resources.ApplyResources(FBD, "FBD");
            // 
            // Tray
            // 
			Tray.MouseClick += Tray_MouseClick;
            // 
            // Toolbar
            // 
            resources.ApplyResources(Toolbar, "Toolbar");
            Toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            btnSavePath,
            toolStripSeparator1,
            URL,
            toolStripSeparator2,
            lbTask,
            MaxDownloaders,
            lbSplit,
            MaxConnections,
            btnOption});
            Toolbar.Name = "Toolbar";
            Toolbar.ShowItemToolTips = false;
			Toolbar.MouseDoubleClick += Toolbar_MouseDoubleClick;
            // 
            // lbSplit
            // 
            lbSplit.Name = "lbSplit";
            resources.ApplyResources(lbSplit, "lbSplit");
            // 
            // MaxConnections
            // 
            MaxConnections.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(MaxConnections, "MaxConnections");
            MaxConnections.Items.AddRange(new object[] {
            resources.GetString("MaxConnections.Items"),
            resources.GetString("MaxConnections.Items1"),
            resources.GetString("MaxConnections.Items2"),
            resources.GetString("MaxConnections.Items3"),
            resources.GetString("MaxConnections.Items4"),
            resources.GetString("MaxConnections.Items5"),
            resources.GetString("MaxConnections.Items6"),
            resources.GetString("MaxConnections.Items7"),
            resources.GetString("MaxConnections.Items8")});
            MaxConnections.Name = "MaxConnections";
			MaxConnections.DropDownClosed += MaxDownloaders_DropDownClosed;
			MaxConnections.SelectedIndexChanged += MaxConnections_SelectedIndexChanged;
            // 
            // btnOption
            // 
            btnOption.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            btnOption.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            miShutdown,
            miEditINI,
            miTrace,
            miLog,
            toolStripSeparator3,
            miProxy,
            miConnection,
            toolStripMenuItem2,
            miClipboard,
            miParseOnly,
            miSeparator0,
            miStayTop,
            miLanguage});
            resources.ApplyResources(btnOption, "btnOption");
            btnOption.Name = "btnOption";
            // 
            // miEditINI
            // 
            miEditINI.Name = "miEditINI";
            resources.ApplyResources(miEditINI, "miEditINI");
			miEditINI.Click += miEditINI_Click;
            // 
            // miTrace
            // 
            miTrace.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            debug,
            info,
            notice,
            warn,
            error});
            miTrace.Name = "miTrace";
            resources.ApplyResources(miTrace, "miTrace");
			miTrace.DropDownItemClicked += miTrace_DropDownItemClicked;
			miTrace.Click += miTrace_Click;
            // 
            // debug
            // 
            debug.Name = "debug";
            resources.ApplyResources(debug, "debug");
            // 
            // info
            // 
            info.Name = "info";
            resources.ApplyResources(info, "info");
            // 
            // notice
            // 
            notice.Name = "notice";
            resources.ApplyResources(notice, "notice");
            // 
            // warn
            // 
            warn.Name = "warn";
            resources.ApplyResources(warn, "warn");
            // 
            // error
            // 
            error.Name = "error";
            resources.ApplyResources(error, "error");
            // 
            // miLog
            // 
            miLog.Name = "miLog";
            resources.ApplyResources(miLog, "miLog");
			miLog.Click += miLog_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(toolStripSeparator3, "toolStripSeparator3");
            // 
            // miProxy
            // 
            miProxy.Name = "miProxy";
            resources.ApplyResources(miProxy, "miProxy");
			miProxy.Click += miProxy_Click;
            // 
            // miConnection
            // 
            miConnection.Name = "miConnection";
            resources.ApplyResources(miConnection, "miConnection");
			miConnection.Click += miConnection_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            resources.ApplyResources(toolStripMenuItem2, "toolStripMenuItem2");
            // 
            // miClipboard
            // 
            miClipboard.Name = "miClipboard";
            resources.ApplyResources(miClipboard, "miClipboard");
			miClipboard.Click += miClipboard_Click;
            // 
            // miParseOnly
            // 
            miParseOnly.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            miCopyURL,
            miRunDownloader});
            miParseOnly.Name = "miParseOnly";
            resources.ApplyResources(miParseOnly, "miParseOnly");
			miParseOnly.DropDownItemClicked += miParseOnly_DropDownItemClicked;
			miParseOnly.Click += miParseOnly_Click;
            // 
            // miCopyURL
            // 
            miCopyURL.Name = "miCopyURL";
            resources.ApplyResources(miCopyURL, "miCopyURL");
            // 
            // miRunDownloader
            // 
            miRunDownloader.Name = "miRunDownloader";
            resources.ApplyResources(miRunDownloader, "miRunDownloader");
            // 
            // miSeparator0
            // 
            miSeparator0.Name = "miSeparator0";
            resources.ApplyResources(miSeparator0, "miSeparator0");
            // 
            // miStayTop
            // 
            miStayTop.CheckOnClick = true;
            miStayTop.Name = "miStayTop";
            resources.ApplyResources(miStayTop, "miStayTop");
			miStayTop.CheckedChanged += miStayTop_CheckedChanged;
            // 
            // miLanguage
            // 
            miLanguage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            miLangAuto,
            toolStripMenuItem1,
            miLangChs,
            miLangEn});
            miLanguage.Name = "miLanguage";
            resources.ApplyResources(miLanguage, "miLanguage");
			miLanguage.DropDownItemClicked += miLanguage_DropDownItemClicked;
            // 
            // miLangAuto
            // 
            miLangAuto.Name = "miLangAuto";
            resources.ApplyResources(miLangAuto, "miLangAuto");
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // miLangChs
            // 
            miLangChs.Name = "miLangChs";
            resources.ApplyResources(miLangChs, "miLangChs");
            // 
            // miLangEn
            // 
            miLangEn.Name = "miLangEn";
            resources.ApplyResources(miLangEn, "miLangEn");
            // 
            // cmTaskList
            // 
            resources.ApplyResources(cmTaskList, "cmTaskList");
            cmTaskList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            miOpen,
            miOpenDir,
            miSeparator1,
            miMoveTop,
            miMoveUp,
            miMoveDown,
            miMoveBottom,
            miSeparator2,
            miDelete,
            miSeparator3,
            miSelectAll});
            cmTaskList.Name = "cmTaskList";
			cmTaskList.Opening += cmTaskList_Opening;
            // 
            // miOpen
            // 
            miOpen.Name = "miOpen";
            resources.ApplyResources(miOpen, "miOpen");
			miOpen.Click += miOpen_Click;
            // 
            // miOpenDir
            // 
            miOpenDir.Name = "miOpenDir";
            resources.ApplyResources(miOpenDir, "miOpenDir");
			miOpenDir.Click += miOpenDir_Click;
            // 
            // miSeparator1
            // 
            miSeparator1.Name = "miSeparator1";
            resources.ApplyResources(miSeparator1, "miSeparator1");
            // 
            // miMoveTop
            // 
            miMoveTop.Name = "miMoveTop";
            resources.ApplyResources(miMoveTop, "miMoveTop");
			miMoveTop.Click += miMoveTop_Click;
            // 
            // miMoveUp
            // 
            miMoveUp.Name = "miMoveUp";
            resources.ApplyResources(miMoveUp, "miMoveUp");
			miMoveUp.Click += miMoveUp_Click;
            // 
            // miMoveDown
            // 
            miMoveDown.Name = "miMoveDown";
            resources.ApplyResources(miMoveDown, "miMoveDown");
			miMoveDown.Click += miMoveDown_Click;
            // 
            // miMoveBottom
            // 
            miMoveBottom.Name = "miMoveBottom";
            resources.ApplyResources(miMoveBottom, "miMoveBottom");
			miMoveBottom.Click += miMoveBottom_Click;
            // 
            // miSeparator2
            // 
            miSeparator2.Name = "miSeparator2";
            resources.ApplyResources(miSeparator2, "miSeparator2");
            // 
            // miDelete
            // 
            miDelete.Name = "miDelete";
            resources.ApplyResources(miDelete, "miDelete");
			miDelete.Click += miDelete_Click;
            // 
            // miSeparator3
            // 
            miSeparator3.Name = "miSeparator3";
            resources.ApplyResources(miSeparator3, "miSeparator3");
            // 
            // miSelectAll
            // 
            miSelectAll.Name = "miSelectAll";
            resources.ApplyResources(miSelectAll, "miSelectAll");
			miSelectAll.Click += miSelectAll_Click;
            // 
            // OFD
            // 
            resources.ApplyResources(OFD, "OFD");
            OFD.SupportMultiDottedExtensions = true;
            // 
            // MainSplit
            // 
            resources.ApplyResources(MainSplit, "MainSplit");
            MainSplit.Name = "MainSplit";
            // 
            // MainSplit.Panel1
            // 
            MainSplit.Panel1.Controls.Add(TaskList);
            // 
            // MainSplit.Panel2
            // 
            MainSplit.Panel2.Controls.Add(Memo);
            MainSplit.TabStop = false;
			MainSplit.SplitterMoved += MainSplit_SplitterMoved;
            // 
            // TaskList
            // 
            TaskList.AllowColumnReorder = true;
            TaskList.AllowDrop = true;
            TaskList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            TaskList.Columns.AddRange(new [] {
            colURL,
            colName,
            colSize,
            colProgress,
            colSpeed,
            colETA});
            TaskList.ContextMenuStrip = cmTaskList;
            resources.ApplyResources(TaskList, "TaskList");
            TaskList.FullRowSelect = true;
            TaskList.Name = "TaskList";
            TaskList.OwnerDraw = true;
            TaskList.ShowItemToolTips = true;
            TaskList.SmallImageList = ImageList;
            TaskList.UseCompatibleStateImageBehavior = false;
            TaskList.View = System.Windows.Forms.View.Details;
			TaskList.ColumnClick += TaskList_ColumnClick;
			TaskList.DragDrop += TaskList_DragDrop;
			TaskList.DragEnter += TaskList_DragEnter;
			TaskList.DoubleClick += TaskList_DoubleClick;
			TaskList.KeyDown += TaskList_KeyDown;
			TaskList.MouseEnter += MaxDownloaders_DropDownClosed;
            // 
            // colURL
            // 
            resources.ApplyResources(colURL, "colURL");
            // 
            // colName
            // 
            resources.ApplyResources(colName, "colName");
            // 
            // colSize
            // 
            resources.ApplyResources(colSize, "colSize");
            // 
            // colProgress
            // 
            resources.ApplyResources(colProgress, "colProgress");
            // 
            // colSpeed
            // 
            resources.ApplyResources(colSpeed, "colSpeed");
            // 
            // colETA
            // 
            resources.ApplyResources(colETA, "colETA");
            // 
            // Memo
            // 
            Memo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(Memo, "Memo");
            Memo.Name = "Memo";
            Memo.ReadOnly = true;
			Memo.LinkClicked += Memo_LinkClicked;
			Memo.DoubleClick += Memo_DoubleClick;
            // 
            // miShutdown
            // 
            miShutdown.CheckOnClick = true;
            miShutdown.Name = "miShutdown";
            resources.ApplyResources(miShutdown, "miShutdown");
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(MainSplit);
            Controls.Add(Toolbar);
            Name = "MainForm";
			FormClosing += MainForm_FormClosing;
			Shown += MainForm_Shown;
            Toolbar.ResumeLayout(false);
            Toolbar.PerformLayout();
            cmTaskList.ResumeLayout(false);
            MainSplit.Panel1.ResumeLayout(false);
            MainSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(MainSplit)).EndInit();
            MainSplit.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        System.Windows.Forms.FolderBrowserDialog FBD;
        System.Windows.Forms.ToolStripButton btnSavePath;
        System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        System.Windows.Forms.ToolStripTextBox URL;
        System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        System.Windows.Forms.ToolStripLabel lbTask;
        System.Windows.Forms.ToolStripComboBox MaxDownloaders;
        System.Windows.Forms.ImageList ImageList;
        System.Windows.Forms.NotifyIcon Tray;
        System.Windows.Forms.ToolStrip Toolbar;
        System.Windows.Forms.ToolStripDropDownButton btnOption;
        System.Windows.Forms.ToolStripMenuItem miLanguage;
        System.Windows.Forms.ToolStripMenuItem miLangAuto;
        System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        System.Windows.Forms.ToolStripMenuItem miLangChs;
        System.Windows.Forms.ToolStripMenuItem miLangEn;
        System.Windows.Forms.ToolStripSeparator miSeparator0;
        System.Windows.Forms.ToolStripMenuItem miConnection;
        System.Windows.Forms.ContextMenuStrip cmTaskList;
        System.Windows.Forms.ToolStripMenuItem miMoveTop;
        System.Windows.Forms.ToolStripMenuItem miMoveUp;
        System.Windows.Forms.ToolStripMenuItem miMoveDown;
        System.Windows.Forms.ToolStripMenuItem miMoveBottom;
        System.Windows.Forms.ToolStripSeparator miSeparator1;
        System.Windows.Forms.ToolStripMenuItem miDelete;
        System.Windows.Forms.ToolStripMenuItem miSelectAll;
        System.Windows.Forms.ToolStripSeparator miSeparator2;
        System.Windows.Forms.ToolStripSeparator miSeparator3;
        System.Windows.Forms.ToolStripMenuItem miOpen;
        System.Windows.Forms.ToolStripMenuItem miClipboard;
        System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        System.Windows.Forms.ToolStripMenuItem miParseOnly;
        System.Windows.Forms.ToolStripMenuItem miCopyURL;
        System.Windows.Forms.ToolStripMenuItem miRunDownloader;
        System.Windows.Forms.OpenFileDialog OFD;
        System.Windows.Forms.SplitContainer MainSplit;
        KK.ListViewEX TaskList;
        System.Windows.Forms.ColumnHeader colURL;
        System.Windows.Forms.ColumnHeader colName;
        System.Windows.Forms.ColumnHeader colSize;
        System.Windows.Forms.ColumnHeader colProgress;
        System.Windows.Forms.ColumnHeader colSpeed;
        System.Windows.Forms.ColumnHeader colETA;
        System.Windows.Forms.RichTextBox Memo;
        System.Windows.Forms.ToolStripMenuItem miEditINI;
        System.Windows.Forms.ToolStripMenuItem miOpenDir;
        System.Windows.Forms.ToolStripMenuItem miProxy;
        System.Windows.Forms.ToolStripMenuItem miLog;
        System.Windows.Forms.ToolStripMenuItem miTrace;
        System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        System.Windows.Forms.ToolStripLabel lbSplit;
        System.Windows.Forms.ToolStripComboBox MaxConnections;
        System.Windows.Forms.ToolStripMenuItem debug;
        System.Windows.Forms.ToolStripMenuItem info;
        System.Windows.Forms.ToolStripMenuItem notice;
        System.Windows.Forms.ToolStripMenuItem warn;
        System.Windows.Forms.ToolStripMenuItem error;
        System.Windows.Forms.ToolStripMenuItem miStayTop;
        System.Windows.Forms.ToolStripMenuItem miShutdown;
    }
}

