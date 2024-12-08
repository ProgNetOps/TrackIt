using Microsoft.AspNetCore.Mvc.Rendering;

namespace TrackIt.Presentation.Utilities;

public static class StaticDetails
{
    public static string IsActive(this IHtmlHelper html, string controller, string action, string cssClass = "active-link")
    {
        var routeData = html.ViewContext.RouteData;
        var routeAction = routeData.Values["action"]?.ToString();
        var routeController = routeData.Values["controller"]?.ToString();

        return controller == routeController && action == routeAction ?
            cssClass :
            string.Empty;
    }
}
