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

namespace VIENNAAddIn.upcc3.repo.DocLibrary
{
    internal class UpccMa : UpccElement, IMa
    {
    	public UpccMa(IUmlClass umlClass):base(umlClass){}
    	
        public IUmlClass UmlClass 
        {
        	get
        	{
        		return this.UmlClassifier as IUmlClass;
        	}
        }

		public IDocLibrary DocLibrary
        {
            get { return new UpccDocLibrary(UmlClass.Package); }
        }

		public override ICctsLibrary library 
		{
			get { return DocLibrary; }
		}

		protected override ICctsAttribute CreateAttribute(IUmlAttribute attribute)
		{
			return null;
		}
	
		protected override ICctsAssociation CreateAssociation(IUmlAssociation association)
		{
			if (association.Stereotypes.Contains("ASMA")) 
				return new UpccAsma(association, this);
			else return null;
		}

		public IEnumerable<IAsma> Asmas
        {
            get
            {
            	return this.Properties.OfType<IAsma>();
            }
        }

		/// <summary>
		/// Creates a(n) ASMA based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a(n) ASMA.</param>
		/// <returns>The newly created ASMA.</returns>
		/// </summary>
		public IAsma CreateAsma(AsmaSpec specification)
		{
		    return new UpccAsma(UmlClass.CreateAssociation(AsmaSpecConverter.Convert(specification, Name)), this);
		}

		/// <summary>
		/// Updates a(n) ASMA to match the given <paramref name="specification"/>.
		/// <param name="asma">A(n) ASMA.</param>
		/// <param name="specification">A new specification for the given ASMA.</param>
		/// <returns>The updated ASMA. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IAsma UpdateAsma(IAsma asma, AsmaSpec specification)
		{
		    return new UpccAsma(UmlClass.UpdateAssociation(((UpccAsma) asma).UmlAssociation, AsmaSpecConverter.Convert(specification, Name)), this);
		}

		/// <summary>
		/// Removes a(n) ASMA from this MA.
		/// <param name="asma">A(n) ASMA.</param>
		/// </summary>
        public void RemoveAsma(IAsma asma)
		{
            UmlClass.RemoveAssociation(((UpccAsma) asma).UmlAssociation);
		}

        public bool Equals(UpccMa other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.UmlClass, UmlClass);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (UpccMa)) return false;
            return Equals((UpccMa) obj);
        }

        public override int GetHashCode()
        {
            return (UmlClass != null ? UmlClass.GetHashCode() : 0);
        }

        public static bool operator ==(UpccMa left, UpccMa right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UpccMa left, UpccMa right)
        {
            return !Equals(left, right);
        }
	}
}
