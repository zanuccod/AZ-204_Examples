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

            switch(input.ElementAt(0))
            {
                case 'n':
                    AddMessages(input[2..])
                        .GetAwaiter()
                        .GetResult();
                    break;
                default:
                    Console.WriteLine("Given command is invalid, insert a valid input command");
                    PrintIntroMessages();
                    break;
            }
        }

        private static async Task AddMessages(string message)
        {
            var queueClient = new QueueClient(connectionStr, queueName);

            await queueClient.CreateIfNotExistsAsync();

            await queueClient.SendMessageAsync(message);

            Console.WriteLine($"New message added to the queue <{queueName}>, message: <{message}>");
        }

        private static void PrintIntroMessages()
        {
            Console.WriteLine("Type one of the following commands:");
            Console.WriteLine("\t n <message>: to add a new message");
        }
    }
}
