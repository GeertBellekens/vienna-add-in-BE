/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections;
using System.Diagnostics;
using VIENNAAddIn.Utils;

namespace VIENNAAddIn.validator 
{
	/// <sUMM2ary>
	/// A Constraint
	/// </sUMM2ary>
	internal class ErrorMessage	{
		
		
		private String description     = null;
		private String originClass     = null;
		private String originMethod    = null;
		private IList  affectedElements = null;
		private String originPackage   = null;
		private String originView      = null;


		private bool isTaggedValueConstraintError = false;

		//This variable indicates, where the constraint was violated
		//BRV, BDV, BTV, -
		private String source        = "-";
		
			
		/// <sUMM2ary>
		/// Constructor 1
		/// </sUMM2ary>
		/// <param name="description">The description of the errormessage</param>
		internal ErrorMessage(String description) {
			this.description = description;
		}
         

		/// <sUMM2ary>
		/// Constructor 3
		/// </sUMM2ary>
		/// <param name="description">The description of the errormessage</param>
		/// <param name="o">The class, where the error was raised</param>
		/// <param name="st">The StackFrame</param>
		/// <param name="isTaggedValueConstraintError">If this parameter is set, 
		/// the error was raised during a tagged value validation</param>
		internal ErrorMessage (String description, object o, StackFrame st, bool isTaggedValueConstraintError) {
			this.isTaggedValueConstraintError = true;
			processConstruction(description, o, st);
		}

		/// <sUMM2ary>
		/// Constructor 4 - called directly
		/// </sUMM2ary>
		/// <param name="description">The description of the errormessage</param>
		/// <param name="o">The class, where the error was raised as object</param>
		/// <param name="st">The StackFrame</param>
		/// <param name="affectedElement">The element, which was affected by the constraint</param>
		internal ErrorMessage (String description, object o, StackFrame st, EAElementWrapper affectedElement  ) {
			//Only one affectedElement has been passed over - convert it
			//to an ArrayList
			ArrayList l = new ArrayList();
			if (affectedElement != null) {				
				String s = "Name: ";
				if (affectedElement.Name == "")
					s += " - ";
				else
					s += affectedElement.Name;

				s += " Stereotype: ";

				if (affectedElement.Stereotype == "")
					s += " - ";
				else
					s += affectedElement.Stereotype;

				s += " Type: ";

				if (affectedElement.Type == "")
					s += " - ";
				else
					s += affectedElement.Type;
			
				l.Add(s);
			}
			this.affectedElements = l;
			
			processConstruction(description, o, st);
		}



		/// <sUMM2ary>
		/// Constructor 5 - called directly
		/// </sUMM2ary>
		/// <param name="description">The description of the errormessage</param>
		/// <param name="o">The class, where the error was raised as object</param>
		/// <param name="st">The StackFrame</param>
		/// <param name="affectedElement">The element, which was affected by the constraint</param>
		internal ErrorMessage (String description, object o, StackFrame st, IList affectedElements) {
			//Only one affectedElement has been passed over - convert it
			//to an ArrayList
			if (affectedElements != null) {
				ArrayList l = new ArrayList();
				foreach (EAElementWrapper wr in affectedElements) {
					String s = "Name: ";
					if (wr.Name == "")
						s += " - ";
					else
						s += wr.Name;

					s += " Stereotype: ";

					if (wr.Stereotype == "")
						s += " - ";
					else
						s += wr.Stereotype;

					s += " Type: ";

					if (wr.Type == "")
						s += " - ";
					else
						s += wr.Type;
					l.Add(s);
				}
				this.affectedElements = l;
			}
			processConstruction(description, o, st);
		}

		/// <sUMM2ary>
		/// Constructor 5
		/// </sUMM2ary>
		/// <param name="description">The description of the errormessage</param>
		/// <param name="o">The class, where the error was raised as object</param>
		/// <param name="st">The StackFrame</param>
		internal ErrorMessage(String description, object o, StackFrame st) {
			processConstruction(description, o, st);
		}

		/// <sUMM2ary>
		/// processes the object construction
		/// </sUMM2ary>
		/// <param name="description"></param>
		/// <param name="o"></param>
		/// <param name="st"></param>
		private void processConstruction(String description, object o, StackFrame st) {
			String source        = "";
			String originClass   = "";
			String originView    = "";
			String originMethod  = "";
			String originPackage = "";

			//If no source class is given
			//ignore
			if (o != null) {	
                //commented by Kristina for clean up process Aug 08
                //if (o.GetType().Equals(typeof (GIEM.Validator.BDVValidator))) {
                //    originClass = "BDVValidator";
                //    originView =  "Business Domain View";
                //    source = "BDV";
                //}
                //else if (o.GetType().Equals(typeof (GIEM.Validator.BRVValidator))) {
                //    originClass = "BRVValidator";
                //    originView = "Business Requirements View";
                //    source = "BRV";
                //}
                //else if (o.GetType().Equals(typeof (GIEM.Validator.BTVValidator))) {
                //    originClass = "BTVValidator";
                //    originView = "Business Transaction View";
                //    source = "BTV";
                //}
                //else if (o.GetType().Equals(typeof (GIEM.Validator.Validator))) {
                //    originClass = "Validator";						
                //    source = "Model";
                //}
                //else if (o.GetType().Equals(typeof (GIEM.Validator.BCMValidator))) {
                //    originClass = "BCMValidator";
                //    originView = "The model itself";
                //    source = "BCM";
                //}	

		
				//Try to determine the originPackage (BusinessProcessView, BusinessEntityView...)
				originPackage = getOriginPackage(o);
			}

						
			originMethod = st.GetMethod().ToString();

			try {
				int begin = originMethod.IndexOf("checkOCL_");
				int end   = originMethod.IndexOf("(");
				if (begin != -1) {
					int length = end - (begin + 9);
					originMethod = originMethod.Substring(begin + 9, length);
				}
			}
            catch (Exception e) { Console.WriteLine(e.StackTrace); }


			this.description   = description;
			this.originView    = originView;
			this.originClass   = originClass;
			this.originMethod  = originMethod;
			this.source        = source;
			this.originPackage = originPackage;
		}

		



		/// <sUMM2ary>
		/// Returns the source of the constraint violation
		/// </sUMM2ary>
		/// <returns></returns>
		internal String getSource() {
			return this.source;
		}

		/// <sUMM2ary>
		/// Build the message of an ErrorMessage, which is presented in the detail message
		/// window in the ValidatorForm
		/// </sUMM2ary>
		/// <returns></returns>
		internal String getDetailMessage() {
			String rv = "";
			
            //If no description is provided provide a standard sentence
			if (this.description == null || this.description == "") 
				rv = "No detail message available.";			
			else
				rv = description;

			rv += "\n";

			//Are there affected elements?
			if (this.affectedElements != null && this.affectedElements.Count != 0) {
				//We can be sure, that the objects which are included in the 
				//are fully qualified Strings of the form 
				//Name: nameOfElement Stereotype: stereotypeOfElement
				String aelem = "";
				foreach (String elementName in affectedElements) {
					aelem += elementName + "\n";
				}
				rv += aelem;
			}
			rv += "\n";

			//Get the package, where the error occured
			rv += "Package: ";
			if (this.originPackage == null || this.originPackage == "")				
				rv += "-";
			else
				rv += this.originPackage;

			rv +=  "\n";

			//Get the view, where the error occured
			rv += "View: ";
			if (this.originView == null || this.originView == "")
				rv += "-";
			else
				rv += originView;

			rv += "\n\n";

			//Get the class, where the error occured
			rv += "Class: ";
			if (this.originClass == null || this.originClass == "")
				rv += "-";
			else
				rv += this.originClass;

			rv += "\n";

			//Get the method, where the error occured
			rv += "Method: ";
			if (this.originMethod == null || this.originMethod == "")
				rv += "-";
			else
				rv += this.originMethod;

			return rv;
		}

		internal bool getTaggedValueConstraintError() {
			return isTaggedValueConstraintError;
		}
	


		/// <sUMM2ary>
		/// Returns the package, where the errorMessage originates from
		/// </sUMM2ary>
		/// <param name="o"></param>
		/// <returns></returns>
		private String getOriginPackage(object o) {
			String originPackage = "";
			//o cannot be null
            //commented by Kristina Aug'08, not sure on this Validator
            //if (o.GetType().Equals(typeof (GIEM.Validator.BCMValidator)) ||
            //    o.GetType().Equals(typeof (GIEM.Validator.BDVValidator)) ||
            //    o.GetType().Equals(typeof (GIEM.Validator.BRVValidator)) ||
            //    o.GetType().Equals(typeof (GIEM.Validator.BTVValidator))) {
            //    try {
            //        IValidator v = (IValidator)o;
            //        EA.Package p = v.repository.GetPackageByID(Int32.Parse(v.scope));
            //        originPackage = p.Name + " (" + p.Element.Stereotype + ")";
            //    }
            //    catch (Exception e) {}
            //}


			
			return originPackage;
		}


	}
}
