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

namespace VIENNAAddIn.upcc3.repo.BieLibrary
{
    internal class UpccAsbie : UpccAssociation, IAsbie
    {
    	public UpccAsbie(IUmlAssociation umlAssociation, IAbie associatingAbie):base(umlAssociation,associatingAbie)
    	{}

		public override ICctsElement AssociatedElement {
			get 
			{
				return AssociatedAbie;
			}
		}

        public IAbie AssociatingAbie { get; private set; }
		
		public IAbie AssociatedAbie
		{
			get
			{
				return new UpccAbie((IUmlClass) UmlAssociation.AssociatedClassifier);
			}
		}

		public IAscc BasedOn
        {
            get
            {
                if (AssociatingAbie == null)
                {
                    return null;
                }
                var acc = AssociatingAbie.BasedOn;
                if (acc == null)
                {
                    return null;
                }
                string nameWithoutQualifiers = Name.Substring(Name.LastIndexOf('_') + 1);
                foreach (var ascc in acc.Asccs)
                {
                    if (nameWithoutQualifiers == ascc.Name)
                    {
                        return ascc;
                    }
                }
                return null;
            }
        }

        

        public bool Equals(UpccAsbie other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.UmlAssociation, UmlAssociation);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (UpccAsbie)) return false;
            return Equals((UpccAsbie) obj);
        }

        public override int GetHashCode()
        {
            return (UmlAssociation != null ? UmlAssociation.GetHashCode() : 0);
        }

        public static bool operator ==(UpccAsbie left, UpccAsbie right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UpccAsbie left, UpccAsbie right)
        {
            return !Equals(left, right);
        }
	}
}

