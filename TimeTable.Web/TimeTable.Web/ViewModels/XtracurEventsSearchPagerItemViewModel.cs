using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SpbuEducation.TimeTable.Common.Web.ViewModels;

namespace SpbuEducation.TimeTable.Web.ViewModels
{
    public class XtracurEventsSearchPagerItemViewModel : IViewModel
    {
        public int Offset { get; private set; }
        public string DisplayText { get; private set; }
        public bool IsEnabled { get; private set; }
        public bool IsActive { get; private set; }
        public string Class { get; private set; }

        public static XtracurEventsSearchPagerItemViewModel Build(string text, int offset, bool isEnabled, bool isActive)
        {
            return new XtracurEventsSearchPagerItemViewModel
            {
                Offset = offset,
                DisplayText = text,
                IsEnabled = isEnabled,
                IsActive = isActive,
                Class = RenderCssClass(isEnabled, isActive)
            };
        }

        private static string RenderCssClass(bool isEnabled, bool isActive)
        {
            if (isActive)
            {
                return "active";
            }
            else if (!isEnabled)
            {
                return "disabled";
            }

            return String.Empty;
        }
    }
}
