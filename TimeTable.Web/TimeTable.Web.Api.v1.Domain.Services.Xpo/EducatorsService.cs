using SpbuEducation.TimeTable.Appointments.Repositories;
using SpbuEducation.TimeTable.Helpers;
using SpbuEducation.TimeTable.Helpers.Multilingual;
using SpbuEducation.TimeTable.Web.Api.v1.Localization;
using SpbuEducation.TimeTable.Web.Api.v1.DataContracts;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Localization;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Abstractions;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.Helpers;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.Mappers;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.Repositories;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo
{
    internal class EducatorsService : IEducatorsService
    {
        private readonly EducatorRepository educatorRepository;
        private readonly EventLocationMapper eventLocationMapper;
        private readonly EducatorIdTupleMapper educatorIdMapper;
        private readonly ContingentNameTupleMapper contingentNameMapper;
        private readonly LanguageCode language;

        public EducatorsService(
            EducatorRepository educatorRepository,
            EventLocationMapper eventLocationMapper,
            EducatorIdTupleMapper educatorIdMapper,
            ContingentNameTupleMapper contingentNameMapper,
            LocaleInfo locale)
        {
            this.educatorRepository = educatorRepository ??
                throw new ArgumentNullException(nameof(educatorRepository));

            this.eventLocationMapper = eventLocationMapper ??
                throw new ArgumentNullException(nameof(eventLocationMapper));

            this.educatorIdMapper = educatorIdMapper ??
                throw new ArgumentNullException(nameof(educatorIdMapper));

            this.contingentNameMapper = contingentNameMapper ??
                throw new ArgumentNullException(nameof(contingentNameMapper));

            language = locale.Language;
        }

        public EducatorsContract SearchByLastName(string lastNameQuery)
        {
            return new EducatorsContract
            {
                EducatorLastNameQuery = lastNameQuery,
                Educators = educatorRepository
                    .SearchByLastName(lastNameQuery)
                    .Select(e => new EducatorContract
                    {
                        Id = e.Id,
                        DisplayName = e.GetDisplayNameByLanguage(language),
                        FullName = $"{e.LastName} {e.FirstName} {e.MiddleName}",
                        Employments = e.Persons
                            .SelectMany(p => p.EducatorEmployments)
                            .Select(ee => new EducatorContract.Employment
                            {
                                Position = Regex.Replace(
                                    ee?.PositionShort?.GetNameByLanguage(language) ?? string.Empty,
                                    @"[\d-]",
                                    string.Empty
                                ),
                                Department = ee?.SapDepartment?.Name ?? string.Empty
                            })
                            .Distinct(new EducatorContract.EmploymentEqualityComparer())
                    })
            };
        }

        public EducatorEventsContract GetEvents(int educatorId, int? showNextTerm)
        {
            var educator = educatorRepository.Get(educatorId);

            if (educator == null)
            {
                return null;
            }

            var employments = educator.Persons.SelectMany(p => p.EducatorEmployments);
            var educatorDisplayText = educator.GetDisplayNameByLanguage(language);
            var educatorLongDisplayText = educator.GetLongDisplayNameByLanguage(language);
            var now = DateTime.Now;
            var summerTermBoundary = new DateTime(now.Month == 1 ? now.Year - 1 : now.Year, 8, 1);
            var winterTermBoundary = new DateTime(now >= summerTermBoundary && now.Month > 1 ? now.Year + 1 : now.Year, 2, 1);
            var nextSummerTermBoundary = new DateTime(summerTermBoundary.Year + 1, summerTermBoundary.Month, summerTermBoundary.Day);
            var nextWinterTermBoundary = new DateTime(winterTermBoundary.Year + 1, winterTermBoundary.Month, winterTermBoundary.Day);
            var isSpringTerm = winterTermBoundary < now && now < summerTermBoundary;
            var showNext = showNextTerm.HasValue && showNextTerm.Value > 0;
            var fromDate = showNext ?
                (isSpringTerm ? summerTermBoundary : winterTermBoundary) :
                (isSpringTerm ? winterTermBoundary : summerTermBoundary);
            var toDate = showNext ?
                (isSpringTerm ? nextWinterTermBoundary : nextSummerTermBoundary) :
                (isSpringTerm ? summerTermBoundary : winterTermBoundary);
            var title = $"{Resources.TimetableForEducator} {educatorLongDisplayText}";

            DayOfWeek[] daysOfWeek = {
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday,
                DayOfWeek.Friday,
                DayOfWeek.Saturday
            };

            using (var repository = new EducatorAppointmentsRepository(educator, employments, fromDate, toDate))
            {
                var appointments = repository.GetAppointments().ToList();
                var eventsDays = daysOfWeek.Select(day =>
                {
                    var dayEvents = appointments
                        .Where(a => a.IsPublicMaster && !a.IsCancelled)
                        .Where(a => a.Start.DayOfWeek == day)
                        .Select(a => new AggregatedContingent(a, language))
                        .Select(ac => new AggregatedDates(ac, language))
                        .GroupBy(ad => ad.Key)
                        .OrderBy(ad => ad.Key.Start)
                        .ThenBy(ad => ad.Key.Subject)
                        .Select(g => new EducatorEventsContract.Event
                        {
                            Start = g.Key.Start,
                            End = g.Key.End,
                            Subject = g.Key.Subject,
                            Dates = new DateSeries(g.Select(ad => ad.Date), language).Select(d => d.ToString()),
                            TimeIntervalString = g.Key.GetTimeIntervalString(),
                            EducatorsDisplayText = g.Key.EducatorsDisplayText,
                            IsCanceled = g.Key.IsCanceled,
                            StudyEventsTimeTableKindCode = (int)g.Key.StudyEventsTimeTableKindCode,
                            ContingentUnitNames = g.Key.ContingentUnits.Select(contingentNameMapper.Map),
                            EducatorIds = g.Key.EventLocations.SelectMany(el => el.Educators).Select(educatorIdMapper.Map),
                            EventLocations = g.Key.EventLocations.Select(eventLocationMapper.Map)
                        })
                        .ToList();

                    return new EducatorEventsContract.EventsDay
                    {
                        Day = day,
                        DayString = DateTimeHelper.GetDayOfWeekStringByLanguage(day, language),
                        DayStudyEventsCount = dayEvents.Count(),
                        DayStudyEvents = dayEvents,
                    };
                })
                .ToList();

                return new EducatorEventsContract
                {
                    From = fromDate,
                    To = toDate,
                    IsNextTerm = showNextTerm,
                    DisplayText = educatorDisplayText,
                    LongDisplayText = educatorLongDisplayText,
                    DateRangeDisplayText = string.Format("{0:d MMMM yyyy} - {1:d MMMM yyyy}", fromDate, toDate),
                    TimetableTitle = title,
                    Id = educator.Id,
                    IsSpringTerm = isSpringTerm,
                    SpringTermLinkAvailable = (isSpringTerm == showNext),
                    AutumnTermLinkAvailable = (isSpringTerm != showNext),
                    HasEvents = eventsDays.Any(d => d.DayStudyEvents.Any()),
                    EducatorEventsDays = eventsDays
                };
            }
        }
    }
}