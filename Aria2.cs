using KK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;

namespace ParseGet
{
    internal static class Aria2
    {
        private static string JsonRpcUrl = "http://127.0.0.1:6800/jsonrpc";
        public static List<string> ids = new List<string>();

        private static string RPC(string method, string param = null)
        {
            string url = string.Format("{0}?method=aria2.{1}&id=PG", JsonRpcUrl, method);
            if (!string.IsNullOrEmpty(param))
            {
                url += "&params=" + Convert.ToBase64String(Encoding.UTF8.GetBytes("[" + param + "]"));
            }
            return Web.HttpGet(url);
        }

        public static string AddUri(string param)
        {
            string id = Util.GetSubStr(RPC("addUri", param), "result\":", "}");
            if (!string.IsNullOrEmpty(id))
            {
                ids.Add(id);
            }
            return id;
        }

        public static void Remove(string id)
        {
            ids.Remove(id);
            try
            {
                RPC("remove", id);
            }
            catch {};
        }

        public static string TellStatus(string id, string param = null)
        {
            if (!string.IsNullOrEmpty(param))
            {
                id += ", [\"" + param.Replace(", ", "\", \"") + "\"]";
            }
            return RPC("tellStatus", id);
        }

        public static void SetOptions(string options)
        {
            RPC("changeGlobalOption", "{\"" + options.Replace(":", "\":\"").Replace(",", "\", \"") + "\"}");
        }

        private static void SetOptions_DoWork(object sender, DoWorkEventArgs e)
        {
            string options = (string)(e.Argument as object[])[0];
            bool pause = (bool)(e.Argument as object[])[1];

            SetOptions(options);
            if (ids.Count > 0)
            {
                if (pause)
                {
                    RPC("pauseAll");
                    do
                    {
                        Thread.Sleep(300);
                    } while (!RPC("tellActive", "[\"gid\"]").EndsWith("[]}"));
                }

                options = options.Replace(":", "\":\"").Replace(",", "\", \"");
                foreach (string id in ids)
                {
                    RPC("changeOption", id + ", {\"" + options + "\"}");
                }

                if (pause)
                {
                    RPC("unpauseAll");
                }
            }
        }

        public static void SetOptions(string options, bool pause)
        {
            BackgroundWorker bg = new BackgroundWorker();
            bg.DoWork += new DoWorkEventHandler(SetOptions_DoWork);
            bg.RunWorkerAsync(new object[] { options, pause });
        }

    }
}
