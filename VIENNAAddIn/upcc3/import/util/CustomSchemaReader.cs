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
using System.Xml;

namespace VIENNAAddIn.upcc3.import.util
{
    public class QualifiedName
    {
        public string Prefix { get; set; }
        public string Name { get; set; }

        public QualifiedName(string prefix, string name)
        {
            Prefix = prefix;
            Name = name;
        }
    }

    public class Include
    {
        public string SchemaLocation { get; set; }
    }

    public class ComplexType
    {
        public string Name { get; set; }
        public IList<object> Items { get; private set; }

        public ComplexType()
        {
            Items = new List<object>();
            Name = "";
        }
    }

    public class Element
    {
        public string Name { get; set; }
        public QualifiedName Ref { get; set; }
        public QualifiedName Type { get; set; }
        public string MinOccurs { get; set; }
        public string MaxOccurs { get; set; }      

        public Element()
        {
            Name = "";
            Ref = new QualifiedName("", "");
            Type = new QualifiedName("", "");
            MinOccurs = "";
            MaxOccurs = "";
        }
    }

    public static class ExtensionMethods
    {
        public static Include ParseInclude(this XmlNodeReader reader)
        {
            Include include = new Include();

            if (reader.MoveToAttribute("schemaLocation"))
            {
                include.SchemaLocation = reader.ReadContentAsString();
            }

            return include;
        }

        public static ComplexType ParseComplexType(this XmlNodeReader reader)
        {
            ComplexType newComplexType = new ComplexType();

            XmlReader subtree = reader.ReadSubtree();
            while (subtree.Read())
            {
                if (subtree.NodeType == XmlNodeType.Element)
                {
                    if (subtree.Name.Equals("xsd:element"))
                    {
                        
                        Element element = subtree.ParseElement();
                        newComplexType.Items.Add(element);
                        //Console.WriteLine(element.Name);
                    }
                }
            }

            if (reader.MoveToAttribute("name"))
            {
                newComplexType.Name = reader.ReadContentAsString();
            }

            return newComplexType;
        }
        
        public static Element ParseElement(this XmlReader reader)
        {
            Element element = new Element();

            if (reader.MoveToAttribute("name"))
            {
                element.Name = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("ref"))
            {
                string reference = reader.ReadContentAsString();
                int endPrefix = reference.IndexOf(':');

                element.Ref.Prefix = reference.Substring(0, endPrefix);
                element.Ref.Name = reference.Substring(endPrefix + 1);                
            }

            if (reader.MoveToAttribute("type"))
            {
                string type = reader.ReadContentAsString();
                int endPrefix = type.IndexOf(':');

                element.Type.Prefix = type.Substring(0, endPrefix);
                element.Type.Name = type.Substring(endPrefix + 1);
            }

            if (reader.MoveToAttribute("minOccurs"))
            {
                element.MinOccurs = reader.ReadContentAsString();
            }

            if (reader.MoveToAttribute("maxOccurs"))
            {
                element.MaxOccurs = reader.ReadContentAsString();
            }

            return element;
        }

        public static void MoveToSchemaRoot(this XmlNodeReader reader)
        {
            while (reader.Read())
            {
                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "xsd:schema"))
                {
                    return;
                }
            }
        }

        public static IDictionary<string, string> ParseSchemaHeader(this XmlNodeReader reader)
        {
            IDictionary<string, string> nsTable = new Dictionary<string, string>();

            for (int i = 0; i < reader.AttributeCount; i++)
            {
                reader.MoveToAttribute(i);
                
                if (reader.Name.Contains("xmlns:"))
                {
                    nsTable.Add(reader.Name.Substring(6), reader.ReadContentAsString());
                }
            }

            return nsTable;
        }
    }

    public class CustomSchemaReader
    {
        private XmlNodeReader reader;

        public IDictionary<string, string> NamespaceTable { get; private set; }
        public IList<object> Items { get; private set; }
        public IList<Include> Includes { get; private set; }

        private Boolean Skip = false;      

        private Boolean CheckToSkip(XmlReader reader)
        {
            if(Skip)
            {
                this.Skip = false;
                reader.Skip();
                if (reader.EOF)
                    return false;
                else
                    return true;
 
            } else
                return reader.Read();  
        }      
  
        public CustomSchemaReader(XmlDocument xmlDocument)
        {
            Items = new List<object>();
            Includes = new List<Include>();         
 
            reader = new XmlNodeReader(xmlDocument);

            reader.MoveToSchemaRoot();

            NamespaceTable = reader.ParseSchemaHeader();

            while (CheckToSkip(reader))
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "xsd:element":
                        {
                            Element element = reader.ParseElement();

                            Items.Add(element);

                            break;
                        }
                        case "xsd:complexType":
                        {
                            ComplexType newComplexType = reader.ParseComplexType();

                            Items.Add(newComplexType);
                            this.Skip = true;
                            break;
                        }
                        case "xsd:include":
                        {
                            Include include = reader.ParseInclude();

                            Includes.Add(include);
                                
                            break;
                        }
                    }
                }
            }
        }
    }
}