﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace hololens_server20180722 {
    public static class WebApiConfig {
        public static void Register(HttpConfiguration config) {
            // Web API の構成およびサービス

            // Web API ルート
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
