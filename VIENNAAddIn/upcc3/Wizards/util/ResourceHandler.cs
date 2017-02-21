// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.IO;
using System.Net;

namespace VIENNAAddIn.upcc3.Wizards.util
{
    ///<summary>
    /// The ResourceHandler class may be used to retrieve resources located at a particular URI on 
    /// the web to the local file system. The configuration of the ResourceHandler is achieved 
    /// through the the constructor. More details may be found in the documentation of the 
    /// constructor. 
    ///</summary>
    public class ResourceHandler
    {
        #region Class Fields

        private readonly ResourceDescriptor resourceDescriptor;

        #endregion

        #region Constructor

        public ResourceHandler(ResourceDescriptor resourceDescriptor)
        {
            this.resourceDescriptor = resourceDescriptor;
        }

        #endregion

        #region Class Methods

        ///<summary>
        /// The method retrieves resources located at a particular URI and caches these resources on the 
        /// local file system. Retrieving of the resources is performed based upon the parameters set in 
        /// the ResourceHandler constructor.
        ///</summary>
        public void CacheResourcesLocally()
        {
            if (resourceDescriptor.DownloadUri.StartsWith("http://"))
            {
                if (!(Directory.Exists(resourceDescriptor.StorageDirectory)))
                {
                    Directory.CreateDirectory(resourceDescriptor.StorageDirectory);
                }

                foreach (string resourceFile in resourceDescriptor.Resources)
                {
                    if (!(File.Exists(resourceDescriptor.StorageDirectory + resourceFile)))
                    {
                        CacheSingleResourceLocally(resourceDescriptor.DownloadUri, resourceDescriptor.StorageDirectory, resourceFile);
                    }                    
                }
            }
        }

        #endregion

        #region Private Class Methods

        ///<summary>
        /// The method retrieves a resource, such as an XMI file, located at a particular URI. 
        /// The content of the resource is returned as a string. 
        ///</summary>
        ///<param name="uri">
        /// A URI pointing to the resource to be retrieved. An example would be 
        /// "http://www.umm-dev.org/xmi/primlibrary.xmi".
        /// </param>
        ///<returns>
        /// A string representation from the content located at the specified URI.
        ///</returns>
        private static string RetrieveContentFromUri(string uri)
        {
            using (var client = new WebClient())
            {
                return client.DownloadString(uri);
            }
        }

        ///<summary>
        /// The method writes content to a particular file on a file system. The name of the output file
        /// as well as the content to be written to the output file are specified in the parameters of the
        /// method.
        ///</summary>
        ///<param name="downloadUri">
        /// A URI pointing to the resource to be retrieved. An example would be 
        /// "http://www.umm-dev.org/xmi/primlibrary.xmi".
        /// </param>        
        ///<param name="storageDirectory">A path including the file name that the content is stored at. 
        /// An example would be "c:\\temp\\output\\file.xmi". 
        /// </param>        
        private static void CacheSingleResourceLocally(string downloadUri, string storageDirectory, string resourceFile)
        {
            string localResourcePath = storageDirectory + resourceFile;

            if (!(File.Exists(localResourcePath)))
            {
                using (StreamWriter writer = File.CreateText(localResourcePath))
                {
                    writer.Write(RetrieveContentFromUri(downloadUri + resourceFile));
                }
            }
        }

        #endregion
    }
}