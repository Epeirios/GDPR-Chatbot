using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace GDPR_Chatbot.Serialization
{
    [DataContract]
    public class Utterance
    {
        [DataMember]
        public string query { get; set; }
        [DataMember]
        public List<Intent> intents { get; set; }
        [DataMember]
        public List<Entity> entities { get; set; }
    }
}