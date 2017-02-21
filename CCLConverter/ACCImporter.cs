using System;
using System.Collections.Generic;
using CctsRepository.CcLibrary;
using VIENNAAddIn.Utils;

namespace CCLImporter
{
    public static class ACCImporter
    {
        public static void ImportACCs(ICcLibrary ccLibrary, IEnumerable<AccSpec> accSpecs, Dictionary<string, List<AsccSpecWithAssociatedAccName>> accAsccSpecs)
        {
            // need two passes:
            //  (1) create the ACCs
            //  (2) create the ASCCs
            var accs = new Dictionary<string, IAcc>();
            foreach (var accSpec in accSpecs)
            {
                Console.WriteLine("INFO: Importing ACC " + accSpec.Name + ".");
                accs[accSpec.Name] = ccLibrary.CreateAcc(accSpec);
            }
            foreach (var accSpec in accSpecs)
            {
                Console.WriteLine("INFO: Importing ASCCs for ACC " + accSpec.Name + ".");
                var associatingAcc = accs[accSpec.Name];
                foreach (var asccSpec in accAsccSpecs.GetAndCreate(accSpec.Name))
                {
                    asccSpec.AssociatedAcc = accs[asccSpec.AssociatedAccName];
                    associatingAcc.CreateAscc(asccSpec);
                }
            }
        }
    }
}