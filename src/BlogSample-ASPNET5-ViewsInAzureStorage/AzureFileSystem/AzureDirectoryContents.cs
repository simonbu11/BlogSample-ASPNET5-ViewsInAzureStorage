using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.FileProviders;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BlogSample_ASPNET5_ViewsInAzureStorage.AzureFileSystem
{
    public class AzureDirectoryContents : IDirectoryContents
    {
        private readonly CloudBlobDirectory _blob;
        private readonly BlobResultSegment _directoryContent;

        public AzureDirectoryContents(CloudBlobDirectory blob)
        {
            _blob = blob;
            _directoryContent = blob.ListBlobsSegmented(null);
            Exists = _directoryContent.Results != null && _directoryContent.Results.Any();
        }

        public IEnumerator<IFileInfo> GetEnumerator()
        {
            return ContentToFileInfo().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ContentToFileInfo().GetEnumerator();
        }

        public bool Exists { get; }

        private IEnumerable<IFileInfo> ContentToFileInfo()
        {
            if (_directoryContent == null || _directoryContent.Results == null || !_directoryContent.Results.Any())
            {
                return new IFileInfo[0];
            }

            return _directoryContent.Results.Select(blob => new AzureFileInfo(blob));
        }
    }
}