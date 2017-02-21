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

namespace VIENNAAddIn.upcc3.repo.EnumLibrary
{
    internal static partial class EnumLibrarySpecConverter
    {
		internal static UmlPackageSpec Convert(EnumLibrarySpec enumLibrarySpec)
		{
			var umlPackageSpec = new UmlPackageSpec
				{
					Stereotype = "ENUMLibrary",
					Name = enumLibrarySpec.Name,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("businessTerm", enumLibrarySpec.BusinessTerms) ,
							new UmlTaggedValueSpec("copyright", enumLibrarySpec.Copyrights) ,
							new UmlTaggedValueSpec("owner", enumLibrarySpec.Owners) ,
							new UmlTaggedValueSpec("reference", enumLibrarySpec.References) ,
							new UmlTaggedValueSpec("status", enumLibrarySpec.Status) ,
							new UmlTaggedValueSpec("uniqueIdentifier", enumLibrarySpec.UniqueIdentifier) { DefaultValue = GenerateUniqueIdentifierDefaultValue(enumLibrarySpec) },
							new UmlTaggedValueSpec("versionIdentifier", enumLibrarySpec.VersionIdentifier) ,
							new UmlTaggedValueSpec("baseURN", enumLibrarySpec.BaseURN) ,
							new UmlTaggedValueSpec("namespacePrefix", enumLibrarySpec.NamespacePrefix) ,
						},
					DiagramType = UmlDiagramType.Class,
				};

			return umlPackageSpec;
		}
	}
}

