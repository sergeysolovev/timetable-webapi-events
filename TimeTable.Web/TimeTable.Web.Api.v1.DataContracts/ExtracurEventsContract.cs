using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SpbuEducation.TimeTable.Web.Api.v1.DataContracts
{
    /// <summary>
    /// Extracurricular Events Data Contract
    /// </summary>
    public partial class ExtracurEventsContract
    {
        /// <summary>
        /// Extracurricular Division's Alias
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Timetable Title (from view model)
        /// </summary>
        [JsonProperty("Title")]
        public string TimetableTitle { get; set; }

        /// <summary>
        /// Determines if there are events presented
        /// </summary>
        public bool HasEventsToShow { get; set; }

        #region Month-Specific
        /// <summary>
        /// Chosen Month Display Text (from view model)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ChosenMonthDisplayText { get; set; }

        /// <summary>
        /// Previous Month Display Text (from view model)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PreviousMonthDisplayText { get; set; }

        /// <summary>
        /// Previous Month Date (from view model)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PreviousMonthDate { get; set; }

        /// <summary>
        /// Next Month Display Text (from view model)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string NextMonthDisplayText { get; set; }

        /// <summary>
        /// Next Month Date (from view model)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string NextMonthDate { get; set; }

        /// <summary>
        /// Deprecated Field (from view model)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsCurrentMonthReferenceAvailable { get; set; }

        /// <summary>
        /// Deprecated Field (from view model)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? ShowGroupingCaptions { get; set; }

        /// <summary>
        /// Events grouped by kinds
        /// </summary>
        [JsonProperty("EventGroupings", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<EventsByKind> KindsEvents { get; set; }

        /// <summary>
        /// Events' Kind Data Contract
        /// </summary>
        public class EventsByKind
        {
            /// <summary>
            /// Events' Kind
            /// </summary>
            [JsonProperty("Caption")]
            public string Kind { get; set; }

            /// <summary>
            /// Events
            /// </summary>
            public IEnumerable<ExtracurEventContract> Events { get; set; }
        }
        #endregion

        #region Week-Specific
        /// <summary>
        /// Deprecated Field (from view model)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsPreviousWeekReferenceAvailable { get; set; }

        /// <summary>
        /// Deprecated Field (from view model)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsNextWeekReferenceAvailable { get; set; }

        /// <summary>
        /// Deprecated Field (from view model)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsCurrentWeekReferenceAvailable { get; set; }

        /// <summary>
        /// Previous Week Monday (from view model)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PreviousWeekMonday { get; set; }

        /// <summary>
        /// Next Week Monday (from view model)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string NextWeekMonday { get; set; }

        /// <summary>
        /// Presented week's display text (from view model)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WeekDisplayText { get; set; }

        /// <summary>
        /// Presented week's monday date
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WeekMonday { get; set; }

        /// <summary>
        /// Earlier events with start datetime before requested range
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<ExtracurEventContract> EarlierEvents { get; set; }

        /// <summary>
        /// Events grouped by days
        /// </summary>
        [JsonProperty("Days", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<EventsDay> EventsDays { get; set; }

        /// <summary>
        /// Events' Day Data Contract
        /// </summary>
        public class EventsDay
        {
            /// <summary>
            /// Day datetime
            /// </summary>
            public DateTime Day { get; set; }

            /// <summary>
            /// Day Display Text
            /// </summary>
            public string DayString { get; set; }

            /// <summary>
            /// Events
            /// </summary>
            public IEnumerable<ExtracurEventContract> DayEvents { get; set; }
        }
        #endregion
    }
}
