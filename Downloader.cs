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

        private void Downloader_DoWork(object sender, DoWorkEventArgs e)
        {
            string url = e.Argument as string;
            string param, s;

            // Set Thread CultureInfo
            Program.SetCultureInfo();

            // Retrieving real download url ...
            if (Parser.IsValidURL(url) > 0)
            {
                //ReportStatus(Resources.RetrieveURL);
                if (!Parser.Parse(this, ref url, ref FileName))
                {
                    throw new Exception("Cann't retrieving a real URL");
                }
                else if (!url.StartsWith("http") || AppConfig.Settings.ParseOnly)
                {
                    e.Result = url;
                    return;
                }
            }

            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }

            if (string.IsNullOrEmpty(FileName))
            {
                param = string.Format("[\"{0}\"], {{\"dir\":\"{1}\"}}", url, SavePath.Replace("\\", "/"));
            }
            else
            {
                FileName = Util.ValidFileName(FileName);
                param = string.Format("[\"{0}\"], {{\"dir\":\"{1}\", \"out\":\"{2}\"}}", url, SavePath.Replace("\\", "/"), FileName);
            }

            // add a new download task to aria2
            id = Aria2.AddUri(param);
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
            e.Cancel = CancellationPending;
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
    }
}