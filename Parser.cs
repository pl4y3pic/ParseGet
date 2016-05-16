using KK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;

namespace ParseGet
{
    static class Parser
    {
        public static string ScriptFile = Application.StartupPath + "\\parser.ini";
        public static bool Trace;

        public static int IsValidURL(string url)
        {
            foreach (string s in File.ReadAllLines(ScriptFile))
            {
                if (s.StartsWith("^", StringComparison.Ordinal) || s.StartsWith("http", StringComparison.Ordinal))
                {
                    if (Regex.IsMatch(url, s, RegexOptions.IgnoreCase))
                    {
                        return (s.StartsWith("^", StringComparison.Ordinal) ? 1 : 2);
                    }
                }
            }

            return 0;
        }

        public static string ToURL(string filename)
        {
            string url = null;
            foreach (string s in File.ReadAllLines(ScriptFile))
            {
                if (string.IsNullOrEmpty(s)) break;
                int i = s.IndexOf(" = ", StringComparison.Ordinal);
                Match m = Regex.Match(filename, s.Substring(0, i));
                if (m.Success)
                {
                    url = s.Substring(i + 3) + Util.GetSubStr(m.Value, "_", "_");
                    break;
                }
            }
            return url;
        }

        public static bool Parse(Downloader downloader, ref string url)
        {
            var vars = new VarList();
        reset:
            int retrys = 3;
        retry:
            bool bExec = false;
            string name = null;
            string script = null;
            string s;
            int line = 0;
            int i;

            vars["url"] = url;
            try
            {
                foreach (string ss in File.ReadAllLines(ScriptFile))
                {
                    s = script = ss.TrimStart();
                    line++; // line number
                    if (bExec) {
						if (string.IsNullOrEmpty(s)) {
							break; // Blank line, End
						}
						if (s.StartsWith("^", StringComparison.Ordinal)) {
							continue; // fall through
						}

						i = s.IndexOf('=');
						name = s.Substring(0, i).TrimEnd();
						s = s.Substring(i + 1);
						vars[name] = Evaluate(ref s, vars);
						if (Trace) {
							downloader.ReportStatus(string.Format("[{0}] {1} = {2}\r\n", line, name, vars[name]));
						}

						if (name == "result") {
							break;
						}
						if (name == "test") { // check result
							WebHeaderCollection headers = Web.HttpHead(vars["test"]);
							if (headers != null) {
								vars["result"] = vars["test"];
								break; // Got a valid result, End
							}
						} else if (name == "url") { // url changed
							url = vars["url"];
							downloader.ReportProgress(Downloader.CHANGEURL, url);
							vars.Clear();
							goto reset;
						} else if (name == "regexp") { // find match links
							MatchCollection m = Regex.Matches(Web.HttpGet(url), vars[name]);
							if (m.Count > 0) {
								i = 0;
								while (i < m.Count) {
									s = Util.GetSubStr(m[i++].Value, "\"", "\"");
									if (s.StartsWith("//", StringComparison.Ordinal)) {
#if false
                                        s = Regex.Match(url, "https?:").Value + s;
#else
										continue; // skip
#endif
									}
									if (s.StartsWith("/", StringComparison.Ordinal)) {
										s = Regex.Match(url, "https?://[^/]+/").Value + s.Remove(0, 1);
									} else {
										s = url.Remove(url.LastIndexOf("/", StringComparison.Ordinal) + 1) + s;
									}
									downloader.ReportProgress(Downloader.NEWURL, s);
								}
							}
							break;
						} else if (name == "rs" && !vars["rs"].Contains("http")) { // no result
							break; // End
						}
					}
                    else if (s.StartsWith("^", StringComparison.Ordinal) || s.StartsWith("http", StringComparison.Ordinal))
                    {
                        bExec = Regex.IsMatch(url, s, RegexOptions.IgnoreCase);
                    }
                }

                if (bExec)
                {
                    downloader.Referer = string.IsNullOrEmpty(vars["referer"]) ? url : vars["referer"];
                    url = vars["result"];
                    downloader.FileName = HttpUtility.HtmlDecode(vars["filename"]);
                    if (!string.IsNullOrEmpty(vars["dir"]))
                    {
                        downloader.SavePath += "\\" + vars["dir"];
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is WebException || ex is IOException)
                {
                    throw; //ex;
                }
                if (retrys-- > 0) goto retry;
                throw new Exception(string.Format("Script got error: [{0}] {1}", line, script));
            }
            finally
            {
                vars.Clear();
            }

            return true;
        }

        enum TokenType
        {
            Unkown = 0,
            Const,
            Variable,
            Function
        }

        static char[] WhiteChars = { ' ', '\t', ',' };

        static int Match(string s, int i, char start, char end)
        {
            int level = 0;
            while (i < s.Length)
            {
                char c = s[i];
                if (c == '"')
                {
                    i = ParseStr(s, i);
                }
                else if (c == start)
                {
                    level++;
                }
                else if (c == end)
                {
                    if (--level == 0)
                    {
                        break;
                    }
                }
                i++;
            }

            return i;
        }

        static int ParseStr(string s, int i)
        {
            do
            {
                i = s.IndexOf('\"', ++i);
                if (i < 0)
                {
                    throw new Exception();
                }
            }
            while (s[i - 1] == '\\');

            return i;
        }

        static string GetToken(ref string s, out TokenType type)
        {
            string result = null;
            int i = 0;

            type = TokenType.Unkown;
            s = s.Trim(WhiteChars);
            if (!string.IsNullOrEmpty(s))
            {
                if (s[0] == '\"')
                {
                    i = ParseStr(s, 0);
                    type = TokenType.Const;
                    result = s.Substring(1, i - 1);
                    s = s.Substring(i + 1);
                }
                else
                {
                    i = s.IndexOf(',');
                    int j = s.IndexOf('(');
                    if (i < 0 || (j >= 0 && j < i))
                    {
                        if (j < 0)
                        {
                            result = s;
                            s = null;
                        }
                        else
                        {
                            // find the match ')'
                            i = Match(s, j, '(', ')');
                            result = s.Remove(i);
                            s = s.Substring(i + 1);
                        }
                    }
                    else
                    {
                        result = s.Remove(i);
                        s = s.Substring(i + 1);
                    }
                    result = result.Trim().TrimEnd(';');
                    if (!string.IsNullOrEmpty(result))
                    {
                        type = result.Contains("(") ? TokenType.Function : (Char.IsDigit(result[0]) ? TokenType.Const : TokenType.Variable);
                    }
                }
            }

            return result;
        }

        static string Evaluate(ref string s, VarList vars)
        {
            TokenType type;
            //string ss = s;
            string token = GetToken(ref s, out type);

            if (!string.IsNullOrEmpty(token))
            {
                switch (type)
                {
                    case TokenType.Const:
                        return token.Replace("\\\"", "\"").Replace("\\\\", "\\").Replace("\\r", "\r").Replace("\\n", "\n").Replace("\\t", "\t");

                    case TokenType.Variable:
                        return vars[token];

                    case TokenType.Function:
                        return CallFunction(token, vars);
                }
            }

            //throw new Exception(string.Format("[Script] Get token fail: \"{0}\"", ss));
            return string.Empty;
        }

        private delegate string ParserFunc(string s, VarList vars);

        static SortedList<string, ParserFunc> funcs = new SortedList<string, ParserFunc>
        {
            {"decode",  _decode},
            {"encode",  _encode},
            {"format",  _format},
            {"replace", _replace},
            {"substr",  _substr},
            {"lsubstr", _lsubstr},
            {"prefer",  _prefer},
            {"get",     _get},
        };

        static string CallFunction(string s, VarList vars)
        {
            int i = s.IndexOf('(');
            string fname = s.Remove(i).Trim();
            s = s.Substring(i + 1);

            if (funcs.ContainsKey(fname))
            {
                return funcs[fname](s, vars);
            }
            throw new Exception("[Script] Unkown function: " + fname);
        }

        static string _get(string s, VarList vars)
		{
			string url = Evaluate(ref s, vars);
			return string.IsNullOrEmpty(s) ? Web.HttpGet(url) : Web.HttpGet(url, Encoding.GetEncoding(Evaluate(ref s, vars)));
		}

        static string _prefer(string s, VarList vars)
        {
            string ss = Evaluate(ref s, vars);
            string start, end;
            int i, j, n, max, max_n;
            bool not_http;

            // find the best index
            start = Evaluate(ref s, vars);
            end = Evaluate(ref s, vars);
            max = 0;
            max_n = 0;
            i = 0;
            j = 0;
            n = 0;
            while (ss.IndexOf(start, i, StringComparison.Ordinal) >= 0)
            {
                i = ss.IndexOf(start, i, StringComparison.Ordinal) + start.Length;
				j = ss.IndexOf(end, i, StringComparison.Ordinal);
                Int32.TryParse(ss.Substring(i, j - i), out j);
                if (j > max)
                {
                    max = j;
                    max_n = n;
                }
                n++;
            }
            vars["prefer"] = max.ToString();

            // pick the prefer item
            start = Evaluate(ref s, vars);
            end = Evaluate(ref s, vars);
            not_http = !start.StartsWith("http", StringComparison.Ordinal);
            i = 0;
            j = 0;
            n = 0;
            while (ss.IndexOf(start, i, StringComparison.Ordinal) >= 0)
            {
				i = ss.IndexOf(start, j, StringComparison.Ordinal);
                if (not_http)
                {
                    i += start.Length;
                }
				j = ss.IndexOf(end, i, StringComparison.Ordinal);
                if (n == max_n)
                {
                    break;
                }
                n++;
            }

            return ss.Substring(i, j - i);
        }

        static string _format(string s, VarList vars)
        {
            string format = Evaluate(ref s, vars);
            var args = new ArrayList();
            while (!string.IsNullOrEmpty(s))
            {
                args.Add(Evaluate(ref s, vars));
            }
            return string.Format(format, args.ToArray());
        }

        static string _substr(string s, VarList vars)
        {
            return __substr(s, vars, true);
        }

        static string _lsubstr(string s, VarList vars)
        {
            return __substr(s, vars, false);
        }

        static string __substr(string s, VarList vars, bool substr)
        {
            string ss = Evaluate(ref s, vars);
            string start = Evaluate(ref s, vars);
            int i;
            if (!Int32.TryParse(start, out i))
            {
                i = substr ? ss.IndexOf(start, StringComparison.Ordinal) : ss.LastIndexOf(start, StringComparison.Ordinal);
                if (i < 0)
                {
                    return null;
                }
                if (!start.StartsWith("http", StringComparison.Ordinal))
                {
                    i += start.Length;
                }
            }
            if (!string.IsNullOrEmpty(s))
            {
                int j;
                string end = Evaluate(ref s, vars);
                if (Int32.TryParse(end, out j))
                {
                    return ss.Substring(i, j);
                }
				j = ss.IndexOf(end, i, StringComparison.Ordinal);
                if (j >= 0)
                {
                    return ss.Substring(i, j - i);
                }
            }
            return ss.Substring(i);
        }

        static string _decode(string s, VarList vars)
        {
            string ss = Evaluate(ref s, vars);
			return !string.IsNullOrEmpty(s) ? HttpUtility.UrlDecode(ss, Encoding.GetEncoding(Evaluate(ref s, vars))) : Util.Decode(ss);
        }

        static string _encode(string s, VarList vars)
        {
            return HttpUtility.UrlEncode(Evaluate(ref s, vars));
        }

        static string _replace(string s, VarList vars)
        {
            return Evaluate(ref s, vars).Replace(Evaluate(ref s, vars), Evaluate(ref s, vars));
        }
    }
}