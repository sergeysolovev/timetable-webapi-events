using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using SpbuEducation.TimeTable.BusinessObjects.Education;
using System.Linq;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.Repositories
{
    internal class StudyYearRepository : XpoRepository
    {
        public StudyYearRepository(Session session) : base(session)
        {
        }

        public StudyYear GetCurrent()
        {
            var currentStudyYears = new XPCollection<StudyYear>(session,
                new BinaryOperator("IsCurrent", true),
                new SortProperty("Number", SortingDirection.Descending))
            {
                TopReturnedObjects = 1
            };

            return currentStudyYears.FirstOrDefault();
        }
    }
}