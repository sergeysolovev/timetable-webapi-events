using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using SpbuEducation.TimeTable.BusinessObjects.Personnel;
using System.Collections.Generic;
using System.Linq;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.Repositories
{
    internal class EducatorRepository : XpoRepository
    {
        public EducatorRepository(Session session) : base(session)
        {
        }

        public EducatorMasterPerson Get(int id)
        {
            return session.FindObject<EducatorMasterPerson>(
                new BinaryOperator("Id", id)
            );
        }

        public IEnumerable<EducatorMasterPerson> SearchByLastName(string lastNameQuery)
        {
            if (string.IsNullOrEmpty(lastNameQuery))
            {
                return Enumerable.Empty<EducatorMasterPerson>();
            }

            var likeQuery = $"{lastNameQuery}%";
            var criteria = CriteriaOperator.Or(
                new BinaryOperator("DisplayName", likeQuery, BinaryOperatorType.Like),
                new BinaryOperator("DisplayNameEnglish", likeQuery, BinaryOperatorType.Like));

            return new XPCollection<EducatorMasterPerson>(session, criteria,
                new SortProperty("DisplayName", DevExpress.Xpo.DB.SortingDirection.Ascending)
            );
        }
    }
}