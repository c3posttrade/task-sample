using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Task.Models
{
    public class Task
    {

        [Display(Name = "id")]
        public string Id { get; set; }

        [Required]
        [Display(Name = "description")]
        public string Description { get; set; }

        [Display(Name = "completed")]
        public bool Completed { get; set; }

        [Required]
        [Display(Name = "owner")]
        public string Owner { get; set; }


        [Display(Name = "created")]
        public string Created { get; set; }


        [Display(Name = "updated")]
        public string Updated { get; set; }



        //id": "61275fac8924ac0017e24359",
        //"description": "sample task",
        //"completed": true,
        //"owner": "6008cc3e5ab46f0017b8f64e",
        //"created": "2021-08-26T09:32:28.325Z",
        //"updated": "2021-09-08T06:01:20.092Z"
    }
}