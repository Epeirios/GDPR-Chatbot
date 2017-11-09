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
    public class ReviewDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Microsoft.Bot.Connector.Activity;

            //var reply = context.MakeMessage();

            //var options = new[]
            //{
            //    Resources.ReviewDialog_OptYes,
            //    Resources.ReviewDialog_OptNo,
            //};
            //reply.AddHeroCard(
            //    Resources.ReviewDialog_ReviewQuestion,
            //    options
            //    );
            //await context.PostAsync(reply);

            //context.Wait(this.OnFeedbackGiven);

            var reply = activity.CreateReply(Resources.ReviewDialog_ReviewQuestion);
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
            var message = await result as Microsoft.Bot.Connector.Activity;

            if (message.Text == Resources.ReviewDialog_OptYes)
            {
                using (data.ConversationDataContext dataContext = new data.ConversationDataContext())
                {
                    Answer answer = dataContext.Answers
                        .Where(x => x.AnswerText == message.ReactionsAdded.Last().ToString())
                        .Select(x => x)
                        .SingleOrDefault();                    
                }
                // update database
                await context.PostAsync($"you said: {Resources.ReviewDialog_OptYes}");
            }
            else if (message.Text == Resources.ReviewDialog_OptNo)
            {
                // updata database
                await context.PostAsync($"you said: {Resources.ReviewDialog_OptNo}");
            }
            // else interper message a new dialog

            context.Wait(this.MessageReceivedAsync);
        }
    }
}