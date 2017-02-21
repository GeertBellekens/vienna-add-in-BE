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

namespace VIENNAAddIn.upcc3.repo.BieLibrary
{
    internal static partial class AbieSpecConverter
    {
		internal static UmlClassSpec Convert(AbieSpec abieSpec)
		{
			var umlClassSpec = new UmlClassSpec
				{
					Stereotype = "ABIE",
					Name = abieSpec.Name,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("businessTerm", abieSpec.BusinessTerms) ,
							new UmlTaggedValueSpec("definition", abieSpec.Definition) ,
							new UmlTaggedValueSpec("dictionaryEntryName", abieSpec.DictionaryEntryName) { DefaultValue = GenerateDictionaryEntryNameDefaultValue(abieSpec) },
							new UmlTaggedValueSpec("languageCode", abieSpec.LanguageCode) ,
							new UmlTaggedValueSpec("uniqueIdentifier", abieSpec.UniqueIdentifier) { DefaultValue = GenerateUniqueIdentifierDefaultValue(abieSpec) },
							new UmlTaggedValueSpec("versionIdentifier", abieSpec.VersionIdentifier) ,
							new UmlTaggedValueSpec("usageRule", abieSpec.UsageRules) ,
						},
				};

			var dependencySpecs = new List<UmlDependencySpec>();
			if (abieSpec.IsEquivalentTo != null)
			{
				dependencySpecs.Add(new UmlDependencySpec
									{
										Stereotype = "isEquivalentTo",
										Target = ((UpccAbie) abieSpec.IsEquivalentTo).UmlClass,
										LowerBound = "0",
										UpperBound = "1",
									});
			}
			if (abieSpec.BasedOn != null)
			{
				dependencySpecs.Add(new UmlDependencySpec
									{
										Stereotype = "basedOn",
										Target = ((UpccAcc) abieSpec.BasedOn).UmlClass,
										LowerBound = "0",
										UpperBound = "1",
									});
			}
			umlClassSpec.Dependencies = dependencySpecs;

			var attributeSpecs = new List<UmlAttributeSpec>();
			if (abieSpec.Bbies != null)
			{
				foreach (var bbieSpec in abieSpec.Bbies)
				{
					attributeSpecs.Add(BbieSpecConverter.Convert(bbieSpec, abieSpec.Name));
				}
			}
			umlClassSpec.Attributes = MakeAttributeNamesUnique(attributeSpecs);

			var associationSpecs = new List<UmlAssociationSpec>();
			if (abieSpec.Asbies != null)
			{
				foreach (var asbieSpec in abieSpec.Asbies)
				{
					associationSpecs.Add(AsbieSpecConverter.Convert(asbieSpec, abieSpec.Name));
				}
			}
			umlClassSpec.Associations = MakeAssociationNamesUnique(associationSpecs);

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

