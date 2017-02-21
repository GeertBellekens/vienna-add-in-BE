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

namespace VIENNAAddIn.upcc3.repo.BieLibrary
{
    internal static partial class BieLibrarySpecConverter
    {
		internal static UmlPackageSpec Convert(BieLibrarySpec bieLibrarySpec)
		{
			var umlPackageSpec = new UmlPackageSpec
				{
					Stereotype = "BIELibrary",
					Name = bieLibrarySpec.Name,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("businessTerm", bieLibrarySpec.BusinessTerms) ,
							new UmlTaggedValueSpec("copyright", bieLibrarySpec.Copyrights) ,
							new UmlTaggedValueSpec("owner", bieLibrarySpec.Owners) ,
							new UmlTaggedValueSpec("reference", bieLibrarySpec.References) ,
							new UmlTaggedValueSpec("status", bieLibrarySpec.Status) ,
							new UmlTaggedValueSpec("uniqueIdentifier", bieLibrarySpec.UniqueIdentifier) { DefaultValue = GenerateUniqueIdentifierDefaultValue(bieLibrarySpec) },
							new UmlTaggedValueSpec("versionIdentifier", bieLibrarySpec.VersionIdentifier) ,
							new UmlTaggedValueSpec("baseURN", bieLibrarySpec.BaseURN) ,
							new UmlTaggedValueSpec("namespacePrefix", bieLibrarySpec.NamespacePrefix) ,
						},
					DiagramType = UmlDiagramType.Class,
				};

			return umlPackageSpec;
		}
	}
}

