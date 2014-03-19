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

        private readonly MemoryStream _mCaptureStream;
        private readonly string _mFilename;
        private ICaptureDetails _mCaptureDetails;
        private IGreenshotHost _mHost;

        #endregion

        #region FogBugz Fields

        private FogBugz _fb;
        private readonly FogBugzConfiguration _mCfg;
        private readonly FogBugzData _mData;

        #endregion

        #endregion

        public CreateCaseForm(FogBugzConfiguration cfg,
            IGreenshotHost host,
            string filename,
            ICaptureDetails captureDetails,
            MemoryStream captureStream,
            FogBugzData data)
        {
            InitializeComponent();

            _mCfg = cfg;
            _mHost = host;
            _mFilename = filename;
            _mCaptureDetails = captureDetails;
            _mCaptureStream = captureStream;
            _mData = data;

            CaseCreated = false;
        }

        private void CreateCaseForm_Load(object sender, EventArgs e)
        {
            _fb = new FogBugz(new Uri(_mCfg.FogBugzServerUrl), _mCfg.FogBugzLoginToken);

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
            cbProject.DataSource = _mData.Projects;
            cbProject.DisplayMember = "ProjectName";
            cbProject.ValueMember = "ProjectID";
            cbProject.SelectedIndexChanged += cbProject_SelectedIndexChanged;
        }

        private void DataBindArea()
        {
            cbArea.DataSource = _mData.Areas.Where(x => x.ProjectID == Convert.ToInt32(cbProject.SelectedValue)).ToList();
            cbArea.DisplayMember = "AreaName";
            cbArea.ValueMember = "AreaID";
            cbArea.SelectedIndexChanged += cbArea_SelectedIndexChanged;
        }

        private void DataBindCategories()
        {
            cbCategory.DataSource = _mData.Categories;
            cbCategory.DisplayMember = "CategoryName";
            cbCategory.ValueMember = "CategoryID";
            cbCategory.SelectedIndexChanged += cbCategory_SelectedIndexChanged;
        }

        private void DataBindStatuses()
        {
            var currentCategory = _mData.Categories.FirstOrDefault(x => x.CategoryID == Convert.ToInt32(cbCategory.SelectedValue));

            cbStatus.DataSource =
                _mData.Statuses.Where(x => x.CategoryID == Convert.ToInt32(cbCategory.SelectedValue) && !x.WorkDone && !x.Resolved).ToList();
            cbStatus.DisplayMember = "StatusName";
            cbStatus.ValueMember = "StatusID";

            if (currentCategory != null)
                cbStatus.SelectedValue = currentCategory.DefaultActiveStatusID;
        }

        private void DataBindPeople()
        {
            cbAssignedTo.DataSource = _mData.People;
            cbAssignedTo.DisplayMember = "FullName";
            cbAssignedTo.ValueMember = "PersonID";

            var currentArea = _mData.Areas.FirstOrDefault(x => x.AreaID == Convert.ToInt32(cbArea.SelectedValue));
            if (currentArea != null && currentArea.OwnerID != -1)
            {
                cbAssignedTo.SelectedValue = currentArea.OwnerID;
            }
        }

        private void DataBindMilestones()
        {
            cbMilestone.DataSource = _mData.Milestones.Where(x => x.ProjectID == 0 || x.ProjectID == Convert.ToInt32(cbProject.SelectedValue)).ToList();
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
                _mFilename,
                _mCaptureStream.GetBuffer(),
                projectID,
                areaID,
                milestoneID,
                categoryID,
                assignedToPersonID,
                statusID,
                dtDueDate.Value);

            // Set the configuration for next time
            _mCfg.LastCaseId = caseId;
            try
            {
                var caseUrl = string.Concat(_mCfg.FogBugzServerUrl, "?", caseId);
                if (_mCfg.CopyCaseUrlToClipboardAfterSend)
                    Clipboard.SetText(caseUrl);
                if (_mCfg.OpenBrowserAfterSend)
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