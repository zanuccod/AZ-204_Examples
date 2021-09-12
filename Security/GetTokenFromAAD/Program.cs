using System;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;

namespace GetTokenFromAAD
{
    public class Program
    {
        // data defined on App Registration on AAD
        private const string TENNANT_ID = "b00367e2-193a-4f48-94de-7245d45c0947";
        private const string CLIENT_ID = "b5f53607-b6a2-42e6-b38c-e16c95ea1219";
        private const string REDIRECT_URI = "http://localhost";

        public static async Task Main(string[] args)
        {
            Console.WriteLine("Try to login...");

            try
            {
                var appInstance = PublicClientApplicationBuilder.
                    Create(CLIENT_ID)
                    .WithAuthority(AzureCloudInstance.AzurePublic, TENNANT_ID)
                    .WithRedirectUri(REDIRECT_URI)
                    .Build();
                string[] scopes = { "user.read" };

                // Get Access Token from AAD
                var result = await appInstance
                    .AcquireTokenInteractive(scopes)
                    .ExecuteAsync();

                Console.WriteLine($"Received token:\n{result.AccessToken}");

                // Get resource from Microsoft Graph

                var provider = new InteractiveAuthenticationProvider(appInstance, scopes);
                var client = new GraphServiceClient(provider);

                var me = await client
                    .Me
                    .Request()
                    .GetAsync();

                Console.WriteLine($"Display Name:\t{me.DisplayName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to get the token: {ex.Message}");
            }
        }
    }
}
