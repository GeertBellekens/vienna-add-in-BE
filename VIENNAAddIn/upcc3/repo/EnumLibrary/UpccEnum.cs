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

namespace VIENNAAddIn.upcc3.repo.EnumLibrary
{
    internal class UpccEnum : UpccElement, IEnum
    {
    	public UpccEnum(IUmlEnumeration umlEnumeration):base(umlEnumeration){}


        public IUmlEnumeration UmlEnumeration 
        {
    		get
    		{
    			return this.UmlClassifier as IUmlEnumeration;
    		}
        }

		public bool IsAssembled 
		{
			get 
			{
				return this.UmlEnumeration.Stereotypes.Contains("Assembled");
			}
		}
       	#region implemented abstract members of UpccElement
		protected override UpccElement createSimilarElement(IUmlClassifier otherclassifier)
		{
			var otherEnum = otherclassifier as IUmlEnumeration;
			return otherEnum != null ? new UpccEnum(otherEnum) : null;
		}
		#endregion

		public IEnumLibrary EnumLibrary
        {
            get { return new UpccEnumLibrary(UmlEnumeration.Package); }
        }
				
		public override ICctsLibrary library 
		{
			get { return EnumLibrary; }
		}

		public IEnum IsEquivalentTo
        {
            get
            {
                var dependency = UmlEnumeration.GetFirstDependencyByStereotype("isEquivalentTo");
				if (dependency != null)
				{
					var target = dependency.Target as IUmlEnumeration;
					if (target != null)
					{
						return new UpccEnum(target);
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
			var enumerationLiteral = attribute as IUmlEnumerationLiteral ;
			return enumerationLiteral != null && (enumerationLiteral.Stereotypes.Contains("CodelistEntry")
			                                      || enumerationLiteral.Stereotypes.Contains("e_CodeListEntry"))
				? new UpccCodelistEntry(enumerationLiteral, this) : null;
		}
		
		public IEnumerable<ICodelistEntry> CodelistEntries
        {
            get { return this.Attributes.OfType<ICodelistEntry>(); }
        }

		/// <summary>
		/// Creates a(n) CodelistEntry based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a(n) CodelistEntry.</param>
		/// <returns>The newly created CodelistEntry.</returns>
		/// </summary>
		public ICodelistEntry CreateCodelistEntry(CodelistEntrySpec specification)
		{
		    return new UpccCodelistEntry(UmlEnumeration.CreateEnumerationLiteral(CodelistEntrySpecConverter.Convert(specification)), this);
		}

		/// <summary>
		/// Updates a(n) CodelistEntry to match the given <paramref name="specification"/>.
		/// <param name="codelistEntry">A(n) CodelistEntry.</param>
		/// <param name="specification">A new specification for the given CodelistEntry.</param>
		/// <returns>The updated CodelistEntry. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public ICodelistEntry UpdateCodelistEntry(ICodelistEntry codelistEntry, CodelistEntrySpec specification)
		{
		    return new UpccCodelistEntry(UmlEnumeration.UpdateEnumerationLiteral(((UpccCodelistEntry) codelistEntry).UmlEnumerationLiteral, CodelistEntrySpecConverter.Convert(specification)), this);
		}

		/// <summary>
		/// Removes a(n) CodelistEntry from this ENUM.
		/// <param name="codelistEntry">A(n) CodelistEntry.</param>
		/// </summary>
        public void RemoveCodelistEntry(ICodelistEntry codelistEntry)
		{
            UmlEnumeration.RemoveEnumerationLiteral(((UpccCodelistEntry) codelistEntry).UmlEnumerationLiteral);
		}


        ///<summary>
        /// Tagged value 'codeListAgencyIdentifier'.
        ///</summary>
        public string CodeListAgencyIdentifier
        {
            get { return UmlEnumeration.GetTaggedValue("codeListAgencyIdentifier").Value; }
        }

        ///<summary>
        /// Tagged value 'codeListAgencyName'.
        ///</summary>
        public string CodeListAgencyName
        {
            get { return UmlEnumeration.GetTaggedValue("codeListAgencyName").Value; }
        }

        ///<summary>
        /// Tagged value 'codeListIdentifier'.
        ///</summary>
        public string CodeListIdentifier
        {
            get { return UmlEnumeration.GetTaggedValue("codeListIdentifier").Value; }
        }

        ///<summary>
        /// Tagged value 'codeListName'.
        ///</summary>
        public string CodeListName
        {
            get { return UmlEnumeration.GetTaggedValue("codeListName").Value; }
        }

        ///<summary>
        /// Tagged value 'enumerationURI'.
        ///</summary>
        public string EnumerationURI
        {
            get { return UmlEnumeration.GetTaggedValue("enumerationURI").Value; }
        }

        ///<summary>
        /// Tagged value 'modificationAllowedIndicator'.
        ///</summary>
        public string ModificationAllowedIndicator
        {
            get { return UmlEnumeration.GetTaggedValue("modificationAllowedIndicator").Value; }
        }

        ///<summary>
        /// Tagged value 'restrictedPrimitive'.
        ///</summary>
        public string RestrictedPrimitive
        {
            get { return UmlEnumeration.GetTaggedValue("restrictedPrimitive").Value; }
        }

        ///<summary>
        /// Tagged value 'status'.
        ///</summary>
        public string Status
        {
            get { return UmlEnumeration.GetTaggedValue("status").Value; }
        }

        public bool Equals(UpccEnum other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.UmlEnumeration, UmlEnumeration);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (UpccEnum)) return false;
            return Equals((UpccEnum) obj);
        }

        public override int GetHashCode()
        {
            return (UmlEnumeration != null ? UmlEnumeration.GetHashCode() : 0);
        }

        public static bool operator ==(UpccEnum left, UpccEnum right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UpccEnum left, UpccEnum right)
        {
            return !Equals(left, right);
        }

	}
}
