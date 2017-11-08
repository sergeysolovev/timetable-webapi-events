using SpbuEducation.TimeTable.Appointments.Repositories;
using SpbuEducation.TimeTable.Helpers;
using SpbuEducation.TimeTable.Helpers.Multilingual;
using SpbuEducation.TimeTable.Web.Api.v1.Localization;
using SpbuEducation.TimeTable.Web.Api.v1.DataContracts;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Localization;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Abstractions;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.Mappers;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.Repositories;
using System;
using System.Linq;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo
{
    class EventsService  : IEventsService
    {
        private readonly GroupRepository groupRepository;
        private readonly EventLocationMapper eventLocationMapper;
        private readonly EducatorIdTupleMapper educatorIdMapper;
        private readonly ContingentDivisionCourseMapper contingentDivCourseMapper;
        private readonly LanguageCode language;
        private readonly TimeTableKindCodeMapper timetableMapper;
        private readonly TimetableKindRepository timetableKindRepository;

        public EventsService(GroupRepository groupRepository,
            EventLocationMapper eventLocationMapper,
            EducatorIdTupleMapper educatorIdMapper,
            ContingentDivisionCourseMapper contingentDivCourseMapper,
            TimetableKindRepository timetableKindRepository,
            TimeTableKindCodeMapper timetableMapper,
            LocaleInfo locale)
        {
            this.groupRepository = groupRepository ??
                 throw new ArgumentNullException(nameof(groupRepository));

            this.eventLocationMapper = eventLocationMapper ??
                throw new ArgumentNullException(nameof(eventLocationMapper));

            this.educatorIdMapper = educatorIdMapper ??
                throw new ArgumentNullException(nameof(educatorIdMapper));

            this.contingentDivCourseMapper = contingentDivCourseMapper ??
                throw new ArgumentNullException(nameof(contingentDivCourseMapper));
            this.timetableKindRepository = timetableKindRepository ??
                throw new ArgumentNullException(nameof(timetableKindRepository));
            this.timetableMapper = timetableMapper ??
                throw new ArgumentNullException(nameof(timetableMapper));

            language = locale.Language;
        }
        public GroupEventsContract GetEvents(int id, DateTime from, DateTime to,
            TimeTableKindСode localTimeTableKindCode = TimeTableKindСode.Unknown)
        {
            var group = groupRepository.Get(id);

            if (group == null)
            {
                return null;
            }

            var timetableKindCode = timetableMapper.Map(localTimeTableKindCode);
            var timetableKind = timetableKindRepository.Get(timetableKindCode);

            var contract = new GroupEventsContract
            {
                Id = group.Id,
                DisplayName = $"{Resources.StudentGroup} {group.Name}",
                TimeTableDisplayName = (language == LanguageCode.English) ? "All classes" : "Все занятия",
                WeekDisplayText = DateTimeHelper.GetWeekDisplayText(language, from, to),
            };

            var isWebAvailable = group.IsPrimaryAvailableOnWeb
                                 || group.IsIntermediaryAttestationAvailableOnWeb
                                 || group.IsFinalAttestationAvailableOnWeb;


            if (isWebAvailable)
            {
                using (var repository = new StudentGroupAppointmentsRepository(group, timetableKind, from, to))
                {
                    contract.Days = repository
                        .GetAppointments()
                        .Where(a => a.IsPublicMaster)
                        .Where(a => a.EducatorsDisplayText != null)
                        .OrderBy(a => a.Start)
                        .ThenBy(a => a.SubjectEnglish)
                        .Select(a => new GroupEventsContract.Event
                        {
                            ContingentUnitName = a.ContingentUnitName,
                            DivisionAndCourse = contingentDivCourseMapper.Map(a.ContingentUnit),
                            StudyEventsTimeTableKindCode = timetableKind != null ? (int)timetableKind.Code : 0,
                            Start = a.Start,
                            End = a.End,
                            TimeIntervalString = a.GetTimeIntervalByLanguage(language),
                            DateWithTimeIntervalString = a.DateTimeIntervalString,
                            EducatorsDisplayText = a.GetEducatorsDisplayTextByLanguage(language),
                            LocationsDisplayText = a.GetLocationsDisplayTextByLanguage(language),
                            HasEducators = !string.IsNullOrEmpty(a.EducatorsDisplayText),
                            Subject = a.GetSubjectByLanguage(language),
                            ElectiveDisciplinesCount = a.EducatorAssignment?.FirstWorkUnit?.StudyModule?.ElectiveDisciplinesCount ?? 1,
                            IsElective = a.EducatorAssignment?.FirstWorkUnit?.StudyModule?.IsFacultative ?? false,
                            IsAssigned = a.WasScheduled,
                            IsCancelled = a.IsCancelled,
                            TimeWasChanged = a.TimeWasChanged,
                            LocationsWereChanged = a.LocationsWereChanged,
                            EducatorsWereReassigned = a.EducatorsWereReassigned,
                            HasTheSameTimeAsPreviousItem = false,
                            ContingentUnitsDisplayTest = null,
                            IsStudy = false,
                            AllDay = false,
                            WithinTheSameDay = false,
                            DisplayDateAndTimeIntervalString = a.DateTimeIntervalString,
                            EducatorIds = a.EventLocations.SelectMany(el => el.Educators).Select(educatorIdMapper.Map),
                            EventLocations = a.EventLocations.Select(eventLocationMapper.Map)
                        })
                        .GroupBy(e => e.Start.Date)
                        .OrderBy(g => g.Key)
                        .Select(g => new GroupEventsContract.EventsDay
                        {
                            Day = g.Key,
                            DayStudyEvents = g.AsEnumerable(),
                            DayString = (language == LanguageCode.English) ?
                                g.Key.ToString("dddd, MMMM d") :
                                g.Key.ToString("dddd, d MMMM")
                        })
                        .ToList();
                }
            }
            else
            {
                contract.Days = Enumerable.Empty<GroupEventsContract.EventsDay>();
            }

            return contract;
        }
    }
}
