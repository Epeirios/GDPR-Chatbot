using GDPR_Chatbot.data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDPR_Chatbot.data
{
    public class Answer
    {

        public Answer()
        {
            ActiveQuestion = true;
        }

        public int Id { get; set; }
        public string AnswerText { get; set; }
        public AnswerTypeEnum Type { get; set; }
        public int TotalReviews { get; set; }
        public int PositiveReviews { get; set; }
        public bool ActiveQuestion { get; set; }

    }
}
