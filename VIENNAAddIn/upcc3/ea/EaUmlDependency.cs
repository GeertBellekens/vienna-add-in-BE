using System;
using EA;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.ea
{
    internal class EaUmlDependency : IUmlDependency, IEquatable<EaUmlDependency>
    {
        private readonly Connector eaConnector;
        private readonly Repository eaRepository;

        public EaUmlDependency(Repository eaRepository, Connector eaConnector)
        {
            this.eaRepository = eaRepository;
            this.eaConnector = eaConnector;
        }

        #region IUmlDependency Members

        public IUmlClassifier Target
        {
            get
            {
                Element targetElement = eaRepository.GetElementByID(eaConnector.SupplierID);
                return new EaUmlClassifier(eaRepository, targetElement);
            }
        }

        #endregion

        public bool Equals(EaUmlDependency other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.eaConnector.ConnectorID, eaConnector.ConnectorID);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (EaUmlDependency)) return false;
            return Equals((EaUmlDependency) obj);
        }

        public override int GetHashCode()
        {
            return (eaConnector != null ? eaConnector.ConnectorID : 0);
        }

        public static bool operator ==(EaUmlDependency left, EaUmlDependency right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EaUmlDependency left, EaUmlDependency right)
        {
            return !Equals(left, right);
        }
    }
}