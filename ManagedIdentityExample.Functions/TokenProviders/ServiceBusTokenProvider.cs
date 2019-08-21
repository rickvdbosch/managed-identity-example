using System;
using System.Threading.Tasks;

using Microsoft.Azure.ServiceBus.Primitives;
using Microsoft.Azure.Services.AppAuthentication;

namespace ManagedIdentityExample.Functions.TokenProviders
{
    public class ServiceBusTokenProvider : TokenProvider
    {
        #region Constants

        private const string RESOURCE_SERVICE_BUS = "https://servicebus.azure.net/";

        #endregion

        #region Fields

        private string _tenantId;

        #endregion

        #region Constructors

        public ServiceBusTokenProvider(string tenantId = null)
        {
            _tenantId = tenantId;
        }

        #endregion

        public override async Task<SecurityToken> GetTokenAsync(string appliesTo, TimeSpan timeout)
        {
            var tokenProvider = new AzureServiceTokenProvider();
            var token = await tokenProvider.GetAccessTokenAsync(RESOURCE_SERVICE_BUS, _tenantId);

            return new JsonSecurityToken(token, appliesTo);
        }
    }
}