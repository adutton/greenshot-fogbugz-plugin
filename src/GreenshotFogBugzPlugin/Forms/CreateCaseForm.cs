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

        private List<Area> _areas;
        private List<Project> _projects;

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

            DataBindProject();
            DataBindArea();
        }

        #region Data Binding

        private void DataBindProject()
        {
            cbProject.DataSource = _projects;
            cbProject.DisplayMember = "ProjectName";
            cbProject.ValueMember = "ID";
            cbProject.SelectedIndexChanged += cbProject_SelectedIndexChanged;
        }

        private void DataBindArea()
        {
            cbArea.DataSource = _areas.Where(x => x.ProjectID == Convert.ToInt32(cbProject.SelectedValue));
            cbArea.DisplayMember = "AreaName";
            cbArea.ValueMember = "ID";
            cbArea.SelectedIndexChanged += cbArea_SelectedIndexChanged;
        }

        #endregion

        #region Event Handling

        private void cbProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataBindArea();
        }

        private void cbArea_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        #endregion
    }
}