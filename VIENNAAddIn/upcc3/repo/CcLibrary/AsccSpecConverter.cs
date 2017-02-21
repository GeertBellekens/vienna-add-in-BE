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
    internal static partial class AsccSpecConverter
    {
		internal static UmlAssociationSpec Convert(AsccSpec asccSpec, string associatingClassName)
		{
			var associatedClassifierType = ((UpccAcc) asccSpec.AssociatedAcc).UmlClass;
			var umlAssociationSpec = new UmlAssociationSpec
				{
					Stereotype = "ASCC",
					Name = asccSpec.Name,
					UpperBound = asccSpec.UpperBound,
					LowerBound = asccSpec.LowerBound,
					AggregationKind = AggregationKind.Shared,
					AssociatedClassifier = associatedClassifierType,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("businessTerm", asccSpec.BusinessTerms) ,
							new UmlTaggedValueSpec("definition", asccSpec.Definition) ,
							new UmlTaggedValueSpec("dictionaryEntryName", asccSpec.DictionaryEntryName) { DefaultValue = GenerateDictionaryEntryNameDefaultValue(asccSpec, associatingClassName) },
							new UmlTaggedValueSpec("languageCode", asccSpec.LanguageCode) ,
							new UmlTaggedValueSpec("sequencingKey", asccSpec.SequencingKey) ,
							new UmlTaggedValueSpec("uniqueIdentifier", asccSpec.UniqueIdentifier) { DefaultValue = GenerateUniqueIdentifierDefaultValue(asccSpec, associatingClassName) },
							new UmlTaggedValueSpec("versionIdentifier", asccSpec.VersionIdentifier) ,
							new UmlTaggedValueSpec("usageRule", asccSpec.UsageRules) ,
						},
				};

			return umlAssociationSpec;
		}
	}
}

