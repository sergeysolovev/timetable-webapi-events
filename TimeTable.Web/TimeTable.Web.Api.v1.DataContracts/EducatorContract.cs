using System.Collections.Generic;

namespace SpbuEducation.TimeTable.Web.Api.v1.DataContracts
{
    /// <summary>
    /// Educator Data Contract
    /// </summary>
    public class EducatorContract
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Display Name
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Full Name
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Educator Employments
        /// </summary>
        public IEnumerable<Employment> Employments { get; set; }

        /// <summary>
        /// Educator Employment Data Contract
        /// </summary>
        public class Employment
        {
            /// <summary>
            /// Educator Employment Position
            /// </summary>
            public string Position { get; set; }

            /// <summary>
            /// Educator Employment Department
            /// </summary>
            public string Department { get; set; }
        }

        /// <summary>
        /// Educator Employment Equality Comparer
        /// </summary>
        public class EmploymentEqualityComparer : IEqualityComparer<Employment>
        {
            /// <summary>
            /// Equals
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public bool Equals(Employment x, Employment y)
            {
                return string.Equals(x.Department, y.Department) && string.Equals(x.Position, y.Position);
            }

            /// <summary>
            /// Gets Hash Code
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int GetHashCode(Employment obj)
            {
                return obj.Department.GetHashCode() + obj.Position.GetHashCode();
            }
        }
    }
}