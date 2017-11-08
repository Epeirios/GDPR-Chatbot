using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Text;
using GDPR_Chatbot.Serialization;
using System.Collections.Generic;
using System.Threading;

namespace GDPR_Chatbot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            Utterance response = await Services.LuisHandler.GetResponse((message.Text ?? string.Empty)); // this takes some time, based on the speed of luis service. 

            switch (response.intents[0].intent)
            {
                case "None":
                    await context.Forward(new SuggestionDialog(), null, message);
                    break;
                case "Help":
                    await context.Forward(new SuggestionDialog(), null, message);
                    break;
                default:
                    context.Wait(MessageReceivedAsync);
                    break;
            }            
        }
    }
}