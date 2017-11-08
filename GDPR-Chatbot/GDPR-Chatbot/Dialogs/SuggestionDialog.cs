using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace GDPR_Chatbot.Dialogs
{
    [Serializable]
    public class SuggestionDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);

            return Task.CompletedTask;
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            const string opt1 = "What is GDPR?";
            const string opt2 = "What are the penalties for not complying with the GDPR?";
            const string opt3 = "Who does the GDPR affect?";

            var activity = await result as Activity;

            var reply = activity.CreateReply("I don't understand the question you are asking me, please try something like this:");
            reply.Type = ActivityTypes.Message;
            reply.TextFormat = TextFormatTypes.Plain;

            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                    {
                        new CardAction(){ Title = opt1, Type=ActionTypes.ImBack, Value=opt1 },
                        new CardAction(){ Title = opt2, Type=ActionTypes.ImBack, Value=opt2 },
                        new CardAction(){ Title = opt3, Type=ActionTypes.ImBack, Value=opt3 }
                    }
            };

            await context.PostAsync(reply);

            context.Wait(MessageReceivedAsync);
        }
    }
}