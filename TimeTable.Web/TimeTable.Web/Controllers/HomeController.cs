using SpbuEducation.TimeTable.Web.Models;
using SpbuEducation.TimeTable.Web.ViewModels;
using System.Web.Mvc;

namespace SpbuEducation.TimeTable.Web.Controllers
{
    public class HomeController : BaseController
    {
        private PublicDivisionRepository publicDivisionRepository;
        private XtracurDivisionRepository xtracurDivisionRepository;

        public HomeController()
        {
            this.publicDivisionRepository = new PublicDivisionRepository();
            this.xtracurDivisionRepository = new XtracurDivisionRepository();
        }

        //
        // GET: /Divisions/
        public ActionResult Index()
        {
            var publicDivisions = publicDivisionRepository.GetPublicDivisions();
            var xtracurDivisions = xtracurDivisionRepository.GetXtracurDivisions();
            HomeIndexViewModel viewModel = HomeIndexViewModel.Build(publicDivisions, xtracurDivisions);
            return View(viewModel);
        }

        protected override void Dispose(bool disposing)
        {
 	        base.Dispose(disposing);
            if (disposing)
            {
                if (this.publicDivisionRepository != null)
                {
                    this.publicDivisionRepository.Dispose();
                }
                if (this.xtracurDivisionRepository != null)
                {
                    this.xtracurDivisionRepository.Dispose();
                }
            }
        }
    }
}
