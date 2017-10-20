using Autofac;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Abstractions;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.Mappers;
using SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.Repositories;

namespace SpbuEducation.TimeTable.Web.Api.v1.Domain.Services.Xpo.DependencyInjection
{
    public static class DomainServicesExtensions
    {
        public static ContainerBuilder RegisterDomainServices(this ContainerBuilder builder)
        {
            builder.RegisterType<DevExpress.Xpo.Session>().InstancePerRequest();

            builder.RegisterType<EducatorRepository>();
            builder.RegisterType<ClassroomRepository>();
            builder.RegisterType<DivisionRepository>();
            builder.RegisterType<ExtracurDivisionRepository>();
            builder.RegisterType<StudyYearRepository>();
            builder.RegisterType<ProgramRepository>();
            builder.RegisterType<GroupRepository>();

            builder.RegisterType<EventLocationMapper>();
            builder.RegisterType<AddressLocationMapper>();
            builder.RegisterType<EducatorIdTupleMapper>();
            builder.RegisterType<ContingentDivisionCourseMapper>();
            builder.RegisterType<ContingentNameTupleMapper>();
            builder.RegisterType<ExtracurEventMapper>();
            builder.RegisterType<SeatingMapper>();

            builder.RegisterType<AddressesService>().As<IAddressesService>();
            builder.RegisterType<ClassroomsService>().As<IClassroomsService>();
            builder.RegisterType<EducatorsService>().As<IEducatorsService>();
            builder.RegisterType<StudyDivisionsService>().As<IStudyDivisionsService>();
            builder.RegisterType<ExtracurDivisionsService>().As<IExtracurDivisionsService>();
            builder.RegisterType<ProgramsService>().As<IProgramsService>();
            builder.RegisterType<GroupsService>().As<IGroupsService>();

            return builder;
        }
    }
}
