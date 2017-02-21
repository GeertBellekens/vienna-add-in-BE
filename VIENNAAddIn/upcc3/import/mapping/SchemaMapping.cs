using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using CctsRepository;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using VIENNAAddInUtils;
using VIENNAAddIn.Utils;

namespace VIENNAAddIn.upcc3.import.mapping
{
    public class SchemaMapping
    {
        private readonly Dictionary<string, List<ComplexTypeMapping>> complexTypeMappings = new Dictionary<string, List<ComplexTypeMapping>>();
        private readonly List<SimpleTypeToCdtMapping> simpleTypeMappings = new List<SimpleTypeToCdtMapping>();
        private readonly MapForceSourceItemTree sourceItemStore;
        private readonly TargetElementStore targetElementStore;
        private readonly MappingFunctionStore mappingFunctionStore;
        private readonly List<ElementMapping> elementMappings = new List<ElementMapping>();

        public SchemaMapping(MapForceMapping mapForceMapping, XmlSchemaSet xmlSchemaSet, ICcLibrary ccLibrary, ICctsRepository cctsRepository)
        {
            sourceItemStore = new MapForceSourceItemTree(mapForceMapping, xmlSchemaSet);

            targetElementStore = new TargetElementStore(mapForceMapping, ccLibrary, cctsRepository);
            
            mappingFunctionStore = new MappingFunctionStore(mapForceMapping, targetElementStore);

            RootElementMapping = MapElement(sourceItemStore.RootSourceItem, "/" + sourceItemStore.RootSourceItem.Name, new Stack<XmlQualifiedName>());

            elementMappings.Add(RootElementMapping);

            elementMappings = new List<ElementMapping>(ResolveTypeMappings(elementMappings));

            foreach (KeyValuePair<string, List<ComplexTypeMapping>> pair in complexTypeMappings)
            {
                foreach (ComplexTypeMapping complexTypeMapping in pair.Value)
                {
                    complexTypeMapping.RemoveInvalidAsmaMappings();
                }
            }

            Dictionary<string, List<ComplexTypeMapping>> relevantComplexTypeMappings = new Dictionary<string, List<ComplexTypeMapping>>();
            foreach (KeyValuePair<string, List<ComplexTypeMapping>> pair in complexTypeMappings)
            {
                string complexTypeName = pair.Key;
                ComplexTypeMapping relevantMapping = GetComplexTypeMappingWithMostChildren(pair.Value);
                if (relevantMapping is ComplexTypeToMaMapping)
                {
                    relevantMapping = CreateComplexTypeMappingForChildMappings(relevantMapping.Children, complexTypeName,relevantMapping.SourceElementName);
                }
                if (relevantMapping != null)
                {
                    relevantComplexTypeMappings[complexTypeName] = new List<ComplexTypeMapping> {relevantMapping};
                }
            }
            complexTypeMappings = relevantComplexTypeMappings;

            foreach (ElementMapping elementMapping in elementMappings)
            {
                if (elementMapping is AsmaMapping)
                {
                    AsmaMapping asmaMapping = (AsmaMapping) elementMapping;
                    ComplexTypeMapping complexTypeMapping = CreateComplexTypeMappingForChildMappings(asmaMapping.TargetMapping.Children, asmaMapping.TargetMapping.ComplexTypeName, asmaMapping.TargetMapping.SourceElementName);
                    asmaMapping.TargetMapping = complexTypeMapping;
                }
            }

            foreach (KeyValuePair<string, List<ComplexTypeMapping>> pair in complexTypeMappings)
            {
                foreach (ComplexTypeMapping complexTypeMapping in pair.Value)
                {
                    complexTypeMapping.RemoveInvalidAsmaMappings();
                }
            }

            
            // The following lines of code were used for the CEC 2010 paper evaluation.

            //Console.Out.WriteLine("Kennzahl 3 (implizite Mappings): haendisch");
            //Console.Out.WriteLine("Kennzahl 4 (Anzahl der gemappten Elemente)): " + (elementMappings.Count + simpleTypeMappings.Count + complexTypeMappings.Count));

            //Console.Out.WriteLine("Importer Kennzahl 3a (anhand elementMapping variable): " + elementMappings.Count);
            //Console.Out.WriteLine("Importer Kennzahl 3b (anhand simpleTypeMappings): " + simpleTypeMappings.Count);
            //Console.Out.WriteLine("Importer Kennzahl 3c (anhand complexTypeMappings): " + complexTypeMappings.Count);            
        }

        private IEnumerable<ElementMapping> ResolveTypeMappings(IEnumerable<ElementMapping> unresolvedElementMappings)
        {
            foreach (ElementMapping elementMapping in unresolvedElementMappings)
            {
                if (elementMapping.ResolveTypeMapping(this))
                {
                    yield return elementMapping;
                }
            }
        }

        public ElementMapping RootElementMapping { get; private set; }

        public HashSet<string> EdgeDescriptions
        {
            get
            {
                HashSet<string> edgeDescriptions = new HashSet<string>();
                foreach (SourceItem mappedSourceItem in sourceItemStore.MappedSourceItems)
                {
                    object targetCc = targetElementStore.GetTargetCc(mappedSourceItem.MappingTargetKey);

                    string targetElementName = null;

                    if (targetCc is IBcc)
                    {
                        targetElementName = ((IBcc) targetCc).Name;
                    }
                    else if (targetCc is ICdtSup)
                    {
                        targetElementName = ((ICdtSup)targetCc).Name;
                    }
                    else if (targetCc is IAscc)
                    {
                        targetElementName = ((IAscc)targetCc).Name;
                    }
                    
                    if (targetElementName != null)
                    {
                        edgeDescriptions.Add(mappedSourceItem.Path + " -> " + targetElementName);    
                    }
                    else
                    {
                        MappingFunction mappingFunction = mappingFunctionStore.GetMappingFunction(mappedSourceItem.MappingTargetKey);

                        if ((mappingFunction != null) && (mappingFunction.IsSplit))
                        {
                            edgeDescriptions.Add(mappedSourceItem.Path + " -> SPLIT Function");
                        }
                        else
                        {
                            edgeDescriptions.Add(mappedSourceItem.Path + " -> ??? that would be interesting");
                        }
                    }
                    
                }
                return edgeDescriptions;
            }
        }

        public HashSet<string> MappingDescriptions
        {
            get 
            {
                HashSet<string> mappingDescriptions = new HashSet<string>();

                foreach (ElementMapping em in elementMappings)
                {
                    SourceItem mappedSourceItem = em.SourceItem;

                    if (mappedSourceItem.MappingTargetKey != null)
                    {
                        object targetCc = targetElementStore.GetTargetCc(mappedSourceItem.MappingTargetKey);

                        string targetElementName = null;

                        if (targetCc is IBcc)
                        {
                            targetElementName = ((IBcc) targetCc).Name;
                        }
                        else if (targetCc is ICdtSup)
                        {
                            targetElementName = ((ICdtSup) targetCc).Name;
                        }
                        else if (targetCc is IAscc)
                        {
                            targetElementName = ((IAscc) targetCc).Name;
                        }

                        if (targetElementName != null)
                        {
                            mappingDescriptions.Add(mappedSourceItem.Path + " -> " + targetElementName);
                        }
                        else
                        {
                            MappingFunction mappingFunction =
                                mappingFunctionStore.GetMappingFunction(mappedSourceItem.MappingTargetKey);

                            if ((mappingFunction != null) && (mappingFunction.IsSplit))
                            {
                                mappingDescriptions.Add(mappedSourceItem.Path + " -> SPLIT Function");
                            }
                            else
                            {
                                mappingDescriptions.Add(mappedSourceItem.Path + " -> ??? that would be interesting");
                            }
                        }
                    }
                }

                return mappingDescriptions;
            }
        }

        /// <exception cref="MappingError">Simple typed element mapped to non-BCC CCTS element.</exception>
        private ElementMapping MapElement(SourceItem sourceElement, string path, Stack<XmlQualifiedName> parentComplexTypeNames)
        {
            if (sourceElement.HasSimpleType())
            {
                if (!sourceElement.IsMapped)
                {
                    // ignore element
                    return ElementMapping.NullElementMapping;
                }
                if (IsMappedToBcc(sourceElement))
                {
                    SimpleTypeToCdtMapping simpleTypeToCdtMapping = MapSimpleType(sourceElement);
                    return new AttributeOrSimpleElementOrComplexElementToBccMapping(sourceElement, (IBcc) GetTargetElement(sourceElement), simpleTypeToCdtMapping);
                }
                if (IsMappedToSup(sourceElement))
                {
                    return new AttributeOrSimpleElementToSupMapping(sourceElement, (ICdtSup) GetTargetElement(sourceElement));
                }
                if (IsMappedToSplitFunction(sourceElement))
                {
                    List<SimpleTypeToCdtMapping> simpleTypeToCdtMappings = new List<SimpleTypeToCdtMapping>();
                    MappingFunction splitFunction = GetMappingFunction(sourceElement);
                    foreach (IBcc targetBcc in splitFunction.TargetCcs)
                    {
                        simpleTypeToCdtMappings.Add(MapSimpleType(sourceElement, targetBcc));
                    }
                    return new SplitMapping(sourceElement, splitFunction.TargetCcs.Convert(cc => (IBcc) cc), simpleTypeToCdtMappings);
                }
                throw new MappingError("Simple typed element '" + path + "' mapped to non-BCC CCTS element.");
            }
            
            if (sourceElement.HasComplexType())
            {
                MapComplexType(sourceElement, path, parentComplexTypeNames);

                if (!sourceElement.IsMapped)
                {
                    return new AsmaMapping(sourceElement);
                }
                if (IsMappedToAscc(sourceElement))
                {
                    IAscc targetAscc = (IAscc)GetTargetElement(sourceElement);
                    return new ComplexElementToAsccMapping(sourceElement, targetAscc);
                }
                if (IsMappedToBcc(sourceElement))
                {
                    IBcc targetBcc = (IBcc)GetTargetElement(sourceElement);
                    return new AttributeOrSimpleElementOrComplexElementToBccMapping(sourceElement, targetBcc, null);
                }

                throw new MappingError("Complex typed element '" + path + "' not mapped to either a BCC or an ASCC.");
            }
            throw new Exception("Source element '" + path + "' has neither simple nor complex type.");
        }

        private SimpleTypeToCdtMapping MapSimpleType(SourceItem sourceElement)
        {
            return MapSimpleType(sourceElement, (IBcc)GetTargetElement(sourceElement));
        }

        private SimpleTypeToCdtMapping MapSimpleType(SourceItem sourceElement, IBcc targetBcc)
        {
            var simpleTypeName = sourceElement.XsdTypeName;
            var cdt = targetBcc.Cdt;
            foreach (SimpleTypeToCdtMapping simpleTypeMapping in simpleTypeMappings)
            {
                if (simpleTypeMapping.SimpleTypeName == simpleTypeName && simpleTypeMapping.TargetCDT.Id == cdt.Id)
                {
                    return simpleTypeMapping;
                }
            }
            SimpleTypeToCdtMapping newMapping = new SimpleTypeToCdtMapping(simpleTypeName, cdt);
            simpleTypeMappings.Add(newMapping);
            return newMapping;
        }

        private void MapComplexType(SourceItem sourceElement, string path, Stack<XmlQualifiedName> parentComplexTypeNames)
        {
            XmlQualifiedName qualifiedComplexTypeName = sourceElement.XsdType.QualifiedName;
            if (parentComplexTypeNames.Contains(qualifiedComplexTypeName))
            {
                return;
            }

            string complexTypeName = sourceElement.XsdTypeName;

            if ((IsMappedToBcc(sourceElement)))
            {
                MapComplexTypeToCdt(sourceElement, parentComplexTypeNames, qualifiedComplexTypeName,
                                    complexTypeName, path);

            }
            else
            {
                List<ElementMapping> childMappings = GetChildMappings(sourceElement, parentComplexTypeNames,
                                                                      qualifiedComplexTypeName, path);

                ComplexTypeMapping complexTypeMapping = CreateComplexTypeMappingForChildMappings(childMappings, sourceElement.XsdTypeName,sourceElement.Name);

                if (complexTypeMapping != null)
                {
                    complexTypeMappings.GetAndCreate(complexTypeName).Add(complexTypeMapping);
                }
            }
        }

        private static ComplexTypeMapping CreateComplexTypeMappingForChildMappings(IEnumerable<ElementMapping> childMappings, string complexTypeName, string sourceElementName)
        {
            IAcc targetAcc = null;
            bool hasAsmaMapping = false;
            bool hasMultipleAccMappings = false;

            if (childMappings.Count() == 0)
            {
                // complex type not mapped
                return null;
            }

            foreach (ElementMapping child in childMappings)
            {
                if (child is AttributeOrSimpleElementOrComplexElementToBccMapping)
                {
                    if (targetAcc == null)
                    {
                        targetAcc = ((AttributeOrSimpleElementOrComplexElementToBccMapping) child).Acc;
                    }
                    else
                    {
                        if (targetAcc.Id != ((AttributeOrSimpleElementOrComplexElementToBccMapping) child).Acc.Id)
                        {
                            hasMultipleAccMappings = true;
                        }
                    }
                }

                if (child is ComplexElementToAsccMapping)
                {
                    if (targetAcc == null)
                    {
                        targetAcc = ((ComplexElementToAsccMapping) child).Acc;
                    }
                    else
                    {
                        if (targetAcc.Id != ((ComplexElementToAsccMapping) child).Acc.Id)
                        {
                            hasMultipleAccMappings = true;
                        }
                    }
                }

                if (child is AsmaMapping)
                {
                    hasAsmaMapping = true;
                }
            }

            bool hasAccMapping = targetAcc != null;

            if ((hasAccMapping) && (!hasMultipleAccMappings) && (!hasAsmaMapping))
            {
                // ACC
                return new ComplexTypeToAccMapping(sourceElementName, complexTypeName, childMappings);
            }
            if ((!hasMultipleAccMappings && hasAsmaMapping) || ((hasAccMapping) && hasMultipleAccMappings))
            {
                // MA
                return new ComplexTypeToMaMapping(sourceElementName, complexTypeName, childMappings);
            }
            throw new MappingError("Unexpected Mapping Error #375. Complex Type Name: " + complexTypeName);
        }

        private void MapComplexTypeToCdt(SourceItem sourceElement, Stack<XmlQualifiedName> parentComplexTypeNames, XmlQualifiedName qualifiedComplexTypeName, string complexTypeName, string path)
        {
            ICdt targetCdt = null;
            ComplexTypeMapping complexTypeMapping;
            List<ElementMapping> childMappings = GetChildMappings(sourceElement, parentComplexTypeNames, qualifiedComplexTypeName, path);

            if (childMappings.Count() > 0)
            {
                foreach (ElementMapping child in childMappings)
                {
                    if (child is AttributeOrSimpleElementToSupMapping)
                    {
                        if (targetCdt == null)
                        {
                            targetCdt = ((AttributeOrSimpleElementToSupMapping)child).Cdt;
                        }
                        else
                        {
                            if (targetCdt.Id != ((AttributeOrSimpleElementToSupMapping)child).Cdt.Id)
                            {
                                throw new MappingError("Complex type mapped to more than one CDTs");
                            }
                        }
                    }
                }

                complexTypeMapping = new ComplexTypeToCdtMapping(sourceElement.Name,complexTypeName, childMappings);
                complexTypeMappings.GetAndCreate(complexTypeName + ((IBcc)GetTargetElement(sourceElement)).Cdt.Name).Add(complexTypeMapping);

                return;
            }

            targetCdt = ((IBcc)GetTargetElement(sourceElement)).Cdt;

            complexTypeMapping = new ComplexTypeToCdtMapping(sourceElement.Name,complexTypeName, childMappings) { TargetCdt = targetCdt };

            complexTypeMappings.GetAndCreate(complexTypeName + ((IBcc)GetTargetElement(sourceElement)).Cdt.Name).Add(complexTypeMapping);
        }

        private List<ElementMapping> GetChildMappings(SourceItem sourceElement, Stack<XmlQualifiedName> parentComplexTypeNames, XmlQualifiedName qualifiedComplexTypeName, string path)
        {
            List<ElementMapping> childMappings = new List<ElementMapping>();
            foreach (var child in sourceElement.Children)
            {
                parentComplexTypeNames.Push(qualifiedComplexTypeName);
                var childMapping = MapElement(child, path + "/" + child.Name, parentComplexTypeNames);
                parentComplexTypeNames.Pop();
                if (childMapping != ElementMapping.NullElementMapping)
                {
                    elementMappings.Add(childMapping);
                    childMappings.Add(childMapping);
                }
            }
            return childMappings;
        }

        public ComplexTypeMapping GetComplexTypeMapping(XmlSchemaType complexType)
        {
            return GetComplexTypeToCdtMapping(complexType.Name);
        }

        public ComplexTypeMapping GetComplexTypeToCdtMapping(string complexTypeName)
        {
            List<ComplexTypeMapping> mappings;
            if (complexTypeMappings.TryGetValue(complexTypeName, out mappings))
            {
                return GetComplexTypeMappingWithMostChildren(mappings);
            }
            return ComplexTypeMapping.NullComplexTypeMapping;
        }

        private static ComplexTypeMapping GetComplexTypeMappingWithMostChildren(List<ComplexTypeMapping> mappings)
        {
            if (mappings.Count > 0)
            {
                ComplexTypeMapping complexTypeMapping = mappings[0];
                
                foreach (ComplexTypeMapping mapping in mappings)
                {
                    if (mapping.NumberOfChildren > complexTypeMapping.NumberOfChildren)
                    {
                        complexTypeMapping = mapping;
                    }
                }

                return complexTypeMapping;
            }
            return ComplexTypeMapping.NullComplexTypeMapping;
        }

        private bool IsMappedToAscc(SourceItem element)
        {
            object targetElement = GetTargetElement(element);
            return targetElement != null && targetElement is IAscc;
        }

        private bool IsMappedToBcc(SourceItem element)
        {
            object targetElement = GetTargetElement(element);
            return targetElement != null && targetElement is IBcc;
        }

        private bool IsMappedToSup(SourceItem element)
        {
            object targetElement = GetTargetElement(element);
            return targetElement != null && targetElement is ICdtSup;
        }


        private bool IsMappedToSplitFunction(SourceItem element)
        {
            MappingFunction mappingFunction = GetMappingFunction(element);
            return mappingFunction != null && mappingFunction.IsSplit;
        }

        private MappingFunction GetMappingFunction(SourceItem element)
        {
            if (element.IsMapped)
            {
                return mappingFunctionStore.GetMappingFunction(element.MappingTargetKey);
            }

            return null;            
        }

        private object GetTargetElement(SourceItem element)
        {
            if (element.IsMapped)
            {
                return targetElementStore.GetTargetCc(element.MappingTargetKey);
            }

            return null;  
        }

        public IEnumerable<ComplexTypeMapping> GetComplexTypeMappings()
        {
            foreach (List<ComplexTypeMapping> mappings in complexTypeMappings.Values)
            {
                if (mappings.Count > 0 || mappings[0] != null)
                {
                    yield return mappings[0];
                }
            }
        }

        public IEnumerable<SimpleTypeToCdtMapping> GetSimpleTypeMappings()
        {
            return simpleTypeMappings;
        }
    }
}