// TODO: File header
// TODO: Fix formatting
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using GreenshotPlugin.Core;

namespace GreenshotFogBugzPlugin.Forms {
    public partial class SettingsForm : FogBugzForm {
        public SettingsForm() {
            InitializeComponent();
            InitializeTexts();
        }
        
   		void InitializeTexts() {
   			// TODO: Set these values
			/*this.label_url.Text = lang.GetString(LangKey.label_url);
			this.buttonOK.Text = lang.GetString(LangKey.OK);
			this.buttonCancel.Text = lang.GetString(LangKey.CANCEL);
			this.Text = lang.GetString(LangKey.settings_title);
			this.label_upload_format.Text = lang.GetString(LangKey.label_upload_format);
			this.checkbox_usepagelink.Text = lang.GetString(LangKey.use_page_link);
			this.historyButton.Text = lang.GetString(LangKey.fogbugz_history);*/
		}

        public new DialogResult ShowDialog() {
            if (m_loginToken == null) {
                // Need to do initial setup
                this.InitialPanel.Visible = true;
                this.EstablishedPanel.Visible = false;

                this.ServerUrlTextBox.Text = m_serverUrl;
                this.EmailTextBox.Text = m_emailAddress;
            }
            else
            {
                // Setup is established
                this.InitialPanel.Visible = false;
                this.EstablishedPanel.Visible = true;
                
                this.FogBugzServerUrlLabel.Text = m_serverUrl;
                this.FogBugzEmailLabel.Text = m_emailAddress;
            }

            return base.ShowDialog();
        }

        public string FogBugzLoginToken
        {
            get { return m_loginToken; }
            set { m_loginToken = value; }
        }

        public string FogBugzServerUrl
        {
            get { return m_serverUrl; }
            set { m_serverUrl = value; }
        }

        public string FogBugzEmailAddress
        {
            get { return m_emailAddress; }
            set { m_emailAddress = value; }
        }

        public bool OpenBrowserAfterSend
        {
        	get { return m_openBrowserAfterSend; }
        	set { m_openBrowserAfterSend = value; }
        }
        public bool CopyCaseUrlToClipboardAfterSend
        {
        	get { return m_copyCaseUrlToClipboardAfterSend; }
        	set { m_copyCaseUrlToClipboardAfterSend = value; }
        }

        private static string FormatServerAddress(string url)
        {
            if (string.IsNullOrEmpty(url) || url.Trim() == string.Empty)
                return null;
            if (!url.StartsWith("http"))
                url = "http://" + url;
            if (url.EndsWith("/api.xml"))
                url = url.Remove(url.Length - 8);
            if (url.EndsWith("/api.asp"))
                url = url.Remove(url.Length - 8);

            return url;
        }

        private void LoginButtonClick(object sender, EventArgs e)
        {
            this.ServerUrlTextBox.Text = FormatServerAddress(this.ServerUrlTextBox.Text);
            this.InitialLabel.Text = "";
            
            // TODO: Handle Uri parsing failure
            FogBugz fb = new FogBugz(new Uri(this.ServerUrlTextBox.Text), null);

            LoginResult result = fb.Login(this.EmailTextBox.Text, this.PasswordTextBox.Text);

            switch (result)
            {
                case LoginResult.Ok:
                    // Save results
                    m_loginToken = fb.Token;
                    m_serverUrl = this.ServerUrlTextBox.Text;
                    m_emailAddress = this.EmailTextBox.Text;
                    // TODO: Checkboxes
                    m_openBrowserAfterSend = true;
                    m_copyCaseUrlToClipboardAfterSend = true;
                    this.EstablishedPanel.Visible = true;
                    this.InitialPanel.Visible = false;
                    break;
                case LoginResult.Unknown:
                    this.InitialLabel.Text = "Unknown error connecting to FogBugz";
                    this.InitialLabel.ForeColor = Color.Red;
                    break;
                case LoginResult.AccountNotFound:
                    this.InitialLabel.Text = "FogBugz Email/Password not accepted";
                    this.InitialLabel.ForeColor = Color.Red;
                    break;
                case LoginResult.ServerNotFound:
                    this.InitialLabel.Text = "The server could not be contacted";
                    this.InitialLabel.ForeColor = Color.Red;
                    break;
                case LoginResult.ServerNotCapable:
                    this.InitialLabel.Text = "The server does not appear to serve FogBugz";
                    this.InitialLabel.ForeColor = Color.Red;
                    break;
            }
        }

        void OkButtonClick(object sender, EventArgs e)
        {
            // TODO: Save data back to configuration objects
            // I don't think this is actually needed because we do this if everything worked in the
            // Login button method
        }

        void ClearButtonClick(object sender, EventArgs e)
        {
            this.InitialPanel.Visible = true;
            this.EstablishedPanel.Visible = false;

            this.ServerUrlTextBox.Text = m_serverUrl;
            this.EmailTextBox.Text = m_emailAddress;
            this.FogBugzLoginToken = "";
        }

        string m_loginToken;
        string m_serverUrl;
        string m_emailAddress;
        bool m_openBrowserAfterSend;
        bool m_copyCaseUrlToClipboardAfterSend;
    }
}
