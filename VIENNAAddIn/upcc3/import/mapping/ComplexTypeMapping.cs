using System;
using System.Collections.Generic;
using System.Linq;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using VIENNAAddInUtils;

namespace VIENNAAddIn.upcc3.import.mapping
{
    public abstract class ComplexTypeMapping : AbstractMapping
    {
        #region LibraryType enum

        public enum LibraryType
        {
            BIE,
            DOC
        }

        #endregion

        private readonly List<ElementMapping> children;
        
        protected ComplexTypeMapping(string sourceElementName, string complexTypeName, IEnumerable<ElementMapping> children)
        {
            ComplexTypeName = complexTypeName;
            SourceElementName = sourceElementName;
            this.children = new List<ElementMapping>(children);
        }
        public string SourceElementName {get; set; }

        public LibraryType Library
        {
            get { return (IsMappedToSingleACC ? LibraryType.BIE : LibraryType.DOC); }
        }

        public override IEnumerable<ElementMapping> Children
        {
            get { return children; }
        }

        public bool IsMappedToSingleACC
        {
            get { return TargetACCs.Count() == 1 && AsmaMappings.Count() == 0; }
        }

        public IEnumerable<IAcc> TargetACCs
        {
            get
            {
                var targetACCs = new List<IAcc>();
                foreach (ElementMapping child in Children)
                {
                    if (child is AttributeOrSimpleElementOrComplexElementToBccMapping)
                    {
                        IAcc acc = ((AttributeOrSimpleElementOrComplexElementToBccMapping) child).Acc;
                        if (!targetACCs.Contains(acc))
                        {
                            targetACCs.Add(acc);
                        }
                    }
                    else if (child is SplitMapping)
                    {
                        foreach (IAcc acc in ((SplitMapping)child).TargetAccs)
                        {
                            if (!targetACCs.Contains(acc))
                            {
                                targetACCs.Add(acc);
                            }
                        }
                    }
                    else if (child is ComplexElementToAsccMapping)
                    {
                        IAcc acc = ((ComplexElementToAsccMapping) child).Acc;
                        if (!targetACCs.Contains(acc))
                        {
                            targetACCs.Add(acc);
                        }
                    }
                    // else ignore child
                }
                return targetACCs;
            }
        }

        public bool IsMappedToCdt
        {
            get { return TargetCdt != null; }
        }

        private ICdt targetCdt;

        public ICdt TargetCdt
        {
            get
            {
                // if not set explicitly then determine the target-CDT 
                // through the children mappings of the current element
                if (targetCdt == null)
                {
                    ICdt tCdt = null;

                    foreach (ElementMapping child in Children)
                    {
                        if (child is AttributeOrSimpleElementToSupMapping)
                        {
                            ICdt cdt = ((AttributeOrSimpleElementToSupMapping)child).Cdt;
                            if (tCdt == null)
                            {
                                tCdt = cdt;
                            }
                            else
                            {
                                if (tCdt != cdt)
                                {
                                    throw new MappingError("Complex type mapped to more than one CDTs");
                                }
                            }
                        }
                    }

                    targetCdt = tCdt;
                }

                return targetCdt;
            }

            set
            {
                targetCdt = value;
            }
        }
       

        public string ComplexTypeName { get; private set; }

        public IEnumerable<AsmaMapping> AsmaMappings
        {
            get { return Children.FilterByType<ElementMapping, AsmaMapping>(); }
        }

        public bool IsEmpty
        {
            get
            {
                return children.Count == 0;
            }
        }

        public int NumberOfChildren
        {
            get { return children.Count; }
        }

        public List<ElementMapping> GetChildren
        {
            get { return children; }
        }

        public IEnumerable<AttributeOrSimpleElementOrComplexElementToBccMapping> BccMappings(IAcc targetACC)
        {
            foreach (AttributeOrSimpleElementOrComplexElementToBccMapping bccMapping in Children.FilterByType<ElementMapping, AttributeOrSimpleElementOrComplexElementToBccMapping>())
            {
                IAcc acc = bccMapping.Acc;
                if (targetACC.Id == acc.Id)
                {
                    yield return bccMapping;
                }
            }
        }

        public IEnumerable<SplitMapping> SplitMappings()
        {
            return Children.FilterByType<ElementMapping, SplitMapping>();
        }

        public IEnumerable<ComplexElementToAsccMapping> AsccMappings(IAcc targetACC)
        {
            foreach (ComplexElementToAsccMapping asccMapping in Children.FilterByType<ElementMapping, ComplexElementToAsccMapping>())
            {
                IAcc acc = asccMapping.Acc;
                if (targetACC.Id == acc.Id)
                {
                    yield return asccMapping;
                }
            }
        }

        protected bool ChildrenEqual(ComplexTypeMapping other)
        {
            if (other.children.Count != children.Count) return false;
            for (int i = 0; i < children.Count; i++)
            {
                if (!Equals(other.children[i], children[i])) return false;
            }
            return true;
        }

        public static readonly ComplexTypeMapping NullComplexTypeMapping = new NullComplexTypeMapping();

        public void AddChildMapping(ElementMapping childMapping)
        {
            children.Add(childMapping);
        }

        public void RemoveInvalidAsmaMappings()
        {
            List<ElementMapping> validChildren = new List<ElementMapping>();
            foreach (ElementMapping elementMapping in Children)
            {
                if (elementMapping is AsmaMapping)
                {
                    if (((AsmaMapping)elementMapping).IsValid)
                    {
                        validChildren.Add(elementMapping);
                    }
                } else
                {
                    validChildren.Add(elementMapping);
                }
            }
            children.Clear();
            children.AddRange(validChildren);
        }
    }
}