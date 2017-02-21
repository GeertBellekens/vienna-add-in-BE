/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using VIENNAAddIn.validator;

namespace VIENNAAddIn.Utils
{
    /// <sUMM2ary>
    /// This class represents a log Message
    /// </sUMM2ary>
    internal class LogMessageCCTS
    { 
        internal String message;
        internal String dateTime;
        internal String level;
        internal ErrorMessage errorMessage;
        internal int id;

        //Autoincrement id
        private static int count = 0;


        /// <sUMM2ary>
        /// Constructs and object of type LogMessage with
        /// given level, time of occurence, log message and
        /// a constraint object
        /// If not used by the validator - constraint is usually null
        /// </sUMM2ary>
        /// <param name="level"></param>
        /// <param name="dateTime"></param>
        /// <param name="message"></param>
        internal LogMessageCCTS(String level, String dateTime, String message, ErrorMessage e)
        {

            //Increment the counter
            count++;
            //Set the id
            id = count;

            this.errorMessage = e;
            this.level = level;
            if (level.Equals("WARN") || level.Equals("INFO"))
            {
                this.level = level + " ";
            }
            this.dateTime = dateTime;
            this.message = message;
        }



    }
}
