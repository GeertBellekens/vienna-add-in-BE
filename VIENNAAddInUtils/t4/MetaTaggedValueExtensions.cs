using Upcc;

namespace VIENNAAddInUtils.t4
{
    public static class MetaTaggedValueExtensions
    {
        public static string AsPropertyName(this MetaTaggedValue metaTaggedValue)
        {
            string propertyName = metaTaggedValue.Name.FirstCharToUpperCase();
            return metaTaggedValue.Cardinality == MetaCardinality.Many ? propertyName.Plural() : propertyName;
        }
    }
}