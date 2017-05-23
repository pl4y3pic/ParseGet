using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace KK
{
    public enum ByteOrder
    {
        LittleEndian,
        BigEndian
    }

    abstract class Util
    {
        [Flags]
        internal enum ExecutionFlag : uint
        {
            System = 0x00000001,
            Display = 0x00000002,
            Continus = 0x80000000,
        }

        public static class NativeMethods
        {
            internal const UInt32 FLASHW_ALL = 0x00000003;
            internal const UInt32 FLASHW_TIMERNOFG = 0x0000000C;

            internal const int SW_HIDE = 0;
            internal const int SW_SHOWNORMAL = 1;
            internal const int SW_SHOWNOACTIVATE = 4;

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool SetForegroundWindow(IntPtr hWnd);

            [DllImport("user32.dll")]
            internal static extern IntPtr GetForegroundWindow();

            [DllImport("user32.dll")]
            internal static extern bool IsWindowVisible(IntPtr hWnd);

            [DllImport("user32.dll")]
            internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

            [DllImport("user32")]
            internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

            [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
            internal static extern IntPtr LoadLibrary(string path);
            [DllImport("kernel32", SetLastError = true)]
            internal static extern bool FreeLibrary(IntPtr hdl);
            [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
            internal static extern IntPtr GetProcAddress(IntPtr hdl, string name);

            [DllImport("kernel32.dll")]
            internal static extern uint SetThreadExecutionState(ExecutionFlag flags);

            [StructLayout(LayoutKind.Sequential)]
            internal struct FLASHWINFO
            {
                public UInt32 cbSize;
                public IntPtr hwnd;
                public UInt32 dwFlags;
                public UInt32 uCount;
                public UInt32 dwTimeout;
            }
        }

        /// <summary>
        ///阻止系统休眠，直到线程结束恢复休眠策略
        /// </summary>
        /// <param name="includeDisplay">是否阻止关闭显示器</param>
        public static void PreventSleep(bool includeDisplay = false)
        {
            if (includeDisplay)
                NativeMethods.SetThreadExecutionState(ExecutionFlag.System | ExecutionFlag.Display | ExecutionFlag.Continus);
            else
                NativeMethods.SetThreadExecutionState(ExecutionFlag.System | ExecutionFlag.Continus);
        }

        /// <summary>
        ///恢复系统休眠策略
        /// </summary>
        public static void ResotreSleep()
        {
            NativeMethods.SetThreadExecutionState(ExecutionFlag.Continus);
        }

        /// <summary>
        ///重置系统休眠计时器
        /// </summary>
        /// <param name="includeDisplay">是否阻止关闭显示器</param>
        public static void ResetSleepTimer(bool includeDisplay = false)
        {
            if (includeDisplay)
                NativeMethods.SetThreadExecutionState(ExecutionFlag.System | ExecutionFlag.Display);
            else
                NativeMethods.SetThreadExecutionState(ExecutionFlag.System);
        }

        public static void FlashWindow(IntPtr hwnd, UInt32 count)
        {
            // if the current foreground window isn't this window,
            // flash this window in task bar once every 1 second
            if (NativeMethods.GetForegroundWindow() != hwnd)
            {
                var fInfo = new NativeMethods.FLASHWINFO();

                fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
                fInfo.hwnd = hwnd;
                fInfo.dwFlags = NativeMethods.FLASHW_ALL | ((count == 0) ? NativeMethods.FLASHW_TIMERNOFG : 0);
                fInfo.uCount = count;
                fInfo.dwTimeout = 0;

                NativeMethods.FlashWindowEx(ref fInfo);
            }
        }

        public static string Input(IntPtr hwnd, string Prompt)
        {
            NativeMethods.SetForegroundWindow(hwnd);
            return Microsoft.VisualBasic.Interaction.InputBox(Prompt, string.Empty, string.Empty,
                (Screen.PrimaryScreen.WorkingArea.Width - 359) / 2,
                (Screen.PrimaryScreen.WorkingArea.Height - 145) / 2);
        }

        public const string InvalidChars = "\\/:*?<>|";

        public static string ValidFileName(string filename)
        {
            if (string.IsNullOrEmpty(filename)) return string.Empty;
            filename = filename.Trim();
            if (string.IsNullOrEmpty(filename)) return string.Empty;

            filename = filename.Replace("&quot;", "'").Replace("&amp;", "&");
            filename = filename.Replace("//", "~").Replace("/", "~");
            filename = filename.Replace("\\\"", "'").Replace("\"", "'");
            filename = filename.Replace(":", " ~ ");

            char[] s = filename.ToCharArray();
            for (int i = s.Length - 1; i >= 0; i--)
            {
                if (InvalidChars.Contains(s[i].ToString()))
                {
                    s[i] = '.';
                }
            }
            filename = (new StringBuilder()).Append(s).ToString();

            return filename;
        }

        const double KB = 1024.0F;
        const double MB = 1024.0F * 1024.0F;

        public static string ToSpeed(double speed)
        {
            if (speed <= 0)
            {
                return "0 B/s";
            }
            if (speed > MB)
            {
                return (speed / MB).ToString("F02") + " MB/s";
            }
            if (speed > KB)
            {
                return (speed / KB).ToString("F01") + " KB/s";
            }
            return speed + " B/s";
        }

        public static string ToSize(long size)
        {
            string s;

            if (size > 104857600)       // 100 MB
            {
                s = (size / 1048576) + " MB";
            }
            else if (size > 10485760)   // 10 MB
            {
                s = (size / 1048676.0F).ToString("F01") + " MB";
            }
            else if (size > 1048576)    // 1 MB
            {
                s = (size / 1048676.0F).ToString("F02") + " MB";
            }
            else if (size > 1024)       // 1 KB
            {
                s = (size / 1024) + " KB";
            }
            else
            {
                s = size + " B";
            }

            return s;
        }

        public static string GetSubStr(string s, string start)
        {
            int i = s.IndexOf(start, StringComparison.Ordinal);
            if (i < 0)
            {
                return null;
            }
            if (!start.StartsWith("http", StringComparison.Ordinal))
            {
                i += start.Length;
            }
            return s.Substring(i);
        }

        public static string GetSubStr(string s, string start, object end)
        {
            int i = s.IndexOf(start, StringComparison.Ordinal);
            if (i < 0)
            {
                return null;
            }
            if (!start.StartsWith("http", StringComparison.Ordinal))
            {
                i += start.Length;
            }

            int j = s.IndexOf(end.ToString(), i, StringComparison.Ordinal);
			return j < 0 ? s.Substring(i) : s.Substring(i, j - i);
        }

        public static string Base64Decode(string s)
        {
            switch (s.Length % 4)
            {
                case 1:
                    s = s + "===";
                    break;

                case 2:
                    s = s + "==";
                    break;

                case 3:
                    s = s + "=";
                    break;
            }
            byte[] bb = Convert.FromBase64String(s);
            var sb = new StringBuilder();
            foreach (byte b in bb)
            {
                sb.Append(Convert.ToChar(b));
            }

            return sb.ToString();
        }

        public static string Decode(string s)
        {
            int i = s.IndexOf("\\u", StringComparison.Ordinal);
            if (i < 0)
            {
                if (s.Contains("&amp;"))
                {
                    return HttpUtility.HtmlDecode(s);
                }

				if (s.StartsWith("http", StringComparison.Ordinal)) {
					s = s.Replace("\\/", "/");
				}
                return HttpUtility.UrlDecode(s);
            }

            int j = 0;
            var ss = new StringBuilder();
            do
            {
                ss.Append(s.Substring(j, i - j));
                ss.Append(Convert.ToChar(Convert.ToInt32(s.Substring(i + 2, 4), 16)));
                j = i + 6;
				i = s.IndexOf("\\u", j, StringComparison.Ordinal);
            } while (i >= 0);

            if (j < s.Length)
            {
                ss.Append(s.Substring(j));
            }

            return ss.ToString();
        }

        public static string CaculateFileHash(string filePath)
        {
            string hashString = "";
            var file = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            long fileLength = file.Length;
            var offset = new long[4];
            if (fileLength < 8192)
            {
                //a video file less then 8k? impossible! <-- says SPlayer

            }
            else
            {
                const int BlockSize = 4096;
                const int NumOfSegments = 4;

                offset[3] = fileLength - 8192;
                offset[2] = fileLength / 3;
                offset[1] = fileLength / 3 * 2;
                offset[0] = BlockSize;

                MD5 md5 = new MD5CryptoServiceProvider();

                var reader = new BinaryReader(file);
                var sb = new StringBuilder();
                for (int i = 0; i < NumOfSegments; i++)
                {
                    file.Seek(offset[i], SeekOrigin.Begin);
                    byte[] dataBlock = reader.ReadBytes(BlockSize);
                    MD5 md5Crypt = new MD5CryptoServiceProvider();
                    byte[] hash = md5Crypt.ComputeHash(dataBlock);
                    if (sb.Length > 0)
                    {
                        sb.Append(';');
                    }
                    foreach (byte a in hash)
                    {
                        if (a < 16)
                            sb.AppendFormat("0{0}", a.ToString("x"));
                        else
                            sb.Append(a.ToString("x"));
                    }
                }

                reader.Close();
                hashString = sb.ToString();
            }

            return hashString;
        }

        public static int BytesToInt32(byte[] bytes, ByteOrder byteOrder)
        {
            if (byteOrder == ByteOrder.BigEndian)
                Array.Reverse(bytes);

            return BitConverter.ToInt32(bytes, 0);
        }

        public static void PaddingZero(string fileName)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(fileName, FileMode.Append);
                fs.WriteByte(0);
            }
            catch
            {
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }

        public static bool UnGZip(string inFile, string outFile)
        {
            bool ret = false;
            FileStream inStream = null;
            FileStream outStream = null;
            GZipStream decompressStream = null;
            try
            {
                inStream = new FileStream(inFile, FileMode.Open);
                decompressStream = new GZipStream(inStream, CompressionMode.Decompress);
                outStream = new FileStream(outFile, FileMode.OpenOrCreate);

                var buffer = new byte[4096];
                int accuRead = 0;
                while ((accuRead = decompressStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    outStream.Write(buffer, 0, accuRead);
                }
                ret = true;
            }
            // disable once EmptyGeneralCatchClause
            catch
            {
            }
            finally
            {
                if (decompressStream != null)
                {
                    decompressStream.Close();
                }
                else if (inStream != null)
                {
                    inStream.Close();
                }

                if (outStream != null)
                    outStream.Close();
            }

            return ret;
        }

        public static void RunProc(string filePath, string args, bool needElevation)
        {
            var procInfo = new ProcessStartInfo();
            procInfo.UseShellExecute = true;
            procInfo.FileName = filePath;
            procInfo.Arguments = args;
            procInfo.WorkingDirectory = Environment.CurrentDirectory;
            if (needElevation)
                procInfo.Verb = "runas";

            Process proc = Process.Start(procInfo);
            const int timeout = 5000;
            //proc.WaitForInputIdle();
            proc.WaitForExit(timeout);
        }

        public static bool RegisterDll(string dllPath)
        {

            string regsvr32Path = String.Format("\"{0}\\regsvr32.exe\"",
                 Environment.GetFolderPath(Environment.SpecialFolder.System));
            string arg = String.Format("/s \"{0}\"", dllPath);

            try
            {
                //Need administrative privilege to register a COM DLL.
                RunProc(regsvr32Path, arg, !IsAdmin);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static bool UnregisterDll(string dllPath)
        {
            string regsvr32Path = String.Format("\"{0}\\regsvr32.exe\"",
                 Environment.GetFolderPath(Environment.SpecialFolder.System));
            string arg = String.Format("/s /u \"{0}\"", dllPath);

            try
            {
                //Need administrative privilege to register a COM DLL.
                RunProc(regsvr32Path, arg, !IsAdmin);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static bool IsAdmin
        {
            get
            {
                WindowsIdentity id = WindowsIdentity.GetCurrent();
                var p = new WindowsPrincipal(id);
                return p.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        const int BCM_FIRST = 0x1600;
        const int BCM_SETSHIELD = (BCM_FIRST + 0x000C);

        //Add a shield ICON to the button to inform user privilege elevation is required.
        // Only work on Vista or above.
        public static void AddShieldToButton(Button b)
        {
            b.FlatStyle = FlatStyle.System;
            NativeMethods.SendMessage(b.Handle, BCM_SETSHIELD, (IntPtr)0, (IntPtr)0xFFFFFFFF);
        }

        private delegate bool IsWow64Process(
            [In] IntPtr hProcess,
            [Out] out bool wow64Process);

        public static bool Is64BitOS
        {
            get
            {
                if (IntPtr.Size == 8)
                {
                    //Application running in 64-bit mode, must be 64-bit OS.
                    return true;
                }
                else
                {
                    //Dynamicallt load kernel32 and call IsWow64Process.
                    IntPtr module = NativeMethods.LoadLibrary("Kernel32.dll");
                    if (module == IntPtr.Zero)
                    {
                        return false;
                    }

                    IntPtr addr = NativeMethods.GetProcAddress(module, "IsWow64Process");
                    if (addr == IntPtr.Zero)
                    {
                        return false;
                    }

                    //Check if application is running in WOW64 mode.
                    // IsWow64Process only works on Windows XP sp2 or above.
                    //  Dynamically invoke it to avoid unnecessary dependency.
                    var dlg = (IsWow64Process)Marshal.GetDelegateForFunctionPointer(addr, typeof(IsWow64Process));
                    bool retval;
                    dlg.Invoke(Process.GetCurrentProcess().Handle, out retval);

                    return retval;
                }


            }
        }

        public static int GetGetBoundedValue(int intendValue, int lowerBound, int upperBound)
        {
            int trueValue;
            trueValue = Math.Min(intendValue, upperBound);
            trueValue = Math.Max(lowerBound, intendValue);
            return trueValue;
        }
    }
}