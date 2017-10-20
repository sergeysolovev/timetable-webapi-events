using SpbuEducation.TimeTable.Appointments;
using SpbuEducation.TimeTable.BusinessObjects.Events;
using SpbuEducation.TimeTable.BusinessObjects.RealEstate;
using SpbuEducation.TimeTable.Helpers.Multilingual;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.Helpers
{
    internal class AggregatedContingent
    {
        public readonly AggregatedContingentKey Key;
        public readonly TimeEventAppointment Appointment;

        public AggregatedContingent(TimeEventAppointment appointment, LanguageCode language)
        {
            Key = new AggregatedContingentKey(appointment, language);
            Appointment = appointment;
        }

        internal class AggregatedContingentKey
        {
            public readonly StudyEventsTimeTableKindCode StudyEventsTimeTableKindCode;
            public readonly DateTime Start;
            public readonly DateTime End;
            public readonly string Subject;
            public readonly IEnumerable<EventLocation> EventLocations;
            public readonly string EducatorsDisplayText;
            public readonly bool DontShowEndTimeOnWeb;

            private readonly LanguageCode language;

            public AggregatedContingentKey(TimeEventAppointment appointment, LanguageCode language)
            {
                this.language = language;

                StudyEventsTimeTableKindCode = appointment.StudyEventsTimeTableKind != null
                    ? appointment.StudyEventsTimeTableKind.Code
                    : StudyEventsTimeTableKindCode.Unknown;
                Start = appointment.Start;
                End = appointment.End;
                Subject = appointment.GetSubjectByLanguage(language);
                EventLocations = appointment.EventLocations;
                EducatorsDisplayText = appointment.GetEducatorsDisplayTextByLanguage(language);
                DontShowEndTimeOnWeb = appointment.DontShowEndTimeOnWeb;
            }

            public override bool Equals(object obj)
            {
                AggregatedContingentKey o = obj as AggregatedContingentKey;
                if (o == null) return false;
                IEnumerable<Location> tloc = this.EventLocations.Select(el => el.Location);
                IEnumerable<Location> oloc = o.EventLocations.Select(el => el.Location);
                return this.StudyEventsTimeTableKindCode == o.StudyEventsTimeTableKindCode
                    && DateTime.Equals(this.Start, o.Start)
                    && DateTime.Equals(this.End, o.End)
                    && string.Equals(this.Subject, o.Subject)
                    && tloc.Count() == oloc.Count() && !tloc.Except(oloc).Any();
            }

            public override int GetHashCode()
            {
                int hash = StudyEventsTimeTableKindCode.GetHashCode() ^ Start.GetHashCode() ^ End.GetHashCode() ^ Subject.GetHashCode();
                foreach (EventLocation sel in EventLocations) hash ^= sel.Location.Oid.GetHashCode();
                return hash;
            }
        }
    }
}