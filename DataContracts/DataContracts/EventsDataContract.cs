using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SpbuEducation.TimeTable.Web.Api.v1.DataContracts
{
    /// <summary>
    /// Stundent's Group Events Data Contract
    /// </summary>
    public class EventsDataContract
    {
        /// <summary>
        /// Count of Events
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// Url to the next page
        /// </summary>
        public Uri Next { get; set; }
        /// <summary>
        /// Url to the previous page
        /// </summary>
        public Uri Previous { get; set; }
        /// <summary>
        /// List of Events
        /// </summary>
        public IEnumerable<EventContract> Events { get; set; }

        /// <summary>
        /// Event Data Contract
        /// </summary>
        public class EventContract
        {
            /// <summary>
            /// Student's Group Id
            /// </summary>
            public int Id { get; set; }

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
            /// Determines whether event is cancelled
            /// </summary>
            public bool IsCancelled { get; set; }

            /// <summary>
            /// Timetable kind code (from view model)
            /// </summary>
            [JsonProperty("Timetable")]
            public int? TimetableKindCode;

            /// <summary>
            /// Group Id
            /// </summary>
            public string GroupId { get; set; }

            /// <summary>
            /// Division Id
            /// </summary>
            public string DivisionId { get; set; }

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
            /// Determines whether event is elective
            /// </summary>
            public bool IsFacultative { get; set; }

            /// <summary>
            /// Event's locations
            /// </summary>
            public IEnumerable<EventLocationContract> EventLocations { get; set; }

            /// <summary>
            /// Event Location Data Contract
            /// </summary>
            public class EventLocationContract
            {
                /// <summary>
                /// Latitude
                /// </summary>
                public double Latitude { get; set; }

                /// <summary>
                /// Longitude
                /// </summary>
                public double Longitude { get; set; }


            }


        }


    }
}
