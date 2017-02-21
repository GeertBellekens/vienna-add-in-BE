using System.Collections.Generic;
using EA;

namespace VIENNAAddIn.Utils
{
    ///<summary>
    /// <para>Provides functionality to copy the contents of one repository to another repository (all existing content is removed from the target repository before).</para>
    /// 
    /// <para><b>Attention:</b> This implementation is not complete and must be extended as the need arises. A complete implementation is out of scope for the moment.</para>
    ///</summary>
    public class RepositoryCopier
    {
        private readonly List<Connector> connectors = new List<Connector>();
        private readonly Dictionary<int, int> elementMapping = new Dictionary<int, int>();
        private readonly Repository sourceRepository;
        private readonly Repository targetRepository;

        private RepositoryCopier(Repository sourceRepository, Repository targetRepository)
        {
            this.sourceRepository = sourceRepository;
            this.targetRepository = targetRepository;
        }

        ///<summary>
        /// Copy the contents of the sourceRepository repository to the target repository.
        ///</summary>
        ///<param name="sourceRepository"></param>
        ///<param name="targetRepository"></param>
        public static void CopyRepository(Repository sourceRepository, Repository targetRepository)
        {
            new RepositoryCopier(sourceRepository, targetRepository).Copy();
        }

        private static void CopyCollection<T>(Collection sourceCollection, Collection targetCollection,
                                              CopyToCollection<T> copyToCollection, int targetContainerId)
        {
            foreach (T sourceElement in sourceCollection)
            {
                copyToCollection(sourceElement, targetCollection, targetContainerId);
                targetCollection.Refresh();
            }
        }

        private void Copy()
        {
            RemoveContentFromTargetRepository();
            CopyPackageCollection(sourceRepository.Models, targetRepository.Models, 0);
            CopyConnectors();
        }

        /// <summary>
        /// Removes all content from the target repository.
        /// </summary>
        private void RemoveContentFromTargetRepository()
        {
            Collection models = targetRepository.Models;
            for (short i = 0; i < models.Count; i++)
            {
                models.Delete(i);
            }
            models.Refresh();
        }

        private void CopyConnectors()
        {
            foreach (Connector sourceConnector in connectors)
            {
                Element targetClient = targetRepository.GetElementByID(elementMapping[sourceConnector.ClientID]);
                Element targetSupplier = targetRepository.GetElementByID(elementMapping[sourceConnector.SupplierID]);
                CopyConnector(sourceConnector, targetClient.Connectors, targetClient.ElementID, targetSupplier.ElementID);
                targetClient.Connectors.Refresh();
            }
        }

        private void CopyConnector(Connector sourceConnector, Collection targetConnectors, int clientId, int supplierId)
        {
            var targetConnector = (Connector) targetConnectors.AddNew(sourceConnector.Name, sourceConnector.Type);
            targetConnector.ClientID = clientId;
            targetConnector.SupplierID = supplierId;
            targetConnector.Name = sourceConnector.Name;
            targetConnector.Stereotype = sourceConnector.Stereotype;
            targetConnector.Update();
            CopyConnectorEnd(sourceConnector.ClientEnd, targetConnector.ClientEnd);
            CopyConnectorEnd(sourceConnector.SupplierEnd, targetConnector.SupplierEnd);
            targetConnector.Update();
            CopyCollection<TaggedValue>(sourceConnector.TaggedValues, targetConnector.TaggedValues, CopyTaggedValue, targetConnector.ConnectorID);
        }

        private void CopyConnectorEnd(ConnectorEnd sourceEnd, ConnectorEnd targetEnd)
        {
            targetEnd.Role = sourceEnd.Role;
            targetEnd.Aggregation = sourceEnd.Aggregation;
            targetEnd.Cardinality = sourceEnd.Cardinality;
            targetEnd.Containment = sourceEnd.Containment;
            targetEnd.Update();
        }

        private void CopyPackageCollection(Collection sourcePackages, Collection targetPackages, int targetParentID)
        {
            foreach (Package sourcePackage in sourcePackages)
            {
                var targetPackage = (Package) targetPackages.AddNew(sourcePackage.Name, "");
                targetPackage.Update();
                CopyPackage(sourcePackage, targetPackage, targetParentID);
                targetPackages.Refresh();
            }
        }

        private void CopyPackage(Package sourcePackage, Package targetPackage, int targetParentID)
        {
            if (targetParentID != 0)
            {
                targetPackage.ParentID = targetParentID;
            }
            if (sourcePackage.Element != null)
            {
                CopyElementContents(sourcePackage.Element, targetPackage.Element);
            }
            CopyPackageCollection(sourcePackage.Packages, targetPackage.Packages, targetPackage.PackageID);
            CopyCollection<Element>(sourcePackage.Elements, targetPackage.Elements, CopyElement, targetPackage.PackageID);
            CopyCollection<Diagram>(sourcePackage.Diagrams, targetPackage.Diagrams, CopyDiagram, targetPackage.PackageID);
        }

        private void CopyDiagram(Diagram sourceDiagram, Collection targetCollection, int targetPackageId)
        {
            var targetDiagram = (Diagram) targetCollection.AddNew(sourceDiagram.Name, sourceDiagram.Type);
            targetDiagram.PackageID = targetPackageId;
            targetDiagram.Update();
        }

        private void CopyElementContents(Element sourceElement, Element targetElement)
        {
            //targetElement.Name = sourceElement.Name;
            targetElement.Stereotype = sourceElement.Stereotype;
            targetElement.StereotypeEx = sourceElement.StereotypeEx;
            targetElement.Update();
            CopyCollection<TaggedValue>(sourceElement.TaggedValues, targetElement.TaggedValues, CopyTaggedValue,
                                        targetElement.ElementID);
            CopyCollection<Attribute>(sourceElement.Attributes, targetElement.Attributes, CopyAttribute,
                                      targetElement.ElementID);
            foreach (Connector connector in sourceElement.Connectors)
            {
                if (connector.ClientID == sourceElement.ElementID)
                {
                    connectors.Add(connector);
                }
            }
        }

        private void CopyElement(Element sourceElement, Collection targetCollection, int targetPackageId)
        {
            var targetElement = (Element) targetCollection.AddNew(sourceElement.Name, sourceElement.Type);
            targetElement.PackageID = targetPackageId;
            targetElement.Update();
            elementMapping[sourceElement.ElementID] = targetElement.ElementID;
            CopyElementContents(sourceElement, targetElement);
        }

        private void CopyAttribute(Attribute sourceAttribute, Collection targetCollection, int targetElementId)
        {
            var targetAttribute = (Attribute) targetCollection.AddNew(sourceAttribute.Name, sourceAttribute.Type);
            targetAttribute.Default = sourceAttribute.Default;
            targetAttribute.LowerBound = sourceAttribute.LowerBound;
            targetAttribute.UpperBound = sourceAttribute.UpperBound;
            targetAttribute.Update();
            CopyCollection<AttributeTag>(sourceAttribute.TaggedValues, targetAttribute.TaggedValues, CopyAttributeTag,
                                         targetAttribute.AttributeID);
        }

        private void CopyTaggedValue(TaggedValue sourceTaggedValue, Collection targetCollection, int targetElementId)
        {
            var targetTaggedValue = (TaggedValue) targetCollection.AddNew(sourceTaggedValue.Name, "");
            targetTaggedValue.ElementID = targetElementId;
            targetTaggedValue.Value = sourceTaggedValue.Value;
            targetTaggedValue.Update();
        }

        private void CopyAttributeTag(AttributeTag sourceTag, Collection targetCollection, int targetAttributeId)
        {
            var targetTag = (AttributeTag) targetCollection.AddNew(sourceTag.Name, "");
            targetTag.AttributeID = targetAttributeId;
            targetTag.Value = sourceTag.Value;
            targetTag.Update();
        }

        #region Nested type: CopyToCollection

        private delegate void CopyToCollection<T>(T sourceElement, Collection collection, int targetContainerId);

        #endregion
    }
}