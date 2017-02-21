/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections.Generic;

namespace VIENNAAddIn.validator
{
    abstract internal class IValidator
    {
        public event EventHandler<ExceptionEventArgs> Changed;

        protected virtual void OnChange(ExceptionEventArgs args)
        {
            if (Changed != null)
                Changed(this, args); 
        }

        //EA repository the validator is operating upon
        internal EA.Repository repository;
        //The current scope of the validator
        internal String scope;

        abstract internal List<ValidationMessage> validate();
    }
}
