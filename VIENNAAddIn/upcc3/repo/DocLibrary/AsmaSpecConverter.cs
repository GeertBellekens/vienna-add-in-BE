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
    internal static partial class AsmaSpecConverter
    {
		internal static UmlAssociationSpec Convert(AsmaSpec asmaSpec, string associatingClassName)
		{
			IUmlClassifier associatedClassifierType;
			var multiType = asmaSpec.AssociatedBieAggregator;
            if (multiType.IsAbie)
            {
                associatedClassifierType = ((UpccAbie) multiType.Abie).UmlClass;
			}
			else
            if (multiType.IsMa)
            {
                associatedClassifierType = ((UpccMa) multiType.Ma).UmlClass;
			}
			else
			{
				associatedClassifierType = null;
			}
			var umlAssociationSpec = new UmlAssociationSpec
				{
					Stereotype = "ASMA",
					Name = asmaSpec.Name,
					UpperBound = asmaSpec.UpperBound,
					LowerBound = asmaSpec.LowerBound,
					AggregationKind = AggregationKind.Shared,
					AssociatedClassifier = associatedClassifierType,
				};

			return umlAssociationSpec;
		}
	}
}

