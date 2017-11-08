using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Microsoft.Bot.Builder.History;
using Microsoft.Bot.Connector;

namespace GDPR_Chatbot
{
    public class EntityFrameworkActivityLogger : IActivityLogger
    {
        Task IActivityLogger.LogAsync(IActivity activity)
        {
            IMessageActivity msg = activity.AsMessageActivity();
            using (data.ConversationDataContext dataContext = new data.ConversationDataContext())
            {
                var newActivity = Mapper.Map<IMessageActivity, data.Activity>(msg);
                if (string.IsNullOrEmpty(newActivity.Id))
                    newActivity.Id = Guid.NewGuid().ToString();
                dataContext.Activities.Add(newActivity);
                dataContext.SaveChanges();
            }
            return Task.CompletedTask;
        }
    }
}