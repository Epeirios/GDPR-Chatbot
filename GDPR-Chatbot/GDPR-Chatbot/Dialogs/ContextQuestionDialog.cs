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

        public ContextQuestionDialog(string intent)
        {
            this.intent = intent;
        }

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            if (!context.UserData.ContainsKey("contextQuestion.Answer"))
            {
                Answer answer = await GetAnswerWithEntities(this.intent);
                context.UserData.SetValue("contextQuestion.Answer", answer);
            }

            foreach (data.Entity entity in context.UserData.GetValue<Answer>("contextQuestion.Answer").NeededEntities)
            {
                if (!context.UserData.ContainsKey(entity.Name))
                {
                    context.UserData.SetValue("contextQuestion.AllEntitiesAnswered", false);
                    context.UserData.SetValue("contextQuestion.UnansweredEntity", entity);
                }
            }

            if (!context.UserData.GetValue<bool>("contextQuestion.AllEntitiesAnswered"))
            {
                await context.PostAsync(context.UserData.GetValue<data.Entity>("contextQuestion.UnansweredEntity").Question);
                context.Wait(ProcesEntityQuestionAnswer);
            } else
            {
                await context.PostAsync("You gave the following entities : ");

                foreach (data.Entity entity in context.UserData.GetValue<Answer>("contextQuestion.Answer").NeededEntities)
                {
                    await context.PostAsync(entity.Name + " : " + context.UserData.GetValue<string>(entity.Name));
                }

                context.UserData.RemoveValue("contextQuestion.Answer");
                context.UserData.RemoveValue("contextQuestion.UnansweredEntity");
                context.UserData.RemoveValue("contextQuestion.AllEntitiesAnswered");

                context.Done(result);
            }
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

        private async Task ProcesEntityQuestionAnswer (IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            Utterance response = await Services.LuisHandler.GetResponse((message.Text ?? string.Empty)); // this takes some time, based on the speed of luis service. 

            if (response.intents[0].intent != "AnswerEntityQuestion" && 
                context.UserData.GetValue<data.Entity>("contextQuestion.UnansweredEntity").Name == response.entities[0].type)
            {
                context.UserData.SetValue(response.entities[0].type, response.entities[0].entity);
            }
            else
            {
                await context.PostAsync("I didn't understand that.");
            }

            context.Wait(MessageReceivedAsync);
        }
    }
}