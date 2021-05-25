using System;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Queues;

namespace QueueStorage
{
    class Program
    {
        private static readonly string connectionStr = "";
        private static readonly string queueName = "";

        static void Main(string[] args)
        {
            PrintIntroMessages();

            var input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Insert a valid input");
            }

            int nOfMessages = 0;
            switch (input.ElementAt(0))
            {
                case 'n':
                    AddMessages(input[2..])
                        .GetAwaiter()
                        .GetResult();
                    break;

                case 'p':
                    if (int.TryParse(input[2..], out nOfMessages))
                    {
                        PeakMessages(nOfMessages)
                            .GetAwaiter()
                            .GetResult();
                    }
                    else
                    {
                        Console.WriteLine($"Given input <{input[2..]}> is not a valid number");
                    }
                    break;

                case 'r':
                    if (int.TryParse(input[2..], out nOfMessages))
                    {
                        ReadMessages(nOfMessages)
                            .GetAwaiter()
                            .GetResult();
                    }
                    else
                    {
                        Console.WriteLine($"Given input <{input[2..]}> is not a valid number");
                    }
                    break;
                default:
                    Console.WriteLine("Given command is invalid, insert a valid input command");
                    PrintIntroMessages();
                    break;
            }
        }

        private static async Task AddMessages(string message)
        {
            var queueClient = await GetQueueClient();
            await queueClient.SendMessageAsync(message);

            Console.WriteLine($"New message added to the queue <{queueName}>, message: <{message}>");
        }

        // Peak, Read, Delete, Receive

        /// <summary>
        /// Read but leave on the queue the messages
        /// </summary>
        /// <param name="numberOfMessages"></param>
        /// <returns></returns>
        private static async Task PeakMessages(int numberOfMessages)
        {
            var queueClient = await GetQueueClient();

            var items = await queueClient.PeekMessagesAsync(numberOfMessages);

            for(var i = 0; i < items.Value.Length; i++)
            {
                Console.WriteLine($"{i}: <{items.Value.ElementAt(i).MessageId}> <{items.Value.ElementAt(i).Body}>");
            }
        }

        /// <summary>
        /// Get and remove from queue the messages
        /// </summary>
        /// <param name="numberOfMessages"></param>
        /// <returns></returns>
        private static async Task ReadMessages(int numberOfMessages)
        {
            var queueClient = await GetQueueClient();

            var items = await queueClient.ReceiveMessagesAsync(numberOfMessages);

            for (var i = 0; i < items.Value.Length; i++)
            {
                Console.WriteLine($"{i}: <{items.Value.ElementAt(i).MessageId}> <{items.Value.ElementAt(i).Body}>");
            }
        }

        private static void PrintIntroMessages()
        {
            Console.WriteLine("Type one of the following commands:");
            Console.WriteLine("\t n <message>: to add a new message");
            Console.WriteLine("\t p <numberOfMessages>: to peak first <numberOfMessages> messages");
            Console.WriteLine("\t r <numberOfMessages>: to read first <numberOfMessages> messages");
        }

        private static async Task<QueueClient> GetQueueClient()
        {
            var queueClient = new QueueClient(connectionStr, queueName);

            await queueClient.CreateIfNotExistsAsync();

            return queueClient;
        }
    }
}
