using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace st10105598_ABCRetail_CLDV112w.Services
{
    public class QueueStorageService
    {
        private readonly QueueClient _queueClient;

        public QueueStorageService(string connectionString, string queueName)
        {
            _queueClient = new QueueClient(connectionString, queueName);
            _queueClient.CreateIfNotExists();
        }

        // Send message to queue
        public async Task SendMessageAsync(string message)
        {
            await _queueClient.SendMessageAsync(message);
        }

        // Peek messages (see what's in queue without removing)
        public async Task<List<string>> PeekMessagesAsync(int maxMessages = 5)
        {
            var results = await _queueClient.PeekMessagesAsync(maxMessages);
            return results.Value.Select(m => m.MessageText).ToList();
        }

        // Receive and remove messages
        public async Task<List<string>> ReceiveMessagesAsync(int maxMessages = 5)
        {
            var results = await _queueClient.ReceiveMessagesAsync(maxMessages);
            var messages = new List<string>();

            foreach (var msg in results.Value)
            {
                messages.Add(msg.MessageText);
                await _queueClient.DeleteMessageAsync(msg.MessageId, msg.PopReceipt);
            }

            return messages;
        }
    }
}
