#region

using System;
using System.Collections.Generic;
using System.Xml;
using FogBugzTestHarness;

#endregion

/// <summary>
///     Interacts with the FogBugz XML API, wrapping the C# FBApi
///     http://fogbugz.stackexchange.com/fogbugz-xml-api
/// </summary>
public class FogBugz
{
    private readonly FBApiNet35 m_fb;
    private readonly Uri m_server;
    private string m_token;

    public FogBugz(Uri server, string token)
    {
        m_server = server;
        m_token = token;
        m_fb = new FBApiNet35(m_server);
    }

    public string Token { get { return m_token; } }

    public LoginResult Login(string email, string password)
    {
        var result = m_fb.Login(email, password);

        if (result == LoginResult.Ok)
            m_token = m_fb.Token;

        return result;
    }

    public SearchResults SearchWritableCases(string query, int maxCaseCount)
    {
        if (m_fb == null)
            throw new InvalidOperationException("Not connected");

        if (string.IsNullOrEmpty(query) || query.Trim().Length == 0)
            return new SearchResults
            {
                Results = new List<SearchResult>(),
                TotalResults = 0
            };

        /*    string[] fakeResults = new string[] {
                "12345 - Blah Blah Blah - Aaron Dutton",
                "23245 - I love testing - Aaron Dutton",
                "1445 - Another test case - Chad Rockwell",
                "6765 - Fix stuff now - Aaron Dutton",
                "12 - Production is down - Homero Barbosa",
                "38433 - Super interesting result - Chad Rockwell",
                "14345 - Blah Blah Blah - Aaron Dutton",
                "23345 - I love testing - Aaron Dutton",
                "1225 - Another test case - Chad Rockwell",
                "2265 - Fix stuff now - Aaron Dutton",
                "122 - Production is down - Homero Barbosa",
                "82433 - Super interesting result - Chad Rockwell",
                "78945 - Blah Blah Blah - Aaron Dutton",
                "89245 - I love testing - Aaron Dutton",
                "9845 - Another test case - Chad Rockwell",
                "8765 - Fix stuff now - Aaron Dutton",
                "77 - Production is down - Homero Barbosa",
                "78433 - Super interesting result - Chad Rockwell",
                "17845 - Blah Blah Blah - Aaron Dutton",
                "98245 - I love testing - Aaron Dutton",
                "9845 - Another test case - Chad Rockwell",
                "9865 - Fix stuff now - Aaron Dutton",
                "19 - Production is down - Homero Barbosa",
                "89833 - Super interesting result - Chad Rockwell",
            };
            return fakeResults.Where(s => s.ToLowerInvariant().Contains(query.ToLowerInvariant())).ToArray()
        */


        var fb = new FBApiNet35(m_server, m_token);

        var finalQuery = "status:open " + query;
        var caseId = 0;
        if (Int32.TryParse(query, out caseId) && caseId > 0)
            finalQuery = string.Concat("status:open ixBug:", caseId);

        var results = new SearchResults();
        results.Results = new List<SearchResult>();
        var resultDocument = fb.XSearch(finalQuery, "ixBug,sTitle", maxCaseCount);

        var casesNode = resultDocument.SelectSingleNode("/response/cases");
        results.TotalResults = Int32.Parse(casesNode.Attributes["count"].Value);

        var xmlResults = resultDocument.SelectNodes("/response/cases/case");

        foreach (XmlNode node in xmlResults)
        {
            results.Results.Add(new SearchResult
            {
                CaseId = Int32.Parse(node["ixBug"].InnerText),
                Title = node["sTitle"].InnerText
            });
        }

        return results;
    }

    public int CreateNewCase(string caption, string filename, byte[] imageData)
    {
        var fb = new FBApiNet35(m_server, m_token);

        var cmds = new Dictionary<string, string>
        {
            {"sTitle", caption},
            {"sEvent", caption},
            {"cols", "ixBug"}
        };
        var output = fb.XCmd("new", cmds, EncodeSingleFileForFogBugz(filename, imageData));

        var caseNumber = output.InnerText;

        return Int32.Parse(caseNumber);
    }

    public void AttachImageToExistingCase(int caseId, string caption, string filename, byte[] imageData)
    {
        var fb = new FBApiNet35(m_server, m_token);

        var cmds = new Dictionary<string, string>();
        cmds.Add("ixBug", caseId.ToString());
        cmds.Add("sEvent", caption);
        cmds.Add("cols", "ixBug");
        fb.Cmd("edit", cmds, EncodeSingleFileForFogBugz(filename, imageData));
    }

    private FogBugzFile[] EncodeSingleFileForFogBugz(string filename, byte[] imageData)
    {
        return new[]
        {
            new FogBugzFile
            {
                Filename = filename,
                ContentType = "image/jpeg",
                Data = imageData
            }
        };
    }
}

public class SearchResult
{
    public int CaseId;
    public string Title;
}

public class SearchResults
{
    public List<SearchResult> Results;
    public int TotalResults;
}