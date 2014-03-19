#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Greenshot.Plugin;
using GreenshotPlugin.Controls;
using GreenshotPlugin.Core;

#endregion

namespace GreenshotFogBugzPlugin.Forms
{
    public partial class CreateCaseForm : FogBugzForm
    {
        #region Properties
        public Boolean CaseCreated { get; set; }
        #endregion

        #region Fields

        #region Greenshot Fields

        private readonly MemoryStream m_captureStream;
        private readonly FogBugzConfiguration m_cfg;
        private readonly string m_filename;
        private ICaptureDetails m_captureDetails;
        private IGreenshotHost m_host;

        #endregion

        #region FogBugz Fields

        private List<Area> _areas;
        private List<Category> _categories;
        private FogBugz _fb;
        private List<Milestone> _milestones;
        private List<Person> _people;
        private List<Project> _projects;
        private List<Status> _statuses;

        #endregion

        #endregion

        public CreateCaseForm(FogBugzConfiguration cfg,
            IGreenshotHost host,
            string filename,
            ICaptureDetails captureDetails,
            MemoryStream captureStream)
        {
            InitializeComponent();

            m_cfg = cfg;
            m_host = host;
            m_filename = filename;
            m_captureDetails = captureDetails;
            m_captureStream = captureStream;

            CaseCreated = false;
        }

        private void CreateCaseForm_Load(object sender, EventArgs e)
        {
            _fb = new FogBugz(new Uri(m_cfg.FogBugzServerUrl), m_cfg.FogBugzLoginToken);
            _projects = _fb.ListProjects();
            _areas = _fb.ListAreas();
            _categories = _fb.ListCategories();
            _statuses = _fb.ListStatuses();
            _people = _fb.ListPeople();
            _milestones = _fb.ListMilestones();

            DataBindProject();
            DataBindArea();
            DataBindCategories();
            DataBindStatuses();
            DataBindPeople();
            DataBindMilestones();

            dtDueDate.MinDate = DateTime.Now;
            dtDueDate.MaxDate = DateTime.Now.AddYears(1);
        }

        #region Data Binding

        private void DataBindProject()
        {
            cbProject.DataSource = _projects;
            cbProject.DisplayMember = "ProjectName";
            cbProject.ValueMember = "ProjectID";
            cbProject.SelectedIndexChanged += cbProject_SelectedIndexChanged;
        }

        private void DataBindArea()
        {
            cbArea.DataSource = _areas.Where(x => x.ProjectID == Convert.ToInt32(cbProject.SelectedValue)).ToList();
            cbArea.DisplayMember = "AreaName";
            cbArea.ValueMember = "AreaID";
            cbArea.SelectedIndexChanged += cbArea_SelectedIndexChanged;
        }

        private void DataBindCategories()
        {
            cbCategory.DataSource = _categories;
            cbCategory.DisplayMember = "CategoryName";
            cbCategory.ValueMember = "CategoryID";
            cbCategory.SelectedIndexChanged += cbCategory_SelectedIndexChanged;
        }

        private void DataBindStatuses()
        {
            var currentCategory = _categories.FirstOrDefault(x => x.CategoryID == Convert.ToInt32(cbCategory.SelectedValue));

            cbStatus.DataSource =
                _statuses.Where(x => x.CategoryID == Convert.ToInt32(cbCategory.SelectedValue) && !x.WorkDone && !x.Resolved).ToList();
            cbStatus.DisplayMember = "StatusName";
            cbStatus.ValueMember = "StatusID";

            if (currentCategory != null)
                cbStatus.SelectedValue = currentCategory.DefaultActiveStatusID;
        }

        private void DataBindPeople()
        {
            cbAssignedTo.DataSource = _people;
            cbAssignedTo.DisplayMember = "FullName";
            cbAssignedTo.ValueMember = "PersonID";

            var currentArea = _areas.FirstOrDefault(x => x.AreaID == Convert.ToInt32(cbArea.SelectedValue));
            if (currentArea != null && currentArea.OwnerID != -1)
            {
                cbAssignedTo.SelectedValue = currentArea.OwnerID;
            }
        }

        private void DataBindMilestones()
        {
            cbMilestone.DataSource = _milestones.Where(x => x.ProjectID == 0 || x.ProjectID == Convert.ToInt32(cbProject.SelectedValue)).ToList();
            cbMilestone.DisplayMember = "MilestoneName";
            cbMilestone.ValueMember = "MilestoneID";
        }

        #endregion

        #region Event Handling

        private void cbProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataBindArea();
            DataBindMilestones();
        }

        private void cbArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataBindPeople();
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataBindStatuses();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            var backgroundForm = BackgroundForm.ShowAndWait(Language.GetString("fogbugz", LangKey.fogbugz),
                Language.GetString("fogbugz", LangKey.communication_wait));

            Int32 projectID = 0, areaID = 0, milestoneID = 0, categoryID = 0, assignedToPersonID = 0, statusID = 0;

            if (cbProject.SelectedItem != null)
                projectID = Convert.ToInt32(cbProject.SelectedValue);

            if (cbArea.SelectedItem != null)
                areaID = Convert.ToInt32(cbArea.SelectedValue);

            if (cbMilestone.SelectedItem != null)
                milestoneID = Convert.ToInt32(cbMilestone.SelectedValue);

            if (cbCategory.SelectedItem != null)
                categoryID = Convert.ToInt32(cbCategory.SelectedValue);

            if (cbAssignedTo.SelectedItem != null)
                assignedToPersonID = Convert.ToInt32(cbAssignedTo.SelectedValue);

            if (cbStatus.SelectedItem != null)
                statusID = Convert.ToInt32(cbStatus.SelectedValue);

            var caseId = _fb.CreateNewCase(txtTitle.Text,
                txtDescription.Text,
                m_filename,
                m_captureStream.GetBuffer(),
                projectID,
                areaID,
                milestoneID,
                categoryID,
                assignedToPersonID,
                statusID);

            // Set the configuration for next time
            m_cfg.LastCaseId = caseId;
            try
            {
                var caseUrl = string.Concat(m_cfg.FogBugzServerUrl, "?", caseId);
                if (m_cfg.CopyCaseUrlToClipboardAfterSend)
                    Clipboard.SetText(caseUrl);
                if (m_cfg.OpenBrowserAfterSend)
                    Process.Start(caseUrl);
            }
            catch
            {
                // Throw away "after-upload" exceptions
            }

            DialogResult = DialogResult.OK;

            backgroundForm.CloseDialog();

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void txtDescription_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtDescription != null && e.Control && e.KeyCode == Keys.A)
            {
                txtDescription.SelectAll();
            }
        }

        #endregion
    }
}