using SpbuEducation.TimeTable.Appointments;
using SpbuEducation.TimeTable.BusinessObjects.Events;
using SpbuEducation.TimeTable.Helpers;
using SpbuEducation.TimeTable.Web.Api.v1.DataContracts;
using System;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.Mappers
{
    internal class ExtracurEventMapper
    {
        private readonly AddressLocationMapper addressLocationMapper;

        public ExtracurEventMapper(AddressLocationMapper addressLocationMapper)
        {
            this.addressLocationMapper = addressLocationMapper ??
                throw new ArgumentNullException(nameof(addressLocationMapper));
        }

        public ExtracurEventContract Map(TimeEventAppointment a)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));

            var subkind = a.EventSubkind;
            var division = subkind?.Kind?.XtracurDivision;
            var viewKind = division?.WebViewKind ?? XtracurEventsWebViewKind.Week;
            var fromDate = viewKind == XtracurEventsWebViewKind.Month ?
                DateTimeHelper.GetFirstDayOfMonth(a.Start.Date) :
                DateTimeHelper.GetWeekStart(a.Start.Date);
            var fromDateString = DateTimeHelper.GetDateStringForWeb(fromDate);
            var withinTheSameDay = a.WithinTheSameDay;
            var dateWithTimeIntervalString = a.ShortMonthDateTimeIntervalString;
            var fullDateWithTimeIntervalString = a.DateTimeIntervalString;
            var timeIntervalString = a.TimeIntervalString;
            var displayDateAndTimeIntervalString =
                viewKind == XtracurEventsWebViewKind.Month ?
                    dateWithTimeIntervalString :
                    withinTheSameDay ?
                        timeIntervalString :
                        dateWithTimeIntervalString;
            var recurrenceIndex = a.IsBase ? (int?)null : a.RecurrenceIndex;
            var divisionAlias = division.Alias ?? string.Empty;

            return new ExtracurEventContract
            {
                Id = a.IntegerId,
                Start = a.Start,
                End = a.End,
                Subject = a.Subject,
                TimeIntervalString = timeIntervalString,
                EducatorsDisplayText = a.EducatorsDisplayText,
                LocationsDisplayText = a.LocationsDisplayText,
                HasEducators = !string.IsNullOrEmpty(a.EducatorsDisplayText),
                IsCancelled = a.IsCancelled,
                ContingentUnitsDisplayTest = a.ContingentUnitName,
                IsStudy = a.IsStudy,
                AllDay = a.AllDay,
                WithinTheSameDay = withinTheSameDay,
                IsRecurrence = (recurrenceIndex != null),
                RecurrenceIndex = recurrenceIndex,
                FromDate = fromDate,
                FromDateString = fromDateString,
                DivisionAlias = divisionAlias,
                DateWithTimeIntervalString = dateWithTimeIntervalString,
                FullDateWithTimeIntervalString = fullDateWithTimeIntervalString,
                DisplayDateAndTimeIntervalString = displayDateAndTimeIntervalString,
                Year = a.Start.Year,
                ShowYear = false,
                ShowImmediate = false,
                IsShowImmediateHidden = false,
                HasAgenda = a.IsComposite && a.IsMaster,
                SubkindDisplayName = subkind?.Name ?? string.Empty,
                OrderIndex = subkind?.OrderIndex ?? 0,
                Location = addressLocationMapper.Map(a.FirstEventLocation, a.XtracurAddress),
                IsEmpty = false,
                ResponsiblePersonContacts = a.ResponsiblePersonContacts,
                IsPhys = a.EventKind?.Name == "Физкультура",
                ViewKind = (int)viewKind
            };
        }
    }
}