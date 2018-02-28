using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.Wizards.dev.ui;

namespace ViennaStandalone
{
    /// <summary>
    /// Class with program entry point.
    /// </summary>
    internal sealed class Program
    {
        /// <summary>
        /// Program entry point.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(catchAllUnhandledExceptions);

            // Add the event handler for handling UI thread exceptions to the event.
            Application.ThreadException += new ThreadExceptionEventHandler(catchUIThreadException);

            try
            {
                // Set the unhandled exception mode to force all Windows Forms errors to go through
                // our handler.
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            }
            catch (InvalidOperationException)
            {
                //this doesn't work if there are already controls created. I that case we don't bother
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new SelectPackageForm());
        }





        static void catchAllUnhandledExceptions(object sender, UnhandledExceptionEventArgs args)
        {
            processException((Exception)args.ExceptionObject);
        }
        static void catchUIThreadException(object sender, ThreadExceptionEventArgs args)
        {
            processException(args.Exception);
        }
        /// <summary>
        /// Process the given exception by logging it to the log and showing a message box to the user
        /// </summary>
        /// <param name="e">the exception</param>
        public static void processException(Exception e)
        {
            string exceptionInfo = "An unhandled exception has occured." + Environment.NewLine
                        + "Error Message: " + e.Message + Environment.NewLine
                        + "Stacktrace: " + e.StackTrace;
            if (e.InnerException != null)
                exceptionInfo += Environment.NewLine + "Inner Exception: " + e.InnerException.Message + Environment.NewLine 
                    + "Stacktrace: " + e.InnerException.StackTrace;
            //Logger.logError(exceptionInfo);
            MessageBox.Show(exceptionInfo, "Unexpected Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
