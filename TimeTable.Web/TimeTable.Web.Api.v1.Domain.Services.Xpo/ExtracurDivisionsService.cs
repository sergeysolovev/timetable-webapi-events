using SpbuEducation.TimeTable.Appointments.Repositories;
using SpbuEducation.TimeTable.BusinessObjects.Events;
using SpbuEducation.TimeTable.Helpers;
using SpbuEducation.TimeTable.Helpers.Multilingual;
using SpbuEducation.TimeTable.Web.Api.v1.DataContracts;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Localization;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Abstractions;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.Mappers;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo
{
    internal class ExtracurDivisionsService : IExtracurDivisionsService
    {
        private readonly LanguageCode language;
        private readonly CultureInfo culture;
        private readonly ExtracurDivisionRepository divisionsRepository;
        private readonly AddressLocationMapper addressLocationMapper;
        private readonly ExtracurEventMapper extracurEventMapper;

        public ExtracurDivisionsService(
            ExtracurDivisionRepository divisionsRepository,
            AddressLocationMapper addressLocationMapper,
            ExtracurEventMapper extracurEventMapper,
            LocaleInfo locale)
        {
            if (locale == null)
            {
                throw new ArgumentNullException(nameof(locale));
            }

            this.divisionsRepository = divisionsRepository ??
                throw new ArgumentNullException(nameof(divisionsRepository));

            this.addressLocationMapper = addressLocationMapper ??
                throw new ArgumentNullException(nameof(addressLocationMapper));

            this.extracurEventMapper = extracurEventMapper ??
                throw new ArgumentNullException(nameof(extracurEventMapper));

            language = locale.Language;
            culture = locale.Culture;
        }

        public IEnumerable<ExtracurDivisionContract> Get()
        {
            return divisionsRepository
                .Get()
                .Select(d => new ExtracurDivisionContract
                {
                    Name = d.GetNameByLanguage(language),
                    Alias = d.Alias
                })
                .OrderBy(d => d.Name);
        }

        public ExtracurEventsContract GetEvents(string alias, DateTime? fromDate = null)
        {
            var division = divisionsRepository.Get(alias);
            if (division == null)
            {
                return null;
            }

            switch (division.WebViewKind)
            {
                case XtracurEventsWebViewKind.Week:
                    return GetWeekEvents(division, fromDate);
                case XtracurEventsWebViewKind.Month:
                    return GetMonthEvents(division, month: fromDate);
                default:
                    throw new NotSupportedException($"Division '{division.Alias}' is not supported");
            }
        }

        private ExtracurEventsContract GetWeekEvents(XtracurDivision division, DateTime? fromDate = null)
        {
            var weekFromDate = DateTimeHelper.GetWeekStart(fromDate);
            var weekToDate = weekFromDate.AddDays(7);
            var previousWeekFromDate = weekFromDate.AddDays(-7);
            var nextWeekFromDate = weekFromDate.AddDays(7);
            var currentWeekFromDate = DateTimeHelper.GetWeekStart(DateTime.Today);

            using (var repository = new XtracurAppointmentsRepository(division, weekFromDate, weekToDate))
            {
                var events = repository
                    .GetAppointments()
                    .Where(a => a.IsPublicMaster)
                    .Select(extracurEventMapper.Map)
                    .OrderBy(c => c.Start)
                    .ToList();

                var eventsDays = events
                    .Where(e => e.Start >= weekFromDate)
                    .GroupBy(e => e.Start.Date)
                    .OrderBy(g => g.Key)
                    .Select(g =>
                    {
                        var day = g.Key;
                        var dayEvents = g.AsEnumerable();

                        return new ExtracurEventsContract.EventsDay
                        {
                            Day = day,
                            DayString = (language == LanguageCode.English) ?
                                day.ToString("dddd, MMMM d") :
                                day.ToString("dddd, d MMMM"),
                            DayEvents = hideRepeatedTimeFromEvents(dayEvents)
                        };
                    })
                    .ToList();

                return new ExtracurEventsContract
                {
                    Alias = division.Alias,
                    TimetableTitle = division.GetNameByLanguage(language),
                    EarlierEvents = events.Where(e => e.Start < weekFromDate),
                    EventsDays = eventsDays,
                    WeekDisplayText = DateTimeHelper.GetWeekDisplayText(language, weekFromDate, weekToDate),
                    PreviousWeekMonday = DateTimeHelper.GetDateStringForWeb(previousWeekFromDate),
                    WeekMonday = DateTimeHelper.GetDateStringForWeb(weekFromDate),
                    NextWeekMonday = DateTimeHelper.GetDateStringForWeb(nextWeekFromDate),
                    IsCurrentWeekReferenceAvailable = (currentWeekFromDate != weekFromDate),
                    HasEventsToShow = events.Any(e => !e.IsShowImmediateHidden),
                    IsPreviousWeekReferenceAvailable = true,
                    IsNextWeekReferenceAvailable = true
                };

                /// <summary>
                /// Returns events with <see cref="ExtracurEventContract.HasTheSameTimeAsPreviousItem"/> and
                /// <see cref="ExtracurEventContract.DisplayDateAndTimeIntervalString"/> set
                /// </summary>
                /// <param name="dayEvents"></param>
                /// <returns></returns>
                IEnumerable<ExtracurEventContract> hideRepeatedTimeFromEvents(
                    IEnumerable<ExtracurEventContract> dayEvents)
                {
                    // day events ordered by subject in time interval
                    var dayEventsOrdered = dayEvents
                        .GroupBy(de => de.TimeIntervalString)
                        .Select(g => g.OrderBy(e => e.Subject))
                        .SelectMany(g => g.AsEnumerable())
                        .ToList();

                    var eventWithTimeShown = dayEventsOrdered[0];
                    for (int i = 1; i < dayEventsOrdered.Count; i++)
                    {
                        var @event = dayEventsOrdered[i];

                        if (@event.TimeIntervalString == eventWithTimeShown.TimeIntervalString)
                        {
                            @event.HasTheSameTimeAsPreviousItem = true;
                            @event.DisplayDateAndTimeIntervalString =
                                @event.WithinTheSameDay ?
                                    @event.HasTheSameTimeAsPreviousItem ?
                                        string.Empty :
                                        @event.TimeIntervalString :
                                    @event.DateWithTimeIntervalString;
                        }
                        else
                        {
                            eventWithTimeShown = @event;
                        }
                    }
                    return dayEventsOrdered;
                }
            }
        }

        private ExtracurEventsContract GetMonthEvents(XtracurDivision division, DateTime? month = null)
        {
            var firstDayOfCurrentMonth = DateTimeHelper.GetFirstDayOfCurrentMonth();
            var firstDayOfChosenMonth = DateTimeHelper.GetFirstDayOfMonth(month);
            var firstDayOfTheNextMonth = DateTimeHelper.GetFirstDayOfTheNextMonth(firstDayOfChosenMonth);
            var firstDayOfThePreviousMonth = DateTimeHelper.GetFirstDayOfThePreviousMonth(firstDayOfChosenMonth);

            using (var repository = new XtracurAppointmentsRepository(division, firstDayOfChosenMonth, firstDayOfTheNextMonth))
            {
                var eventsByKind = repository
                    .GetAppointments()
                    .Where(a => a.IsPublicMaster)
                    .Select(extracurEventMapper.Map)
                    .OrderBy(c => c.Start)
                    .OrderBy(c => c.OrderIndex)
                    .GroupBy(c => c.SubkindDisplayName)
                    .Select(g => new ExtracurEventsContract.EventsByKind
                    {
                        Kind = g.Key,
                        Events = g.AsEnumerable()
                    })
                    .ToList();

                return new ExtracurEventsContract
                {
                    Alias = division.Alias,
                    TimetableTitle = division.GetNameByLanguage(language),
                    IsCurrentMonthReferenceAvailable = firstDayOfCurrentMonth != firstDayOfChosenMonth,
                    ChosenMonthDisplayText = firstDayOfChosenMonth.ToString("MMMM yyyy"),
                    PreviousMonthDisplayText = $"« {firstDayOfThePreviousMonth.ToString("MMMM yyyy")}",
                    PreviousMonthDate = DateTimeHelper.GetDateStringForWeb(firstDayOfThePreviousMonth),
                    NextMonthDisplayText = $"{firstDayOfTheNextMonth.ToString("MMMM yyyy")} »",
                    NextMonthDate = DateTimeHelper.GetDateStringForWeb(firstDayOfTheNextMonth),
                    KindsEvents = eventsByKind,
                    ShowGroupingCaptions = eventsByKind.Count() > 1,
                    HasEventsToShow = eventsByKind.Any(eg => eg.Events.Any(e => !e.IsShowImmediateHidden))
                };
            }
        }
    }
}