using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace VacationManager.Models
{
    public class MainContextDb : DbContext
    {
        public MainContextDb()
            : base("DefaultConnection")
        {

        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Vacation> Vacations { get; set; }

    }
}