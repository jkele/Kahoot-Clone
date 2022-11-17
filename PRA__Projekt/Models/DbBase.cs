using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRA__Projekt.Models
{
    public class DbBase
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
