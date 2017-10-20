using System.Collections.Generic;
using SpbuEducation.TimeTable.Web.Api.v1.DataContracts;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Abstractions
{
    /// <summary>
    /// Study Divisions Service
    /// </summary>
    public interface IStudyDivisionsService
    {
        /// <summary>
        /// Gets study divisions
        /// </summary>
        /// <returns></returns>
        IEnumerable<StudyDivisionContract> Get();

        /// <summary>
        /// Gets a study division by alias
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        StudyDivisionContract Get(string alias);

        /// <summary>
        /// Gets study program levels for a division
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        IEnumerable<StudyDivisionProgramLevelContract> GetProgramLevels(string alias);
    }
}