// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System;
using IBM.Data.DB2;
using NUnit.Framework;

namespace VIENNAAddInUnitTests.upcc3.Wizards.util
{
    [TestFixture]
    public class DatabaseHandlerTest
    {
        #region Test Settings

        #endregion

        [Test]
        public void UselessTest()
        {
            string conString = "Server=localhost:50000;Database=SAMPLE;UID=db2admin;PWD=db2admin";

            try
            {
                DB2Connection conn = new DB2Connection(conString);
                conn.Open();

                Console.WriteLine("connected to database SAMPLE ...");

                conn.Close();

                Console.WriteLine("disconnected from database SAMPLE ...");                
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        # region Private Class Methods
        
        #endregion
    }
}