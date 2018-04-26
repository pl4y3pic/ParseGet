using System;
using System.IO;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using KK;

namespace ParseGet
{
    static class Program
    {
        public static bool IsClosing;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                AppConfig.Load();
                SetCultureInfo();

                if (args.Length > 0)
                {
                    string s = args[0];
                    if (s == "/sub")
                    {
                        for (int i = 1; i < args.Length; i++)
                        {
                            ShooterDownloader.Start(args[i]);
                        }
                    }
                    else if (s == "/crypt")
                    {
                        for (int i = 1; i < args.Length; i++)
                        {
                            Util.Crypt(args[i]);
                        }
                        //MessageBox.Show("All done!");
                    }
                    else if (s == "/namer")
                    {
                        for (int i = 1; i < args.Length; i++)
                        {
                        	s = args[i];
                        	int j = s.LastIndexOf('\\') + 1;
                        	string dir = s.Substring(0, j);
                       		string id = s.Substring(j, s.LastIndexOf('.') - j);
                       		string ss;
                       		bool subbed = Regex.Match(id, "[cCrR]$").Success;

                       		foreach (Match m in Regex.Matches(id, "[a-zA-Z]{2,5}-?\\d{2,5}"))
                       		{
	                       		try
	                        	{
		                       		id = m.Value;
		                       		#if false
		                       		if (!id.Contains("-"))
		                       		{
		                       			id = id.Insert(Regex.Match(id, "\\d+").Index, "-");
		                       		}
		                       		#endif
		
		                        	s = Web.HttpGet("http://www.19lib.com/cn/vl_searchbyid.php?keyword=" + id);
		                        	j = s.IndexOf("<div class=\"id\">" + id + "</div>");
		                        	if (j > 0)
		                        	{
		                        		j = s.IndexOf("<a id=\"", j) + 7;
		                        		s = Web.HttpGet("http://www.19lib.com/cn/?v=" + s.Substring(j, 10));
		                        	}
		                        	id = Util.GetSubStr(s, "<title>", " - JAVLibrary</title>");
		                        	s = Util.GetSubStr(s, "<div id=\"video_cast\"", "</tr>");
		                        	if (string.IsNullOrEmpty(s)) continue;
		                        	j = 0;
		                        	while (true)
		                        	{
		                        		j = s.IndexOf("rel=\"tag\">", j);
		                        		if (j < 0) break;
		                        		j = j + 10;
		                        		ss = s.Substring(j, s.IndexOf('<', j) - j);
		                        		if (!id.Contains(ss))
		                        		{
		                        			id += " " + ss;
		                        		}
		                        	}

		                        	if (subbed) id += " (S)";
		                        	File.Move(args[i], dir + id + ".mpv");
		                        	break;
	                        	}
	                       		catch {}
                       		}
                        }
                    }
                }
                else
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new MainForm());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                AppConfig.Save();
            }
        }

        static readonly string[] Languages =
        {
            "zh-CHS",
            "en"
        };

        public static void SetCultureInfo()
        {
            int i = AppConfig.Settings.Language;
            Thread.CurrentThread.CurrentUICulture = (i < 2) ? // Auto detect
            	CultureInfo.CurrentCulture :
            	CultureInfo.GetCultureInfo(Languages[i - 2]);
        }
    }
}