using DevExpress.Data.Filtering;
using DevExpress.Utils;
using DevExpress.Xpo;
using SpbuEducation.TimeTable.BusinessObjects.Contingent;
using SpbuEducation.TimeTable.BusinessObjects.Education;
using SpbuEducation.TimeTable.BusinessObjects.Events;
using SpbuEducation.TimeTable.Common.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SpbuEducation.TimeTable.Models;

namespace SpbuEducation.TimeTable.Web.Models
{
    class StudentGroupRepository : IDomainModelRepository<StudentGroup>
    {
        private IDomainModelRepository<StudentGroup> internalRepository;

        public StudentGroupRepository(IDomainModelRepository<StudentGroup> internalRepository)
        {
            Guard.ArgumentNotNull(internalRepository, "internalRepository");
            this.internalRepository = internalRepository;
        }

        public StudentGroupRepository()
            : this(new XpoDomainModelRepository<StudentGroup>())
        {
        }

        public StudentGroup GetStudentGroupById(int studentGroupId)
        {
            return Session.FindObject<StudentGroup>(new BinaryOperator("Id", studentGroupId));
        }

        #region IDomainModelRepository<M>
        public IQueryable<StudentGroup> GetObjects()
        {
            return internalRepository.GetObjects();
        }

        public StudentGroup GetObjectByKey(object key)
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