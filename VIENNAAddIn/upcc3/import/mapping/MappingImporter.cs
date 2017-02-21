using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using CctsRepository;
using CctsRepository.BLibrary;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.import.mapping
{
    public class MappingImporter
    {
        private readonly ICcLibrary ccLibrary;
        private readonly IBLibrary bLibrary;
        private readonly string[] mapForceMappingFiles;
        private readonly string[] xmlSchemaFiles; 
        private readonly string docLibraryName;
        private readonly string bieLibraryName;
        private readonly string bdtLibraryName;
        private readonly string qualifier;
        private readonly string rootElementName;
        private ICctsRepository cctsRepository;
        public SchemaMapping mappings { get; private set; }

        /// <summary>
        /// </summary>
        /// <param name="xmlSchemaFiles"></param>
        /// <param name="ccLibrary">The CC Library.</param>
        /// <param name="bLibrary">The bLibrary.</param>
        /// <param name="mapForceMappingFiles">The MapForce mapping file.</param>
        /// <param name="docLibraryName">The name of the DOCLibrary to be created.</param>
        /// <param name="bieLibraryName">The name of the BIELibrary to be created.</param>
        /// <param name="bdtLibraryName">The name of the BDTLibrary to be created.</param>
        /// <param name="qualifier">The qualifier for the business domain (e.g. "ebInterface").</param>
        /// <param name="rootElementName"></param>
        public MappingImporter(IEnumerable<string> mapForceMappingFiles, IEnumerable<string> xmlSchemaFiles, ICcLibrary ccLibrary, IBLibrary bLibrary, string docLibraryName, string bieLibraryName, string bdtLibraryName, string qualifier, string rootElementName, ICctsRepository cctsRepository)
        {
            this.ccLibrary = ccLibrary;
            this.bLibrary = bLibrary;
            this.mapForceMappingFiles = new List<string>(mapForceMappingFiles).ToArray();
            this.xmlSchemaFiles = new List<string>(xmlSchemaFiles).ToArray();
            this.docLibraryName = docLibraryName;
            this.bieLibraryName = bieLibraryName;
            this.bdtLibraryName = bdtLibraryName;
            this.qualifier = qualifier;
            this.rootElementName = rootElementName;
            this.cctsRepository = cctsRepository;
        }

        /// <summary>
        /// </summary>
        public void ImportMapping()
        {
            var mapForceMapping = LinqToXmlMapForceMappingImporter.ImportFromFiles(mapForceMappingFiles);

            var xmlSchemaSet = new XmlSchemaSet();
            foreach (string xmlSchema in xmlSchemaFiles)
            {
                xmlSchemaSet.Add(XmlSchema.Read(XmlReader.Create(xmlSchema), null));    
            }

            mappings = new SchemaMapping(mapForceMapping, xmlSchemaSet, ccLibrary, cctsRepository);
            var mappedLibraryGenerator = new MappedLibraryGenerator(mappings, bLibrary, docLibraryName, bieLibraryName, bdtLibraryName, qualifier, rootElementName);
            mappedLibraryGenerator.GenerateLibraries();
        }
    }
}