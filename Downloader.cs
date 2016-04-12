using KK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading;

namespace ParseGet
{
    internal class Downloader : BackgroundWorker
    {
        public const int PROGRESS = 0;
        public const int FILEINFO = -1;
        public const int STATUS = -2;
        public const int CHANGEURL = -3;
        public const int NEWURL = -4;

        public object Tag;
        public string Referer;
        public string SavePath;
        public string FileName;
        public long FileSize;
        public long Transfered;
        public int Speed;
        public int ErrCode;

        private static SortedList<int, string> ErrMsgs = new SortedList<int, string>
        {
            {2, "time out"},
            {3, "resource not found"},
            {5, "speed too low, aborted"},
            {8, "server not support resume"},
            {9, "not enough disk space"},
            {13, "file already existed"},
        };

        private static int count;

        public static int Count
        {
            get
            {
                return count;
            }
        }

        public Downloader()
        {
            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
            DoWork += new DoWorkEventHandler(Downloader_DoWork);

            count++;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) count--;
            base.Dispose(disposing);
        }

        public void ReportStatus(string s)
        {
            ReportProgress(STATUS, s);
        }

        public void Cancel()
        {
            CancelAsync();
        }

        private string id;

        private void Download_File(string url)
        {
            string r = string.IsNullOrEmpty(Referer) ? null : string.Format(", \"referer\":\"{0}\"", Referer);
            string s = string.IsNullOrEmpty(FileName) ? null : string.Format(", \"out\":\"{0}\"", FileName);

            // add a new download task to aria2
            s = string.Format("[\"{0}\"], {{\"dir\":\"{1}\"{2}{3}}}", url, SavePath.Replace("\\", "/"), r, s);
            id = Aria2.AddUri(s);
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception("addUri fail");
            }

            // begin download
            do
            {
                Thread.Sleep(1000);
                s = Aria2.TellStatus(id, "errorCode, downloadSpeed, completedLength, status, files");
                Int32.TryParse(Util.GetSubStr(s, "errorCode\":\"", "\""), out ErrCode);
                Int32.TryParse(Util.GetSubStr(s, "downloadSpeed\":\"", "\""), out Speed);
                Int64.TryParse(Util.GetSubStr(s, "completedLength\":\"", "\""), out Transfered);
                if (FileSize == 0)
                {
                    FileName = Path.GetFileName(Util.GetSubStr(s, "path\":\"", "\""));
                    FileSize = Int64.Parse(Util.GetSubStr(s, "\"length\":\"", "\""));
                    ReportProgress(FILEINFO);
                }
                ReportProgress(PROGRESS, Transfered);
            } while (!CancellationPending && (s.Contains("status\":\"active") || s.Contains("status\":\"paused")));

            // end download
            Aria2.Remove(id);

            if (ErrCode != 0)
            {
                try
                {
                    s = ErrMsgs[ErrCode];
                }
                catch
                {
                    s = "unknown error occurred";
                }
                throw new Exception(s);
            }
        }

        private void Download_Playlist(string url)
        {
            string host = url.Remove(url.IndexOf("/", 7));
            string[] ss = Web.HttpGet(url).Split('\n');
            byte[] buffer = new byte[32768];
            FileStream f = new FileStream(SavePath + "\\" + FileName, FileMode.OpenOrCreate);
            long Downloaded = f.Length;

            f.Seek(0, SeekOrigin.End);
            ReportProgress(FILEINFO);

            foreach (string s in ss)
            {
                int retrys = 3;

                if (!s.StartsWith("/")) continue;

            retry:
                try
                {
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(host + s);
                    req.Proxy = Web.Proxy;
                    HttpWebResponse res = (HttpWebResponse)req.GetResponse();

                    FileSize = Int32.Parse(res.Headers["Content-Length"]);
                    if (Downloaded >= FileSize)
                    {
                        Downloaded -= FileSize;
                        res.Close();
                        continue;
                    }
                    Transfered = Downloaded;
                    if (Transfered > 0)
                    {
                        Downloaded = 0;
                        res.Close();
                        req = (HttpWebRequest)WebRequest.Create(host + s);
                        req.Proxy = Web.Proxy;
                        req.AddRange(Transfered);
                        res = (HttpWebResponse)req.GetResponse();
                    }

                    using (Stream rs = res.GetResponseStream())
                    {
                        int len = 0;
                        long Last = 0;
                        long LastTick = DateTime.Now.Ticks;

                        while (!CancellationPending && (len = rs.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            f.Write(buffer, 0, len);
                            Transfered += len;

                            long Tick = DateTime.Now.Ticks;
                            double t = TimeSpan.FromTicks(Tick - LastTick).TotalSeconds;
                            if (t > 1)
                            {
                                Speed = (int)((Transfered - Last) / t);
                                Last = Transfered;
                                LastTick = Tick;
                                ReportProgress(PROGRESS, Transfered);
                            }
                        }
                    }
                    if (CancellationPending) break;
                }
                catch (Exception ex)
                {
                    if (Web.IsNeedRetry(ex) && retrys-- > 0) goto retry;
                    f.Close();
                    throw;
                }
            }
            f.Close();
        }

        private void Downloader_DoWork(object sender, DoWorkEventArgs e)
        {
            string url = e.Argument as string;

            // Set Thread CultureInfo
            Program.SetCultureInfo();

            // Retrieving real download url ...
            if (Parser.IsValidURL(url) > 0)
            {
                //ReportStatus(Resources.RetrieveURL);
                if (!Parser.Parse(this, ref url))
                {
                    throw new Exception("Cann't retrieving a real URL");
                }
                else if (!url.StartsWith("http") || AppConfig.Settings.ParseOnly)
                {
                    e.Result = url;
                    return;
                }
            }

            FileName = Util.ValidFileName(FileName);
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }

            if (url.EndsWith(".m3u8")) /* playlist */
            {
                Download_Playlist(url);
            }
            else
            {
                Download_File(url);
            }

            e.Cancel = CancellationPending;
        }
    }
}