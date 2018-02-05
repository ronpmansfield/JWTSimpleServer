﻿using JWTSimpleServer;
using Microsoft.AspNetCore.Http;
using System;

namespace Microsoft.AspNetCore.Builder
{
    public static class JwtSimpleServerAppBuilderExtensions
    {
        private const string XFormUrlEncoded = "application/x-www-form-urlencoded";
        public static IApplicationBuilder UseJwtSimpleServer(this IApplicationBuilder app, Action<JwtSimpleServerOptions> serverSetup)
        {
            var simpleServerOptions = new JwtSimpleServerOptions();
            serverSetup(simpleServerOptions);

            app.MapWhen( context => IsValidJwtMiddlewareRequest(context, simpleServerOptions),                
                      appBuilder => appBuilder.UseMiddleware<JwtSimpleServerMiddleware>(simpleServerOptions));            

            return app;
        }
        private static bool IsValidJwtMiddlewareRequest(HttpContext context, JwtSimpleServerOptions options)
        {
            return context.Request.Method == HttpMethods.Post &&   
                   context.Request.ContentType == XFormUrlEncoded &&
                   context.Request.Path == options.Path;
        }
    }
}