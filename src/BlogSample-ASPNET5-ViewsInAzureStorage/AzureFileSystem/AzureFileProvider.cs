using System;
using Microsoft.AspNet.FileProviders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BlogSample_ASPNET5_ViewsInAzureStorage.AzureFileSystem
{
    public class AzureFileProvider : IFileProvider
    {
        private readonly CloudBlobContainer _container;

        public AzureFileProvider(IConfigurationRoot configurationRoot)
        {
            var connectionString = configurationRoot.Get<string>("Azure:StorageConnectionString");
            var containerName = configurationRoot.Get<string>("Azure:BlobContainerName");

            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            _container = blobClient.GetContainerReference(containerName);
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            var azurePath = ConvertPath(subpath);
            var blob = _container.GetBlockBlobReference(azurePath);
            return new AzureFileInfo(blob);
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            var azurePath = ConvertPath(subpath);
            var blob = _container.GetDirectoryReference(azurePath);
            return new AzureDirectoryContents(blob);
        }

        public IChangeToken Watch(string filter)
        {
            return new NoWatchChangeToken();
        }

        private string ConvertPath(string path)
        {
            if (path.StartsWith("/Views/", StringComparison.OrdinalIgnoreCase))
            {
                path = path.Substring(7);
            }
            if (path.StartsWith("Views/", StringComparison.OrdinalIgnoreCase))
            {
                path = path.Substring(6);
            }
            if (path.StartsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                path = path.Substring(1);
            }
            return path;
        }
    }
}
