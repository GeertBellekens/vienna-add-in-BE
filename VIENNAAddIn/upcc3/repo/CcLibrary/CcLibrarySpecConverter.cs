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

namespace VIENNAAddIn.upcc3.repo.CcLibrary
{
    internal static partial class CcLibrarySpecConverter
    {
		internal static UmlPackageSpec Convert(CcLibrarySpec ccLibrarySpec)
		{
			var umlPackageSpec = new UmlPackageSpec
				{
					Stereotype = "CCLibrary",
					Name = ccLibrarySpec.Name,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("businessTerm", ccLibrarySpec.BusinessTerms) ,
							new UmlTaggedValueSpec("copyright", ccLibrarySpec.Copyrights) ,
							new UmlTaggedValueSpec("owner", ccLibrarySpec.Owners) ,
							new UmlTaggedValueSpec("reference", ccLibrarySpec.References) ,
							new UmlTaggedValueSpec("status", ccLibrarySpec.Status) ,
							new UmlTaggedValueSpec("uniqueIdentifier", ccLibrarySpec.UniqueIdentifier) { DefaultValue = GenerateUniqueIdentifierDefaultValue(ccLibrarySpec) },
							new UmlTaggedValueSpec("versionIdentifier", ccLibrarySpec.VersionIdentifier) ,
							new UmlTaggedValueSpec("baseURN", ccLibrarySpec.BaseURN) ,
							new UmlTaggedValueSpec("namespacePrefix", ccLibrarySpec.NamespacePrefix) ,
						},
					DiagramType = UmlDiagramType.Class,
				};

			return umlPackageSpec;
		}
	}
}

