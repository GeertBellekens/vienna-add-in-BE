using System;
using EA;

namespace VIENNAAddInUnitTests.TestRepository
{
    public class EAConnectorTag : IEACollectionElement, ConnectorTag
    {
        #region ConnectorTag Members

        public bool Update()
        {
            return true;
        }

        public string GetLastError()
        {
            throw new NotImplementedException();
        }

        public int TagID { get; set; }

        public int ConnectorID { get; set;}

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