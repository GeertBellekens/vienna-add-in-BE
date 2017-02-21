using System;
using System.Collections.Generic;
using CctsRepository.CdtLibrary;

namespace VIENNAAddIn.upcc3.otf
{
    public class CDTLibrary : BusinessLibrary, ICdtLibrary
    {
        public CDTLibrary(ItemId id, string name, ItemId parentId, string status, string uniqueIdentifier, string versionIdentifier, string baseUrn, string namespacePrefix, IEnumerable<string> businessTerms, IEnumerable<string> copyrights, IEnumerable<string> owners, IEnumerable<string> references)
            : base(id, name, parentId, status, uniqueIdentifier, versionIdentifier, baseUrn, namespacePrefix, businessTerms, copyrights, owners, references)
        {
        }

        #region ICdtLibrary Members

        int ICdtLibrary.Id
        {
            get { return Id.Value; }
        }

        public IEnumerable<ICdt> Cdts
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        public ICdt GetCdtByName(string name)
        {
            throw new NotImplementedException();
        }

        public ICdt CreateCdt(CdtSpec spec)
        {
            throw new NotImplementedException();
        }

        public ICdt UpdateCdt(ICdt element, CdtSpec spec)
        {
            throw new NotImplementedException();
        }

        public void RemoveCdt(ICdt cdt)
        {
            throw new NotImplementedException();
        }
    }
}