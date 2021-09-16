using System;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Azure.Security.KeyVault.Secrets;

namespace ServiceBusQueueSender
{
    /**
     * ServiceBus namespace: namespacericopegnata
     * ServiceBus queue: queuericopegnata
     */
    public class Program
    {
        private const string TENNANT_ID = "";
        private const string CLIENT_ID = "";
        private const string CLIENT_SECRET = "";

        private const string KEY_VAULT_URL = "";
        private const string SENDER_CON_STR_KEY_NAME = "Sender";
        private const string QUEUE_NAME = "queuericopegnata";

        public static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine($"Retrive client credentials...");
                var clientCredentials = GetClientSecretCredential();
                Console.WriteLine($"Ok\n");

                Console.WriteLine($"Retrive hub connection string from KeyVault");
                var keyVaultSecretClient = new SecretClient(new Uri(KEY_VAULT_URL), clientCredentials);
                var connStr = await keyVaultSecretClient
                    .GetSecretAsync(SENDER_CON_STR_KEY_NAME);
                Console.WriteLine($"Ok\n");

                var serviceBusClient = new ServiceBusClient(connStr.Value.Value);
                var sender = serviceBusClient
                    .CreateSender(QUEUE_NAME);

                var messageId = Guid.NewGuid();
                var message = new ServiceBusMessage($"Message with id <{messageId}> sent from Sender");
                Console.WriteLine($"Try to send message with id <{messageId}> ");
                await sender
                    .SendMessageAsync(new ServiceBusMessage($"Message with id <{Guid.NewGuid()}> sent from Sender"));
                Console.WriteLine($"Ok\n");

                await sender
                    .DisposeAsync();
                await Task.Delay(2000);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to send the message: {ex.Message}");
            }
        }

        private static ClientSecretCredential GetClientSecretCredential()
        {
            return new ClientSecretCredential(TENNANT_ID, CLIENT_ID, CLIENT_SECRET);
        }
    }
}
