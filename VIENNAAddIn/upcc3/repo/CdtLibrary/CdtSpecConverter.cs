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

namespace VIENNAAddIn.upcc3.repo.CdtLibrary
{
    internal static partial class CdtSpecConverter
    {
		internal static UmlClassSpec Convert(CdtSpec cdtSpec)
		{
			var umlClassSpec = new UmlClassSpec
				{
					Stereotype = "CDT",
					Name = cdtSpec.Name,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("businessTerm", cdtSpec.BusinessTerms) ,
							new UmlTaggedValueSpec("definition", cdtSpec.Definition) ,
							new UmlTaggedValueSpec("dictionaryEntryName", cdtSpec.DictionaryEntryName) { DefaultValue = GenerateDictionaryEntryNameDefaultValue(cdtSpec) },
							new UmlTaggedValueSpec("languageCode", cdtSpec.LanguageCode) ,
							new UmlTaggedValueSpec("uniqueIdentifier", cdtSpec.UniqueIdentifier) { DefaultValue = GenerateUniqueIdentifierDefaultValue(cdtSpec) },
							new UmlTaggedValueSpec("versionIdentifier", cdtSpec.VersionIdentifier) ,
							new UmlTaggedValueSpec("usageRule", cdtSpec.UsageRules) ,
						},
				};

			var dependencySpecs = new List<UmlDependencySpec>();
			if (cdtSpec.IsEquivalentTo != null)
			{
				dependencySpecs.Add(new UmlDependencySpec
									{
										Stereotype = "isEquivalentTo",
										Target = ((UpccCdt) cdtSpec.IsEquivalentTo).UmlClass,
										LowerBound = "0",
										UpperBound = "1",
									});
			}
			umlClassSpec.Dependencies = dependencySpecs;

			var attributeSpecs = new List<UmlAttributeSpec>();
			if (cdtSpec.Con != null)
			{
				attributeSpecs.Add(CdtConSpecConverter.Convert(cdtSpec.Con, cdtSpec.Name));
			}
			if (cdtSpec.Sups != null)
			{
				foreach (var cdtSupSpec in cdtSpec.Sups)
				{
					attributeSpecs.Add(CdtSupSpecConverter.Convert(cdtSupSpec, cdtSpec.Name));
				}
			}
			umlClassSpec.Attributes = MakeAttributeNamesUnique(attributeSpecs);

			return umlClassSpec;
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

