using System;
using System.Collections.Generic;
using System.Xml.Schema;

namespace VIENNAAddIn.upcc3.import.mapping
{
    public class MapForceSourceItemTree
    {
        private readonly MapForceMapping mapForceMapping;

        /// <exception cref="ArgumentException">The MapForce mapping does not contain any input schema components.</exception>
        public MapForceSourceItemTree(MapForceMapping mapForceMapping, XmlSchemaSet xmlSchemaSet)
        {
            this.mapForceMapping = mapForceMapping;
            xmlSchemaSet.Compile();
            /// Retrieve the schema component containing the input schemas' root element.
            /// 
            /// If there is only one input schema component, then this component is the root schema component.
            /// 
            /// If there are more than one input schema components, we look for a constant component with value "Root: *", where "*" must
            /// be the name of the root XSD element of the input schemas. We then return the schema component containing this element as its root.

            var inputSchemaComponents = new List<SchemaComponent>(mapForceMapping.GetInputSchemaComponents());

            var schemaComponentTrees = new List<SourceItem>();
            foreach (SchemaComponent inputSchemaComponent in inputSchemaComponents)
            {
                XmlSchemaElement rootXsdElement = (XmlSchemaElement) xmlSchemaSet.GlobalElements[inputSchemaComponent.InstanceRoot];
                if (rootXsdElement == null)
                {
                    throw new MappingError("Root element of input schema component [" + inputSchemaComponent.InstanceRoot + "] not found in XSD global elements.");
                }
                SourceItem tree = CreateSourceItemTree(inputSchemaComponent.RootEntry, rootXsdElement);
                schemaComponentTrees.Add(tree);
            }

            if (schemaComponentTrees.Count == 0)
            {
                throw new MappingError("The MapForce mapping does not contain any input schema components.");
            }
            else if (schemaComponentTrees.Count == 1)
            {
                RootSourceItem = schemaComponentTrees[0];                
            } 
            else
            {
                var rootElementName = mapForceMapping.GetConstant("Root");
                if (string.IsNullOrEmpty(rootElementName))
                {
                    throw new MappingError("The MapForce mapping does not specify the root source element name.");
                }
                var nonRootElementTrees = new List<SourceItem>();
                foreach (SourceItem tree in schemaComponentTrees)
                {
                    if (tree.Name == rootElementName)
                    {
                        RootSourceItem = tree;
                    }
                    else
                    {
                        nonRootElementTrees.Add(tree);
                    }
                }
                if (RootSourceItem == null)
                {
                    throw new ArgumentException("The MapForce mapping does not contain an input schema component with the specified root element: " + rootElementName);
                }
                List<SourceItem> unattachedNonRootItemTrees = new List<SourceItem>(nonRootElementTrees);
                while (unattachedNonRootItemTrees.Count > 0)
                {
                    bool atLeastOneTreeAttached = false;
                    foreach (var nonRootElementTree in new List<SourceItem>(unattachedNonRootItemTrees))
                    {
                        // TODO iteratively attach trees until no attachable trees are left
                        if (AttachSubTree(nonRootElementTree, RootSourceItem))
                        {
                            atLeastOneTreeAttached = true;
                            unattachedNonRootItemTrees.Remove(nonRootElementTree);
                        }
                    }
                    if (!atLeastOneTreeAttached)
                    {
                        break;
                    }
                }
                if (unattachedNonRootItemTrees.Count > 0)
                {
                    List<string> itemTreeNames = new List<string>();
                    foreach (SourceItem tree in unattachedNonRootItemTrees)
                    {
                        itemTreeNames.Add(tree.Name);
                    }
                    throw new MappingError("The following schema components could not be attached to the source item tree: " + string.Join(", ", itemTreeNames.ToArray()));
                }
            }
        }

        public SourceItem RootSourceItem { get; private set; }

        public IEnumerable<SourceItem> MappedSourceItems
        {
            get { return FindMappedSourceItems(RootSourceItem); }
        }

        private IEnumerable<SourceItem> FindMappedSourceItems(SourceItem sourceItem)
        {
            if (sourceItem.MappingTargetKey != null)
            {
                yield return sourceItem;
            }
            foreach (SourceItem child in sourceItem.Children)
            {
                foreach (SourceItem mappedChildren in FindMappedSourceItems(child))
                {
                    yield return mappedChildren;
                }
            }
        }

        /// <summary>
        /// Perform name matching between the sub-tree's root element name and element names in the sourceElementTree.
        /// 
        /// When a matching element is found, attach the sub-tree root's children to the element in the source tree.
        /// </summary>
        /// <param name="subTreeRoot"></param>
        /// <param name="sourceElementTree"></param>
        private static bool AttachSubTree(SourceItem subTreeRoot, SourceItem sourceElementTree)
        {
            bool hasBeenAttached = false;
            // The al TODO documentation
            foreach (var child in sourceElementTree.Children)
            {
                hasBeenAttached = hasBeenAttached || AttachSubTree(subTreeRoot, child);
            }
            if (subTreeRoot.XsdTypeName == sourceElementTree.XsdTypeName)
            {
                sourceElementTree.MergeWith(subTreeRoot);
                
                hasBeenAttached = true;
            }
            return hasBeenAttached;
        }


        private SourceItem CreateSourceItemTree(Entry mapForceEntry, XmlSchemaObject sourceXsdObject)
        {
            XmlSchemaType xsdType = GetXsdTypeForXsdObject(sourceXsdObject);

            var sourceItem = new SourceItem(mapForceEntry.Name, xsdType, mapForceEntry.XsdObjectType, mapForceMapping.GetMappingTargetKey(mapForceEntry.InputOutputKey.Value));

            IEnumerable<XmlSchemaObject> childrenOfXsdType = GetChildElementsAndAttributesDefinedByXsdType(sourceItem.XsdType);
            
            //HashSet<string> childrenOfXsdTypeNames = new HashSet<string>();

            //int unknownChildTypeCount = 0;
            //foreach (XmlSchemaObject xmlSchemaObject in childrenOfXsdType)
            //{
            //    if (xmlSchemaObject is XmlSchemaElement)
            //    {
            //        childrenOfXsdTypeNames.Add("E_" + ((XmlSchemaElement)xmlSchemaObject).QualifiedName.Name);
            //    }
            //    else if (xmlSchemaObject is XmlSchemaAttribute)
            //    {
            //        childrenOfXsdTypeNames.Add("A_" + ((XmlSchemaAttribute)xmlSchemaObject).QualifiedName.Name);
            //    }
            //    else
            //    {
            //        childrenOfXsdTypeNames.Add("Unknown Child Type["+(++unknownChildTypeCount)+"]: " + xmlSchemaObject.GetType().Name);
            //    }
            //}

            //HashSet<string> childrenOfMapForceEntryNames = new HashSet<string>();

            //foreach (Entry subEntry in mapForceEntry.SubEntries)
            //{
            //    switch (subEntry.XsdObjectType)
            //    {
            //        case XsdObjectType.Element:
            //            childrenOfMapForceEntryNames.Add("E_" + subEntry.Name);
            //            break;
            //        case XsdObjectType.Attribute:
            //            childrenOfMapForceEntryNames.Add("A_" + subEntry.Name);
            //            break;
            //    }
            //}

            //HashSet<string> mapForceEntriesNotFoundInXsd = new HashSet<string>(childrenOfMapForceEntryNames);
            //mapForceEntriesNotFoundInXsd.ExceptWith(childrenOfXsdTypeNames);
            //if (mapForceEntriesNotFoundInXsd.Count > 0)
            //{
            //    Console.Out.WriteLine("AAAHHHHHHHHH!!!!!!!!!!!!!!!!!!!!!!");
            //}
            
            foreach (XmlSchemaObject childOfXsdType in childrenOfXsdType)
            {
                Entry mapForceSubEntry;

                if (childOfXsdType is XmlSchemaElement)
                {
                    mapForceSubEntry = mapForceEntry.GetSubEntryForElement(((XmlSchemaElement)childOfXsdType).QualifiedName.Name);
                }
                else if (childOfXsdType is XmlSchemaAttribute)
                {
                    mapForceSubEntry = mapForceEntry.GetSubEntryForAttribute(((XmlSchemaAttribute)childOfXsdType).QualifiedName.Name);
                }
                else
                {
                    throw new Exception("Child of XSD Type is neither an XSD Element nor an XSD Attribute. The type of the Child is " + sourceXsdObject.GetType());
                }

                if (mapForceSubEntry != null)
                {
                    SourceItem sourceItemTreeForChild = CreateSourceItemTree(mapForceSubEntry, childOfXsdType);

                    sourceItem.AddChild(sourceItemTreeForChild);
                }
                else
                {
                    XmlSchemaType xsdTypeForChild = GetXsdTypeForXsdObject(childOfXsdType);

                    if (childOfXsdType is XmlSchemaElement)
                        sourceItem.AddChild(new SourceItem(((XmlSchemaElement)childOfXsdType).QualifiedName.Name, xsdTypeForChild, XsdObjectType.Element, null));
                        

                    if (childOfXsdType is XmlSchemaAttribute)
                        sourceItem.AddChild(new SourceItem(((XmlSchemaAttribute)childOfXsdType).QualifiedName.Name, xsdTypeForChild, XsdObjectType.Attribute, null));
                }
            }

            return sourceItem;
        }

        private static XmlSchemaType GetXsdTypeForXsdObject(XmlSchemaObject sourceXsdObject)
        {
            if (sourceXsdObject is XmlSchemaElement)
            {
                return ((XmlSchemaElement)sourceXsdObject).ElementSchemaType;    
            }

            if (sourceXsdObject is XmlSchemaAttribute)
            {
                return ((XmlSchemaAttribute)sourceXsdObject).AttributeSchemaType;
            }

            throw new ArgumentException("Source XSD Object is neither an XSD Element nor an XSD Attribute. The type of the XSD Object is " + sourceXsdObject.GetType());
        }

        private static IEnumerable<XmlSchemaObject> GetChildElementsAndAttributesDefinedByXsdType(XmlSchemaType xmlSchemaType)
        {
            List<XmlSchemaObject> myChildren = new List<XmlSchemaObject>();
           if (xmlSchemaType is XmlSchemaComplexType)
           {
               XmlSchemaComplexType complexType = (XmlSchemaComplexType) xmlSchemaType;

               if (complexType.Particle is XmlSchemaGroupBase)
               {
                    myChildren.AddRange(GetChildElementsAndAttributesDefinedByXsdGroup((XmlSchemaGroupBase) complexType.Particle));
               }

               if (complexType.ContentModel is XmlSchemaSimpleContent)
               {
                   XmlSchemaSimpleContent contentModel = (XmlSchemaSimpleContent) complexType.ContentModel;

                   if (contentModel.Content is XmlSchemaSimpleContentExtension)
                   {
                       foreach (XmlSchemaObject attribute in ((XmlSchemaSimpleContentExtension)contentModel.Content).Attributes)
                       {
                           myChildren.Add(attribute);
                       }                       
                   } 
                   else if (contentModel.Content is XmlSchemaSimpleContentRestriction)
                   {
                       foreach (XmlSchemaObject attribute in ((XmlSchemaSimpleContentRestriction)contentModel.Content).Attributes)
                       {
                           myChildren.Add(attribute);
                       }
                   }
               }

               foreach (XmlSchemaAttribute attribute in complexType.Attributes)
               {
                   myChildren.Add(attribute);
               }

               foreach (XmlSchemaObject child in myChildren)
               {
                   yield return child;
               }
           }

           if (xmlSchemaType.BaseXmlSchemaType != null)
           {
               List<XmlSchemaObject> x = new List<XmlSchemaObject>(GetChildElementsAndAttributesDefinedByXsdType(xmlSchemaType.BaseXmlSchemaType));
               foreach (var child in x)
               {
                   if (!ContainsXsdObject(myChildren, child))
                   {
                       yield return child;
                   }
               }
           }
        }

        private static bool ContainsXsdObject(IEnumerable<XmlSchemaObject> xsdObjects, XmlSchemaObject xsdObject)
        {
            foreach (XmlSchemaObject o in xsdObjects)
            {
                if ((o is XmlSchemaAttribute) && (xsdObject is XmlSchemaAttribute))
                {
                    if (((XmlSchemaAttribute) o).Name == ((XmlSchemaAttribute) xsdObject).Name)
                    {
                        return true;
                    }                    
                }
                else if ((o is XmlSchemaElement) && (xsdObject is XmlSchemaElement))
                {
                    if (((XmlSchemaElement)o).Name == ((XmlSchemaElement)xsdObject).Name)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static IEnumerable<XmlSchemaObject> GetChildElementsAndAttributesDefinedByXsdGroup(XmlSchemaGroupBase xsdGroup)
        {
            foreach (XmlSchemaObject item in xsdGroup.Items)
            {
                if (item is XmlSchemaElement)
                {
                    yield return item;
                }
                else if (item is XmlSchemaGroupBase)
                {
                    foreach (var child in GetChildElementsAndAttributesDefinedByXsdGroup((XmlSchemaGroupBase)item))
                    {
                        yield return child;
                    }
                }
            }
        }
    }
}