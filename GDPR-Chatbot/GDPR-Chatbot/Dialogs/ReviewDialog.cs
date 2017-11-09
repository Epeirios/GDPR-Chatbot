using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using GDPR_Chatbot.data;
using GDPR_Chatbot.Properties;

namespace GDPR_Chatbot.Dialogs
{
    [Serializable]
    public class ReviewDialog : IDialog<IMessageActivity>
    {
        string _question;

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            _question = message.Text;

            var reply = context.MakeMessage();
            reply.Text = Resources.ReviewDialog_ReviewQuestion;
            reply.Type = ActivityTypes.Message;
            reply.TextFormat = TextFormatTypes.Plain;
            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                    {
                        new CardAction(){ Title = Resources.ReviewDialog_OptYes, Type=ActionTypes.ImBack, Value=Resources.ReviewDialog_OptYes },
                        new CardAction(){ Title = Resources.ReviewDialog_OptNo, Type=ActionTypes.ImBack, Value=Resources.ReviewDialog_OptNo },
                    }
            };

            await context.PostAsync(reply);

            context.Wait(this.OnFeedbackGiven);
        }

        private async Task OnFeedbackGiven(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (message.Text == Resources.ReviewDialog_OptYes)
            {
                message.Text = string.Empty;

                // update database
                await context.PostAsync($"You said: {Resources.ReviewDialog_OptYes}");
            }
            else if (message.Text == Resources.ReviewDialog_OptNo)
            {
                message.Text = string.Empty;

                // updata database
                await context.PostAsync($"You said: {Resources.ReviewDialog_OptNo}");
            }
            //else
            //{
            //    // else interper message a new dialog

            //    context.Done(message);
            //}

            context.Done(message);
        }
    }
}