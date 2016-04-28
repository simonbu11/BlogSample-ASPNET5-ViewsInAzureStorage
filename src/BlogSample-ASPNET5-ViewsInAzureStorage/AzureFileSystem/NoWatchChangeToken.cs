using System;
using Microsoft.Extensions.Primitives;

namespace BlogSample_ASPNET5_ViewsInAzureStorage.AzureFileSystem
{
    public class NoWatchChangeToken : IChangeToken
    {
        public IDisposable RegisterChangeCallback(Action<object> callback, object state)
        {
            return new EmptyDisposable();
        }

        public bool HasChanged => false;
        public bool ActiveChangeCallbacks => false;
    }
}