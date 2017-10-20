using Autofac;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Localization;
using System.Threading;

namespace SpbuEducation.TimeTable.Web.Api.v1.Localization.DependencyInjection
{
    public static class LocalizationExtensions
    {
        public static ContainerBuilder RegisterLocalizationServices(this ContainerBuilder builder)
        {
            builder.RegisterInstance(new LocaleInfo(Thread.CurrentThread.CurrentCulture));
            return builder;
        }
    }
}
