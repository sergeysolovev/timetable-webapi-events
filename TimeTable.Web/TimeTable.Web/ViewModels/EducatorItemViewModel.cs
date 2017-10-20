using SpbuEducation.TimeTable.BusinessObjects.Personnel;
using SpbuEducation.TimeTable.Common.Web.Helpers;
using SpbuEducation.TimeTable.Common.Web.ViewModels;
using SpbuEducation.TimeTable.Helpers.Multilingual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpbuEducation.TimeTable.Web.ViewModels
{
    public class EducatorItemViewModel : IViewModel
    {
        public int Id { get; private set; }
        public string DisplayName { get; private set; }
        public IEnumerable<EducatorEmploymentItemViewModel> Employments { get; private set; }

        private class EducatorEmploymentItemViewModelEqualityComparer : IEqualityComparer<EducatorEmploymentItemViewModel>
        {
            public bool Equals(EducatorEmploymentItemViewModel x, EducatorEmploymentItemViewModel y)
            {
                return string.Equals(x.Department, y.Department) && string.Equals(x.Position, y.Position);
            }

            public int GetHashCode(EducatorEmploymentItemViewModel obj)
            {
                return obj.Department.GetHashCode() + obj.Position.GetHashCode();
            }
        }

        public static EducatorItemViewModel Build(EducatorMasterPerson masterPerson)
        {
            var language = CultureHelper.CurrentLanguage;
            return new EducatorItemViewModel
            {
                Id = masterPerson.Id,
                DisplayName = masterPerson.GetDisplayNameByLanguage(language),
                FullName = $"{masterPerson.LastName} {masterPerson.FirstName} {masterPerson.MiddleName}",
                Employments = masterPerson.Persons
                    .SelectMany(p => p.EducatorEmployments)
                    .Select(ee => EducatorEmploymentItemViewModel.Build(ee))
                    .Distinct(new EducatorEmploymentItemViewModelEqualityComparer())
            };
        }

        public string FullName { get; set; }
    }
}