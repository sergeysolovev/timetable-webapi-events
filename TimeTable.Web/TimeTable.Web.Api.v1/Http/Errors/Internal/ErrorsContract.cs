using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;

namespace SpbuEducation.TimeTable.Web.Api.v1.Http.Errors.Internal
{
    /// <summary>
    /// Local default for <see cref="Errors.ErrorsContract"/>
    /// </summary>
    [DataContract]
    internal class ErrorsContract : Errors.ErrorsContract
    {
        public ErrorsContract(HttpStatusCode statusCode, string message, IEnumerable<string> otherErrors = null)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            StatusCode = GetStatusCodeString(statusCode);
            Errors = (otherErrors == null) ?
                new List<string> { message } :
                new List<string> { message }.Concat(otherErrors).ToList();
        }

        public ErrorsContract(HttpStatusCode statusCode, IEnumerable<string> errors = null)
        {
            StatusCode = GetStatusCodeString(statusCode);
            Errors = errors?.ToList();
        }

        private string GetStatusCodeString(HttpStatusCode statusCode)
        {
            var code = (int)statusCode;
            var isErrorStatusCode = (code >= 400 && code < 600);
            if (!isErrorStatusCode)
            {
                throw new ArgumentException(nameof(statusCode));
            }

            return $"{code} ({statusCode})";
        }
    }
}