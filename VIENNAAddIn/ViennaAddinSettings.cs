using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VIENNAAddIn.Utils;

namespace VIENNAAddIn
{
    public class ViennaAddinSettings:AddinSettings
    {
        protected override string configSubPath
        {
            get { return @"\Bellekens\ViennaAddin\"; }
        }

        protected override string defaultConfigFilePath
        {
            get { return Assembly.GetExecutingAssembly().Location; }
        }
        public string lastUsedExportPath
        {
            get { return this.getValue("lastUsedExportPath"); }
            set { this.setValue("lastUsedExportPath", value); }
        }
    }
}
