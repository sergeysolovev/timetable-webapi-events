using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using SpbuEducation.TimeTable.Appointments.Spreadsheets.Models;
using SpbuEducation.TimeTable.BusinessObjects.Contingent;
using SpbuEducation.TimeTable.BusinessObjects.Events;
using SpbuEducation.TimeTable.Module.Helpers.Spreadsheet;
using SpbuEducation.TimeTable.Web.ViewModels;
using SpbuEducation.TimeTable.Web.Models;
using SpbuEducation.TimeTable.Helpers;

namespace SpbuEducation.TimeTable.Web.Controllers
{
    public class StudentGroupEventsController : BaseController
    {
        private StudentGroupRepository studentGroupRepository;
        private PublicDivisionRepository publicDivisionRepository;

        public StudentGroupEventsController()
        {
            studentGroupRepository = new StudentGroupRepository();
            publicDivisionRepository = new PublicDivisionRepository();
        }

        public ActionResult Primary(string publicDivisionAlias, int studentGroupId, DateTime? weekMonday)
        {
            var studentGroup = studentGroupRepository.GetStudentGroupById(studentGroupId);
            var publicDivision = publicDivisionRepository.GetPublicDivisionByAlias(publicDivisionAlias);
            if (studentGroup != null)
            {
                var viewModel = StudentGroupEventsPrimaryViewModel.Build(studentGroup, publicDivision, weekMonday);

                return View(viewModel.ViewName, viewModel);
            }
            return View();
        }

        public ActionResult Semester(string publicDivisionAlias, int studentGroupId, int? autumn)
        {
            var studentGroup = studentGroupRepository.GetStudentGroupById(studentGroupId);
            var publicDivision = publicDivisionRepository.GetPublicDivisionByAlias(publicDivisionAlias);
            if (studentGroup != null)
            {
                var viewModel = StudentGroupEventsSemesterViewModel.Build(studentGroup, publicDivision, autumn);

                return View(viewModel.ViewName, viewModel);
            }
            return View();
        }

        public FileStreamResult ExcelSemester(string publicDivisionAlias, int studentGroupId, int? autumn)
        {
            var studentGroup = studentGroupRepository.GetStudentGroupById(studentGroupId);
            var publicDivision = publicDivisionRepository.GetPublicDivisionByAlias(publicDivisionAlias);
            if (studentGroup != null)
            {
                var viewModel = StudentGroupEventsSemesterViewModel.Build(studentGroup, publicDivision, autumn);
                return File(
                    GetXmlContentAsStream(studentGroup, viewModel),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"расписание {studentGroup.Name} {(viewModel.IsSpringSemester ? "весенний": "осенний")} семестр {studentGroup.CurrentStudyYear.DisplayName}.xlsx");
            }
            return null;
        }

        public FileStreamResult ExcelWeek(int studentGroupId, DateTime? weekMonday)
        {
            var defaultWeekStart = DateTimeHelper.GetWeekStart(DateTime.Today);
            var from = weekMonday ?? defaultWeekStart;
            var to = from.AddDays(7);
            var studentGroup = studentGroupRepository.GetStudentGroupById(studentGroupId);
            if (studentGroup != null)
            {
                var model = StudentGroupEventsWeekModel.Build(studentGroup, from, to);
                return File(
                    GetWeekExcelContentAsStream(studentGroup, model, from, to),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"расписание {studentGroup.Name} {from}-{to}.xlsx");
            }
            return null;
        }

        private Stream GetWeekExcelContentAsStream(StudentGroup studentGroup, StudentGroupEventsWeekModel model, DateTime from, DateTime to)
        {
            var eventDayModels = model.Days.Select(day =>
                new EventDayModel(day.DayString, day.DayStudyEvents.Select(@event =>
                    new EventModel(
                        @event.DateTime.Time.Value,
                        @event.Subject,
                        @event.Cohort,
                        @event.ShowCohort,
                        string.Join("\n", @event.EventLocations.Select(el => el.DisplayName)),
                        string.Join("\n", @event.EventLocations.Select(el => string.Join("; ", el.Educators.Select(e => e.Name))))
                        ))));
            var stream = AppointmentsSpreadsheetsHelper.GetStudentGroupWeekTimetableWorkbookStream(from, to,
                $"{studentGroup.Name}", eventDayModels);

            return stream;
        }

        private Stream GetXmlContentAsStream(StudentGroup studentGroup, StudentGroupEventsSemesterViewModel viewModel)
        {
            var aggregatedEventDayModels = viewModel.Days.Select(day =>
                new AggregatedEventDayModel(day.DayString, day.DayStudyEvents.Select(aggregatedEvent =>
                    new AggregatedEventModel(
                        aggregatedEvent.DateTime.Time.Value,
                        string.Join("\n", aggregatedEvent.DateTime.Dates.Select(d => d.Value)),
                        aggregatedEvent.Subject,
                        aggregatedEvent.Cohort,
                        aggregatedEvent.ShowCohort,
                        string.Join("\n", aggregatedEvent.EventLocations.Select(el => el.DisplayName)),
                        string.Join("\n", aggregatedEvent.EventLocations.Select(el => string.Join("; ", el.Educators.Select(e => e.Name)))),
                        null))));
            var stream = AppointmentsSpreadsheetsHelper.GetStudentGroupSemesterTimetableWorkbookStream(studentGroup.TermStart, studentGroup.TermEnd, 
                $"{studentGroup.Name}", aggregatedEventDayModels);

            return stream;
        }

        public ActionResult Attestation(string publicDivisionAlias, int studentGroupId)
        {
            var studentGroup = studentGroupRepository.GetStudentGroupById(studentGroupId);
            var publicDivision = publicDivisionRepository.GetPublicDivisionByAlias(publicDivisionAlias);
            if (studentGroup != null)
            {
                StudentGroupEventsAttestationViewModel viewModel = StudentGroupEventsAttestationViewModel.Build(studentGroup,
                    publicDivision, StudyEventsTimeTableKindCode.Attestation);
                return View(viewModel.ViewName, viewModel);
            }
            return View();
        }

        public ActionResult Final(string publicDivisionAlias, int studentGroupId)
        {
            var studentGroup = studentGroupRepository.GetStudentGroupById(studentGroupId);
            var publicDivision = publicDivisionRepository.GetPublicDivisionByAlias(publicDivisionAlias);
            if (studentGroup != null)
            {
                StudentGroupEventsAttestationViewModel viewModel = StudentGroupEventsAttestationViewModel.Build(studentGroup,
                    publicDivision, StudyEventsTimeTableKindCode.Final);
                return View(viewModel.ViewName, viewModel);
            }
            return View();
        }

        protected override void Dispose(bool disposing)
        {
 	        base.Dispose(disposing);
            if (disposing)
            {
                if (studentGroupRepository != null)
                {
                    studentGroupRepository.Dispose();
                }
                if (publicDivisionRepository != null)
                {
                    publicDivisionRepository.Dispose();
                }
            }
        }
    }
}
