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
    class EventRepository : IDomainModelRepository<Event>
    {
        private IDomainModelRepository<Event> internalRepository;

        public EventRepository(IDomainModelRepository<Event> internalRepository)
        {
            Guard.ArgumentNotNull(internalRepository, "internalRepository");
            this.internalRepository = internalRepository;
        }

        public EventRepository()
            : this(new XpoDomainModelRepository<Event>())
        {
        }

        public Event GetEventById(int eventId)
        {
            return Session.FindObject<Event>(new BinaryOperator("Id", eventId));
        }

        #region IDomainModelRepository<M>
        public IQueryable<Event> GetObjects()
        {
            return internalRepository.GetObjects();
        }

        public Event GetObjectByKey(object key)
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