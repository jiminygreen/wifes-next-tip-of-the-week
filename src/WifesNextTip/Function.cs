using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace WifesNextTip
{

    public class Function
    {

        private static readonly HttpClient client = new HttpClient();

        private static async Task<string> GetCallingIP()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("User-Agent", "AWS Lambda .Net Client");

            var msg = await client.GetStringAsync("http://checkip.amazonaws.com/").ConfigureAwait(continueOnCapturedContext: false);

            return msg.Replace("\n", "");
        }

        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest apigProxyEvent, ILambdaContext context)
        {

            var location = await GetCallingIP();
            var body = new Dictionary<string, string>
            {
                 {"uid", "b2309c98-cfc9-4a05-9c9c-5ebabc3f452b"},
                 {"updateDate", "2020-06-24T10:24:05.924Z"},
 {"titleText", "Wifes tip of the Week"},
                 {"mainText", "That is not a good idea. Cooking chocolate is for cooking and can make you sick if it is eaten raw!"},
 {"redirectionUrl",  "https://www.quora.com/Can-cooking-chocolate-be-eaten"}
            };

            return new APIGatewayProxyResponse
            {
                Body = JsonConvert.SerializeObject(body),
                StatusCode = 200,
                Headers = new Dictionary<string, string> {
                    { "Content-Type", "application/json" }
                     }
            };
        }
    }
}
