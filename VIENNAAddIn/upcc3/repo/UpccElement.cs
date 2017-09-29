using System.Collections.Generic;
using System.Linq;
using System;
using CctsRepository;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.repo
{
	/// <summary>
	/// Description of UpccElement.
	/// </summary>
	public abstract class UpccElement: ICctsElement
	{

		protected UpccElement(IUmlClassifier umlClasssifier)
        {
            UmlClassifier = umlClasssifier;
        }
		
		public IUmlClassifier UmlClassifier { get; private set; }

 		public int Id
        {
            get { return UmlClassifier.Id; }
        }

        public string Name
        {
            get { return UmlClassifier.Name; }
		}
        List<ICctsProperty> _properties;
		public IEnumerable<ICctsProperty> Properties 
		{
			get 
			{
				if (_properties == null)
				{
					_properties = new List<ICctsProperty>();
					foreach (var attribute in UmlClassifier.Attributes) 
					{
						var newAttribute = this.CreateAttribute(attribute);
						if (newAttribute != null) _properties.Add(newAttribute);
					}
					foreach (var association in UmlClassifier.Associations) 
					{
						var newAssociation = this.CreateAssociation(association);
						if (newAssociation != null) _properties.Add(newAssociation);
					}
					this.setPropertyOrder();
				}
				return _properties;
			}
		}
		private void setPropertyOrder()
		{
			var orderdProperties = this.Properties.OrderBy(x => x.position).ThenBy(y => y.Name);
			this._properties = orderdProperties.ToList();
			//set the position attributes
			for (int i = 0; i < this._properties.Count; i++) 
			{
				this._properties[i].position = i;
			}
		}
		protected abstract ICctsAttribute CreateAttribute(IUmlAttribute attribute);

		protected abstract ICctsAssociation CreateAssociation(IUmlAssociation association);

		public IEnumerable<ICctsAttribute> Attributes
		{
			get {return this.Properties.OfType<ICctsAttribute>();}
		}
		
		public IEnumerable<ICctsAssociation> Associations 
		{
			get {return this.Properties.OfType<ICctsAssociation>();}
		}     
        
        public abstract ICctsLibrary library {get;}
		        ///<summary>
        /// Tagged value 'businessTerm'.
        ///</summary>
        public IEnumerable<string> BusinessTerms
        {
            get { return UmlClassifier.GetTaggedValue("businessTerm").SplitValues; }
        }

        ///<summary>
        /// Tagged value 'definition'.
        ///</summary>
        public string Definition
        {
            get { return UmlClassifier.GetTaggedValue("definition").Value; }
        }

        ///<summary>
        /// Tagged value 'dictionaryEntryName'.
        ///</summary>
        public string DictionaryEntryName
        {
            get { return UmlClassifier.GetTaggedValue("dictionaryEntryName").Value; }
        }

        ///<summary>
        /// Tagged value 'languageCode'.
        ///</summary>
        public string LanguageCode
        {
            get { return UmlClassifier.GetTaggedValue("languageCode").Value; }
        }

        ///<summary>
        /// Tagged value 'uniqueIdentifier'.
        ///</summary>
        public string UniqueIdentifier
        {
            get 
            {
            	var tvId = UmlClassifier.GetTaggedValue("uniqueID");
            	if (string.IsNullOrEmpty(tvId.Value)) tvId = UmlClassifier.GetTaggedValue("uniqueIdentifier"); 
            	return tvId.Value;
            }
        }

        ///<summary>
        /// Tagged value 'versionIdentifier'.
        ///</summary>
        public string VersionIdentifier
        {
        	get
        	{
                string tvValue = UmlClassifier.GetTaggedValue("versionIdentifier").Value;
            	if (string.IsNullOrEmpty(tvValue)) 
            		tvValue = UmlClassifier.GetTaggedValue("versionID").Value;
            	return tvValue;
        	}
        }

        ///<summary>
        /// Tagged value 'usageRule'.
        ///</summary>
        public IEnumerable<string> UsageRules
        {
            get { return UmlClassifier.GetTaggedValue("usageRule").SplitValues; }
        }
	}
}
