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
    class PublicDivisionRepository : IDomainModelRepository<PublicDivision>
    {
        private IDomainModelRepository<PublicDivision> internalRepository;

        public PublicDivisionRepository(IDomainModelRepository<PublicDivision> internalRepository)
        {
            Guard.ArgumentNotNull(internalRepository, "internalRepository");
            this.internalRepository = internalRepository;
        }

        public PublicDivisionRepository()
            : this(new XpoDomainModelRepository<PublicDivision>())
        {
        }

        public IQueryable<PublicDivision> GetPublicDivisions()
        {
            return GetObjects().OrderBy(d => d.Name);
        }

        public PublicDivision GetPublicDivisionByKey(object key)
        {
            return GetObjectByKey(key);
        }

        public PublicDivision GetPublicDivisionByAlias(string alias)
        {
            return Session.FindObject<PublicDivision>(new BinaryOperator("Alias", alias));
        }

        #region IDomainModelRepository<M>
        public IQueryable<PublicDivision> GetObjects()
        {
            return internalRepository.GetObjects();
        }

        public PublicDivision GetObjectByKey(object key)
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