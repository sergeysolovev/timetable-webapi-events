using Autofac;
using Autofac.Integration.WebApi;
using NLog;
using SpbuEducation.TimeTable.Web.Api.v1.Http.Errors;
using SpbuEducation.TimeTable.Web.Api.v1.Http.Errors.Internal;
using SpbuEducation.TimeTable.Web.Api.v1.Http.ModelBinding;
using System.Reflection;

namespace SpbuEducation.TimeTable.Web.Api.v1.DependencyInjection
{
    /// <summary>
    /// Web Api extension methods
    /// </summary>
    public static class WebApiExtensions
    {
        /// <summary>
        /// Composes web api services
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ContainerBuilder RegisterHttpServices(this ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<ErrorsResultFactory>().As<IErrorsResultFactory>();
            builder.RegisterType<ParameterParser>();

            return builder;
        }

        /// <summary>
        /// Composes logging services
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ContainerBuilder RegisterLoggingServices(this ContainerBuilder builder)
        {
            builder.Register(c => LogManager.GetLogger("WebApi")).As<ILogger>();

            return builder;
        }
    }
}