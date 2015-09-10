using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace KK
{
    internal abstract class Web
    {
        private const string ProxyAddr = "127.0.0.1:8087";
        private static WebProxy Proxy;
        private static CookieContainer CC = new CookieContainer();

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

        private static object HttpRequest(string url, object encoding = null)
        {
            object result = null;
            int retrys = 3;

        retry:
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Proxy = Proxy;
                req.CookieContainer = CC;
                req.AutomaticDecompression = DecompressionMethods.GZip;
                req.Timeout = 10000; // ms
                req.Method = (encoding != null) ? "GET" : "HEAD";
                //req.KeepAlive = true;
                //req.UserAgent = "Safari/537.36";

                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                if (encoding == null)
                {
                    result = res.Headers;
                }
                else
                {
                    string s;
                    using (StreamReader reader = new StreamReader(res.GetResponseStream(), (Encoding)encoding))
                    {
                        s = reader.ReadToEnd();
                    }
                    result = s;
                }
                res.Close();
            }
            catch (Exception ex)
            {
                if (ex is WebException)
                {
                    WebException wx = ex as WebException;
                    HttpStatusCode code = (wx.Response == null) ? HttpStatusCode.Unused : ((HttpWebResponse)wx.Response).StatusCode;
                    if (wx.Status == WebExceptionStatus.Timeout ||
                        wx.Status == WebExceptionStatus.ReceiveFailure ||
                        code == HttpStatusCode.BadGateway ||
                        code == HttpStatusCode.ServiceUnavailable ||
                        //code == HttpStatusCode.InternalServerError ||
                        (code == HttpStatusCode.ProxyAuthenticationRequired && Credential.ProxyCredential(wx.Response) == Credential.CredUIReturnCodes.NO_ERROR)) // 代理服务器授权验证
                    {
                        if (retrys-- > 0) goto retry;
                    }
                }
                else if (ex is IOException)
                {
                    if (retrys-- > 0) goto retry;
                }
                throw;
            }

            return result;
        }
    }
}
