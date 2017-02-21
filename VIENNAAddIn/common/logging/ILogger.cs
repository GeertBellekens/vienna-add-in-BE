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
    /// Interface for basic logging functionality. 
    /// </summary>
    public interface ILogger
    {
        void LogError(LogMessage message);
        void LogWarning(LogMessage message);
        void LogInfo(LogMessage message); 
    }
}
