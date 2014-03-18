#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Greenshot.Plugin;

#endregion

namespace GreenshotFogBugzPlugin.Forms
{
    public partial class CreateCaseForm : FogBugzForm
    {
        #region Fields

        #region Greenshot Fields

        private readonly MemoryStream m_captureStream;
        private readonly FogBugzConfiguration m_cfg;
        private readonly string m_filename;
        private ICaptureDetails m_captureDetails;
        private IGreenshotHost m_host;

        #endregion

        #region FogBugz Fields
        private List<Project> _projects;
        private List<Area> _areas;
        private List<Category> _categories;
        private List<Status> _statuses;
        private List<Person> _people; 
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
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void CreateCaseForm_Load(object sender, EventArgs e)
        {
            var fb = new FogBugz(new Uri(m_cfg.FogBugzServerUrl), m_cfg.FogBugzLoginToken);
            _projects = fb.ListProjects();
            _areas = fb.ListAreas();
            _categories = fb.ListCategories();
            _statuses = fb.ListStatuses();
            _people = fb.ListPeople();

            DataBindProject();
            DataBindArea();
            DataBindCategories();
            DataBindStatuses();
            DataBindPeople();
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

            cbStatus.DataSource = _statuses.Where(x => x.CategoryID == Convert.ToInt32(cbCategory.SelectedValue) && !x.WorkDone).ToList();
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
        #endregion

        #region Event Handling

        private void cbProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataBindArea();
        }

        private void cbArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataBindPeople();
        }
        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataBindStatuses();
        }

        #endregion

        private void tableLayoutPanel1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {

        }
    }
}