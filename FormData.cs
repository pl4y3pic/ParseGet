using System.Collections.Generic;
using System.Text;

namespace ParseGet
{
    class FormData
    {
        readonly Dictionary<string, string> postData = new Dictionary<string,string>();

		public string Boundary {
			get;
			set;
		}

        public void AddData(string key, string value)
        {
            postData.Add(key, value);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in postData)
            {
                sb.AppendFormat("--{0}\r\n", Boundary);
                sb.AppendFormat("Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}\r\n",
                    pair.Key,
                    pair.Value);
            }
            sb.AppendFormat("--{0}--\r\n", Boundary);
            return sb.ToString();
        }
    }
}
