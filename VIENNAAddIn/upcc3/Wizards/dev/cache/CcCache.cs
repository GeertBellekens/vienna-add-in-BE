// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System;
using System.Collections.Generic;
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using CctsRepository.DocLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.cache
{
    public class CcCache
    {
        private static CcCache ClassInstance;
        private readonly ICctsRepository mCctsRepository;
        private readonly List<CacheItemBdtLibrary> mBdtLibraries;
        private readonly List<CacheItemBieLibrary> mBieLibraries;
        private readonly List<CacheItemCcLibrary> mCcLibraries;
        private readonly List<CacheItemCdtLibrary> mCdtLibraries;
        private readonly List<CacheItemDocLibrary> mDocLibraries;

        private bool librariesLoaded;

        /// <summary>
        /// Saves Representations of EA Elements in lists and implements "Lazy Loading" from repository
        /// </summary>
        /// <param name="cctsRepository">The Repository where Elements are stored in.</param>
        private CcCache(ICctsRepository cctsRepository)
        {
            mCdtLibraries = new List<CacheItemCdtLibrary>();
            mCcLibraries = new List<CacheItemCcLibrary>();
            mBdtLibraries = new List<CacheItemBdtLibrary>();
            mBieLibraries = new List<CacheItemBieLibrary>();
            mDocLibraries = new List<CacheItemDocLibrary>();
            librariesLoaded = false;
            mCctsRepository = cctsRepository;
        }

        /// <summary>
        /// Create a new Instance of CcCache automatically
        /// </summary>
        /// <param name="cctsRepository">The repository where Elements are stored in.</param>
        /// <returns>An instance of CcCache.</returns>
        public static CcCache GetInstance(ICctsRepository cctsRepository)
        {
            if (ClassInstance == null || ClassInstance.mCctsRepository != cctsRepository)
            {
                ClassInstance = new CcCache(cctsRepository);
            }
            return ClassInstance;
        }

        public static CcCache GetInstance()
        {
            if (ClassInstance == null)
            {
                throw new Exception("Can't return instantiate Cache since Ccts Repository is not set. The method GetInstance(ICctsRepository cctsRepository) needs to be called at least one. ");
            }

            return ClassInstance;
        }

        /// <summary>
        /// Tries to load CC Libraries from cache, if not present load it from repository.
        /// </summary>
        /// <returns>A list of CC Libraries in the repository.</returns>
        public List<ICcLibrary> GetCcLibraries()
        {
            LoadLibraries();
            return mCcLibraries.ConvertAll(new Converter<CacheItemCcLibrary, ICcLibrary>(CacheItemCCLibraryToCCLibrary));
        }

        private void LoadLibraries()
        {
            if (!librariesLoaded)
            {
                librariesLoaded = true;
                foreach (object library in mCctsRepository.GetAllLibraries())
                {
                    if (library is ICcLibrary)
                    {
                        mCcLibraries.Add(new CacheItemCcLibrary((ICcLibrary) library));
                    }
                    else if (library is IBdtLibrary)
                    {
                        mBdtLibraries.Add(new CacheItemBdtLibrary((IBdtLibrary) library));
                    }
                    else if (library is IBieLibrary)
                    {
                        mBieLibraries.Add(new CacheItemBieLibrary((IBieLibrary) library));
                    }
                    else if (library is ICdtLibrary)
                    {
                        mCdtLibraries.Add(new CacheItemCdtLibrary((ICdtLibrary) library));
                    }
                    else if (library is IDocLibrary)
                    {
                        mDocLibraries.Add(new CacheItemDocLibrary((IDocLibrary) library));
                    }
                }
            }
        }

        /// <summary>
        /// Tries to load CDT Libraries from cache, if not present load it from repository.
        /// </summary>
        /// <returns>A list of CDT Libraries in the repository.</returns>
        public List<ICdtLibrary> GetCdtLibraries()
        {
            LoadLibraries();
            return
                mCdtLibraries.ConvertAll(new Converter<CacheItemCdtLibrary, ICdtLibrary>(CacheItemCDTLibraryToCDTLibrary));
        }

        /// <summary>
        /// Tries to load BDT Libraries from cache, if not present load it from repository.
        /// </summary>
        /// <returns>A list of BDT Libraries in the repository.</returns>
        public List<IBdtLibrary> GetBdtLibraries()
        {
            LoadLibraries();
            return
                mBdtLibraries.ConvertAll(new Converter<CacheItemBdtLibrary, IBdtLibrary>(CacheItemBDTLibraryToBDTLibrary));
        }

        /// <summary>
        /// Tries to load BIE Libraries from cache, if not present load it from repository.
        /// </summary>
        /// <returns>A list of BIE Libraries in the repository.</returns>
        public List<IBieLibrary> GetBieLibraries()
        {
            LoadLibraries();
            return
                mBieLibraries.ConvertAll(new Converter<CacheItemBieLibrary, IBieLibrary>(CacheItemBIELibraryToBIELibrary));
        }

        /// <summary>
        /// Tries to load DOC Libraries from cache, if not present load it from repository.
        /// </summary>
        /// <returns>A list of BIE Libraries in the repository.</returns>
        public List<IDocLibrary> GetDocLibraries()
        {
            LoadLibraries();
            return
                mDocLibraries.ConvertAll(new Converter<CacheItemDocLibrary, IDocLibrary>(CacheItemDocLibraryToDocLibrary));
        }


        /// <summary>
        /// Tries to get CDTs from Library <paramref name="cdtLibraryName"/>. If there are no CDT Libraries in Cache, repository Loading is triggered.
        /// </summary>
        /// <param name="cdtLibraryName">The name of the CDT Library CDTs should be retrieved from.</param>
        /// <returns>A list of CDTs.</returns>
        public List<ICdt> GetCdtsFromCdtLibrary(string cdtLibraryName)
        {
            LoadLibraries();
            foreach (CacheItemCdtLibrary cdtLibrary in mCdtLibraries)
            {
                if (cdtLibrary.CdtLibrary.Name.Equals(cdtLibraryName))
                {
                    if (cdtLibrary.CdtsInLibrary == null)
                    {
                        cdtLibrary.CdtsInLibrary = new List<ICdt>(cdtLibrary.CdtLibrary.Cdts);
                    }
                    return cdtLibrary.CdtsInLibrary;
                }
            }
            throw new Exception("No corresponding CDT Library found with name '" + cdtLibraryName + "'.");
        }

        /// <summary>
        /// Tries to retrieve a CDT with <paramref name="cdtName"/> from CDT Library with Name <paramref name="cdtLibraryName"/>.
        /// This method uses GetCDTsFromCDTLibrary <seealso cref="GetCdtsFromCdtLibrary"/> for retrieving available CDTs.
        /// If CDT or CDT Library cannot be found an exception will be thrown.
        /// </summary>
        /// <param name="cdtLibraryName">The name of the CDT Library where the CDT should be retrieved from.</param>
        /// <param name="cdtName">The name of the CDT which should be retrieved.</param>
        /// <returns>The corresponding UPCC3 CDT Element.</returns>
        public ICdt GetCdtFromCdtLibrary(string cdtLibraryName, string cdtName)
        {
            List<ICdt> tmpCDTs = GetCdtsFromCdtLibrary(cdtLibraryName);
            foreach (ICdt cdt in tmpCDTs)
            {
                if (cdt.Name.Equals(cdtName))
                {
                    return cdt;
                }
            }
            throw new Exception("No corresponding CDT with Name '" + cdtName + "' found in CDT Library '" +
                                cdtLibraryName + "'.");
        }

        /// <summary>
        /// Tries to get ACCs from Library <paramref name="ccLibraryName"/>. If there are no CC Libraries in Cache, repository loading is triggered.
        /// </summary>
        /// <param name="ccLibraryName">The name of the CC Library ACCs should be retrieved from.</param>
        /// <returns>A list of ACCs.</returns>
        public List<IAcc> GetCcsFromCcLibrary(string ccLibraryName)
        {
            LoadLibraries();
            foreach (CacheItemCcLibrary ccLibrary in mCcLibraries)
            {
                if (ccLibrary.CcLibrary.Name.Equals(ccLibraryName))
                {
                    if (ccLibrary.CcsInLibrary == null)
                    {
                        ccLibrary.CcsInLibrary = new List<IAcc>(ccLibrary.CcLibrary.Accs);
                    }
                    return ccLibrary.CcsInLibrary;
                }
            }
            throw new Exception("No corresponding CC Library found with name '" + ccLibraryName + "'.");
        }

        /// <summary>
        /// Try to retrieve a specific DOC Library by Name. If not found an exception will be thrown.
        /// </summary>
        /// <param name="docLibraryName">The Name of the DOC Library to retrieve.</param>
        /// <returns>A UPPC3 DOC Library Element.</returns>
        public IDocLibrary GetDocLibraryByName(string docLibraryName)
        {
            LoadLibraries();
            foreach (CacheItemDocLibrary docLibrary in mDocLibraries)
            {
                if (docLibrary.DocLibrary.Name.Equals(docLibraryName))
                {
                    return docLibrary.DocLibrary;
                }
            }
            throw new Exception("No corresponding DOC Library found with name '" + docLibraryName + "'.");
        }
        /// <summary>
        /// Tries to get MAs from Library <paramref name="docLibraryName"/>. If there are no DOC Libraries in Cache, repository loading is triggered.
        /// </summary>
        /// <param name="docLibraryName">The name of the Doc Library MAs should be retrieved from.</param>
        /// <returns>A list of MAs.</returns>
        public List<IMa> GetMasFromDocLibrary(string docLibraryName)
        {
            LoadLibraries();
            foreach (CacheItemDocLibrary docLibrary in mDocLibraries)
            {
                if (docLibrary.DocLibrary.Name.Equals(docLibraryName))
                {
                    if (docLibrary.MasInLibrary == null)
                    {
                        docLibrary.MasInLibrary = new List<IMa>(docLibrary.DocLibrary.Mas);
                    }
                    return docLibrary.MasInLibrary;
                }
            }
            throw new Exception("No corresponding DOC Library found with name '" + docLibraryName + "'.");
        }

        /// <summary>
        /// Tries to retrieve a CDT with <paramref name="ccName"/> from CC Library with name <paramref name="ccLibraryName"/>.
        /// This method uses GetCCsFromCCLibrary <seealso cref="GetCcsFromCcLibrary"/> for retrieving available ACCs.
        /// If ACC or CC Library cannot be found an exception will be thrown.
        /// </summary>
        /// <param name="ccLibraryName"></param>
        /// <param name="ccName"></param>
        /// <returns></returns>
        public IAcc GetCcFromCcLibrary(string ccLibraryName, string ccName)
        {
            List<IAcc> tmpCCs = GetCcsFromCcLibrary(ccLibraryName);
            foreach (IAcc acc in tmpCCs)
            {
                if (acc.Name.Equals(ccName))
                {
                    return acc;
                }
            }
            throw new Exception("No corresponding ACC with Name '" + ccName + "' found in CC Library '" + ccLibraryName +
                                "'.");
        }

        /// <summary>
        /// Try to retrieve a specific BDT Library by Name. If not found an exception will be thrown.
        /// </summary>
        /// <param name="bdtLibraryName">The Name of the BDT Library to retrieve.</param>
        /// <returns>A UPPC3 BDT Library Element.</returns>
        public IBdtLibrary GetBdtLibraryByName(string bdtLibraryName)
        {
            LoadLibraries();
            foreach (CacheItemBdtLibrary bdtLibrary in mBdtLibraries)
            {
                if (bdtLibrary.BdtLibrary.Name.Equals(bdtLibraryName))
                {
                    return bdtLibrary.BdtLibrary;
                }
            }
            throw new Exception("No corresponding BDT Library found with name '" + bdtLibraryName + "'.");
        }

        public List<IBdt> GetBdtsFromBdtLibrary(string bdtLibraryName)
        {
            LoadLibraries();
            foreach (CacheItemBdtLibrary bdtLibrary in mBdtLibraries)
            {
                if (bdtLibrary.BdtLibrary.Name.Equals(bdtLibraryName))
                {
                    if (bdtLibrary.BdtsInLibrary == null)
                    {                        
                        bdtLibrary.BdtsInLibrary = new List<IBdt>(bdtLibrary.BdtLibrary.Bdts);
                    }

                    return bdtLibrary.BdtsInLibrary;
                }                
            }

            throw new Exception("No corresponding BDT Library found with name '" + bdtLibraryName + "'.");
        }

        /// <summary>
        /// Try to retrieve a specific BIE Library by Name. If not found an exception will be thrown.
        /// </summary>
        /// <param name="bieLibraryName">The Name of the BIE Library to retrieve.</param>
        /// <returns>A UPCC3 BIE Library Element.</returns>
        public IBieLibrary GetBieLibraryByName(string bieLibraryName)
        {
            LoadLibraries();
            foreach (CacheItemBieLibrary bieLibrary in mBieLibraries)
            {
                if (bieLibrary.BieLibrary.Name.Equals(bieLibraryName))
                {
                    return bieLibrary.BieLibrary;
                }
            }
            throw new Exception("No corresponding BIE  Library found with name '" + bieLibraryName + "'.");
        }

        public List<IAbie> GetBiesFromBieLibrary(string bieLibraryName)
        {
            LoadLibraries();
            foreach (CacheItemBieLibrary bieLibrary in mBieLibraries)
            {
                if (bieLibrary.BieLibrary.Name.Equals(bieLibraryName))
                {
                    if (bieLibrary.AbiesInLibrary == null)
                    {
                        bieLibrary.AbiesInLibrary = new List<IAbie>(bieLibrary.BieLibrary.Abies);   
                    }

                    return bieLibrary.AbiesInLibrary;
                }                
            }

            throw new Exception("No corresponding BIE Library found with name '" + bieLibraryName + "'.");
        }

        public void Refresh()
        {
            librariesLoaded = false;
            mCdtLibraries.Clear();
            mBdtLibraries.Clear();
            mBieLibraries.Clear();
            mCcLibraries.Clear();
        }


        #region Converters used for ConvertAll

        /// <summary>
        /// Static function to convert a cacheItemCDTLibrary to a CDTLibrary.
        /// </summary>
        /// <param name="cacheItemCDTLibrary">A Cache representation of a CDT Library</param>
        /// <returns>A UPCC3 CDT Library Element.</returns>
        private static ICdtLibrary CacheItemCDTLibraryToCDTLibrary(CacheItemCdtLibrary cacheItemCDTLibrary)
        {
            return cacheItemCDTLibrary.CdtLibrary;
        }

        /// <summary>
        /// Static function to convert a cacheItemCCLibrary to a CCLibrary.
        /// </summary>
        /// <param name="cacheItemCCLibrary">A Cache representation of a CC Library</param>
        /// <returns>A UPCC3 CC Library Element.</returns>
        private static ICcLibrary CacheItemCCLibraryToCCLibrary(CacheItemCcLibrary cacheItemCCLibrary)
        {
            return cacheItemCCLibrary.CcLibrary;
        }

        /// <summary>
        /// Static function to convert a cacheItemDocLibrary to a DocLibrary.
        /// </summary>
        /// <param name="cacheItemDocLibrary">A Cache representation of a CC Library</param>
        /// <returns>A UPCC3 CC Library Element.</returns>
        private IDocLibrary CacheItemDocLibraryToDocLibrary(CacheItemDocLibrary cacheItemDocLibrary)
        {
            return cacheItemDocLibrary.DocLibrary;
        }

        /// <summary>
        /// Static function to convert a cacheItemBDTLibrary to a BDTLibrary.
        /// </summary>
        /// <param name="cacheItemBDTLibrary">A Cache representation of a BDT Library</param>
        /// <returns>A UPCC3 BDT Library Element.</returns>
        private static IBdtLibrary CacheItemBDTLibraryToBDTLibrary(CacheItemBdtLibrary cacheItemBDTLibrary)
        {
            return cacheItemBDTLibrary.BdtLibrary;
        }

        /// <summary>
        /// Static function to convert a cacheItemBIELibrary to a BIELibrary.
        /// </summary>
        /// <param name="cacheItemBIELibrary">A Cache representation of a BIE Library</param>
        /// <returns>A UPCC3 BIE Library Element.</returns>
        private static IBieLibrary CacheItemBIELibraryToBIELibrary(CacheItemBieLibrary cacheItemBIELibrary)
        {
            return cacheItemBIELibrary.BieLibrary;
        }

        #endregion
    }
}