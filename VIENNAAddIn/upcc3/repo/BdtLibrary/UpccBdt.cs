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

namespace VIENNAAddIn.upcc3.repo.BdtLibrary
{
    internal class UpccBdt : UpccElement, IBdt
    {
    	public UpccBdt(IUmlClass umlClass):base(umlClass){}
    	
    	public IUmlClass UmlClass
        {
        	get {return this.UmlClassifier as IUmlClass;}
        }
		/// <summary>
		/// indicates that this BDT will be translated directly into it's underlying XSD datatype in the messages.
		/// this indicator is stored in the tagged value directXSDType
		/// </summary>
		public bool isDirectXSDType 
		{
			get 
			{
				return this.UmlClass.GetTaggedValue("directXSDType").Value.Equals("true",System.StringComparison.InvariantCultureIgnoreCase);
			}
		}
        /// <summary>
        /// the xsdType that corresponds to this BDT.
        /// Only valid for direct xsd types
        /// </summary>
        public string xsdType
        {
            get
            {
                var baseType = this.UmlClassifier.BaseClassifiers.FirstOrDefault();
                return baseType != null ? baseType.Name : string.Empty;
            }
        }

        #region implemented abstract members of UpccElement
        protected override UpccElement createSimilarElement(IUmlClassifier otherclassifier)
		{
			var otherClass = otherclassifier as IUmlClass;
			return otherClass != null ? new UpccBdt(otherClass) : null;
		}
		#endregion
		public IBdtLibrary BdtLibrary
        {
            get { return new UpccBdtLibrary(UmlClassifier.Package); }
        }
		public override ICctsLibrary library 
		{
			get { return BdtLibrary; }
		}

		public IBdt IsEquivalentTo
        {
            get
            {
                var dependency = UmlClass.GetFirstDependencyByStereotype("isEquivalentTo");
				if (dependency != null)
				{
					var target = dependency.Target as IUmlClass;
					if (target != null)
					{
						return new UpccBdt(target);
					}
				}
				return null;
            }
        }

		public ICdt BasedOn
        {
            get
            {
                var dependency = UmlClass.GetFirstDependencyByStereotype("basedOn");
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
				return new UpccBdtCon(attribute, this);
			if (attribute.Stereotypes.Contains("SUP"))
				return new UpccBdtSup(attribute, this);
			else
				return null;
		}
		public IBdtCon Con
        {
			get { return this.Attributes.OfType<IBdtCon>().FirstOrDefault(); }
        }

		public IEnumerable<IBdtSup> Sups
        {
			get { return this.Attributes.OfType<IBdtSup>(); }
        }

		/// <summary>
		/// Creates a(n) SUP based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a(n) SUP.</param>
		/// <returns>The newly created SUP.</returns>
		/// </summary>
		public IBdtSup CreateBdtSup(BdtSupSpec specification)
		{
		    return new UpccBdtSup(UmlClass.CreateAttribute(BdtSupSpecConverter.Convert(specification, Name)), this);
		}

		/// <summary>
		/// Updates a(n) SUP to match the given <paramref name="specification"/>.
		/// <param name="bdtSup">A(n) SUP.</param>
		/// <param name="specification">A new specification for the given SUP.</param>
		/// <returns>The updated SUP. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IBdtSup UpdateBdtSup(IBdtSup bdtSup, BdtSupSpec specification)
		{
		    return new UpccBdtSup(UmlClass.UpdateAttribute(((UpccBdtSup) bdtSup).UmlAttribute, BdtSupSpecConverter.Convert(specification, Name)), this);
		}

		/// <summary>
		/// Removes a(n) SUP from this BDT.
		/// <param name="bdtSup">A(n) SUP.</param>
		/// </summary>
        public void RemoveBdtSup(IBdtSup bdtSup)
		{
            UmlClass.RemoveAttribute(((UpccBdtSup) bdtSup).UmlAttribute);
		}

        

        public bool Equals(UpccBdt other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.UmlClass, UmlClass);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (UpccBdt)) return false;
            return Equals((UpccBdt) obj);
        }

        public override int GetHashCode()
        {
            return (UmlClass != null ? UmlClass.GetHashCode() : 0);
        }

        public static bool operator ==(UpccBdt left, UpccBdt right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UpccBdt left, UpccBdt right)
        {
            return !Equals(left, right);
        }
	}
}
