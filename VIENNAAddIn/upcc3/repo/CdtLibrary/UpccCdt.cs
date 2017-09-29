// ReSharper disable RedundantUsingDirective
using System.Linq;
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
using System.Collections.Generic;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.repo.CdtLibrary
{
    internal class UpccCdt : UpccElement, ICdt
    {
    	public UpccCdt(IUmlClass umlClass):base(umlClass){}
		
    	public IUmlClass UmlClass
        {
        	get {return this.UmlClassifier as IUmlClass;}
        }

		public ICdtLibrary CdtLibrary
        {
            get { return new UpccCdtLibrary(UmlClassifier.Package); }
        }
		
		public override ICctsLibrary library 
		{
			get { return CdtLibrary; }
		}

		public ICdt IsEquivalentTo
        {
            get
            {
                var dependency = UmlClassifier.GetFirstDependencyByStereotype("isEquivalentTo");
				if (dependency != null)
				{
					var target = dependency.Target as IUmlClass;
					if (target != null)
					{
						return new UpccCdt(target);
					}
				}
				return null;
            }
        }

		public override ICctsAssociation CreateAssociation(IUmlAssociation association)
		{
			return null;
		}

		public override ICctsAttribute CreateAttribute(IUmlAttribute attribute)
		{
			if (attribute.Stereotypes.Contains("CON"))
				return new UpccCdtCon(attribute, this);
			if (attribute.Stereotypes.Contains("SUP"))
				return new UpccCdtSup(attribute, this);
			else
				return null;
		}
		public ICdtCon Con
        {
			get { return this.Attributes.OfType<ICdtCon>().FirstOrDefault(); }
        }

		public IEnumerable<ICdtSup> Sups
        {
			get { return this.Attributes.OfType<ICdtSup>(); }
        }

		/// <summary>
		/// Creates a(n) SUP based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a(n) SUP.</param>
		/// <returns>The newly created SUP.</returns>
		/// </summary>
		public ICdtSup CreateCdtSup(CdtSupSpec specification)
		{
		    return new UpccCdtSup(UmlClass.CreateAttribute(CdtSupSpecConverter.Convert(specification, Name)), this);
		}

		/// <summary>
		/// Updates a(n) SUP to match the given <paramref name="specification"/>.
		/// <param name="cdtSup">A(n) SUP.</param>
		/// <param name="specification">A new specification for the given SUP.</param>
		/// <returns>The updated SUP. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public ICdtSup UpdateCdtSup(ICdtSup cdtSup, CdtSupSpec specification)
		{
		    return new UpccCdtSup(UmlClass.UpdateAttribute(((UpccCdtSup) cdtSup).UmlAttribute, CdtSupSpecConverter.Convert(specification, Name)), this);
		}

		/// <summary>
		/// Removes a(n) SUP from this CDT.
		/// <param name="cdtSup">A(n) SUP.</param>
		/// </summary>
        public void RemoveCdtSup(ICdtSup cdtSup)
		{
            UmlClass.RemoveAttribute(((UpccCdtSup) cdtSup).UmlAttribute);
		}

        public bool Equals(UpccCdt other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.UmlClassifier, UmlClassifier);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (UpccCdt)) return false;
            return Equals((UpccCdt) obj);
        }

        public override int GetHashCode()
        {
            return (UmlClassifier != null ? UmlClassifier.GetHashCode() : 0);
        }

        public static bool operator ==(UpccCdt left, UpccCdt right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UpccCdt left, UpccCdt right)
        {
            return !Equals(left, right);
        }
	}
}
