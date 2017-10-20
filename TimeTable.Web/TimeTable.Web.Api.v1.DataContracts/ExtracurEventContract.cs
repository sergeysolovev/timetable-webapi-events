using System;

namespace SpbuEducation.TimeTable.Web.Api.v1.DataContracts
{   
    /// <summary>
    /// Extracurricular Event Data Contract
    /// </summary>
    public class ExtracurEventContract
    {
        /// <summary>
        /// Id
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
        /// Time Interval String
        /// </summary>
        public string TimeIntervalString { get; set; }

        /// <summary>
        /// Date with Time Interval String
        /// </summary>
        public string DateWithTimeIntervalString { get; set; }

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
        /// Display Date and Time Interval String
        /// </summary>
        public string DisplayDateAndTimeIntervalString { get; set; }

        /// <summary>
        /// Deprecated Field (from view model)
        /// </summary>
        public int ViewKind { get; set; }

        /// <summary>
        /// Division Alias
        /// </summary>
        public string DivisionAlias { get; set; }

        /// <summary>
        /// Zero-based number of event in recurrence chain
        /// </summary>
        public int? RecurrenceIndex { get; set; }

        /// <summary>
        /// Full Date with Time Interval String
        /// </summary>
        public string FullDateWithTimeIntervalString { get; set; }

        /// <summary>
        /// Year
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Determines whether to show year (from view model)
        /// </summary>
        public bool ShowYear { get; set; }

        /// <summary>
        /// Deprecated Field (from view model)
        /// </summary>
        public bool ShowImmediate { get; set; }

        /// <summary>
        /// Deprecated Field (from view model)
        /// </summary>
        public bool IsShowImmediateHidden { get; set; }

        /// <summary>
        /// Determines whether event has agenda
        /// </summary>
        public bool HasAgenda { get; set; }

        /// <summary>
        /// Determines whether event is a part of recurrence chain
        /// </summary>
        public bool IsRecurrence { get; set; }

        /// <summary>
        /// Subkind Display Name
        /// </summary>
        public string SubkindDisplayName { get; set; }

        /// <summary>
        /// Subkind Order Index (from view model)
        /// </summary>
        public decimal OrderIndex { get; set; }

        /// <summary>
        /// Location
        /// </summary>
        public AddressLocationContract Location { get; set; }

        /// <summary>
        /// Determines whether event is empty
        /// </summary>
        public bool IsEmpty { get; set; }

        /// <summary>
        /// Deprecated Field (from view model)
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Deprecated Field (from view model)
        /// </summary>
        public string FromDateString { get; set; }

        /// <summary>
        /// Deprecated Field (from view model)
        /// </summary>
        public bool IsPhys { get; set; }

        /// <summary>
        /// Responsible Person Contacts
        /// </summary>
        public string ResponsiblePersonContacts { get; set; }
    }
}
