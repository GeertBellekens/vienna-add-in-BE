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

namespace VIENNAAddIn.upcc3.repo.CcLibrary
{
    internal class UpccAscc : UpccAssociation, IAscc
    {
    	public UpccAscc(IUmlAssociation umlAssociation, IAcc associatingAcc):base(umlAssociation, associatingAcc)
    	{}

		public override ICctsElement AssociatedElement {
			get 
			{
				return AssociatedAcc;
			}
		}

        public IAcc AssociatingAcc { get; private set; }
		
		public IAcc AssociatedAcc
		{
			get
			{
				return new UpccAcc((IUmlClass) UmlAssociation.AssociatedClassifier);
			}
		}

        public bool Equals(UpccAscc other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.UmlAssociation, UmlAssociation);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (UpccAscc)) return false;
            return Equals((UpccAscc) obj);
        }

        public override int GetHashCode()
        {
            return (UmlAssociation != null ? UmlAssociation.GetHashCode() : 0);
        }

        public static bool operator ==(UpccAscc left, UpccAscc right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UpccAscc left, UpccAscc right)
        {
            return !Equals(left, right);
        }
	}
}

