// ReSharper disable RedundantUsingDirective
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.BLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using CctsRepository.DocLibrary;
using CctsRepository.EnumLibrary;
using CctsRepository.PrimLibrary;
using VIENNAAddIn.upcc3.repo;
using VIENNAAddIn.upcc3.repo.BdtLibrary;
using VIENNAAddIn.upcc3.repo.BieLibrary;
using VIENNAAddIn.upcc3.repo.BLibrary;
using VIENNAAddIn.upcc3.repo.CcLibrary;
using VIENNAAddIn.upcc3.repo.CdtLibrary;
using VIENNAAddIn.upcc3.repo.DocLibrary;
using VIENNAAddIn.upcc3.repo.EnumLibrary;
using VIENNAAddIn.upcc3.repo.PrimLibrary;
// ReSharper restore RedundantUsingDirective
using System;
using System.Linq;
using System.Collections.Generic;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.repo.PrimLibrary
{
    internal class UpccPrimLibrary : UpccLibrary, IPrimLibrary
    {
    	public UpccPrimLibrary(IUmlPackage umlPackage):base(umlPackage)
        {         
        }

        public IUmlPackage UmlPackage { get; private set; }

		/// <summary>
		/// The bLibrary containing this PRIMLibrary.
		/// </summary>
		public IBLibrary BLibrary
        {
            get { return new UpccBLibrary(UmlPackage.Parent); }
        }

		/// <summary>
		/// The PRIMs contained in this PRIMLibrary.
		/// </summary>
		public IEnumerable<IPrim> Prims
		{
            get
            {
            	return this.Elements.OfType<IPrim>();
            }
		}

		/// <summary>
		/// Retrieves a PRIM by name.
		/// <param name="name">A PRIM's name.</param>
		/// <returns>The PRIM with the given <paramref name="name"/> or <c>null</c> if no such PRIM is found.</returns>
		/// </summary>
        public IPrim GetPrimByName(string name)
		{
            foreach (IPrim prim in Prims)
            {
                if (prim.Name == name)
                {
                    return prim;
                }
            }
            return null;
		}

		/// <summary>
		/// Creates a PRIM based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a PRIM.</param>
		/// <returns>The newly created PRIM.</returns>
		/// </summary>
		public IPrim CreatePrim(PrimSpec specification)
		{
		    return new UpccPrim(UmlPackage.CreateDataType(PrimSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Updates a PRIM to match the given <paramref name="specification"/>.
		/// <param name="prim">A PRIM.</param>
		/// <param name="specification">A new specification for the given PRIM.</param>
		/// <returns>The updated PRIM. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IPrim UpdatePrim(IPrim prim, PrimSpec specification)
		{
		    return new UpccPrim(UmlPackage.UpdateDataType(((UpccPrim) prim).UmlDataType, PrimSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Removes a PRIM from this PRIMLibrary.
		/// <param name="prim">A PRIM.</param>
		/// </summary>
        public void RemovePrim(IPrim prim)
		{
            UmlPackage.RemoveDataType(((UpccPrim) prim).UmlDataType);
		}

        

        public bool Equals(UpccPrimLibrary other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.UmlPackage, UmlPackage);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (UpccPrimLibrary)) return false;
            return Equals((UpccPrimLibrary) obj);
        }

        public override int GetHashCode()
        {
            return (UmlPackage != null ? UmlPackage.GetHashCode() : 0);
        }

        public static bool operator ==(UpccPrimLibrary left, UpccPrimLibrary right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UpccPrimLibrary left, UpccPrimLibrary right)
        {
            return !Equals(left, right);
        }
	}
}
