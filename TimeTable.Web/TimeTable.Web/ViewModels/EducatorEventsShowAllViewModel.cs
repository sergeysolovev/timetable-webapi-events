using System;
using System.Collections.Generic;
using System.Linq;
using SpbuEducation.TimeTable.Common.Web.Helpers;
using SpbuEducation.TimeTable.Common.Web.Breadcrumb;
using SpbuEducation.TimeTable.Web.Helpers;
using SpbuEducation.TimeTable.BusinessObjects.Personnel;
using SpbuEducation.TimeTable.Helpers.Multilingual;

namespace SpbuEducation.TimeTable.Web.ViewModels
{
    public class EducatorEventsShowAllViewModel : BreadcrumbViewModel
    {
        public string EducatorDisplayText { get; private set; }
        public string DateRangeDisplayText { get; private set; }
        public string Title { get; private set; }
        public int EducatorMasterId { get; private set; }
        public bool IsSpringTerm { get; private set; }
        public bool SpringTermLinkAvailable { get; private set; }
        public bool AutumnTermLinkAvailable { get; private set; }
        public bool HasEvents { get; private set; }
        public IEnumerable<StudyEventAggregatedDayItemViewModel> EducatorEventsDays { get; private set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        
        public static EducatorEventsShowAllViewModel Build(EducatorMasterPerson educatorMasterPerson, int? next)
        {
            bool showNext = next.HasValue && next.Value > 0;
            LanguageCode language = CultureHelper.CurrentLanguage;
            string educatorDisplayText = educatorMasterPerson.GetDisplayNameByLanguage(language);
            string educatorLongDisplayText = educatorMasterPerson.GetLongDisplayNameByLanguage(language);
            DateTime summerTermBoundary = new DateTime(DateTime.Now.Month == 1 ? DateTime.Now.Year - 1 : DateTime.Now.Year, 8, 1);
            DateTime winterTermBoundary = new DateTime(DateTime.Now >= summerTermBoundary  && DateTime.Now.Month > 1 ? DateTime.Now.Year + 1 : DateTime.Now.Year, 2, 1);
            DateTime nextSummerTermBoundary = new DateTime(summerTermBoundary.Year + 1, summerTermBoundary.Month, summerTermBoundary.Day);
            DateTime nextWinterTermBoundary = new DateTime(winterTermBoundary.Year + 1, winterTermBoundary.Month, winterTermBoundary.Day);
            bool isSpringTerm = winterTermBoundary < DateTime.Now && DateTime.Now < summerTermBoundary;
            DateTime fromDate = showNext
                ? (isSpringTerm ? summerTermBoundary : winterTermBoundary)
                : (isSpringTerm ? winterTermBoundary : summerTermBoundary);
            DateTime toDate = showNext
                ? (isSpringTerm ? nextWinterTermBoundary : nextSummerTermBoundary)
                : (isSpringTerm ? summerTermBoundary : winterTermBoundary);
            string title = string.Format("{0} {1}", Resources.Resources.TimetableForEducator,
                educatorLongDisplayText);
            IEnumerable<StudyEventAggregatedDayItemViewModel> educatorEventsDays = StudyEventsViewModelHelper.GetEducatorAggregatedEventsDays(educatorMasterPerson, fromDate, toDate);

            return new EducatorEventsShowAllViewModel
            {
                Next = next,
                From = fromDate,
                To = toDate,
                EducatorDisplayText = educatorDisplayText,
                EducatorLongDisplayText = educatorLongDisplayText,
                DateRangeDisplayText = string.Format("{0:d MMMM yyyy} - {1:d MMMM yyyy}", fromDate, toDate),
                Title = title,
                EducatorMasterId = educatorMasterPerson.Id,
                EducatorEventsDays = educatorEventsDays,
                IsSpringTerm = isSpringTerm,
                SpringTermLinkAvailable = isSpringTerm == showNext,
                AutumnTermLinkAvailable = isSpringTerm != showNext,
                HasEvents = educatorEventsDays.Any(seadivm => seadivm.DayStudyEvents.Any()),
                Breadcrumb = new Breadcrumb()
                {
                    BreadcrumbHelper.GetBreadcrumbRootItem(false),
                    BreadcrumbHelper.GetBreadcrumbEducatorsItem(false),
                    new BreadcrumbItem
                    {
                        IsActive = true,
                        DisplayText = educatorDisplayText,
                    }
                }
            };
        }

        public string EducatorLongDisplayText { get; set; }
        public int? Next { get; set; }
    }
}
