using System;
using System.Collections.Generic;
using System.Xml.Schema;
using CctsRepository;
using CctsRepository.DocLibrary;

namespace VIENNAAddIn.upcc3.export.cctsndr
{
    ///<summary>
    ///</summary>
    public class GeneratorContext
    {
        private readonly int progress;
        private readonly List<SchemaInfo> schemas = new List<SchemaInfo>();

        ///<summary>
        ///</summary>
        ///<param name="repository"></param>
        ///<param name="targetNamespace"></param>
        ///<param name="namespacePrefix"></param>
        ///<param name="annotate"></param>
        ///<param name="allschemas"></param>
        ///<param name="outputDirectory"></param>
        ///<param name="docLibrary"></param>
        public GeneratorContext(ICctsRepository repository, string targetNamespace, string namespacePrefix, bool annotate, bool allschemas, string outputDirectory, IDocLibrary docLibrary)
        {
            Allschemas = allschemas;
            Repository = repository;
            TargetNamespace = targetNamespace;
            NamespacePrefix = namespacePrefix;
            Annotate = annotate;
            OutputDirectory = outputDirectory;
            DocLibrary = docLibrary;
            progress = 100/(allschemas ? 5 : 3);
        }

        ///<summary>
        ///</summary>
        public ICctsRepository Repository { get; private set; }

        ///<summary>
        ///</summary>
        public string TargetNamespace { get; private set; }

        ///<summary>
        ///</summary>
        public List<SchemaInfo> Schemas
        {
            get { return schemas; }
        }

        ///<summary>
        ///</summary>
        public string NamespacePrefix { get; private set; }

        ///<summary>
        ///</summary>
        public bool Annotate { get; private set; }

        ///<summary>
        ///</summary>
        public bool Allschemas { get; private set; }

        ///<summary>
        ///</summary>
        public string OutputDirectory { get; private set; }

        ///<summary>
        ///</summary>
        public IDocLibrary DocLibrary { get; private set; }

        ///<summary>
        ///</summary>
        ///<param name="schema"></param>
        ///<param name="fileName"></param>
        public void AddSchema(XmlSchema schema, string fileName, Schematype schematype)
        {
            Schemas.Add(new SchemaInfo(schema, fileName,schematype));
            if (SchemaAdded != null)
            {
                SchemaAdded(this, new SchemaAddedEventArgs(fileName, progress));
            }
        }

        public event EventHandler<SchemaAddedEventArgs> SchemaAdded;
    }

    public class GenerationMessage
    {
    }

    public class SchemaAddedEventArgs : EventArgs
    {
        public SchemaAddedEventArgs(string fileName, int progress)
        {
            FileName = fileName;
            Progress = progress;
        }

        public string FileName { get; private set; }
        public int Progress { get; private set; }
    }

    ///<summary>
    ///</summary>
    public class SchemaInfo
    {
        ///<summary>
        ///</summary>
        ///<param name="schema"></param>
        ///<param name="fileName"></param>
        public SchemaInfo(XmlSchema schema, string fileName, Schematype schematype)
        {
            Schema = schema;
            FileName = fileName;
            Schematype = schematype;
        }

        ///<summary>
        ///</summary>
        public XmlSchema Schema { get; private set; }

        ///<summary>
        ///</summary>
        public string FileName { get; private set; }
        public Schematype Schematype {get; private set;}
    }
    public enum Schematype : int
    {
    	BDT,
    	BIE,
    	ROOT,
    	ACC,
    	CDT,
    	ENUM
    }
}