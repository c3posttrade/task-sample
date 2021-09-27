using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class TaskModel
    {


        [Display(Name = "id")]
        public string Id { get; set; }

        [Display(Name = "description")]
        public string Description { get; set; }

        [Display(Name = "completed")]
        public bool Completed { get; set; }


        [Display(Name = "owner")]
        public string Owner { get; set; }


        [Display(Name = "created")]
        public string Created { get; set; }


        [Display(Name = "updated")]
        public string Updated { get; set; }

        public TaskModel()
        {
        }
    }
}