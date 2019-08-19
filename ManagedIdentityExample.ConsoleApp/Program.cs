using System;
using System.Threading.Tasks;

using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;

namespace ManagedIdentityExample.ConsoleApp
{
    public class Program
    {
        #region Constants

        // Name of the secret to get from Key Vault
        private const string SECRET_NAME = "<YOUR_SECRET'S_NAME>";

        // The URL to the Key Vault to get the secret from
        private const string VAULT_URL = "https://<YOUR_VAULT'S_NAME>.vault.azure.net/";

        #endregion

        static async Task Main(string[] args)
        {
            var tokenProvider = new AzureServiceTokenProvider();
            using (var kvc = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(tokenProvider.KeyVaultTokenCallback)))
            {
                var x = await kvc.GetSecretAsync(VAULT_URL, SECRET_NAME);
                Console.WriteLine($"The value of the secret we got from Key Vault: {x.Value}");
            }
            Console.ReadLine();
        }
    }
}