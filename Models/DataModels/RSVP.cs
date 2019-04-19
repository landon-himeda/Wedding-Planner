using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models
{
    public class RSVP : DataModel
    {
        [Key]
        public int RSVPId {get;set;}

        public int UserId {get;set;}

        public int WeddingId {get;set;}

        public User user {get;set;}
        public Wedding wedding {get;set;}
    }
}