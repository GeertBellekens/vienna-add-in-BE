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

namespace VIENNAAddIn.upcc3.repo.EnumLibrary
{
    internal class UpccCodelistEntry : UpccAttribute, ICodelistEntry
    {
    	public UpccCodelistEntry(IUmlEnumerationLiteral umlEnumerationLiteral, IEnum @enum):base(umlEnumerationLiteral)
        {
			Enum = @enum;
        }

        public IUmlEnumerationLiteral UmlEnumerationLiteral
        {
        	get {return this.UmlAttribute as IUmlEnumerationLiteral;}
        }

        public IEnum Enum { get; private set; }
        public override ICctsElement Owner 
       	{
       		get {return this.Enum;}
		}
        
        protected override int getPosition()
        {
            //don't need sequencingkey here
            return this.UmlAttribute.position;
        }

        ///<summary>
        /// Tagged value 'codeName'.
        ///</summary>
        public string CodeName
        {
            get { return UmlEnumerationLiteral.GetTaggedValue("codeName").Value; }
        }

        ///<summary>
        /// Tagged value 'status'.
        ///</summary>
        public string Status
        {
            get { return UmlEnumerationLiteral.GetTaggedValue("status").Value; }
        }


        public bool Equals(UpccCodelistEntry other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.UmlEnumerationLiteral, UmlEnumerationLiteral);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (UpccCodelistEntry)) return false;
            return Equals((UpccCodelistEntry) obj);
        }

        public override int GetHashCode()
        {
            return (UmlEnumerationLiteral != null ? UmlEnumerationLiteral.GetHashCode() : 0);
        }

        public static bool operator ==(UpccCodelistEntry left, UpccCodelistEntry right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UpccCodelistEntry left, UpccCodelistEntry right)
        {
            return !Equals(left, right);
        }
	}
}

