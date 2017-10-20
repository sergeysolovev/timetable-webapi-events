using SpbuEducation.TimeTable.BusinessObjects.RealEstate;
using SpbuEducation.TimeTable.Web.Api.v1.DataContracts;
using System;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.Mappers
{
    internal class SeatingMapper
    {
        public SeatingMapper()
        {
        }

        public SeatingType? Map(Seating? seating)
        {
            switch (seating)
            {
                case null:
                    return null;
                case Seating.Theater:
                    return SeatingType.Theater;
                case Seating.Amphitheater:
                    return SeatingType.Amphitheater;
                case Seating.RoundTable:
                    return SeatingType.RoundTable;
                default:
                    throw new ArgumentOutOfRangeException(nameof(seating));
            }
        }
    }
}
