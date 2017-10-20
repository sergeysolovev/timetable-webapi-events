using System.Web.Mvc;
using System.Web.Routing;

namespace SpbuEducation.TimeTable.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "XtracurEvents.Search",
                url: "Events/{alias}/Search",
                defaults: new
                {
                    controller = "XtracurEvents",
                    action = "Search",
                }
            );

            routes.MapRoute(
                name: "XtracurEvents.GetModalContents",
                url: "Events/ModalContents/{eventId}/{recurrenceIndex}",
                defaults: new
                {
                    controller = "XtracurEvents",
                    action = "GetModalContents",
                    recurrenceIndex = UrlParameter.Optional
                }
            );

            routes.MapRoute(
                name: "XtracurEvents.Index",
                url: "Events/{alias}/{fromDate}/{showImmediateEventId}/{showImmediateRecurrenceIndex}",
                defaults: new
                {
                    controller = "XtracurEvents",
                    action = "Index",
                    fromDate = UrlParameter.Optional,
                    showImmediateEventId = UrlParameter.Optional,
                    showImmediateRecurrenceIndex = UrlParameter.Optional
                }
            );

            routes.MapRoute(
                name: "XtracurEvents.ShowImage",
                url: "Image/{id}",
                defaults: new
                {
                    controller = "XtracurEvents",
                    action = "ShowImage"
                }
            );

            routes.MapRoute(
                name: "EducatorEvents.Index",
                url: "EducatorEvents/Index/{q}",
                defaults: new
                {
                    controller = "EducatorEvents",
                    action = "Index",
                    q = UrlParameter.Optional
                }
            );

            routes.MapRoute(
                name: "EducatorEvents.Excel",
                url: "EducatorEvents/{masterId}/Excel/{next}",
                defaults: new
                {
                    controller = "EducatorEvents",
                    action = "Excel",
                    next = UrlParameter.Optional
                }
            );

            routes.MapRoute(
                name: "EducatorEvents.WeekExcel",
                url: "EducatorEvents/{masterId}/Week/Excel/{weekMonday}",
                defaults: new
                {
                    controller = "EducatorEvents",
                    action = "WeekExcel",
                    weekMonday = UrlParameter.Optional
                }
            );

            routes.MapRoute(
                name: "EducatorEvents.ShowWeek",
                url: "WeekEducatorEvents/{masterId}/{weekMonday}",
                defaults: new
                {
                    controller = "EducatorEvents",
                    action = "ShowWeek",
                    weekMonday = UrlParameter.Optional
                }
            );

            routes.MapRoute(
                name: "EducatorEvents.Show",
                url: "EducatorEvents/{masterId}/{next}",
                defaults: new
                {
                    controller = "EducatorEvents",
                    action = "Show",
                    next = UrlParameter.Optional
                }
            );

            routes.MapRoute(
                name: "StudyProgram.Show",
                url: "{publicDivisionAlias}/StudyProgram/{id}",
                defaults: new { controller = "StudyProgram", action = "Show", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "StudentGroupEvents.WeekDays",
                url: "{publicDivisionAlias}/StudentGroupEvents/WeekDays/{studentGroupId}/{weekMonday}",
                defaults: new
                {
                    controller = "StudentGroupEvents",
                    action = "WeekDays",
                    weekMonday = UrlParameter.Optional
                }
            );

            routes.MapRoute(
                name: "StudentGroupEvents.Primary",
                url: "{publicDivisionAlias}/StudentGroupEvents/Primary/{studentGroupId}/{weekMonday}",
                defaults: new
                {
                    controller = "StudentGroupEvents",
                    action = "Primary",
                    weekMonday = UrlParameter.Optional
                }
            );

            routes.MapRoute(
                name: "StudentGroupEvents.Semester",
                url: "{publicDivisionAlias}/StudentGroupEvents/Semester/{studentGroupId}/{autumn}",
                defaults: new
                {
                    controller = "StudentGroupEvents",
                    action = "Semester",
                    autumn = UrlParameter.Optional
                }
            );

            routes.MapRoute(
                name: "StudentGroupEvents.Attestation",
                url: "{publicDivisionAlias}/StudentGroupEvents/Attestation/{studentGroupId}",
                defaults: new { controller = "StudentGroupEvents", action = "Attestation" }
            );

            routes.MapRoute(
                name: "StudentGroupEvents.Final",
                url: "{publicDivisionAlias}/StudentGroupEvents/Final/{studentGroupId}",
                defaults: new { controller = "StudentGroupEvents", action = "Final" }
            );

            routes.MapRoute(
                name: "Division.Show",
                url: "{alias}",
                defaults: new
                {
                    controller = "Division",
                    action = "Show",
                }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}