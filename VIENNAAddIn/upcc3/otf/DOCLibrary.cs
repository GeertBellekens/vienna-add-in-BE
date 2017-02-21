using System;
using System.Collections.Generic;
using CctsRepository.BieLibrary;
using CctsRepository.DocLibrary;

namespace VIENNAAddIn.upcc3.otf
{
    public class DOCLibrary : BusinessLibrary, IDocLibrary
    {
        public DOCLibrary(ItemId id, string name, ItemId parentId, string status, string uniqueIdentifier, string versionIdentifier, string baseUrn, string namespacePrefix, IEnumerable<string> businessTerms, IEnumerable<string> copyrights, IEnumerable<string> owners, IEnumerable<string> references)
            : base(id, name, parentId, status, uniqueIdentifier, versionIdentifier, baseUrn, namespacePrefix, businessTerms, copyrights, owners, references)
        {
        }

        #region IDocLibrary Members

        public IAbie RootAbie
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<IAbie> NonRootAbies
        {
            get { throw new NotImplementedException(); }
        }

        public IMa DocumentRoot
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<IMa> NonRootMas
        {
            get { throw new NotImplementedException(); }
        }

        int IDocLibrary.Id
        {
            get { return Id.Value; }
        }

        public IEnumerable<IMa> Mas
        {
            get { throw new NotImplementedException(); }
        }

        public IMa GetMaByName(string name)
        {
            throw new NotImplementedException();
        }

        public IMa CreateMa(MaSpec specification)
        {
            throw new NotImplementedException();
        }

        public IMa UpdateMa(IMa ma, MaSpec specification)
        {
            throw new NotImplementedException();
        }

        public void RemoveMa(IMa ma)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IAbie> Abies
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        public IAbie GetAbieByName(string name)
        {
            throw new NotImplementedException();
        }

        public IAbie CreateAbie(AbieSpec spec)
        {
            throw new NotImplementedException();
        }

        public IAbie UpdateAbie(IAbie element, AbieSpec spec)
        {
            throw new NotImplementedException();
        }
    }
}