using System;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ParseGet
{
    public class AppSettings
    {
        public int MaxDownloaders;
        public int MaxConnections;
        public int Language;
        public int ExternalMethod;
        public bool ParseOnly;
        public bool Clipboard;
        public bool Trace;
        public bool UseProxy;
        public bool Log;
        public string LogLevel;
        public string SavePath;
        public string Proxy;
        public string ExternalDownloader;
        public StringCollection Tasks;

        public static AppSettings Default()
        {
            var settings = new AppSettings();
            settings.MaxDownloaders = 2;
            settings.MaxConnections = 1;
            settings.Language = 0;
            settings.ExternalMethod = 0;
            settings.ParseOnly = false;
            settings.Clipboard = true;
            settings.Trace = false;
            settings.UseProxy = true;
            settings.Log = true;
            settings.LogLevel = "debug";
            settings.Proxy = "127.0.0.1:8787";
            settings.SavePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            settings.ExternalDownloader = null;
            settings.Tasks = new StringCollection();

            return settings;
        }

        AppSettings()
        {
        }
    }

    public static class AppConfig
    {
        public static AppSettings Settings;

        static readonly XmlSerializer serializer = new XmlSerializer(typeof(AppSettings));

        public static void Load()
        {
            // Deserialization
            try
            {
                using (TextReader r = new StreamReader(Application.ExecutablePath + ".App.Config"))
                {
                    Settings = (AppSettings)serializer.Deserialize(r);
                }
            }
            // disable once EmptyGeneralCatchClause
            catch { }
            finally
            {
                if (Settings == null)
                {
                    Settings = AppSettings.Default();
                }
                if (string.IsNullOrEmpty(Settings.LogLevel))
                {
                    Settings.LogLevel = "debug";
                }
            }
        }

        public static void Save()
        {
            // Serialization
            using (TextWriter w = new StreamWriter(Application.ExecutablePath + ".App.Config"))
            {
                serializer.Serialize(w, Settings);
            }
        }
    }
}