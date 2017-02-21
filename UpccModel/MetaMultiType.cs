using System.Collections.Generic;
using System.Linq;

namespace Upcc
{
    /// <summary>
    /// Meta multi-types are used in places where an attribute's/connector's type/target can be of more than one type.
    /// 
    /// The multi-type should provide boolean properties for determining the concrete type, as well as properties returning the concrete type.
    /// 
    /// Furthermore, the multi-type can provide properties for all tagged values shared by the individual types.
    /// </summary>
    public class MetaMultiType : MetaClassifier
    {
        public MetaClassifier[] Classifiers { get; internal set; }

        /// <summary>
        /// Tagged values shared by all classifiers (the intersection of the sets of tagged values of all classifiers).
        /// </summary>
        public MetaTaggedValue[] CommonTaggedValues
        {
            get
            {
                var commonTaggedValues = new HashSet<MetaTaggedValue>();
                if (Classifiers.Length > 0)
                {
                    commonTaggedValues.UnionWith(Classifiers[0].TaggedValues);
                }
                for (int i = 1; i < Classifiers.Length; ++i )
                {
                    commonTaggedValues.IntersectWith(Classifiers[i].TaggedValues);
                }
                return commonTaggedValues.ToArray();
            }
        }

        public bool HasCommonTaggedValues
        {
            get { return CommonTaggedValues != null && CommonTaggedValues.Length > 0; }
        }
    }
}