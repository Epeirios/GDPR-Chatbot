using GDPR_Chatbot.data;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GDPR_Chatbot.Dialogs
{
    [Serializable]
    public class NoContextQuestionDialog : IDialog<IMessageActivity>
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

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            using (data.ConversationDataContext dataContext = new data.ConversationDataContext())
            {
                Answer answer = dataContext.Answers
                    .Where(x => x.Intent.Name == this.intent)
                    .SingleOrDefault();

                await context.PostAsync(answer.AnswerText);

                Microsoft.Bot.Connector.Activity activity = new Microsoft.Bot.Connector.Activity();
                
                activity.Text = answer.AnswerText;

                context.Done(activity);
            }
        }
    }
}