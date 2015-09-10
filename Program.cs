﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace ParseGet
{
    internal static class Program
    {
        public static bool IsClosing;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            try
            {
                if (args.Length > 0)
                {
                    foreach (string s in args)
                    {
                        ShooterDownloader.Start(s);
                    }
                }
                else
                {
                    AppConfig.Load();

                    SetCultureInfo();

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

        private static string[] Languages =
        {
            "zh-CHS",
            "en"
        };

        public static void SetCultureInfo()
        {
            int i = AppConfig.Settings.Language;
            if (i < 2) // Auto detect
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CurrentCulture;
            }
            else
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Languages[i - 2]);
            }
        }
    }
}