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
    public class ContextQuestionDialog : IDialog<object>
    {
        private Answer answer;

        public ContextQuestionDialog(Answer answer)
        {
            this.answer = answer;
        }

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync(this.answer.AnswerText);
            context.Done(result);
        }
    }
}