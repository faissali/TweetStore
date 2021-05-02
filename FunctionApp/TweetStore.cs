using System.IO;
using Microsoft.Azure.WebJobs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Storage.Blob;

namespace FunctionApp
{
    public static class TweetStore
    {
        [FunctionName("StoreTweets")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Blob("tweets/{rand-guid}.json", FileAccess.Write, Connection ="AzureWebJobsStorage")] CloudBlockBlob outBlob,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request coming from twitter.");

            //Get Body of request
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync(); 

            //Create a new Blob in tweets container that store request Body
            await outBlob.UploadTextAsync(requestBody);

            //return 200OK response
            return new OkObjectResult("OK");
        }
    }
}
