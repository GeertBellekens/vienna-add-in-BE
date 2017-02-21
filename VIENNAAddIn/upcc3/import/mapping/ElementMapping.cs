using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.import.mapping
{
    public abstract class ElementMapping : AbstractMapping
    {
        protected ElementMapping(SourceItem sourceItem)
        {
            SourceItem = sourceItem;
        }

        public override IEnumerable<ElementMapping> Children
        {
            get { yield break; }
        }

        public SourceItem SourceItem { get; private set; }

        public static readonly ElementMapping NullElementMapping = new NullMapping();
        public abstract bool ResolveTypeMapping(SchemaMapping schemaMapping);
    }
}