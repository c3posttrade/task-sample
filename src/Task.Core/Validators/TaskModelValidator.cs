using System;
using TaskManager.Core.DTO;

namespace TaskManager.Core.Validators
{
    public static class TaskModelValidator
    {
        public static bool IsValid(this TaskModel model)
        {
            if (model == null) return false;
            if (model.Owner == Guid.Empty) return false;
            return true;
        }
    }
}
