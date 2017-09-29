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
using System.Collections.Generic;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.repo.PrimLibrary
{
    internal class UpccPrim : UpccElement,IPrim
    {
    	public UpccPrim(IUmlDataType umlDataType):base(umlDataType){}

        public IUmlDataType UmlDataType
    	{
    		get {return this.UmlClassifier as IUmlDataType;}
    	}

		public IPrimLibrary PrimLibrary
        {
            get { return new UpccPrimLibrary(UmlDataType.Package); }
        }
				
		public override ICctsLibrary library 
		{
			get { return PrimLibrary; }
		}

		protected override ICctsAttribute CreateAttribute(IUmlAttribute attribute)
		{
			return null;
		}
		
		protected override ICctsAssociation CreateAssociation(IUmlAssociation association)
		{
			return null;
		}
		
		public IPrim IsEquivalentTo
        {
            get
            {
                var dependency = UmlDataType.GetFirstDependencyByStereotype("isEquivalentTo");
				if (dependency != null)
				{
					var target = dependency.Target as IUmlDataType;
					if (target != null)
					{
						return new UpccPrim(target);
					}
				}
				return null;
            }
        }

        ///<summary>
        /// Tagged value 'fractionDigits'.
        ///</summary>
        public string FractionDigits
        {
            get { return UmlDataType.GetTaggedValue("fractionDigits").Value; }
        }

        ///<summary>
        /// Tagged value 'length'.
        ///</summary>
        public string Length
        {
            get { return UmlDataType.GetTaggedValue("length").Value; }
        }

        ///<summary>
        /// Tagged value 'maximumExclusive'.
        ///</summary>
        public string MaximumExclusive
        {
            get { return UmlDataType.GetTaggedValue("maximumExclusive").Value; }
        }

        ///<summary>
        /// Tagged value 'maximumInclusive'.
        ///</summary>
        public string MaximumInclusive
        {
            get { return UmlDataType.GetTaggedValue("maximumInclusive").Value; }
        }

        ///<summary>
        /// Tagged value 'maximumLength'.
        ///</summary>
        public string MaximumLength
        {
            get { return UmlDataType.GetTaggedValue("maximumLength").Value; }
        }

        ///<summary>
        /// Tagged value 'minimumExclusive'.
        ///</summary>
        public string MinimumExclusive
        {
            get { return UmlDataType.GetTaggedValue("minimumExclusive").Value; }
        }

        ///<summary>
        /// Tagged value 'minimumInclusive'.
        ///</summary>
        public string MinimumInclusive
        {
            get { return UmlDataType.GetTaggedValue("minimumInclusive").Value; }
        }

        ///<summary>
        /// Tagged value 'minimumLength'.
        ///</summary>
        public string MinimumLength
        {
            get { return UmlDataType.GetTaggedValue("minimumLength").Value; }
        }

        ///<summary>
        /// Tagged value 'pattern'.
        ///</summary>
        public string Pattern
        {
            get { return UmlDataType.GetTaggedValue("pattern").Value; }
        }

        ///<summary>
        /// Tagged value 'totalDigits'.
        ///</summary>
        public string TotalDigits
        {
            get { return UmlDataType.GetTaggedValue("totalDigits").Value; }
        }

        ///<summary>
        /// Tagged value 'whiteSpace'.
        ///</summary>
        public string WhiteSpace
        {
            get { return UmlDataType.GetTaggedValue("whiteSpace").Value; }
        }

        public bool Equals(UpccPrim other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.UmlDataType, UmlDataType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (UpccPrim)) return false;
            return Equals((UpccPrim) obj);
        }

        public override int GetHashCode()
        {
            return (UmlDataType != null ? UmlDataType.GetHashCode() : 0);
        }

        public static bool operator ==(UpccPrim left, UpccPrim right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UpccPrim left, UpccPrim right)
        {
            return !Equals(left, right);
        }
	}
}
