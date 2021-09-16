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
        private const string TENNANT_ID = "";
        private const string CLIENT_ID = "";
        private const string CLIENT_SECRET = "";

        private const string KEY_VAULT_URL = "";
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
