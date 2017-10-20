using NLog;
using System;
using System.Web.Http.ExceptionHandling;

namespace SpbuEducation.TimeTable.Web.Api.v1.Http.Logging
{
    /// <summary>
    /// Logs all unhandled exceptions
    /// </summary>
    public class CustomExceptionLogger : ExceptionLogger
    {
        private readonly ILogger logger;

        /// <summary>
        /// Creates an instance of <see cref="CustomExceptionLogger"/>
        /// </summary>
        /// <param name="logger"></param>
        public CustomExceptionLogger(ILogger logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Determines whether to log an exception.
        /// Overrides default behavior in the way that
        /// all incoming exceptions are being logged
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool ShouldLog(ExceptionLoggerContext context) => true;

        /// <summary>
        /// Logs an unhandled exception
        /// </summary>
        /// <param name="context"></param>
        public override void Log(ExceptionLoggerContext context)
        {
            logger.Error(context.Exception, context.Exception.Message);
        }
    }
}
