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

namespace VIENNAAddIn.upcc3.repo.CcLibrary
{
    internal class UpccAcc : UpccElement, IAcc
    {
    	public UpccAcc(IUmlClass umlClass):base(umlClass){}

        public IUmlClass UmlClass
        {
        	get {return this.UmlClassifier as IUmlClass;}
        }

		public ICcLibrary CcLibrary
        {
            get { return new UpccCcLibrary(UmlClass.Package); }
        }
		public override ICctsLibrary library 
		{
			get { return CcLibrary; }
		}

		public IAcc IsEquivalentTo
        {
            get
            {
                var dependency = UmlClass.GetFirstDependencyByStereotype("isEquivalentTo");
				if (dependency != null)
				{
					var target = dependency.Target as IUmlClass;
					if (target != null)
					{
						return new UpccAcc(target);
					}
				}
				return null;
            }
        }

		#region implemented abstract members of UpccElement
		protected override ICctsAttribute CreateAttribute(IUmlAttribute attribute)
		{
			return attribute.Stereotypes.Contains("BCC") ?
				new UpccBcc(attribute, this): null;
		}
		#endregion
		public IEnumerable<IBcc> Bccs
        {
            get { return this.Attributes.OfType<UpccBcc>(); }
        }

		/// <summary>
		/// Creates a(n) BCC based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a(n) BCC.</param>
		/// <returns>The newly created BCC.</returns>
		/// </summary>
		public IBcc CreateBcc(BccSpec specification)
		{
		    return new UpccBcc(UmlClass.CreateAttribute(BccSpecConverter.Convert(specification, Name)), this);
		}

		/// <summary>
		/// Updates a(n) BCC to match the given <paramref name="specification"/>.
		/// <param name="bcc">A(n) BCC.</param>
		/// <param name="specification">A new specification for the given BCC.</param>
		/// <returns>The updated BCC. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IBcc UpdateBcc(IBcc bcc, BccSpec specification)
		{
		    return new UpccBcc(UmlClass.UpdateAttribute(((UpccBcc) bcc).UmlAttribute, BccSpecConverter.Convert(specification, Name)), this);
		}

		/// <summary>
		/// Removes a(n) BCC from this ACC.
		/// <param name="bcc">A(n) BCC.</param>
		/// </summary>
        public void RemoveBcc(IBcc bcc)
		{
            UmlClass.RemoveAttribute(((UpccBcc) bcc).UmlAttribute);
		}

		public IEnumerable<IAscc> Asccs
        {
            get
            {
                foreach (var association in UmlClass.GetAssociationsByStereotype("ASCC"))
                {
                    yield return new UpccAscc(association, this);
                }
            }
        }

		/// <summary>
		/// Creates a(n) ASCC based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a(n) ASCC.</param>
		/// <returns>The newly created ASCC.</returns>
		/// </summary>
		public IAscc CreateAscc(AsccSpec specification)
		{
		    return new UpccAscc(UmlClass.CreateAssociation(AsccSpecConverter.Convert(specification, Name)), this);
		}

		/// <summary>
		/// Updates a(n) ASCC to match the given <paramref name="specification"/>.
		/// <param name="ascc">A(n) ASCC.</param>
		/// <param name="specification">A new specification for the given ASCC.</param>
		/// <returns>The updated ASCC. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IAscc UpdateAscc(IAscc ascc, AsccSpec specification)
		{
		    return new UpccAscc(UmlClass.UpdateAssociation(((UpccAscc) ascc).UmlAssociation, AsccSpecConverter.Convert(specification, Name)), this);
		}

		/// <summary>
		/// Removes a(n) ASCC from this ACC.
		/// <param name="ascc">A(n) ASCC.</param>
		/// </summary>
        public void RemoveAscc(IAscc ascc)
		{
            UmlClass.RemoveAssociation(((UpccAscc) ascc).UmlAssociation);
		}

        public bool Equals(UpccAcc other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.UmlClass, UmlClass);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (UpccAcc)) return false;
            return Equals((UpccAcc) obj);
        }

        public override int GetHashCode()
        {
            return (UmlClass != null ? UmlClass.GetHashCode() : 0);
        }

        public static bool operator ==(UpccAcc left, UpccAcc right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UpccAcc left, UpccAcc right)
        {
            return !Equals(left, right);
        }
	}
}
