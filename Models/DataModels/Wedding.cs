using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models
{
    public class Wedding : DataModel
    {
        [Key]
        public int WeddingId {get;set;}

        public string WedderOne {get;set;}

        public string WedderTwo {get;set;}

        public DateTime Date {get;set;}

        public string Address {get;set;}

        public int UserId {get;set;}

        public User user {get;set;}

        public List<RSVP> RSVPs {get;set;}
    }
}