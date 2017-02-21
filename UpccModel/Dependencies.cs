using System.Collections.Generic;

namespace Upcc
{
    internal class Dependencies
    {
        internal Dependencies(Classes classes, DataTypes dataTypes, Enumerations enumerations)
        {
            All = new[]
                  {
                      new MetaDependency
                      {
                          Stereotype = MetaStereotype.isEquivalentTo,
                          SourceClassifierType = dataTypes.Prim,
                          TargetClassifierType = dataTypes.Prim,
                      },
                      new MetaDependency
                      {
                          Stereotype = MetaStereotype.isEquivalentTo,
                          SourceClassifierType = enumerations.Enum,
                          TargetClassifierType = enumerations.Enum,
                      },
                      new MetaDependency
                      {
                          Stereotype = MetaStereotype.isEquivalentTo,
                          SourceClassifierType = classes.Cdt,
                          TargetClassifierType = classes.Cdt,
                      },
                      new MetaDependency
                      {
                          Stereotype = MetaStereotype.isEquivalentTo,
                          SourceClassifierType = classes.Acc,
                          TargetClassifierType = classes.Acc,
                      },
                      new MetaDependency
                      {
                          Stereotype = MetaStereotype.isEquivalentTo,
                          SourceClassifierType = classes.Bdt,
                          TargetClassifierType = classes.Bdt,
                      },
                      new MetaDependency
                      {
                          Stereotype = MetaStereotype.isEquivalentTo,
                          SourceClassifierType = classes.Abie,
                          TargetClassifierType = classes.Abie,
                      },
                      new MetaDependency
                      {
                          Stereotype = MetaStereotype.basedOn,
                          SourceClassifierType = classes.Bdt,
                          TargetClassifierType = classes.Cdt,
                      },
                      new MetaDependency
                      {
                          Stereotype = MetaStereotype.basedOn,
                          SourceClassifierType = classes.Abie,
                          TargetClassifierType = classes.Acc,
                      },
                  };
        }

        internal IEnumerable<MetaDependency> All { get; private set; }
    }
}