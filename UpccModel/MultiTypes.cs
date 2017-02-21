using System.Collections.Generic;
using System.Reflection;

namespace Upcc
{
    internal class MultiTypes
    {
        /// <summary>
        /// Abstract base class for PRIM, ENUM and IDSCHEME.
        /// </summary>
        internal readonly MetaMultiType BasicType;

        /// <summary>
        /// Abstract base class for ABIE and MA.
        /// </summary>
        internal readonly MetaMultiType BieAggregator;

        internal MultiTypes(Classes classes, DataTypes dataTypes, Enumerations enumerations)
        {
            BasicType = new MetaMultiType
                        {
                            Name = "BasicType",
                            Classifiers = new MetaClassifier[]
                                          {
                                              dataTypes.Prim,
                                              dataTypes.IdScheme,
                                              enumerations.Enum,
                                          },
                        };
            BieAggregator = new MetaMultiType
                            {
                                Name = "BieAggregator",
                                Classifiers = new MetaClassifier[]
                                              {
                                                  classes.Abie,
                                                  classes.Ma,
                                              },
                            };
        }

        internal IEnumerable<MetaMultiType> All
        {
            get
            {
                foreach (FieldInfo field in GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
                {
                    yield return (MetaMultiType) field.GetValue(this);
                }
            }
        }
    }
}