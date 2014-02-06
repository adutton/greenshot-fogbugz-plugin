#region

using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Greenshot.Plugin;
using GreenshotPlugin.Controls;
using GreenshotPlugin.Core;

#endregion

namespace GreenshotFogBugzPlugin.Forms
{
    public partial class CaseSearchForm : FogBugzForm, IDisposable
    {
        // TODO: Move this to a language setting
        private const string c_langMakeNewCase = "**** Create new case *****";
        private readonly MemoryStream m_captureStream;
        private readonly FogBugzConfiguration m_cfg;
        private readonly string m_filename;
        private ICaptureDetails m_captureDetails;
        private IGreenshotHost m_host;
        private Timer m_searchTimer;

        public CaseSearchForm(FogBugzConfiguration cfg,
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

        public new void Dispose()
        {
            if (m_searchTimer != null)
            {
                m_searchTimer.Dispose();
                m_searchTimer = null;
            }

            base.Dispose();
        }

        private void CaseSearchFormLoad(object sender, EventArgs e)
        {
            const int searchDelayMilliseconds = 700;

            ResultsListBox.Items.Add(c_langMakeNewCase);
            ResultsListBox.SelectedIndex = 0;
            m_searchTimer = new Timer();
            m_searchTimer.Interval = searchDelayMilliseconds;
            m_searchTimer.Tick += searchTimerTick;

            KeywordsTextBox.Focus();
        }

        private void CaseSearchFormShown(object sender, EventArgs e)
        {
            if (m_cfg.LastCaseId != 0)
            {
                KeywordsTextBox.Text = m_cfg.LastCaseId.ToString();
                // Trigger a search
                searchTimerTick(null, null);
            }
        }

        private void searchTimerTick(object sender, EventArgs e)
        {
            const int c_maxResultsToRetrieve = 20;
            m_searchTimer.Stop();

            // Retrieve search results from FogBugz
            var fb = new FogBugz(new Uri(m_cfg.FogBugzServerUrl), m_cfg.FogBugzLoginToken);

            var results = fb.SearchWritableCases(KeywordsTextBox.Text, c_maxResultsToRetrieve);

            // Insert results
            foreach (SearchResult r in results.Results)
                ResultsListBox.Items.Add(r.CaseId + " - " + r.Title);
            ResultsListBox.SelectedIndex = (ResultsListBox.Items.Count == 1) ? 0 : 1;

            Cursor = Cursors.Default;
        }

        private void SendToButtonClick(object sender, EventArgs e)
        {
            var backgroundForm = BackgroundForm.ShowAndWait(Language.GetString("fogbugz", LangKey.fogbugz),
                Language.GetString("fogbugz", LangKey.communication_wait));

            var success = false;
            var caseId = 0;

            if (ResultsListBox.SelectedIndex == 0)
            {
                // TODO: Open new case dialog
                var fb = new FogBugz(new Uri(m_cfg.FogBugzServerUrl), m_cfg.FogBugzLoginToken);
                caseId = fb.CreateNewCase(CaptionTextBox.Text, m_filename, m_captureStream.GetBuffer());
                success = true;
            }
            else
            {
                var item = ResultsListBox.SelectedItem.ToString();
                caseId = Convert.ToInt32(item.Substring(0, item.IndexOf(" ")));

                var fb = new FogBugz(new Uri(m_cfg.FogBugzServerUrl), m_cfg.FogBugzLoginToken);
                fb.AttachImageToExistingCase(caseId, CaptionTextBox.Text, m_filename, m_captureStream.GetBuffer());
                success = true;
            }

            // Set the configuration for next time
            if (success)
            {
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
            }
            backgroundForm.CloseDialog();
        }

        private void KeywordsTextBoxTextChanged(object sender, EventArgs e)
        {
            m_searchTimer.Stop();
            m_searchTimer.Start();
            Cursor = Cursors.AppStarting;
            ResultsListBox.Items.Clear();
            ResultsListBox.Items.Add(c_langMakeNewCase);
            ResultsListBox.SelectedIndex = 0;
        }

        private void KeywordsTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            var index = ResultsListBox.SelectedIndex;
            var count = ResultsListBox.Items.Count;

            // TODO: Figure out how many to page
            const int pageSize = 10;

            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (index > 0)
                        ResultsListBox.SelectedIndex--;
                    e.SuppressKeyPress = true;
                    break;
                case Keys.Down:
                    if (index < count - 1)
                        ResultsListBox.SelectedIndex++;
                    e.SuppressKeyPress = true;
                    break;
                case Keys.PageUp:
                    if (index <= pageSize)
                        ResultsListBox.SelectedIndex = 0;
                    if (index > pageSize)
                        ResultsListBox.SelectedIndex -= pageSize;
                    break;
                case Keys.PageDown:
                    if (index >= count - pageSize - 1)
                        ResultsListBox.SelectedIndex = count - 1;
                    if (index < count - pageSize - 1)
                        ResultsListBox.SelectedIndex += pageSize;
                    break;
                case Keys.Enter:
                    SendToButtonClick(sender, e);
                    break;
                case Keys.Escape:
                    Close();
                    break;
            }
        }

        private void ResultsListBoxMouseDoubleClick(object sender, MouseEventArgs e)
        {
            SendToButtonClick(sender, e);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}