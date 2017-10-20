using SpbuEducation.TimeTable.BusinessObjects.Personnel;
using SpbuEducation.TimeTable.Common.Web.Helpers;
using SpbuEducation.TimeTable.Common.Web.ViewModels;
using SpbuEducation.TimeTable.Helpers.Multilingual;
using System;
using System.Text.RegularExpressions;

namespace SpbuEducation.TimeTable.Web.ViewModels
{
    public class EducatorEmploymentItemViewModel : IViewModel
    {
        public string Position { get; private set; }
        public string Department { get; private set; }

        public static EducatorEmploymentItemViewModel Build(EducatorEmployment educatorEmployment)
        {
            var language = CultureHelper.CurrentLanguage;
            var position = GetPositionShort(educatorEmployment, language);
            var formatedPosition = Regex.Replace(position, @"[\d-]", string.Empty);
            return new EducatorEmploymentItemViewModel
            {
                Position = formatedPosition,
                Department = GetSapDepartment(educatorEmployment),
            };
        }

        private static string GetSapDepartment(EducatorEmployment educatorEmployment)
        {
            return educatorEmployment.SapDepartment == null ?
                string.Empty :
                educatorEmployment.SapDepartment.Name;
        }

        private static string GetPositionShort(EducatorEmployment educatorEmployment, LanguageCode language)
        {
            return educatorEmployment.PositionShort == null ?
                string.Empty :
                educatorEmployment.PositionShort.GetNameByLanguage(language) ?? string.Empty;
        }
    }
}