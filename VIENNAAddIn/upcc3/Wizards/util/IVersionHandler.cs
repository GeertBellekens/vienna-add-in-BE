using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.Wizards.util
{
    public interface IVersionHandler
    {
        List<string> GetMajorVersions();
        List<string> GetMinorVersions(string majorVersion);
        string GetComment(string majorVersion, string minorVersion);
    }
}