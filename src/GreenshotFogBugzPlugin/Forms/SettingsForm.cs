#region

using System;
using System.Drawing;
using System.Windows.Forms;

#endregion

namespace GreenshotFogBugzPlugin.Forms
{
    public partial class SettingsForm : FogBugzForm
    {
        private bool m_copyCaseUrlToClipboardAfterSend;
        private string m_emailAddress;
        private string m_loginToken;
        private bool m_openBrowserAfterSend;
        private string m_serverUrl;

        public SettingsForm()
        {
            InitializeComponent();
            InitializeTexts();
        }

        public string FogBugzLoginToken { get { return m_loginToken; } set { m_loginToken = value; } }

        public string FogBugzServerUrl { get { return m_serverUrl; } set { m_serverUrl = value; } }

        public string FogBugzEmailAddress { get { return m_emailAddress; } set { m_emailAddress = value; } }

        public bool OpenBrowserAfterSend { get { return m_openBrowserAfterSend; } set { m_openBrowserAfterSend = value; } }

        public bool CopyCaseUrlToClipboardAfterSend
        {
            get { return m_copyCaseUrlToClipboardAfterSend; }
            set { m_copyCaseUrlToClipboardAfterSend = value; }
        }

        private void InitializeTexts()
        {
            // TODO: Set these values
            /*this.label_url.Text = lang.GetString(LangKey.label_url);
			this.buttonOK.Text = lang.GetString(LangKey.OK);
			this.buttonCancel.Text = lang.GetString(LangKey.CANCEL);
			this.Text = lang.GetString(LangKey.settings_title);
			this.label_upload_format.Text = lang.GetString(LangKey.label_upload_format);
			this.checkbox_usepagelink.Text = lang.GetString(LangKey.use_page_link);
			this.historyButton.Text = lang.GetString(LangKey.fogbugz_history);*/
        }

        public new DialogResult ShowDialog()
        {
            if (m_loginToken == null)
            {
                // Need to do initial setup
                InitialPanel.Visible = true;
                EstablishedPanel.Visible = false;

                ServerUrlTextBox.Text = m_serverUrl;
                EmailTextBox.Text = m_emailAddress;
            }
            else
            {
                // Setup is established
                InitialPanel.Visible = false;
                EstablishedPanel.Visible = true;

                FogBugzServerUrlLabel.Text = m_serverUrl;
                FogBugzEmailLabel.Text = m_emailAddress;
            }

            return base.ShowDialog();
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
            ServerUrlTextBox.Text = FormatServerAddress(ServerUrlTextBox.Text);
            InitialLabel.Text = "";

            // TODO: Handle Uri parsing failure
            var fb = new FogBugz(new Uri(ServerUrlTextBox.Text), null);

            var result = fb.Login(EmailTextBox.Text, PasswordTextBox.Text);

            switch (result)
            {
                case LoginResult.Ok:
                    // Save results
                    m_loginToken = fb.Token;
                    m_serverUrl = ServerUrlTextBox.Text;
                    m_emailAddress = EmailTextBox.Text;
                    // TODO: Checkboxes
                    m_openBrowserAfterSend = true;
                    m_copyCaseUrlToClipboardAfterSend = true;
                    EstablishedPanel.Visible = true;
                    InitialPanel.Visible = false;
                    break;
                case LoginResult.Unknown:
                    InitialLabel.Text = "Unknown error connecting to FogBugz";
                    InitialLabel.ForeColor = Color.Red;
                    break;
                case LoginResult.AccountNotFound:
                    InitialLabel.Text = "FogBugz Email/Password not accepted";
                    InitialLabel.ForeColor = Color.Red;
                    break;
                case LoginResult.ServerNotFound:
                    InitialLabel.Text = "The server could not be contacted";
                    InitialLabel.ForeColor = Color.Red;
                    break;
                case LoginResult.ServerNotCapable:
                    InitialLabel.Text = "The server does not appear to serve FogBugz";
                    InitialLabel.ForeColor = Color.Red;
                    break;
            }
        }

        private void OkButtonClick(object sender, EventArgs e)
        {
            // TODO: Save data back to configuration objects
            // I don't think this is actually needed because we do this if everything worked in the
            // Login button method
        }

        private void ClearButtonClick(object sender, EventArgs e)
        {
            InitialPanel.Visible = true;
            EstablishedPanel.Visible = false;

            ServerUrlTextBox.Text = m_serverUrl;
            EmailTextBox.Text = m_emailAddress;
            FogBugzLoginToken = "";
        }
    }
}