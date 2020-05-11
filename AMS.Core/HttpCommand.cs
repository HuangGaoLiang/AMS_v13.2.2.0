using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

namespace AMS.Core
{
    public class HttpCommand
    {
        public HttpCommand(string url, string requestMethod)
        {
            this.Url = url;
            this.RequestMethod = requestMethod;

            this.Headers = new NameValueCollection();
            this.Encoding = Encoding.UTF8;
            this.RetryTimes = 0;
            this.ExcutedTimes = 0;
        }

        public string Url
        {
            get;
            set;
        }

        public string ContentType
        {
            get;
            set;
        }

        public int RetryTimes
        {
            get;
            set;
        }

        public string RequestMethod
        {
            get;
            set;
        }

        public Encoding Encoding
        {
            get;
            set;
        }

        public NameValueCollection Headers
        {
            get;
            set;
        }

        public string Data
        {
            get;
            set;
        }


        public string Execute()
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(this.Url);
            try
            {
                request.Method = this.RequestMethod;
                request.ContentType = ContentType;
                request.Headers.Add(this.Headers);

                if (string.IsNullOrEmpty(this.Data) == false)
                {
                    byte[] postBytes = this.Encoding.GetBytes(this.Data);
                    request.ContentLength = postBytes.Length;
                    Stream stream = request.GetRequestStream();
                    stream.Write(postBytes, 0, postBytes.Length);
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(responseStream, this.Encoding);
                string result = sr.ReadToEnd();
                sr.Close();
                return result;
            }
            catch (WebException e)
            {
                if (e is WebException)
                {
                    var webEx = e as WebException;
                    var response = webEx.Response as HttpWebResponse;
                    if (response == null) return string.Empty;
                    var responseStream = response.GetResponseStream();
                    if (responseStream == null) return string.Empty;
                    var reader = new StreamReader(responseStream, Encoding.UTF8);
                    var result = reader.ReadToEnd(); // 返回的数据
                    reader.Close();
                    responseStream.Close();

                    return result;
                }
                return string.Empty;
            }
            finally
            {
                this.ExcutedTimes++;
            }
        }

        internal int ExcutedTimes
        {
            get;
            set;
        }
    }
}
