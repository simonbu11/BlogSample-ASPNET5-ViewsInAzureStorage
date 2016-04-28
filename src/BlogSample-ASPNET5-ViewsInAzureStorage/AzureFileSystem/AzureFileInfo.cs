using System;
using System.IO;
using Microsoft.AspNet.FileProviders;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BlogSample_ASPNET5_ViewsInAzureStorage.AzureFileSystem
{
    public class AzureFileInfo : IFileInfo
    {
        private readonly CloudBlockBlob _blockBlob;

        public AzureFileInfo(IListBlobItem blob)
        {
            IsDirectory = (blob is CloudBlobDirectory);

            if (!IsDirectory)
            {
                _blockBlob = (CloudBlockBlob)blob;
                Exists = _blockBlob.Exists();
                if (Exists)
                {
                    Length = _blockBlob.Properties.Length;
                    PhysicalPath = _blockBlob.Uri.ToString();
                    Name = _blockBlob.Name;
                    LastModified = _blockBlob.Properties.LastModified.HasValue ? _blockBlob.Properties.LastModified.Value : DateTimeOffset.MinValue;
                }
            }
        }

        public Stream CreateReadStream()
        {
            var stream = new MemoryStream();
            _blockBlob.DownloadToStream(stream);
            stream.Position = 0;
            return stream;
        }

        public bool Exists { get; }
        public long Length { get; }
        public string PhysicalPath { get; }
        public string Name { get; }
        public DateTimeOffset LastModified { get; }
        public bool IsDirectory { get; }
    }
}
