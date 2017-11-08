using GDPR_Chatbot.data;
using GDPR_Chatbot.Serialization;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GDPR_Chatbot.Dialogs
{
    [LuisModel("83680a15-a6c8-4a5a-8b10-a20075a998bc", "6322289be2084851b1b6b93efbc48ac9", domain: "westus.api.cognitive.microsoft.com")]
    [Serializable]
    public class DynamicDialog : LuisDialog<object>
    {
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Sorry, I did not understand '{result.Query}'. Type 'help' if you need assistance.";

            await context.PostAsync(message);

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Help")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Hi! Try asking me things like 'What is gdpr?', 'what are the penalties for not complying with the gdpr?' or 'Who does the gdpr affect?' ");

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("informationGDPR")]
        public async Task Search(IDialogContext context, LuisResult result)
        {
            using (data.ConversationDataContext dataContext = new data.ConversationDataContext())
            {
                Answer answer = dataContext.Intents
                    .Where(x => x.Name == "informationGDPR")
                    .Select(x => x.Answer)
                    .SingleOrDefault();

                await context.PostAsync(answer.AnswerText);
            }

            context.Wait(this.MessageReceived);
        }
    }
}