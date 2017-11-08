using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Text;

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
            StringBuilder output = new StringBuilder();

            var activity = await result as Activity;

            var responce = await Services.LuisHandler.GetResponse((activity.Text ?? string.Empty));

            if (responce != null)
            {
                await context.Call(new ContextlessDialog(), MessageReceivedAsync);
            }
            else
            {
                output.Append("Luis Returned null");
            }

            await context.PostAsync(output.ToString());

            context.Wait(MessageReceivedAsync);
        }
    }
}