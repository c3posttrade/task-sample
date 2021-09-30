using FluentValidation;
using TaskSample.Services.Features.Tasks.Models;

namespace TaskSample.Services.Features.Tasks.Validators
{
    public class TaskCreateValidator : AbstractValidator<TaskCreateModel>
    {
        public TaskCreateValidator()
        {
            RuleFor(x => x.Description).NotNull().Length(20, 200);
            RuleFor(x => x.OwnerId).NotNull();
        }
    }
}
