using System.Collections.Generic;
using DevExpress.Xpo;
using SpbuEducation.TimeTable.Common.Web.Breadcrumb;
using SpbuEducation.TimeTable.Web.Helpers;

namespace SpbuEducation.TimeTable.Web.ViewModels
{
    public class EducatorEventsIndexViewModel : BreadcrumbViewModel
    {
        public string EducatorLastNameQuery { get; private set; }
        public IEnumerable<EducatorItemViewModel> Educators { get; private set; }

        public static EducatorEventsIndexViewModel Build(Session session, string educatorLastNameQuery)
        {
            return new EducatorEventsIndexViewModel
            {
                EducatorLastNameQuery = educatorLastNameQuery,
                Educators = EducatorEventsHelper.SearchEducatorsByLastName(session, educatorLastNameQuery),
                Breadcrumb = new Breadcrumb()
                {
                    BreadcrumbHelper.GetBreadcrumbRootItem(false),
                    BreadcrumbHelper.GetBreadcrumbEducatorsItem(true),
                }
            };
        }
    }
}
