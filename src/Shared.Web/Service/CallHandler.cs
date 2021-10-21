using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Shared.Web.Service
{
    public class CallHandler
    {
        public static async Task<IActionResult> Process<T>(Func<Task<ServiceResponse<T>>> action, ILogger _logger, ControllerBase caller)
        {
            try
            {
                var response = await action.Invoke();
                if (response.Status == ResponseStatus.OK)
                    return caller.Ok(response.Value);
                else if (response.Status == ResponseStatus.NotFound)
                    return caller.NotFound();
                else if (response.Status == ResponseStatus.NotAllowed)
                    return caller.Unauthorized();
                return caller.BadRequest(response.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return caller.Problem();
            }
        }
    }
}
