using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using SpbuEducation.TimeTable.BusinessObjects.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.Repositories
{
    internal class ClassroomRepository : XpoRepository
    {
        public ClassroomRepository(Session session) : base(session)
        {
        }

        public Classroom Get(Guid oid)
        {
            return session.GetObjectByKey<Classroom>(oid);
        }

        public IEnumerable<Classroom> Get(
            Guid addressId,
            IEnumerable<string> equipmentElements,
            SeatingType? seating = default(SeatingType?),
            int? capacity = default(int?))
        {
            var address = session.GetObjectByKey<Address>(addressId);
            if (address == null)
            {
                return null;
            }

            return GetByCriteria(addressId, equipmentElements, seating, capacity);
        }

        public IEnumerable<Classroom> Get(
            IEnumerable<string> equipmentElements,
            SeatingType? seating = default(SeatingType?),
            int? capacity = default(int?))
        {
            return GetByCriteria(Guid.Empty, equipmentElements, seating, capacity);
        }

        private IEnumerable<Classroom> GetByCriteria(
            Guid addressOid,
            IEnumerable<string> equipmentElements,
            SeatingType? seating = default(SeatingType?),
            int? capacity = default(int?))
        {
            var criteria = new CriteriaOperatorCollection();

            if (addressOid != Guid.Empty)
            {
                criteria.Add(
                    new BinaryOperator("Address", addressOid, BinaryOperatorType.Equal)
                );
            }

            if (equipmentElements?.Any() ?? false)
            {
                criteria.Add(CriteriaOperator.Or(
                    equipmentElements.Select(e => new BinaryOperator(
                        "AdditionalInfo",
                        $"%{e}%",
                        BinaryOperatorType.Like
                    ))
                ));
            }

            if (capacity.HasValue)
            {
                criteria.Add(
                    new BinaryOperator(
                        "Capacity",
                        capacity.Value,
                        BinaryOperatorType.GreaterOrEqual
                    )
                );
            }

            if (seating.HasValue)
            {
                criteria.Add(new BinaryOperator("SeatingType", seating.Value));
            }

            return criteria.Any() ?
                new XPCollection<Classroom>(session, CriteriaOperator.And(criteria)) :
                new XPCollection<Classroom>(session);
        }
    }
}