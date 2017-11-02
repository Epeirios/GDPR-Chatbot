using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDPR_Chatbot.data
{
    public class ConversationDataContext : DbContext
    {
        public ConversationDataContext()
        : base("Data Source=bm01-chatbot.database.windows.net;Initial Catalog=bm01-chatbot;Integrated Security=False;User ID=bm01-chatbot;Password=Mediaan123;Connect Timeout=15;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
        {
        }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<EntityQuestion> AdditionalQuestions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Entity> Entities { get; set; }
        public DbSet<Intent> Intents { get; set; }
    }
}
