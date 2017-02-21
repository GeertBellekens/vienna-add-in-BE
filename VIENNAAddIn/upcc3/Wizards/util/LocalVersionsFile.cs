namespace VIENNAAddIn.upcc3.Wizards.util
{
    public class LocalVersionsFile : IVersionsFile
    {
        private string versionsFilePath;

        public LocalVersionsFile(string filePath)
        {
            versionsFilePath = filePath;
        }

        public string GetContent()
        {
            return System.IO.File.ReadAllText(versionsFilePath);
        }
    }
}