using GDPR_Chatbot.data;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GDPR_Chatbot.Dialogs
{
    [Serializable]
    public class NoContextQuestionDialog : IDialog<object>
    {
        private string intent;

        public NoContextQuestionDialog(string intent)
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

            using (data.ConversationDataContext dataContext = new data.ConversationDataContext())
            {
                Answer answer = dataContext.Answers
                    .Where(x => x.Intent.Name == this.intent)
                    .SingleOrDefault();

                await context.PostAsync(answer.AnswerText);

                context.Done(answer.AnswerText);

            }
        }
    }
}