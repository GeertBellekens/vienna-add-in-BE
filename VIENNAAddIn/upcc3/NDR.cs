using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using CctsRepository.DocLibrary;
using VIENNAAddIn.upcc3.export.cctsndr;
using VIENNAAddIn.upcc3.import.util;
using VIENNAAddIn.upcc3.repo;
using VIENNAAddIn.upcc3.repo.EnumLibrary;
using VIENNAAddInUtils;

namespace VIENNAAddIn.upcc3
{
    public static class NDR
    {
        public const string CCTSNamespace = "urn:un:unece:uncefact:documentation:common:3:standard:CoreComponentsTechnicalSpecification:3";
        private const string Identification = "Identification";
        private const string Identifier = "Identifier";
        private const string Indication = "Indication";
        private const string Indicator = "Indicator";
        private const string Text = "Text";

        public static string GenerateBCCName(IBcc bcc)
        {
            return GenerateBCCOrBBIEName(bcc.Name, bcc.Cdt.Name, bcc.DictionaryEntryName);
        }

        public static string GetXsdElementNameFromBbie(IBbie bbie)
        {
            return bbie != null && bbie.Bdt != null ? 
            	GenerateBCCOrBBIEName(bbie.Name, bbie.Bdt.Name, bbie.DictionaryEntryName)
            	: "Error_BDT_Missing";
        }
        public static string getTargetNameSpace(GeneratorContext context,bool generic )
        {
        	string targetnameSpace = string.Empty;
        	if (context.DocLibrary != null)
        	{
        		targetnameSpace += context.DocLibrary.BaseURN;
        		
        		if (! generic && context.DocLibrary.DocumentRoot != null)
        		{
        			targetnameSpace += "-" + context.DocLibrary.DocumentRoot.Name;
        		}
        	}
        	return targetnameSpace;
        }

        /// <exception cref="ArgumentException"><c>ArgumentException</c>.</exception>
        private static string GenerateBCCOrBBIEName(string propertyTerm, string representationTerm, string dictionaryEntryName)
        {
            if (!String.IsNullOrEmpty(dictionaryEntryName))
            {
                string[] parts = dictionaryEntryName.Replace(" ", String.Empty).Replace("-", String.Empty).Split('.');
                if (parts.Length != 3)
                {
                    throw new ArgumentException(
                        "Expected DictionaryEntryName <ObjectClassTerm. PropertyTerm. RepresentationTerm>, but is " +
                        dictionaryEntryName);
                }
                propertyTerm = parts[1];
                representationTerm = parts[2];
            }
            if ((propertyTerm.EndsWith(Identification)) && (representationTerm.Equals(Identifier)))
            {
                return propertyTerm.Remove(propertyTerm.Length - Identification.Length) + Identifier;
            }

            if ((propertyTerm.EndsWith(Indication)) && (representationTerm.Equals(Indicator)))
            {
                return propertyTerm.Remove(propertyTerm.Length - Indication.Length) + Indicator;
            }

            if (representationTerm.Equals(Text))
            {
                return propertyTerm;
            }

            return NDR.TrimElementName(propertyTerm);// + representationTerm;
        }

        public static string GenerateASCCName(IAscc ascc)
        {
        	return ascc.Name + TrimElementName(ascc.AssociatedAcc.Name);
        }

        public static string GetXsdElementNameFromAsbie(IAsbie asbie)
        {
        	return asbie.Name + TrimElementName(asbie.AssociatedAbie.Name);
        }

        public static string GetXsdElementNameFromAsma(IAsma asma)
        {
            return asma.Name + TrimElementName(asma.AssociatedBieAggregator.Name);
        }
        public static string TrimElementName(string elementName)
        {
        	return elementName.Replace("_",string.Empty);
        }

        public static string GetAsbieNameFromXsdElement(Element element, string associatedElementName)
        {
            return element.Ref.Name.Minus(associatedElementName);
        }
        private static string GetXsdAttributeName(Object someattribute)
        {
        	var attribute = someattribute as UpccAttribute;
        	string name = attribute != null ? attribute.Name : "Error_attribute_missing";
        	string basicTypeName = GetBasicTypeName(attribute);
        	return (name + basicTypeName).Replace(".", String.Empty);
        }
        public static string GetBasicTypeName(UpccAttribute attribute)
        {
        	return attribute != null ? GetBasicTypeName(attribute.BasicType) :"Error_attribute_missing";
        }
        public static string GetBasicTypeName(BasicType basicType)
        {
        	if (basicType == null) return "Error_No_BasicType";
        	return GetBasicTypeName(basicType.ActualType);
        }
        public static string GetBasicTypeName(ICctsElement element)
        {
        	if (element == null) return "Error_No_BasicType";
        	var enumType = element as UpccEnum;
        	//assembled types have a different naming convention
        	if (enumType != null 
        		&& enumType.IsAssembled)
        		return "Assembled" + element.Name + "ContentType";
        	return element.Name + element.UniqueIdentifier;
        }
        public static string GetXsdAttributeNameFromSup(IBdtSup sup)
        {
        	return GetXsdAttributeName(sup);
        }

        public static string GetXsdAttributeNameFromSup(ICdtSup sup)
        {
            return GetXsdAttributeName(sup);
        }


        /// <summary>
        /// [R A7B8]
        /// The name of a BDT that uses a primitive to define its content component BVD MUST be
        /// - the BDT ccts:DataTypeQualifier(s) if any, plus
        /// - the ccts:DataTypeTerm, plus
        /// - the primitive type name,
        /// - followed by the word ‘Type’
        /// - with the separators removed and approved abbreviations and acronyms applied.
        /// Deviation: Separators ('_') are not removed.
        ///
        /// [R 90FB]
        /// The name of a BDT that includes one or more Supplementary Components MUST be:
        /// - The BDT ccts:DataTypeQualifier(s) if any, plus
        /// - The ccts:DataTypeTerm, plus
        /// - The suffix of the Content Component Business Value Domain where the suffix is 
        ///   the primitive type name, the code list token, the series of code list tokens, 
        ///   or the identifier scheme token.
        /// plus
        /// - The ccts:DictionaryEntryName for each Supplementary Component present following the order
        ///   defined in the Data Type Catalogue, plus
        /// - The suffix that represents the Supplementary Component BVD where the suffix is the primitive 
        ///   type name, the code list token, the series of code list tokens, or the identifier scheme token, plus
        /// - The word ‘Type’.
        /// - With all separators removed and approved abbreviations and acronyms applied.
        /// Deviation: Ignoring the SUPs, which means that we name complex types in the same way as simple types.
        /// Deviation: Separators ('_') are not removed.
        /// </summary>
        /// <param name="bdt"></param>
        /// <returns></returns>
        public static string GetXsdTypeNameFromBdt(IBdt bdt)
        {
        	if ( bdt == null) return "Error_No_BDT";
        	
        	return TrimElementName(bdt.Name) + "_" + bdt.UniqueIdentifier;
        }
        public static string getConBasicTypeName(IBdt bdt)
        {
        	if (bdt.Con != null && bdt.Con.BasicType != null)
        	{
        		return TrimElementName(GetBasicTypeName(bdt.Con as UpccAttribute));
        	}
        	return "Error_No_BasicType";
        }
        public static string getConBasicTypeName(ICdt cdt)
        {
        	if (cdt.Con != null && cdt.Con.BasicType != null)
        	{
        		return TrimElementName(GetBasicTypeName(cdt.Con as UpccAttribute));
        	}
        	return "Error_No_BasicType";
        }
        public static string ConvertXsdTypeNameToBasicTypeName(string xsdTypeName)
        {
            switch (xsdTypeName.ToLower())
            {
                case "string":
                    return "String";
                case "decimal":
                    return "Decimal";
                case "base64binary":
                    return "Base64Binary";
                case "token":
                    return "Token";
                case "double":
                    return "Double";
                case "integer":
                    return "Integer";
                case "long":
                    return "Long";
                case "datetime":
                    return "DateTime";
                case "date":
                    return "Date";
                case "time":
                    return "Time";
                case "boolean":
                    return "Boolean";
                default:
                    throw new Exception("Undefined XSD datatype name: " + xsdTypeName);
            }
        }

        public static bool IsXsdDataTypeName(XmlQualifiedName typeName)
        {
            return typeName.Namespace == "http://www.w3.org/2001/XMLSchema";
        }
        /// <summary>
		/// Creates a relative path from one file or folder to another.
		/// </summary>
		/// <param name="fromPath">Contains the directory that defines the start of the relative path.</param>
		/// <param name="toPath">Contains the path that defines the endpoint of the relative path.</param>
		/// <returns>The relative path from the start directory to the end path.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="fromPath"/> or <paramref name="toPath"/> is <c>null</c>.</exception>
		/// <exception cref="UriFormatException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		public static string GetRelativePath(string fromPath, string toPath)
		{
		    if (string.IsNullOrEmpty(fromPath))
		    {
		        throw new ArgumentNullException("fromPath");
		    }
		
		    if (string.IsNullOrEmpty(toPath))
		    {
		        throw new ArgumentNullException("toPath");
		    }
		
		    Uri fromUri = new Uri(AppendDirectorySeparatorChar(fromPath));
		    Uri toUri = new Uri(AppendDirectorySeparatorChar(toPath));
		
		    if (fromUri.Scheme != toUri.Scheme)
		    {
		        return toPath;
		    }
		
		    Uri relativeUri = fromUri.MakeRelativeUri(toUri);
		    string relativePath = Uri.UnescapeDataString(relativeUri.ToString());
		
		    if (string.Equals(toUri.Scheme, Uri.UriSchemeFile, StringComparison.OrdinalIgnoreCase))
		    {
		        relativePath = relativePath.Replace(System.IO.Path.AltDirectorySeparatorChar,System.IO.Path.DirectorySeparatorChar);
		    }
		
		    return relativePath;
		}
		
		private static string AppendDirectorySeparatorChar(string path)
		{
		    // Append a slash only if the path is a directory and does not have a slash.
		    if (!System.IO.Path.HasExtension(path) &&
		        !path.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
		    {
		        return path + System.IO.Path.DirectorySeparatorChar;
		    }
		
		    return path;
		}
    }
}