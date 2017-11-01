using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GDPR_Chatbot.Serialization
{
    public class Intent
    {
        public string intent { get; set; }
        public double score { get; set; }
    }
}