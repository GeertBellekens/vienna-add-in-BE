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
using VIENNAAddIn.Utils;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.repo.EnumLibrary
{
    internal static partial class IdSchemeSpecConverter
    {
		internal static UmlDataTypeSpec Convert(IdSchemeSpec idSchemeSpec)
		{
			var umlDataTypeSpec = new UmlDataTypeSpec
				{
					Stereotype = "IDSCHEME",
					Name = idSchemeSpec.Name,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("businessTerm", idSchemeSpec.BusinessTerms) ,
							new UmlTaggedValueSpec("definition", idSchemeSpec.Definition) ,
							new UmlTaggedValueSpec("dictionaryEntryName", idSchemeSpec.DictionaryEntryName) { DefaultValue = GenerateDictionaryEntryNameDefaultValue(idSchemeSpec) },
							new UmlTaggedValueSpec("identifierSchemeAgencyIdentifier", idSchemeSpec.IdentifierSchemeAgencyIdentifier) ,
							new UmlTaggedValueSpec("identifierSchemeAgencyName", idSchemeSpec.IdentifierSchemeAgencyName) ,
							new UmlTaggedValueSpec("modificationAllowedIndicator", idSchemeSpec.ModificationAllowedIndicator) ,
							new UmlTaggedValueSpec("pattern", idSchemeSpec.Pattern) ,
							new UmlTaggedValueSpec("restrictedPrimitive", idSchemeSpec.RestrictedPrimitive) ,
							new UmlTaggedValueSpec("uniqueIdentifier", idSchemeSpec.UniqueIdentifier) { DefaultValue = GenerateUniqueIdentifierDefaultValue(idSchemeSpec) },
							new UmlTaggedValueSpec("versionIdentifier", idSchemeSpec.VersionIdentifier) ,
						},
				};

			return umlDataTypeSpec;
		}

        private static IEnumerable<UmlAttributeSpec> MakeAttributeNamesUnique(List<UmlAttributeSpec> specs)
        {
            var specsByName = new Dictionary<string, List<UmlAttributeSpec>>();
            foreach (var spec in specs)
            {
                specsByName.GetAndCreate(spec.Name).Add(spec);
            }
            foreach (var specList in specsByName.Values)
            {
                if (specList.Count > 1)
                {
                    foreach (var spec in specList)
                    {
                        spec.Name = spec.Name + spec.Type.Name;
                    }
                }
            }
            return specs;
        }

        private static IEnumerable<UmlAssociationSpec> MakeAssociationNamesUnique(List<UmlAssociationSpec> specs)
        {
            var specsByName = new Dictionary<string, List<UmlAssociationSpec>>();
            foreach (var spec in specs)
            {
                specsByName.GetAndCreate(spec.Name).Add(spec);
            }
            foreach (var specList in specsByName.Values)
            {
                if (specList.Count > 1)
                {
                    foreach (var spec in specList)
                    {
                        spec.Name = spec.Name + spec.AssociatedClassifier.Name;
                    }
                }
            }
            return specs;
        }
	}
}

