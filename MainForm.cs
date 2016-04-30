using KK;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

[assembly: CLSCompliant(true)]

namespace ParseGet
{
    using Properties;

    public partial class MainForm : Form
    {
        private const int ITEM_URL = 0;
        private const int ITEM_NAME = 1;
        private const int ITEM_SIZE = 2;
        private const int ITEM_DONE = 3;
        private const int ITEM_SPEED = 4;
        private const int ITEM_ETA = 5;

        //private FormWindowState SaveState;
        private IntPtr Aria2Wnd;
        private ClipboardViewer Viewer = new ClipboardViewer();

        public MainForm()
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = Application.StartupPath + "\\aria2c.exe";
            psi.Arguments = "-k2M -x9 -j9 --enable-rpc=true --auto-file-renaming=false --max-download-result=9" +
                " -l- --log-level=" + AppConfig.Settings.LogLevel +
                " --stop-with-process=" + Process.GetCurrentProcess().Id;
            psi.WindowStyle = ProcessWindowStyle.Minimized;

            Process p = Process.Start(psi);
            while (!Util.NativeMethods.IsWindowVisible(p.MainWindowHandle))
            {
                Thread.Sleep(5);
            }
            Aria2Wnd = p.MainWindowHandle;
            ShowAria2(AppConfig.Settings.Trace);

            InitializeComponent();
            Tray.Icon = Icon;
            Tray.Text = Text;

            // Load tasks ...
            TaskList.BeginUpdate();
            foreach (string s in AppConfig.Settings.Tasks)
            {
                NewTask(s);
            }
            TaskList.EndUpdate();
            AppConfig.Settings.Tasks.Clear();

            // Load settings ...
            if (Directory.Exists(AppConfig.Settings.SavePath))
            {
                FBD.SelectedPath = AppConfig.Settings.SavePath;
            }
            else
            {
                FBD.SelectedPath = AppConfig.Settings.SavePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            Web.ProxyAddr = AppConfig.Settings.Proxy;
            OFD.FileName = AppConfig.Settings.ExternalDownloader;
            (miLanguage.DropDownItems[AppConfig.Settings.Language] as ToolStripMenuItem).Checked = true;
            (miParseOnly.DropDownItems[AppConfig.Settings.ExternalMethod] as ToolStripMenuItem).Checked = true;
            (miTrace.DropDownItems[AppConfig.Settings.LogLevel] as ToolStripMenuItem).Checked = true;
            miParseOnly.Checked = AppConfig.Settings.ParseOnly;
            Parser.Trace = miTrace.Checked = AppConfig.Settings.Trace;
            Web.UseProxy = miProxy.Checked = AppConfig.Settings.UseProxy;
            MainSplit.Panel2Collapsed = !(miLog.Checked = AppConfig.Settings.Log) && !Parser.Trace;
            foreach (ToolStripMenuItem mi in miParseOnly.DropDownItems) mi.Visible = miParseOnly.Checked;
            foreach (ToolStripMenuItem mi in miTrace.DropDownItems) mi.Visible = miTrace.Checked;

            Viewer.ClipboardChanged += new EventHandler(Viewer_ClipboardChanged);
            Viewer.Enabled = miClipboard.Checked = AppConfig.Settings.Clipboard;
        }

        private void MaxConnections_SelectedIndexChanged(object sender, EventArgs e)
        {
            AppConfig.Settings.MaxConnections = Convert.ToInt32(MaxConnections.Text);
            Aria2.SetOptions("split:" + MaxConnections.Text, true);
        }

        private void ShowAria2(bool show)
        {
            if (show)
            {
                Util.NativeMethods.ShowWindow(Aria2Wnd, Util.NativeMethods.SW_SHOWNOACTIVATE);
                BringToFront();
            }
            else
            {
                Util.NativeMethods.ShowWindow(Aria2Wnd, Util.NativeMethods.SW_HIDE);
            }
        }

        private void URL_DoubleClick(object sender, EventArgs e)
        {
            URL.Text = Clipboard.GetText();
            StartDownload(URL.Text);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            Credential.OwnerHWnd = Handle;

            MaxDownloaders.Text = AppConfig.Settings.MaxDownloaders.ToString();
            MaxConnections.Text = AppConfig.Settings.MaxConnections.ToString();
        }

        private void RunTask(ListViewItem item)
        {
            if (Downloader.Count < AppConfig.Settings.MaxDownloaders)
            {
                Downloader downloader = new Downloader();
                downloader.ProgressChanged += new ProgressChangedEventHandler(downloader_ProgressChanged);
                downloader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(downloader_RunWorkerCompleted);
                downloader.Tag = item;
                downloader.FileName = item.SubItems[ITEM_NAME].Text;
                downloader.SavePath = FBD.SelectedPath;
                downloader.RunWorkerAsync(item.Name);
                Taskbar.SetProgressState(TaskbarState.Normal);

                item.ImageIndex = 1;
                item.Tag = downloader;

                Util.PreventSleep();
            }
        }

        private void RunTasks()
        {
            foreach (ListViewItem item in TaskList.Items)
            {
                if (Downloader.Count >= AppConfig.Settings.MaxDownloaders)
                {
                    break;
                }
                if (item.Tag == null)
                {
                    RunTask(item);
                }
            }
        }

        private void Viewer_ClipboardChanged(Object sender, EventArgs e)
        {
            if (Clipboard.ContainsText() && (Visible || Tray.Visible))
            {
                string s = Clipboard.GetText();
                int i = s.IndexOf("\r\n");
                if (i > 0)
                {
                    s = s.Remove(i);
                }
                if (Parser.IsValidURL(s) == 1)
                {
                    URL.Text = s;
                    StartDownload(s);
                }
            }
        }

        private void URL_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                StartDownload(URL.Text);
            }
        }

        private void Log(string s, object o = null)
        {
            if (s.StartsWith(Resources.Error))
            {
                Memo.SelectionColor = Color.Red;
            }
            if (o != null)
            {
                s = ((o as Downloader).Tag as ListViewItem).Name + ' ' + s + "\r\n";
            }
            Memo.SelectionStart = Memo.TextLength;
            Memo.AppendText(s);
            Memo.SelectionLength = s.Length;
            Memo.ScrollToCaret();
        }

        private ListViewItem NewTask(string url)
        {
            ListViewItem item = new ListViewItem(url, 0);
            item.Name = url;
            item.SubItems.Add(string.Empty);    // FileName
            item.SubItems.Add(string.Empty);    // Size
            item.SubItems.Add(string.Empty);    // Done
            item.SubItems.Add(string.Empty);    // Speed
            item.SubItems.Add(string.Empty);    // ETA
            TaskList.Items.Add(item);

            return item;
        }

        private void StartNewTask(string url)
        {
            if (!TaskList.Items.ContainsKey(url))
            {
                //Clipboard.Clear();
                if (Visible) Util.FlashWindow(Handle, 3);
                //else Tray.ShowBalloonTip(3000, Resources.NewTask, url, ToolTipIcon.Info);
                RunTask(NewTask(url));
            }
            else
            {
                //Log(Resources.ErrorSameURL);
            }
        }

        private void StartDownload(string s)
        {
            // Check ...
            if (!string.IsNullOrEmpty(s))
            {
                if (s.StartsWith("http") || Parser.IsValidURL(s) > 0)
                {
                    Match m = Regex.Match(s, "{\\d+-\\d+}");
                    if (m.Success)
                    {
                        int i, end;
                        Int32.TryParse(Util.GetSubStr(m.Value, "{", "-"), out i);
                        Int32.TryParse(Util.GetSubStr(m.Value, "-", "}"), out end);
                        s = s.Replace(m.Value, "{0}");
                        while (i <= end)
                        {
                            StartNewTask(string.Format(s, i.ToString()));
                            i++;
                        }
                        return;
                    }
                    StartNewTask(s);
                }
                else
                {
                    // Keyword for search
                    Process.Start("https://vimeo.com/search?q=" + s);
                }
            }
        }

        private void downloader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!Program.IsClosing)
            {
                Downloader downloader = sender as Downloader;

                if (e.Error != null)
                {
                    Log(Resources.Error + e.Error.Message, sender);
                }
                else if (e.Cancelled)
                {
                    Log(Resources.DownloadCancelled, sender);
                }
                else if (e.Result is String)
                {
                    string s = e.Result as string;
                    if (s.StartsWith("http"))
                    {
                        Log("=> " + s, sender);
                        if (AppConfig.Settings.ExternalMethod == 0)
                        {
                            Clipboard.SetText(s);   // Copy URL
                        }
                        else
                        {
                            try
                            {
                                Process.Start(AppConfig.Settings.ExternalDownloader, s);  // Run Program ...
                            }
                            catch (Exception ex)
                            {
                                Log(Resources.Error + ex.Message);
                            }
                        }
                    }
                }
                else
                {
                    Log(Resources.DownloadCompleted, sender);
                    if (!Visible)
                    {
                        Tray.ShowBalloonTip(3000, Resources.DownloadCompleted,
                            (downloader.Tag as ListViewItem).SubItems[ITEM_NAME].Text, ToolTipIcon.Info);
                    }
                    //Util.FlashWindow(Handle, 3);
                }

                TaskList.Items.Remove(downloader.Tag as ListViewItem);
                downloader.Dispose();

                if (TaskList.Items.Count > 0)
                {
                    RunTasks();
                }
                else // all tasks completed
                {
                    if (Visible) Util.FlashWindow(Handle, 0);
                    else Tray.ShowBalloonTip(5000, Resources.DownloadCompleted, Text, ToolTipIcon.Info);
                    Taskbar.SetProgressState(TaskbarState.NoProgress);
                    if (miShutdown.Checked && !e.Cancelled)
                    {
                        Process.Start("shutdown.exe", "-r -f");
                    }
                    Util.ResotreSleep();
                }
            }
        }

        //private int count = 0;
        private void downloader_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (!Program.IsClosing)
            {
                Downloader downloader = sender as Downloader;
                ListViewItem item = downloader.Tag as ListViewItem;

                switch (e.ProgressPercentage)
                {
                    case Downloader.STATUS:
                        //TaskList_ColumnClick(null, null);
                        Log(e.UserState as string, sender);
                        break;

                    case Downloader.FILEINFO:
                        item.SubItems[ITEM_NAME].Text = downloader.FileName;
                        if (downloader.FileSize > 0)
                        {
                            item.SubItems[ITEM_SIZE].Text = Util.ToSize(downloader.FileSize);
                        }
                        break;

                    case Downloader.CHANGEURL:
                        item.Name = e.UserState as string;
                        break;

                    case Downloader.NEWURL:
                        StartDownload(e.UserState as string);
                        break;

                    default:
#if false
                        if (count == 0)
                        {
                            TaskList.BeginUpdate();
                        }
                        count++;
#endif
                        long progress = (long)e.UserState;
                        long filesize = downloader.FileSize;
                        int speed = downloader.Speed;

                        item.SubItems[ITEM_SIZE].Text = Util.ToSize(downloader.FileSize);
                        item.SubItems[ITEM_SPEED].Text = Util.ToSpeed(speed);
                        if (filesize > 0)
                        {
                            string s = (progress / (double)filesize).ToString("P1");
                            if (s == "100.0%" && progress < filesize)
                            {
                                s = "99.9%";
                            }
                            item.SubItems[ITEM_DONE].Text = s;

                            Taskbar.SetProgressValue(progress, filesize);
                            if (speed != 0)
                            {
                                item.SubItems[ITEM_ETA].Text = TimeSpan.FromSeconds((filesize - progress) / speed).ToString();
                            }
                            else
                            {
                                item.SubItems[ITEM_ETA].Text = string.Empty;
                            }
                        }
#if false
                        if (count >= PWeb.Count)
                        {
                            count = 0;
                            TaskList.EndUpdate();
                        }
#endif
                        break;
                }
            }
        }

        private void btnSavePath_Click(object sender, EventArgs e)
        {
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                AppConfig.Settings.SavePath = FBD.SelectedPath;
            }
        }

        private void MaxDownloaders_SelectedIndexChanged(object sender, EventArgs e)
        {
            AppConfig.Settings.MaxDownloaders = Convert.ToInt32(MaxDownloaders.Text);
            RunTasks();
        }

        private void TaskList_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem item = TaskList.SelectedItems[0];
            string s = TaskList.HitTest(TaskList.PointToClient(Control.MousePosition)).SubItem.Text;

            if (s.StartsWith("http") || string.IsNullOrEmpty(item.SubItems[ITEM_DONE].Text))
            {
                Process.Start(item.Name);
            }
            else
            {
                string filename = (item.Tag as Downloader).SavePath + "\\" + item.SubItems[ITEM_NAME].Text;
                try
                {
                    Process.Start(filename);
                }
                catch (Win32Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    Process.Start("rundll32.exe", "shell32.dll,OpenAs_RunDLL " + filename);
                }
            }
        }

        private void TaskList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && TaskList.SelectedItems.Count == 1)
            {
                (e.Control ? miOpenDir : miOpen).PerformClick();
            }
            else if (e.KeyCode == Keys.Delete)
            {
                miDelete.PerformClick();
            }
        }

        private void Memo_DoubleClick(object sender, EventArgs e)
        {
            Memo.Clear();
        }

        private void Memo_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            if ((Control.MouseButtons & MouseButtons.Right) != 0)
            {
                Process.Start(e.LinkText);
            }
            else
            {
                //Clipboard.SetText(e.LinkText);
                StartDownload(e.LinkText);
            }
        }

        private void TaskList_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("UniformResourceLocator") || e.Data.GetDataPresent("UniformResourceLocatorW") || e.Data.GetDataPresent("FileDrop"))
            {
                e.Effect = DragDropEffects.Link;
            }
        }

        private void TaskList_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("UniformResourceLocatorW"))
            {
                using (StreamReader reader = new StreamReader(e.Data.GetData("UniformResourceLocatorW") as MemoryStream, Encoding.Unicode))
                {
                    StartDownload(reader.ReadToEnd().TrimEnd('\0'));
                }
            }
            else if (e.Data.GetDataPresent("UniformResourceLocator"))
            {
                using (StreamReader reader = new StreamReader(e.Data.GetData("UniformResourceLocator") as MemoryStream))
                {
                    StartDownload(reader.ReadToEnd().TrimEnd('\0'));
                }
            }
            else
            {
                foreach (string path in e.Data.GetData("FileDrop") as string[])
                {
                    string url = Parser.ToURL(Path.GetFileName(path));
                    if (!string.IsNullOrEmpty(url))
                    {
                        StartDownload(url);
                    }
                }
            }
        }

        private void Tray_MouseClick(object sender, MouseEventArgs e)
        {
            Tray.Visible = false;
            Show();
            //WindowState = SaveState;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.IsClosing = true;
            Viewer.Enabled = false;

            // Save tasks ...
            foreach (ListViewItem item in TaskList.Items)
            {
                Downloader downloader = item.Tag as Downloader;
                if (downloader != null)
                {
                    downloader.Cancel();
                }
                AppConfig.Settings.Tasks.Add(item.Name);
            }
        }

        private void miLanguage_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripMenuItem mi = e.ClickedItem as ToolStripMenuItem;
            if (!mi.Checked)
            {
                miLangAuto.Checked = miLangChs.Checked = miLangEn.Checked = false;
                mi.Checked = true;

                AppConfig.Settings.Language = miLanguage.DropDownItems.IndexOf(mi);

                Program.SetCultureInfo();

                // Change UI ...
                ComponentResourceManager resources = new ComponentResourceManager(typeof(MainForm));

                resources.ApplyResources(this.FBD, "FBD");
                resources.ApplyResources(this.OFD, "OFD");

                resources.ApplyResources(this.colURL, "colURL");
                resources.ApplyResources(this.colName, "colName");
                resources.ApplyResources(this.colSize, "colSize");
                resources.ApplyResources(this.colProgress, "colProgress");
                resources.ApplyResources(this.colSpeed, "colSpeed");
                resources.ApplyResources(this.colETA, "colETA");

                resources.ApplyResources(this.btnSavePath, "btnSavePath");
                resources.ApplyResources(this.URL, "URL");
                resources.ApplyResources(this.lbTask, "lbTask");
                resources.ApplyResources(this.MaxDownloaders, "MaxDownloaders");
                resources.ApplyResources(this.btnOption, "btnOption");

                resources.ApplyResources(this.miLanguage, "miLanguage");
                resources.ApplyResources(this.miLangAuto, "miLangAuto");
                resources.ApplyResources(this.miLangChs, "miLangChs");
                resources.ApplyResources(this.miLangEn, "miLangEn");
                resources.ApplyResources(this.miConnection, "miConnection");
                resources.ApplyResources(this.miClipboard, "miClipboard");
                resources.ApplyResources(this.miParseOnly, "miParseOnly");
                resources.ApplyResources(this.miCopyURL, "miCopyURL");
                resources.ApplyResources(this.miRunDownloader, "miRunDownloader");

                resources.ApplyResources(this.miOpen, "miOpen");
                resources.ApplyResources(this.miMoveTop, "miMoveTop");
                resources.ApplyResources(this.miMoveUp, "miMoveUp");
                resources.ApplyResources(this.miMoveDown, "miMoveDown");
                resources.ApplyResources(this.miMoveBottom, "miMoveBottom");
                resources.ApplyResources(this.miDelete, "miDelete");
                resources.ApplyResources(this.miSelectAll, "miSelectAll");
            }
        }

        private void miConnection_Click(object sender, EventArgs e)
        {
            Process.Start("rundll32.exe", "shell32.dll,Control_RunDLL inetcpl.cpl,,4");
        }

        private void cmTaskList_Opening(object sender, CancelEventArgs e)
        {
            int n = TaskList.Items.Count;
#if false
            if (n == 0)
            {
                e.Cancel = true;
            }
            else
#endif
            {
                int i = TaskList.SelectedItems.Count;
                miOpen.Visible = miSeparator1.Visible = (i == 1);
                miMoveTop.Visible = miMoveUp.Visible = miMoveDown.Visible = miMoveBottom.Visible = miSeparator2.Visible = (n > i && i > 0);
                miDelete.Visible = (i > 0);
                miSelectAll.Visible = (n > i);
                miSeparator3.Visible = (i > 0 && n > i);
            }
        }

        private void miMoveTop_Click(object sender, EventArgs e)
        {
            if (TaskList.Items.Count > 1 && TaskList.SelectedItems.Count > 0)
            {
                TaskList.BeginUpdate();

                int n = 0;
                int fi = TaskList.SelectedItems.IndexOf(TaskList.FocusedItem);
                foreach (ListViewItem item in TaskList.SelectedItems)
                {
                    int ii = item.Index;
                    if (ii > n)
                    {
                        item.Remove();
                        TaskList.Items.Insert(n, item);
                    }
                    n++;
                }
                if (fi >= 0)
                {
                    TaskList.Items[fi].Focused = true;
                }

                TaskList.EndUpdate();
            }
        }

        private void miMoveUp_Click(object sender, EventArgs e)
        {
            if (TaskList.Items.Count > 1 && TaskList.SelectedItems.Count > 0)
            {
                TaskList.BeginUpdate();

                int n = 0;
                int fi = TaskList.FocusedItem.Index;
                foreach (ListViewItem item in TaskList.SelectedItems)
                {
                    int ii = item.Index;
                    if (ii > n)
                    {
                        item.Remove();
                        TaskList.Items.Insert(ii - 1, item);
                    }
                    else
                    {
                        n++;
                    }
                }
                if (fi > n)
                {
                    TaskList.Items[fi - 1].Focused = true;
                }

                TaskList.EndUpdate();
            }
        }

        private void miMoveDown_Click(object sender, EventArgs e)
        {
            int n = TaskList.Items.Count - 1;
            int i = TaskList.SelectedItems.Count;
            if (n > 0 && i > 0)
            {
                TaskList.BeginUpdate();

                int fi = TaskList.FocusedItem.Index;
                while (i-- > 0)
                {
                    ListViewItem item = TaskList.SelectedItems[i];
                    int ii = item.Index;
                    if (ii < n)
                    {
                        item.Remove();
                        TaskList.Items.Insert(ii + 1, item);
                    }
                    else
                    {
                        n--;
                    }
                }
                if (fi < n)
                {
                    TaskList.Items[fi + 1].Focused = true;
                }

                TaskList.EndUpdate();
            }
        }

        private void miMoveBottom_Click(object sender, EventArgs e)
        {
            int n = TaskList.Items.Count - 1;
            int i = TaskList.SelectedItems.Count;
            if (n > 0 && i > 0)
            {
                TaskList.BeginUpdate();

                int fi = TaskList.SelectedItems.IndexOf(TaskList.FocusedItem);
                while (i-- > 0)
                {
                    ListViewItem item = TaskList.SelectedItems[i];
                    int ii = item.Index;
                    if (ii < n)
                    {
                        item.Remove();
                        TaskList.Items.Insert(n, item);
                    }
                    n--;
                }
                if (fi >= 0)
                {
                    TaskList.SelectedItems[fi].Focused = true;
                }

                TaskList.EndUpdate();
            }
        }

        private void miDelete_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in TaskList.SelectedItems)
            {
                if (item.Tag != null)
                {
                    (item.Tag as Downloader).Cancel();
                }
                item.Remove();
            }
            if (TaskList.FocusedItem != null)
            {
                TaskList.FocusedItem.Selected = true;
            }
        }

        private void miSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in TaskList.Items)
            {
                item.Selected = true;
            }
        }

        private void miOpen_Click(object sender, EventArgs e)
        {
            TaskList_DoubleClick(sender, e);
        }

        private void miClipboard_Click(object sender, EventArgs e)
        {
            AppConfig.Settings.Clipboard = miClipboard.Checked = Viewer.Enabled = !miClipboard.Checked;
        }

        private void miTrace_Click(object sender, EventArgs e)
        {
            AppConfig.Settings.Trace = miTrace.Checked = Parser.Trace = !miTrace.Checked;
            foreach (ToolStripMenuItem mi in miTrace.DropDownItems) mi.Visible = miTrace.Checked;
            ShowAria2(Parser.Trace);
            if (miTrace.Checked)
            {
                MainSplit.Panel2Collapsed = miLog.Enabled = false;
                //AppConfig.Settings.Log = miLog.Checked = true;
                btnOption.ShowDropDown();
                miTrace.ShowDropDown();
            }
            else
            {
                miLog.Enabled = true;
                MainSplit.Panel2Collapsed = !miLog.Checked;
                btnOption.HideDropDown();
            }
        }

        private void miTrace_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            foreach (ToolStripMenuItem mi in miTrace.DropDownItems) mi.Checked = false;
            (e.ClickedItem as ToolStripMenuItem).Checked = true;
            AppConfig.Settings.LogLevel = e.ClickedItem.Name;

            Aria2.SetOptions("log:,log-level:" + AppConfig.Settings.LogLevel);
            Aria2.SetOptions("log:-"); // restart logging
        }

        private void miProxy_Click(object sender, EventArgs e)
        {
            AppConfig.Settings.UseProxy = miProxy.Checked = Web.UseProxy = !miProxy.Checked;
        }

        private void miLog_Click(object sender, EventArgs e)
        {
            MainSplit.Panel2Collapsed = !(AppConfig.Settings.Log = miLog.Checked = MainSplit.Panel2Collapsed);
        }

        private void miParseOnly_Click(object sender, EventArgs e)
        {
            AppConfig.Settings.ParseOnly = miParseOnly.Checked = !miParseOnly.Checked;
            foreach (ToolStripMenuItem mi in miParseOnly.DropDownItems) mi.Visible = miParseOnly.Checked;
            if (miParseOnly.Checked)
            {
                btnOption.ShowDropDown();
                miParseOnly.ShowDropDown();
            }
            else
            {
                btnOption.HideDropDown();
            }
        }

        private void miParseOnly_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripMenuItem mi = e.ClickedItem as ToolStripMenuItem;

            btnOption.HideDropDown();

            if (mi == miRunDownloader && (miRunDownloader.Checked || !File.Exists(AppConfig.Settings.ExternalDownloader)) &&
                OFD.ShowDialog() == DialogResult.OK)
            {
                AppConfig.Settings.ExternalDownloader = OFD.FileName;
            }

            if (File.Exists(AppConfig.Settings.ExternalDownloader))
            {
                miCopyURL.Checked = miRunDownloader.Checked = false;
                mi.Checked = true;
            }
            else
            {
                miCopyURL.Checked = true;
                miRunDownloader.Checked = false;
                mi = miCopyURL;
            }

            AppConfig.Settings.ExternalMethod = miParseOnly.DropDownItems.IndexOf(mi);
        }

        private void miEditINI_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(Parser.ScriptFile);  // Edit Parser Script ...
            }
            catch (Exception ex)
            {
                Log(Resources.Error + ex.Message);
            }
        }

        private void miOpenDir_Click(object sender, EventArgs e)
        {
            string dir = AppConfig.Settings.SavePath;

            if (TaskList.SelectedItems.Count > 0)
            {
                Downloader downloader = TaskList.SelectedItems[0].Tag as Downloader;
                if (downloader != null)
                {
                    dir = downloader.SavePath;
                }
            }
            Process.Start(dir);
        }

        private void TaskList_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            int w = TaskList.ClientSize.Width;
            int ww = TaskList.Columns[ITEM_URL].Width + TaskList.Columns[ITEM_NAME].Width;

            for (int i = ITEM_SIZE; i <= ITEM_ETA; i++)
            {
                //TaskList.Columns[i].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                w -= TaskList.Columns[i].Width;
            }
            TaskList.Columns[ITEM_NAME].Width = TaskList.Columns[ITEM_NAME].Width * w / ww;
            TaskList.Columns[ITEM_URL].Width = w - TaskList.Columns[ITEM_NAME].Width;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            TaskList_ColumnClick(null, null);
        }

        private void MainSplit_SplitterMoved(object sender, SplitterEventArgs e)
        {
            //TaskList.Select();
            TaskList.Focus();
        }

        private void MaxDownloaders_DropDownClosed(object sender, EventArgs e)
        {
            //TaskList.Select();
            TaskList.Focus();
        }

        private void URL_MouseEnter(object sender, EventArgs e)
        {
            URL.Focus();
        }

        private void Toolbar_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Hide();
            Tray.Visible = true;
        }

        private void miStayTop_CheckedChanged(object sender, EventArgs e)
        {
            TopMost = miStayTop.Checked;
        }
    }
}