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
    public class EAAttributeTag : IEACollectionElement, AttributeTag
    {
        #region AttributeTag Members

        public bool Update()
        {
            return true;
        }

        public string GetLastError()
        {
            throw new NotImplementedException();
        }

        public int TagID
        {
            get { throw new NotImplementedException(); }
        }

        public int AttributeID { get; set;}

        public string Name { get; set; }

        public string Value { get; set; }

        public string Notes
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string TagGUID
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public ObjectType ObjectType
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