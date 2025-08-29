using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Files.Shares;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace st10105598_ABCRetail_CLDV112w.Controllers
{
    public class LogsController : Controller
    {
        private readonly ShareClient _shareClient;

        public LogsController(IConfiguration config)
        {
            var conn = config.GetConnectionString("AzureStorage");
            var shareName = config["FileShareName"];

            if (string.IsNullOrEmpty(conn) || string.IsNullOrEmpty(shareName))
            {
                throw new InvalidOperationException("AzureStorage connection string or FileShareName is not configured.");
            }

            _shareClient = new ShareClient(conn, shareName);
            _shareClient.CreateIfNotExists();
        }

        public async Task<IActionResult> Index()
        {
            var directory = _shareClient.GetRootDirectoryClient();
          

            var logContents = new List<string>();

            await foreach (var file in directory.GetFilesAndDirectoriesAsync())
            {
                if (!file.IsDirectory)
                {
                    var fileClient = directory.GetFileClient(file.Name);
                    var download = await fileClient.DownloadAsync();

                    using (var reader = new StreamReader(download.Value.Content))
                    {
                        logContents.Add(await reader.ReadToEndAsync());
                    }
                }
            }

            return View(logContents);
        }
    }
}
