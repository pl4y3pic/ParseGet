using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace KK
{
    abstract class Credential
    {
        const int MAX_USERNAME = 100;
        const int MAX_PASSWORD = 100;
        const int MAX_DOMAIN = 100;

        public struct CREDUI_INFO
        {
            internal int cbSize;
            internal IntPtr hwndParent;
            internal string pszMessageText;
            internal string pszCaptionText;
            internal IntPtr hbmBanner;
        }

        public enum CredUIReturnCodes
        {
            NO_ERROR = 0,
            ERROR_CANCELLED = 1223,
            ERROR_NO_SUCH_LOGON_SESSION = 1312,
            ERROR_NOT_FOUND = 1168,
            ERROR_INVALID_ACCOUNT_NAME = 1315,
            ERROR_INSUFFICIENT_BUFFER = 122,
            ERROR_INVALID_PARAMETER = 87,
            ERROR_INVALID_FLAGS = 1004,
        }

        public enum CREDUI_FLAGS
        {
            NONE = 0x0,
            INCORRECT_PASSWORD = 0x1,
            DO_NOT_PERSIST = 0x2,
            REQUEST_ADMINISTRATOR = 0x4,
            EXCLUDE_CERTIFICATES = 0x8,
            REQUIRE_CERTIFICATE = 0x10,
            SHOW_SAVE_CHECK_BOX = 0x40,
            ALWAYS_SHOW_UI = 0x80,
            REQUIRE_SMARTCARD = 0x100,
            PASSWORD_ONLY_OK = 0x200,
            VALIDATE_USERNAME = 0x400,
            COMPLETE_USERNAME = 0x800,
            PERSIST = 0x1000,
            SERVER_CREDENTIAL = 0x4000,
            EXPECT_CONFIRMATION = 0x20000,
            GENERIC_CREDENTIALS = 0x40000,
            USERNAME_TARGET_CREDENTIALS = 0x80000,
            KEEP_USERNAME = 0x100000,
        }

        public static IntPtr OwnerHWnd = IntPtr.Zero;

        static CREDUI_FLAGS Flag = CREDUI_FLAGS.GENERIC_CREDENTIALS | CREDUI_FLAGS.SHOW_SAVE_CHECK_BOX | CREDUI_FLAGS.EXPECT_CONFIRMATION;

        static string ProxyUsername = string.Empty;
        static string ProxyPassword = string.Empty;

        internal static class NativeMethods
        {
            [DllImport("credui", CharSet = CharSet.Unicode)]
            internal static extern CredUIReturnCodes CredUIPromptForCredentials(
                                ref CREDUI_INFO creditUR,
                                string targetName,
                                IntPtr reserved1,
                                int iError,
                                StringBuilder userName,
                                int maxUserName,
                                StringBuilder password,
                                int maxPassword,
                                [MarshalAs(UnmanagedType.Bool)] ref bool pfSave,
                                CREDUI_FLAGS flags);

            [DllImport("credui.dll", EntryPoint = "CredUIConfirmCredentialsW", CharSet = CharSet.Unicode)]
            internal static extern CredUIReturnCodes CredUIConfirmCredentials(
                                string targetName,
                                [MarshalAs(UnmanagedType.Bool)] bool confirm);
        }

        public static CredUIReturnCodes ProxyCredential(WebResponse res)
        {
            var username = new StringBuilder(ProxyUsername, MAX_USERNAME);
            var password = new StringBuilder(ProxyPassword, MAX_PASSWORD);

            string host = WebRequest.DefaultWebProxy.GetProxy(res.ResponseUri).Host;
            bool savePwd = true;

            var info = new CREDUI_INFO();
            info.cbSize = Marshal.SizeOf(info);
            info.hwndParent = OwnerHWnd;
            info.pszCaptionText = "Connect to " + host;
            info.pszMessageText = Util.GetSubStr(res.Headers[HttpResponseHeader.ProxyAuthenticate], "\"", "\"");

            CredUIReturnCodes result = NativeMethods.CredUIPromptForCredentials(
                            ref info,
                            host,
                            IntPtr.Zero,
                            0,
                            username,
                            MAX_USERNAME,
                            password,
                            MAX_PASSWORD,
                            ref savePwd,
                            Flag);

            if (result == CredUIReturnCodes.NO_ERROR)
            {
                NativeMethods.CredUIConfirmCredentials(host, savePwd);

                ProxyUsername = username.ToString();
                ProxyPassword = password.ToString();
                if (ProxyUsername.Contains("\\"))
                {
                    ProxyUsername = ProxyUsername.Substring(ProxyUsername.LastIndexOf('\\') + 1);
                }
                WebRequest.DefaultWebProxy.Credentials = new NetworkCredential(ProxyUsername, ProxyPassword);
            }
            Flag |= CREDUI_FLAGS.ALWAYS_SHOW_UI;

            return result;
        }
    }
}