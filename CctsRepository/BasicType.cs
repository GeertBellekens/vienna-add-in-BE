
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
    public class BasicType
    {
        private readonly ICctsElement actualType;
        public ICctsElement ActualType
        {
        	get { return actualType; }
        }

        public BasicType(ICctsElement actualType)
        {
            this.actualType = actualType;
        }
        public EnumerationType EnumerationType
        {
        	get
        	{
        		//only valid for enums
        		if (IsEnum)
        		{
        			return Enum.EnumerationType;
        		}
        		else return EnumerationType.Regular;
        	}
        }

		/// <summary>
		/// The actual type's ID.
		/// </summary>
        public int Id
		{ 
			get
			{
				return actualType.Id;
			}
		}
		
		/// <summary>
		/// The actual type's name.
		/// </summary>
        public string Name
		{ 
			get
			{
				return actualType.Name;
			}
		}
		

        /// <summary>
        /// <c>true</c> if the actual type is a(n) PRIM.
        /// </summary>
		public bool IsPrim
		{
			get
			{
				return actualType is IPrim;
			}
		}
		
        /// <summary>
        /// The actual type, if it is a(n) PRIM.
        /// </summary>
		public IPrim Prim
		{
			get
			{
				return actualType as IPrim;
			}
		}

        /// <summary>
        /// <c>true</c> if the actual type is a(n) IDSCHEME.
        /// </summary>
		public bool IsIdScheme
		{
			get
			{
				return actualType is IIdScheme;
			}
		}
		
        /// <summary>
        /// The actual type, if it is a(n) IDSCHEME.
        /// </summary>
		public IIdScheme IdScheme
		{
			get
			{
				return actualType as IIdScheme;
			}
		}

        /// <summary>
        /// <c>true</c> if the actual type is a(n) ENUM.
        /// </summary>
		public bool IsEnum
		{
			get
			{
				return actualType is IEnum;
			}
		}
		
        /// <summary>
        /// The actual type, if it is a(n) ENUM.
        /// </summary>
		public IEnum Enum
		{
			get
			{
				return actualType as IEnum;
			}
		}

		#region Common Tagged Values

        /// <summary>
        /// The actual type's tagged value 'businessTerm'.
        /// </summary>
		public IEnumerable<string> BusinessTerms 
		{
			get
			{
				return actualType.BusinessTerms;
			}
		}

        /// <summary>
        /// The actual type's tagged value 'definition'.
        /// </summary>
		public string Definition 
		{
			get
			{
				return actualType.Definition;
			}
		}

        /// <summary>
        /// The actual type's tagged value 'dictionaryEntryName'.
        /// </summary>
		public string DictionaryEntryName 
		{
			get
			{
				return actualType.DictionaryEntryName;
			}
		}

        /// <summary>
        /// The actual type's tagged value 'uniqueIdentifier'.
        /// </summary>
		public string UniqueIdentifier 
		{
			get
			{
				return actualType.UniqueIdentifier;
			}
		}

        /// <summary>
        /// The actual type's tagged value 'versionIdentifier'.
        /// </summary>
		public string VersionIdentifier 
		{
			get
			{
				return actualType.VersionIdentifier;
			}
		}

		#endregion
	}
}

