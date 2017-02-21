// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using EA;
using Attribute=EA.Attribute;

namespace VIENNAAddInUnitTests.TestRepository
{
    internal class EAAttribute : Attribute, IEACollectionElement
    {
        public EAAttribute(EARepository repository, int elementId)
        {
            TaggedValues = new EAAttributeTagCollection(repository, this);
            ParentID = elementId;
            Default = "";
            UpperBound = "1";
            LowerBound = "1";
        }

        #region Attribute Members

        public bool Update()
        {
            return true;
        }

        public string GetLastError()
        {
            throw new NotImplementedException();
        }

        public string Name { get; set; }

        public string Visibility
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Stereotype { get; set; }

        public string Containment
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsStatic
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsCollection
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsOrdered
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool AllowDuplicates
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string LowerBound { get; set; }

        public string UpperBound { get; set; }

        public string Container
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Notes
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsDerived
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int AttributeID { get; set; }

        public int Pos
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Length
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Precision
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Scale
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsConst
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Style
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int ClassifierID { get; set; }
//        {
//            get { return (ClassifierPath != null ? Repository.Resolve<Element>(ClassifierPath).ElementID : classifierId); }
//            set { classifierId = value; }
//        }

        public string Default { get; set; }

        public string Type { get; set; }
//        {
//            get { return Repository.Resolve<Element>(ClassifierPath).Name; }
//            set { }
//        }

        public Collection Constraints
        {
            get { throw new NotImplementedException(); }
        }

        public Collection TaggedValues { get; private set; }

        public string AttributeGUID
        {
            get { return AttributeID.ToString(); }
            set { throw new NotImplementedException(); }
        }

        public string StyleEx
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public ObjectType ObjectType
        {
            get { throw new NotImplementedException(); }
        }

        public int ParentID { get; private set; }

        public string StereotypeEx
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Collection TaggedValuesEx
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IEACollectionElement Members

        string IEACollectionElement.Name
        {
            get { return Name; }
            set { Name = value; }
        }

        #endregion
    }
}