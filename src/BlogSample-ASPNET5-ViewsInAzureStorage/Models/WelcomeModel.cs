using System;

namespace BlogSample_ASPNET5_ViewsInAzureStorage.Models
{
    public class WelcomeModel
    {
        public string DisplayName { get; set; }
        public DateTime LastSeen { get; set; }
        public int NumberOfMessages { get; set; }
    }
}
