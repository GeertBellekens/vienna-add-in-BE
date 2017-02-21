using System;
using System.IO;
using EA;
using VIENNAAddIn.Utils;
using Attribute=EA.Attribute;
using File=System.IO.File;

namespace VIENNAAddInUnitTests.TestRepository
{
    public class TemporaryFileBasedRepository : Repository, IDisposable
    {
        private readonly Repository repo = new Repository();
        private string tempFileName;

        /// <summary>
        /// Copy the contents of the given repository to a new temporary file-based repository.
        /// </summary>
        /// <param name="repository"></param>
        public TemporaryFileBasedRepository(Repository repository) : this()
        {
            RepositoryCopier.CopyRepository(repository, repo);
        }

        /// <summary>
        /// Create an empty temporary file-based repository.
        /// </summary>
        public TemporaryFileBasedRepository()
        {
            Console.WriteLine("Creating temporary file-based repository: \"{0}\"", TempFileName);
            repo.CreateModel(CreateModelType.cmEAPFromBase, TempFileName, 0);
            repo.OpenFile(TempFileName);
        }

        /// <summary>
        /// Copy the given repository file to a temporary file and open it.
        /// </summary>
        /// <param name="repositoryFilePath"></param>
        public TemporaryFileBasedRepository(string repositoryFilePath)
        {
            Console.Write("Creating temporary file-based repository: \"{0}\" ... ", TempFileName);
            File.Copy(repositoryFilePath, TempFileName, true);
            repo.OpenFile(TempFileName);
            Console.WriteLine("done");
        }

        private string TempFileName
        {
            get
            {
                if (tempFileName == null)
                {
                    tempFileName = Path.GetTempFileName();
                    tempFileName = TempFileName.Substring(0, TempFileName.Length - Path.GetExtension(TempFileName).Length - 1) + ".eap";
                }
                return tempFileName;
            }
        }

        public void CloseButKeepFile()
        {
            repo.CloseFile();
        }

        #region IDisposable Members

        /// <summary>
        /// Close the repository and delete the temporary file.
        /// </summary>
        public void Dispose()
        {
            Console.WriteLine("Deleting temporary file-based repository: \"{0}\" ... ", TempFileName);
            repo.CloseFile();
            File.Delete(TempFileName);
            Console.WriteLine("done");
        }

        #endregion

        #region Repository Members

        public bool OpenFile(string FilePath)
        {
            return repo.OpenFile(FilePath);
        }

        public void Exit()
        {
            repo.Exit();
        }

        public Element GetElementByID(int ElementID)
        {
            return repo.GetElementByID(ElementID);
        }

        public Package GetPackageByID(int PackageID)
        {
            return repo.GetPackageByID(PackageID);
        }

        public string GetLastError()
        {
            return repo.GetLastError();
        }

        public Diagram GetDiagramByID(int DiagramID)
        {
            return repo.GetDiagramByID(DiagramID);
        }

        public Reference GetReferenceList(string ListName)
        {
            return repo.GetReferenceList(ListName);
        }

        public Project GetProjectInterface()
        {
            return repo.GetProjectInterface();
        }

        public bool OpenFile2(string FilePath, string Username, string Password)
        {
            return repo.OpenFile2(FilePath, Username, Password);
        }

        public void ShowWindow(int Show)
        {
            repo.ShowWindow(Show);
        }

        public Diagram GetCurrentDiagram()
        {
            return repo.GetCurrentDiagram();
        }

        public Connector GetConnectorByID(int ConnectorID)
        {
            return repo.GetConnectorByID(ConnectorID);
        }

        public ObjectType GetTreeSelectedItem(out object Item)
        {
            return repo.GetTreeSelectedItem(out Item);
        }

        public Package GetTreeSelectedPackage()
        {
            return repo.GetTreeSelectedPackage();
        }

        public void CloseAddins()
        {
            repo.CloseAddins();
        }

        public Package GetPackageByGuid(string GUID)
        {
            return repo.GetPackageByGuid(GUID);
        }

        public void AdviseElementChange(int ElementID)
        {
            repo.AdviseElementChange(ElementID);
        }

        public void AdviseConnectorChange(int ConnectorID)
        {
            repo.AdviseConnectorChange(ConnectorID);
        }

        public void OpenDiagram(int DiagramID)
        {
            repo.OpenDiagram(DiagramID);
        }

        public void CloseDiagram(int DiagramID)
        {
            repo.CloseDiagram(DiagramID);
        }

        public void ActivateDiagram(int DiagramID)
        {
            repo.ActivateDiagram(DiagramID);
        }

        public void SaveDiagram(int DiagramID)
        {
            repo.SaveDiagram(DiagramID);
        }

        public void ReloadDiagram(int DiagramID)
        {
            repo.ReloadDiagram(DiagramID);
        }

        public void CloseFile()
        {
            repo.CloseFile();
        }

        public int __TempDebug(int No, DateTime No2, out int pNo3)
        {
            return repo.__TempDebug(No, No2, out pNo3);
        }

        public ObjectType GetTreeSelectedItemType()
        {
            return repo.GetTreeSelectedItemType();
        }

        public string GetTechnologyVersion(string ID)
        {
            return repo.GetTechnologyVersion(ID);
        }

        public bool IsTechnologyLoaded(string ID)
        {
            return repo.IsTechnologyLoaded(ID);
        }

        public bool ImportTechnology(string Technology)
        {
            return repo.ImportTechnology(Technology);
        }

        public string GetCounts()
        {
            return repo.GetCounts();
        }

        public Collection GetElementSet(string IDList, int Unused)
        {
            return repo.GetElementSet(IDList, Unused);
        }

        public bool DeleteTechnology(string ID)
        {
            return repo.DeleteTechnology(ID);
        }

        public object AddTab(string TabName, string ControlID)
        {
            return repo.AddTab(TabName, ControlID);
        }

        public void CreateOutputTab(string Name)
        {
            repo.CreateOutputTab(Name);
        }

        public void RemoveOutputTab(string Name)
        {
            repo.RemoveOutputTab(Name);
        }

        public void WriteOutput(string Name, string String, int ID)
        {
            repo.WriteOutput(Name, String, ID);
        }

        public void ClearOutput(string Name)
        {
            repo.ClearOutput(Name);
        }

        public void EnsureOutputVisible(string Name)
        {
            repo.EnsureOutputVisible(Name);
        }

        public Element GetElementByGuid(string GUID)
        {
            return repo.GetElementByGuid(GUID);
        }

        public Connector GetConnectorByGuid(string GUID)
        {
            return repo.GetConnectorByGuid(GUID);
        }

        public void ShowDynamicHelp(string Topic)
        {
            repo.ShowDynamicHelp(Topic);
        }

        public ObjectType GetContextItem(out object Item)
        {
            return repo.GetContextItem(out Item);
        }

        public ObjectType GetContextItemType()
        {
            return repo.GetContextItemType();
        }

        public void RefreshModelView(int PackageID)
        {
            repo.RefreshModelView(PackageID);
        }

        public void ActivateTab(string Name)
        {
            repo.ActivateTab(Name);
        }

        public void ShowInProjectView(object Object)
        {
            repo.ShowInProjectView(Object);
        }

        public object GetDiagramByGuid(string GUID)
        {
            return repo.GetDiagramByGuid(GUID);
        }

        public string GetCurrentLoginUser(bool GetGuid)
        {
            return repo.GetCurrentLoginUser(GetGuid);
        }

        public bool ChangeLoginUser(string Name, string Password)
        {
            return repo.ChangeLoginUser(Name, Password);
        }

        public object GetTreeSelectedObject()
        {
            return repo.GetTreeSelectedObject();
        }

        public void Execute(string SQL)
        {
            repo.Execute(SQL);
        }

        public void ShowProfileToolbox(string Technology, string Profile, bool Show)
        {
            repo.ShowProfileToolbox(Technology, Profile, Show);
        }

        public void SetUIPerspective(string Perspective)
        {
            repo.SetUIPerspective(Perspective);
        }

        public bool ActivatePerspective(string Perspective, int Options)
        {
            return repo.ActivatePerspective(Perspective, Options);
        }

        public bool AddPerspective(string Perspective, int Options)
        {
            return repo.AddPerspective(Perspective, Options);
        }

        public bool DeletePerspective(string Perspective, int Options)
        {
            return repo.DeletePerspective(Perspective, Options);
        }

        public string GetActivePerspective()
        {
            return repo.GetActivePerspective();
        }

        public string HasPerspective(string Perspective)
        {
            return repo.HasPerspective(Perspective);
        }

        public bool CreateModel(CreateModelType CreateType, string FilePath, int ParentWnd)
        {
            return repo.CreateModel(CreateType, FilePath, ParentWnd);
        }

        public Attribute GetAttributeByGuid(string GUID)
        {
            return repo.GetAttributeByGuid(GUID);
        }

        public Method GetMethodByGuid(string GUID)
        {
            return repo.GetMethodByGuid(GUID);
        }

        public void RemoveTab(string Name)
        {
            repo.RemoveTab(Name);
        }

        public string CustomCommand(string ClassName, string MethodName, string Parameters)
        {
            return repo.CustomCommand(ClassName, MethodName, Parameters);
        }

        public int IsTabOpen(string TabName)
        {
            return repo.IsTabOpen(TabName);
        }

        public bool AddDefinedSearches(string sXML)
        {
            return repo.AddDefinedSearches(sXML);
        }

        public void ShowBrowser(string TabName, string URL)
        {
            repo.ShowBrowser(TabName, URL);
        }

        public Collection GetElementsByQuery(string QueryName, string SearchTerm)
        {
            return repo.GetElementsByQuery(QueryName, SearchTerm);
        }

        public void RunModelSearch(string QueryName, string SearchTerm, string SearchOptions, string SearchData)
        {
            repo.RunModelSearch(QueryName, SearchTerm, SearchOptions, SearchData);
        }

        public string SQLQuery(string SQL)
        {
            return repo.SQLQuery(SQL);
        }

        public string GetTreeXML(int RootPackageID)
        {
            return repo.GetTreeXML(RootPackageID);
        }

        public string GetTreeXMLByGUID(string GUID)
        {
            return repo.GetTreeXMLByGUID(GUID);
        }

        public void RefreshOpenDiagrams(bool FullReload)
        {
            repo.RefreshOpenDiagrams(FullReload);
        }

        public string GetTreeXMLForElement(int ElementID)
        {
            return repo.GetTreeXMLForElement(ElementID);
        }

        public void ImportPackageBuildScripts(string PackageGUID, string BuildScriptXML)
        {
            repo.ImportPackageBuildScripts(PackageGUID, BuildScriptXML);
        }

        public void ExecutePackageBuildScript(int ScriptOptions, string PackageGUID)
        {
            repo.ExecutePackageBuildScript(ScriptOptions, PackageGUID);
        }

        public bool ActivateToolbox(string Toolbox, int Options)
        {
            return repo.ActivateToolbox(Toolbox, Options);
        }

        public bool SaveAuditLogs(string FilePath, object StateDateTime, object EndDateTime)
        {
            return repo.SaveAuditLogs(FilePath, StateDateTime, EndDateTime);
        }

        public bool ClearAuditLogs(object StateDateTime, object EndDateTime)
        {
            return repo.ClearAuditLogs(StateDateTime, EndDateTime);
        }

        public ModelWatcher CreateModelWatcher()
        {
            return repo.CreateModelWatcher();
        }

        public bool ActivateTechnology(string ID)
        {
            return repo.ActivateTechnology(ID);
        }

        public void SaveAllDiagrams()
        {
            repo.SaveAllDiagrams();
        }

        public string GetFormatFromField(string Format, string Text)
        {
            return repo.GetFormatFromField(Format, Text);
        }

        public string GetFieldFromFormat(string Format, string Text)
        {
            return repo.GetFieldFromFormat(Format, Text);
        }

        public bool IsTechnologyEnabled(string ID)
        {
            return repo.IsTechnologyEnabled(ID);
        }

        public Attribute GetAttributeByID(int AttributeID)
        {
            return repo.GetAttributeByID(AttributeID);
        }

        public Method GetMethodByID(int MethodID)
        {
            return repo.GetMethodByID(MethodID);
        }

        public object GetContextObject()
        {
            return repo.GetContextObject();
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
            get { return repo.Models; }
        }

        public Collection Terms
        {
            get { return repo.Terms; }
        }

        public Collection Issues
        {
            get { return repo.Issues; }
        }

        public Collection Authors
        {
            get { return repo.Authors; }
        }

        public Collection Clients
        {
            get { return repo.Clients; }
        }

        public Collection Tasks
        {
            get { return repo.Tasks; }
        }

        public Collection Datatypes
        {
            get { return repo.Datatypes; }
        }

        public Collection Resources
        {
            get { return repo.Resources; }
        }

        public Collection Stereotypes
        {
            get { return repo.Stereotypes; }
        }

        public ObjectType ObjectType
        {
            get { return repo.ObjectType; }
        }

        public int LibraryVersion
        {
            get { return repo.LibraryVersion; }
        }

        public string LastUpdate
        {
            get { return repo.LastUpdate; }
        }

        public bool FlagUpdate
        {
            get { return repo.FlagUpdate; }
            set { repo.FlagUpdate = value; }
        }

        public string InstanceGUID
        {
            get { return repo.InstanceGUID; }
        }

        public string ConnectionString
        {
            get { return repo.ConnectionString; }
        }

        public Collection PropertyTypes
        {
            get { return repo.PropertyTypes; }
        }

        public bool EnableUIUpdates
        {
            get { return repo.EnableUIUpdates; }
            set { repo.EnableUIUpdates = value; }
        }

        public bool BatchAppend
        {
            get { return repo.BatchAppend; }
            set { repo.BatchAppend = value; }
        }

        public bool IsSecurityEnabled
        {
            get { return repo.IsSecurityEnabled; }
        }

        public App App
        {
            get { return repo.App; }
        }

        public EAEditionTypes EAEdition
        {
            get { return repo.EAEdition; }
        }

        public bool SuppressEADialogs
        {
            get { return repo.SuppressEADialogs; }
            set { repo.SuppressEADialogs = value; }
        }

        public bool EnableCache
        {
            get { return repo.EnableCache; }
            set { repo.EnableCache = value; }
        }

        public int EnableEventFlags
        {
            get { return repo.EnableEventFlags; }
            set { repo.EnableEventFlags = value; }
        }

        public string ProjectGUID
        {
            get { return repo.ProjectGUID; }
        }

        public bool SuppressSecurityDialog
        {
            get { return repo.SuppressSecurityDialog; }
            set { repo.SuppressSecurityDialog = value; }
        }

        public EAEditionTypes EAEditionEx
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}