using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SpbuEducation.TimeTable.BusinessObjects.Education;
using DevExpress.Xpo;
using SpbuEducation.TimeTable.Common.Web.ViewModels;
using SpbuEducation.TimeTable.Common.Web.Breadcrumb;
using SpbuEducation.TimeTable.Web.Helpers;
using SpbuEducation.TimeTable.BusinessObjects.Events;

namespace SpbuEducation.TimeTable.Web.ViewModels
{
    public class HomeIndexViewModel : BreadcrumbViewModel
    {
        public IEnumerable<DivisionItemViewModel> Divisions { get; set; }
        public IEnumerable<XtracurDivisionItemViewModel> XtracurDivisions { get; set; }
        public IEnumerable<XtracurDivisionItemViewModel> PhysTrainingDivisions { get; set; }

        public static HomeIndexViewModel Build(IEnumerable<PublicDivision> publicDivisions,
            IEnumerable<XtracurDivision> xtracurDivisions)
        {
            return new HomeIndexViewModel
            {
                Divisions = publicDivisions.Select(pd => DivisionItemViewModel.Build(pd)),
                XtracurDivisions = xtracurDivisions
                    .Where(xd => xd.Alias != "PhysTraining")
                    .Select(xd => XtracurDivisionItemViewModel.Build(xd)),
                PhysTrainingDivisions = xtracurDivisions
                    .Where(xd => xd.Alias == "PhysTraining")
                    .Select(xd => XtracurDivisionItemViewModel.Build(xd)),
                Breadcrumb = new Breadcrumb
                {
                    BreadcrumbHelper.GetBreadcrumbRootItem(true)
                }
            };
        }
    }
}
