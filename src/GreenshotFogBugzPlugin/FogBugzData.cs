#region

using System;
using System.Collections.Generic;
using System.Threading;

#endregion

namespace GreenshotFogBugzPlugin
{
    public class FogBugzData
    {
        #region Properties

        public List<Area> Areas { get; private set; }
        public List<Category> Categories { get; private set; }
        public List<Milestone> Milestones { get; private set; }
        public List<Person> People { get; private set; }
        public List<Project> Projects { get; private set; }
        public List<Status> Statuses { get; private set; }

        #endregion

        #region Fields

        private readonly FogBugz _fb;

        #endregion

        public FogBugzData(FogBugzConfiguration config, Boolean loadNow = true)
        {
            _fb = new FogBugz(new Uri(config.FogBugzServerUrl), config.FogBugzLoginToken);

            if (loadNow)
                RefreshData();
        }

        public void RefreshData()
        {
            var t = new Thread(RefreshDataInternal);
            t.Start();
        }

        private void RefreshDataInternal()
        {
            Areas = _fb.ListAreas();
            Categories = _fb.ListCategories();
            Milestones = _fb.ListMilestones();
            People = _fb.ListPeople();
            Projects = _fb.ListProjects();
            Statuses = _fb.ListStatuses();
        }
    }
}