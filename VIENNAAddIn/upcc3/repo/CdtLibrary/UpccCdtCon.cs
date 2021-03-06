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

namespace VIENNAAddIn.upcc3.repo.CdtLibrary
{
    internal class UpccCdtCon : UpccAttribute, ICdtCon
    {
    	public UpccCdtCon(IUmlAttribute umlAttribute, ICdt cdt):base(umlAttribute)
        {
			Cdt = cdt;
        }

        public ICdt Cdt { get; private set; }
        public override ICctsElement Owner 
       	{
       		get {return this.Cdt;}
		}

		public bool Equals(UpccCdtCon other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.UmlAttribute, UmlAttribute);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (UpccCdtCon)) return false;
            return Equals((UpccCdtCon) obj);
        }

        public override int GetHashCode()
        {
            return (UmlAttribute != null ? UmlAttribute.GetHashCode() : 0);
        }

        public static bool operator ==(UpccCdtCon left, UpccCdtCon right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UpccCdtCon left, UpccCdtCon right)
        {
            return !Equals(left, right);
        }
	}
}

