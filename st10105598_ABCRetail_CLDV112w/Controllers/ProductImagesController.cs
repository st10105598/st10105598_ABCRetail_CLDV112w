using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using st10105598_ABCRetail_CLDV112w.Services;
using System;
using System.Threading.Tasks;

namespace st10105598_ABCRetail_CLDV112w.Controllers
{
    public class ProductImagesController : Controller
    {
        private readonly BlobStorageService _blobService;
        private readonly QueueStorageService _queueService;
        private readonly FileStorageService _fileService;

        public ProductImagesController(IConfiguration config)
        {
            var conn = config.GetConnectionString("AzureStorage");
            var containerName = "product-images";  // updated here
            var queueName = config["QueueName"];
            var shareName = config["FileShareName"];

            if (string.IsNullOrEmpty(conn) ||
                string.IsNullOrEmpty(queueName) ||
                string.IsNullOrEmpty(shareName))
            {
                throw new InvalidOperationException("Azure Storage connection string, queue name, or file share name is not configured.");
            }

            _blobService = new BlobStorageService(conn, containerName);
            _queueService = new QueueStorageService(conn, queueName);
            _fileService = new FileStorageService(conn, shareName);
        }

        // Display uploaded images
        public IActionResult Index()
        {
            var images = _blobService.GetAllImages();
            return View(images);
        }

        // Upload form (GET)
        public IActionResult Upload()
        {
            return View();
        }

        // Upload handler (POST)
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var imageUrl = await _blobService.UploadImageAsync(file);

                string message = $"Uploaded image: {file.FileName} ({imageUrl})";
                await _queueService.SendMessageAsync(message);

                string logFileName = $"upload-{DateTime.Now:yyyyMMddHHmmss}.txt";
                string logContent = $"Uploaded image {file.FileName} at {DateTime.Now}\nURL: {imageUrl}";
                await _fileService.UploadLogAsync(logFileName, logContent);
            }

            return RedirectToAction("Index");
        }
    }
}
