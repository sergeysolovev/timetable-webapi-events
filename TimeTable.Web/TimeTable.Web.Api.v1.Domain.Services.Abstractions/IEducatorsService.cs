using SpbuEducation.TimeTable.Web.Api.v1.DataContracts;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Abstractions
{
    /// <summary>
    /// Educators Service
    /// </summary>
    public interface IEducatorsService
    {
        /// <summary>
        /// Gets events for an educator for the current or the next study term
        /// </summary>
        /// <param name="educatorId"></param>
        /// <param name="showNextTerm"></param>
        /// <returns></returns>
        EducatorEventsContract GetEvents(int educatorId, int? showNextTerm);

        /// <summary>
        /// Searches educators by last name query
        /// </summary>
        /// <param name="lastNameQuery"></param>
        /// <returns></returns>
        EducatorsContract SearchByLastName(string lastNameQuery);
    }
}