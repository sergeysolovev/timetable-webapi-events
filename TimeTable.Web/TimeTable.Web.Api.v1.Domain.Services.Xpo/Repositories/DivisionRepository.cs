using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using SpbuEducation.TimeTable.BusinessObjects.Education;
using System.Collections.Generic;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.Repositories
{
    internal class DivisionRepository : XpoRepository
    {
        public DivisionRepository(Session session) : base(session)
        {
        }

        public IEnumerable<PublicDivision> Get()
        {
            return new XPCollection<PublicDivision>(session)
            {
                Sorting = { new SortProperty("Name", SortingDirection.Ascending) }
            };
        }

        public PublicDivision Get(string alias)
        {
            return session.FindObject<PublicDivision>(
                new BinaryOperator("Alias", alias)
            );
        }
    }
}