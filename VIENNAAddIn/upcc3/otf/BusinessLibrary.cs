using System.Collections.Generic;
using CctsRepository;
using CctsRepository.BLibrary;

namespace VIENNAAddIn.upcc3.otf
{
	public abstract class BusinessLibrary: AbstractEAPackage, ICctsLibrary
    {
        protected BusinessLibrary(ItemId id, string name, ItemId parentId, string status, string uniqueIdentifier, string versionIdentifier, string baseUrn, string namespacePrefix, IEnumerable<string> businessTerms, IEnumerable<string> copyrights, IEnumerable<string> owners, IEnumerable<string> references) : base(id, name, parentId)
        {
            Status = status;
            UniqueIdentifier = uniqueIdentifier;
            VersionIdentifier = versionIdentifier;
            BaseURN = baseUrn;
            NamespacePrefix = namespacePrefix;
            BusinessTerms = businessTerms;
            Copyrights = copyrights;
            Owners = owners;
            References = references;
        }

		int ICctsLibrary.Id 
		{
			get { return Id.Value; }
		}
		public IEnumerable<ICctsElement> Elements 
		{
			get 
			{
				throw new System.NotImplementedException();
			}
		}
        public IBLibrary BLibrary
        {
            get { return ParentPackage as IBLibrary; }
        }

        public string Status { get; private set; }
        public string UniqueIdentifier { get; private set; }
        public string VersionIdentifier { get; private set; }
        public string BaseURN { get; private set; }
        public string NamespacePrefix { get; private set; }
        public IEnumerable<string> BusinessTerms { get; private set; }
        public IEnumerable<string> Copyrights { get; private set; }
        public IEnumerable<string> Owners { get; private set; }
        public IEnumerable<string> References { get; private set; }
    }
}