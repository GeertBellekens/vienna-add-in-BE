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

namespace VIENNAAddIn.upcc3.repo.CcLibrary
{
    internal static partial class AccSpecConverter
    {
		internal static UmlClassSpec Convert(AccSpec accSpec)
		{
			var umlClassSpec = new UmlClassSpec
				{
					Stereotype = "ACC",
					Name = accSpec.Name,
					TaggedValues = new[]
						{
							new UmlTaggedValueSpec("businessTerm", accSpec.BusinessTerms) ,
							new UmlTaggedValueSpec("definition", accSpec.Definition) ,
							new UmlTaggedValueSpec("dictionaryEntryName", accSpec.DictionaryEntryName) { DefaultValue = GenerateDictionaryEntryNameDefaultValue(accSpec) },
							new UmlTaggedValueSpec("languageCode", accSpec.LanguageCode) ,
							new UmlTaggedValueSpec("uniqueIdentifier", accSpec.UniqueIdentifier) { DefaultValue = GenerateUniqueIdentifierDefaultValue(accSpec) },
							new UmlTaggedValueSpec("versionIdentifier", accSpec.VersionIdentifier) ,
							new UmlTaggedValueSpec("usageRule", accSpec.UsageRules) ,
						},
				};

			var dependencySpecs = new List<UmlDependencySpec>();
			if (accSpec.IsEquivalentTo != null)
			{
				dependencySpecs.Add(new UmlDependencySpec
									{
										Stereotype = "isEquivalentTo",
										Target = ((UpccAcc) accSpec.IsEquivalentTo).UmlClass,
										LowerBound = "0",
										UpperBound = "1",
									});
			}
			umlClassSpec.Dependencies = dependencySpecs;

			var attributeSpecs = new List<UmlAttributeSpec>();
			if (accSpec.Bccs != null)
			{
				foreach (var bccSpec in accSpec.Bccs)
				{
					attributeSpecs.Add(BccSpecConverter.Convert(bccSpec, accSpec.Name));
				}
			}
			umlClassSpec.Attributes = MakeAttributeNamesUnique(attributeSpecs);

			var associationSpecs = new List<UmlAssociationSpec>();
			if (accSpec.Asccs != null)
			{
				foreach (var asccSpec in accSpec.Asccs)
				{
					associationSpecs.Add(AsccSpecConverter.Convert(asccSpec, accSpec.Name));
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

