using SpbuEducation.TimeTable.Web.Models;
using SpbuEducation.TimeTable.Web.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SpbuEducation.TimeTable.Web.Controllers
{
    public class StudyProgramController : BaseController
    {
        private readonly StudyProgramRepository studyProgramRepository;
        private readonly PublicDivisionRepository publicDivisionRepository;

        public StudyProgramController()
        {
            studyProgramRepository = new StudyProgramRepository();
            publicDivisionRepository = new PublicDivisionRepository();
        }

        public ActionResult Show(string publicDivisionAlias, IEnumerable<int> id)
        {
            var studyPrograms = studyProgramRepository.GetStudyPrograms(id);
            var publicDivision = publicDivisionRepository.GetPublicDivisionByAlias(publicDivisionAlias);
            
            #region Here comes Pizdec
            var studyProgram = studyPrograms.First();
            if (publicDivision.Alias == "BIOL"  && studyProgram.Name == "Биология" && studyProgram.StudyLevel.Name == "магистратура" && studyProgram.AdmissionYear.Number >= 2016)
            {
                return RedirectToRoute("XtracurEvents.Index", new { alias = "BIOL" });
            }
            #endregion

            var studyProgramStudentGroupsViewModel = StudyProgramShowViewModel.Build(studyPrograms, publicDivision);
            return View(studyProgramStudentGroupsViewModel);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (studyProgramRepository != null)
                {
                    studyProgramRepository.Dispose();
                }
                if (publicDivisionRepository != null)
                {
                    publicDivisionRepository.Dispose();
                }
            }
        }
    }
}
