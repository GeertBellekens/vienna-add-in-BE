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
    internal class UpccCdtSup : UpccAttribute, ICdtSup
    {
    	public UpccCdtSup(IUmlAttribute umlAttribute, ICdt cdt):base(umlAttribute)
        {
			Cdt = cdt;
        }

        public ICdt Cdt { get; private set; }

		public bool Equals(UpccCdtSup other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.UmlAttribute, UmlAttribute);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (UpccCdtSup)) return false;
            return Equals((UpccCdtSup) obj);
        }

        public override int GetHashCode()
        {
            return (UmlAttribute != null ? UmlAttribute.GetHashCode() : 0);
        }

        public static bool operator ==(UpccCdtSup left, UpccCdtSup right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UpccCdtSup left, UpccCdtSup right)
        {
            return !Equals(left, right);
        }
	}
}

