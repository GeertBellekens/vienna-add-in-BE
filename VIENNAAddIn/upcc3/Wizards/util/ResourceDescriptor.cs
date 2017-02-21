// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System;

namespace VIENNAAddIn.upcc3.Wizards.util
{
    public class ResourceDescriptor
    {
        /// A string array of the resources to be retrieved. An example would be:
        /// new string[] {"primlibrary.xmi", "cdtlibrary.xmi"}
        public string[] Resources { get; private set; }

        /// A string representing the URI where the resources are to be retrieved from. An
        /// example would be "http://www.myresources.com/xmi/".
        public string DownloadUri { get; private set; }

        /// A string representing the location on the local file system specifying where the
        /// resources retrieved should be stored at. An example would be "c:\\temp\\cache\\".
        public string StorageDirectory { get; private set; }

        public ResourceDescriptor()
        {
            Resources = new[] {"enumlibrary.xmi", "primlibrary.xmi", "cdtlibrary.xmi", "cclibrary.xmi"};
            DownloadUri = "http://www.umm-dev.org/xmi/";
            //changed to users folder in order to resolve issue 70
            StorageDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\ViennaAddIn\\upcc3\\resources\\ccl\\";
        }

        public ResourceDescriptor(ResourceDescriptor descriptor)
        {
            Resources = descriptor.Resources;
            DownloadUri = descriptor.DownloadUri;
            StorageDirectory = descriptor.StorageDirectory;
        }

        public ResourceDescriptor(string[] resources, string downloadURI, string storageDirectory)
        {
            Resources = resources;
            DownloadUri = downloadURI;
            StorageDirectory = storageDirectory;
        }

        public ResourceDescriptor(string downloadUri, string majorVersion, string minorVersion) : this()
        {
            string relativePath = majorVersion + "_" + minorVersion;

            DownloadUri = downloadUri + relativePath;

            if (downloadUri.StartsWith("http://"))
            {
                DownloadUri += "/";
            }
            else
            {
                DownloadUri += "\\";
            }
            
            StorageDirectory += relativePath + "\\";            
        }

    }
}