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
    internal static partial class AsbieSpecConverter
    {
		internal static UmlAssociationSpec Convert(AsbieSpec asbieSpec, string associatingClassName)
		{
			var associatedClassifierType = ((UpccAbie) asbieSpec.AssociatedAbie).UmlClass;
			var umlAssociationSpec = new UmlAssociationSpec
				{
					Stereotype = "ASBIE",
					Name = asbieSpec.Name,
					UpperBound = asbieSpec.UpperBound,
					LowerBound = asbieSpec.LowerBound,
					AggregationKind = asbieSpec.AggregationKind == AggregationKind.Shared ? AggregationKind.Shared : AggregationKind.Composite,
					AssociatedClassifier = associatedClassifierType,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("businessTerm", asbieSpec.BusinessTerms) ,
							new UmlTaggedValueSpec("definition", asbieSpec.Definition) ,
							new UmlTaggedValueSpec("dictionaryEntryName", asbieSpec.DictionaryEntryName) { DefaultValue = GenerateDictionaryEntryNameDefaultValue(asbieSpec, associatingClassName) },
							new UmlTaggedValueSpec("languageCode", asbieSpec.LanguageCode) ,
							new UmlTaggedValueSpec("sequencingKey", asbieSpec.SequencingKey) ,
							new UmlTaggedValueSpec("uniqueIdentifier", asbieSpec.UniqueIdentifier) { DefaultValue = GenerateUniqueIdentifierDefaultValue(asbieSpec, associatingClassName) },
							new UmlTaggedValueSpec("versionIdentifier", asbieSpec.VersionIdentifier) ,
							new UmlTaggedValueSpec("usageRule", asbieSpec.UsageRules) ,
						},
				};

			return umlAssociationSpec;
		}
	}
}

