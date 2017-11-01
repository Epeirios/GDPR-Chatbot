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
                output.AppendLine($"You send: {activity.Text}");

                if (responce.entities.Count >= 1)
                {
                    output.AppendLine($"\nEntity Name: {responce.entities[0].entity}");
                    output.AppendLine($"\nEntity Score: {responce.entities[0].score}");
                    output.AppendLine($"\nEntity Type: {responce.entities[0].type}");
                }
                else
                {
                    output.AppendLine($"\nNo Entities");
                }

                if (responce.intents.Count >= 1)
                {
                    output.AppendLine($"\nIntent Name: {responce.intents[0].intent}");
                    output.AppendLine($"\nIntent Score: {responce.intents[0].score}");
                }
                else
                {
                    output.AppendLine($"No Intents");
                }
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