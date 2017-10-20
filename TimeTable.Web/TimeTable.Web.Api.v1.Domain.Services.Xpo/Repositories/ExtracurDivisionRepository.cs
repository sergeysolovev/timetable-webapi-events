using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using SpbuEducation.TimeTable.BusinessObjects.Events;
using System.Collections.Generic;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.Repositories
{
    internal class ExtracurDivisionRepository : XpoRepository
    {
        public ExtracurDivisionRepository(Session session) : base(session)
        {
        }

        public IEnumerable<XtracurDivision> Get()
        {
            return new XPCollection<XtracurDivision>(session,
                new BinaryOperator("IsPublic", true),
                new SortProperty("Name", SortingDirection.Ascending)
            );
        }

        public XtracurDivision Get(string alias)
        {
            return session.FindObject<XtracurDivision>(
                new BinaryOperator("Alias", alias)
            );
        }
    }
}