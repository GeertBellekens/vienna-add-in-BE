using System.Collections.Generic;
using System.Linq;
using System;
using CctsRepository;
using VIENNAAddIn.upcc3.repo.BdtLibrary;
using VIENNAAddIn.upcc3.repo.BieLibrary;
using VIENNAAddIn.upcc3.repo.CcLibrary;
using VIENNAAddIn.upcc3.repo.CdtLibrary;
using VIENNAAddIn.upcc3.repo.DocLibrary;
using VIENNAAddIn.upcc3.repo.EnumLibrary;
using VIENNAAddIn.upcc3.repo.PrimLibrary;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.repo
{
	/// <summary>
	/// Description of UpccLibrary.
	/// </summary>
	public abstract class UpccLibrary:ICctsLibrary
	{
		protected UpccLibrary(IUmlPackage umlPackage)
		{
			UmlPackage = umlPackage;
		}
        public IUmlPackage UmlPackage { get; private set; }

		#region ICctsLibrary implementation

		/// <summary>
		/// The Libraries's unique ID.
		/// </summary>
        public int Id
        {
            get { return UmlPackage.Id; }
        }

		/// <summary>
		/// The Library's name.
		/// </summary>
        public string Name
        {
            get { return UmlPackage.Name; }
        }
        private List<ICctsElement> _Elements;
		public IEnumerable<ICctsElement> Elements 
		{
			get 
			{
				if( _Elements == null)
				{
					_Elements = new List<ICctsElement>();
					foreach (var umlClassifier in this.UmlPackage.Classifiers) 
					{
						if (umlClassifier.Stereotypes.Contains("BDT")) _Elements.Add(new UpccBdt(umlClassifier as IUmlClass));
						else if (umlClassifier.Stereotypes.Contains("ABIE")) _Elements.Add(new UpccAbie(umlClassifier as IUmlClass));
						else if (umlClassifier.Stereotypes.Contains("ACC")) _Elements.Add(new UpccAcc(umlClassifier as IUmlClass));
						else if (umlClassifier.Stereotypes.Contains("CDT")) _Elements.Add(new UpccCdt(umlClassifier as IUmlClass));
						else if (umlClassifier.Stereotypes.Contains("MA")) _Elements.Add(new UpccMa(umlClassifier as IUmlClass));
						else if (umlClassifier.Stereotypes.Contains("ENUM")) _Elements.Add(new UpccEnum(umlClassifier as IUmlEnumeration));
						else if (umlClassifier.Stereotypes.Contains("IDSCHEME")) _Elements.Add(new UpccIdScheme(umlClassifier as IUmlDataType));
						else if (umlClassifier.Stereotypes.Contains("PRIM")) _Elements.Add(new UpccPrim(umlClassifier as IUmlDataType));
					}
				}
				return _Elements;
			}
		}
		
		///<summary>
        /// Tagged value 'businessTerm'.
        ///</summary>
        public IEnumerable<string> BusinessTerms
        {
            get { return UmlPackage.GetTaggedValue("businessTerm").SplitValues; }
        }

        ///<summary>
        /// Tagged value 'copyright'.
        ///</summary>
        public IEnumerable<string> Copyrights
        {
            get { return UmlPackage.GetTaggedValue("copyright").SplitValues; }
        }

        ///<summary>
        /// Tagged value 'owner'.
        ///</summary>
        public IEnumerable<string> Owners
        {
            get { return UmlPackage.GetTaggedValue("owner").SplitValues; }
        }

        ///<summary>
        /// Tagged value 'reference'.
        ///</summary>
        public IEnumerable<string> References
        {
            get { return UmlPackage.GetTaggedValue("reference").SplitValues; }
        }

        ///<summary>
        /// Tagged value 'status'.
        ///</summary>
        public string Status
        {
            get { return UmlPackage.GetTaggedValue("status").Value; }
        }

        ///<summary>
        /// Tagged value 'uniqueIdentifier'.
        ///</summary>
        public string UniqueIdentifier
        {
            get 
            {
            	string tvValue = UmlPackage.GetTaggedValue("uniqueIdentifier").Value;
            	if (string.IsNullOrEmpty(tvValue)) 
            		tvValue = UmlPackage.GetTaggedValue("uniqueID").Value;
            	return tvValue;
        	}
        }

        ///<summary>
        /// Tagged value 'versionIdentifier'.
        ///</summary>
        public string VersionIdentifier
        {
            get 
            {
            	string tvValue = UmlPackage.GetTaggedValue("versionIdentifier").Value;
            	if (string.IsNullOrEmpty(tvValue)) 
            		tvValue = UmlPackage.GetTaggedValue("versionID").Value;
            	return tvValue;
        	}
        }

        ///<summary>
        /// Tagged value 'baseURN'.
        ///</summary>
        public string BaseURN
        {
            get { return UmlPackage.GetTaggedValue("baseURN").Value; }
        }

        ///<summary>
        /// Tagged value 'namespacePrefix'.
        ///</summary>
        public string NamespacePrefix
        {
            get { return UmlPackage.GetTaggedValue("namespacePrefix").Value; }
        }

		#endregion
	}
}
