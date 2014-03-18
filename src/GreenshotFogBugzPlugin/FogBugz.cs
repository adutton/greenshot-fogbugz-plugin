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

    public List<Project> ListProjects(Boolean onlyWritable = true)
    {
        var fb = new FBApiNet35(m_server, m_token);

        var projects = new List<Project>();

        var projectNodes = fb.XListProjects(onlyWritable);

        foreach (XmlNode projectNode in projectNodes)
        {
            projects.Add(new Project
            {
                ProjectID = Convert.ToInt32(projectNode["ixProject"].InnerText),
                ProjectName = projectNode["sProject"].InnerText,
                OwnerID = Convert.ToInt32(projectNode["ixPersonOwner"].InnerText),
                OwnerName = projectNode["sPersonOwner"].InnerText,
                OwnerEmail = projectNode["sEmail"].InnerText,
                OwnerPhone = projectNode["sPhone"].InnerText,
                Inbox = Convert.ToBoolean(projectNode["fInbox"].InnerText),
                WorkflowID = Convert.ToInt32(projectNode["ixWorkflow"].InnerText),
                Deleted = Convert.ToBoolean(projectNode["fDeleted"].InnerText)
            });
        }

        return projects;
    }

    public List<Area> ListAreas(Boolean onlyWritable = true, int projectID = -1, int areaID = -1)
    {
        var fb = new FBApiNet35(m_server, m_token);

        var areas = new List<Area>();

        var areaNodes = fb.XListAreas(onlyWritable, projectID, areaID);

        foreach (XmlNode areaNode in areaNodes)
        {
            areas.Add(new Area
            {
                AreaID = Convert.ToInt32(areaNode["ixArea"].InnerText),
                AreaName = areaNode["sArea"].InnerText,
                ProjectID = Convert.ToInt32(areaNode["ixProject"].InnerText),
                ProjectName = areaNode["sProject"].InnerText,
                OwnerID = String.IsNullOrEmpty(areaNode["ixPersonOwner"].InnerText) ? -1 : Convert.ToInt32(areaNode["ixPersonOwner"].InnerText),
                OwnerName = areaNode["sPersonOwner"].InnerText,
                Type = (AreaType) Convert.ToInt32(areaNode["nType"].InnerText),
                DocumentsTrained = Convert.ToInt32(areaNode["cDoc"].InnerText),
                Deleted = false
            });
        }

        return areas;
    }

    public List<Category> ListCategories()
    {
        var fb = new FBApiNet35(m_server, m_token);

        var categories = new List<Category>();

        var categoriesNodes = fb.XListCategories();

        foreach (XmlNode categoryNode in categoriesNodes)
        {
            categories.Add(new Category
            {
                CategoryID = Convert.ToInt32(categoryNode["ixCategory"].InnerText),
                CategoryName = categoryNode["sCategory"].InnerText,
                CategoryNamePlural = categoryNode["sPlural"].InnerText,
                DefaultResolvedStatusID = Convert.ToInt32(categoryNode["ixStatusDefault"].InnerText),
                DefaultActiveStatusID = Convert.ToInt32(categoryNode["ixStatusDefaultActive"].InnerText),
                IsScheduleItem = Convert.ToBoolean(categoryNode["fIsScheduleItem"].InnerText)
            });
        }

        return categories;
    }

    public List<Status> ListStatuses(Int32 categoryID = -1, Boolean onlyResovled = false)
    {
        var fb = new FBApiNet35(m_server, m_token);

        var statuses = new List<Status>();

        var statusNodes = fb.XListStatuses(categoryID, onlyResovled);

        foreach (XmlNode statusNode in statusNodes)
        {
            statuses.Add(new Status
            {
                StatusID = Convert.ToInt32(statusNode["ixStatus"].InnerText),
                StatusName = statusNode["sStatus"].InnerText,
                CategoryID = Convert.ToInt32(statusNode["ixCategory"].InnerText),
                WorkDone = Convert.ToBoolean(statusNode["fWorkDone"].InnerText),
                Resolved = Convert.ToBoolean(statusNode["fResolved"].InnerText),
                Duplicate = Convert.ToBoolean(statusNode["fDuplicate"].InnerText),
                Deleted = Convert.ToBoolean(statusNode["fDeleted"].InnerText),
                Order = Convert.ToInt32(statusNode["iOrder"].InnerText)
            });
        }

        return statuses;
    }

    public List<Person> ListPeople(Boolean includeActive = true,
        Boolean includeNormal = true,
        Boolean includeDeleted = false,
        Boolean includeCommunity = false,
        Boolean includeVirtual = false)
    {
        var fb = new FBApiNet35(m_server, m_token);

        var people = new List<Person>();

        var peopleNodes = fb.XListPeople(includeActive, includeNormal, includeDeleted, includeCommunity, includeVirtual);

        foreach (XmlNode personNode in peopleNodes)
        {
            people.Add(new Person
            {
                PersonID = Convert.ToInt32(personNode["ixPerson"].InnerText),
                FullName = personNode["sFullName"].InnerText,
                Email = personNode["sEmail"].InnerText,
                Phone = personNode["sPhone"].InnerText,
                Administrator = Convert.ToBoolean(personNode["fAdministrator"].InnerText),
                Community = Convert.ToBoolean(personNode["fCommunity"].InnerText),
                Virtual = Convert.ToBoolean(personNode["fVirtual"].InnerText),
                Deleted = Convert.ToBoolean(personNode["fDeleted"].InnerText),
                Notify = Convert.ToBoolean(personNode["fNotify"].InnerText),
                Homepage = personNode["sHomepage"].InnerText
            });
        }

        return people;
    }
}

public class SearchResult
{
    public int CaseId { get; set; }
    public string Title { get; set; }
}

public class SearchResults
{
    public List<SearchResult> Results { get; set; }
    public int TotalResults { get; set; }
}

public class Project
{
    public Boolean Deleted { get; set; }
    public int ProjectID { get; set; }
    public Boolean Inbox { get; set; }
    public String ProjectName { get; set; }
    public String OwnerEmail { get; set; }
    public int OwnerID { get; set; }
    public String OwnerName { get; set; }
    public String OwnerPhone { get; set; }
    public int WorkflowID { get; set; }
}

public enum AreaType
{
    Normal = 0,
    NotSpam = 1,
    Undecided = 2,
    Spam = 3
}

public class Area
{
    public int AreaID { get; set; }
    public String AreaName { get; set; }
    public int OwnerID { get; set; }
    public String OwnerName { get; set; }
    public int ProjectID { get; set; }
    public string ProjectName { get; set; }
    public AreaType Type { get; set; }
    public Boolean Deleted { get; set; }
    public int DocumentsTrained { get; set; }
}

public class Category
{
    public int CategoryID { get; set; }
    public String CategoryName { get; set; }
    public String CategoryNamePlural { get; set; }
    public Int32 DefaultResolvedStatusID { get; set; }
    public Boolean IsScheduleItem { get; set; }
    public Boolean IsDeleted { get; set; }
    public Int32 Order { get; set; }
    public Int32 DefaultActiveStatusID { get; set; }
}

public class Status
{
    public Int32 StatusID { get; set; }
    public String StatusName { get; set; }
    public Int32 CategoryID { get; set; }
    public Boolean WorkDone { get; set; }
    public Boolean Resolved { get; set; }
    public Boolean Duplicate { get; set; }
    public Boolean Deleted { get; set; }
    public Int32 Order { get; set; }
}

public class Person
{
    public Int32 PersonID { get; set; }
    public String FullName { get; set; }
    public String Email { get; set; }
    public String Phone { get; set; }
    public Boolean Administrator { get; set; }
    public Boolean Community { get; set; }
    public Boolean Virtual { get; set; }
    public Boolean Deleted { get; set; }
    public Boolean Notify { get; set; }
    public String Homepage { get; set; }
}