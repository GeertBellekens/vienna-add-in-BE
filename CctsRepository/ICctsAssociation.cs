using System.Collections.Generic;
using System.Linq;
using System;

namespace CctsRepository
{
	/// <summary>
	/// Description of ICctsAssociation.
	/// </summary>
	public interface ICctsAssociation : ICctsProperty
	{
		
        AggregationKind AggregationKind { get; }
        /// <summary>
        /// Source Element
        /// </summary>
        ICctsElement AssociatingElement { get;}
        /// <summary>
        /// Target Element
        /// </summary>
        ICctsElement AssociatedElement { get;}
        
	}
}
