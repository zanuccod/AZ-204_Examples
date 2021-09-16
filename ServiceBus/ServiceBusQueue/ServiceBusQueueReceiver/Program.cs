using System;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Azure.Security.KeyVault.Secrets;

namespace ServiceBusQueueReceiver
{
    /**
     * ServiceBus namespace: namespacericopegnata
     * ServiceBus queue: queuericopegnata
     */
    public class Program
    {
        private const string TENNANT_ID = "b00367e2-193a-4f48-94de-7245d45c0947";
        private const string CLIENT_ID = "fca738e9-37f4-4a07-b335-8e1e66a5c977";
        private const string CLIENT_SECRET = "C5q7Q~JxrnZPKP9NHHFEUwBnTip4cpJw4r5qM";

        private const string KEY_VAULT_URL = "https://keyvaultricopegnata.vault.azure.net/";
        private const string SENDER_CON_STR_KEY_NAME = "Receiver";
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
                var receiver = serviceBusClient
                    .CreateReceiver(QUEUE_NAME);

                Console.WriteLine($"Try to receive message on the queue <{QUEUE_NAME}>");

                var messages = await receiver
                    .ReceiveMessagesAsync(10);
                Console.WriteLine($"Found <{messages.Count}> messages");

                foreach (var message in messages)
                {
                    Console.WriteLine($"{message.Body}");
                }
                await receiver
                    .DisposeAsync();
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
