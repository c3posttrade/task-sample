using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.Core.DTO
{
    public class TaskModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }

        [Required]
        public Guid Owner { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
