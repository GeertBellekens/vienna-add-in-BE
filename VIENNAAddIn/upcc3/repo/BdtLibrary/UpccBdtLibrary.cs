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

namespace VIENNAAddIn.upcc3.repo.BdtLibrary
{
    internal class UpccBdtLibrary : UpccLibrary, IBdtLibrary
    {
    	public UpccBdtLibrary(IUmlPackage umlPackage):base(umlPackage)
    	{}

		/// <summary>
		/// The bLibrary containing this BDTLibrary.
		/// </summary>
		public IBLibrary BLibrary
        {
            get { return new UpccBLibrary(UmlPackage.Parent); }
        }

		/// <summary>
		/// The BDTs contained in this BDTLibrary.
		/// </summary>
		public IEnumerable<IBdt> Bdts
		{
			get { return this.Elements.OfType<IBdt>(); }
		}

		/// <summary>
		/// Retrieves a BDT by name.
		/// <param name="name">A BDT's name.</param>
		/// <returns>The BDT with the given <paramref name="name"/> or <c>null</c> if no such BDT is found.</returns>
		/// </summary>
        public IBdt GetBdtByName(string name)
		{
            foreach (IBdt bdt in Bdts)
            {
                if (bdt.Name == name)
                {
                    return bdt;
                }
            }
            return null;
		}

		/// <summary>
		/// Creates a BDT based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a BDT.</param>
		/// <returns>The newly created BDT.</returns>
		/// </summary>
		public IBdt CreateBdt(BdtSpec specification)
		{
		    return new UpccBdt(UmlPackage.CreateClass(BdtSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Updates a BDT to match the given <paramref name="specification"/>.
		/// <param name="bdt">A BDT.</param>
		/// <param name="specification">A new specification for the given BDT.</param>
		/// <returns>The updated BDT. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IBdt UpdateBdt(IBdt bdt, BdtSpec specification)
		{
		    return new UpccBdt(UmlPackage.UpdateClass(((UpccBdt) bdt).UmlClass, BdtSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Removes a BDT from this BDTLibrary.
		/// <param name="bdt">A BDT.</param>
		/// </summary>
        public void RemoveBdt(IBdt bdt)
		{
            UmlPackage.RemoveClass(((UpccBdt) bdt).UmlClass);
		}

        public bool Equals(UpccBdtLibrary other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.UmlPackage, UmlPackage);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (UpccBdtLibrary)) return false;
            return Equals((UpccBdtLibrary) obj);
        }

        public override int GetHashCode()
        {
            return (UmlPackage != null ? UmlPackage.GetHashCode() : 0);
        }

        public static bool operator ==(UpccBdtLibrary left, UpccBdtLibrary right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UpccBdtLibrary left, UpccBdtLibrary right)
        {
            return !Equals(left, right);
        }
	}
}
