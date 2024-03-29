using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models
{
    public class RegUser
    {
        [Required]
        [MinLength(2)]
        [Display(Name = "First Name")] 
        public string FirstName {get;set;}

        [Required]
        [MinLength(2)]
        [Display(Name = "Last Name")] 
        public string LastName {get;set;}

        [Required]
        [MinLength(2)]
        [Display(Name = "Username")] 
        public string Username {get;set;}

        [Required]
        [EmailAddress]
        [Display(Name = "Email")] 
        public string Email {get;set;}

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")] 
        public string Password {get;set;}

        [Required]
        [Compare("Password", ErrorMessage = "Password does not match")]
        [Display(Name = "Confirm Password")] 
        public string ConfirmPassword {get;set;}
    }
}