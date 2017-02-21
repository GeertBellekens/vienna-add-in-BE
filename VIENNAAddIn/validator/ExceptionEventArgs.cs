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
using System.ComponentModel;

namespace VIENNAAddIn.validator
{
    public class ExceptionEventArgs : EventArgs
    {
        private Exception exception;
         
        public Exception Exception
        {
            get { return this.exception; }
            
        }
                
        public ExceptionEventArgs(Exception exception)
        {
            this.exception = exception;

        }
    }
}
