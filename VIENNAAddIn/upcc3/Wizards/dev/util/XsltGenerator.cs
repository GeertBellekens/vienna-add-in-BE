// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Collections.Generic;
using System.Xml;
using VIENNAAddIn.upcc3.import.mapping;

namespace VIENNAAddIn.upcc3.Wizards.dev.util
{
    public class XsltGenerator
    {
        private readonly SchemaMapping SchemaMapping;
        private readonly string TargetNamespace;
        private XmlDocument OutputDoc;
        private XmlElement Template;
        public XsltGenerator(string targetNamespace, SchemaMapping schemaMapping)
        {
            TargetNamespace = targetNamespace;
            SchemaMapping = schemaMapping;
        }

        private void PrepareXsltDocument()
        {
            OutputDoc = new XmlDocument();
            XmlElement rootElement = OutputDoc.CreateElement("xsl", "stylesheet", "http://www.w3.org/1999/XSL/Transform");
            OutputDoc.AppendChild(rootElement);
            XmlDeclaration xmldecl = OutputDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            rootElement.SetAttribute("version", "2.0");
            XmlAttribute attributeNode = OutputDoc.CreateAttribute("xmlns", "ns0", "http://www.w3.org/2000/xmlns/");
            attributeNode.Value = TargetNamespace;
            OutputDoc.DocumentElement.SetAttributeNode(attributeNode);
            attributeNode = OutputDoc.CreateAttribute("xmlns", "xs", "http://www.w3.org/2000/xmlns/");
            attributeNode.Value = "http://www.w3.org/2001/XMLSchema";
            OutputDoc.DocumentElement.SetAttributeNode(attributeNode);
            attributeNode = OutputDoc.CreateAttribute("xmlns", "fn", "http://www.w3.org/2000/xmlns/");
            attributeNode.Value = "http://www.w3.org/2005/xpath-functions";
            OutputDoc.DocumentElement.SetAttributeNode(attributeNode);
            rootElement.SetAttribute("exclude-result-prefixes", "ns0 xs fn");
            XmlElement outputMethod = OutputDoc.CreateElement("xsl", "output", "http://www.w3.org/1999/XSL/Transform");
            outputMethod.SetAttribute("method", "xml");
            outputMethod.SetAttribute("encoding", "UTF-8");
            outputMethod.SetAttribute("indent", "yes");
            rootElement.AppendChild(outputMethod);
            Template = OutputDoc.CreateElement("xsl", "template", "http://www.w3.org/1999/XSL/Transform");
            Template.SetAttribute("match", "/");
            rootElement.AppendChild(Template);

            OutputDoc.InsertBefore(xmldecl, rootElement);
        }
        public XmlDocument SetUpXsltToCClDocument()
        {
            PrepareXsltDocument();
            foreach (ComplexTypeMapping complexTypeMapping in SchemaMapping.GetComplexTypeMappings())
            {
                if (complexTypeMapping.IsMappedToSingleACC)
                {
                    var targetName = complexTypeMapping.BIEName.Substring(complexTypeMapping.BIEName.IndexOf("_") + 1);
                    var newElement = OutputDoc.CreateElement(targetName);
                    newElement.SetAttribute("xmlns", "ccts.org");
                    foreach (var child in complexTypeMapping.GetChildren)
                    {
                        newElement.AppendChild(CreateXslForEach(targetName,child.BIEName.Substring(child.BIEName.IndexOf("_") + 1),child.SourceItem.Name));
                    }
                    Template.AppendChild(newElement);
                }
            }

            return OutputDoc;
        }

        public XmlDocument SetUpXsltToSourceDocument()
        {
            PrepareXsltDocument();

            var elementNameList = new Dictionary<string,XmlElement>();
            foreach (ComplexTypeMapping complexTypeMapping in SchemaMapping.GetComplexTypeMappings())
            {
                if (complexTypeMapping.IsMappedToSingleACC)
                {
                    foreach (var targetAcC in complexTypeMapping.TargetACCs)
                    {
                        XmlElement newElement;
                        if (!elementNameList.ContainsKey(targetAcC.Name))
                        {
                            newElement = OutputDoc.CreateElement(complexTypeMapping.SourceElementName);
                            newElement.SetAttribute("xmlns", TargetNamespace);
                            elementNameList.Add(targetAcC.Name,newElement);
                        }
                        else
                        {
                             elementNameList.TryGetValue(targetAcC.Name,out newElement);
                        }
                        foreach (var bcc in complexTypeMapping.BccMappings(targetAcC))
                        {
                            newElement.AppendChild(CreateXslForEach(complexTypeMapping.BIEName.Substring(complexTypeMapping.BIEName.IndexOf("_") + 1), bcc.SourceItem.Name,bcc.Bcc.Name));
                        }
                        Template.AppendChild(newElement);
                    }
                }
            }
            return OutputDoc;
        }
        private XmlElement CreateXslForEach(string targetName, string sourceName, string selectName)
        {
            var forEach = OutputDoc.CreateElement("xsl", "for-each",
                                      "http://www.w3.org/1999/XSL/Transform");
            forEach.SetAttribute("select", "ns0:" + targetName);
            var childElement = OutputDoc.CreateElement(sourceName);
            var sequence = OutputDoc.CreateElement("xsl", "sequence",
                                                   "http://www.w3.org/1999/XSL/Transform");
            sequence.SetAttribute("select", "fn:string(ns0:" + selectName + ")");
            childElement.AppendChild(sequence);
            forEach.AppendChild(childElement);
            return forEach;
        }
    }
}
