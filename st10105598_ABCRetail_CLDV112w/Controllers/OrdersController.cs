using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using st10105598_ABCRetail_CLDV112w.Services;
using System;
using System.Threading.Tasks;

namespace st10105598_ABCRetail_CLDV112w.Controllers
{
    public class OrdersController : Controller
    {
        private readonly QueueStorageService _queueService;
        private readonly FileStorageService _fileService;

        public OrdersController(IConfiguration config)
        {
            var conn = config.GetConnectionString("AzureStorage");
            var queueName = config["QueueName"];
            var shareName = config["FileShareName"];

            if (string.IsNullOrEmpty(conn) || string.IsNullOrEmpty(queueName) || string.IsNullOrEmpty(shareName))
            {
                throw new InvalidOperationException("AzureStorage connection string, QueueName, or FileShareName is not properly configured.");
            }

            _queueService = new QueueStorageService(conn, queueName);
            _fileService = new FileStorageService(conn, shareName);
        }

        // GET: Show order form
        [HttpGet]
        public IActionResult CreateOrder()
        {
            return View();
        }

        // POST: Process order
        [HttpPost]
        public async Task<IActionResult> CreateOrder(int orderId)
        {
            if (orderId <= 0)
            {
                ViewBag.Message = "Please enter a valid order ID.";
                return View();
            }

            string message = $"Processing order #{orderId} at {DateTime.Now}";
            await _queueService.SendMessageAsync(message);

            // Log to Azure File Share
            string logFileName = $"order-{orderId}-{DateTime.Now:yyyyMMddHHmmss}.txt";
            await _fileService.UploadLogAsync(logFileName, message);

            ViewBag.Message = $"Order #{orderId} added to queue and logged!";
            return View();
        }

        // GET: View messages in the queue
        public async Task<IActionResult> ViewQueue()
        {
            var messages = await _queueService.PeekMessagesAsync();
            return View(messages);
        }
    }
}
