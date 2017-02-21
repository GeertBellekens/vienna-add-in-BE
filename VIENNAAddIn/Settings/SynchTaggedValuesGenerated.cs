/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using EA;
using VIENNAAddInUtils;
using Attribute=EA.Attribute;
using Stereotype=VIENNAAddIn.upcc3.Stereotype;

namespace VIENNAAddIn.Settings
{
    public partial class SynchTaggedValues
    {
        private void FixPackage(Path path, Package package)
        {
            switch (package.Element.Stereotype)
            {
                case Stereotype.BDTLibrary:
                    FixBdtLibrary(path/package.Name, package);
                    break;
                case Stereotype.BIELibrary:
                    FixBieLibrary(path/package.Name, package);
                    break;
                case Stereotype.bLibrary:
                    FixBLibrary(path/package.Name, package);
                    break;
                case Stereotype.CCLibrary:
                    FixCcLibrary(path/package.Name, package);
                    break;
                case Stereotype.CDTLibrary:
                    FixCdtLibrary(path/package.Name, package);
                    break;
                case Stereotype.DOCLibrary:
                    FixDocLibrary(path/package.Name, package);
                    break;
                case Stereotype.ENUMLibrary:
                    FixEnumLibrary(path/package.Name, package);
                    break;
                case Stereotype.PRIMLibrary:
                    FixPrimLibrary(path/package.Name, package);
                    break;
            }
        }

        private void FixBdtLibrary(Path path, Package package)
        {
            AddMissingTaggedValues(path, package, "businessTerm", "copyright", "owner", "reference", "status", "uniqueIdentifier", "versionIdentifier", "baseURN", "namespacePrefix");
            foreach (Element element in package.Elements)
            {
                switch (element.Stereotype)
                {
                    case Stereotype.BDT:
                        AddMissingTaggedValues(path/element.Name, element, "businessTerm", "definition", "dictionaryEntryName", "languageCode", "uniqueIdentifier", "versionIdentifier", "usageRule");
						foreach (Attribute attribute in element.Attributes)
						{
							switch (attribute.Stereotype)
							{
								case Stereotype.CON:
									AddMissingTaggedValues(path/element.Name/attribute.Name, attribute, "businessTerm", "definition", "dictionaryEntryName", "enumeration", "fractionDigits", "languageCode", "maximumExclusive", "maximumInclusive", "maximumLength", "minimumExclusive", "minimumInclusive", "minimumLength", "modificationAllowedIndicator", "pattern", "totalDigits", "uniqueIdentifier", "usageRule", "versionIdentifier");
									break;
								case Stereotype.SUP:
									AddMissingTaggedValues(path/element.Name/attribute.Name, attribute, "businessTerm", "definition", "dictionaryEntryName", "enumeration", "fractionDigits", "languageCode", "maximumExclusive", "maximumInclusive", "maximumLength", "minimumExclusive", "minimumInclusive", "minimumLength", "modificationAllowedIndicator", "pattern", "totalDigits", "uniqueIdentifier", "usageRule", "versionIdentifier");
									break;
							}
						}
                        break;
                }
            }
        }

        private void FixBieLibrary(Path path, Package package)
        {
            AddMissingTaggedValues(path, package, "businessTerm", "copyright", "owner", "reference", "status", "uniqueIdentifier", "versionIdentifier", "baseURN", "namespacePrefix");
            foreach (Element element in package.Elements)
            {
                switch (element.Stereotype)
                {
                    case Stereotype.ABIE:
                        AddMissingTaggedValues(path/element.Name, element, "businessTerm", "definition", "dictionaryEntryName", "languageCode", "uniqueIdentifier", "versionIdentifier", "usageRule");
						foreach (Attribute attribute in element.Attributes)
						{
							switch (attribute.Stereotype)
							{
								case Stereotype.BBIE:
									AddMissingTaggedValues(path/element.Name/attribute.Name, attribute, "businessTerm", "definition", "dictionaryEntryName", "languageCode", "sequencingKey", "uniqueIdentifier", "versionIdentifier", "usageRule");
									break;
							}
						}
						foreach (Connector connector in element.Connectors)
						{
							switch (connector.Stereotype)
							{
								case Stereotype.ASBIE:
									AddMissingTaggedValues(path/element.Name/connector.Name, connector, "businessTerm", "definition", "dictionaryEntryName", "languageCode", "sequencingKey", "uniqueIdentifier", "versionIdentifier", "usageRule");
									break;
							}
						}
                        break;
                }
            }
        }

        private void FixBLibrary(Path path, Package package)
        {
            AddMissingTaggedValues(path, package, "businessTerm", "copyright", "owner", "reference", "status", "uniqueIdentifier", "versionIdentifier");
            foreach (Package subPackage in package.Packages)
            {
                FixPackage(path/subPackage.Name, subPackage);
            }
        }

        private void FixCcLibrary(Path path, Package package)
        {
            AddMissingTaggedValues(path, package, "businessTerm", "copyright", "owner", "reference", "status", "uniqueIdentifier", "versionIdentifier", "baseURN", "namespacePrefix");
            foreach (Element element in package.Elements)
            {
                switch (element.Stereotype)
                {
                    case Stereotype.ACC:
                        AddMissingTaggedValues(path/element.Name, element, "businessTerm", "definition", "dictionaryEntryName", "languageCode", "uniqueIdentifier", "versionIdentifier", "usageRule");
						foreach (Attribute attribute in element.Attributes)
						{
							switch (attribute.Stereotype)
							{
								case Stereotype.BCC:
									AddMissingTaggedValues(path/element.Name/attribute.Name, attribute, "businessTerm", "definition", "dictionaryEntryName", "languageCode", "sequencingKey", "uniqueIdentifier", "versionIdentifier", "usageRule");
									break;
							}
						}
						foreach (Connector connector in element.Connectors)
						{
							switch (connector.Stereotype)
							{
								case Stereotype.ASCC:
									AddMissingTaggedValues(path/element.Name/connector.Name, connector, "businessTerm", "definition", "dictionaryEntryName", "languageCode", "sequencingKey", "uniqueIdentifier", "versionIdentifier", "usageRule");
									break;
							}
						}
                        break;
                }
            }
        }

        private void FixCdtLibrary(Path path, Package package)
        {
            AddMissingTaggedValues(path, package, "businessTerm", "copyright", "owner", "reference", "status", "uniqueIdentifier", "versionIdentifier", "baseURN", "namespacePrefix");
            foreach (Element element in package.Elements)
            {
                switch (element.Stereotype)
                {
                    case Stereotype.CDT:
                        AddMissingTaggedValues(path/element.Name, element, "businessTerm", "definition", "dictionaryEntryName", "languageCode", "uniqueIdentifier", "versionIdentifier", "usageRule");
						foreach (Attribute attribute in element.Attributes)
						{
							switch (attribute.Stereotype)
							{
								case Stereotype.CON:
									AddMissingTaggedValues(path/element.Name/attribute.Name, attribute, "businessTerm", "definition", "dictionaryEntryName", "languageCode", "modificationAllowedIndicator", "uniqueIdentifier", "versionIdentifier", "usageRule");
									break;
								case Stereotype.SUP:
									AddMissingTaggedValues(path/element.Name/attribute.Name, attribute, "businessTerm", "definition", "dictionaryEntryName", "languageCode", "modificationAllowedIndicator", "uniqueIdentifier", "versionIdentifier", "usageRule");
									break;
							}
						}
                        break;
                }
            }
        }

        private void FixDocLibrary(Path path, Package package)
        {
            AddMissingTaggedValues(path, package, "businessTerm", "copyright", "owner", "reference", "status", "uniqueIdentifier", "versionIdentifier", "baseURN", "namespacePrefix");
            foreach (Element element in package.Elements)
            {
                switch (element.Stereotype)
                {
                }
            }
        }

        private void FixEnumLibrary(Path path, Package package)
        {
            AddMissingTaggedValues(path, package, "businessTerm", "copyright", "owner", "reference", "status", "uniqueIdentifier", "versionIdentifier", "baseURN", "namespacePrefix");
            foreach (Element element in package.Elements)
            {
                switch (element.Stereotype)
                {
                    case Stereotype.ENUM:
                        AddMissingTaggedValues(path/element.Name, element, "businessTerm", "codeListAgencyIdentifier", "codeListAgencyName", "codeListIdentifier", "codeListName", "dictionaryEntryName", "definition", "enumerationURI", "languageCode", "modificationAllowedIndicator", "restrictedPrimitive", "status", "uniqueIdentifier", "versionIdentifier");
						foreach (Attribute attribute in element.Attributes)
						{
							switch (attribute.Stereotype)
							{
								case Stereotype.CodelistEntry:
									AddMissingTaggedValues(path/element.Name/attribute.Name, attribute, "codeName", "status");
									break;
							}
						}
                        break;
                    case Stereotype.IDSCHEME:
                        AddMissingTaggedValues(path/element.Name, element, "businessTerm", "definition", "dictionaryEntryName", "identifierSchemeAgencyIdentifier", "identifierSchemeAgencyName", "modificationAllowedIndicator", "pattern", "restrictedPrimitive", "uniqueIdentifier", "versionIdentifier");
                        break;
                }
            }
        }

        private void FixPrimLibrary(Path path, Package package)
        {
            AddMissingTaggedValues(path, package, "businessTerm", "copyright", "owner", "reference", "status", "uniqueIdentifier", "versionIdentifier", "baseURN", "namespacePrefix");
            foreach (Element element in package.Elements)
            {
                switch (element.Stereotype)
                {
                    case Stereotype.PRIM:
                        AddMissingTaggedValues(path/element.Name, element, "businessTerm", "definition", "dictionaryEntryName", "fractionDigits", "languageCode", "length", "maximumExclusive", "maximumInclusive", "maximumLength", "minimumExclusive", "minimumInclusive", "minimumLength", "pattern", "totalDigits", "uniqueIdentifier", "versionIdentifier", "whiteSpace");
                        break;
                }
            }
        }
	}
}

