using System;
using System.Collections.Generic;
using CctsRepository.EnumLibrary;

namespace VIENNAAddIn.upcc3.otf
{
    public class ENUMLibrary : BusinessLibrary, IEnumLibrary
    {
        public ENUMLibrary(ItemId id, string name, ItemId parentId, string status, string uniqueIdentifier, string versionIdentifier, string baseUrn, string namespacePrefix, IEnumerable<string> businessTerms, IEnumerable<string> copyrights, IEnumerable<string> owners, IEnumerable<string> references)
            : base(id, name, parentId, status, uniqueIdentifier, versionIdentifier, baseUrn, namespacePrefix, businessTerms, copyrights, owners, references)
        {
        }

        #region IEnumLibrary Members

        int IEnumLibrary.Id
        {
            get { return Id.Value; }
        }

        public IEnumerable<IEnum> Enums
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        public IEnum GetEnumByName(string name)
        {
            throw new NotImplementedException();
        }

        public IEnum CreateEnum(EnumSpec spec)
        {
            throw new NotImplementedException();
        }

        public IEnum UpdateEnum(IEnum element, EnumSpec spec)
        {
            throw new NotImplementedException();
        }

        public void RemoveEnum(IEnum @enum)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IIdScheme> IdSchemes
        {
            get { throw new NotImplementedException(); }
        }

        public IIdScheme GetIdSchemeByName(string name)
        {
            throw new NotImplementedException();
        }

        public IIdScheme CreateIdScheme(IdSchemeSpec specification)
        {
            throw new NotImplementedException();
        }

        public IIdScheme UpdateIdScheme(IIdScheme idScheme, IdSchemeSpec specification)
        {
            throw new NotImplementedException();
        }

        public void RemoveIdScheme(IIdScheme idScheme)
        {
            throw new NotImplementedException();
        }
    }
}