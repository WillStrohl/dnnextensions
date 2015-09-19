
using DotNetNuke.Web.Api;

namespace WillStrohl.Modules.CodeCamp.Components
{
    public class RouteMapper : IServiceRouteMapper
    {
        public void RegisterRoutes(IMapRoute mapRouteManager)
        {
            mapRouteManager.MapHttpRoute("CodeCamp", "default", "{controller}/{action}", new[] { "WillStrohl.Modules.CodeCamp.Services" });
        }
    }
}