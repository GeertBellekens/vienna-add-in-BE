using System.Collections.Generic;

namespace Upcc
{
    internal class AttributeDependencies
    {
        internal AttributeDependencies(Attributes attributes)
        {
            All = new[]
                  {
                      new MetaAttributeDependency
                      {
                          Stereotype = MetaStereotype.basedOn,
                          SourceAttribute = attributes.Bbie,
                          TargetAttribute = attributes.Bcc,
                      },
                  };
        }

        internal IEnumerable<MetaAttributeDependency> All { get; private set; }
    }
}