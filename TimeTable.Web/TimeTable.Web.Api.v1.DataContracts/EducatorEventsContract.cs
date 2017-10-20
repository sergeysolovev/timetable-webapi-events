using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SpbuEducation.TimeTable.Web.Api.v1.DataContracts
{
    /// <summary>
    /// Educator's Events Data Contract
    /// </summary>
    public class EducatorEventsContract
    {
        /// <summary>
        /// Educator's Timetable Title (from a view model)
        /// </summary>
        [JsonProperty("Title")]
        public string TimetableTitle { get; set; }

        /// <summary>
        /// Display Text
        /// </summary>
        [JsonProperty("EducatorDisplayText")]
        public string DisplayText { get; set; }

        /// <summary>
        /// Long Display Text
        /// </summary>
        [JsonProperty("EducatorLongDisplayText")]
        public string LongDisplayText { get; set; }

        /// <summary>
        /// Educator Display Text
        /// </summary>
        public string DateRangeDisplayText { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        [JsonProperty("EducatorMasterId")]
        public int Id { get; set; }

        /// <summary>
        /// Determines whether the events for the spring study term are presented
        /// </summary>
        public bool IsSpringTerm { get; set; }

        /// <summary>
        /// Events' range start
        /// </summary>
        public DateTime From { get; set; }

        /// <summary>
        /// Events' range end
        /// </summary>
        public DateTime To { get; set; }

        /// <summary>
        /// Determines whether the events for the next study term are presented
        /// </summary>
        [JsonProperty("Next")]
        public int? IsNextTerm { get; set; }

        /// <summary>
        /// Determines whether the spring term link is available (from view model)
        /// </summary>
        public bool SpringTermLinkAvailable { get; set; }

        /// <summary>
        /// Determines whether the autumn term link is available (from view model)
        /// </summary>
        public bool AutumnTermLinkAvailable { get; set; }

        /// <summary>
        /// Determines if there are any events presented
        /// </summary>
        public bool HasEvents { get; set; }

        /// <summary>
        /// Events grouped by days of week
        /// </summary>
        public IEnumerable<EventsDay> EducatorEventsDays { get; set; }

        /// <summary>
        /// Educator's Event Data Contract
        /// </summary>
        public class Event
        {
            /// <summary>
            /// Start
            /// </summary>
            public TimeSpan Start { get; set; }

            /// <summary>
            /// End
            /// </summary>
            public TimeSpan End { get; set; }
            
            /// <summary>
            /// Subject
            /// </summary>
            public string Subject { get; set; }

            /// <summary>
            /// Time Interval
            /// </summary>
            public string TimeIntervalString { get; set; }

            /// <summary>
            /// Dates
            /// </summary>
            public IEnumerable<string> Dates { get; set; }

            /// <summary>
            /// Educators Display Text
            /// </summary>
            public string EducatorsDisplayText { get; set; }

            /// <summary>
            /// Determines whether the event is cancelled
            /// </summary>
            public bool IsCanceled { get; set; }

            /// <summary>
            /// Timetable Kind Code
            /// </summary>
            public int StudyEventsTimeTableKindCode { get; set; }

            /// <summary>
            /// Educator Ids
            /// </summary>
            public IEnumerable<Tuple<int, string>> EducatorIds { get; set; }

            /// <summary>
            /// Event Locations
            /// </summary>
            public IEnumerable<EventLocationContract> EventLocations { get; set; }

            /// <summary>
            /// Contingent Units' Names
            /// </summary>
            public IEnumerable<Tuple<string, string>> ContingentUnitNames { get; set; }
        }

        /// <summary>
        /// Educator Events' Day Data Contract
        /// </summary>
        public class EventsDay
        {
            /// <summary>
            /// Day of Week
            /// </summary>
            public DayOfWeek Day { get; set; }

            /// <summary>
            /// Day Display Text
            /// </summary>
            public string DayString { get; set; }

            /// <summary>
            /// Number of Events
            /// </summary>
            public int DayStudyEventsCount { get; set; }

            /// <summary>
            /// Events
            /// </summary>
            public IEnumerable<Event> DayStudyEvents { get; set; }
        }
    }
}
