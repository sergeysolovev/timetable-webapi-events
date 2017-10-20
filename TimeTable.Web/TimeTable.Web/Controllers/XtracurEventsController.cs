using System;
using System.Web.Mvc;
using SpbuEducation.TimeTable.Web.ViewModels;
using SpbuEducation.TimeTable.Web.Models;
using SpbuEducation.TimeTable.Common.Web.Helpers;
using SpbuEducation.TimeTable.BusinessObjects.Events;

namespace SpbuEducation.TimeTable.Web.Controllers
{
    public class XtracurEventsController : BaseController
    {
        private XtracurDivisionRepository xtracurDivisionRepository;
        private EventRepository eventRepository;

        public XtracurEventsController()
        {
            this.xtracurDivisionRepository = new XtracurDivisionRepository();
            this.eventRepository = new EventRepository();
        }

        public ActionResult GetModalContents(int eventId, int? recurrenceIndex)
        {
            Event timeEvent = eventRepository.GetEventById(eventId);
            var viewModel = EventsViewModelHelper.GetXtracurShowViewModel(timeEvent, recurrenceIndex ?? 0);
            return PartialView(viewModel);
        }

        public ActionResult Index(string alias, DateTime? fromDate, int? showImmediateEventId, int? showImmediateRecurrenceIndex)
        {
            // Если получен showImmediateEventId, сделать редирект с корректными alias и fromDate,
            // поскольку они могли поменяться:
            if (showImmediateEventId.HasValue)
            {
                Event showImmediateTimeEvent = eventRepository.GetEventById(showImmediateEventId.Value);
                if (showImmediateTimeEvent != null)
                {
                    var showImmediateEventViewModel = EventsViewModelHelper.GetXtracurShowImmediateItemViewModel(
                        showImmediateTimeEvent, showImmediateRecurrenceIndex ?? 0);
                    if (!showImmediateEventViewModel.IsEmpty)
                    {
                        var showImmediateEventXtracurDivision = xtracurDivisionRepository.GetXtracurDivisionByAlias(showImmediateEventViewModel.DivisionAlias);
                        if (showImmediateEventXtracurDivision != null)
                        {
                            if (alias != showImmediateEventXtracurDivision.Alias || fromDate != showImmediateEventViewModel.FromDate)
                            {
                                return RedirectToAction("Index",
                                    new
                                    {
                                        alias = showImmediateEventXtracurDivision.Alias,
                                        fromDate = showImmediateEventViewModel.FromDateString,
                                        showImmediateEventId = showImmediateEventId,
                                        showImmediateRecurrenceIndex = showImmediateRecurrenceIndex
                                    });
                            }
                        }
                    }
                }
            }
            // Основная логика:
            var xtracurDivision = xtracurDivisionRepository.GetXtracurDivisionByAlias(alias);
            if (xtracurDivision != null)
            {
                var viewModel = XtracurEventsIndexViewModelBase.Build(xtracurDivision, fromDate, showImmediateEventId,
                    showImmediateRecurrenceIndex);
                return View(viewModel.ViewName, viewModel);
            }
            return HttpNotFound();
        }

        public ActionResult ShowImage(Guid id)
        {
            XtracurImageAttachment imageAttachment = xtracurDivisionRepository.Session.GetObjectByKey<XtracurImageAttachment>(id);
            return imageAttachment != null && imageAttachment.ImageData != null
                ? (ActionResult)File(imageAttachment.ImageData, "image/png")
                : HttpNotFound();
        }

        public ActionResult Search(string alias, string q, int? offset, bool? showPast)
        {
            XtracurDivision xtracurDivision = xtracurDivisionRepository.GetXtracurDivisionByAlias(alias);
            var viewModel = XtracurEventsSearchViewModel.Build(xtracurDivision, q, offset, showPast);
            return xtracurDivision != null ? (ActionResult)View(viewModel) : HttpNotFound();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (this.xtracurDivisionRepository != null)
                {
                    this.xtracurDivisionRepository.Dispose();
                }
                if (this.eventRepository != null)
                {
                    this.eventRepository.Dispose();
                }
            }
        }
    }
}
