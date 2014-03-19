#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

    public int CreateNewCase(string title,
        string description,
        string filename,
        byte[] imageData,
        Int32 projectID = -1,
        Int32 areaID = -1,
        Int32 milestoneID = -1,
        Int32 categoryID = -1,
        Int32 assignedToPersonID = -1,
        Int32 statusID = -1)
    {
        var fb = new FBApiNet35(m_server, m_token);

        var cmds = new Dictionary<string, string>
        {
            {"sTitle", title},
            {"sEvent", description},
            {"cols", "ixBug"}
        };

        if (projectID > 0) cmds.Add("ixProject", projectID.ToString(CultureInfo.InvariantCulture));
        if (areaID > 0) cmds.Add("ixArea", areaID.ToString(CultureInfo.InvariantCulture));
        if (milestoneID > 0) cmds.Add("ixFixFor", milestoneID.ToString(CultureInfo.InvariantCulture));
        if (categoryID > 0) cmds.Add("ixCategory", categoryID.ToString(CultureInfo.InvariantCulture));
        if (assignedToPersonID > 0) cmds.Add("ixPersonAssignedTo", assignedToPersonID.ToString(CultureInfo.InvariantCulture));
        if (statusID > 0) cmds.Add("ixStatus", statusID.ToString(CultureInfo.InvariantCulture));

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

        var projectNodes = fb.XListProjects(onlyWritable);

        return (from XmlNode projectNode in projectNodes
            select new Project
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
            }).ToList();
    }

    public List<Area> ListAreas(Boolean onlyWritable = true, int projectID = -1, int areaID = -1)
    {
        var fb = new FBApiNet35(m_server, m_token);

        var areaNodes = fb.XListAreas(onlyWritable, projectID, areaID);

        return (from XmlNode areaNode in areaNodes
            select new Area
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
            }).ToList();
    }

    public List<Category> ListCategories()
    {
        var fb = new FBApiNet35(m_server, m_token);

        var categoriesNodes = fb.XListCategories();

        return (from XmlNode categoryNode in categoriesNodes
            select new Category
            {
                CategoryID = Convert.ToInt32(categoryNode["ixCategory"].InnerText),
                CategoryName = categoryNode["sCategory"].InnerText,
                CategoryNamePlural = categoryNode["sPlural"].InnerText,
                DefaultResolvedStatusID = Convert.ToInt32(categoryNode["ixStatusDefault"].InnerText),
                DefaultActiveStatusID = Convert.ToInt32(categoryNode["ixStatusDefaultActive"].InnerText),
                IsScheduleItem = Convert.ToBoolean(categoryNode["fIsScheduleItem"].InnerText)
            }).ToList();
    }

    public List<Status> ListStatuses(Int32 categoryID = -1, Boolean onlyResovled = false)
    {
        var fb = new FBApiNet35(m_server, m_token);

        var statusNodes = fb.XListStatuses(categoryID, onlyResovled);

        return (from XmlNode statusNode in statusNodes
            select new Status
            {
                StatusID = Convert.ToInt32(statusNode["ixStatus"].InnerText),
                StatusName = statusNode["sStatus"].InnerText,
                CategoryID = Convert.ToInt32(statusNode["ixCategory"].InnerText),
                WorkDone = Convert.ToBoolean(statusNode["fWorkDone"].InnerText),
                Resolved = Convert.ToBoolean(statusNode["fResolved"].InnerText),
                Duplicate = Convert.ToBoolean(statusNode["fDuplicate"].InnerText),
                Deleted = Convert.ToBoolean(statusNode["fDeleted"].InnerText),
                Order = Convert.ToInt32(statusNode["iOrder"].InnerText)
            }).ToList();
    }

    public List<Person> ListPeople(Boolean includeActive = true,
        Boolean includeNormal = true,
        Boolean includeDeleted = false,
        Boolean includeCommunity = false,
        Boolean includeVirtual = false)
    {
        var fb = new FBApiNet35(m_server, m_token);

        var peopleNodes = fb.XListPeople(includeActive, includeNormal, includeDeleted, includeCommunity, includeVirtual);

        return (from XmlNode personNode in peopleNodes
            select new Person
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
            }).ToList();
    }

    public List<Milestone> ListMilestones(Int32 projectID = -1)
    {
        var fb = new FBApiNet35(m_server, m_token);

        var fixForNodes = fb.XListFixFors(projectID);

        return (from XmlNode fixForNode in fixForNodes
            select new Milestone
            {
                MilestoneID = Convert.ToInt32(fixForNode["ixFixFor"].InnerText),
                MilestoneName = fixForNode["sFixFor"].InnerText,
                Inactive = Convert.ToBoolean(fixForNode["fDeleted"].InnerText),
                ProjectID = String.IsNullOrEmpty(fixForNode["ixProject"].InnerText) ? -1 : Convert.ToInt32(fixForNode["ixProject"].InnerText)
            }).ToList();
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

public class Milestone
{
    public Int32 MilestoneID { get; set; }
    public String MilestoneName { get; set; }
    public Boolean Inactive { get; set; }
    public Int32 ProjectID { get; set; }
}