using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.otf
{
    public abstract class AbstractEAPackage : AbstractEAItem, IEAPackage
    {
        protected readonly List<IEAElement> elements = new List<IEAElement>();
        protected readonly List<IEAPackage> subPackages = new List<IEAPackage>();

        #region IEAPackage Members

        protected AbstractEAPackage(ItemId id, string name, ItemId parentId) : base(id, name)
        {
            ParentId = parentId;
        }

        public IEnumerable<IEAPackage> SubPackages
        {
            get { return subPackages; }
        }

        public ItemId ParentId { get; private set; }

        public void AddSubPackage(IEAPackage package)
        {
            subPackages.Add(package);
            package.ParentPackage = this;
        }

        public IEAPackage ParentPackage { get; set; }

        #endregion
    }
}