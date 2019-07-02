using System;
using System.Threading.Tasks;

using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;

namespace ManagedIdentityExample.ConsoleApp
{
    class Program
    {
        private const string SECRET_NAME = "SomeSecret";
        private const string VAULT_URL = "https://mi-example.vault.azure.net/";

        static async Task Main(string[] args)
        {
            var tokenProvider = new AzureServiceTokenProvider();
            var kvc = new KeyVaultClient(
                new KeyVaultClient.AuthenticationCallback(
                    tokenProvider.KeyVaultTokenCallback));

            var x = await kvc.GetSecretAsync(VAULT_URL, SECRET_NAME);
            Console.WriteLine($"The secret we got from Key Vault: {x.Value}");
            Console.ReadLine();
        }
    }
}