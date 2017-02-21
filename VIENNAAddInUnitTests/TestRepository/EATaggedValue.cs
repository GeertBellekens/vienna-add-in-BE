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

namespace VIENNAAddInUnitTests.TestRepository
{
    internal class EATaggedValue : IEACollectionElement, TaggedValue
    {
        #region IEACollectionElement Members

        string IEACollectionElement.Name
        {
            get { return Name; }
            set { Name = value; }
        }

        #endregion

        #region TaggedValue Members

        public bool Update()
        {
            return true;
        }

        public string GetLastError()
        {
            throw new NotImplementedException();
        }

        public int PropertyID
        {
            get { throw new NotImplementedException(); }
        }

        public string PropertyGUID
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Name{ get; set;}

        public string Value { get; set;}

        public string Notes
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int ElementID { get; set; }

        public ObjectType ObjectType
        {
            get { throw new NotImplementedException(); }
        }

        public int ParentID
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}