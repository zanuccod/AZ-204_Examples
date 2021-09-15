using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Security.KeyVault.Secrets;

namespace Reader
{

    public class Program
    {
        private const string TENNANT_ID = "b00367e2-193a-4f48-94de-7245d45c0947";
        private const string CLIENT_ID = "dac36c36-9b51-4b47-86b7-3d5e62c5f8d3";
        private const string CLIENT_SECRET = ".aZgOw.-Zm88xZd-Vs3.wdr2VKXQ_tA4kf";

        private const string KEY_VAULT_URL = "https://eventhubvaultricopegnata.vault.azure.net/";
        private const string READER_CON_STR_KEY_NAME = "ReaderConnectionString";

        private const string CONSUMER_GROUP = "$Default";

        public static async Task Main(string[] args)
        {
            Console.WriteLine($"Retrive client credentials...");
            var clientCredentials = GetClientSecretCredential();
            Console.WriteLine($"Ok\n");

            Console.WriteLine($"Retrive hub connection string from KeyVault");
            var hubConnectionString = new SecretClient(new Uri(KEY_VAULT_URL), clientCredentials);
            var connStr = await hubConnectionString
                .GetSecretAsync(READER_CON_STR_KEY_NAME);
            Console.WriteLine($"Ok\n");

            var consumerGroup = new EventHubConsumerClient(CONSUMER_GROUP, connStr.Value.Value);
            await foreach(var evt in consumerGroup.ReadEventsAsync())
            {
                Console.WriteLine($"PartitionId: <{evt.Partition}>");
                Console.WriteLine($"Data Offset: <{evt.Data.Offset}>");
                Console.WriteLine($"Event Body: <{Encoding.UTF8.GetString(evt.Data.EventBody)}>");
            }

        }

        private static ClientSecretCredential GetClientSecretCredential()
        {
            return new ClientSecretCredential(TENNANT_ID, CLIENT_ID, CLIENT_SECRET);
        }
    }
}
