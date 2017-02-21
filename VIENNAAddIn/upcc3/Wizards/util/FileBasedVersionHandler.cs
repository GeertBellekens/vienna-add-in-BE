// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.Wizards.util
{
    public class FileBasedVersionHandler : IVersionHandler
    {
        private readonly IVersionsFile versionsFile;

        public FileBasedVersionHandler(IVersionsFile versionsFile)
        {
            this.versionsFile = versionsFile;
        }

        public List<VersionDescriptor> AvailableVersions { get; private set; }

        #region IVersionHandler Members

        public List<string> GetMajorVersions()
        {
            var majorVersions = new List<string>();

            foreach (VersionDescriptor descriptor in AvailableVersions)
            {
                if (!(majorVersions.Contains(descriptor.Major)))
                {
                    majorVersions.Add(descriptor.Major);
                }
            }

            return majorVersions;
        }

        public List<string> GetMinorVersions(string majorVersion)
        {
            var minorVersions = new List<string>();

            foreach (VersionDescriptor descriptor in AvailableVersions)
            {
                if (descriptor.Major == majorVersion)
                {
                    minorVersions.Add(descriptor.Minor);
                }
            }

            return minorVersions;
        }

        public string GetComment(string majorVersion, string minorVersion)
        {
            foreach (VersionDescriptor descriptor in AvailableVersions)
            {
                if ((descriptor.Major == majorVersion) && (descriptor.Minor == minorVersion))
                {
                    return descriptor.Comment;
                }
            }

            return "";
        }

        #endregion

        public void RetrieveAvailableVersions()
        {
            AvailableVersions = new List<VersionDescriptor>();

            string versionsString = versionsFile.GetContent();

            foreach (string version in versionsString.Split('\n'))
            {
                if (version.Trim() != "")
                {
                    AvailableVersions.Add(VersionDescriptor.ParseVersionString(version));
                }
            }
        }
    }
}