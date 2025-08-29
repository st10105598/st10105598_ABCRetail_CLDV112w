using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace st10105598_ABCRetail_CLDV112w.Services
{
    public class BlobStorageService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobStorageService(string connectionString, string containerName)
        {
            _containerClient = new BlobContainerClient(connectionString, containerName);
            _containerClient.CreateIfNotExists();
        }

        // Upload an image
        public async Task<string> UploadImageAsync(IFormFile file)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            BlobClient blobClient = _containerClient.GetBlobClient(fileName);

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            return blobClient.Uri.ToString(); // return image URL
        }

        // Get all images
        public List<string> GetAllImages()
        {
            var urls = new List<string>();
            foreach (BlobItem blobItem in _containerClient.GetBlobs())
            {
                var blobClient = _containerClient.GetBlobClient(blobItem.Name);
                urls.Add(blobClient.Uri.ToString());
            }
            return urls;
        }
    }
}
