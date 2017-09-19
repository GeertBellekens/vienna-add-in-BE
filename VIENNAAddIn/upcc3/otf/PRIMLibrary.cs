using System;
using System.Collections.Generic;
using CctsRepository;
using CctsRepository.PrimLibrary;

namespace VIENNAAddIn.upcc3.otf
{
    public class PRIMLibrary : BusinessLibrary, IPrimLibrary
    {
        public PRIMLibrary(ItemId id, string name, ItemId parentId, string status, string uniqueIdentifier, string versionIdentifier, string baseUrn, string namespacePrefix, IEnumerable<string> businessTerms, IEnumerable<string> copyrights, IEnumerable<string> owners, IEnumerable<string> references) : base(id, name, parentId, status, uniqueIdentifier, versionIdentifier, baseUrn, namespacePrefix, businessTerms, copyrights, owners, references)
        {
        }

        #region IPrimLibrary Members

        int ICctsLibrary.Id
        {
            get { return Id.Value; }
        }

        public IEnumerable<IPrim> Prims
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        public IPrim GetPrimByName(string name)
        {
            throw new NotImplementedException();
        }

        public IPrim CreatePrim(PrimSpec spec)
        {
            throw new NotImplementedException();
        }

        public IPrim UpdatePrim(IPrim element, PrimSpec spec)
        {
            throw new NotImplementedException();
        }

        public void RemovePrim(IPrim prim)
        {
            throw new NotImplementedException();
        }
    }
}