using DevExpress.Data.Filtering;
using DevExpress.Utils;
using DevExpress.Xpo;
using SpbuEducation.TimeTable.BusinessObjects.Contingent;
using SpbuEducation.TimeTable.BusinessObjects.Education;
using SpbuEducation.TimeTable.BusinessObjects.Events;
using SpbuEducation.TimeTable.BusinessObjects.Personnel;
using SpbuEducation.TimeTable.Common.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SpbuEducation.TimeTable.Models;

namespace SpbuEducation.TimeTable.Web.Models
{
    class EducatorEmploymentRepository : IDomainModelRepository<EducatorEmployment>
    {
        private IDomainModelRepository<EducatorEmployment> internalRepository;

        public EducatorEmploymentRepository(IDomainModelRepository<EducatorEmployment> internalRepository)
        {
            Guard.ArgumentNotNull(internalRepository, "internalRepository");
            this.internalRepository = internalRepository;
        }

        public EducatorEmploymentRepository()
            : this(new XpoDomainModelRepository<EducatorEmployment>())
        {
        }

        public EducatorEmployment GetEducatorEmploymentById(int educatorEmploymentId)
        {
            return Session.FindObject<EducatorEmployment>(new BinaryOperator("Id", educatorEmploymentId));
        }

        public EducatorMasterPerson GetEducatorMasterPersonById(int masterId)
        {
            return Session.FindObject<EducatorMasterPerson>(new BinaryOperator("Id", masterId));
        }

        #region IDomainModelRepository<M>
        public IQueryable<EducatorEmployment> GetObjects()
        {
            return internalRepository.GetObjects();
        }

        public EducatorEmployment GetObjectByKey(object key)
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