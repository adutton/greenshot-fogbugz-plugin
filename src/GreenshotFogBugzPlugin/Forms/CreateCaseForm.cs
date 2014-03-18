#region

using System;
using System.IO;
using Greenshot.Plugin;

#endregion

namespace GreenshotFogBugzPlugin.Forms
{
    public partial class CreateCaseForm : FogBugzForm
    {
        private readonly MemoryStream m_captureStream;
        private readonly FogBugzConfiguration m_cfg;
        private readonly string m_filename;
        private ICaptureDetails m_captureDetails;
        private IGreenshotHost m_host;

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
        }
    }
}