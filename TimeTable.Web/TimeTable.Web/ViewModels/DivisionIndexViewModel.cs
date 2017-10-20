using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SpbuEducation.TimeTable.BusinessObjects.Education;
using DevExpress.Xpo;
using SpbuEducation.TimeTable.Common.Web.ViewModels;
using SpbuEducation.TimeTable.Common.Web.Breadcrumb;
using SpbuEducation.TimeTable.Web.Helpers;

namespace SpbuEducation.TimeTable.Web.ViewModels
{
    public class DivisionIndexViewModel : BreadcrumbViewModel
    {
        public IEnumerable<DivisionItemViewModel> Divisions { get; set; }

        public static DivisionIndexViewModel Build(IEnumerable<PublicDivision> publicDivisions)
        {
            return new DivisionIndexViewModel
            {
                Divisions = publicDivisions.Select(pd => DivisionItemViewModel.Build(pd)),
                Breadcrumb = new Breadcrumb
                {
                    BreadcrumbHelper.GetBreadcrumbRootItem(true)
                }
            };
        }
    }
}
