using System.Collections.Generic;
using SpbuEducation.TimeTable.Web.API.v1.Domain.DataContracts;

namespace SpbuEducation.TimeTable.Web.API.v1.Domain.Services
{
    public interface IStudyDivisionsService
    {
        IEnumerable<StudyDivisionContract> Get();
        StudyDivisionContract Get(string alias);
        IEnumerable<StudyDivisionProgramLevelContract> GetProgramLevels(string alias);
    }
}