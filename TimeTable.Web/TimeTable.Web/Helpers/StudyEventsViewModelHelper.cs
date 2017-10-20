using DevExpress.XtraScheduler;
using SpbuEducation.TimeTable.Appointments;
using SpbuEducation.TimeTable.Appointments.Repositories;
using SpbuEducation.TimeTable.BusinessObjects.Events;
using SpbuEducation.TimeTable.BusinessObjects.Personnel;
using SpbuEducation.TimeTable.BusinessObjects.Contingent;
using SpbuEducation.TimeTable.BusinessObjects.RealEstate;
using SpbuEducation.TimeTable.Common.Web.ViewModels;
using SpbuEducation.TimeTable.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SpbuEducation.TimeTable.Common.Web.Helpers;

namespace SpbuEducation.TimeTable.Web.Helpers
{
    public class StudyEventsViewModelHelper
    {
        private static IEnumerable<StudyEventItemViewModel> GetEducatorEvents(
            EducatorMasterPerson educatorMasterPerson, DateTime fromDate, DateTime toDate)
        {
            using (var appointmentsRepository = new EducatorAppointmentsRepository(educatorMasterPerson.Persons.SelectMany(p => p.EducatorEmployments), fromDate, toDate))
            {
                var appointments = appointmentsRepository.GetAppointments();
                return appointments
                    .Where(a => a.IsPublicMaster)
                    .OrderBy(a => a.Start)
                    .ThenBy(a => a.SubjectEnglish)
                    .Select(a => StudyEventItemViewModel.Build(a, forEducator: educatorMasterPerson));
            }
        }

        private static bool IsTimeTableKindWebAvailable(StudentGroup studentGroup, StudyEventsTimeTableKind studyEventsTimeTableKind)
        {
            if (studyEventsTimeTableKind == null)
                    return studentGroup.IsPrimaryAvailableOnWeb || studentGroup.IsIntermediaryAttestationAvailableOnWeb || studentGroup.IsFinalAttestationAvailableOnWeb;
            switch (studyEventsTimeTableKind.Code)
            {
                case StudyEventsTimeTableKindCode.Unknown:
                    return false;
                case StudyEventsTimeTableKindCode.Primary:
                    return studentGroup.IsPrimaryAvailableOnWeb;
                case StudyEventsTimeTableKindCode.Attestation:
                    return studentGroup.IsIntermediaryAttestationAvailableOnWeb;
                case StudyEventsTimeTableKindCode.Final:
                    return studentGroup.IsFinalAttestationAvailableOnWeb;
                default:
                    return false;
            }
        }

        private static IEnumerable<StudyEventItemViewModel> GetStudyEventIndexViewModelsByDateRange(
            StudentGroup studentGroup,
            StudyEventsTimeTableKind studyEventsTimeTableKind,
            DateTime fromDate, DateTime toDate)
        {
            if (IsTimeTableKindWebAvailable(studentGroup, studyEventsTimeTableKind))
            {
                using (var appointmentsRepository = new StudentGroupAppointmentsRepository(studentGroup, studyEventsTimeTableKind, fromDate, toDate))
                {
                        var appointments = appointmentsRepository.GetAppointments();
                        return appointments
                            .Where(a => a.IsPublicMaster)
                            .Where(a => a.EducatorsDisplayText != null)
                            .OrderBy(a => a.Start)
                            .ThenBy(a => a.SubjectEnglish)
                            .Select(a => StudyEventItemViewModel.Build(a));
                }
            }
            return Enumerable.Empty<StudyEventItemViewModel>();
        }

        public static IEnumerable<StudyEventItemViewModel> GetStudyEventIndexViewModelsForTerm(
            StudentGroup studentGroup,
            StudyEventsTimeTableKind studyEventsTimeTableKind,
            StudyEventsTimeTableKindCode studyEventsTimeTableKindCode)
        {
            if (IsTimeTableKindWebAvailable(studentGroup, studyEventsTimeTableKind))
            {
                using (var appointmentsRepository = new StudentGroupAppointmentsRepository(studentGroup, studyEventsTimeTableKind))
                {
                    var appointments = appointmentsRepository.GetAppointments();
                    return appointments
                        .Where(a => a.IsPublicMaster)
                        .Where(a => !String.IsNullOrEmpty(a.EducatorsDisplayText))
                        .OrderBy(a => a.Start)
                        .ThenBy(a => a.Subject)
                        .Select(a => StudyEventItemViewModel.Build(a));
                }
            }
            return Enumerable.Empty<StudyEventItemViewModel>();
        }

        public static IEnumerable<StudyEventMonthViewModel> GetStudyEventsMonthsViewModelsForTerm(
            StudentGroup studentGroup,
            StudyEventsTimeTableKind studyEventsTimeTableKind,
            StudyEventsTimeTableKindCode studyEventsTimeTableKindCode)
        {
            var monthsViewModels = GetStudyEventIndexViewModelsForTerm(studentGroup, studyEventsTimeTableKind, studyEventsTimeTableKindCode)
                .GroupBy(vm => GetFirstDayOfMonth(vm.Start))
                .OrderBy(g => g.Key)
                .Select(g => StudyEventMonthViewModel.Build(g.Key, g.AsEnumerable()))
                .ToList();
            SetActiveMonth(monthsViewModels);
            return monthsViewModels;
        }

        /// <summary>
        /// Устанавливаем активный месяц на основе текущей даты.
        /// Это либо текущий месяц, либо ближайший будущий месяц.
        /// </summary>
        /// <param name="monthsViewModels"></param>
        private static void SetActiveMonth(List<StudyEventMonthViewModel> monthsViewModels)
        {
            var firstDayOfCurrentMonth = GetFirstDayOfMonth(DateTime.Now);
            var activeMonthViewModel = monthsViewModels.SingleOrDefault(vm => vm.FirstDayOfMonth == firstDayOfCurrentMonth);
            if (activeMonthViewModel != null)
            {
                activeMonthViewModel.IsActive = true;
            }
            else
            {
                activeMonthViewModel = monthsViewModels.FirstOrDefault(vm => vm.FirstDayOfMonth > firstDayOfCurrentMonth);
                if (activeMonthViewModel != null)
                {
                    activeMonthViewModel.IsActive = true;
                }
            }
        }

        private static DateTime GetFirstDayOfMonth(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        public static IEnumerable<StudyEventDayItemViewModel> GetStudyEventsDaysViewModelsForDateRange(
            StudentGroup studentGroup,
            StudyEventsTimeTableKind studyEventsTimeTableKind,
            DateTime fromDate, DateTime toDate)
        {
            return GetStudyEventIndexViewModelsByDateRange(studentGroup, studyEventsTimeTableKind, fromDate, toDate)
                .GroupBy(vm => vm.Start.Date)
                .OrderBy(g => g.Key)
                .Select(g => StudyEventDayItemViewModel.Build(g.Key, g.AsEnumerable()));
        }

        public static IEnumerable<StudyEventDayItemViewModel> GetEducatorEventsDayViewModels(
            EducatorMasterPerson educatorMasterPerson, DateTime fromDate, DateTime toDate)
        {
            return GetEducatorEvents(educatorMasterPerson, fromDate, toDate)
                .GroupBy(vm => vm.Start.Date)
                .OrderBy(g => g.Key)
                .Select(g => StudyEventDayItemViewModel.Build(g.Key, g.AsEnumerable()));
        }

        public static IEnumerable<StudyEventDayItemModel> GetEducatorEventsDayModels(
            EducatorMasterPerson educatorMasterPerson, DateTime fromDate, DateTime toDate)
        {
            return GetEducatorEvents(educatorMasterPerson, fromDate, toDate)
                .GroupBy(vm => vm.Start.Date)
                .OrderBy(g => g.Key)
                .Select(g => StudyEventDayItemModel.Build(g.Key, g.AsEnumerable()));
        }

        public class StudyEventAggregateContingentsHelper
        {
            public class StudyEventAggregateContingentsHelperKey
            {
                public readonly StudyEventsTimeTableKindCode StudyEventsTimeTableKindCode;
                public readonly DateTime Start;
                public readonly DateTime End;
                public readonly string Subject;
                public readonly string Cohort;
                public readonly bool ShowCohort;
                public readonly IEnumerable<EventLocation> EventLocations;
                public readonly string EducatorsDisplayText;
                public readonly bool DontShowEndTimeOnWeb;

                public StudyEventAggregateContingentsHelperKey(TimeEventAppointment appointment)
                {
                    StudyEventsTimeTableKindCode = appointment.StudyEventsTimeTableKind != null
                        ? appointment.StudyEventsTimeTableKind.Code
                        : StudyEventsTimeTableKindCode.Unknown;
                    Start = appointment.Start;
                    End = appointment.End;
                    Subject = appointment.GetSubjectByLanguage(CultureHelper.CurrentLanguage);
                    ShowCohort = appointment.ShowCohort;
                    Cohort = appointment.GetCohortDisplayTextByLanguage(CultureHelper.CurrentLanguage);
                    EventLocations = appointment.EventLocations;
                    EducatorsDisplayText = appointment.GetEducatorsDisplayTextByLanguage(CultureHelper.CurrentLanguage);
                    DontShowEndTimeOnWeb = appointment.DontShowEndTimeOnWeb;
                    var language = CultureHelper.CurrentLanguage;
                    EducatorIds = appointment.EventLocations
                        .SelectMany(el => el.Educators)
                        .Select(le => le.EducatorEmployment != null
                            ? new Tuple<int, string>(
                                le.EducatorEmployment.EducatorPerson.MasterPerson != null
                                    ? le.EducatorEmployment.EducatorPerson.MasterPerson.Id
                                    : -1,
                                le.EducatorEmployment.GetDisplayNameByLanguageFormated(language))
                            : le.Educator != null
                                ? new Tuple<int, string>(le.Educator.Id, le.Educator.GetDisplayNameByLanguage(language))
                                : null)
                        .ToArray();
                }

                public Tuple<int, string>[] EducatorIds { get; set; }

                public override bool Equals(object obj)
                {
                    StudyEventAggregateContingentsHelperKey o = obj as StudyEventAggregateContingentsHelperKey;
                    if (o == null) return false;
                    IEnumerable<Location> tloc = this.EventLocations.Select(el => el.Location);
                    IEnumerable<Location> oloc = o.EventLocations.Select(el => el.Location);
                    return this.StudyEventsTimeTableKindCode == o.StudyEventsTimeTableKindCode
                        && DateTime.Equals(this.Start, o.Start)
                        && DateTime.Equals(this.End, o.End)
                        && string.Equals(this.Subject, o.Subject)
                        && tloc.Count() == oloc.Count() && !tloc.Except(oloc).Any();
                }
                public override int GetHashCode()
                {
                    int hash = StudyEventsTimeTableKindCode.GetHashCode() ^ Start.GetHashCode() ^ End.GetHashCode() ^ Subject.GetHashCode();
                    foreach (EventLocation sel in EventLocations) hash ^= sel.Location.Oid.GetHashCode();
                    return hash;
                }
            }
            public readonly StudyEventAggregateContingentsHelperKey Key;
            public readonly TimeEventAppointment Appointment;
            public StudyEventAggregateContingentsHelper(TimeEventAppointment appointment)
            {
                Key = new StudyEventAggregateContingentsHelperKey(appointment);
                Appointment = appointment;
            }
        }

        public class StudyEventAggregateDatesHelper
        {
            public class StudyEventAggregateDatesHelperKey
            {
                public readonly StudyEventsTimeTableKindCode StudyEventsTimeTableKindCode;
                public readonly TimeSpan Start;
                public readonly TimeSpan End;
                public readonly string Subject;
                public readonly string Cohort;
                public readonly bool ShowCohort;
                public readonly IEnumerable<EventLocation> EventLocations;
                public readonly IEnumerable<ContingentUnit> ContingentUnits;
                public readonly string EducatorsDisplayText;
                public readonly bool IsCanceled;
                public readonly bool DontShowEndTimeOnWeb;
                public StudyEventAggregateDatesHelperKey(StudyEventAggregateContingentsHelper g)
                {
                    StudyEventsTimeTableKindCode = g.Key.StudyEventsTimeTableKindCode;
                    Start = g.Key.Start.TimeOfDay;
                    End = g.Key.End.TimeOfDay;
                    Subject = g.Key.Subject;
                    Cohort = g.Key.Cohort;
                    ShowCohort = g.Key.ShowCohort;
                    EventLocations = g.Key.EventLocations;
                    ContingentUnits = new[] { g.Appointment.ContingentUnit };
                    EducatorsDisplayText = g.Key.EducatorsDisplayText;
                    IsCanceled = g.Appointment.IsCancelled;
                    EducatorIds = g.Key.EducatorIds;
                    DontShowEndTimeOnWeb = g.Key.DontShowEndTimeOnWeb;
                }

                public Tuple<int, string>[] EducatorIds { get; set; }

                public override bool Equals(object obj)
                {
                    StudyEventAggregateDatesHelperKey o = obj as StudyEventAggregateDatesHelperKey;
                    if (o == null) return false;
                    IEnumerable<Location> tloc = this.EventLocations.Select(el => el.Location);
                    IEnumerable<Location> oloc = o.EventLocations.Select(el => el.Location);
                    return this.StudyEventsTimeTableKindCode == o.StudyEventsTimeTableKindCode
                        && TimeSpan.Equals(this.Start, o.Start)
                        && TimeSpan.Equals(this.End, o.End)
                        && string.Equals(this.Subject, o.Subject)
                        && string.Equals(this.Cohort, o.Cohort)
                        && bool.Equals(this.ShowCohort, o.ShowCohort)
                        && tloc.Count() == oloc.Count() && !tloc.Except(oloc).Any()
                        && this.ContingentUnits.Count() == o.ContingentUnits.Count() && !this.ContingentUnits.Except(o.ContingentUnits).Any();
                }
                public override int GetHashCode()
                {
                    int hash = StudyEventsTimeTableKindCode.GetHashCode() ^ Start.GetHashCode() ^ End.GetHashCode() ^ Subject.GetHashCode();
                    foreach (EventLocation sel in EventLocations) hash ^= sel.Location.Oid.GetHashCode();
                    foreach (ContingentUnit cu in ContingentUnits) if (cu != null) hash ^= cu.Oid.GetHashCode();
                    return hash;
                }
            }
            public readonly StudyEventAggregateDatesHelperKey Key;
            public readonly DateTime Date;
            public StudyEventAggregateDatesHelper(StudyEventAggregateContingentsHelper g)
            {
                Key = new StudyEventAggregateDatesHelperKey(g);
                Date = g.Key.Start.Date;
            }
        }

        public static IEnumerable<StudyEventAggregatedDayItemViewModel> GetEducatorAggregatedEventsDays(
            EducatorMasterPerson educatorMasterPerson, DateTime fromDate, DateTime toDate)
        {
            using (var appointmentsRepository = new EducatorAppointmentsRepository(educatorMasterPerson, educatorMasterPerson.Persons.SelectMany(p => p.EducatorEmployments), fromDate, toDate))
            {
                return GetStudyEventAggregatedDayItemViewModels(appointmentsRepository, forEducator: educatorMasterPerson);
            }
        }

        public static IEnumerable<StudyEventAggregatedDayItemViewModel> GetStudentGroupAggregatedEventsDays(
            StudentGroup studentGroup, StudyEventsTimeTableKind studyEventsTimeTableKind, DateTime fromDate, DateTime toDate)
        {
            using (var appointmentsRepository = new StudentGroupAppointmentsRepository(studentGroup, studyEventsTimeTableKind, fromDate, toDate))
            {
                return GetStudyEventAggregatedDayItemViewModels(appointmentsRepository);
            }
        }

        public static IEnumerable<StudyEventAggregatedDayItemViewModel> GetXtracurTimetableAggregatedEventsDays(
            XtracurDivision xtracurDivision, DateTime fromDate, DateTime toDate)
        {
            using (var appointmentsRepository = new XtracurAppointmentsRepository(xtracurDivision, fromDate, toDate))
            {
                return GetStudyEventAggregatedDayItemViewModels(appointmentsRepository);
            }
        }

        private static IEnumerable<StudyEventAggregatedDayItemViewModel> GetStudyEventAggregatedDayItemViewModels(
            IAppointmentRepository<TimeEventAppointment> appointmentsRepository,
            EducatorMasterPerson forEducator = null)
        {
            DayOfWeek[] daysOfWeek = {DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday};

            var appointments = appointmentsRepository.GetAppointments().Where(a => a.IsPublicMaster && !a.IsCancelled);

            return daysOfWeek.Select(d => GetStudyEventAggregatedDayItemViewModel(d, appointments, forEducator));
        }

        private static StudyEventAggregatedDayItemViewModel GetStudyEventAggregatedDayItemViewModel(
            DayOfWeek d,
            IEnumerable<TimeEventAppointment> appointments,
            EducatorMasterPerson forEducator = null)
        {
            var studyEventAggregateDatesHelpers = appointments
                .Where(a => a.Start.DayOfWeek == d)
                .Select(a => new StudyEventAggregateContingentsHelper(a))
                .Select(ch => new StudyEventAggregateDatesHelper(ch));

            var studyEventAggregatedItemViewModels = studyEventAggregateDatesHelpers
                .GroupBy(seadh => seadh.Key)
                .OrderBy(seadh => seadh.Key.Start)
                .ThenBy(seadh => seadh.Key.Subject)
                .Select(g => StudyEventAggregatedItemViewModel.Build(
                    g.Key,
                    g.AsEnumerable().Select(seadh => seadh.Date),
                    forEducator
                ));

            return StudyEventAggregatedDayItemViewModel.Build(d, studyEventAggregatedItemViewModels.ToList());
        }
    }
}