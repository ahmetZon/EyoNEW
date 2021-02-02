using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class DynamicRouting : IRouteConstraint
{

    public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {

        if (values["link"] != null && values["link"] != "" && !values["link"].ToStr().Contains("/") && !values["link"].ToStr().Contains(".") && values["action"].ToStr() == "BaseContent")
        {
            var url = Uri.EscapeDataString(values["link"].ToString());
            httpContext.Items["cmspage"] = Uri.EscapeDataString(url);
            return true;
        }
        else
        {
            return false;
        }
    }

}




