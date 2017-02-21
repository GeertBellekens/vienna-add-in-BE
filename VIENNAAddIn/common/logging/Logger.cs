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
using System.Windows.Forms;
using System.Reflection;

namespace VIENNAAddIn.common.logging
{
    /// <summary>
    /// Default logger for the VIENNAAddIn. Implements the ILogger interface. 
    /// </summary>
    public class Logger : ILogger
    {
        #region Logger Static Members

        /// <summary>
        /// The singleton instance of the Logger class. 
        /// </summary>
        private static ILogger _instance;
    
        /// <summary>
        /// Public property for the singleton instance. If the instance equals null a new Logger instance will
        /// be created. 
        /// </summary>
        public static ILogger Instance
        {
            get
            {
                // make sure that the private static ILogger member is initialized
                if (Logger._instance == null)
                    Logger._instance = new Logger();
                return Logger._instance;
            }
        }

        /// <summary>
        /// Logging method which allows the developer to log a message without instantiating a logger instance. 
        /// </summary>
        /// <param name="message">The message which should be logged.</param>
        /// <param name="type">Identifies the type of information which should be logged.</param>
        public static void Log(string message, LogType type)
        {
            // instanciate the default logger instance
            ILogger logger = Logger.Instance;
            LogMessage lMessage = new LogMessage(message, type);

            // decide which logging type should be used
            if (type == LogType.ERROR)
                logger.LogError(lMessage);
            else if (type == LogType.INFO)
                logger.LogInfo(lMessage);
            else if (type == LogType.WARNING)
                logger.LogWarning(lMessage); 
        }

        #endregion

        #region Logger Members

        /// <summary>
        /// Specifies the log file location. Readonly variable which is currently hardcoded and initialized within 
        /// the Logger class constructor
        /// </summary>
        private readonly string loglocation; 

        #endregion

        #region Logger Constructor

        /// <summary>
        /// Initializes a default logger by setting the log-file's location. The log file is saved in the applications
        /// path and can be found within the log folder. Currently the logger creates separate log files for each day. 
        /// </summary>
        protected Logger()
        {
            // initialize the location path ...

            this.loglocation = this.GetLocationPath(); 
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Helper method which returns the relative path of the applications' location. 
        /// </summary>
        /// <returns>Relative application path.</returns>
        private string GetLocationPath()
        {
            StringBuilder builder = new StringBuilder();
            string location = Assembly.GetExecutingAssembly().Location;
            location = location.Remove(location.LastIndexOf('\\'), location.Length - (location.LastIndexOf('\\')));

            builder.Append(location);
            builder.Append("\\");

            return builder.ToString(); 
        }

        /// <summary>
        /// Returns the log file name.
        /// </summary>
        /// <returns>Name of the log file.</returns>
        private string GetLogFileName()
        {
            StringBuilder builder = new StringBuilder();
            DateTime time = DateTime.Now;

            builder.Append(time.Day.ToString());
            builder.Append(time.Month.ToString());
            builder.Append(time.Year.ToString());
            builder.Append(".log");

            return builder.ToString(); 
        }

        #endregion

        #region ILogger Members

        /// <summary>
        /// Default implementation of the ILogger interface method. Writes an error log message to the 
        /// log file.
        /// </summary>
        /// <param name="message">LogMessage object containing all necessary logging information.</param>
        public void LogError(LogMessage message)
        {
            LogMessageWriter.LogError(this.loglocation, this.GetLogFileName(), message); 
        }

        /// <summary>
        /// Default implementation of the ILogger interface method. Writes a warning log message to the 
        /// log file.
        /// </summary>
        /// <param name="message">LogMessage object containing all necessary logging information.</param>
        public void LogWarning(LogMessage message)
        {
            LogMessageWriter.LogWarning(this.loglocation, this.GetLogFileName(), message); 
        }

        /// <summary>
        /// Default implementation of the ILogger interface method. Writes an info log message to the 
        /// log file.
        /// </summary>
        /// <param name="message">LogMessage object containing all necessary logging information.</param>
        public void LogInfo(LogMessage message)
        {
            LogMessageWriter.LogInfo(this.loglocation, this.GetLogFileName(), message); 
        }

        #endregion
    }
}
