using Azure.Storage.Files.Shares;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace st10105598_ABCRetail_CLDV112w.Services
{
    public class FileStorageService
    {
        private readonly ShareClient _shareClient;

        public FileStorageService(string connectionString, string shareName)
        {
            if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(shareName))
            {
                throw new ArgumentException("Connection string or share name is not provided.");
            }

            _shareClient = new ShareClient(connectionString, shareName);
            _shareClient.CreateIfNotExists();
        }

        // Uploads a text-based log file to Azure File Share
        public async Task UploadLogAsync(string logFileName, string logContent)
        {
            var directoryClient = _shareClient.GetRootDirectoryClient();
            await directoryClient.CreateIfNotExistsAsync();

            var fileClient = directoryClient.GetFileClient(logFileName);

            byte[] fileBytes = Encoding.UTF8.GetBytes(logContent);
            using (var stream = new MemoryStream(fileBytes))
            {
                await fileClient.CreateAsync(stream.Length);
                await fileClient.UploadAsync(stream);
            }
        }
    }
}
