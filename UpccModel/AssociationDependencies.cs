using System.Collections.Generic;

namespace Upcc
{
    internal class AssociationDependencies
    {
        internal AssociationDependencies(Associations associations)
        {
            All = new[]
                  {
                      new MetaAssociationDependency()
                      {
                          Stereotype = MetaStereotype.basedOn,
                          SourceAssociation = associations.Asbie,
                          TargetAssociation = associations.Ascc,
                      },
                  };
        }

        internal IEnumerable<MetaAssociationDependency> All { get; private set; }
    }
}