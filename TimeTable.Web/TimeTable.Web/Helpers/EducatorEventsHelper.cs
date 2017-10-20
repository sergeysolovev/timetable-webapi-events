using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using SpbuEducation.TimeTable.BusinessObjects.Personnel;
using SpbuEducation.TimeTable.Web.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SpbuEducation.TimeTable.Web.Helpers
{
    public class EducatorEventsHelper
    {
        public static IEnumerable<EducatorItemViewModel> SearchEducatorsByLastName(
            Session session, string educatorLastNameQuery)
        {
            if (!string.IsNullOrEmpty(educatorLastNameQuery))
            {
                string likeQuery = $"{educatorLastNameQuery}%";
                var criteria = BinaryOperator.Or(
                    new BinaryOperator("DisplayName", likeQuery, BinaryOperatorType.Like),
                    new BinaryOperator("DisplayNameEnglish", likeQuery, BinaryOperatorType.Like));
                var searchedPersons = new XPCollection<EducatorMasterPerson>(session, criteria,
                    new SortProperty("DisplayName", DevExpress.Xpo.DB.SortingDirection.Ascending));
                return searchedPersons.Select(mp => EducatorItemViewModel.Build(mp));
            }
            return null;
        }
    }
}