using System.Net;

namespace VIENNAAddIn.upcc3.Wizards.util
{
    public class RemoteVersionsFile : IVersionsFile
    {
        private readonly string versionsFileUri;

        public RemoteVersionsFile(string uri)
        {
            versionsFileUri = uri;
        }

        public string GetContent()
        {
            using (var client = new WebClient())
            {
                return client.DownloadString(versionsFileUri);
            }
        }
    }
}