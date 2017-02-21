// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3;
using Attribute=EA.Attribute;

namespace VIENNAAddInUnitTests.TestRepository
{
    /// <summary>
    /// An implementation of the <c>EA.Repository</c> API for unit testing.
    /// 
    /// <para>Even though it is possible to implement unit tests by loading an <c>EA.Repository</c> from a file, 
    /// this process is to slow for efficient unit testing. Therefore, this class offers an efficient 
    /// alternative for testing algorithms operating an <c>EA.Repository</c>.</para>
    /// 
    /// <remarks>
    /// Note that this implementation is by no means complete. Instead, it reflects the demands of our unit tests 
    /// up to date and should be extended as required by future tests and in compliance with the <c>EA.Repository</c> API.
    /// </remarks>
    /// </summary>
    public class EARepository : Repository
    {
        private readonly List<EAConnector> connectors = new List<EAConnector>();
        private readonly Dictionary<int, EAElement> elementsById = new Dictionary<int, EAElement>();

        /// <summary>
        /// Counter for unique repository element IDs.
        /// </summary>
        private readonly IDFactory idFactory = new IDFactory();

        protected readonly EACollection models;
        private readonly Dictionary<int, EAPackage> packagesById = new Dictionary<int, EAPackage>();
        private readonly Project project = new EAProject();

        public EARepository()
        {
            models = new EAPackageCollection(this, null);
        }

        internal IEnumerable<EAConnector> Connectors
        {
            get { return connectors; }
        }

        #region Repository Members

        public bool OpenFile(string FilePath)
        {
            throw new NotImplementedException();
        }

        public void Exit()
        {
            throw new NotImplementedException();
        }

        public Element GetElementByID(int id)
        {
            return elementsById[id];
        }

        public Package GetPackageByID(int id)
        {
            return packagesById[id];
        }

        public string GetLastError()
        {
            throw new NotImplementedException();
        }

        public Diagram GetDiagramByID(int DiagramID)
        {
            throw new NotImplementedException();
        }

        public Reference GetReferenceList(string ListName)
        {
            throw new NotImplementedException();
        }

        public Project GetProjectInterface()
        {
            return project;
        }

        public bool OpenFile2(string FilePath, string Username, string Password)
        {
            throw new NotImplementedException();
        }

        public void ShowWindow(int Show)
        {
            throw new NotImplementedException();
        }

        public Diagram GetCurrentDiagram()
        {
            throw new NotImplementedException();
        }

        public Connector GetConnectorByID(int ConnectorID)
        {
            throw new NotImplementedException();
        }

        public ObjectType GetTreeSelectedItem(out object Item)
        {
            throw new NotImplementedException();
        }

        public Package GetTreeSelectedPackage()
        {
            throw new NotImplementedException();
        }

        public void CloseAddins()
        {
            throw new NotImplementedException();
        }

        public Package GetPackageByGuid(string GUID)
        {
            throw new NotImplementedException();
        }

        public void AdviseElementChange(int ElementID)
        {
            throw new NotImplementedException();
        }

        public void AdviseConnectorChange(int ConnectorID)
        {
            throw new NotImplementedException();
        }

        public void OpenDiagram(int DiagramID)
        {
            throw new NotImplementedException();
        }

        public void CloseDiagram(int DiagramID)
        {
            throw new NotImplementedException();
        }

        public void ActivateDiagram(int DiagramID)
        {
            throw new NotImplementedException();
        }

        public void SaveDiagram(int DiagramID)
        {
            throw new NotImplementedException();
        }

        public void ReloadDiagram(int DiagramID)
        {
            throw new NotImplementedException();
        }

        public void CloseFile()
        {
            throw new NotImplementedException();
        }

        public int __TempDebug(int No, DateTime No2, out int pNo3)
        {
            throw new NotImplementedException();
        }

        public ObjectType GetTreeSelectedItemType()
        {
            throw new NotImplementedException();
        }

        public string GetTechnologyVersion(string ID)
        {
            throw new NotImplementedException();
        }

        public bool IsTechnologyLoaded(string ID)
        {
            throw new NotImplementedException();
        }

        public bool ImportTechnology(string Technology)
        {
            throw new NotImplementedException();
        }

        public string GetCounts()
        {
            throw new NotImplementedException();
        }

        public Collection GetElementSet(string IDList, int Unused)
        {
            throw new NotImplementedException();
        }

        public bool DeleteTechnology(string ID)
        {
            throw new NotImplementedException();
        }

        public object AddTab(string TabName, string ControlID)
        {
            throw new NotImplementedException();
        }

        public void CreateOutputTab(string Name)
        {
            throw new NotImplementedException();
        }

        public void RemoveOutputTab(string Name)
        {
            throw new NotImplementedException();
        }

        public void WriteOutput(string Name, string String, int ID)
        {
            throw new NotImplementedException();
        }

        public void ClearOutput(string Name)
        {
            throw new NotImplementedException();
        }

        public void EnsureOutputVisible(string Name)
        {
            throw new NotImplementedException();
        }

        public Element GetElementByGuid(string GUID)
        {
            throw new NotImplementedException();
        }

        public Connector GetConnectorByGuid(string GUID)
        {
            throw new NotImplementedException();
        }

        public void ShowDynamicHelp(string Topic)
        {
            throw new NotImplementedException();
        }

        public ObjectType GetContextItem(out object Item)
        {
            Item = null;
            return ObjectType.otNone;
        }

        public ObjectType GetContextItemType()
        {
            throw new NotImplementedException();
        }

        public void RefreshModelView(int PackageID)
        {
            // do nothing
        }

        public void ActivateTab(string Name)
        {
            throw new NotImplementedException();
        }

        public void ShowInProjectView(object Object)
        {
            throw new NotImplementedException();
        }

        public object GetDiagramByGuid(string GUID)
        {
            throw new NotImplementedException();
        }

        public string GetCurrentLoginUser(bool GetGuid)
        {
            throw new NotImplementedException();
        }

        public bool ChangeLoginUser(string Name, string Password)
        {
            throw new NotImplementedException();
        }

        public object GetTreeSelectedObject()
        {
            throw new NotImplementedException();
        }

        public void Execute(string SQL)
        {
            throw new NotImplementedException();
        }

        public void ShowProfileToolbox(string Technology, string Profile, bool Show)
        {
            throw new NotImplementedException();
        }

        public void SetUIPerspective(string Perspective)
        {
            throw new NotImplementedException();
        }

        public bool ActivatePerspective(string Perspective, int Options)
        {
            throw new NotImplementedException();
        }

        public bool AddPerspective(string Perspective, int Options)
        {
            throw new NotImplementedException();
        }

        public bool DeletePerspective(string Perspective, int Options)
        {
            throw new NotImplementedException();
        }

        public string GetActivePerspective()
        {
            throw new NotImplementedException();
        }

        public string HasPerspective(string Perspective)
        {
            throw new NotImplementedException();
        }

        public bool CreateModel(CreateModelType CreateType, string FilePath, int ParentWnd)
        {
            throw new NotImplementedException();
        }

        public Attribute GetAttributeByGuid(string GUID)
        {
            throw new NotImplementedException();
        }

        public Method GetMethodByGuid(string GUID)
        {
            throw new NotImplementedException();
        }

        public void RemoveTab(string Name)
        {
            throw new NotImplementedException();
        }

        public string CustomCommand(string ClassName, string MethodName, string Parameters)
        {
            throw new NotImplementedException();
        }

        public int IsTabOpen(string TabName)
        {
            throw new NotImplementedException();
        }

        public bool AddDefinedSearches(string sXML)
        {
            throw new NotImplementedException();
        }

        public void ShowBrowser(string TabName, string URL)
        {
            throw new NotImplementedException();
        }

        public Collection GetElementsByQuery(string QueryName, string SearchTerm)
        {
            throw new NotImplementedException();
        }

        public void RunModelSearch(string QueryName, string SearchTerm, string SearchOptions, string SearchData)
        {
            throw new NotImplementedException();
        }

        public string SQLQuery(string SQL)
        {
            throw new NotImplementedException();
        }

        public string GetTreeXML(int RootPackageID)
        {
            throw new NotImplementedException();
        }

        public string GetTreeXMLByGUID(string GUID)
        {
            throw new NotImplementedException();
        }

        public void RefreshOpenDiagrams(bool FullReload)
        {
        }

        public string GetTreeXMLForElement(int ElementID)
        {
            throw new NotImplementedException();
        }

        public void ImportPackageBuildScripts(string PackageGUID, string BuildScriptXML)
        {
            throw new NotImplementedException();
        }

        public void ExecutePackageBuildScript(int ScriptOptions, string PackageGUID)
        {
            throw new NotImplementedException();
        }

        public bool ActivateToolbox(string Toolbox, int Options)
        {
            throw new NotImplementedException();
        }

        public bool SaveAuditLogs(string FilePath, object StateDateTime, object EndDateTime)
        {
            throw new NotImplementedException();
        }

        public bool ClearAuditLogs(object StateDateTime, object EndDateTime)
        {
            throw new NotImplementedException();
        }

        public ModelWatcher CreateModelWatcher()
        {
            throw new NotImplementedException();
        }

        public bool ActivateTechnology(string ID)
        {
            throw new NotImplementedException();
        }

        public void SaveAllDiagrams()
        {
            throw new NotImplementedException();
        }

        public string GetFormatFromField(string Format, string Text)
        {
            throw new NotImplementedException();
        }

        public string GetFieldFromFormat(string Format, string Text)
        {
            throw new NotImplementedException();
        }

        public bool IsTechnologyEnabled(string ID)
        {
            throw new NotImplementedException();
        }

        public Attribute GetAttributeByID(int AttributeID)
        {
            throw new NotImplementedException();
        }

        public Method GetMethodByID(int MethodID)
        {
            throw new NotImplementedException();
        }

        public object GetContextObject()
        {
            throw new NotImplementedException();
        }

        public Collection GetTreeSelectedElements()
        {
            throw new System.NotImplementedException();
        }

        public void VersionControlResynchPkgStatuses(bool ClearSettings)
        {
            throw new System.NotImplementedException();
        }

        public object AddWindow(string TabName, string ControlID)
        {
            throw new System.NotImplementedException();
        }

        public Collection Models
        {
            get { return models; }
        }

        public Collection Terms
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Issues
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Authors
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Clients
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Tasks
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Datatypes
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Resources
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Stereotypes
        {
            get { throw new NotImplementedException(); }
        }

        public ObjectType ObjectType
        {
            get { throw new NotImplementedException(); }
        }

        public int LibraryVersion
        {
            get { throw new NotImplementedException(); }
        }

        public string LastUpdate
        {
            get { throw new NotImplementedException(); }
        }

        public bool FlagUpdate
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string InstanceGUID
        {
            get { throw new NotImplementedException(); }
        }

        public string ConnectionString
        {
            get { throw new NotImplementedException(); }
        }

        public Collection PropertyTypes
        {
            get { throw new NotImplementedException(); }
        }

        public bool EnableUIUpdates
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool BatchAppend
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsSecurityEnabled
        {
            get { throw new NotImplementedException(); }
        }

        public App App
        {
            get { throw new NotImplementedException(); }
        }

        public EAEditionTypes EAEdition
        {
            get { throw new NotImplementedException(); }
        }

        public bool SuppressEADialogs
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool EnableCache
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int EnableEventFlags
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string ProjectGUID
        {
            get { throw new NotImplementedException(); }
        }

        public bool SuppressSecurityDialog
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public EAEditionTypes EAEditionEx
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        public EAPackage CreatePackage(string name, string type, int parentId)
        {
            int id = idFactory.NextID;
            var package = new EAPackage(this) {Name = name, PackageID = id, ParentID = parentId};
            packagesById[id] = package;
            if (!package.IsModel)
            {
                ((EAElement)package.Element).ElementID = idFactory.NextID;
                elementsById[package.Element.ElementID] = (EAElement) package.Element;
            }
            return package;
        }

        public EAConnectorTag CreateConnectorTag(string name, string type, int connectorId)
        {
            return new EAConnectorTag {Name = name, TagID = idFactory.NextID, ConnectorID = connectorId};
        }

        public static EAAttributeTag CreateAttributeTag(string name, string type, int attributeId)
        {
            return new EAAttributeTag {Name = name, AttributeID = attributeId};
        }

        public static EADiagramObject CreateDiagramObject(string name, string type, int diagramId)
        {
            return new EADiagramObject {Name = name, DiagramID = diagramId};
        }

        public static IEACollectionElement CreateTaggedValue(string name, string type, int elementId)
        {
            return new EATaggedValue {Name = name, ElementID = elementId};
        }

        public IEACollectionElement CreateAttribute(string name, string type, int elementId)
        {
            return new EAAttribute(this, elementId) {Name = name};
        }

        public IEACollectionElement CreateConnector(string name, string type, int clientId)
        {
            return new EAConnector(this) {Name = name, Type = type, ClientID = clientId};
        }

        public IEACollectionElement CreateElement(string name, string type, int packageId)
        {
            int id = idFactory.NextID;
            var element = new EAElement(this) {Name = name, Type = type, ElementID = id, PackageID = packageId};
            elementsById[id] = element;
            return element;
        }

        public IEACollectionElement CreateDiagram(string name, string type, int packageId)
        {
            return new EADiagram(this) {Name = name, Type = type, PackageID = packageId};
        }

        internal void AddConnector(EAConnector connector)
        {
            connectors.Add(connector);
        }

        internal void RemoveConnector(EAConnector connector)
        {
            connectors.Remove(connector);
        }

        public static Action<Element> ElementStereotype(string stereotype)
        {
            return element => { element.Stereotype = stereotype; };
        }

        protected static Action<Element> BCCs(Element type, params string[] names)
        {
            return element => element.AddBCCs(type, names);
        }

        protected static Action<Element> BBIEs(Element type, params string[] names)
        {
            return element => element.AddBBIEs(type, names);
        }

        protected static Action<Element> CON(Element type)
        {
            return element => element.AddCON(type);
        }

        protected static Action<Element> SUPs(Element type, params string[] names)
        {
            return element => element.AddSUPs(type, names);
        }

        protected static Action<Element> TaggedValue(TaggedValues key, string value)
        {
            return element => element.AddTaggedValue(key.ToString()).WithValue(value);
        }
    }
}