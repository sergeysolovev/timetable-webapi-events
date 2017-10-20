using DevExpress.Xpo;
using SpbuEducation.TimeTable.BusinessObjects.Education;
using System.Collections.Generic;
using System.Linq;

namespace SpbuEducation.TimeTable.Web.Models
{
    public class StudyProgramNameLevelCombination
    {
        public string Name { get; set; }
        public string StudyLevelName { get; set; }
        public IEnumerable<StudyProgram> StudyPrograms { get; set; }

        public Session Session
        {
            get
            {
                if (StudyPrograms != null && StudyPrograms.Any())
                {
                    return StudyPrograms.First().Session;
                }
                return null;
            }
        }
    }
}