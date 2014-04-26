using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VacationManager.Models
{   
    
    public class Vacation
    {
        [Key]
        public int VacationId { get; set; }


        [ForeignKey("UserProfile")]
        public int UserId { get; set; }

        public DateTime VacationDate { get; set; }

        public Boolean IsApproved { get; set; }

        public virtual UserProfile UserProfile { get; set; }
    }
}