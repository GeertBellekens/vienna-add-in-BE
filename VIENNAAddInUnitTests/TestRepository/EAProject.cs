using System;
using EA;

namespace VIENNAAddInUnitTests.TestRepository
{
    internal class EAProject : Project
    {
        public bool LoadProject(string FileName)
        {
            throw new NotImplementedException();
        }

        public bool ReloadProject()
        {
            throw new NotImplementedException();
        }

        public bool LoadDiagram(string DiagramGUID)
        {
            throw new NotImplementedException();
        }

        public string SaveDiagramImageToFile(string FileName)
        {
            throw new NotImplementedException();
        }

        public string GetElement(string ElementGUID)
        {
            throw new NotImplementedException();
        }

        public string EnumViews()
        {
            throw new NotImplementedException();
        }

        public string EnumPackages(string PackageGUID)
        {
            throw new NotImplementedException();
        }

        public string EnumElements(string PackageGUID)
        {
            throw new NotImplementedException();
        }

        public string EnumLinks(string PackageID)
        {
            throw new NotImplementedException();
        }

        public string EnumDiagrams(string PackageGUID)
        {
            throw new NotImplementedException();
        }

        public string EnumDiagramElements(string DiagramGUID)
        {
            throw new NotImplementedException();
        }

        public string EnumDiagramLinks(string DiagramID)
        {
            throw new NotImplementedException();
        }

        public string GetLink(string LinkGUID)
        {
            throw new NotImplementedException();
        }

        public string GetDiagram(string DiagramGUID)
        {
            throw new NotImplementedException();
        }

        public string GetElementConstraints(string ElementGUID)
        {
            throw new NotImplementedException();
        }

        public string GetElementEffort(string ElementGUID)
        {
            throw new NotImplementedException();
        }

        public string GetElementMetrics(string ElementGUID)
        {
            throw new NotImplementedException();
        }

        public string GetElementFiles(string ElementGUID)
        {
            throw new NotImplementedException();
        }

        public string GetElementRequirements(string ElementGUID)
        {
            throw new NotImplementedException();
        }

        public string GetElementProblems(string ElementGUID)
        {
            throw new NotImplementedException();
        }

        public string GetElementResources(string ElementGUID)
        {
            throw new NotImplementedException();
        }

        public string GetElementRisks(string ElementGUID)
        {
            throw new NotImplementedException();
        }

        public string GetElementScenarios(string ElementGUID)
        {
            throw new NotImplementedException();
        }

        public string GetElementTests(string ElementGUID)
        {
            throw new NotImplementedException();
        }

        public void ShowWindow(int Show)
        {
            throw new NotImplementedException();
        }

        public void Exit()
        {
            throw new NotImplementedException();
        }

        public bool PutDiagramImageOnClipboard(string DiagramGUID, int Type)
        {
            throw new NotImplementedException();
        }

        public bool PutDiagramImageToFile(string DiagramGUID, string FilePath, int Type)
        {
            throw new NotImplementedException();
        }

        public string ExportPackageXMI(string PackageGUID, EnumXMIType XMIType, int DiagramXML, int DiagramImage, int FormatXML, int UseDTD, string FileName)
        {
            throw new NotImplementedException();
        }

        public string EnumProjects()
        {
            throw new NotImplementedException();
        }

        public string EnumViewEx(string ProjectGUID)
        {
            throw new NotImplementedException();
        }

        public void RunReport(string PackageGUID, string TemplateName, string FileName)
        {
            throw new NotImplementedException();
        }

        public string GetLastError()
        {
            throw new NotImplementedException();
        }

        public string GetElementProperties(string ElementGUID)
        {
            throw new NotImplementedException();
        }

        public string GUIDtoXML(string GUID)
        {
            throw new NotImplementedException();
        }

        public string XMLtoGUID(string GUID)
        {
            throw new NotImplementedException();
        }

        public void RunHTMLReport(string PackageGUID, string ExportPath, string ImageFormat, string Style, string Extension)
        {
            throw new NotImplementedException();
        }

        public string ImportPackageXMI(string PackageGUID, string FileName, int ImportDiagrams, int StripGUID)
        {
            throw new NotImplementedException();
        }

        public string SaveControlledPackage(string PackageGUID)
        {
            throw new NotImplementedException();
        }

        public string LoadControlledPackage(string PackageGUID)
        {
            throw new NotImplementedException();
        }

        public bool LayoutDiagram(string DiagramGUID, int LayoutStyle)
        {
            throw new NotImplementedException();
        }

        public bool GenerateXSD(string PackageGUID, string FileName, string Encoding, string Options)
        {
            throw new NotImplementedException();
        }

        public bool LayoutDiagramEx(string DiagramGUID, int LayoutStyle, int Iterations, int LayerSpacing, int ColumnSpacing, bool SavetoDiagram)
        {
            throw new NotImplementedException();
        }

        public string DefineRule(string CategoryID, EnumMVErrorType Severity, string ErrorMsg)
        {
            throw new NotImplementedException();
        }

        public string DefineRuleCategory(string Description)
        {
            throw new NotImplementedException();
        }

        public bool PublishResult(string RuleID, EnumMVErrorType Severity, string ErrorMsg)
        {
            throw new NotImplementedException();
        }

        public bool ValidateElement(string ElementGUID)
        {
            throw new NotImplementedException();
        }

        public bool ValidatePackage(string PackageGUID)
        {
            throw new NotImplementedException();
        }

        public bool ValidateDiagram(string DiagramGUID)
        {
            throw new NotImplementedException();
        }

        public bool IsValidating()
        {
            throw new NotImplementedException();
        }

        public bool CanValidate()
        {
            throw new NotImplementedException();
        }

        public void CancelValidation()
        {
            throw new NotImplementedException();
        }

        public void RunModelSearch(string QueryName, string SearchTerm, bool ShowInEA)
        {
            throw new NotImplementedException();
        }

        public bool GenerateClass(string ElementGUID, string ExtraOptions)
        {
            throw new NotImplementedException();
        }

        public bool GeneratePackage(string PackageGUID, string ExtraOptions)
        {
            throw new NotImplementedException();
        }

        public bool TransformElement(string transformName, string ElementGUID, string TargetPackageGUID, string ExtraOptions)
        {
            throw new NotImplementedException();
        }

        public bool TransformPackage(string transformName, string SourcePackageGUID, string TargetPackageGUID, string ExtraOptions)
        {
            throw new NotImplementedException();
        }

        public bool SynchronizeClass(string ElementGUID, string ExtraOptions)
        {
            throw new NotImplementedException();
        }

        public bool SynchronizePackage(string PackageGUID, string ExtraOptions)
        {
            throw new NotImplementedException();
        }

        public bool ImportDirectory(string PackageGUID, string Language, string DirectoryPath, string ExtraOptions)
        {
            throw new NotImplementedException();
        }

        public bool ImportFile(string PackageGUID, string Language, string FileName, string ExtraOptions)
        {
            throw new NotImplementedException();
        }

        public string DoBaselineCompare(string PackageGUID, string Baseline, string ConnectString)
        {
            throw new NotImplementedException();
        }

        public string DoBaselineMerge(string PackageGUID, string Baseline, string MergeInstructions, string ConnectString)
        {
            throw new NotImplementedException();
        }

        public string GetBaselines(string PackageGUID, string ConnectString)
        {
            throw new NotImplementedException();
        }

        public bool CreateBaseline(string PackageGUID, string Version, string Notes)
        {
            throw new NotImplementedException();
        }

        public bool DeleteBaseline(string Baseline)
        {
            throw new NotImplementedException();
        }

        public void MigrateToBPMN11(string GUID, string Type)
        {
            throw new System.NotImplementedException();
        }

        public string ExportPackageXMIEx(string PackageGUID, EnumXMIType XMIType, int DiagramXML, int DiagramImage, int FormatXML, int UseDTD, string FileName, int Flags)
        {
            throw new System.NotImplementedException();
        }

        public bool CreateBaselineEx(string PackageGUID, string Version, string Notes, int Flags)
        {
            throw new System.NotImplementedException();
        }

        public bool GenerateDiagramFromScenario(string ElementGUID, EnumScenarioDiagramType DiagramType, int Options)
        {
            throw new System.NotImplementedException();
        }

        public bool GenerateTestFromScenario(string ElementGUID, EnumScenarioTestType TestType)
        {
            throw new System.NotImplementedException();
        }

        public ObjectType ObjectType
        {
            get { throw new NotImplementedException(); }
        }
    }
}