// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using EA;

namespace VIENNAAddInUnitTests.TestRepository
{
    public abstract class EACollection : Collection
    {
        #region Delegates

        public delegate IEACollectionElement ElementFactory(string name, string type, int containerId);

        #endregion

        protected readonly Func<int> containerId;
        protected readonly ElementFactory createElement;
        private readonly List<IEACollectionElement> elements = new List<IEACollectionElement>();

        protected EACollection(ObjectType objectType, ElementFactory elementFactory, Func<int> containerId)
        {
            ObjectType = objectType;
            this.createElement = elementFactory;
            this.containerId = containerId;
        }

        public List<IEACollectionElement> Elements
        {
            get { return elements; }
        }

        #region Collection Members

        public object GetAt(short index)
        {
            return Elements[index];
        }

        public void DeleteAt(short index, bool Refresh)
        {
            Delete(index);
        }

        public string GetLastError()
        {
            throw new NotImplementedException();
        }

        public object GetByName(string name)
        {
            if (Elements.Count == 0)
            {
                return null;
            }
            IEACollectionElement element = Elements.Find(e => e.Name == name);
            if (element == null)
            {
                throw new IndexOutOfRangeException(name);
            }
            return element;
        }

        public void Refresh()
        {
            // do nothing
        }

        public virtual object AddNew(string Name, string Type)
        {
            IEACollectionElement element = createElement(Name, Type, containerId());
            Elements.Add(element);
            Elements.Sort(CompareElementsByName);
            return element;
        }

        private int CompareElementsByName(IEACollectionElement e1, IEACollectionElement e2)
        {
            return String.Compare(e1.Name, e2.Name);
        }

        public virtual void Delete(short index)
        {
            Elements.RemoveAt(index);
        }

        IEnumerator IDualCollection.GetEnumerator()
        {
            return GetEnumerator();
        }

        public short Count
        {
            get { return (short) Elements.Count; }
        }

        public ObjectType ObjectType { get; set; }

        public IEnumerator GetEnumerator()
        {
            return new List<IEACollectionElement>(Elements).GetEnumerator();
        }

        #endregion
    }

    internal class EATaggedValueCollection : EACollection
    {
        public EATaggedValueCollection(EARepository repository, EAElement element)
            : base(ObjectType.otTaggedValue, EARepository.CreateTaggedValue, () => element.ElementID)
        {
        }
    }

    internal class EADiagramObjectCollection : EACollection
    {
        public EADiagramObjectCollection(EARepository repository, EADiagram diagram)
            : base(ObjectType.otDiagramObject, EARepository.CreateDiagramObject, () => diagram.DiagramID)
        {
        }
    }

    internal class EAAttributeTagCollection : EACollection
    {
        public EAAttributeTagCollection(EARepository repository, EAAttribute attribute)
            : base(ObjectType.otAttributeTag, EARepository.CreateAttributeTag, () => attribute.AttributeID)
        {
        }
    }

    internal class EAConnectorTagCollection : EACollection
    {
        public EAConnectorTagCollection(EARepository repository, EAConnector connector)
            : base(ObjectType.otConnectorTag, repository.CreateConnectorTag, () => connector.ConnectorID)
        {
        }
    }

    internal class EAPackageCollection : EACollection
    {
        public EAPackageCollection(EARepository repository, EAPackage parent)
            : base(ObjectType.otPackage, repository.CreatePackage, () => parent != null ? parent.PackageID : 0)
        {
        }
    }

    internal class EAAttributeCollection : EACollection
    {
        public EAAttributeCollection(EARepository repository, EAElement element)
            : base(ObjectType.otAttribute, repository.CreateAttribute, () => element.ElementID)
        {
        }
    }

    internal class EAConnectorCollection : EACollection
    {
        private readonly EARepository repository;

        public EAConnectorCollection(EARepository repository, EAElement element)
            : base(ObjectType.otConnector, repository.CreateConnector, () => element.ElementID)
        {
            this.repository = repository;
        }

        public override object AddNew(string Name, string Type)
        {
            var connector = (EAConnector) createElement(Name, Type, containerId());
            repository.AddConnector(connector);
            return connector;
        }

        public override void Delete(short index)
        {
            var connector = (EAConnector) Elements[index];
            repository.RemoveConnector(connector);
        }
    }

    internal class EAElementCollection : EACollection
    {
        public EAElementCollection(EARepository repository, EAPackage package)
            : base(ObjectType.otElement, repository.CreateElement, () => package.PackageID)
        {
        }
    }

    internal class EADiagramCollection : EACollection
    {
        public EADiagramCollection(EARepository repository, EAPackage package)
            : base(ObjectType.otDiagram, repository.CreateDiagram, () => package.PackageID)
        {
        }
    }

    public interface IEACollectionElement
    {
        string Name { get; set; }
    }
}