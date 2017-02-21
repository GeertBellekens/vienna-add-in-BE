using System;
using EA;

namespace VIENNAAddInUnitTests.TestRepository
{
    internal class EAConnectorEnd : ConnectorEnd
    {
        public EAConnectorEnd()
        {
            Role = "";
        }

        #region ConnectorEnd Members

        public bool Update()
        {
            // do nothing
            return true;
        }

        public string GetLastError()
        {
            throw new NotImplementedException();
        }

        public string End
        {
            get { throw new NotImplementedException(); }
        }

        public string Cardinality { get; set; }

        public string Visibility
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Role { get; set; }

        public string RoleType
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string RoleNote
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Containment { get; set;}

        public int Aggregation { get; set; }

        public int Ordering
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Qualifier
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Constraint
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsNavigable
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string IsChangeable
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Collection TaggedValues
        {
            get { throw new NotImplementedException(); }
        }

        public string Stereotype
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public ObjectType ObjectType
        {
            get { throw new NotImplementedException(); }
        }

        public string StereotypeEx
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Navigable
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool OwnedByClassifier
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool Derived
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool DerivedUnion
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool AllowDuplicates
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Alias
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        #endregion
    }
}