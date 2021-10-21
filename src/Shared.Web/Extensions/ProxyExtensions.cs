using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Shared.Web.Extensions
{
    public static class ProxyExtensions
    {
        public static string ToClientIp(this HttpRequest request)
        {
            var ip = request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
            return ip;
        }
    }
}
