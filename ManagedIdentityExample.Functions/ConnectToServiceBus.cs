using System.IO;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

using ManagedIdentityExample.Functions.TokenProviders;

namespace ManagedIdentityExample.Functions
{
    public static class ConnectToServiceBus
    {
        #region Constants

        private const string NAMESPACE = "https://<YOUR_NAMESPACE>.servicebus.windows.net";
        private const string QUEUE_NAME = "<YOUR_QUEUE_NAME>";
        // TENANT_ID can both be the Directory ID (GUID) or the 'xxx.onmicrosoft.com' name of the tenant.
        private const string TENANT_ID = "<YOUR_TENANT_ID>";

        #endregion

        [FunctionName("ConnectToServiceBus")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            using var streamReader = new StreamReader(req.Body);
            var messageContent = await streamReader.ReadToEndAsync();

            var queueClient = new QueueClient(NAMESPACE, QUEUE_NAME, new ServiceBusTokenProvider(TENANT_ID));
            var message = new Message(Encoding.UTF8.GetBytes(messageContent));
            await queueClient.SendAsync(message);

            return new OkResult();
        }
    }
}