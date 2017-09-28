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
    internal class UpccBbie :UpccAttribute, IBbie
    {
    	public UpccBbie(IUmlAttribute umlAttribute, IAbie abie):base(umlAttribute)
        {
			Abie = abie;
        }

        public IAbie Abie { get; private set; }

		public IBdt Bdt
		{
			get
			{
				var type = UmlAttribute.Type;
				return type != null ? new UpccBdt((IUmlClass) type) : null;
			}
		}

		public IBcc BasedOn
        {
            get
            {
                if (Abie == null)
                {
                    return null;
                }
                var acc = Abie.BasedOn;
                if (acc == null)
                {
                    return null;
                }
                string nameWithoutQualifiers = Name.Substring(Name.LastIndexOf('_') + 1);
                foreach (var bcc in acc.Bccs)
                {
                    if (nameWithoutQualifiers == bcc.Name)
                    {
                        return bcc;
                    }
                }
                return null;
            }
        }


        public bool Equals(UpccBbie other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.UmlAttribute, UmlAttribute);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (UpccBbie)) return false;
            return Equals((UpccBbie) obj);
        }

        public override int GetHashCode()
        {
            return (UmlAttribute != null ? UmlAttribute.GetHashCode() : 0);
        }

        public static bool operator ==(UpccBbie left, UpccBbie right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UpccBbie left, UpccBbie right)
        {
            return !Equals(left, right);
        }
	}
}

