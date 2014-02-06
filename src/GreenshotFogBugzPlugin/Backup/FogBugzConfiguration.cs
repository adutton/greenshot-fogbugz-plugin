/*
 * Greenshot - a free and open source screenshot tool
 * Copyright (C) 2007-2012  Thomas Braun, Jens Klingen, Robin Krom
 * 
 * For more information see: http://getgreenshot.org/
 * The Greenshot project is hosted on Sourceforge: http://sourceforge.net/projects/greenshot/
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 1 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Collections.Generic;
using System.Windows.Forms;

using GreenshotPlugin.Controls;
using GreenshotPlugin.Core;
using Greenshot.IniFile;
using GreenshotFogBugzPlugin.Forms;

namespace GreenshotFogBugzPlugin {
	[IniSection("FogBugz", Description="Greenshot FogBugz Plugin configuration")]
	public class FogBugzConfiguration : IniSection {
		[IniProperty("FogBugzServerUrl", Description="Url to FogBugz system.", DefaultValue="https://example.fogbugz.com/")]
		public string FogBugzServerUrl;
		[IniProperty("FogBugzEmailAddress", Description="Email address for FogBugz", DefaultValue="name@example.com")]
		public string FogBugzEmailAddress;
		[IniProperty("FogBugzLoginToken", Description="Token for access FogBugz", DefaultValue="")]
		public string FogBugzLoginToken;
		
		[IniProperty("LastCaseId", Description="The last bug number that was accessed", DefaultValue="0")]
		public int LastCaseId;
        
		[IniProperty("OpenBrowserAfterSend", Description="Open the case Url in a new browser after image is sent", DefaultValue="True")]
		public bool OpenBrowserAfterSend;
		[IniProperty("CopyCaseUrlToClipboardAfterSend", Description="Copy the case Url to clipboard after image is sent", DefaultValue="True")]
		public bool CopyCaseUrlToClipboardAfterSend;

		/// <summary>
		/// A form for username/password
		/// </summary>
		/// <returns>bool true if OK was pressed, false if cancel</returns>
		public bool ShowConfigDialog() {
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
			DialogResult result = settingsForm.ShowDialog();
			if (result == DialogResult.OK) {
				if (!settingsForm.FogBugzEmailAddress.Equals(FogBugzEmailAddress) 
				    || !settingsForm.FogBugzLoginToken.Equals(FogBugzLoginToken)
					|| !!settingsForm.FogBugzServerUrl.Equals(FogBugzServerUrl)
					|| !!settingsForm.OpenBrowserAfterSend.Equals(OpenBrowserAfterSend)
					|| !!settingsForm.CopyCaseUrlToClipboardAfterSend.Equals(CopyCaseUrlToClipboardAfterSend)) {
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
