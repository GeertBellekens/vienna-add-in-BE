/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.IO;
using System.Collections;
using VIENNAAddIn.Utils;
using VIENNAAddIn.validator;

namespace VIENNAAddIn.Utils
{

	
	internal class Logger {
		
		internal enum Priority : int {			
			DEBUG, 
			INFO,
			WARN,
			ERROR, 
			FATAL,
			OFF,
		}
			
		private StreamWriter sw;

		private bool isFileLogger = false;

		private Priority minLogLevel;

		private ArrayList logMessages;

		private String logDirectory;

		private static object monitor = new Object();

		/**
		 * The validator uses the Logger as well. As an additional
		 * functionality the validator requires to pass a constraint
		 * with every logmessage		  
		 */
        //private ErrorMessage errorMessage;

		

		internal Logger() {
			String strLogLevel = "DEBUG";
			minLogLevel = (Priority)Enum.Parse(typeof(Priority),strLogLevel,true);
			logMessages = new ArrayList();
						
			
		}
		internal Logger(String logDirectory) : this() {
			
			if (!File.Exists(logDirectory)) {
				FileStream fs = File.Create(logDirectory);
				fs.Close();
			}
			this.logDirectory = logDirectory;
			isFileLogger = true;
		}
		
		internal void Debug(String message) {
			this.Append(message, Priority.DEBUG,null);
		}
		internal void Debug(String message, ErrorMessage e) {
			this.Append(message, Priority.DEBUG, e);
		}
		
		internal void Info(String message) {
			this.Append(message, Priority.INFO,null);
		}
		internal void Info(String message, ErrorMessage e) {
			this.Append(message, Priority.INFO,e);
		}
		
		internal void Warn(String message) {
			this.Append(message, Priority.WARN,null);
		}
		internal void Warn(String message, ErrorMessage e) {
			this.Append(message, Priority.WARN,e);
		}
		
		internal void Error(String message) {
			this.Append(message, Priority.ERROR,null);
		}
		internal void Error(String message, ErrorMessage e) {
			this.Append(message, Priority.ERROR,e);
		}
		
		internal void Fatal(String message) {
			this.Append(message, Priority.FATAL,null);
		}
		internal void Fatal(String message,ErrorMessage e) {
			this.Append(message, Priority.FATAL,e);
		}
					
		private void Append(String message, Priority level, ErrorMessage e) {


            if (e == null)
                e = new ErrorMessage("");



			String now = DateTime.Now.ToString("hh:mm:ss");

			if (level >= minLogLevel) {
				LogMessage msg = new LogMessage(level.ToString(),now,message,e);
				
				//Every log message is reported to the GEIMAddIn-messages window,
				//where it is displayed
                //GIEM.GIEMAddIn.AppendLogMessage = msg; //commented by Kristina Aug'08


				logMessages.Add(msg);
				if(isFileLogger) {
					try {
						lock(monitor) {
							sw = File.AppendText(logDirectory);
							sw.Write("[" + msg.level + "] ");
							sw.Write(msg.dateTime + ": ");
							sw.Write(msg.message + "\n");
							sw.Flush();
							sw.Close();
						}
					} catch  {
						// do nothing
					}
				}
			}
		}

		/// <summary>
		/// Count the logged messages, which have the priority "level"
		/// </summary>
		/// <param name="level"></param>
		/// <returns></returns>
		internal int getMessageCount(Priority level) {
			int count = 0;

			if (this.logMessages != null && this.logMessages.Count != 0) {								
				foreach(LogMessage m in this.logMessages) {
					if (m.level.ToString().ToLower().Equals(level.ToString().ToLower()))
						count++;
				}				
			}
			return count;
		}


		/// <summary>
		/// The methods returns true, if the logged messages only have priority "INFO" or lower.
		/// This method is used by the validator in order to determine, if a validation run
		/// was successful or not
		/// </summary>
		/// <returns></returns>
		internal bool containsOnlyINFOMessages() {
			bool rv = true;

			if (this.logMessages != null && this.logMessages.Count != 0) {
				foreach (LogMessage m in this.logMessages) {
					Priority p = (Priority)(Enum.Parse(typeof (Priority),m.level,true));
					if (p > Priority.INFO) {
						rv = false;
						break;
					}
				}
			}

			return rv;
		}

		/// <summary>
		/// The methods returns true, if the logged messages only have priority "WARN" or lower.
		/// This method is used by the validator in order to determine, if a validation run
		/// was successful or not
		/// </summary>
		/// <returns></returns>
		internal bool containsOnlyWARNMessages() {
			bool rv = true;

			if (this.logMessages != null && this.logMessages.Count != 0) {
				foreach (LogMessage m in this.logMessages) {
					Priority p = (Priority)(Enum.Parse(typeof (Priority),m.level,true));
					if (p > Priority.WARN) {
						rv = false;
						break;
					}
				}
			}

			return rv;
		}
		

		/// <summary>
		/// Return the logged messages
		/// </summary>
		/// <returns></returns>
		internal IList getLogMessages() {
			return this.logMessages;
		}



		/// <summary>
		/// Set the log messages
		/// </summary>
		/// <param name="m"></param>
		public void setLogMessages(ArrayList m) {
			this.logMessages = m;
		}



		/// <summary>
		/// writes all logmessages to a file
		/// </summary>
		/// <param name="filename">the path and filename of the destination file</param>
		internal void writeToFile(String filename) {
			sw = File.CreateText(filename);
			foreach(LogMessage msg in this.logMessages) {
				sw.Write("[" + msg.level + "] ");
				sw.Write(msg.dateTime + ": ");
				sw.Write(msg.message + "\n");
			}
			sw.Flush();
			sw.Close();
			
		}


	}
}
