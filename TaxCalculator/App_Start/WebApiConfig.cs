using System.Web.Http;

namespace TaxCalculator
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            UnityBootstrapper.Init(config);
            var formatters = GlobalConfiguration.Configuration.Formatters;
            formatters.Remove(formatters.XmlFormatter);
            formatters.JsonFormatter.Indent = true;
            
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
