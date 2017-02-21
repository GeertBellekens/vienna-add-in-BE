namespace VIENNAAddIn.upcc3.otf
{
    public abstract class AbstractEAItem : IEAItem
    {
        protected AbstractEAItem(ItemId id, string name)
        {
            Id = id;
            Name = name;
        }

        #region IEAItem Members

        public string Name { get; private set; }
        public ItemId Id { get; private set; }

        #endregion
    }
}