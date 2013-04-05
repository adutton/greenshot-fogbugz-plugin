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
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Threading;

using Greenshot.Plugin;
using GreenshotFogBugzPlugin.Forms;
using GreenshotPlugin.Controls;
using GreenshotPlugin.Core;
using Greenshot.IniFile;

namespace GreenshotFogBugzPlugin {
	public class FogBugzPlugin : IGreenshotPlugin {
		private static readonly log4net.ILog LOG = log4net.LogManager.GetLogger(typeof(FogBugzPlugin));
		private static FogBugzConfiguration config;
		public static PluginAttribute Attributes;
		private IGreenshotHost host;
		private ComponentResourceManager resources;
		private ToolStripMenuItem itemPlugInRoot;
		
		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing) {
			if (disposing) {
				if (itemPlugInRoot != null) {
					itemPlugInRoot.Dispose();
					itemPlugInRoot = null;
				}
			}
		}
		
		public FogBugzPlugin() {
		}

		public IEnumerable<IDestination> Destinations() {
			yield return new FogBugzDestination(this);
		}

		public IEnumerable<IProcessor> Processors() {
			yield break;
		}

		/// <summary>
		/// Implementation of the IGreenshotPlugin.Initialize
		/// </summary>
		/// <param name="host">Use the IGreenshotPluginHost interface to register events</param>
		/// <param name="captureHost">Use the ICaptureHost interface to register in the MainContextMenu</param>
		/// <param name="pluginAttribute">My own attributes</param>
		/// <returns>true if plugin is initialized, false if not (doesn't show)</returns>
		public virtual bool Initialize(IGreenshotHost pluginHost, PluginAttribute myAttributes) {
			this.host = (IGreenshotHost)pluginHost;
			Attributes = myAttributes;

			// Get configuration
			config = IniConfig.GetIniSection<FogBugzConfiguration>();
			resources = new ComponentResourceManager(typeof(FogBugzPlugin));
			
			itemPlugInRoot = new ToolStripMenuItem(Language.GetString("fogbugz", LangKey.fogbugz_configure));
			itemPlugInRoot.Image = (Image)resources.GetObject("FogBugz");
			itemPlugInRoot.Tag = host;
			itemPlugInRoot.Click += delegate {
				config.ShowConfigDialog();
			};

			PluginUtils.AddToContextMenu(host, itemPlugInRoot);
			Language.LanguageChanged += new LanguageChangedHandler(OnLanguageChanged);
			return true;
		}
		
		public void OnLanguageChanged(object sender, EventArgs e) {
			if (itemPlugInRoot != null) {
				itemPlugInRoot.Text = Language.GetString("fogbugz", LangKey.fogbugz_configure);
			}
		}
		
		public virtual void Shutdown() {
			LOG.Debug("FogBugz Plugin shutdown.");
			Language.LanguageChanged -= new LanguageChangedHandler(OnLanguageChanged);
		}

		public virtual void Configure() {
			config.ShowConfigDialog();
		}

		/// <summary>
		/// This will be called when Greenshot is shutting down
		/// </summary>
		public void Closing(object sender, FormClosingEventArgs e) {
			LOG.Debug("Application closing, de-registering FogBugz Plugin!");
			Shutdown();
		}
				
		/// <summary>
		/// Upload the capture to FogBugz
		/// </summary>
		/// <returns>true if the upload succeeded</returns>
		public bool Upload(ICaptureDetails captureDetails, ISurface surfaceToUpload) {
			SurfaceOutputSettings outputSettings = new SurfaceOutputSettings(OutputFormat.jpg, 90, false);
			using (MemoryStream stream = new MemoryStream()) {
				surfaceToUpload.GetImageForExport().Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);

				try {
					string filename = Path.GetFileName(FilenameHelper.GetFilename(OutputFormat.jpg, captureDetails));
					
					CaseSearchForm caseSearchForm = new CaseSearchForm(config, this.host, filename, captureDetails, stream);
					
					if (caseSearchForm.ShowDialog() == DialogResult.OK) {
						IniConfig.Save();
						return true;
					}
				} catch (Exception e) {
					MessageBox.Show(Language.GetString("fogbugz", LangKey.upload_failure) + " " + e.Message);
				}
			}
			return false;
		}
	}
}
