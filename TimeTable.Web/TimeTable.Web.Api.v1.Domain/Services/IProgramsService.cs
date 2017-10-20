using SpbuEducation.TimeTable.Web.API.v1.Domain.DataContracts;

namespace SpbuEducation.TimeTable.Web.API.v1.Domain.Services
{
    public interface IProgramsService
    {
        ProgramGroupsContract GetGroups(int id);
    }
}