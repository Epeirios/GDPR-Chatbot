using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Text;
using GDPR_Chatbot.Serialization;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using GDPR_Chatbot.data;

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
                // handle exeptional cases
                case "None":
                    await context.Forward(new SuggestionDialog(), null, message);
                    break;
                case "Help":
                    await context.Forward(new ReviewDialog(), null, message);
                    break;
                // default is question handler
                default:
                    // determine if context or nocontext question
                    using (data.ConversationDataContext dataContext = new data.ConversationDataContext())
                    {
                        Answer answer = dataContext.Intents
                            .Where(x => x.Name == response.intents[0].intent)
                            .Select(x => x.Answer)
                            .SingleOrDefault();

                        if (answer.Type == data.Models.AnswerTypeEnum.Context)
                        {
                            await context.Forward(new ContextQuestionDialog(), ResumeAfterQuestion, message);
                        }
                        else
                        {
                            await context.Forward(new NoContextQuestionDialog(), ResumeAfterQuestion, message);
                        }
                    }
                    break;
            }
        }

        private async Task ResumeAfterQuestion(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result as IMessageActivity;

            await context.Forward(new ReviewDialog(), null, message);

            context.Wait(this.MessageReceivedAsync);
        }
    }
}