using GDPR_Chatbot.data;
using GDPR_Chatbot.Dialogs.SubDialogs;
using GDPR_Chatbot.Serialization;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GDPR_Chatbot.Dialogs
{
    [LuisModel("83680a15-a6c8-4a5a-8b10-a20075a998bc", "6322289be2084851b1b6b93efbc48ac9", domain: "westus.api.cognitive.microsoft.com")]
    [Serializable]
    public class ContextlessDialog : LuisDialog<object>
    {
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Sorry, I did not understand '{result.Query}'. Type 'help' if you need assistance.";

            await context.PostAsync(message);

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Help")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Hi! Try asking me things like 'What is gdpr?', 'what are the penalties for not complying with the gdpr?' or 'Who does the gdpr affect?' ");

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("informationGDPR")]
        public async Task Search(IDialogContext context, LuisResult result)
        {
            using (data.ConversationDataContext dataContext = new data.ConversationDataContext())
            {
                Answer answer = dataContext.Intents
                    .Where(x => x.Name == "informationGDPR")
                    .Select(x => x.Answer)
                    .SingleOrDefault();

                await context.PostAsync(answer.AnswerText);
            }

            context.Wait(this.MessageReceived);
        }



        //public Task StartAsync(IDialogContext context)
        //{
        //    context.Wait(MessageReceivedAsync);

        //    return Task.CompletedTask;
        //}

        //private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        //{
        //    StringBuilder output = new StringBuilder();

        //    var activity = await result as Activity;

        //    Utterance response = await Services.LuisHandler.GetResponse((activity.Text ?? string.Empty)); // this takes some time, based on the speed of luis service. 

        //    // TODO add snippet query database crap. // probably also takes some time.
        //    //  - get database LUISintent record.
        //    //  - check with usermodel required context based said record.
        //    //  - if context needed 
        //    //      get from database questionToUser and subdialogType
        //    //    else (no context needed)
        //    //      get from database response bases on userQuestion (opt based on UserModel)
        //    //  - generate SubDialog (could be awnser, could be follow up question)
        //    //  - return said subDialog.

        //    // the sum of duration getluisresponse and databasequery snippet defines the response time.

        //    ISubDialog subDialog = new NoContextSubDialog();




        //    // bases op luisresponce add dialog part.


        //    await context.PostAsync(output.ToString());

        //    context.Wait(MessageReceivedAsync);
        //}
    }
}