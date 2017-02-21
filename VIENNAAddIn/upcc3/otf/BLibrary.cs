using System;
using System.Collections.Generic;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.BLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using CctsRepository.DocLibrary;
using CctsRepository.EnumLibrary;
using CctsRepository.PrimLibrary;

namespace VIENNAAddIn.upcc3.otf
{
    public class BLibrary : BusinessLibrary, IBLibrary
    {
        public BLibrary(ItemId id, string name, ItemId parentId, string status, string uniqueIdentifier, string versionIdentifier, string baseUrn, string namespacePrefix, IEnumerable<string> businessTerms, IEnumerable<string> copyrights, IEnumerable<string> owners, IEnumerable<string> references) : base(id, name, parentId, status, uniqueIdentifier, versionIdentifier, baseUrn, namespacePrefix, businessTerms, copyrights, owners, references)
        {
        }

        int IBLibrary.Id
        {
            get { return Id.Value; }
        }

        public IBLibrary Parent
        {
            get { return BLibrary; }
        }

        #region IBLibrary Members

        public IEnumerable<IBLibrary> GetBLibraries()
        {
            throw new NotImplementedException();
        }

        public IBLibrary GetBLibraryByName(string name)
        {
            throw new NotImplementedException();
        }

        public IBLibrary CreateBLibrary(BLibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        public IBLibrary UpdateBLibrary(IBLibrary bLibrary, BLibrarySpec specification)
        {
            throw new NotImplementedException();
        }

        public void RemoveBLibrary(IBLibrary bLibrary)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IPrimLibrary> GetPrimLibraries()
        {
            throw new NotImplementedException();
        }

        public IPrimLibrary GetPrimLibraryByName(string name)
        {
            throw new NotImplementedException();
        }

        public IPrimLibrary CreatePrimLibrary(PrimLibrarySpec specification)
        {
            throw new NotImplementedException();
        }

        public IPrimLibrary UpdatePrimLibrary(IPrimLibrary primLibrary, PrimLibrarySpec specification)
        {
            throw new NotImplementedException();
        }

        public void RemovePrimLibrary(IPrimLibrary primLibrary)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IEnumLibrary> GetEnumLibraries()
        {
            throw new NotImplementedException();
        }

        public IEnumLibrary GetEnumLibraryByName(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumLibrary CreateEnumLibrary(EnumLibrarySpec specification)
        {
            throw new NotImplementedException();
        }

        public IEnumLibrary UpdateEnumLibrary(IEnumLibrary enumLibrary, EnumLibrarySpec specification)
        {
            throw new NotImplementedException();
        }

        public void RemoveEnumLibrary(IEnumLibrary enumLibrary)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ICdtLibrary> GetCdtLibraries()
        {
            throw new NotImplementedException();
        }

        public ICdtLibrary GetCdtLibraryByName(string name)
        {
            throw new NotImplementedException();
        }

        public ICdtLibrary CreateCdtLibrary(CdtLibrarySpec specification)
        {
            throw new NotImplementedException();
        }

        public ICdtLibrary UpdateCdtLibrary(ICdtLibrary cdtLibrary, CdtLibrarySpec specification)
        {
            throw new NotImplementedException();
        }

        public void RemoveCdtLibrary(ICdtLibrary cdtLibrary)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ICcLibrary> GetCcLibraries()
        {
            throw new NotImplementedException();
        }

        public ICcLibrary GetCcLibraryByName(string name)
        {
            throw new NotImplementedException();
        }

        public ICcLibrary CreateCcLibrary(CcLibrarySpec specification)
        {
            throw new NotImplementedException();
        }

        public ICcLibrary UpdateCcLibrary(ICcLibrary ccLibrary, CcLibrarySpec specification)
        {
            throw new NotImplementedException();
        }

        public void RemoveCcLibrary(ICcLibrary ccLibrary)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IBdtLibrary> GetBdtLibraries()
        {
            throw new NotImplementedException();
        }

        public IBdtLibrary GetBdtLibraryByName(string name)
        {
            throw new NotImplementedException();
        }

        public IBdtLibrary CreateBdtLibrary(BdtLibrarySpec specification)
        {
            throw new NotImplementedException();
        }

        public IBdtLibrary UpdateBdtLibrary(IBdtLibrary bdtLibrary, BdtLibrarySpec specification)
        {
            throw new NotImplementedException();
        }

        public void RemoveBdtLibrary(IBdtLibrary bdtLibrary)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IBieLibrary> GetBieLibraries()
        {
            throw new NotImplementedException();
        }

        public IBieLibrary GetBieLibraryByName(string name)
        {
            throw new NotImplementedException();
        }

        public IBieLibrary CreateBieLibrary(BieLibrarySpec specification)
        {
            throw new NotImplementedException();
        }

        public IBieLibrary UpdateBieLibrary(IBieLibrary bieLibrary, BieLibrarySpec specification)
        {
            throw new NotImplementedException();
        }

        public void RemoveBieLibrary(IBieLibrary bieLibrary)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IDocLibrary> GetDocLibraries()
        {
            throw new NotImplementedException();
        }

        public IDocLibrary GetDocLibraryByName(string name)
        {
            throw new NotImplementedException();
        }

        public IDocLibrary CreateDocLibrary(DocLibrarySpec specification)
        {
            throw new NotImplementedException();
        }

        public IDocLibrary UpdateDocLibrary(IDocLibrary docLibrary, DocLibrarySpec specification)
        {
            throw new NotImplementedException();
        }

        public void RemoveDocLibrary(IDocLibrary docLibrary)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}