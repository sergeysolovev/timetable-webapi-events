using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using SpbuEducation.TimeTable.Appointments.Spreadsheets.Models;
using SpbuEducation.TimeTable.BusinessObjects.Personnel;
using SpbuEducation.TimeTable.Web.ViewModels;
using SpbuEducation.TimeTable.Web.Models;
using SpbuEducation.TimeTable.Module.Helpers.Spreadsheet;

namespace SpbuEducation.TimeTable.Web.Controllers
{
    public class EducatorEventsController : BaseController
    {
        private EducatorEmploymentRepository educatorEmploymentRepository;

        public EducatorEventsController()
        {
            this.educatorEmploymentRepository = new EducatorEmploymentRepository();
        }

        public ActionResult ShowWeek(int masterId, DateTime? weekMonday)
        {
            var masterPerson = educatorEmploymentRepository.GetEducatorMasterPersonById(masterId);
            if (masterPerson != null)
            {
                var viewModel = EducatorEventsShowViewModel.Build(masterPerson, weekMonday);
                return View(viewModel);
            }
            return View();
        }

        public ActionResult Show(int masterId, int? next)
        {
            var masterPerson = educatorEmploymentRepository.GetEducatorMasterPersonById(masterId);
            if (masterPerson != null)
            {
                var viewModel = EducatorEventsShowAllViewModel.Build(masterPerson, next);
                return View(viewModel);
            }
            return View();
        }

        public ActionResult Index(string q)
        {
            string educatorLastNameQuery = q;
            var session = educatorEmploymentRepository.Session;
            var viewModel = EducatorEventsIndexViewModel.Build(session, educatorLastNameQuery);
            return View(viewModel);
        }

        public FileStreamResult Excel(int masterId, int? next)
        {
            var masterPerson = educatorEmploymentRepository.GetEducatorMasterPersonById(masterId);
            if (masterPerson != null)
            {
                var viewModel = EducatorEventsShowAllViewModel.Build(masterPerson, next);
                return File(
                    GetXmlContentAsStream(masterPerson, viewModel),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"расписание преподавателя на семестр - {masterPerson.DisplayName} ({viewModel.From:dd MMMM yyyy} - {viewModel.To:dd MMMM yyyy} г. на дату {DateTime.Now:dd.MM}).xlsx");
            }
            return null;
        }

        public FileStreamResult WeekExcel(int masterId, DateTime? weekMonday)
        {
            var masterPerson = educatorEmploymentRepository.GetEducatorMasterPersonById(masterId);
            if (masterPerson != null)
            {
                var viewModel = EducatorEventsShowModel.Build(masterPerson, weekMonday);
                return File(
                    GetEducatorWeekExcelContentAsStream(masterPerson, viewModel, viewModel.WeekStart),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"расписание преподавателя на неделю - {masterPerson.DisplayName} ({viewModel.WeekStart:dd MMMM yyyy} - {viewModel.WeekStart.AddDays(7):dd MMMM yyyy} г. на дату {DateTime.Now:dd.MM}).xlsx");
            }
            return null;
        }

        private Stream GetEducatorWeekExcelContentAsStream(EducatorMasterPerson masterPerson, EducatorEventsShowModel model, DateTime? weekMonday)
        {
            var eventDayModels = model.EducatorEventsDays.Select(day =>
                new EventDayModel(day.DayString, day.DayStudyEvents.Select(@event =>
                    new EventModel(
                        @event.DateTime.Time.Value,
                        @event.Subject,
                        @event.Cohort,
                        @event.ShowCohort,
                        string.Join("\n", @event.EventLocations.Select(el => el.DisplayName)),
                        string.Join("\n", @event.ContingentUnits.Select(cu => cu.Name))
                        ))));
            var stream = AppointmentsSpreadsheetsHelper.GetEducatorWeekTimetableWorkbookStream(weekMonday.Value, weekMonday.Value.AddDays(7),
                $"{masterPerson.DisplayName}", eventDayModels);

            return stream;
        }

        private Stream GetXmlContentAsStream(EducatorMasterPerson masterPerson,  EducatorEventsShowAllViewModel viewModel)
        {
            var aggregatedEventDayModels = viewModel.EducatorEventsDays.Select(day =>
                new AggregatedEventDayModel(day.DayString, day.DayStudyEvents.Select(aggregatedEvent =>
                new AggregatedEventModel(
                    aggregatedEvent.DateTime.Time.Value,
                    string.Join("\n", aggregatedEvent.DateTime.Dates.Select(d => d.Value)),
                    aggregatedEvent.Subject,
                    aggregatedEvent.Cohort,
                    aggregatedEvent.ShowCohort,
                    aggregatedEvent.EventLocations.FirstOrDefault().DisplayName,
                    string.Join("; ",aggregatedEvent.ContingentUnits.Select(cu => $"{cu.Name} ({cu.DivisionAndCourse})")),
                    null))));
            var stream = AppointmentsSpreadsheetsHelper.GetEducatorTimetableWorkbookStream(viewModel.From, viewModel.To,
                $"{masterPerson.LastName} {masterPerson.FirstName} {masterPerson.MiddleName}", aggregatedEventDayModels);
            
            return stream;
        }
        protected override void Dispose(bool disposing)
        {
 	        base.Dispose(disposing);
            if (disposing)
            {
                if (this.educatorEmploymentRepository != null)
                {
                    this.educatorEmploymentRepository.Dispose();
                }
            }
        }
    }
}
