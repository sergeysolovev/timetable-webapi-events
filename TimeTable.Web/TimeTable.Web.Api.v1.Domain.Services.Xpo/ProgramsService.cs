using SpbuEducation.TimeTable.BusinessObjects.Contingent;
using SpbuEducation.TimeTable.Helpers.Multilingual;
using SpbuEducation.TimeTable.Web.Api.v1.DataContracts;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Localization;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Abstractions;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.Repositories;
using System;
using System.Linq;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo
{
    internal class ProgramsService : IProgramsService
    {
        private readonly ProgramRepository programRepository;
        private readonly StudyYearRepository studyYearRepository;
        private readonly LanguageCode language;

        public ProgramsService(
            ProgramRepository programRepository,
            StudyYearRepository studyYearRepository,
            LocaleInfo locale)
        {
            this.programRepository = programRepository ??
                throw new ArgumentNullException(nameof(programRepository));

            this.studyYearRepository = studyYearRepository ??
                throw new ArgumentNullException(nameof(studyYearRepository));

            language = locale.Language;
        }

        public ProgramGroupsContract GetGroups(int id)
        {
            var programs = programRepository.GetRelated(id);

            if (programs == null)
            {
                return null;
            }

            var currentStudyYear = studyYearRepository.GetCurrent();

            if (currentStudyYear == null)
            {
                throw new InvalidOperationException("Failed to get the current study year");
            }

            return new ProgramGroupsContract
            {
                Id = id,
                Groups = programs
                    .SelectMany(sp => sp.ContingentUnits)
                    .Where(cu => cu is StudentGroup && !cu.IsArchived)
                    .Cast<StudentGroup>()
                    .OrderBy(sg => sg.StudyPlan?.StudyForm?.Name)
                    .ThenBy(sg => sg.Name)
                    .Where(sg => sg.CurrentStudyYear == currentStudyYear)
                    .Select(sg => new ProgramGroupsContract.Group
                    {
                        Id = sg.Id,
                        Name = sg.Name,
                        Form = sg.StudyPlan?.StudyForm?.GetNameByLanguage(language) ?? string.Empty,
                        Profiles = Trim(sg.GetProfilesStringByLanguage(language), 60)
                    })
            };
        }

        #region Helpers
        private string Trim(string value, int maxLength)
        {
            return (value.Length < maxLength) ? value : $"{value.Substring(0, maxLength)}...";
        } 
        #endregion
    }
}