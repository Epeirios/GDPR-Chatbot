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
    public class SuggestionDialog : IDialog<IMessageActivity>
    {
        List<string> options = new List<string>(){
            "What is GDPR?",
            "What are the penalties for not complying with the GDPR?",
            "Who does the GDPR affect?"
        };

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            // TODO search database for similar questions based on message

            PromptDialog.Choice(context, this.OnOptionSelected, options, Properties.Resources.SuggestionDialog_NotUnderstandQuestion);
        }

        private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            string optionSelected = await result;

            try
            {
                if (options.Contains(optionSelected))
                {
                    Activity temp = new Activity();
                    temp.Text = optionSelected;

                    context.Done(temp);
                }
            }
            catch (TooManyAttemptsException ex)
            {
                if (options.Contains(optionSelected))
                {
                    Activity temp = new Activity();
                    temp.Text = optionSelected;

                    context.Done(temp);
                }
            }
        }
    }
}
