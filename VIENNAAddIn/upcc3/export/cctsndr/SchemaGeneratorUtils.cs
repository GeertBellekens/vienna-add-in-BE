using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Schema;
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.EnumLibrary;
using VIENNAAddIn.upcc3.repo;
using VIENNAAddIn.upcc3.repo.EnumLibrary;
using VIENNAAddInUtils;
namespace VIENNAAddIn.upcc3.export.cctsndr
{
	/// <summary>
	/// Description of SchemaGeneratorUtils.
	/// </summary>
	public static class SchemaGeneratorUtils
	{
		private const string NSPREFIX_DOC = "ccts";
		private const string NS_DOC = "urn:un:unece:uncefact:documentation:common:3:standard:CoreComponentsTechnicalSpecification:3";
		
		public static void AddAnnotations(XmlDocument xml, List<XmlNode> annNodes, string name,
                                           IEnumerable<string> values)
        {
            foreach (string item in values)
            {
                AddAnnotation(xml, annNodes, name, item);
            }
        }

        public static void AddAnnotation(XmlDocument xml, List<XmlNode> annNodes, string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                XmlElement annotation = xml.CreateElement(NSPREFIX_DOC, name,NS_DOC);
                annotation.InnerText = value;
                annNodes.Add(annotation);
            }
        }
	}
}
