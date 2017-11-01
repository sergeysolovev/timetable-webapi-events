using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpbuEducation.TimeTable.Web.Api.v1.DataContracts
{
    public enum TimeTableKindСode
        {		
           /// <summary>		
            /// Unknow timetable kind		
            /// </summary>		
            Unknown = 0,		
            /// <summary>		
            /// Primary timetable kind		
            /// </summary>		
            Primary = 1,
            /// <summary>		
            /// Interim (intermediary) attestation		
            /// </summary>		
            Attestation = 2,
            /// <summary>		
            /// Final attestation		
            /// </summary>		
            Final = 3		
        }		
}
