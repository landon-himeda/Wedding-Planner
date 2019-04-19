using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models
{
    public class DashboardView
    {
        public User user {get;set;}

        public List<Wedding> weddings {get;set;}
    }
}