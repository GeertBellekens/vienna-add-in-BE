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

namespace VIENNAAddIn.upcc3.repo.DocLibrary
{
    internal static partial class DocLibrarySpecConverter
    {
		internal static UmlPackageSpec Convert(DocLibrarySpec docLibrarySpec)
		{
			var umlPackageSpec = new UmlPackageSpec
				{
					Stereotype = "DOCLibrary",
					Name = docLibrarySpec.Name,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("businessTerm", docLibrarySpec.BusinessTerms) ,
							new UmlTaggedValueSpec("copyright", docLibrarySpec.Copyrights) ,
							new UmlTaggedValueSpec("owner", docLibrarySpec.Owners) ,
							new UmlTaggedValueSpec("reference", docLibrarySpec.References) ,
							new UmlTaggedValueSpec("status", docLibrarySpec.Status) ,
							new UmlTaggedValueSpec("uniqueIdentifier", docLibrarySpec.UniqueIdentifier) { DefaultValue = GenerateUniqueIdentifierDefaultValue(docLibrarySpec) },
							new UmlTaggedValueSpec("versionIdentifier", docLibrarySpec.VersionIdentifier) ,
							new UmlTaggedValueSpec("baseURN", docLibrarySpec.BaseURN) ,
							new UmlTaggedValueSpec("namespacePrefix", docLibrarySpec.NamespacePrefix) ,
						},
					DiagramType = UmlDiagramType.Class,
				};

			return umlPackageSpec;
		}
	}
}

