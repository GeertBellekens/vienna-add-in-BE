
// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using System.Collections.Generic;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.BLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using CctsRepository.DocLibrary;
using CctsRepository.EnumLibrary;
using CctsRepository.PrimLibrary;

namespace CctsRepository
{
    public class BieAggregator
    {
        private readonly object actualType;

        public BieAggregator(object actualType)
        {
            this.actualType = actualType;
        }

		/// <summary>
		/// The actual type's ID.
		/// </summary>
        public int Id
		{ 
			get
			{
				if (IsAbie)
				{
					return Abie.Id;
				}
				if (IsMa)
				{
					return Ma.Id;
				}
				throw new Exception("Invalid BieAggregator: " + actualType.GetType());
			}
		}
		
		/// <summary>
		/// The actual type's name.
		/// </summary>
        public string Name
		{ 
			get
			{
				if (IsAbie)
				{
					return Abie.Name;
				}
				if (IsMa)
				{
					return Ma.Name;
				}
				throw new Exception("Invalid BieAggregator: " + actualType.GetType());
			}
		}
		

        /// <summary>
        /// <c>true</c> if the actual type is a(n) ABIE.
        /// </summary>
		public bool IsAbie
		{
			get
			{
				return actualType is IAbie;
			}
		}
		
        /// <summary>
        /// The actual type, if it is a(n) ABIE.
        /// </summary>
		public IAbie Abie
		{
			get
			{
				return actualType as IAbie;
			}
		}

        /// <summary>
        /// <c>true</c> if the actual type is a(n) MA.
        /// </summary>
		public bool IsMa
		{
			get
			{
				return actualType is IMa;
			}
		}
		
        /// <summary>
        /// The actual type, if it is a(n) MA.
        /// </summary>
		public IMa Ma
		{
			get
			{
				return actualType as IMa;
			}
		}

		#region Common Tagged Values

		#endregion
	}
}

