using SpbuEducation.TimeTable.BusinessObjects.Education;
using SpbuEducation.TimeTable.Common.Web.Breadcrumb;
using SpbuEducation.TimeTable.Common.Web.Helpers;
using SpbuEducation.TimeTable.Web.Helpers;
using SpbuEducation.TimeTable.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpbuEducation.TimeTable.Web.ViewModels
{
    public class DivisionShowViewModel : BreadcrumbViewModel
    {
        private static string alias;
        public string Title { get; set; }
        public string StudyProgramsTitle { get; set; }
        public IEnumerable<StudyProgramLevelViewModel> StudyProgramLevels { get; set; }
        public string Alias => alias;

        public static DivisionShowViewModel Build(PublicDivision publicDivision)
        {
            alias = publicDivision.Alias;
            var publicDivisionStudentGroups = publicDivision.StudentGroups;
            var language = CultureHelper.CurrentLanguage;

            var studyProgramNameLevelCombinations = publicDivisionStudentGroups.Where(sg => sg.StudyProgram != null)
                .Select(sg => sg.StudyProgram)
                .Distinct()
                .GroupBy(sp => new {
                    Name = sp.GetNameByLanguage(language),
                    StudyLevelName = sp.StudyLevel.GetNameByLanguage(language),
                })
                .Select(g => new StudyProgramNameLevelCombination
                {
                    Name = g.Key.Name,
                    StudyLevelName = g.Key.StudyLevelName,
                    StudyPrograms = g.AsEnumerable()
                })
                .OrderBy(x => x.StudyLevelName);

            return new DivisionShowViewModel
            {
                Title = GetTitle(publicDivision),
                StudyProgramsTitle = GetStudyProgramsTitle(studyProgramNameLevelCombinations),
                StudyProgramLevels = studyProgramNameLevelCombinations
                    .GroupBy(x => x.StudyLevelName)
                    .Select(g => StudyProgramLevelViewModel.Build(g.Key, g, publicDivision)),
                Breadcrumb = new Breadcrumb()
                {
                    BreadcrumbHelper.GetBreadcrumbRootItem(false),
                    new BreadcrumbItem
                    {
                        IsActive = true,
                        DisplayText = publicDivision.GetNameByLanguage(language)
                    }
                }
            };
        }

        private static string GetTitle(PublicDivision publicDivision)
        {
            var language = CultureHelper.CurrentLanguage;
            string preamble = GetPreambleForTitle(publicDivision);
            return String.Concat(preamble, " ", publicDivision.GetNameByLanguage(language));
        }

        private static string GetPreambleForTitle(PublicDivision publicDivision)
        {
            bool isPlural = publicDivision.Name.Contains(",");
            if (isPlural)
            {
                return Resources.Resources.Divisions;
            }
            else
            {
                return Resources.Resources.Division;
            }
        }

        private static string GetStudyProgramsTitle(IEnumerable<StudyProgramNameLevelCombination> studyProgramNameLevelGroupings)
        {
            if (studyProgramNameLevelGroupings.Any())
            {
                return "";
            }
            else
            {
                return Resources.Resources.TimetablesUnavailable;
            }
        }
    }
}
