using GDPR_Chatbot.data;
using GDPR_Chatbot.Serialization;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GDPR_Chatbot.Dialogs
{
    [Serializable]
    public class ContextQuestionDialog : IDialog<object>
    {
        private string intent;
        private Dictionary<string, string> entities;

        public ContextQuestionDialog(string intent)
        {
            this.intent = intent;
            this.entities = new Dictionary<string, string>();
        }

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            Answer answer = await GetAnswerWithEntities(this.intent);

            foreach (GDPR_Chatbot.data.Entity entity in answer.NeededEntities)
            {
                while (!this.entities.Keys.Contains(entity.Name))
                {
                    await context.PostAsync(entity.Question);
                    context.Wait(this.ProcesEntityQuestionAnswer);
                }
            }

            await context.PostAsync("You gave the following entities : ");

            foreach (var entity in this.entities)
            {
                await context.PostAsync(entity.Key + " : " + entity.Value);
            }

            Microsoft.Bot.Connector.Activity activity = new Microsoft.Bot.Connector.Activity();
            activity.Text = answer.AnswerText;
            context.Done(activity);
        }

        private async Task<Answer> GetAnswerWithEntities(string intentName)
        {
            using (data.ConversationDataContext dataContext = new data.ConversationDataContext())
            {
                return dataContext.Answers
                    .Where(x => x.Intent.Name == intentName)
                    .Include(y => y.NeededEntities)
                    .SingleOrDefault();
            }
        }

        private async Task ProcesEntityQuestionAnswer(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            Utterance response = await Services.LuisHandler.GetResponse((message.Text ?? string.Empty)); // this takes some time, based on the speed of luis service. 

            if (response.intents[0].intent != "AnswerEntityQuestion")
            {
                this.entities.Add(response.entities[0].type, response.entities[0].entity);
            } else
            {
                await context.PostAsync("I didn't understand that.");
            }
        }
    }
}