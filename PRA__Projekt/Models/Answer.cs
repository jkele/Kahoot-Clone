using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRA__Projekt.Models
{
    public class Answer : DbBase
    {
        public string UserId { get; set; }
        public string AnswerText { get; set; }

        public int QuizId { get; set; }
        public virtual Quiz Quiz { get; set; }

        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }
}
