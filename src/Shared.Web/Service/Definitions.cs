using System.Collections.Generic;
using System.ComponentModel;

namespace Shared.Web.Service
{
    public enum ResponseStatus
    {
        [Description("OK")]
        OK = 200,

        [Description("Bad Request")]
        BadRequest = 400,

        [Description("Not Allowed")]
        NotAllowed = 401,

        [Description("Not Found")]
        NotFound = 404,

        [Description("System Error")]
        SystemError = 500
    }

    public class ServiceResponse<T>
    {
        public ResponseStatus Status { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public T Value { get; set; }

        public ServiceResponse()
        {
        }

        public ServiceResponse(T value)
        {
            Value = value;
            Status = ResponseStatus.OK;
        }
    }

    public class PageConfig
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class PageResults<T>
    {
        public List<T> Items { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
    }
}
