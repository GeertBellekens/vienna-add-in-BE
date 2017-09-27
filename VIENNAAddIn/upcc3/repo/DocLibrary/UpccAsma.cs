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
using System;
using System.Collections.Generic;
using VIENNAAddIn.upcc3.uml;
using System.Linq;

namespace VIENNAAddIn.upcc3.repo.DocLibrary
{
    internal class UpccAsma : UpccAssociation, IAsma
    {
    	public UpccAsma(IUmlAssociation umlAssociation, IMa associatingMa):base(umlAssociation,associatingMa)
    	{}


		public override ICctsElement AssociatedElement {
			get 
			{
				return AssociatedBieAggregator.Element;
			}
		}


        public int Id
        {
            get { return UmlAssociation.Id; }
        }

        public string Name
        {
            get { return UmlAssociation.Name; }
        }

        public string UpperBound
		{
            get { return UmlAssociation.UpperBound; }
		}
		
        public string LowerBound
		{
            get { return UmlAssociation.LowerBound; }
		}
		
        public bool IsOptional()
        {
            int i;
            return Int32.TryParse(LowerBound, out i) && i == 0;
        }
		
        public IMa AssociatingMa { get; private set; }
		
		public BieAggregator AssociatedBieAggregator
		{
			get
			{
				var associatedClassifier = UmlAssociation.AssociatedClassifier;
				if (associatedClassifier.Stereotypes.Contains("ABIE")) 
					return new BieAggregator(new UpccAbie((IUmlClass) associatedClassifier));
				if (associatedClassifier.Stereotypes.Contains("MA"))
					return new BieAggregator(new UpccMa((IUmlClass) associatedClassifier));
				return null;
			}
		}

        public bool Equals(UpccAsma other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.UmlAssociation, UmlAssociation);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (UpccAsma)) return false;
            return Equals((UpccAsma) obj);
        }

        public override int GetHashCode()
        {
            return (UmlAssociation != null ? UmlAssociation.GetHashCode() : 0);
        }

        public static bool operator ==(UpccAsma left, UpccAsma right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UpccAsma left, UpccAsma right)
        {
            return !Equals(left, right);
        }
	}
}

