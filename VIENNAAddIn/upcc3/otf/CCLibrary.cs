using System;
using System.Collections.Generic;
using CctsRepository;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.otf
{
    public class CCLibrary : BusinessLibrary, ICcLibrary
    {
        public CCLibrary(ItemId id, string name, ItemId parentId, string status, string uniqueIdentifier, string versionIdentifier, string baseUrn, string namespacePrefix, IEnumerable<string> businessTerms, IEnumerable<string> copyrights, IEnumerable<string> owners, IEnumerable<string> references)
            : base(id, name, parentId, status, uniqueIdentifier, versionIdentifier, baseUrn, namespacePrefix, businessTerms, copyrights, owners, references)
        {
        }

        #region ICcLibrary Members

        int ICctsLibrary.Id
        {
            get { return Id.Value; }
        }

        public IEnumerable<IAcc> Accs
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        public IAcc GetAccByName(string name)
        {
            throw new NotImplementedException();
        }

        public IAcc CreateAcc(AccSpec spec)
        {
            throw new NotImplementedException();
        }

        public IAcc UpdateAcc(IAcc element, AccSpec spec)
        {
            throw new NotImplementedException();
        }

        public void RemoveAcc(IAcc acc)
        {
            throw new NotImplementedException();
        }
    }
}