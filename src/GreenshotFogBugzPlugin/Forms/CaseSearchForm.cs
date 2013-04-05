// TODO: File header
// TODO: Fix header

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using GreenshotPlugin.Controls;
using GreenshotPlugin.Core;
using Greenshot.IniFile;
using Greenshot.Plugin;

namespace GreenshotFogBugzPlugin.Forms {
	public partial class CaseSearchForm : FogBugzForm, IDisposable {
		const string c_langMakeNewCase = "**** Create new case *****";
		
        public CaseSearchForm(FogBugzConfiguration cfg, IGreenshotHost host, string filename,
		                      ICaptureDetails captureDetails, MemoryStream captureStream) {
			InitializeComponent();
			
            this.m_cfg = cfg;
            this.m_host = host;
            this.m_filename = filename;
            this.m_captureDetails = captureDetails;
            this.m_captureStream = captureStream;
		}

		void CaseSearchFormLoad(object sender, EventArgs e)
		{
            const int searchDelayMilliseconds = 700;

            ResultsListBox.Items.Add(c_langMakeNewCase);
            ResultsListBox.SelectedIndex = 0;
            m_searchTimer = new System.Windows.Forms.Timer();
            m_searchTimer.Interval = searchDelayMilliseconds;
            m_searchTimer.Tick += new EventHandler(searchTimerTick);

            KeywordsTextBox.Focus();
		}

		void CaseSearchFormShown(object sender, EventArgs e)
		{
            if (m_cfg.LastCaseId != 0)
            {
                KeywordsTextBox.Text = m_cfg.LastCaseId.ToString();
                // Trigger a search
                searchTimerTick(null, null);
            }
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
		
        void searchTimerTick(object sender, EventArgs e)
        {
        	const int c_maxResultsToRetrieve = 20;
            m_searchTimer.Stop();

            // Retrieve search results from FogBugz
            FogBugz fb = new FogBugz(new Uri(m_cfg.FogBugzServerUrl), m_cfg.FogBugzLoginToken);

            SearchResults results = fb.SearchWritableCases(KeywordsTextBox.Text, c_maxResultsToRetrieve);

            // Insert results
            foreach (SearchResult r in results.Results)
            	ResultsListBox.Items.Add(r.CaseId + " - " + r.Title);
            ResultsListBox.SelectedIndex = (ResultsListBox.Items.Count == 1) ? 0 : 1;
            
            this.Cursor = Cursors.Default;
        }

        void SendToButtonClick(object sender, EventArgs e)
        {
        	BackgroundForm backgroundForm = BackgroundForm.ShowAndWait(Language.GetString("fogbugz", LangKey.fogbugz), Language.GetString("fogbugz", LangKey.communication_wait));
        	
            bool success = false;            
            int caseId = 0;

            if (ResultsListBox.SelectedIndex == 0)
            {
                // TODO: Open new case dialog
                FogBugz fb = new FogBugz(new Uri(m_cfg.FogBugzServerUrl), m_cfg.FogBugzLoginToken);
                caseId = fb.CreateNewCase(CaptionTextBox.Text, m_filename, m_captureStream.GetBuffer());
                success = true;
            }
            else
            {
                string item = ResultsListBox.SelectedItem.ToString();
                caseId = Convert.ToInt32(item.Substring(0, item.IndexOf(" ")));

                FogBugz fb = new FogBugz(new Uri(m_cfg.FogBugzServerUrl), m_cfg.FogBugzLoginToken);
                fb.AttachImageToExistingCase(caseId, CaptionTextBox.Text, m_filename, m_captureStream.GetBuffer());
                success = true;
            }

            // Set the configuration for next time
            if (success)
            {
                m_cfg.LastCaseId = caseId;
                try
                {
                	string caseUrl = string.Concat(m_cfg.FogBugzServerUrl, "?", caseId);
                	if (m_cfg.CopyCaseUrlToClipboardAfterSend)
	                    System.Windows.Forms.Clipboard.SetText(caseUrl);
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
		
		void KeywordsTextBoxTextChanged(object sender, EventArgs e)
		{
			m_searchTimer.Stop();
            m_searchTimer.Start();
            this.Cursor = Cursors.AppStarting;
            ResultsListBox.Items.Clear();
            ResultsListBox.Items.Add(c_langMakeNewCase);
            ResultsListBox.SelectedIndex = 0;            
		}
		
		void KeywordsTextBoxKeyDown(object sender, KeyEventArgs e)
		{
			int index = ResultsListBox.SelectedIndex;
            int count = ResultsListBox.Items.Count;

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
                    this.Close();
                    break;
            }
		}
		
		void ResultsListBoxMouseDoubleClick(object sender, MouseEventArgs e)
		{
            SendToButtonClick(sender, e);
            this.DialogResult = DialogResult.OK;
            this.Close();
		}
						
		System.Windows.Forms.Timer m_searchTimer;
        FogBugzConfiguration m_cfg;
        IGreenshotHost m_host;
        string m_filename;
        ICaptureDetails m_captureDetails;
        MemoryStream m_captureStream;
	}
}
