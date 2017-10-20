using Autofac;
using Autofac.Integration.WebApi;
using Newtonsoft.Json.Serialization;
using NLog;
using SpbuEducation.TimeTable.Web.Api.v1.DependencyInjection;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.DependencyInjection;
using SpbuEducation.TimeTable.Web.Api.v1.Http.Errors;
using SpbuEducation.TimeTable.Web.Api.v1.Http.ExceptionHandling;
using SpbuEducation.TimeTable.Web.Api.v1.Http.Logging;
using SpbuEducation.TimeTable.Web.Api.v1.Http.MessageHandlers;
using SpbuEducation.TimeTable.Web.Api.v1.Localization.DependencyInjection;
using Swashbuckle.Application;
using System;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using WebApiThrottle;

namespace SpbuEducation.TimeTable.Web.Api.v1
{
    /// <summary>
    /// Web Api Entry Point
    /// </summary>
    public static class WebApi
    {
        /// <summary>
        /// Route Prefix
        /// </summary>
        public const string RoutePrefix = "api/v1";

        /// <summary>
        /// Path to api docs
        /// </summary>
        public const string HelpUrl = "api/docs";

        /// <summary>
        /// Bootstraps web api
        /// </summary>
        public static void Bootstrap()
        {
            GlobalConfiguration.Configure(config =>
            {
                SetRoutes(config);
                SetFormatters(config);
                SetDependencyResolver(config);
                SetMessageHandlers(config);
                SetThrottling(config);
                ComposeCustomExceptionHandler(config);
                ComposeCustomExceptionLogger(config);
            });
        }

        /// <summary>
        /// Replaces default instance of <see cref="IExceptionHandler"/>
        /// <remarks>
        /// Sadly we can't compose this through DI container
        /// </remarks>
        /// </summary>
        /// <param name="config"></param>
        private static void ComposeCustomExceptionHandler(HttpConfiguration config)
        {
            var resolver = config.DependencyResolver;
            var errorsFactory = resolver.GetService(typeof(IErrorsResultFactory))
                as IErrorsResultFactory;

            if (errorsFactory != null)
            {
                var customHandler = new CustomExceptionHandler(errorsFactory);
                config.Services.Replace(typeof(IExceptionHandler), customHandler);
            }
        }

        /// <summary>
        /// Replaces default instance of <see cref="IExceptionLogger"/>
        /// <remarks>
        /// Sadly we can't compose this through DI container
        /// </remarks>
        /// </summary>
        /// <param name="config"></param>
        private static void ComposeCustomExceptionLogger(HttpConfiguration config)
        {
            var resolver = config.DependencyResolver;
            var logger = resolver.GetService(typeof(ILogger)) as ILogger;

            if (logger != null)
            {
                var customLogger = new CustomExceptionLogger(logger);
                config.Services.Add(typeof(IExceptionLogger), customLogger);
            }
        }

        private static void SetDependencyResolver(HttpConfiguration config)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(
                BuildContainer()
            );
        }

        /// <summary>
        /// DI composition root for the API using Autofac
        /// </summary>
        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterLoggingServices();
            builder.RegisterHttpServices();
            builder.RegisterLocalizationServices();
            builder.RegisterDomainServices();

            return builder.Build();
        }

        private static void SetRoutes(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "Docs",
                routeTemplate: HelpUrl,
                defaults: null,
                constraints: null,
                handler: new RedirectHandler(SwaggerDocsConfig.DefaultRootUrlResolver, "help/ui/index"));

            config.Routes.MapHttpRoute(
                name: "DocsObsolete",
                routeTemplate: "help",
                defaults: null,
                constraints: null,
                handler: new RedirectHandler(SwaggerDocsConfig.DefaultRootUrlResolver, "help/ui/index"));

            config.Routes.MapHttpRoute(
                name: "NotFound",
                routeTemplate: $"{RoutePrefix}/{{*path}}",
                defaults: new { controller = "Errors", action = "NotFound" }
            );
        }

        private static void SetMessageHandlers(HttpConfiguration config)
        {
            config.MessageHandlers.Add(new ResponseCorrelationIdHandler());
            config.MessageHandlers.Add(new LogCorrelationIdHandler());
        }

        private static void SetThrottling(HttpConfiguration config)
        {
            config.MessageHandlers.Add(new ThrottlingHandler
            {
                Policy = new ThrottlePolicy(perSecond: 30, perDay: 1500)
                {
                    IpThrottling = true
                },
                Repository = new CacheRepository()
            });
        }

        private static void SetFormatters(HttpConfiguration config)
        {
            // Set the same contract resolver as for JsonResult
            // to keep consistent json serialization when
            // returning JsonResult and NegotiatedContentResult
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new DefaultContractResolver();

            // By default most browsers will receive xml with
            // Content-Type: 'application/xml' because they
            // request 'application/xml' along with 'text/html'
            // within Accept header.
            // To make browsers receive json with
            // Content-Type: 'application/json' we map 'text/html'
            // to JsonFormatter
            config.Formatters.JsonFormatter.MediaTypeMappings.Add(
                new System.Net.Http.Formatting.RequestHeaderMapping(
                    "Accept",
                    "text/html",
                    StringComparison.InvariantCultureIgnoreCase,
                    true,
                    "application/json"
                )
            );
        }
    }
}
