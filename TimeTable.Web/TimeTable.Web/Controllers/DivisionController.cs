using SpbuEducation.TimeTable.Web.Models;
using SpbuEducation.TimeTable.Web.ViewModels;
using System;
using System.Web.Mvc;

namespace SpbuEducation.TimeTable.Web.Controllers
{
    public class DivisionController : BaseController
    {
        private PublicDivisionRepository publicDivisionRepository;

        public DivisionController()
        {
            this.publicDivisionRepository = new PublicDivisionRepository();
        }

        //
        // GET: /Divisions/Show/{alias}
        public ActionResult Show(string alias)
        {
            var publicDivision = publicDivisionRepository.GetPublicDivisionByAlias(alias);
            if (!String.IsNullOrEmpty(publicDivision.ExternalWebSiteReference))
            {
                return Redirect(publicDivision.ExternalWebSiteReference);
            }
            DivisionShowViewModel divisionViewModel = DivisionShowViewModel.Build(publicDivision);
            return View(divisionViewModel);
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
            }
        }
    }
}
