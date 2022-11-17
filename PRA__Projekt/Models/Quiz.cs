using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PRA__Projekt.Models
{
    public class Quiz : DbBase
    {
        public string UserId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Code { get; set; }

        public bool Started { get; set; }
        public bool Ended { get; set; }
    }
}
