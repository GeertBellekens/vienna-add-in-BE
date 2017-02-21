// ReSharper disable RedundantUsingDirective
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.BLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using CctsRepository.DocLibrary;
using CctsRepository.EnumLibrary;
using CctsRepository.PrimLibrary;
using VIENNAAddIn.upcc3.repo;
using VIENNAAddIn.upcc3.repo.BdtLibrary;
using VIENNAAddIn.upcc3.repo.BieLibrary;
using VIENNAAddIn.upcc3.repo.BLibrary;
using VIENNAAddIn.upcc3.repo.CcLibrary;
using VIENNAAddIn.upcc3.repo.CdtLibrary;
using VIENNAAddIn.upcc3.repo.DocLibrary;
using VIENNAAddIn.upcc3.repo.EnumLibrary;
using VIENNAAddIn.upcc3.repo.PrimLibrary;
// ReSharper restore RedundantUsingDirective
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.repo.BdtLibrary
{
    internal static partial class BdtLibrarySpecConverter
    {
		internal static UmlPackageSpec Convert(BdtLibrarySpec bdtLibrarySpec)
		{
			var umlPackageSpec = new UmlPackageSpec
				{
					Stereotype = "BDTLibrary",
					Name = bdtLibrarySpec.Name,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("businessTerm", bdtLibrarySpec.BusinessTerms) ,
							new UmlTaggedValueSpec("copyright", bdtLibrarySpec.Copyrights) ,
							new UmlTaggedValueSpec("owner", bdtLibrarySpec.Owners) ,
							new UmlTaggedValueSpec("reference", bdtLibrarySpec.References) ,
							new UmlTaggedValueSpec("status", bdtLibrarySpec.Status) ,
							new UmlTaggedValueSpec("uniqueIdentifier", bdtLibrarySpec.UniqueIdentifier) { DefaultValue = GenerateUniqueIdentifierDefaultValue(bdtLibrarySpec) },
							new UmlTaggedValueSpec("versionIdentifier", bdtLibrarySpec.VersionIdentifier) ,
							new UmlTaggedValueSpec("baseURN", bdtLibrarySpec.BaseURN) ,
							new UmlTaggedValueSpec("namespacePrefix", bdtLibrarySpec.NamespacePrefix) ,
						},
					DiagramType = UmlDiagramType.Class,
				};

			return umlPackageSpec;
		}
	}
}

