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
        ///<summary>
        ///</summary>
        public static GeneratorContext GenerateSchemas(GeneratorContext context)
        {
            BDTSchemaGenerator.GenerateXSD(context, CollectBDTs(context));
            BIESchemaGenerator.GenerateXSD(context, CollectABIEs(context));
            RootSchemaGenerator.GenerateXSD(context);

            if (context.Allschemas)
            {
                CDTSchemaGenerator.GenerateXSD(context, CollectCDTs(context));
                CCSchemaGenerator.GenerateXSD(context, CollectACCs(context));
            }

            if (!Directory.Exists(context.OutputDirectory))
            {
                Directory.CreateDirectory(context.OutputDirectory);
            }
            foreach (SchemaInfo schemaInfo in context.Schemas)
            {
                var xmlWriterSettings = new XmlWriterSettings
                                            {
                                                Indent = true,
                                                Encoding = Encoding.UTF8,
                                            };
                using (
                    XmlWriter xmlWriter = XmlWriter.Create(context.OutputDirectory + "\\" + schemaInfo.FileName,
                                                           xmlWriterSettings))
                {
// ReSharper disable AssignNullToNotNullAttribute
                    schemaInfo.Schema.Write(xmlWriter);
// ReSharper restore AssignNullToNotNullAttribute
// ReSharper disable PossibleNullReferenceException
                    xmlWriter.Close();
// ReSharper restore PossibleNullReferenceException
                }
            }

            if (context.Annotate)
            {
                try
                {
                    CopyFolder(AddInSettings.CommonXSDPath, context.OutputDirectory);
                }
                catch (DirectoryNotFoundException)
                {
                    MessageBox.Show("Directory '" + AddInSettings.CommonXSDPath + "' not found!", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (IOException ioe)
                {
                    Console.Out.WriteLine("Exception occured:" + ioe.Message);
                }
            }

            return context;
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