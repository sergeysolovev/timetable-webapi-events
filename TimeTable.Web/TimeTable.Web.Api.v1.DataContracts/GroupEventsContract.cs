using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SpbuEducation.TimeTable.Web.Api.v1.DataContracts
{
    /// <summary>
    /// Student Group's Events Data Contract
    /// </summary>
    public class GroupEventsContract
    {
        /// <summary>
        /// Student's Group Id
        /// </summary>
        [JsonProperty("StudentGroupId")]
        public int Id { get; set; }

        /// <summary>
        /// Display Name
        /// </summary>
        [JsonProperty("StudentGroupDisplayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Timetable Display Name
        /// </summary>
        public string TimeTableDisplayName { get; set; }

        /// <summary>
        /// Previous Week Monday (from view model)
        /// </summary>
        public string PreviousWeekMonday { get; set; }

        /// <summary>
        /// Next Week Monday (from view model)
        /// </summary>
        public string NextWeekMonday { get; set; }

        /// <summary>
        /// Deprecated Field (from view model)
        /// </summary>
        public bool IsPreviousWeekReferenceAvailable { get; set; }
        
        /// <summary>
        /// Deprecated Field (from view model)
        /// </summary>
        public bool IsNextWeekReferenceAvailable { get; set; }

        /// <summary>
        /// Deprecated Field (from view model)
        /// </summary>
        public bool IsCurrentWeekReferenceAvailable { get; set; }

        /// <summary>
        /// Requested week's display text
        /// </summary>
        public string WeekDisplayText { get; set; }

        /// <summary>
        /// Requested week's monday date
        /// </summary>
        public string WeekMonday { get; set; }

        /// <summary>
        /// Events grouped by days
        /// </summary>
        public IEnumerable<EventsDay> Days { get; set; }

        /// <summary>
        /// Event Data Contract
        /// </summary>
        public class Event
        {
            /// <summary>
            /// Start
            /// </summary>
            public DateTime Start { get; set; }

            /// <summary>
            /// End
            /// </summary>
            public DateTime End { get; set; }

            /// <summary>
            /// Subject
            /// </summary>
            public string Subject { get; set; }

            /// <summary>
            /// Time Interval String
            /// </summary>
            public string TimeIntervalString { get; set; }

            /// <summary>
            /// Date with Time Interval String
            /// </summary>
            public string DateWithTimeIntervalString { get; set; }

            /// <summary>
            /// Display Date and Time Interval String
            /// </summary>
            public string DisplayDateAndTimeIntervalString { get; set; }

            /// <summary>
            /// Locations Display Text
            /// </summary>
            public string LocationsDisplayText { get; set; }

            /// <summary>
            /// Educators Display Text
            /// </summary>
            public string EducatorsDisplayText { get; set; }

            /// <summary>
            /// Determines whether event has educators
            /// </summary>
            public bool HasEducators { get; set; }

            /// <summary>
            /// Determines whether event is cancelled
            /// </summary>
            public bool IsCancelled { get; set; }

            /// <summary>
            /// Timetable kind code (from view model)
            /// </summary>
            public int? StudyEventsTimeTableKindCode;

            /// <summary>
            /// Contingent Unit's Name (from view model)
            /// </summary>
            public string ContingentUnitName { get; set; }

            /// <summary>
            /// Contingent Unit's Division and Course (from view model)
            /// </summary>
            public string DivisionAndCourse { get; set; }

            /// <summary>
            /// Determines whether the event was assigned
            /// after all the events have been planned
            /// </summary>
            public bool IsAssigned { get; set; }

            /// <summary>
            /// Determines whether event's time was changed
            /// </summary>
            public bool TimeWasChanged { get; set; }

            /// <summary>
            /// Determines whether event's locations were changed
            /// </summary>
            public bool LocationsWereChanged { get; set; }

            /// <summary>
            /// Determines whether event's educators were substituted
            /// </summary>
            public bool EducatorsWereReassigned { get; set; }

            /// <summary>
            /// Elective Disipline Count
            /// </summary>
            public int ElectiveDisciplinesCount { get; set; }

            /// <summary>
            /// Determines whether event is elective
            /// </summary>
            public bool IsElective { get; set; }

            /// <summary>
            /// Determines whether event has the same time as the previous
            /// event in the list (from view model)
            /// </summary>
            public bool HasTheSameTimeAsPreviousItem { get; set; }

            /// <summary>
            /// Deprecated Field
            /// </summary>
            public string ContingentUnitsDisplayTest { get; set; }

            /// <summary>
            /// Determines whether event is a study event
            /// </summary>
            public bool IsStudy { get; set; }

            /// <summary>
            /// Determines whether event is for all day (no time defined)
            /// </summary>
            public bool AllDay { get; set; }

            /// <summary>
            /// Determines whether event occurs within the same day
            /// </summary>
            public bool WithinTheSameDay { get; set; }

            /// <summary>
            /// Event's locations
            /// </summary>
            public IEnumerable<EventLocationContract> EventLocations { get; set; }

            /// <summary>
            /// Educators' Ids
            /// </summary>
            public IEnumerable<Tuple<int, string>> EducatorIds { get; set; }
        }

        /// <summary>
        /// Events' Day Data Contract
        /// </summary>
        public class EventsDay
        {
            /// <summary>
            /// Day's datetime
            /// </summary>
            public DateTime Day { get; set; }

            /// <summary>
            /// Day's display text
            /// </summary>
            public string DayString { get; set; }
            
            /// <summary>
            /// Events
            /// </summary>
            public IEnumerable<Event> DayStudyEvents { get; set; }
        }
    }
}