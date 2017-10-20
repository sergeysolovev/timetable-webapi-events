using System;
using SpbuEducation.TimeTable.BusinessObjects.Education;
using SpbuEducation.TimeTable.Common.Web.ViewModels;
using SpbuEducation.TimeTable.Common.Web.Helpers;

namespace SpbuEducation.TimeTable.Web.ViewModels
{
    public class DivisionItemViewModel : IViewModel
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public Guid Oid { get; set; }

        public static DivisionItemViewModel Build(PublicDivision publicDivision)
        {
            return new DivisionItemViewModel
            {
                Oid = publicDivision.Oid,
                Name = publicDivision.GetNameByLanguage(CultureHelper.CurrentLanguage),
                Alias = publicDivision.Alias
            };
        }
    }
}
