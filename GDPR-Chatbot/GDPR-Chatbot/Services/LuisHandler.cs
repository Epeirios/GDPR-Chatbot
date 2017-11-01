using GDPR_Chatbot.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GDPR_Chatbot.Services
{
    public class LuisHandler
    {
        public static async Task<Utterance> GetResponse(string message)
        {
            using (var client = new HttpClient())
            {
                const string appID = "83680a15-a6c8-4a5a-8b10-a20075a998bc";
                const string subKey = "fc5cc1eaa79941ca8abbcab6a2bdf306";

                var url = $"https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/{appID}?subscription-key={subKey}&timezoneOffset=0&verbose=true&q={message}";
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode) return null;
                var result = await response.Content.ReadAsStringAsync();

                var js = new DataContractJsonSerializer(typeof(Utterance));
                var ms = new MemoryStream(Encoding.ASCII.GetBytes(result));
                var list = (Utterance)js.ReadObject(ms);

                return list;
            }
        }
    }
}