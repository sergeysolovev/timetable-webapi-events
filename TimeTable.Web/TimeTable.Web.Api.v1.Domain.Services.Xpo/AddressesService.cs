using SpbuEducation.TimeTable.Web.Api.v1.DataContracts;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Abstractions;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.Mappers;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo
{
    internal class AddressesService : IAddressesService
    {
        private readonly ClassroomRepository classroomRepository;
        private readonly SeatingMapper seatingMapper;

        public AddressesService(
            ClassroomRepository classroomRepository,
            SeatingMapper seatingMapper)
        {
            this.classroomRepository = classroomRepository ??
                throw new ArgumentNullException(nameof(classroomRepository));

            this.seatingMapper = seatingMapper ??
                throw new ArgumentNullException(nameof(seatingMapper));
        }

        public IEnumerable<ClassroomContract> GetClassrooms(
            Guid oid,
            IEnumerable<string> equipmentElements,
            Seating? seating = default(Seating?),
            int? capacity = default(int?))
        {
            var seatingType = seatingMapper.Map(seating);
            var classrooms = classroomRepository.Get(oid, equipmentElements, seatingType, capacity);

            if (classrooms == null)
            {
                return null;
            }

            return classrooms
                .Select(c => new ClassroomContract
                {
                    Oid = c.Oid,
                    DisplayName = c.DisplayName1,
                    SeatingType = (int)c.SeatingType,
                    Capacity = c.Capacity,
                    AdditionalInfo = c.AdditionalInfo,
                    Equipment = equipmentElements.Any() ?
                        string.Join("; ", equipmentElements.Where(e =>
                            c.AdditionalInfo.IndexOf(e, StringComparison.OrdinalIgnoreCase) < 0)
                        ) :
                        null
                })
                .OrderBy(c => c?.Equipment?.Length ?? 0);
        }

        public IEnumerable<AddressContract> Get(
            IEnumerable<string> equipmentElements,
            Seating? seating = default(Seating?),
            int? capacity = default(int?))
        {
            var seatingType = seatingMapper.Map(seating);
            var classrooms = classroomRepository.Get(equipmentElements, seatingType, capacity);

            return classrooms
                .Select(c => c.Address)
                .Distinct()
                .Select(a => new AddressContract
                {
                    Oid = a.Oid,
                    DisplayName = a.DisplayName1,
                    Matches = classrooms.Count(c => c.Address.Oid == a.Oid),
                    Equipment = equipmentElements.Any() ?
                        string.Join("; ",
                            equipmentElements.Where(e => classrooms
                                .Where(c => c.Address.Oid == a.Oid)
                                .All(c => c.AdditionalInfo.IndexOf(e, StringComparison.OrdinalIgnoreCase) < 0)
                            )
                        ) :
                        null
                })
                .OrderByDescending(a => a.Matches);
        }
    }
}