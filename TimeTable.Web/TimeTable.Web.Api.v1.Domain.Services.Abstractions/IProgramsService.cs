using SpbuEducation.TimeTable.Web.Api.v1.DataContracts;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Abstractions
{
    /// <summary>
    /// Programs Service
    /// </summary>
    public interface IProgramsService
    {
        /// <summary>
        /// Gets student groups for a program
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ProgramGroupsContract GetGroups(int id);
    }
}