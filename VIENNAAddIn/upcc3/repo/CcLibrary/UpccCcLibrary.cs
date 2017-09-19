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

namespace VIENNAAddIn.upcc3.repo.CcLibrary
{
    internal class UpccCcLibrary : UpccLibrary, ICcLibrary
    {
        public UpccCcLibrary(IUmlPackage umlPackage):base(umlPackage)
    	{}


		/// <summary>
		/// The bLibrary containing this CCLibrary.
		/// </summary>
		public IBLibrary BLibrary
        {
            get { return new UpccBLibrary(UmlPackage.Parent); }
        }

		/// <summary>
		/// The ACCs contained in this CCLibrary.
		/// </summary>
		public IEnumerable<IAcc> Accs
		{
			get { return this.Elements.OfType<IAcc>(); }
		}

		/// <summary>
		/// Retrieves a ACC by name.
		/// <param name="name">A ACC's name.</param>
		/// <returns>The ACC with the given <paramref name="name"/> or <c>null</c> if no such ACC is found.</returns>
		/// </summary>
        public IAcc GetAccByName(string name)
		{
            foreach (IAcc acc in Accs)
            {
                if (acc.Name == name)
                {
                    return acc;
                }
            }
            return null;
		}

		/// <summary>
		/// Creates a ACC based on the given <paramref name="specification"/>.
		/// <param name="specification">A specification for a ACC.</param>
		/// <returns>The newly created ACC.</returns>
		/// </summary>
		public IAcc CreateAcc(AccSpec specification)
		{
		    return new UpccAcc(UmlPackage.CreateClass(AccSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Updates a ACC to match the given <paramref name="specification"/>.
		/// <param name="acc">A ACC.</param>
		/// <param name="specification">A new specification for the given ACC.</param>
		/// <returns>The updated ACC. Depending on the implementation, this might be the same updated instance or a new instance!</returns>
		/// </summary>
        public IAcc UpdateAcc(IAcc acc, AccSpec specification)
		{
		    return new UpccAcc(UmlPackage.UpdateClass(((UpccAcc) acc).UmlClass, AccSpecConverter.Convert(specification)));
		}

		/// <summary>
		/// Removes a ACC from this CCLibrary.
		/// <param name="acc">A ACC.</param>
		/// </summary>
        public void RemoveAcc(IAcc acc)
		{
            UmlPackage.RemoveClass(((UpccAcc) acc).UmlClass);
		}

        public bool Equals(UpccCcLibrary other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.UmlPackage, UmlPackage);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (UpccCcLibrary)) return false;
            return Equals((UpccCcLibrary) obj);
        }

        public override int GetHashCode()
        {
            return (UmlPackage != null ? UmlPackage.GetHashCode() : 0);
        }

        public static bool operator ==(UpccCcLibrary left, UpccCcLibrary right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UpccCcLibrary left, UpccCcLibrary right)
        {
            return !Equals(left, right);
        }
	}
}
