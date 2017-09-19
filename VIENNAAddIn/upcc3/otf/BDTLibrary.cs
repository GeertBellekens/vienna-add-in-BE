using System;
using System.Collections.Generic;
using CctsRepository;
using CctsRepository.BdtLibrary;


namespace VIENNAAddIn.upcc3.otf
{
    public class BDTLibrary : BusinessLibrary, IBdtLibrary
    {
        public BDTLibrary(ItemId id, string name, ItemId parentId, string status, string uniqueIdentifier, string versionIdentifier, string baseUrn, string namespacePrefix, IEnumerable<string> businessTerms, IEnumerable<string> copyrights, IEnumerable<string> owners, IEnumerable<string> references)
            : base(id, name, parentId, status, uniqueIdentifier, versionIdentifier, baseUrn, namespacePrefix, businessTerms, copyrights, owners, references)
        {
        }

        #region IBdtLibrary Members

        public IEnumerable<IBdt> Bdts
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        public IBdt GetBdtByName(string name)
        {
            throw new NotImplementedException();
        }

        public IBdt CreateBdt(BdtSpec spec)
        {
            throw new NotImplementedException();
        }

        public IBdt UpdateBdt(IBdt element, BdtSpec spec)
        {
            throw new NotImplementedException();
        }

        public void RemoveBdt(IBdt bdt)
        {
            throw new NotImplementedException();
        }
    }
}