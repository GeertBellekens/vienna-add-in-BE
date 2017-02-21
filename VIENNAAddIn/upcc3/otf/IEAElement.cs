namespace VIENNAAddIn.upcc3.otf
{
    public interface IEAElement : IEAItem
    {
        ItemId PackageId { get; }
        IEAPackage Package { get; set; }
    }
}