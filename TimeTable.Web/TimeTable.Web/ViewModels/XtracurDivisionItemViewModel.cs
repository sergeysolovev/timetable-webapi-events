using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SpbuEducation.TimeTable.BusinessObjects.Education;
using DevExpress.Xpo;
using SpbuEducation.TimeTable.Common.Web.ViewModels;
using SpbuEducation.TimeTable.Helpers.Multilingual;
using SpbuEducation.TimeTable.Common.Web.Helpers;
using SpbuEducation.TimeTable.BusinessObjects.Events;

namespace SpbuEducation.TimeTable.Web.ViewModels
{
    public class XtracurDivisionItemViewModel : IViewModel
    {
        public string Name { get; set; }
        public string Alias { get; set; }

        public static XtracurDivisionItemViewModel Build(XtracurDivision xtracurDivision)
        {
            return new XtracurDivisionItemViewModel
            {
                Name = xtracurDivision.GetNameByLanguage(CultureHelper.CurrentLanguage),
                Alias = xtracurDivision.Alias
            };
        }
    }
}
