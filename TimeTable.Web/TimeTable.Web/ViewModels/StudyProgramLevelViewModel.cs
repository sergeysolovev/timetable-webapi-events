using SpbuEducation.TimeTable.BusinessObjects.Education;
using SpbuEducation.TimeTable.Common.Web.ViewModels;
using SpbuEducation.TimeTable.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpbuEducation.TimeTable.Web.ViewModels
{
    public class  StudyProgramLevelViewModel : IViewModel
    {
        public string StudyLevelName { get; set; }
        public IEnumerable<StudyProgramCombinationViewModel> StudyProgramCombinations { get; set; }
        public bool IsMagistery =>
            String.Equals(StudyLevelName, "master studies", StringComparison.CurrentCultureIgnoreCase) ||
            String.Equals(StudyLevelName, "магистратура", StringComparison.CurrentCultureIgnoreCase);

        public static StudyProgramLevelViewModel Build(string studyLevelName, IEnumerable<StudyProgramNameLevelCombination> studyProgramCombinations, PublicDivision publicDivision)
        {
            return new StudyProgramLevelViewModel
            {
                StudyLevelName = CapitalizeFirstLetter(studyLevelName),
                StudyProgramCombinations = studyProgramCombinations
                    .Select(x => StudyProgramCombinationViewModel.Build(x, publicDivision))
                    .OrderBy(sp => sp.Name)
            };
        }

        private static string CapitalizeFirstLetter(string inputString)
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(inputString.ToLower());
        }
    }
}

