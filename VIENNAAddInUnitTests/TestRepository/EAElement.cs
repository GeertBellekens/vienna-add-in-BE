// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using System.Linq;
using EA;

namespace VIENNAAddInUnitTests.TestRepository
{
    internal class EAElement : Element, IEACollectionElement
    {
        private readonly EARepository repository;
        private readonly Collection taggedValues;
        private readonly Collection attributes;
        private string stereotypeEx;

        public EAElement(EARepository repository)
        {
            this.repository = repository;
            attributes = new EAAttributeCollection(repository, this);
            taggedValues = new EATaggedValueCollection(repository, this);
        }

        #region Element Members

        public bool Update()
        {
            return true;
        }

        public string GetLastError()
        {
            throw new NotImplementedException();
        }

        public void Refresh()
        {
        }

        public void SetAppearance(int Scope, int Item, int Value)
        {
            throw new NotImplementedException();
        }

        public string GetRelationSet(EnumRelationSetType Type)
        {
            throw new NotImplementedException();
        }

        public string GetStereotypeList()
        {
            throw new NotImplementedException();
        }

        public string GetLinkedDocument()
        {
            throw new NotImplementedException();
        }

        public bool LoadLinkedDocument(string FileName)
        {
            throw new NotImplementedException();
        }

        public bool SaveLinkedDocument(string FileName)
        {
            throw new NotImplementedException();
        }

        public bool ApplyUserLock()
        {
            throw new NotImplementedException();
        }

        public bool ReleaseUserLock()
        {
            throw new NotImplementedException();
        }

        public bool ApplyGroupLock(string aGroupName)
        {
            throw new NotImplementedException();
        }

        public string Name { get; set; }

        public Collection Requirements
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Constraints
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Scenarios
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Files
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Efforts
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Risks
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Metrics
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Issues
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Tests
        {
            get { throw new NotImplementedException(); }
        }

        public Collection TaggedValues
        {
            get
            {
                return taggedValues;
            }
        }

        public Collection Connectors
        {
            get
            {
                var myConnectors = new EAConnectorCollection(repository, this);
                myConnectors.Elements.AddRange(from c1 in repository.Connectors where c1.ClientID == ElementID || c1.SupplierID == ElementID select c1 as IEACollectionElement);
//                myConnectors.Elements.AddRange(from c1 in repository.Connectors where c1.ClientID == ElementID || c1.SupplierID == ElementID orderby c1.Name select c1 as IEACollectionElement);
                return myConnectors;
            }
        }

        public string Notes
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool Locked
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Version
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Multiplicity
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string ElementGUID
        {
            get { return "" + ElementID; }
        }

        public string ExtensionPoints
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Tablespace
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Tag
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Stereotype { get; set; }

        public string Genlinks
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Abstract
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Alias
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Author
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Complexity
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Visibility
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Priority
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Phase
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Persistence
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsActive
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsLeaf
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsNew
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsSpec
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int Subtype
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Type { get; set; }

        public int ClassfierID
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string ClassifierName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string ClassifierType
        {
            get { throw new NotImplementedException(); }
        }

        public DateTime Created
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public DateTime Modified
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Difficulty
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Genfile
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Gentype
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public object Header1
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public object Header2
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int ElementID { get; set; }

        public int PackageID { get; set; }

        public Collection Methods
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Attributes
        {
            get { return attributes; }
        }

        public Collection Resources
        {
            get { throw new NotImplementedException(); }
        }

        public string StyleEx
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string EventFlags
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string ActionFlags
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int ClassifierID
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Status
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int TreePos
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Collection Elements
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Diagrams
        {
            get { throw new NotImplementedException(); }
        }

        public int ParentID
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public ObjectType ObjectType { get; set; }

        public Collection Partitions
        {
            get { throw new NotImplementedException(); }
        }

        public Collection CustomProperties
        {
            get { throw new NotImplementedException(); }
        }

        public Collection StateTransitions
        {
            get { throw new NotImplementedException(); }
        }

        public Collection EmbeddedElements
        {
            get { throw new NotImplementedException(); }
        }

        public Collection BaseClasses
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Realizes
        {
            get { throw new NotImplementedException(); }
        }

        public Collection TaggedValuesEx
        {
            get { throw new NotImplementedException(); }
        }

        public Collection AttributesEx
        {
            get { throw new NotImplementedException(); }
        }

        public Collection MethodsEx
        {
            get { throw new NotImplementedException(); }
        }

        public Collection ConstraintsEx
        {
            get { throw new NotImplementedException(); }
        }

        public Collection RequirementsEx
        {
            get { throw new NotImplementedException(); }
        }

        public string StereotypeEx
        {
            get { return stereotypeEx ?? Stereotype; }
            set { stereotypeEx = value; }
        }

        public int PropertyType
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Properties Properties
        {
            get { throw new NotImplementedException(); }
        }

        public string MetaType
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string RunState
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public object CompositeDiagram
        {
            get { throw new NotImplementedException(); }
        }

        public string get_MiscData(int index)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEACollectionElement Members

        string IEACollectionElement.Name
        {
            get { return Name; }
            set { Name = value; }
        }

        #endregion
    }
}