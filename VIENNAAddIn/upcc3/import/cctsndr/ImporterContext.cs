using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.BLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using CctsRepository.PrimLibrary;
using Path=System.IO.Path;

namespace VIENNAAddIn.upcc3.import.cctsndr
{
    public interface IImporterContext
    {
        string RootSchemaPath { get; }
        string RootSchemaFileName { get; }
        XmlSchema BDTSchema { get; }
        string BIESchemaPath { get; }
        ICdtLibrary CDTLibrary { get; }
        ICcLibrary CCLibrary { get; }
        IBdtLibrary BDTLibrary { get; }
        IBieLibrary BIELibrary { get; }
        IPrimLibrary PRIMLibrary { get; }
        IBLibrary BLibrary { get; }
    }

    public class ImporterContext : IImporterContext
    {
        /// <exception cref="Exception">Root schema does not include a BIE schema.</exception>
        public ImporterContext(ICctsRepository cctsRepository, string rootSchemaPath)
        {
            RootSchemaPath = rootSchemaPath;
            RootSchemaFileName = Path.GetFileName(rootSchemaPath);

            string inputDirectory = Path.GetDirectoryName(rootSchemaPath) + @"\";

            foreach (string schemaLocation in GetIncludedSchemaLocations(rootSchemaPath))
            {
                if (schemaLocation.StartsWith("BusinessInformationEntity"))
                {
                    BIESchemaPath = inputDirectory + schemaLocation;
                }
                else if (schemaLocation.StartsWith("BusinessDataType"))
                {
                    BDTSchemaPath = inputDirectory + schemaLocation;
                }
            }
            if (BDTSchemaPath == null)
            {
                throw new Exception("Root schema does not include a BDT schema.");
            }
            if (BIESchemaPath == null)
            {
                throw new Exception("Root schema does not include a BIE schema.");
            }

            CDTLibrary = cctsRepository.GetCdtLibraries().ElementAt(0);
            CCLibrary = cctsRepository.GetCcLibraries().ElementAt(0);
            BDTLibrary = cctsRepository.GetBdtLibraries().ElementAt(0);
            BIELibrary = cctsRepository.GetBieLibraries().ElementAt(0);
            PRIMLibrary = cctsRepository.GetPrimLibraries().ElementAt(0);
            BLibrary = cctsRepository.GetBLibraries().ElementAt(0);
        }

        public string RootSchemaPath { get; private set; }

        public string RootSchemaFileName { get; private set; }

        private string BDTSchemaPath { get; set; }

        public XmlSchema BDTSchema
        {
            get { return XmlSchema.Read(XmlReader.Create(BDTSchemaPath), null); }
        }

        public string BIESchemaPath { get; private set; }

        public ICdtLibrary CDTLibrary { get; private set; }
        
        public ICcLibrary CCLibrary { get; private set; }

        public IBdtLibrary BDTLibrary { get; private set; }

        public IBieLibrary BIELibrary { get; private set; }

        public IPrimLibrary PRIMLibrary { get; private set; }

        public IBLibrary BLibrary { get; private set; }

        private static IEnumerable<string> GetIncludedSchemaLocations(string rootSchemaPath)
        {
            XmlSchema rootSchema = XmlSchema.Read(XmlReader.Create(rootSchemaPath), null);
            foreach (XmlSchemaObject schemaObject in rootSchema.Includes)
            {
                if (schemaObject is XmlSchemaInclude)
                {
                    yield return ((XmlSchemaInclude) schemaObject).SchemaLocation;
                }
            }
        }
    }
}