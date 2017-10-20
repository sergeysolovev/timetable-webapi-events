using DevExpress.Data.Filtering;
using DevExpress.Utils;
using SpbuEducation.TimeTable.BusinessObjects.Education;
using SpbuEducation.TimeTable.Common.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SpbuEducation.TimeTable.Models;

namespace SpbuEducation.TimeTable.Web.Models
{
    class StudyYearRepository : IDomainModelRepository<StudyYear>
    {
        private IDomainModelRepository<StudyYear> internalRepository;

        public StudyYearRepository(IDomainModelRepository<StudyYear> internalRepository)
        {
            Guard.ArgumentNotNull(internalRepository, "internalRepository");
            this.internalRepository = internalRepository;
        }

        public StudyYearRepository()
            : this(new XpoDomainModelRepository<StudyYear>())
        {
        }

        public StudyYear GetCurrentStudyYear()
        {
            return StudyYearHelper.GetDefaultCurrentStudyYear(this.Session);
        }

        public StudyYear GetPreviousStudyYear()
        {
            var currentStudyYear = GetCurrentStudyYear();
            return this.Session.FindObject<StudyYear>(new BinaryOperator("Number", currentStudyYear.Number - 1));
        }

        #region IDomainModelRepository<M>
        public IQueryable<StudyYear> GetObjects()
        {
            return internalRepository.GetObjects();
        }

        public StudyYear GetObjectByKey(object key)
        {
            return internalRepository.GetObjectByKey(key);
        }

        public void Dispose()
        {
            if (this.internalRepository != null)
            {
                this.internalRepository.Dispose();
            }
        }

        public DevExpress.Xpo.Session Session
        {
            get { return internalRepository.Session; }
        }
        #endregion
    }
}