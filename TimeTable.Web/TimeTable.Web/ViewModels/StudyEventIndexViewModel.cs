using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SpbuEducation.TimeTable.BusinessObjects.Education;
using DevExpress.Xpo;
using SpbuEducation.TimeTable.BusinessObjects.Contingent;
using SpbuEducation.TimeTable.BusinessObjects.Events;
using DevExpress.XtraScheduler;
using SpbuEducation.TimeTable.Appointments;
using SpbuEducation.TimeTable.Common.Web.ViewModels;
using SpbuEducation.TimeTable.Common.Web.Helpers;

namespace SpbuEducation.TimeTable.Web.ViewModels
{
    public class StudyEventIndexViewModel : IViewModel
    {
        public StudyEventsTimeTableKindCode StudyEventsTimeTableKindCode;
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Subject { get; set; }
        public string TimeIntervalString { get; set; }
        public string DateWithTimeIntervalString { get; set; }
        public string LocationsDisplayText { get; set; }
        public string EducatorsDisplayText { get; set; }
        public bool HasEducators { get; set; }
        public IEnumerable<TimeTableStudyEventLocationViewModel> EventLocations { get; set; }

        public static StudyEventIndexViewModel Build(TimeEventAppointment appointment)
        {
            var language = CultureHelper.CurrentLanguage;

            return new StudyEventIndexViewModel
            {
                StudyEventsTimeTableKindCode = GetStudyEventsTimeTableKindCode(appointment),
                Start = appointment.Start,
                End = appointment.End,
                TimeIntervalString = appointment.GetTimeIntervalByLanguage(language),
                DateWithTimeIntervalString = appointment.DateTimeIntervalString,
                EducatorsDisplayText = appointment.GetEducatorsDisplayTextByLanguage(language),
                LocationsDisplayText = appointment.GetLocationsDisplayTextByLanguage(language),
                HasEducators = !String.IsNullOrEmpty(appointment.EducatorsDisplayText),
                Subject = appointment.GetSubjectByLanguage(language),
                EventLocations = appointment.EventLocations.Select(el => TimeTableStudyEventLocationViewModel.Build(el))
            };
        }

        public static StudyEventsTimeTableKindCode GetStudyEventsTimeTableKindCode(TimeEventAppointment appointment)
        {
            if (appointment.StudyEventsTimeTableKind != null)
            {
                return appointment.StudyEventsTimeTableKind.Code;
            }
            return StudyEventsTimeTableKindCode.Unknown;
        }
    }
}
