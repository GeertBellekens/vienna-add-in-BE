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

namespace VIENNAAddIn.common.logging
{
    /// <summary>
    /// A storrage class for log messages. Encapsulates all necessary information of a log message such as the message,
    /// time and date, and the type.
    /// </summary>
    public class LogMessage
    {
        #region LogMessage Members
        
        /// <summary>
        /// The log message.
        /// </summary>
        private readonly string message;

        /// <summary>
        /// Public property for the private member message.
        /// </summary>
        public string Message
        {
            get { return this.message; }
        }

        /// <summary>
        /// The time when the LogMessage object was created.
        /// </summary>
        private readonly DateTime time;

        /// <summary>
        /// Public property for the private member time.
        /// </summary>
        public DateTime Time
        {
            get { return this.time; }
        }

        /// <summary>
        /// The log type of the message (ERROR, INFO, or WARNING).
        /// </summary>
        private readonly LogType type;

        /// <summary>
        /// Public property for the private member type.
        /// </summary>
        public LogType Type
        {
            get { return this.type; }
        }

        #endregion

        #region LogMessage Constructor

        /// <summary>
        /// Default constructor which initializes the LogMessage object.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="messageType">The log type.</param>
        public LogMessage(string message, LogType messageType)
        {
            this.message = message;
            this.type = messageType;
            this.time = DateTime.Now;
        }

        #endregion

    }
}
