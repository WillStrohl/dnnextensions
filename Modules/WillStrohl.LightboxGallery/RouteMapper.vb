
Imports DotNetNuke.Web.Api

Namespace WillStrohl.Modules.Lightbox

    Public Class RouteMapper
        Implements IServiceRouteMapper

        Public Sub RegisterRoutes(ByVal mapRouteManager As IMapRoute) Implements IServiceRouteMapper.RegisterRoutes
            mapRouteManager.MapHttpRoute("WillStrohl.LightboxGallery", "default", "{controller}/{action}", {"WillStrohl.Modules.Lightbox.Services"})
        End Sub

    End Class

End Namespace