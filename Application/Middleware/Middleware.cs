using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Primitives;
using System.Text;

namespace Application.Middleware
{
    public class Middleware
    {
        private readonly RequestDelegate _next;

        public Middleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var req = httpContext.Request.Body;
            try
            {
                //var url = httpContext.Request.GetEncodedUrl().Split("/")[httpContext.Request.GetEncodedUrl().Split("/").Length - 1];
                var url = httpContext.Request.GetEncodedPathAndQuery().Split("/")[1];
                if (url != "AssertionConsumerService" && url != "Error" && url != "WHS0300" && url != "WHS0700" && url != "MaintainScreenProfile" && httpContext.Request.GetEncodedPathAndQuery() != "/Authen/AuthenUserLogin" && httpContext.Request.GetEncodedPathAndQuery() != "/Home/api/UpdateEmailInfo" && httpContext.Request.GetEncodedPathAndQuery() != "/Home/api/InsertEmailInfo")
                {
                    if (url.IndexOf(".") < 0)
                    {

                        httpContext.Request.EnableBuffering();
                        var buffer = new byte[Convert.ToInt32(httpContext.Request.ContentLength)];
                        await httpContext.Request.Body.ReadAsync(buffer, 0, buffer.Length);
                        var body = Encoding.UTF8.GetString(buffer);
                        httpContext.Request.Body.Position = 0;
                        char[] lstSpecialChar = { '<', '>', '!', '#', '|' };
                        int isMath = body.IndexOfAny(lstSpecialChar);
                        if (isMath > 0)
                        {
                            throw new Exception("XSS injection detected from middleware.");
                        }
                    }
                }
                await _next(httpContext);
            }
            catch (Exception ex)
            {

                httpContext.Response.StatusCode = 400;
                var response = httpContext.Response;
                response.ContentType = "application/json";
                await response.WriteAsync(ex.ToString());
            }
        }
    }

    public sealed class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Referrer-Policy
            // TODO Change the value depending of your needs
            //context.Response.Headers.Add("referrer-policy", new StringValues("strict-origin-when-cross-origin"));

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Content-Type-Options
            context.Response.Headers.Add("x-content-type-options", new StringValues("nosniff"));

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Frame-Options
            context.Response.Headers.Add("x-frame-options", new StringValues("SAMEORIGIN"));

            // https://security.stackexchange.com/questions/166024/does-the-x-permitted-cross-domain-policies-header-have-any-benefit-for-my-websit
            context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", new StringValues("none"));

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-XSS-Protection
            context.Response.Headers.Add("x-xss-protection", new StringValues("1; mode=block"));
            context.Response.Headers.Add("server", new StringValues("none"));
            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Expect-CT
            // You can use https://report-uri.com/ to get notified when a misissued certificate is detected
            //context.Response.Headers.Add("Expect-CT", new StringValues("max-age=0, enforce, report-uri=\"https://example.report-uri.com/r/d/ct/enforce\""));

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Feature-Policy
            // https://github.com/w3c/webappsec-feature-policy/blob/master/features.md
            // https://developers.google.com/web/updates/2018/06/feature-policy
            // TODO change the value of each rule and check the documentation to see if new features are available
            /*context.Response.Headers.Add("Feature-Policy", new StringValues(
                "accelerometer 'none';" +
                "ambient-light-sensor 'none';" +
                "autoplay 'none';" +
                "battery 'none';" +
                "camera 'none';" +
                "display-capture 'none';" +
                "document-domain 'none';" +
                "encrypted-media 'none';" +
                "execution-while-not-rendered 'none';" +
                "execution-while-out-of-viewport 'none';" +
                "gyroscope 'none';" +
                "magnetometer 'none';" +
                "microphone 'none';" +
                "midi 'none';" +
                "navigation-override 'none';" +
                "payment 'none';" +
                "picture-in-picture 'none';" +
                "publickey-credentials-get 'none';" +
                "sync-xhr 'none';" +
                "usb 'none';" +
                "wake-lock 'none';" +
                "xr-spatial-tracking 'none';"
                ));
            */
            // https://developer.mozilla.org/en-US/docs/Web/HTTP/CSP
            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy
            // TODO change the value of each rule and check the documentation to see if new rules are available
            /*context.Response.Headers.Add("Content-Security-Policy", new StringValues(
                "base-uri 'none';" +
                "block-all-mixed-content;" +
                "child-src 'none';" +
                "connect-src 'none';" +
                "default-src 'none';" +
                "font-src 'none';" +
                "form-action 'none';" +
                "frame-ancestors 'none';" +
                "frame-src 'none';" +
                "img-src 'none';" +
                "manifest-src 'none';" +
                "media-src 'none';" +
                "object-src 'none';" +
                "sandbox;" +
                "script-src 'none';" +
                "script-src-attr 'none';" +
                "script-src-elem 'none';" +
                "style-src 'none';" +
                "style-src-attr 'none';" +
                "style-src-elem 'none';" +
                "upgrade-insecure-requests;" +
                "worker-src 'none';"
                ));
            */
            return _next(context);
        }
    }
}
