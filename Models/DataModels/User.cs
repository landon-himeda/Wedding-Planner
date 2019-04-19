using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models
{
    public class User : DataModel
    {
        [Key]
        public int UserId {get;set;}

        public string FirstName {get;set;}

        public string LastName {get;set;}

        public string Username {get;set;}

        public string Email {get;set;}

        public string Password {get;set;}

        public List<Wedding> Weddings {get;set;}

        public List<RSVP> RSVPs {get;set;}
    }
}