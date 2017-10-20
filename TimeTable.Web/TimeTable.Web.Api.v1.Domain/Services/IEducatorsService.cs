using SpbuEducation.TimeTable.Web.API.v1.Domain.DataContracts;

namespace SpbuEducation.TimeTable.Web.API.v1.Domain.Services
{
    public interface IEducatorsService
    {
        EducatorEventsContract GetEvents(int educatorId, int? showNextTerm);
        EducatorsContract SearchByLastName(string lastNameQuery);
    }
}