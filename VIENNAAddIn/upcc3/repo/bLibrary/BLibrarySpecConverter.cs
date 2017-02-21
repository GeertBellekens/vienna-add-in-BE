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

namespace VIENNAAddIn.upcc3.repo.BLibrary
{
    internal static partial class BLibrarySpecConverter
    {
		internal static UmlPackageSpec Convert(BLibrarySpec bLibrarySpec)
		{
			var umlPackageSpec = new UmlPackageSpec
				{
					Stereotype = "bLibrary",
					Name = bLibrarySpec.Name,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("businessTerm", bLibrarySpec.BusinessTerms) ,
							new UmlTaggedValueSpec("copyright", bLibrarySpec.Copyrights) ,
							new UmlTaggedValueSpec("owner", bLibrarySpec.Owners) ,
							new UmlTaggedValueSpec("reference", bLibrarySpec.References) ,
							new UmlTaggedValueSpec("status", bLibrarySpec.Status) ,
							new UmlTaggedValueSpec("uniqueIdentifier", bLibrarySpec.UniqueIdentifier) { DefaultValue = GenerateUniqueIdentifierDefaultValue(bLibrarySpec) },
							new UmlTaggedValueSpec("versionIdentifier", bLibrarySpec.VersionIdentifier) ,
						},
					DiagramType = UmlDiagramType.Package,
				};

			return umlPackageSpec;
		}
	}
}

