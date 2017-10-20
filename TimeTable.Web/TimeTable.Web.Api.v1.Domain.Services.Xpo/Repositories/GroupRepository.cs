using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using SpbuEducation.TimeTable.BusinessObjects.Contingent;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.Repositories
{
    internal class GroupRepository : XpoRepository
    {
        public GroupRepository(Session session) : base(session)
        {
        }

        public StudentGroup Get(int id)
        {
            return session.FindObject<StudentGroup>(
                new BinaryOperator("Id", id)
            );
        }
    }
}