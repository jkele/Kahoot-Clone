using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRA__Projekt.Models
{
    public class QuizQuestionsVM
    {
        public Quiz Quiz { get; set; }
        public List<Question> Questions { get; set; }
    }
}
