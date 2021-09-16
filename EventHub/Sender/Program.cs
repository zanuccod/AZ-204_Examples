using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Azure.Security.KeyVault.Secrets;
using Sender.Entities;

namespace Sender
{
    /**
     * Pre requisites
     *  - Create Azure Event Hub resource
     *  - Create Azure Key Vault resource
     *  - Register the application to AAD (Azure Active Directory) to 
     *    retrieve the TENNANT_ID and CLIENT_ID
     *  - Create a new CLIENT_SECRET on the registered app on AAD
     *  - Add Access Policy to Azure Key Vault for previous created app on AAD
     *    with GET and LIST rights
     *  - On the Event Hub resource, create a new Hub and on Shared Access Policies
     *    create a new Policy for the Sender, and retrieve the connection string 
     *    for the sender
     *  - Add new secret on KeyVault with the connection string to Sender Event Hub
     */
    public class Program
    {
        private const string TENNANT_ID = "";
        private const string CLIENT_ID = "";
        private const string CLIENT_SECRET = "";

        private const string KEY_VAULT_URL = "";
        private const string SENDER_CON_STR_KEY_NAME = "SenderConnectionString";

        public static async Task Main(string[] args)
        {
            Console.WriteLine($"Retrive client credentials...");
            var clientCredentials = GetClientSecretCredential();
            Console.WriteLine($"Ok\n");

            Console.WriteLine($"Retrive hub connection string from KeyVault");
            var hubConnectionString = new SecretClient(new Uri(KEY_VAULT_URL), clientCredentials);
            var connStr = await hubConnectionString
                .GetSecretAsync(SENDER_CON_STR_KEY_NAME);
            Console.WriteLine($"Ok\n");


            var eventHubClient = new EventHubProducerClient(connStr.Value.Value);
            var evtDataBatch = await eventHubClient
                .CreateBatchAsync();

            var objToSend = GetCollectionToSend();
            foreach(var obj in objToSend)
            {
                evtDataBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(obj.ToString())));
            }

            Console.WriteLine($"Send <{objToSend.Count()}> object to event hubs");
            await eventHubClient
                .SendAsync(evtDataBatch);
            Console.WriteLine($"Ok\n");
        }

        private static ClientSecretCredential GetClientSecretCredential()
        {
            return new ClientSecretCredential(TENNANT_ID, CLIENT_ID, CLIENT_SECRET);
        }

        private static IEnumerable<ObjectToSend> GetCollectionToSend()
        {
            return new List<ObjectToSend>()
            {
                new ObjectToSend("pippo"),
                new ObjectToSend("pluto"),
                new ObjectToSend("paperino")
            };
        }
    }
}
