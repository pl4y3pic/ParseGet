using KK;
using System;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;

namespace ParseGet
{
    abstract class ShooterDownloader
    {
        //ShooterNet
        public static void Start(string filePath)
        {
            string fileHash = Util.CaculateFileHash(filePath);
            string fileName = Path.GetFileName(filePath);

            var formData = new FormData();
            formData.Boundary = "----------------------------767a02e50d82";
            formData.AddData("pathinfo", fileName);
            formData.AddData("filehash", fileHash);

            string formDataString = formData.ToString();

            byte[] formDataUtf8 = Encoding.UTF8.GetBytes(formDataString);
            var request = WebRequest.Create("http://svplayer.shooter.cn/api/subapi.php") as HttpWebRequest;
            request.Method = "POST";
            request.UserAgent = "SPlayer Build 580";
            request.ContentType = "multipart/form-data; boundary=----------------------------767a02e50d82";
            request.ContentLength = formDataUtf8.Length;
            request.Timeout = 10000; // ms
            Stream requestStream = null;
            bool bReqOk = false;
            try
            {
                requestStream = request.GetRequestStream();
                requestStream.Write(formDataUtf8, 0, formDataUtf8.Length);
                requestStream.Flush();
                bReqOk = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, fileName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (requestStream != null)
                    requestStream.Close();
            }

            string tempFilePath = null;
            bool bRespOk = false;

            if (bReqOk)
            {
                HttpWebResponse response = null;
                Stream responseStream = null;
                FileStream outputStream = null;
                
                tempFilePath = Path.GetTempFileName();

                try
                {
                    response = request.GetResponse() as HttpWebResponse;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        responseStream = response.GetResponseStream();
                        var buffer = new byte[1024];
                        int len = 0;
                        outputStream = new FileStream(tempFilePath, FileMode.OpenOrCreate);
                        while ((len = responseStream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            outputStream.Write(buffer, 0, len);
                        }
                        bRespOk = true;
                    }
                    else
                    {
                        MessageBox.Show(response.StatusCode.ToString(), fileName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, fileName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if(outputStream != null)
                        outputStream.Close();
                    if(response != null)
                        response.Close();
                    if(responseStream != null)
                        responseStream.Close();
                }
            }

            if (bRespOk)
            {
                //Extract subtitle
                var extractor = new ShooterSubExtractor();
                extractor.VideoFilePath = filePath;
                extractor.DumpFilePath = tempFilePath;
                if (extractor.ExtractSubtitles() != ShooterSubExtractor.SubExtractResult.OK)
                {
                    MessageBox.Show("No subtitle found!", fileName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                File.Delete(tempFilePath);
            }
        }
    }
}
