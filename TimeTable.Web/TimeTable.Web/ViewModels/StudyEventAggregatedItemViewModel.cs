using System;
using System.Collections.Generic;
using System.Linq;
using SpbuEducation.TimeTable.Common.Web.ViewModels;
using SpbuEducation.TimeTable.Common.Web.Helpers;
using SpbuEducation.TimeTable.Helpers.Multilingual;
using static SpbuEducation.TimeTable.Web.Helpers.StudyEventsViewModelHelper.StudyEventAggregateDatesHelper;
using SpbuEducation.TimeTable.BusinessObjects.Personnel;

namespace SpbuEducation.TimeTable.Web.ViewModels
{
    public class StudyEventAggregatedItemViewModel : IEventLocationsViewModel, IEventWithSubjectViewModel
    {
        public TimeSpan Start { get; private set; }
        public TimeSpan End { get; private set; }
        public IDateTimeViewModel DateTime { get; private set; }
        public string ClassName => null;

        #region IEventWithSubjectViewModel
        public string Subject { get; private set; }
        public string Cohort { get; private set; }
        public bool ShowCohort { get; private set; }
        public string SubjectTooltip => Properties.Resources.Subject;
        #endregion

        #region IEventLocationsViewModel
        public IEnumerable<ContingentUnitItemViewModel> ContingentUnits { get; set; }
        public int? EducatorId { get; private set; }
        public string EducatorsClassName => null;
        public string EducatorsDisplayText { get; private set; }
        public string EducatorsTooltip => Resources.Resources.Educators;
        public IEnumerable<StudyEventLocationItemViewModel> EventLocations { get; private set; }
        public bool ForEducator { get; private set; }
        public string LocationsClassName => null;
        public string LocationsDisplayText => string.Join("; ", EventLocations.Select(el => el.DisplayName));
        public string LocationsTooltip => Properties.Resources.Locations;
        #endregion

        public static StudyEventAggregatedItemViewModel Build(
            StudyEventAggregateDatesHelperKey aggregateKey,
            IEnumerable<DateTime> dates,
            EducatorMasterPerson forEducator)
        {
            LanguageCode language = CultureHelper.CurrentLanguage;
            return new StudyEventAggregatedItemViewModel
            {
                ContingentUnits = aggregateKey
                    .ContingentUnits
                    .Select(cu => ContingentUnitItemViewModel.Build(cu)),
                ForEducator = (forEducator != null),
                EducatorId = forEducator?.Id,
                Start = aggregateKey.Start,
                End = aggregateKey.End,
                Subject = aggregateKey.Subject,
                Cohort = aggregateKey.Cohort,
                ShowCohort = aggregateKey.ShowCohort,
                EventLocations = aggregateKey.EventLocations.Select(el => new StudyEventLocationItemViewModel(el)),
                DateTime = DateTimeAggregatedViewModel.Build(dates, aggregateKey.Start, aggregateKey.End),
                EducatorsDisplayText = aggregateKey.EducatorsDisplayText
            };
        }
    }
}
