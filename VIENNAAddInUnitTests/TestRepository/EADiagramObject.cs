using System;
using EA;

namespace VIENNAAddInUnitTests.TestRepository
{
    public class EADiagramObject : DiagramObject, IEACollectionElement
    {
        public bool Update()
        {
            // do nothing
            return true;
        }

        public string GetLastError()
        {
            throw new NotImplementedException();
        }

        public int DiagramID { get; set; }

        public int ElementID { get; set; }

        public int left
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int right
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int top
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int bottom
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int Sequence
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public object Style
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int InstanceID
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public ObjectType ObjectType
        {
            get { throw new NotImplementedException(); }
        }

        public string Name { get; set; }
    }
}