using System.Collections.Generic;
using System.Linq;
using System;

namespace CctsRepository
{
	/// <summary>
	/// Description of ICctsAttribute.
	/// </summary>
	public interface ICctsAttribute : ICctsProperty
	{
		IEnumerable<ICctsFacet> AllFacets {get;}
		IEnumerable<ICctsFacet> Facets {get;}
		
		///<summary>
        /// Tagged value 'modificationAllowedIndicator'.
        ///</summary>
		string ModificationAllowedIndicator { get; }

        ///<summary>
        /// Tagged value 'enumeration'.
        ///</summary>
        string Enumeration { get; }

        ///<summary>
        /// Tagged value 'fractionDigits'.
        ///</summary>
		string FractionDigits { get; }
		
		///<summary>
        /// Tagged value 'fractionDigits'.
        ///</summary>
		string Length { get; }

        ///<summary>
        /// Tagged value 'maximumExclusive'.
        ///</summary>
		string MaximumExclusive { get; }

        ///<summary>
        /// Tagged value 'maximumInclusive'.
        ///</summary>
		string MaximumInclusive { get; }

        ///<summary>
        /// Tagged value 'maximumLength'.
        ///</summary>
		string MaximumLength { get; }

        ///<summary>
        /// Tagged value 'minimumExclusive'.
        ///</summary>
		string MinimumExclusive { get; }

        ///<summary>
        /// Tagged value 'minimumInclusive'.
        ///</summary>
		string MinimumInclusive { get; }

        ///<summary>
        /// Tagged value 'minimumLength'.
        ///</summary>
		string MinimumLength { get; }
		
		///<summary>
        /// Tagged value 'pattern'.
        ///</summary>
		string Pattern { get; }

        ///<summary>
        /// Tagged value 'totalDigits'.
        ///</summary>
		string TotalDigits { get; }
		
		///<summary>
        /// Tagged value 'whitespace'.
        ///</summary>
		string WhiteSpace { get; }


	}
}
