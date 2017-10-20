using DevExpress.Data.Filtering;
using DevExpress.Utils;
using DevExpress.Xpo;
using SpbuEducation.TimeTable.BusinessObjects.Contingent;
using SpbuEducation.TimeTable.BusinessObjects.Education;
using SpbuEducation.TimeTable.Common.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SpbuEducation.TimeTable.Models;

namespace SpbuEducation.TimeTable.Web.Models
{
    class StudyProgramRepository : IDomainModelRepository<StudyProgram>
    {
        private IDomainModelRepository<StudyProgram> internalRepository;

        public StudyProgramRepository(IDomainModelRepository<StudyProgram> internalRepository)
        {
            Guard.ArgumentNotNull(internalRepository, "internalRepository");
            this.internalRepository = internalRepository;
        }

        public StudyProgramRepository()
            : this(new XpoDomainModelRepository<StudyProgram>())
        {
        }

        public StudyProgram GetStudyProgramById(int id)
        {
            return Session.FindObject<StudyProgram>(new BinaryOperator("Id", id));
        }

        public IEnumerable<StudyProgram> GetStudyPrograms(IEnumerable<int> ids)
        {
            return new XPCollection<StudyProgram>(Session, new InOperator("Id", ids));
        }

        public IQueryable<StudyProgram> GetStudyPrograms()
        {
            return GetObjects().OrderBy(sp => sp.Name);
        }

        public StudyProgram GetStudyProgramByKey(object key)
        {
            return GetObjectByKey(key);
        }

        #region IDomainModelRepository<M>
        public IQueryable<StudyProgram> GetObjects()
        {
            return internalRepository.GetObjects();
        }

        public StudyProgram GetObjectByKey(object key)
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