using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        private List<ICctsElement> _elements = new List<ICctsElement>();

        public GeneratorContext(ICctsRepository repository, string targetNamespace, string baseURN, string namespacePrefix, bool annotate, 
                                bool allschemas, string outputDirectory, IDocLibrary docLibrary)
        {
            Allschemas = allschemas;
            Repository = repository;
            TargetNamespace = targetNamespace;
            BaseURN = baseURN;
            NamespacePrefix = namespacePrefix;
            Annotate = annotate;
            OutputDirectory = outputDirectory;
            DocLibrary = docLibrary;            
            progress = 100/(allschemas ? 5 : 3);
        }
        public GeneratorContext(GeneratorContext otherContext):this(otherContext.Repository,otherContext.TargetNamespace, otherContext.BaseURN, otherContext.NamespacePrefix,otherContext.Annotate, 
                                                                    otherContext.Allschemas, otherContext.OutputDirectory, null)
        {
        	this.VersionID = otherContext.VersionID;
        	this.SchemaAdded += otherContext.GetSchemaAddedSubscribers();
        }

        public ICctsRepository Repository { get; private set; }
        
        
        public IEnumerable<ICctsElement> Elements 
        {
        	get {return _elements;}
        }
        public void AddElements(IEnumerable<ICctsElement> newElements)
        {
        	_elements.AddRange(newElements.Where( x => this.Elements.Any(x.Equals)));
        }
        public bool isGeneric 
        {
        	get { return DocLibrary == null; }
        }

        ///<summary>
        ///</summary>
        public string TargetNamespace { get; private set; }
        public string VersionID { get; set; }
        public string DocRootName { get; set; }
        
        public string BaseURN { get; private set; }

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

		IDocLibrary _docLibrary;
        ///<summary>
		///</summary>
		public IDocLibrary DocLibrary {
			get {
				return _docLibrary;
			}
			private set 
			{
				_docLibrary = value;
				//set version attributes
				if (_docLibrary != null)
				{
					this.VersionID = _docLibrary.VersionIdentifier;
					if (_docLibrary.DocumentRoot != null) this.DocRootName = _docLibrary.DocumentRoot.Name;
				}
			}
		}

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
        private EventHandler<SchemaAddedEventArgs> GetSchemaAddedSubscribers()
        {        	
        	return this.SchemaAdded;
        }
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