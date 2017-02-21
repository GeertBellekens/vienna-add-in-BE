using System;
using EA;

namespace VIENNAAddInUnitTests.TestRepository
{
    internal class EADiagram : Diagram, IEACollectionElement
    {
        private readonly Collection diagramObjects;

        public EADiagram(EARepository repository)
        {
            diagramObjects = new EADiagramObjectCollection(repository, this);
        }

        public bool Update()
        {
            // do nothing
            return true;
        }

        public string GetLastError()
        {
            throw new NotImplementedException();
        }

        public void ReorderMessages()
        {
            throw new NotImplementedException();
        }

        public void ShowAsElementList(bool ShowAsList, bool Persist)
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

        public int DiagramID { get; set;}

        public int PackageID { get; set;}

        public int ParentID
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Type { get; set; }

        public string Name { get; set; }

        public string Version
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Author
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int ShowDetails
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Notes
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Stereotype
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool ShowPublic
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool ShowPrivate
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool ShowProtected
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Orientation
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int cx
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int cy
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int Scale
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public DateTime CreatedDate
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public DateTime ModifiedDate
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool HighlightImports
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool ShowPackageContents
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string ExtendedStyle
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsLocked
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string DiagramGUID
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Swimlanes
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Collection DiagramObjects
        {
            get { return diagramObjects; }
        }

        public Collection DiagramLinks
        {
            get { throw new NotImplementedException(); }
        }

        public string StyleEx
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Collection SelectedObjects
        {
            get { throw new NotImplementedException(); }
        }

        public ObjectType ObjectType
        {
            get { throw new NotImplementedException(); }
        }

        public string StereotypeEx
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Connector SelectedConnector
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string MetaType
        {
            get { throw new NotImplementedException(); }
        }

        public SwimlaneDef SwimlaneDef
        {
            get { throw new NotImplementedException(); }
        }
    }
}