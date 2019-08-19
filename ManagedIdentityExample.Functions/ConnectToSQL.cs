using System.Data.SqlClient;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace ManagedIdentityExample.Functions
{
    public static class ConnectToSQL
    {
        #region Constants

        private const string CONNECTIONSTRING = "Server=tcp:rickvdbosch01.database.windows.net,1433;Initial Catalog=rvdb-bt001;";
        private const string QUERY = "Select top 1 Descr from Test";

        #endregion

        [FunctionName("ConnectToSQL")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            using (var connection = new SqlConnection(CONNECTIONSTRING))
            using (var command = new SqlCommand(QUERY, connection))
            {
                connection.AccessToken = await (new AzureServiceTokenProvider().GetAccessTokenAsync("https://database.windows.net/"));
                try
                {
                    await connection.OpenAsync();
                    var result = (await command.ExecuteScalarAsync()).ToString();

                    return new OkObjectResult(result);
                }
                catch (System.Exception e)
                {
                    log.LogError(e.Message);
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
        }
    }
}