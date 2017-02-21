/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using EA;

namespace VIENNAAddIn.Utils
{
	/// <sUMM2ary>
	/// SUMM2ary description for EAElementWrapper.
	/// </sUMM2ary>
	public class EAElementWrapper {

		private String name = "";
		private String stereotype = "";
		private String type = "";

		internal String Name {
			get { return name;}
		}

		internal String Type {
			get { return type;}
		}

		internal String Stereotype {
			get { return stereotype;}
		}

		public EAElementWrapper(EA.Diagram d){			
			this.name = d.Name;
			this.stereotype = d.Stereotype;	
			this.type = d.Type;
		}

		public EAElementWrapper(EA.Package p){			
			this.name = p.Name;
			this.stereotype = p.Element.Stereotype;	
			this.type = "";
		}


		public EAElementWrapper(EA.Element e){			
			this.name = e.Name;
			this.stereotype = e.Stereotype;	
			this.type = e.Type;
		}

		public EAElementWrapper(EA.Connector con, Repository repository) {
            EA.Element client = repository.GetElementByID(con.ClientID);
            EA.Element supplier = repository.GetElementByID(con.SupplierID);
			this.name = con.Name + "(from: " + client.Name + " to: " + supplier.Name + ")";
			this.stereotype = con.Stereotype;
			this.type = con.Type;
		}

	}
}
