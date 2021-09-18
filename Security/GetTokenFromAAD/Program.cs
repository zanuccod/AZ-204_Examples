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
        private const string TENNANT_ID = "";
        private const string CLIENT_ID = "";
        private const string REDIRECT_URI = "http://localhost";

        public static void Main(string[] args)
        {
            Console.WriteLine("Try to login...");

            try
            {
                //var appInstance = PublicClientApplicationBuilder.
                //    Create(CLIENT_ID)
                //    .WithAuthority(AzureCloudInstance.AzurePublic, TENNANT_ID)
                //    .WithRedirectUri(REDIRECT_URI)
                //    .Build();
                //string[] scopes = { "user.read" };

                //// Get Access Token from AAD
                //var result = await appInstance
                //    .AcquireTokenInteractive(scopes)
                //    .ExecuteAsync();

                //Console.WriteLine($"Received token:\n{result.AccessToken}");

                //// Get resource from Microsoft Graph

                //var provider = new InteractiveAuthenticationProvider(appInstance, scopes);
                //var client = new GraphServiceClient(provider);

                //var me = await client
                //    .Me
                //    .Request()
                //    .GetAsync();

                //Console.WriteLine($"Display Name:\t{me.DisplayName}");

                var publicApp = BuildPublicClientApplication();
                var confidentialApp = BuildConfidentialClientApplication();

                Task.WaitAll(publicApp, confidentialApp);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to get the token: {ex.Message}");
            }
        }

        private static async Task BuildPublicClientApplication()
        {
            Console.WriteLine($"Try to login with a PublicClientApplication");

            // data defined on App Registration on AAD
            const string TENNANT_ID = "";
            const string CLIENT_ID = "";
            const string REDIRECT_URI = "http://localhost";

            var publicClientApp = PublicClientApplicationBuilder
                .Create(CLIENT_ID)
                .WithAuthority(AzureCloudInstance.AzurePublic, TENNANT_ID)
                .WithRedirectUri(REDIRECT_URI)
                .Build();

            string[] scopes = { "user.read" };

            var result = await publicClientApp
                .AcquireTokenInteractive(scopes)
                .ExecuteAsync();

            Console.WriteLine($"Received token:\n{result.AccessToken}");

            await RetrivePersonalNameFromMicrosoftGraph(publicClientApp, scopes);
        }

        private static async Task BuildConfidentialClientApplication()
        {
            // data defined on App Registration on AAD
            const string TENNANT_ID = "";
            const string CLIENT_ID = "";
            const string CLIENT_SECRET = "";
            const string REDIRECT_URI = "http://localhost";

            var confidentialClientApp = ConfidentialClientApplicationBuilder
                .Create(CLIENT_ID)
                .WithAuthority(AzureCloudInstance.AzurePublic, TENNANT_ID)
                .WithClientSecret(CLIENT_SECRET)
                .WithRedirectUri(REDIRECT_URI)
                .Build();

            string[] scopes = { "https://graph.microsoft.com/.default" };

            var result = await confidentialClientApp
                .AcquireTokenForClient(scopes)
                .ExecuteAsync();

            Console.WriteLine($"Received token:\n{result.AccessToken}");
        }

        private static async Task RetrivePersonalNameFromMicrosoftGraph(IPublicClientApplication clientApp, string[] scopes)
        {
            var provider = new InteractiveAuthenticationProvider(clientApp, scopes);
            var client = new GraphServiceClient(provider);
            var me = await client
                .Me
                .Request()
                .GetAsync();

            Console.WriteLine($"Display Name:\t{me.DisplayName}");
        }
    }
}
