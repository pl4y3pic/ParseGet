using System;
using System.IO;
using System.Net;
using System.Text;

namespace KK
{
    abstract class Web
    {
        public static string ProxyAddr;
        public static WebProxy Proxy;
        public static CookieContainer CC = new CookieContainer();

        public static bool UseProxy
        {
            get
            {
                return (Proxy != null);
            }
            set
            {
                Proxy = value ? new WebProxy(ProxyAddr) : null;
            }
        }

        public static string HttpGet(string url)
        {
            return HttpGet(url, Encoding.UTF8);
        }

        public static string HttpGet(string url, Encoding encoding)
        {
            return (string)HttpRequest(url, encoding);
        }

        public static WebHeaderCollection HttpHead(string url)
        {
            return (WebHeaderCollection)HttpRequest(url);
        }

        public static bool IsNeedRetry(Exception ex)
        {
			var webException = ex as WebException;
            if (webException != null)
            {
                var wx = webException;
                HttpStatusCode code = (wx.Response == null) ? HttpStatusCode.Unused : ((HttpWebResponse)wx.Response).StatusCode;
                if (wx.Status == WebExceptionStatus.Timeout ||
                    wx.Status == WebExceptionStatus.ReceiveFailure ||
                    code == HttpStatusCode.BadGateway ||
                    code == HttpStatusCode.ServiceUnavailable ||
                    //code == HttpStatusCode.InternalServerError ||
                    (code == HttpStatusCode.ProxyAuthenticationRequired && Credential.ProxyCredential(wx.Response) == Credential.CredUIReturnCodes.NO_ERROR)) // 代理服务器授权验证
                {
                    return true;
                }
            }
            return (ex is IOException);
        }

        static object HttpRequest(string url, object encoding = null)
        {
            object result = null;
            int retrys = 3;

        retry:
            try
            {
                var req = (HttpWebRequest)WebRequest.Create(url);
                req.Proxy = Proxy;
                req.CookieContainer = CC;
                req.AutomaticDecompression = DecompressionMethods.GZip;
                req.Timeout = 20000; // ms
                req.Method = (encoding != null) ? "GET" : "HEAD";
                req.KeepAlive = true;
                req.UserAgent = "Safari/537.36";

                var res = (HttpWebResponse)req.GetResponse();
                if (encoding == null)
                {
                    result = res.Headers;
                }
                else
                {
                    string s;
                    using (var reader = new StreamReader(res.GetResponseStream(), (Encoding)encoding))
                    {
                        s = reader.ReadToEnd();
                    }
                    result = s;
                }
                res.Close();
            }
            catch (Exception ex)
            {
                if (IsNeedRetry(ex) && retrys-- > 0) goto retry;
                throw;
            }

            return result;
        }
    }
}
