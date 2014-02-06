#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Greenshot.IniFile;
using Greenshot.Plugin;
using GreenshotFogBugzPlugin.Forms;
using GreenshotPlugin.Core;
using log4net;

#endregion

namespace GreenshotFogBugzPlugin
{
    public class FogBugzPlugin : IGreenshotPlugin
    {
        private static readonly ILog LOG = LogManager.GetLogger(typeof (FogBugzPlugin));
        private static FogBugzConfiguration config;
        public static PluginAttribute Attributes;
        private IGreenshotHost host;
        private ToolStripMenuItem itemPlugInRoot;
        private ComponentResourceManager resources;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerable<IDestination> Destinations()
        {
            yield return new FogBugzDestination(this);
        }

        public IEnumerable<IProcessor> Processors()
        {
            yield break;
        }

        /// <summary>
        ///     Implementation of the IGreenshotPlugin.Initialize
        /// </summary>
        /// <param name="host">Use the IGreenshotPluginHost interface to register events</param>
        /// <param name="captureHost">Use the ICaptureHost interface to register in the MainContextMenu</param>
        /// <param name="pluginAttribute">My own attributes</param>
        /// <returns>true if plugin is initialized, false if not (doesn't show)</returns>
        public virtual bool Initialize(IGreenshotHost pluginHost, PluginAttribute myAttributes)
        {
            host = pluginHost;
            Attributes = myAttributes;

            // Get configuration
            config = IniConfig.GetIniSection<FogBugzConfiguration>();
            resources = new ComponentResourceManager(typeof (FogBugzPlugin));

            itemPlugInRoot = new ToolStripMenuItem(Language.GetString("fogbugz", LangKey.fogbugz_configure))
            {
                Image = (Image) resources.GetObject("FogBugz"),
                Tag = host
            };
            itemPlugInRoot.Click += delegate { config.ShowConfigDialog(); };

            PluginUtils.AddToContextMenu(host, itemPlugInRoot);
            Language.LanguageChanged += OnLanguageChanged;
            return true;
        }

        public virtual void Shutdown()
        {
            LOG.Debug("FogBugz Plugin shutdown.");
            Language.LanguageChanged -= OnLanguageChanged;
        }

        public virtual void Configure()
        {
            config.ShowConfigDialog();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (itemPlugInRoot != null)
                {
                    itemPlugInRoot.Dispose();
                    itemPlugInRoot = null;
                }
            }
        }

        public void OnLanguageChanged(object sender, EventArgs e)
        {
            if (itemPlugInRoot != null)
            {
                itemPlugInRoot.Text = Language.GetString("fogbugz", LangKey.fogbugz_configure);
            }
        }

        /// <summary>
        ///     This will be called when Greenshot is shutting down
        /// </summary>
        public void Closing(object sender, FormClosingEventArgs e)
        {
            LOG.Debug("Application closing, de-registering FogBugz Plugin!");
            Shutdown();
        }

        /// <summary>
        ///     Upload the capture to FogBugz
        /// </summary>
        /// <returns>true if the upload succeeded</returns>
        public bool Upload(ICaptureDetails captureDetails, ISurface surfaceToUpload)
        {
            var outputSettings = new SurfaceOutputSettings(OutputFormat.jpg, 90, false);
            using (var stream = new MemoryStream())
            {
                surfaceToUpload.GetImageForExport().Save(stream, ImageFormat.Jpeg);

                try
                {
                    var filename = Path.GetFileName(FilenameHelper.GetFilename(OutputFormat.jpg, captureDetails));

                    var caseSearchForm = new CaseSearchForm(config, host, filename, captureDetails, stream);

                    if (caseSearchForm.ShowDialog() == DialogResult.OK)
                    {
                        IniConfig.Save();
                        return true;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(Language.GetString("fogbugz", LangKey.upload_failure) + " " + e.Message);
                }
            }
            return false;
        }
    }
}