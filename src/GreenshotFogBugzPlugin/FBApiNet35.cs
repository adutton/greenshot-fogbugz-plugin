#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using FogBugzTestHarness;

#endregion

public class FBApiNet35
{
    public delegate void ApiEvent(object sender, string sent, string received);

    private const int c_minFogBugzVersion = 1;
    private readonly Uri m_fogBugzServer;
    private string m_email;
    private bool m_fromLogin;
    private string m_token;

    public FBApiNet35(Uri fogBugzServer)
    {
        m_fogBugzServer = fogBugzServer;
        m_fromLogin = false;
    }

    /// <summary>
    ///     Re-uses an existing token.  Assumes a login has already taken place
    /// </summary>
    public FBApiNet35(Uri fogBugzServer, string token)
    {
        m_fogBugzServer = fogBugzServer;
        m_fromLogin = true;
        m_token = token;
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

    private string ServerUrlApiCheck { get { return string.Concat(m_fogBugzServer.Scheme, "://", m_fogBugzServer.Host, "/api.xml"); } }

    private string ServerUrlCmd { get { return string.Concat(m_fogBugzServer.Scheme, "://", m_fogBugzServer.Host, "/api.asp"); } }

    #region Strongly Typed API

    #region source data

    #region SearchWritableCases

    public string Search(string q)
    {
        return Search(q, "ixBug,sEvent,sTitle,ixProject");
    }

    // CHANGE: XmlDocument instead of XmlNodeList
    public XmlDocument XSearch(string q)
    {
        return XSearch(q, "ixBug,sEvent,sTitle,ixProject");
    }

    public string Search(string q, string cols)
    {
        return Search(q, cols, 0);
    }

    // CHANGE: XmlDocument instead of XmlNodeList
    public XmlDocument XSearch(string q, string cols)
    {
        return XSearch(q, cols, 0);
    }

    public string Search(string q, string cols, int max)
    {
        var args = new Dictionary<string, string>();
        args.Add("q", q);
        args.Add("cols", cols);
        args.Add("max", (max <= 0 ? 10000 : max).ToString());
        return Cmd("search", args);
    }

    // CHANGE: XmlDocument instead of XmlNodeList
    public XmlDocument XSearch(string q, string cols, int max)
    {
        return DocFromXml(Search(q, cols, max));
        // CHANGE: Trimmed off .SelectNodes("/response/cases/case")
    }

    #endregion

    #region Case Meta Data (Projects, Areas, Categories, Priorities, Statuses)

    public string ListProjects()
    {
        return ListProjects(false);
    }

    public XmlNodeList XListProjects()
    {
        return XListProjects(false);
    }

    public string ListProjects(bool onlyWritable)
    {
        var args = new Dictionary<string, string>();
        if (onlyWritable) args.Add("fWrite", "1");
        return Cmd("listProjects", args);
    }

    public XmlNodeList XListProjects(bool onlyWritable)
    {
        return DocFromXml(ListProjects(onlyWritable)).SelectNodes("/response/projects/project");
    }

    public string ListCategories()
    {
        return Cmd("listCategories");
    }

    public XmlNodeList XListCategories()
    {
        return DocFromXml(ListCategories()).SelectNodes("/response/categories/category");
    }

    public string ListFixFors(int ixProject = -1)
    {
        var args = new Dictionary<string, string>();
        if (ixProject > -1) args.Add("ixProject", ixProject.ToString());
        return Cmd("listFixFors", args);
    }

    public XmlNodeList XListFixFors(int ixProject = -1)
    {
        return DocFromXml(ListFixFors(ixProject)).SelectNodes("/response/fixfors/fixfor");
    }

    public String ListAreas(bool onlyWritable = false, int ixProject = -1, int ixArea = -1)
    {
        var args = new Dictionary<String, String>();
        if (onlyWritable) args.Add("fWrite", "1");
        if (ixProject > -1) args.Add("ixProject", ixProject.ToString());
        if (ixArea > -1) args.Add("ixProject", ixArea.ToString());
        return Cmd("listAreas", args);
    }

    public XmlNodeList XListAreas(bool onlyWritable = false, int ixProject = -1, int ixArea = -1)
    {
        return DocFromXml(ListAreas(onlyWritable, ixProject, ixArea)).SelectNodes("/response/areas/area");
    }

    public String ListStatuses(int ixCategory = -1, Boolean fResovled = false)
    {
        var args = new Dictionary<String, String>();
        if (ixCategory > -1) args.Add("ixCategory", ixCategory.ToString());
        if (fResovled) args.Add("fResolved", "1ok ");
        return Cmd("listStatuses", args);
    }

    public XmlNodeList XListStatuses(int ixCategory = -1, Boolean fResovled = false)
    {
        return DocFromXml(ListStatuses(ixCategory, fResovled)).SelectNodes("/response/statuses/status");
    }

    public String ListPeople(Boolean fIncludeActive = true,
        Boolean fIncludeNormal = true,
        Boolean fIncludeDeleted = false,
        Boolean fIncludeCommunity = false,
        Boolean fIncludeVirtual = false)
    {
        var args = new Dictionary<String, String>();
        if (fIncludeActive) args.Add("fIncludeActive", "1");
        if (fIncludeNormal) args.Add("fIncludeNormal", "1");
        if (fIncludeDeleted) args.Add("fIncludeDeleted", "1");
        if (fIncludeCommunity) args.Add("fIncludeCommunity", "1");
        if (fIncludeVirtual) args.Add("fIncludeVirtual", "1");
        return Cmd("listPeople", args);
    }

    public XmlNodeList XListPeople(Boolean fIncludeActive = true,
        Boolean fIncludeNormal = true,
        Boolean fIncludeDeleted = false,
        Boolean fIncludeCommunity = false,
        Boolean fIncludeVirtual = false)
    {
        return
            DocFromXml(ListPeople(fIncludeActive, fIncludeNormal, fIncludeDeleted, fIncludeCommunity, fIncludeVirtual))
                .SelectNodes("/response/people/person");
    }
    #endregion

    #region Scheduling

    public string StartWork(int ixBug)
    {
        var args = new Dictionary<string, string>();
        args.Add("ixBug", ixBug.ToString());
        return Cmd("startWork", args);
    }

    public XmlDocument XStartWork(int ixBug)
    {
        return DocFromXml(StartWork(ixBug));
    }

    public string StopWork()
    {
        return Cmd("stopWork");
    }

    public XmlDocument XStopWork()
    {
        return DocFromXml(StopWork());
    }

    public string NewInterval(int ixBug, DateTime dtStart, DateTime dtEnd)
    {
        var args = new Dictionary<string, string>();
        args.Add("ixBug", ixBug.ToString());
        args.Add("dtStart", dtStart.ToUniversalTime().ToString("s") + "Z");
        args.Add("dtEnd", dtEnd.ToUniversalTime().ToString("s") + "Z");
        return Cmd("newInterval", args);
    }

    public XmlDocument XNewInterval(int ixBug, DateTime dtStart, DateTime dtEnd)
    {
        return DocFromXml(NewInterval(ixBug, dtStart, dtEnd));
    }

    public string ListIntervals(DateTime dtStart, DateTime dtEnd)
    {
        var args = new Dictionary<string, string>();
        args.Add("dtStart", dtStart.ToUniversalTime().ToString("s") + "Z");
        args.Add("dtEnd", dtEnd.ToUniversalTime().ToString("s") + "Z");
        return Cmd("listIntervals", args);
    }

    public XmlNodeList XListIntervals(DateTime dtStart, DateTime dtEnd)
    {
        return DocFromXml(ListIntervals(dtStart, dtEnd)).GetElementsByTagName("interval");
    }

    public XmlNodeList XListIntervals(DateTime dtStart, DateTime dtEnd, int ixBug)
    {
        return DocFromXml(ListIntervals(dtStart, dtEnd)).SelectNodes(string.Concat("/response/intervals/interval[ixBug='", ixBug, "']"));
    }

    #endregion

    #region Subscriptions

    public string Subscribe(int ixBug, int ixWikiPage)
    {
        var args = new Dictionary<string, string>();
        args.Add("ixBug", ixBug.ToString());
        args.Add("ixWikiPage", ixWikiPage.ToString());
        return Cmd("subscribe", args);
    }

    public XmlDocument XSubscribe(int ixBug, int ixWikiPage)
    {
        return DocFromXml(Subscribe(ixBug, ixWikiPage));
    }

    public string Unsubscribe(int ixBug, int ixWikiPage)
    {
        var args = new Dictionary<string, string>();
        args.Add("ixBug", ixBug.ToString());
        args.Add("ixWikiPage", ixWikiPage.ToString());
        return Cmd("unsubscribe", args);
    }

    public XmlDocument XUnsubscribe(int ixBug, int ixWikiPage)
    {
        return DocFromXml(Unsubscribe(ixBug, ixWikiPage));
    }

    public string View(int ixBug)
    {
        var args = new Dictionary<string, string>();
        args.Add("ixBug", ixBug.ToString());
        return Cmd("view", args);
    }

    public XmlDocument XView(int ixBug)
    {
        return DocFromXml(View(ixBug));
    }

    #endregion

    #endregion

    // These are strongly typed methods that wrap specific commands (mostly taken from the APITesting projects
    // provided on the FogBugz blog.

    #endregion

    public LoginResult CheckApi()
    {
        // First check that the server responds to the API call
        // https://example.fogbugz.com/api.xml
        string apiResponse;

        try
        {
            var http = (HttpWebRequest) WebRequest.Create(ServerUrlApiCheck);
            using (var response = http.GetResponse())
            using (var r = http.GetResponse().GetResponseStream())
            using (var reader = new StreamReader(r))
            {
                apiResponse = reader.ReadToEnd();
                reader.Close();
            }
        }
        catch (WebException ex)
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
            var versionRegex = new Regex("<minversion>(?<apiVersion>\\d+)</minversion>");
            var match = versionRegex.Match(apiResponse);

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
            var result = CheckApi();
            if (result != LoginResult.Ok)
                return result;
        }

        // Now that we know it's a FogBugz instance, try to login
        m_email = email;
        m_fromLogin = false;
        try
        {
            var args = new Dictionary<string, string>();
            args.Add("email", m_email);
            args.Add("password", password);
            var doc = XCmd("logon", args);
            var tokens = doc.GetElementsByTagName("token");
            if (tokens.Count != 1)
                // Error, user/password combo didn't work
                return LoginResult.AccountNotFound;
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
            Cmd("logoff");

        m_token = null;
        m_fromLogin = false;
    }

    public string Cmd(string cmd)
    {
        return Cmd(cmd, new Dictionary<string, string>());
    }

    public XmlDocument XCmd(string cmd)
    {
        return XCmd(cmd, new Dictionary<string, string>());
    }

    public XmlNodeList XCmd(string cmd, string xPath)
    {
        return XCmd(cmd, new Dictionary<string, string>(), xPath);
    }

    public string Cmd(string cmd, Dictionary<string, string> args)
    {
        return Cmd(cmd, args, null);
    }

    public XmlDocument XCmd(string cmd, Dictionary<string, string> args)
    {
        return XCmd(cmd, args, new FogBugzFile[0]);
    }

    public XmlNodeList XCmd(string cmd, Dictionary<string, string> args, string xPath)
    {
        return XCmd(cmd, args, null, xPath);
    }

    public XmlDocument XCmd(string cmd, Dictionary<string, string> args, FogBugzFile[] files)
    {
        return DocFromXml(Cmd(cmd, args, files));
    }

    public XmlNodeList XCmd(string cmd, Dictionary<string, string> args, FogBugzFile[] files, string xPath)
    {
        return XCmd(cmd, args, files).SelectNodes(xPath);
    }

    public string Cmd(string cmd, Dictionary<string, string> args, FogBugzFile[] files)
    {
        if (args == null)
            args = new Dictionary<string, string>();
        args.Add("cmd", cmd);
        if (!String.IsNullOrEmpty(m_token))
            args.Add("token", m_token);
        return CallRESTAPIFiles(ServerUrlCmd, args, files);
    }

    public event ApiEvent ApiCalled;

    // <summary>
    // CallRestAPIFiles submits an API request to the FogBugz api using the 
    // multipart/form-data submission method (so you can add files)
    // Don't forget to include nFileCount in your rgArgs collection if you are adding files.
    // </summary>
    private string CallRESTAPIFiles(string sURL, Dictionary<string, string> rgArgs, FogBugzFile[] rgFiles)
    {
        var sBoundaryString = getRandomString(30);
        var sBoundary = "--" + sBoundaryString;
        var encoding = new ASCIIEncoding();
        var utf8encoding = new UTF8Encoding();
        var http = (HttpWebRequest) WebRequest.Create(sURL);
        http.Method = "POST";
        http.AllowWriteStreamBuffering = true;
        http.ContentType = "multipart/form-data; boundary=" + sBoundaryString;
        var vbCrLf = "\r\n";

        var parts = new Queue();

        if (rgArgs == null)
            rgArgs = new Dictionary<string, string>();

        if (rgFiles != null && rgFiles.Length > 0 && !rgArgs.ContainsKey("fileCount"))
            rgArgs["fileCount"] = rgFiles.Length.ToString();

        // add all the normal arguments
        foreach (KeyValuePair<string, string> i in rgArgs)
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
            for (var i = 0; i < rgFiles.Length; i++)
            {
                parts.Enqueue(encoding.GetBytes(sBoundary + vbCrLf));
                parts.Enqueue(encoding.GetBytes("Content-Disposition: form-data; name=\""));
                parts.Enqueue(encoding.GetBytes(string.Concat("File", i+1)));
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
        var nContentLength = 0;
        foreach (Byte[] part in parts)
        {
            nContentLength += part.Length;
        }
        http.ContentLength = nContentLength;

        // write the post
        var sent = new StringBuilder(nContentLength);
        using (var stream = http.GetRequestStream())
        {
            foreach (Byte[] part in parts)
            {
                stream.Write(part, 0, part.Length);
                sent.Append(encoding.GetString(part));
            }
            stream.Close();
        }

        // read the success
        using (var r = http.GetResponse().GetResponseStream())
        {
            var reader = new StreamReader(r);
            var retValue = reader.ReadToEnd();
            reader.Close();
            if (ApiCalled != null)
                ApiCalled(this, sent.ToString(), retValue);
            return retValue;
        }
    }

    private static XmlDocument DocFromXml(string result)
    {
        var doc = new XmlDocument();
        doc.LoadXml(result);
        return doc;
    }

    private string getRandomString(int nLength)
    {
        var chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXTZabcdefghiklmnopqrstuvwxyz";
        var s = "";
        var rand = new Random();
        for (var i = 0; i < nLength; i++)
        {
            var rnum = (int) Math.Floor((double) rand.Next(0, chars.Length - 1));
            s += chars.Substring(rnum, 1);
        }
        return s;
    }
}

public enum LoginResult
{
    Unknown,
    Ok,
    ServerNotFound,
    ServerNotCapable,
    AccountNotFound
}