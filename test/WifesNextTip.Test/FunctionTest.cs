using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Xunit;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.APIGatewayEvents;

namespace WifesNextTip.Tests
{
  public class LambdaResponseBody
  {
    public string uid {get;set;}
    public string updateDate {get;set;}
    public string titleText {get;set;}
    public string mainText {get;set;}
    public string redirectionUrl {get;set;}
    public string failingProperty {get;set;}
  }

  public class FunctionTest
  {
    private static readonly HttpClient client = new HttpClient();

    private static async Task<string> GetCallingIP()
    {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("User-Agent", "AWS Lambda .Net Client");

            var stringTask = client.GetStringAsync("http://checkip.amazonaws.com/").ConfigureAwait(continueOnCapturedContext:false);

            var msg = await stringTask;
            return msg.Replace("\n","");
    }

    [Fact]
    public async Task TestHelloWorldFunctionHandler()
    {
            var request = new APIGatewayProxyRequest();
            var context = new TestLambdaContext();
            // string location = GetCallingIP().Result;
            // Dictionary<string, string> body = new Dictionary<string, string>
            // {
            //     { "uid", "test-key-only" },
            //     { "updateDate", "test-key-only" },
            //     { "titleText", "Wifes tip of the Week" },
            //     { "mainText", "test-key-only" },
            //     { "redirectionUrl", "test-key-only" },
            // };

            // var expectedResponse = new APIGatewayProxyResponse
            // {
            //     Body = JsonConvert.SerializeObject(body),
            //     StatusCode = 200,
            //     Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            // };

            

            var function = new WifesNextTip.Function();
            var response = await function.FunctionHandler(request, context);

            Console.WriteLine("Lambda Response: \n" + response.Body);
            //Console.WriteLine("Expected Response: \n" + expectedResponse.Body);

            var responseBody = JsonConvert.DeserializeObject<LambdaResponseBody>(response.Body);

            Assert.NotNull(responseBody.uid);
            Assert.NotNull(responseBody.updateDate);
            Assert.NotNull(responseBody.titleText);
            Assert.NotNull(responseBody.mainText);
            Assert.NotNull(responseBody.redirectionUrl);
            Assert.Null(responseBody.failingProperty);
            Assert.Equal(200, response.StatusCode);
    }
  }
}