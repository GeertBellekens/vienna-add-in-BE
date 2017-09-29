using System.Collections.Generic;
using System.Linq;
using System;
using CctsRepository;

namespace VIENNAAddIn.upcc3.repo
{
	/// <summary>
	/// Description of UpccFacet.
	/// </summary>
	public class UpccFacet: ICctsFacet
	{
		public UpccFacet (string name, string content)
		{
			this.name = name;
			this.content = content;
		}
		public string name {get;set;}

		public string content {get;set;}
		
	}
}
