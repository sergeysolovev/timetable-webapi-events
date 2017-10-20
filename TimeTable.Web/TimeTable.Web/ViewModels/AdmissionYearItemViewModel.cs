using SpbuEducation.TimeTable.BusinessObjects.Education;
using SpbuEducation.TimeTable.Common.Web.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SpbuEducation.TimeTable.Web.ViewModels
{
    public class AdmissionYearItemViewModel : IViewModel
    {
        public string PublicDivisionAlias { get; set; }
        public IEnumerable<int> StudyProgramIds { get; set; }
        public string YearName { get; set; }
        public int YearNumber { get; set; }
        public bool IsEmpty { get; set; }

        public static AdmissionYearItemViewModel Empty
        {
            get
            {
                return new AdmissionYearItemViewModel { IsEmpty = true };
            }
        }

        public static AdmissionYearItemViewModel Build(
            IEnumerable<StudyProgram> studyPrograms,
            PublicDivision publicDivision)
        {
            var firstProgram = studyPrograms?.FirstOrDefault();

            if (firstProgram == null)
            {
                return Empty;
            }

            return new AdmissionYearItemViewModel
            {
                PublicDivisionAlias = publicDivision.Alias,
                StudyProgramIds = studyPrograms.Select(p => p.Id),
                YearName = firstProgram.AdmissionYear.Name,
                YearNumber = firstProgram.AdmissionYear.Number
            };
        }
    }
}

