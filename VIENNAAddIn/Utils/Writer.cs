/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace VIENNAAddIn.Utils
{
    class Writer {


        private static String PATH = "C:\\Dokumente und Einstellungen\\pliegl\\Desktop\\UMM2debug\\";
        private static String FILENAME = "debug.log";
        
        public static void write(String s) {

            System.IO.Directory.CreateDirectory(PATH);
            Stream outputStream = System.IO.File.Open(PATH + FILENAME, FileMode.Append);

            StreamWriter stream = new StreamWriter(outputStream);
            stream.WriteLine(s);
            stream.Close();           
                       
        }    
    }


}
