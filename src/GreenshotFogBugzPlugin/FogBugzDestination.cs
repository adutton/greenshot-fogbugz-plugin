#region

using System.ComponentModel;
using System.Drawing;
using Greenshot.IniFile;
using Greenshot.Plugin;
using GreenshotPlugin.Core;
using log4net;

#endregion

namespace GreenshotFogBugzPlugin
{
    public class FogBugzDestination : AbstractDestination
    {
        private static ILog LOG = LogManager.GetLogger(typeof (FogBugzDestination));
        private static FogBugzConfiguration config = IniConfig.GetIniSection<FogBugzConfiguration>();
        private readonly FogBugzPlugin plugin;

        public FogBugzDestination(FogBugzPlugin plugin)
        {
            this.plugin = plugin;
        }

        public override string Designation { get { return "FogBugz"; } }

        public override string Description { get { return Language.GetString("fogbugz", LangKey.upload_menu_item); } }

        public override Image DisplayIcon
        {
            get
            {
                var resources = new ComponentResourceManager(typeof (FogBugzPlugin));
                return (Image) resources.GetObject("FogBugz");
            }
        }

        public override ExportInformation ExportCapture(bool manuallyInitiated, ISurface surface, ICaptureDetails captureDetails)
        {
            var exportInformation = new ExportInformation(Designation, Description);
            string uploadURL = null;
            exportInformation.ExportMade = plugin.Upload(captureDetails, surface);
            exportInformation.Uri = uploadURL;
            ProcessExport(exportInformation, surface);
            return exportInformation;
        }
    }
}