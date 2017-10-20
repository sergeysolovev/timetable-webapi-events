using SpbuEducation.TimeTable.BusinessObjects.Contingent;
using SpbuEducation.TimeTable.BusinessObjects.Education;
using SpbuEducation.TimeTable.Common.Web.ViewModels;
using SpbuEducation.TimeTable.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpbuEducation.TimeTable.Web.ViewModels
{
    public class StudyProgramCombinationViewModel : IViewModel
    {
        public string Name { get; set; }
        public List<AdmissionYearItemViewModel> AdmissionYears { get; set; }
        public bool IsBiology =>
            String.Equals(Name, "Biology", StringComparison.CurrentCultureIgnoreCase) ||
            String.Equals(Name, "Биология", StringComparison.CurrentCultureIgnoreCase);

        public static StudyProgramCombinationViewModel Build(StudyProgramNameLevelCombination studyProgramCombination, PublicDivision publicDivision)
        {
            var admissionYears = studyProgramCombination.StudyPrograms
                .Where(sp => sp.ContingentUnits.Any(cu => cu is StudentGroup && !cu.IsArchived))
                .GroupBy(sp => sp.AdmissionYear)
                .Select(g => AdmissionYearItemViewModel.Build(g.AsEnumerable(), publicDivision))
                .OrderByDescending(x => x.YearNumber)
                .ToList();

            return new StudyProgramCombinationViewModel
            {
                Name = studyProgramCombination.Name,
                AdmissionYears = admissionYears
            };
        }
    }
}

