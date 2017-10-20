using SpbuEducation.TimeTable.Web.Api.v1.DataContracts;
using System;

namespace SpbuEducation.TimeTable.Web.Api.v1.Http.ModelBinding
{
    /// <summary>
    /// Parameter parser
    /// </summary>
    public class ParameterParser
    {
        /// <summary>
        /// Tries to parse a value as <see cref="Nullable{DateTime}"/>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryParseNullableDateTime(string value, out DateTime? result)
        {
            if (string.IsNullOrEmpty(value))
            {
                result = null;
                return true;
            }

            DateTime resultValue;
            if (DateTime.TryParse(value, out resultValue))
            {
                result = resultValue;
                return true;
            }

            result = null;
            return false;
        }

        /// <summary>
        /// Tries to parse a value as <see cref="Nullable{Int32}"/>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryParseNullableInt32(string value, out int? result)
        {
            if (string.IsNullOrEmpty(value))
            {
                result = null;
                return true;
            }

            int resultValue;
            if (int.TryParse(value, out resultValue))
            {
                result = resultValue;
                return true;
            }

            result = null;
            return false;
        }

        /// <summary>
        /// Tries to parse a seating value as <see cref="Nullable{Seating}"/>
        /// </summary>
        /// <param name="seating"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryParseNullableSeating(string seating, out Seating? result)
        {
            if (string.IsNullOrEmpty(seating))
            {
                result = null;
                return true;
            }

            Seating resultValue;
            if (Enum.TryParse(seating, ignoreCase: true, result: out resultValue))
            {
                if (Enum.IsDefined(typeof(Seating), resultValue))
                {
                    result = resultValue;
                    return true;
                }
            }

            result = null;
            return false;
        }
    }
}
