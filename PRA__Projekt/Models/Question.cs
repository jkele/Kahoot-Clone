using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PRA__Projekt.Models
{
    public class Question : DbBase
    {
        public int Order { get; set; }
        [Required]
        public string QuestionText { get; set; }

        public bool Ended { get; set; }

        public int QuizId { get; set; }
        public virtual Quiz Quiz { get; set; }
    }
}
