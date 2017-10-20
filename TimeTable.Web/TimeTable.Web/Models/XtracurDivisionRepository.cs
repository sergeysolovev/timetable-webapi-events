using DevExpress.Data.Filtering;
using DevExpress.Utils;
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
    class XtracurDivisionRepository : IDomainModelRepository<XtracurDivision>
    {
        private IDomainModelRepository<XtracurDivision> internalRepository;

        public XtracurDivisionRepository(IDomainModelRepository<XtracurDivision> internalRepository)
        {
            Guard.ArgumentNotNull(internalRepository, "internalRepository");
            this.internalRepository = internalRepository;
        }

        public XtracurDivisionRepository()
            : this(new XpoDomainModelRepository<XtracurDivision>())
        {
        }

        public IQueryable<XtracurDivision> GetXtracurDivisions()
        {
            return GetObjects().Where(d => d.IsPublic).OrderBy(d => d.Name);
        }

        public XtracurDivision GetXtracurDivisionByAlias(string alias)
        {
            return Session.FindObject<XtracurDivision>(new BinaryOperator("Alias", alias));
        }

        #region IDomainModelRepository<M>
        public IQueryable<XtracurDivision> GetObjects()
        {
            return internalRepository.GetObjects();
        }

        public XtracurDivision GetObjectByKey(object key)
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