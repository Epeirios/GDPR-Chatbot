using GDPR_Chatbot.data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDPR_Chatbot.data
{
    public class Answer
    {
        public int Id { get; set; }
        public string AnswerText { get; set; }
        public AnswerTypeEnum Type { get; set; }

    }
}
