using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Collections;
using FogBugzTestHarness;

public class FBApiNet35
{
    public FBApiNet35(Uri fogBugzServer)
    {
        m_fogBugzServer = fogBugzServer;
        m_fromLogin = false;
    }

    /// <summary>
    /// Re-uses an existing token.  Assumes a login has already taken place
    /// </summary>
    public FBApiNet35(Uri fogBugzServer, string token)
    {
        m_fogBugzServer = fogBugzServer;
        m_fromLogin = true;
        m_token = token;
    }

    public LoginResult CheckApi()
    {
        // First check that the server responds to the API call
        // https://example.fogbugz.com/api.xml
        string apiResponse;

        try
        {
            HttpWebRequest http = (HttpWebRequest)HttpWebRequest.Create(ServerUrlApiCheck);
            using (WebResponse response = http.GetResponse())
            using (Stream r = http.GetResponse().GetResponseStream())
            using (StreamReader reader = new StreamReader(r))
            {
                apiResponse = reader.ReadToEnd();
                reader.Close();
            }
        }
        catch (System.Net.WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError)
                return LoginResult.ServerNotCapable;
            return LoginResult.ServerNotFound;
        }
        catch (Exception)
        {
            return LoginResult.ServerNotFound;
        }

        try
        {
            Regex versionRegex = new Regex("<minversion>(?<apiVersion>\\d+)</minversion>");
            Match match = versionRegex.Match(apiResponse);

            if (Int32.Parse(match.Groups["apiVersion"].Value) < c_minFogBugzVersion)
                return LoginResult.ServerNotCapable;
        }
        catch
        {
            return LoginResult.ServerNotCapable;
        }

        return LoginResult.Ok;
    }

    public LoginResult Login(string email, string password)
    {
        return Login(email, password, false);
    }

    public LoginResult Login(string email, string password, bool skipApiCheck)
    {
        if (!skipApiCheck)
        {
            LoginResult result = CheckApi();
            if (result != LoginResult.Ok)
                return result;
        }            

        // Now that we know it's a FogBugz instance, try to login
        m_email = email;
        m_fromLogin = false;
        try
        {
            Dictionary<string, string> args = new Dictionary<string, string>();
            args.Add("email", m_email);
            args.Add("password", password);
            XmlDocument doc = this.XCmd("logon", args);
            XmlNodeList tokens = doc.GetElementsByTagName("token");
            if (tokens.Count != 1)
                // Error, user/password combo didn't work
                return LoginResult.AccountNotFound;
            else
                m_token = tokens[0].InnerText;
            m_fromLogin = true;
        }
        catch
        {
            return LoginResult.Unknown;
        }
        return LoginResult.Ok;
    }

    public void Logout()
    {
        if (m_token != null)
            this.Cmd("logoff");

        m_token = null;
        m_fromLogin = false;
    }

    public string Cmd(string cmd)
    {
        return this.Cmd(cmd, new Dictionary<string, string>());
    }

    public XmlDocument XCmd(string cmd)
    {
        return this.XCmd(cmd, new Dictionary<string, string>());
    }

    public XmlNodeList XCmd(string cmd, string xPath)
    {
        return this.XCmd(cmd, new Dictionary<string, string>(), xPath);
    }

    public string Cmd(string cmd, Dictionary<string, string> args)
    {
        return this.Cmd(cmd, args, null);
    }

    public XmlDocument XCmd(string cmd, Dictionary<string, string> args)
    {
        return this.XCmd(cmd, args, new FogBugzFile[0]);
    }

    public XmlNodeList XCmd(string cmd, Dictionary<string, string> args, string xPath)
    {
        return this.XCmd(cmd, args, null, xPath);
    }

    public XmlDocument XCmd(string cmd, Dictionary<string, string> args, FogBugzFile[] files)
    {
        return DocFromXml(this.Cmd(cmd, args, files));
    }

    public XmlNodeList XCmd(string cmd, Dictionary<string, string> args, FogBugzFile[] files, string xPath)
    {
        return this.XCmd(cmd, args, files).SelectNodes(xPath);
    }

    public string Cmd(string cmd, Dictionary<string, string> args, FogBugzFile[] files)
    {
        if (args == null) 
            args = new Dictionary<string, string>();
        args.Add("cmd", cmd);
        if (!String.IsNullOrEmpty(m_token)) 
            args.Add("token", m_token);
        return this.CallRESTAPIFiles(ServerUrlCmd, args, files);
    }

    public delegate void ApiEvent(object sender, string sent, string received);
    public event ApiEvent ApiCalled;

    // <summary>
    // CallRestAPIFiles submits an API request to the FogBugz api using the 
    // multipart/form-data submission method (so you can add files)
    // Don't forget to include nFileCount in your rgArgs collection if you are adding files.
    // </summary>
    private string CallRESTAPIFiles(string sURL, Dictionary<string, string> rgArgs, FogBugzFile[] rgFiles)
    {
        string sBoundaryString = getRandomString(30);
        string sBoundary = "--" + sBoundaryString;
        ASCIIEncoding encoding = new ASCIIEncoding();
        UTF8Encoding utf8encoding = new UTF8Encoding();
        HttpWebRequest http = (HttpWebRequest)HttpWebRequest.Create(sURL);
        http.Method = "POST";
        http.AllowWriteStreamBuffering = true;
        http.ContentType = "multipart/form-data; boundary=" + sBoundaryString;
        string vbCrLf = "\r\n";

        Queue parts = new Queue();

        if (rgArgs == null)
            rgArgs = new Dictionary<string, string>();

        if (rgFiles != null && rgFiles.Length > 0 && !rgArgs.ContainsKey("fileCount"))
            rgArgs["fileCount"] = rgFiles.Length.ToString();

        // add all the normal arguments
        foreach (System.Collections.Generic.KeyValuePair<string, string> i in rgArgs)
        {
            parts.Enqueue(encoding.GetBytes(sBoundary + vbCrLf));
            parts.Enqueue(encoding.GetBytes("Content-Type: text/plain; charset=\"utf-8\"" + vbCrLf));
            parts.Enqueue(encoding.GetBytes("Content-Disposition: form-data; name=\"" + i.Key + "\"" + vbCrLf + vbCrLf));
            parts.Enqueue(utf8encoding.GetBytes(i.Value));
            parts.Enqueue(encoding.GetBytes(vbCrLf));
        }

        // add all the files
        if (rgFiles != null)
        {
            for (int i = 0; i < rgFiles.Length; i++)
            {
                parts.Enqueue(encoding.GetBytes(sBoundary + vbCrLf));
                parts.Enqueue(encoding.GetBytes("Content-Disposition: form-data; name=\""));
                parts.Enqueue(encoding.GetBytes(string.Concat("File", i)));
                parts.Enqueue(encoding.GetBytes("\"; filename=\""));
                parts.Enqueue(encoding.GetBytes(rgFiles[i].Filename));
                parts.Enqueue(encoding.GetBytes("\"" + vbCrLf));
                parts.Enqueue(encoding.GetBytes("Content-Transfer-Encoding: base64" + vbCrLf));
                parts.Enqueue(encoding.GetBytes("Content-Type: "));
                parts.Enqueue(encoding.GetBytes(rgFiles[i].ContentType));
                parts.Enqueue(encoding.GetBytes(vbCrLf + vbCrLf));
                parts.Enqueue(rgFiles[i].Data);
                parts.Enqueue(encoding.GetBytes(vbCrLf));
            }
        }

        parts.Enqueue(encoding.GetBytes(sBoundary + "--"));

        // calculate the content length
        int nContentLength = 0;
        foreach (Byte[] part in parts)
        {
            nContentLength += part.Length;
        }
        http.ContentLength = nContentLength;

        // write the post
        StringBuilder sent = new StringBuilder(nContentLength);
        using (Stream stream = http.GetRequestStream())
        {
            foreach (Byte[] part in parts)
            {
                stream.Write(part, 0, part.Length);
                sent.Append(encoding.GetString(part));
            }
            stream.Close();
        }

        // read the success
        using (Stream r = http.GetResponse().GetResponseStream())
        {
            StreamReader reader = new StreamReader(r);
            string retValue = reader.ReadToEnd();
            reader.Close();
            if (this.ApiCalled != null)
                this.ApiCalled(this, sent.ToString(), retValue);
            return retValue;
        }
    }

    private static XmlDocument DocFromXml(string result)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(result);
        return doc;
    }
 
    private string getRandomString(int nLength)
    {
        string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXTZabcdefghiklmnopqrstuvwxyz";
        string s = "";
        System.Random rand = new System.Random();
        for (int i = 0; i < nLength; i++)
        {
            int rnum = (int)Math.Floor((double)rand.Next(0, chars.Length - 1));
            s += chars.Substring(rnum, 1);
        }
        return s;
    }
    
    public string Token
    {
        get
        {
            if (!m_fromLogin && String.IsNullOrEmpty(m_token)) 
                throw new Exception("Not logged in...");
            return m_token;
        }
    }

    private string m_token;
    private string m_email;
    private bool m_fromLogin;
    private Uri m_fogBugzServer;

    private const int c_minFogBugzVersion = 1;

    private string ServerUrlApiCheck
    {
        get
        {
            return string.Concat(m_fogBugzServer.Scheme, "://", m_fogBugzServer.Host, "/api.xml");
        }
    }

    private string ServerUrlCmd
    {
        get
        {
            return string.Concat(m_fogBugzServer.Scheme, "://", m_fogBugzServer.Host, "/api.asp");
        }
    }

    #region Strongly Typed API
    // These are strongly typed methods that wrap specific commands (mostly taken from the APITesting projects
    // provided on the FogBugz blog.

    #region source data

    #region SearchWritableCases

    public string Search(string q)
    {
        return this.Search(q, "ixBug,sEvent,sTitle,ixProject");
    }

    // CHANGE: XmlDocument instead of XmlNodeList
    public XmlDocument XSearch(string q)
    {
        return this.XSearch(q, "ixBug,sEvent,sTitle,ixProject");
    }

    public string Search(string q, string cols)
    {
        return this.Search(q, cols, 0);
    }

    // CHANGE: XmlDocument instead of XmlNodeList
    public XmlDocument XSearch(string q, string cols)
    {
        return this.XSearch(q, cols, 0);
    }

    public string Search(string q, string cols, int max)
    {
        Dictionary<string, string> args = new Dictionary<string, string>();
        args.Add("q", q);
        args.Add("cols", cols);
        args.Add("max", (max <= 0 ? 10000 : max).ToString());
        return this.Cmd("search", args);
    }

    // CHANGE: XmlDocument instead of XmlNodeList
    public XmlDocument XSearch(string q, string cols, int max)
    {
        return DocFromXml(this.Search(q, cols, max));
        // CHANGE: Trimmed off .SelectNodes("/response/cases/case")
    }


    #endregion

    #region Editing Cases
    public string ListProjects()
    {
        return this.ListProjects(false);
    }
    public XmlNodeList XListProjects()
    {
        return XListProjects(false);
    }

    public string ListProjects(bool onlyWritable)
    {
        Dictionary<string, string> args = new Dictionary<string, string>();
        if (onlyWritable) args.Add("fWrite", "1");
        return this.Cmd("listProjects", args);
    }
    public XmlNodeList XListProjects(bool onlyWritable)
    {
        return DocFromXml(this.ListProjects(onlyWritable)).SelectNodes("/response/projects/project");
    }

    public string ListCategories()
    {
        return this.Cmd("listCategories");
    }
    public XmlNodeList XListCategories()
    {
        return DocFromXml(this.ListCategories()).SelectNodes("/response/categories/category");
    }

    public string ListFixFors(int ixProject)
    {
        Dictionary<string, string> args = new Dictionary<string, string>();
        args.Add("ixProject", ixProject.ToString());
        return this.Cmd("listFixFors", args);
    }

    public XmlNodeList XListFixFors(int ixProject)
    {
        return DocFromXml(this.ListFixFors(ixProject)).SelectNodes("/response/fixfors/fixfor");
    }

    #endregion

    #region Scheduling
    public string StartWork(int ixBug)
    {
        Dictionary<string, string> args = new Dictionary<string, string>();
        args.Add("ixBug", ixBug.ToString());
        return this.Cmd("startWork", args);
    }

    public XmlDocument XStartWork(int ixBug)
    {
        return DocFromXml(this.StartWork(ixBug));
    }

    public string StopWork()
    {
        return this.Cmd("stopWork");
    }

    public XmlDocument XStopWork()
    {
        return DocFromXml(this.StopWork());
    }

    public string NewInterval(int ixBug, DateTime dtStart, DateTime dtEnd)
    {
        Dictionary<string, string> args = new Dictionary<string, string>();
        args.Add("ixBug", ixBug.ToString());
        args.Add("dtStart", dtStart.ToUniversalTime().ToString("s") + "Z");
        args.Add("dtEnd", dtEnd.ToUniversalTime().ToString("s") + "Z");
        return this.Cmd("newInterval", args);
    }
    public XmlDocument XNewInterval(int ixBug, DateTime dtStart, DateTime dtEnd)
    {
        return DocFromXml(this.NewInterval(ixBug, dtStart, dtEnd));
    }

    public string ListIntervals(DateTime dtStart, DateTime dtEnd)
    {
        Dictionary<string, string> args = new Dictionary<string, string>();
        args.Add("dtStart", dtStart.ToUniversalTime().ToString("s") + "Z");
        args.Add("dtEnd", dtEnd.ToUniversalTime().ToString("s") + "Z");
        return this.Cmd("listIntervals", args);
    }
    public XmlNodeList XListIntervals(DateTime dtStart, DateTime dtEnd)
    {
        return DocFromXml(this.ListIntervals(dtStart, dtEnd)).GetElementsByTagName("interval");
    }
    public XmlNodeList XListIntervals(DateTime dtStart, DateTime dtEnd, int ixBug)
    {
        return DocFromXml(this.ListIntervals(dtStart, dtEnd)).SelectNodes(string.Concat("/response/intervals/interval[ixBug='", ixBug, "']"));
    }
 
    #endregion

    #region Subscriptions
    public string Subscribe(int ixBug, int ixWikiPage)
    {
        Dictionary<string, string> args = new Dictionary<string, string>();
        args.Add("ixBug", ixBug.ToString());
        args.Add("ixWikiPage", ixWikiPage.ToString());
        return this.Cmd("subscribe", args);
    }
    public XmlDocument XSubscribe(int ixBug, int ixWikiPage)
    {
        return DocFromXml(this.Subscribe(ixBug, ixWikiPage));
    }

    public string Unsubscribe(int ixBug, int ixWikiPage)
    {
        Dictionary<string, string> args = new Dictionary<string, string>();
        args.Add("ixBug", ixBug.ToString());
        args.Add("ixWikiPage", ixWikiPage.ToString());
        return this.Cmd("unsubscribe", args);
    }

    public XmlDocument XUnsubscribe(int ixBug, int ixWikiPage)
    {
        return DocFromXml(this.Unsubscribe(ixBug, ixWikiPage));
    }

    public string View(int ixBug)
    {
        Dictionary<string, string> args = new Dictionary<string, string>();
        args.Add("ixBug", ixBug.ToString());
        return this.Cmd("view", args);
    }

    public XmlDocument XView(int ixBug)
    {
        return DocFromXml(this.View(ixBug));
    }

    #endregion

    #endregion

    #endregion
}

public enum LoginResult
{
    Unknown,
    Ok,
    ServerNotFound,
    ServerNotCapable,
    AccountNotFound
}