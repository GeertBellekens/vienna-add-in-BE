using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.otf
{
    public interface IEAPackage : IEAItem
    {
        IEAPackage ParentPackage { get; set; }
        IEnumerable<IEAPackage> SubPackages { get; }
        ItemId ParentId { get; }
    }
}