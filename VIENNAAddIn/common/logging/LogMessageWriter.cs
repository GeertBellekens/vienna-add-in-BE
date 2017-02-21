/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO; 

namespace VIENNAAddIn.common.logging
{
    /// <summary>
    /// Helper class which writes messages to a given location.
    /// </summary>
    internal class LogMessageWriter
    {
        #region Static LoggerUtil Members

        /// <summary>
        /// Creates an error message and writes it to the specified file. The file location is determined 
        /// by location and filename. 
        /// </summary>
        /// <param name="location">The location of the log file.</param>
        /// <param name="filename">The name of the log file.</param>
        /// <param name="message">The message which should be written to the log file.</param>
        internal static void LogError(string location, string filename, LogMessage message)
        {
            StringBuilder logBuilder = new StringBuilder();

            logBuilder.AppendLine("[" + message.Time + "]" + message.Type.ToString() + ": " + message.Message);

            File.AppendAllText(location + filename, logBuilder.ToString());


        }

        /// <summary>
        /// Creates a warning message and writes it to the specified file. The file location is determined 
        /// by location and filename. 
        /// </summary>
        /// <param name="location">The location of the log file.</param>
        /// <param name="filename">The name of the log file.</param>
        /// <param name="message">The message which should be written to the log file.</param>
        internal static void LogWarning(string location, string filename, LogMessage message)
        {
            StringBuilder logBuilder = new StringBuilder();

            logBuilder.AppendLine("[" + message.Time + "]" + message.Type.ToString() + ": " + message.Message);

            File.AppendAllText(location + filename, logBuilder.ToString()); 
        }

        /// <summary>
        /// Creates an information message and writes it to the specified file. The file location is determined 
        /// by location and filename. 
        /// </summary>
        /// <param name="location">The location of the log file.</param>
        /// <param name="filename">The name of the log file.</param>
        /// <param name="message">The message which should be written to the log file.</param>
        internal static void LogInfo(string location, string filename, LogMessage message)
        {
            StringBuilder logBuilder = new StringBuilder();

            logBuilder.AppendLine("[" + message.Time + "]" + message.Type.ToString() + ": " + message.Message);

            File.AppendAllText(location + filename, logBuilder.ToString()); 
        }

        #endregion
    }
}
