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
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using VIENNAAddIn.Settings;

namespace VIENNAAddIn.upcc3.export.cctsndr
{
    ///<summary>
    ///</summary>
    public class XSDGenerator
    {
    	public static void GenerateSchemas(IEnumerable<GeneratorContext> contexts)
    	{
    		//keep a list of all generic contexts, one for each main version.
    		List<GeneratorContext> genericContexts = new List<GeneratorContext>();
    		foreach (var context in contexts) 
    		{
    			//get or add a generic context
    			var genericContext = genericContexts.FirstOrDefault(x => x.BaseURN == context.BaseURN);
    			if (genericContext == null)
    			{
    				genericContext = new GeneratorContext(context);
    				genericContexts.Add(genericContext);
    			}
    			//start generating
    			GenerateSchemas(context, genericContext);
    		}
    		//now start generating the genericContexts
    		foreach (var genericContext in genericContexts) 
    		{
    			GenerateGenericSchemas(genericContext);
    		}
    	}

		public static void GenerateGenericSchemas(GeneratorContext genericContext)
		{
			BDTSchemaGenerator.GenerateXSD(genericContext, genericContext);
            BIESchemaGenerator.GenerateXSD(genericContext);
            
            WriteSchemas(genericContext);
		}

        ///<summary>
        ///</summary>
        public static void GenerateSchemas(GeneratorContext context, GeneratorContext genericContext)
        {
            BDTSchemaGenerator.GenerateXSD(context, genericContext, CollectBDTs(context));
            BIESchemaGenerator.GenerateXSD(context, genericContext, CollectABIEs(context));
            RootSchemaGenerator.GenerateXSD(context, genericContext);
            
            WriteSchemas(context);
        }

		static void WriteSchemas(GeneratorContext context)
		{
			//write only enum schema's in generic
			foreach (SchemaInfo schemaInfo in context.Schemas
			         .Where(x => ! context.isGeneric || x.Schematype == UpccSchematype.ENUM))
			{
				//make sure the directory exists
				var directoryPath = Path.GetDirectoryName(schemaInfo.FileName);
				if (!Directory.Exists(directoryPath)) {
					Directory.CreateDirectory(directoryPath);
				}
				var xmlWriterSettings = new XmlWriterSettings {
					Indent = true,
					Encoding = Encoding.UTF8,
				};
				using (XmlWriter xmlWriter = XmlWriter.Create(schemaInfo.FileName, xmlWriterSettings)) {
					schemaInfo.Schema.Write(xmlWriter);
					xmlWriter.Close();
				}
			}
			if (context.Annotate) {
				try {
					CopyFolder(AddInSettings.CommonXSDPath, context.OutputDirectory);
				} catch (DirectoryNotFoundException) {
					MessageBox.Show("Directory '" + AddInSettings.CommonXSDPath + "' not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				} catch (IOException ioe) {
					Console.Out.WriteLine("Exception occured:" + ioe.Message);
				}
			}
		}
        private static void CopyFolder(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);
            string[] files = Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                string dest = Path.Combine(destFolder, name);
                File.Copy(file, dest);
            }
            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                if (!folder.Contains(".svn"))
                {
                    string name = Path.GetFileName(folder);
                    string dest = Path.Combine(destFolder, name);
                    CopyFolder(folder, dest);
                }
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="context"></param>
        ///<returns></returns>
        public static IEnumerable<IBdt> CollectBDTs(GeneratorContext context)
        {
        	return context.DocLibrary.Elements.OfType<IBdt>();
        }

        ///<summary>
        ///</summary>
        ///<param name="context"></param>
        ///<returns></returns>
        public static IEnumerable<IAbie> CollectABIEs(GeneratorContext context)
        {
			return context.DocLibrary.Elements.OfType<IAbie>();
        }

        ///<summary>
        ///</summary>
        ///<param name="context"></param>
        ///<returns></returns>
        public static IEnumerable<IAcc> CollectACCs(GeneratorContext context)
        {
			return context.DocLibrary.Elements.OfType<IAcc>();
        }


        ///<summary>
        ///</summary>
        ///<param name="context"></param>
        ///<returns></returns>
        public static IEnumerable<ICdt> CollectCDTs(GeneratorContext context)
        {
			return context.DocLibrary.Elements.OfType<ICdt>();
        }
    }
}