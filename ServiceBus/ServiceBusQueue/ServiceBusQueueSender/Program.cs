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
        private const string TENNANT_ID = "b00367e2-193a-4f48-94de-7245d45c0947";
        private const string CLIENT_ID = "fca738e9-37f4-4a07-b335-8e1e66a5c977";
        private const string CLIENT_SECRET = "Zz17Q~cNrmuOi_1pbEDbMQJ6AR1L5iSb2KNin";

        private const string KEY_VAULT_URL = "https://keyvaultricopegnata.vault.azure.net/";
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
