using SpbuEducation.TimeTable.BusinessObjects.Contingent;
using SpbuEducation.TimeTable.BusinessObjects.Events;
using SpbuEducation.TimeTable.BusinessObjects.RealEstate;
using SpbuEducation.TimeTable.Helpers.Multilingual;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.Helpers
{
    internal class AggregatedDates
    {
        public readonly AggregatedDatesKey Key;
        public readonly DateTime Date;

        public AggregatedDates(AggregatedContingent g, LanguageCode language)
        {
            Key = new AggregatedDatesKey(g, language);
            Date = g.Key.Start.Date;
        }

        internal class AggregatedDatesKey
        {
            public readonly StudyEventsTimeTableKindCode StudyEventsTimeTableKindCode;
            public readonly TimeSpan Start;
            public readonly TimeSpan End;
            public readonly string Subject;
            public readonly IEnumerable<EventLocation> EventLocations;
            public readonly IEnumerable<ContingentUnit> ContingentUnits;
            public readonly string EducatorsDisplayText;
            public readonly bool IsCanceled;
            public readonly bool DontShowEndTimeOnWeb;

            private readonly LanguageCode language;

            public AggregatedDatesKey(AggregatedContingent g, LanguageCode language)
            {
                this.language = language;

                StudyEventsTimeTableKindCode = g.Key.StudyEventsTimeTableKindCode;
                Start = g.Key.Start.TimeOfDay;
                End = g.Key.End.TimeOfDay;
                Subject = g.Key.Subject;
                EventLocations = g.Key.EventLocations;
                ContingentUnits = new[] { g.Appointment.ContingentUnit };
                EducatorsDisplayText = g.Key.EducatorsDisplayText;
                IsCanceled = g.Appointment.IsCancelled;
                DontShowEndTimeOnWeb = g.Key.DontShowEndTimeOnWeb;
            }

            public string GetTimeIntervalString()
            {
                return language == LanguageCode.English
                    ? $"{new DateTime(Start.Ticks):H:mm tt}" + (DontShowEndTimeOnWeb ? string.Empty : $"-{new DateTime(End.Ticks):H:mm tt}")
                    : $"{new DateTime(Start.Ticks):H:mm}" + (DontShowEndTimeOnWeb ? string.Empty : $"-{new DateTime(End.Ticks):H:mm}");
            }

            public override bool Equals(object obj)
            {
                AggregatedDatesKey o = obj as AggregatedDatesKey;
                if (o == null) return false;
                IEnumerable<Location> tloc = this.EventLocations.Select(el => el.Location);
                IEnumerable<Location> oloc = o.EventLocations.Select(el => el.Location);
                return this.StudyEventsTimeTableKindCode == o.StudyEventsTimeTableKindCode
                    && TimeSpan.Equals(this.Start, o.Start)
                    && TimeSpan.Equals(this.End, o.End)
                    && string.Equals(this.Subject, o.Subject)
                    && tloc.Count() == oloc.Count() && !tloc.Except(oloc).Any()
                    && this.ContingentUnits.Count() == o.ContingentUnits.Count() && !this.ContingentUnits.Except(o.ContingentUnits).Any();
            }

            public override int GetHashCode()
            {
                int hash = StudyEventsTimeTableKindCode.GetHashCode() ^ Start.GetHashCode() ^ End.GetHashCode() ^ Subject.GetHashCode();
                foreach (EventLocation sel in EventLocations) hash ^= sel.Location.Oid.GetHashCode();
                foreach (ContingentUnit cu in ContingentUnits) if (cu != null) hash ^= cu.Oid.GetHashCode();
                return hash;
            }
        }
    }
}