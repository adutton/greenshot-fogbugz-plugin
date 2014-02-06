#region

using System.Windows.Forms;
using Greenshot.IniFile;
using GreenshotFogBugzPlugin.Forms;

#endregion

namespace GreenshotFogBugzPlugin
{
    [IniSection("FogBugz", Description = "Greenshot FogBugz Plugin configuration")]
    public class FogBugzConfiguration : IniSection
    {
        [IniProperty("CopyCaseUrlToClipboardAfterSend", Description = "Copy the case Url to clipboard after image is sent", DefaultValue = "True")] public bool CopyCaseUrlToClipboardAfterSend;

        [IniProperty("FogBugzEmailAddress", Description = "Email address for FogBugz", DefaultValue = "name@example.com")] public string
            FogBugzEmailAddress;

        [IniProperty("FogBugzLoginToken", Description = "Token for access FogBugz", DefaultValue = "")] public string FogBugzLoginToken;

        [IniProperty("FogBugzServerUrl", Description = "Url to FogBugz system.", DefaultValue = "https://example.fogbugz.com/")] public string
            FogBugzServerUrl;

        [IniProperty("LastCaseId", Description = "The last bug number that was accessed", DefaultValue = "0")] public int LastCaseId;

        [IniProperty("OpenBrowserAfterSend", Description = "Open the case Url in a new browser after image is sent", DefaultValue = "True")] public
            bool OpenBrowserAfterSend;

        /// <summary>
        ///     A form for username/password
        /// </summary>
        /// <returns>bool true if OK was pressed, false if cancel</returns>
        public bool ShowConfigDialog()
        {
            SettingsForm settingsForm;

            /*BackgroundForm backgroundForm = BackgroundForm.ShowAndWait(FogBugzPlugin.Attributes.Name, lang.GetString(LangKey.communication_wait));
			try {*/
            settingsForm = new SettingsForm();
            /*} finally {
				backgroundForm.CloseDialog();
			}*/
            settingsForm.FogBugzEmailAddress = FogBugzEmailAddress;
            settingsForm.FogBugzLoginToken = FogBugzLoginToken;
            settingsForm.FogBugzServerUrl = FogBugzServerUrl;
            settingsForm.OpenBrowserAfterSend = OpenBrowserAfterSend;
            settingsForm.CopyCaseUrlToClipboardAfterSend = CopyCaseUrlToClipboardAfterSend;
            var result = settingsForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (!settingsForm.FogBugzEmailAddress.Equals(FogBugzEmailAddress)
                    || !settingsForm.FogBugzLoginToken.Equals(FogBugzLoginToken)
                    || !!settingsForm.FogBugzServerUrl.Equals(FogBugzServerUrl)
                    || !!settingsForm.OpenBrowserAfterSend.Equals(OpenBrowserAfterSend)
                    || !!settingsForm.CopyCaseUrlToClipboardAfterSend.Equals(CopyCaseUrlToClipboardAfterSend))
                {
                    FogBugzEmailAddress = settingsForm.FogBugzEmailAddress;
                    FogBugzLoginToken = settingsForm.FogBugzLoginToken;
                    FogBugzServerUrl = settingsForm.FogBugzServerUrl;
                    OpenBrowserAfterSend = settingsForm.OpenBrowserAfterSend;
                    CopyCaseUrlToClipboardAfterSend = settingsForm.CopyCaseUrlToClipboardAfterSend;
                }
                IniConfig.Save();
                return true;
            }
            return false;
        }
    }
}