﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

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
                    foreach (string s in args)
                    {
                        ShooterDownloader.Start(s);
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