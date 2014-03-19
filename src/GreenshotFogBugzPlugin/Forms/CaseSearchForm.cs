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
        private const string CLangMakeNewCase = "**** Create new case *****";
        private readonly ICaptureDetails _mCaptureDetails;
        private readonly MemoryStream _mCaptureStream;
        private readonly FogBugzConfiguration _mCfg;
        private readonly string _mFilename;
        private readonly IGreenshotHost _mHost;
        private Timer _mSearchTimer;

        public CaseSearchForm(FogBugzConfiguration cfg,
            IGreenshotHost host,
            string filename,
            ICaptureDetails captureDetails,
            MemoryStream captureStream)
        {
            InitializeComponent();

            _mCfg = cfg;
            _mHost = host;
            _mFilename = filename;
            _mCaptureDetails = captureDetails;
            _mCaptureStream = captureStream;
        }

        public new void Dispose()
        {
            if (_mSearchTimer != null)
            {
                _mSearchTimer.Dispose();
                _mSearchTimer = null;
            }

            base.Dispose();
        }

        private void CaseSearchFormLoad(object sender, EventArgs e)
        {
            const int searchDelayMilliseconds = 700;

            ResultsListBox.Items.Add(CLangMakeNewCase);
            ResultsListBox.SelectedIndex = 0;
            _mSearchTimer = new Timer
            {
                Interval = searchDelayMilliseconds
            };
            _mSearchTimer.Tick += SearchTimerTick;

            KeywordsTextBox.Focus();
        }

        private void CaseSearchFormShown(object sender, EventArgs e)
        {
            if (_mCfg.LastCaseId != 0)
            {
                KeywordsTextBox.Text = _mCfg.LastCaseId.ToString();
                // Trigger a search
                SearchTimerTick(null, null);
            }
        }

        private void SearchTimerTick(object sender, EventArgs e)
        {
            const int cMaxResultsToRetrieve = 20;
            _mSearchTimer.Stop();

            // Retrieve search results from FogBugz
            var fb = new FogBugz(new Uri(_mCfg.FogBugzServerUrl), _mCfg.FogBugzLoginToken);

            var results = fb.SearchWritableCases(KeywordsTextBox.Text, cMaxResultsToRetrieve);

            // Insert results
            foreach (SearchResult r in results.Results)
                ResultsListBox.Items.Add(r.CaseId + " - " + r.Title);
            ResultsListBox.SelectedIndex = (ResultsListBox.Items.Count == 1) ? 0 : 1;

            Cursor = Cursors.Default;
        }

        private void SendToButtonClick(object sender, EventArgs e)
        {
            if (ResultsListBox.SelectedIndex == 0)
            {
                btnNewCase_Click(sender, e);
            }
            else
            {
                var backgroundForm = BackgroundForm.ShowAndWait(Language.GetString("fogbugz", LangKey.fogbugz),
                    Language.GetString("fogbugz", LangKey.communication_wait));


                var item = ResultsListBox.SelectedItem.ToString();
                var caseId = Convert.ToInt32(item.Substring(0, item.IndexOf(" ")));

                var fb = new FogBugz(new Uri(_mCfg.FogBugzServerUrl), _mCfg.FogBugzLoginToken);
                fb.AttachImageToExistingCase(caseId, CaptionTextBox.Text, _mFilename, _mCaptureStream.GetBuffer());


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
                backgroundForm.CloseDialog();
            }
        }

        private void KeywordsTextBoxTextChanged(object sender, EventArgs e)
        {
            _mSearchTimer.Stop();
            _mSearchTimer.Start();
            Cursor = Cursors.AppStarting;
            ResultsListBox.Items.Clear();
            ResultsListBox.Items.Add(CLangMakeNewCase);
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

        private void btnNewCase_Click(object sender, EventArgs e)
        {
            var createCaseForm = new CreateCaseForm(_mCfg, _mHost, _mFilename, _mCaptureDetails, _mCaptureStream);

            if (createCaseForm.ShowDialog() == DialogResult.OK)
                Close();
        }
    }
}