using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using TaskSample.Api.Models;
using TaskSample.Services.Features.Tasks.Validators;

namespace TaskSample.Api.Extensions
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder AddApiBehavior(this IMvcBuilder builder)
        {
            builder.ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage).ToArray();
                    var result = new BadRequestObjectResult(new ErrorModel { Errors = errors });
                    return result;
                };
            });
            return builder;
        }
        public static IMvcBuilder AddApiValidation(this IMvcBuilder builder)
        {
            builder.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<TaskCreateValidator>());
            return builder;
        }
    }
}
