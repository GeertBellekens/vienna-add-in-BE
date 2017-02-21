
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
        private readonly object actualType;

        public BasicType(object actualType)
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
				if (IsPrim)
				{
					return Prim.Id;
				}
				if (IsIdScheme)
				{
					return IdScheme.Id;
				}
				if (IsEnum)
				{
					return Enum.Id;
				}
				throw new Exception("Invalid BasicType: " + actualType.GetType());
			}
		}
		
		/// <summary>
		/// The actual type's name.
		/// </summary>
        public string Name
		{ 
			get
			{
				if (IsPrim)
				{
					return Prim.Name;
				}
				if (IsIdScheme)
				{
					return IdScheme.Name;
				}
				if (IsEnum)
				{
					return Enum.Name;
				}
				throw new Exception("Invalid BasicType: " + actualType.GetType());
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
				if (IsPrim)
				{
					return Prim.BusinessTerms;
				}
				if (IsIdScheme)
				{
					return IdScheme.BusinessTerms;
				}
				if (IsEnum)
				{
					return Enum.BusinessTerms;
				}
				throw new Exception("Invalid BasicType: " + actualType.GetType());
			}
		}

        /// <summary>
        /// The actual type's tagged value 'definition'.
        /// </summary>
		public string Definition 
		{
			get
			{
				if (IsPrim)
				{
					return Prim.Definition;
				}
				if (IsIdScheme)
				{
					return IdScheme.Definition;
				}
				if (IsEnum)
				{
					return Enum.Definition;
				}
				throw new Exception("Invalid BasicType: " + actualType.GetType());
			}
		}

        /// <summary>
        /// The actual type's tagged value 'dictionaryEntryName'.
        /// </summary>
		public string DictionaryEntryName 
		{
			get
			{
				if (IsPrim)
				{
					return Prim.DictionaryEntryName;
				}
				if (IsIdScheme)
				{
					return IdScheme.DictionaryEntryName;
				}
				if (IsEnum)
				{
					return Enum.DictionaryEntryName;
				}
				throw new Exception("Invalid BasicType: " + actualType.GetType());
			}
		}

        /// <summary>
        /// The actual type's tagged value 'uniqueIdentifier'.
        /// </summary>
		public string UniqueIdentifier 
		{
			get
			{
				if (IsPrim)
				{
					return Prim.UniqueIdentifier;
				}
				if (IsIdScheme)
				{
					return IdScheme.UniqueIdentifier;
				}
				if (IsEnum)
				{
					return Enum.UniqueIdentifier;
				}
				throw new Exception("Invalid BasicType: " + actualType.GetType());
			}
		}

        /// <summary>
        /// The actual type's tagged value 'versionIdentifier'.
        /// </summary>
		public string VersionIdentifier 
		{
			get
			{
				if (IsPrim)
				{
					return Prim.VersionIdentifier;
				}
				if (IsIdScheme)
				{
					return IdScheme.VersionIdentifier;
				}
				if (IsEnum)
				{
					return Enum.VersionIdentifier;
				}
				throw new Exception("Invalid BasicType: " + actualType.GetType());
			}
		}

		#endregion
	}
}

