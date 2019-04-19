using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models
{
    public class WeddingForm
    {
        [Required]
        [MinLength(2)]
        [Display(Name = "Wedder Two")] 
        public string WedderOne {get;set;}

        [Required]
        [MinLength(2)]
        [Display(Name = "Wedder Two")]
        public string WedderTwo {get;set;}

        [Required]
        [FutureDate]
        public DateTime Date {get;set;}

        [Required]
        [MinLength(2)]
        public string Address {get;set;}
    }
}