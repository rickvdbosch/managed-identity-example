using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ManagedIdentityExample.Functions
{
    public static class ConnectToStorage
    {
        #region Constants

        private const string BLOBNAME = "https://<YOUR_STORAGEACCOUNT_NAME>.blob.core.windows.net/<YOUR_BLOB_NAME>";

        #endregion

        [FunctionName("ConnectToStorage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {

            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            // Get the initial access token and the interval at which to refresh it.
            var tokenAndFrequency = await TokenRenewerAsync(azureServiceTokenProvider, CancellationToken.None);

            // Create a TokenCredential which can be used to pass into the StorageCredentials constructor.
            var tokenCredential = new TokenCredential(tokenAndFrequency.Token,
                                                      TokenRenewerAsync,
                                                      azureServiceTokenProvider,
                                                      tokenAndFrequency.Frequency.Value);

            var storageCredentials = new StorageCredentials(tokenCredential);
            var blob = new CloudBlockBlob(new Uri(BLOBNAME), storageCredentials);

            return new OkObjectResult(await blob.DownloadTextAsync());
        }

        #region Helper methods

        private static async Task<NewTokenAndFrequency> TokenRenewerAsync(Object state, CancellationToken cancellationToken)
        {
            // Specify the resource ID for requesting Azure AD tokens for Azure Storage.
            // Note that you can also specify the root URI for your storage account as the resource ID.
            const string StorageResource = "https://storage.azure.com/";

            // Use the same token provider to request a new token.
            var authResult = await ((AzureServiceTokenProvider)state).GetAuthenticationResultAsync(StorageResource);

            // Renew the token 5 minutes before it expires.
            var next = (authResult.ExpiresOn - DateTimeOffset.UtcNow) - TimeSpan.FromMinutes(5);
            if (next.Ticks < 0)
            {
                next = default(TimeSpan);
                Console.WriteLine("Renewing token...");
            }

            // Return the new token and the next refresh time.
            return new NewTokenAndFrequency(authResult.AccessToken, next);
        }

        #endregion
    }
}