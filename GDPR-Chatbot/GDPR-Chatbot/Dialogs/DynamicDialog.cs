using GDPR_Chatbot.Dialogs.SubDialogs;
using GDPR_Chatbot.Serialization;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GDPR_Chatbot.Dialogs
{
    [Serializable]
    public class DynamicDialog : IDialog<object>
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

            Utterance response = await Services.LuisHandler.GetResponse((activity.Text ?? string.Empty)); // this takes some time, based on the speed of luis service. 

            // TODO add snippet query database crap. // probably also takes some time.
            //  - get database LUISintent record.
            //  - check with usermodel required context based said record.
            //  - if context needed 
            //      get from database questionToUser and subdialogType
            //    else (no context needed)
            //      get from database response bases on userQuestion (opt based on UserModel)
            //  - generate SubDialog (could be awnser, could be follow up question)
            //  - return said subDialog.

            // the sum of duration getluisresponse and databasequery snippet defines the response time.

            ISubDialog subDialog = new NoContextSubDialog();


            

            // bases op luisresponce add dialog part.


            await context.PostAsync(output.ToString());

            context.Wait(MessageReceivedAsync);
        }
    }
}