using SpbuEducation.TimeTable.BusinessObjects.Contingent;
using SpbuEducation.TimeTable.Helpers.Multilingual;
using SpbuEducation.TimeTable.Web.Api.v1.Localization;
using SpbuEducation.TimeTable.Web.Api.v1.DataContracts;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Localization;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Abstractions;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo
{
    internal class StudyDivisionsService : IStudyDivisionsService
    {
        private readonly LanguageCode language;
        private readonly CultureInfo culture;
        private readonly DivisionRepository divisionsRepository;
        private readonly StudyYearRepository studyYearRepository;

        public StudyDivisionsService(
            DivisionRepository divisionsRepository,
            StudyYearRepository studyYearRepository,
            LocaleInfo locale)
        {
            this.divisionsRepository = divisionsRepository ??
                throw new ArgumentNullException(nameof(divisionsRepository));

            this.studyYearRepository = studyYearRepository ??
                throw new ArgumentNullException(nameof(studyYearRepository));

            this.language = locale?.Language ??
                throw new ArgumentNullException(nameof(locale));

            this.culture = locale?.Culture ??
                throw new ArgumentNullException(nameof(locale));
        }

        public IEnumerable<StudyDivisionContract> Get()
        {
            return divisionsRepository
                .Get()
                .Select(d => new StudyDivisionContract
                {
                    Oid = d.Oid,
                    Name = d.GetNameByLanguage(language),
                    Alias = d.Alias
                })
                .OrderBy(d => d.Name);
        }

        public StudyDivisionContract Get(string alias)
        {
            var division = divisionsRepository.Get(alias);
            if (division == null)
            {
                return null;
            }

            return new StudyDivisionContract
            {
                Oid = division.Oid,
                Name = division.GetNameByLanguage(language),
                Alias = division.Alias
            };
        }

        public IEnumerable<StudyDivisionProgramLevelContract> GetProgramLevels(string alias)
        {
            var division = divisionsRepository.Get(alias);

            if (division == null)
            {
                return null;
            }

            var titlePreamble = division.Name.Contains(",") ?
                Resources.Divisions :
                Resources.Division;

            var divisionTitle = $"{titlePreamble} {division.GetNameByLanguage(language)}";
            var currentStudyYear = studyYearRepository.GetCurrent();

            if (currentStudyYear == null)
            {
                throw new InvalidOperationException("Failed to get the current study year");
            }

            return division
                .StudentGroups
                .Where(sg => sg.StudyProgram != null)
                .Select(sg => sg.StudyProgram)
                .Distinct()
                .GroupBy(sp => new
                {
                    ProgramName = sp.GetNameByLanguage(language),
                    ProgramNameEnglish = sp.NameEnglish,
                    LevelName = sp.StudyLevel.GetNameByLanguage(language),
                    LevelNameEnglish = sp.StudyLevel.NameEnglish
                })
                .Select(g => new
                {
                    ProgramName = g.Key.ProgramName,
                    ProgramNameEnglish = g.Key.ProgramNameEnglish,
                    LevelName = g.Key.LevelName,
                    LevelNameEnglish = g.Key.LevelNameEnglish,
                    Programs = g.AsEnumerable()
                })
                .GroupBy(x => new { x.LevelName, x.LevelNameEnglish })
                .Select(g => new StudyDivisionProgramLevelContract
                {
                    Name = culture.TextInfo.ToTitleCase(g.Key.LevelName.ToLower()),
                    NameEnglish = g.Key.LevelNameEnglish,
                    // TODO: удалить неактуальный HasCourse6 
                    // при выпуске новой версии API
                    HasCourse6 = g.AsEnumerable()
                        .SelectMany(c => c.Programs)
                        .Select(sp => sp.AdmissionYear)
                        .Any(ay => (currentStudyYear.Number - ay.Number + 1) == 6),
                    ProgramCombinations = g.AsEnumerable()
                        .Select(c => new StudyDivisionProgramLevelContract.ProgramCombination
                        {
                            Name = c.ProgramName,
                            NameEnglish = c.ProgramNameEnglish,
                            Years = c.Programs
                                .Where(sp => sp.ContingentUnits.Any(cu => cu is StudentGroup && !cu.IsArchived))
                                .GroupBy(sp => sp.AdmissionYear)
                                .Select(spg => spg.First())
                                .Select(sp => new StudyDivisionProgramLevelContract.Year
                                {
                                    ProgramId = sp.Id,
                                    Name = sp.AdmissionYear.Name,
                                    Number = sp.AdmissionYear.Number,
                                    DivisionAlias = division.Alias,
                                    IsEmpty = false
                                })
                                .OrderByDescending(ay => ay.Number)
                        })
                        .OrderBy(pc => pc.Name)
                })
                .OrderBy(c => c.Name);
        }
    }
}