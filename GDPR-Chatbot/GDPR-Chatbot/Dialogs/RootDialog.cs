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

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result as Microsoft.Bot.Connector.Activity;

            Utterance response = await Services.LuisHandler.GetResponse((message.Text ?? string.Empty)); // this takes some time, based on the speed of luis service. 

            switch (response.intents[0].intent)
            {
                // handle exeptional cases
                case "None":
                    await context.Forward(new NoContextQuestionDialog(), ResumeAfterQuestion, result, CancellationToken.None);
                    break;
                case "Help":
                    await context.Forward(new NoContextQuestionDialog(), ResumeAfterQuestion, result, CancellationToken.None);
                    break;
                // default is question handler
                default:
                    // determine if context or nocontext question
                    using (data.ConversationDataContext dataContext = new data.ConversationDataContext())
                    {
                        string intent = response.intents[0].intent;
                        // this currently only works for one answer per intent
                        Answer answer = dataContext.Answers
                            .Where(x => x.Intent.Name == intent)
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
            var message = await result as Microsoft.Bot.Connector.Activity;

            await context.Forward(new ReviewDialog(), MessageReceivedAsync, message);

            //context.Wait(this.MessageReceivedAsync);
        }
    }
}