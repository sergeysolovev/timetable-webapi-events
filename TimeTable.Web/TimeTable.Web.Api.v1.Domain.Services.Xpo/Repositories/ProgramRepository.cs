using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using System.Collections.Generic;
using SpbuEducation.TimeTable.BusinessObjects.Education;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.Repositories
{
    internal class ProgramRepository : XpoRepository
    {
        public ProgramRepository(Session session) : base(session)
        {
        }

        public StudyProgram Get(int id)
        {
            return session.FindObject<StudyProgram>(
                new BinaryOperator("Id", id)
            );
        }

        public IEnumerable<StudyProgram> GetRelated(int id)
        {
            var source = Get(id);

            if (source == null)
            {
                return null;
            }

            return new XPCollection<StudyProgram>(session, CriteriaOperator.And(
                new BinaryOperator("Name", source.Name),
                new BinaryOperator("AdmissionYear", source.AdmissionYear),
                new BinaryOperator("StudyLevel", source.StudyLevel))
            );
        }
    }
}