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

namespace VIENNAAddIn.upcc3.repo.PrimLibrary
{
    internal static partial class PrimLibrarySpecConverter
    {
		internal static UmlPackageSpec Convert(PrimLibrarySpec primLibrarySpec)
		{
			var umlPackageSpec = new UmlPackageSpec
				{
					Stereotype = "PRIMLibrary",
					Name = primLibrarySpec.Name,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("businessTerm", primLibrarySpec.BusinessTerms) ,
							new UmlTaggedValueSpec("copyright", primLibrarySpec.Copyrights) ,
							new UmlTaggedValueSpec("owner", primLibrarySpec.Owners) ,
							new UmlTaggedValueSpec("reference", primLibrarySpec.References) ,
							new UmlTaggedValueSpec("status", primLibrarySpec.Status) ,
							new UmlTaggedValueSpec("uniqueIdentifier", primLibrarySpec.UniqueIdentifier) { DefaultValue = GenerateUniqueIdentifierDefaultValue(primLibrarySpec) },
							new UmlTaggedValueSpec("versionIdentifier", primLibrarySpec.VersionIdentifier) ,
							new UmlTaggedValueSpec("baseURN", primLibrarySpec.BaseURN) ,
							new UmlTaggedValueSpec("namespacePrefix", primLibrarySpec.NamespacePrefix) ,
						},
					DiagramType = UmlDiagramType.Class,
				};

			return umlPackageSpec;
		}
	}
}

