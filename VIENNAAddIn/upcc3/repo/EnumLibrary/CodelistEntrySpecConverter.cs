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
    internal static partial class CodelistEntrySpecConverter
    {
		internal static UmlEnumerationLiteralSpec Convert(CodelistEntrySpec codelistEntrySpec)
		{
			var umlEnumerationLiteralSpec = new UmlEnumerationLiteralSpec
				{
					Stereotype = "CodelistEntry",
					Name = codelistEntrySpec.Name,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("codeName", codelistEntrySpec.CodeName) ,
							new UmlTaggedValueSpec("status", codelistEntrySpec.Status) ,
						},
	
				};

			return umlEnumerationLiteralSpec;
		}
	}
}

