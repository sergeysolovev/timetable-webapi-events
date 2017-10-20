using SpbuEducation.TimeTable.Web.Api.v1.Http.Errors;
using System;
using System.Web.Http.ExceptionHandling;

namespace SpbuEducation.TimeTable.Web.Api.v1.Http.ExceptionHandling
{
    /// <summary>
    /// Handles all unhandled exceptions
    /// </summary>
    public class CustomExceptionHandler : ExceptionHandler
    {
        private readonly IErrorsResultFactory errorsFactory;

        /// <summary>
        /// Creates an instance of <see cref="CustomExceptionHandler"/>
        /// </summary>
        /// <param name="errorsFactory"></param>
        public CustomExceptionHandler(IErrorsResultFactory errorsFactory)
        {
            this.errorsFactory = errorsFactory ??
                throw new ArgumentNullException(nameof(errorsFactory));
        }

        /// <summary>
        /// Determines whether to handle an exception.
        /// Overrides default behavior in the way that
        /// all incoming exceptions are being handled
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool ShouldHandle(ExceptionHandlerContext context) => true;

        /// <summary>
        /// Handles an unhandled exception and
        /// sets http result to a custom error contract
        /// if request context is available
        /// </summary>
        /// <param name="context"></param>
        public override void Handle(ExceptionHandlerContext context)
        {
            var config = context.RequestContext?.Configuration;
            var request = context.Request;
            if (config != null && request != null)
            {
                context.Result = errorsFactory.CreateInternalServerError(request, config,
                    "Something went wrong"
                );
            }
            else
            {
                base.Handle(context);
            }
        }
    }
}
