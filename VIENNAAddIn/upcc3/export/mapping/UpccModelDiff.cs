using System.Collections.Generic;
using CctsRepository;
using CctsRepository.BieLibrary;
using CctsRepository.DocLibrary;
using System.Linq;
using VIENNAAddIn.Utils;

namespace VIENNAAddIn.upcc3.export.mapping
{
    public class UpccModelDiff
    {
        private readonly IDocLibrary docLibraryComplete;
        private readonly IDocLibrary docLibrarySubset;

        public UpccModelDiff(IDocLibrary docLibraryComplete, IDocLibrary docLibrarySubset)
        {
            this.docLibraryComplete = docLibraryComplete;
            this.docLibrarySubset = docLibrarySubset;
        }

        public Dictionary<string, List<string>> CalculateDiff()
        {
            Dictionary<string, List<string>> diff = new Dictionary<string, List<string>>();

            IMa documentRootComplete = docLibraryComplete.DocumentRoot;
            IMa documentRootSubset = docLibrarySubset.DocumentRoot;

            CalculateDiffForMas(documentRootComplete, documentRootSubset, diff);

            return diff;
        }

        private void CalculateDiffForMas(IMa maComplete, IMa maSubset, Dictionary<string, List<string>> diff)
        {
            Dictionary<string, IAsma> childAsmasOfSubset = new Dictionary<string, IAsma>();

            foreach (IAsma childAsmaOfSubset in maSubset.Asmas)
            {
                childAsmasOfSubset.Add(childAsmaOfSubset.Name, childAsmaOfSubset);
            }
                                   
            foreach (IAsma childAsmaOfComplete in maComplete.Asmas)
            {
                if (childAsmasOfSubset.ContainsKey(childAsmaOfComplete.Name))
                {
                    BieAggregator bieAggregatorOfComplete = childAsmaOfComplete.AssociatedBieAggregator;

                    if (bieAggregatorOfComplete.IsMa)
                    {
                        CalculateDiffForMas(bieAggregatorOfComplete.Ma, childAsmasOfSubset[childAsmaOfComplete.Name].AssociatedBieAggregator.Ma, diff);
                    }
                    else if (bieAggregatorOfComplete.IsAbie)
                    {
                        CalculateDiffForAbie(bieAggregatorOfComplete.Abie, childAsmasOfSubset[childAsmaOfComplete.Name].AssociatedBieAggregator.Abie, diff);
                    }
                }
                else
                {
                    // den subtree welcher am asma haengt entfernen
                }
            }
        }

        private void CalculateDiffForAbie(IAbie abieOfComplete, IAbie abieOfSubset, Dictionary<string, List<string>> diff)
        {           
            HashSet<string> missingBbies = new HashSet<string>();

            foreach (IBbie bbie in abieOfComplete.Bbies)
            {
                missingBbies.Add(bbie.Name);
            }

            missingBbies.ExceptWith(abieOfSubset.Bbies.Select(b => b.Name));

            List<string> values = diff.GetAndCreate(abieOfComplete.Name.Substring(0, abieOfComplete.Name.IndexOf('_')));

            foreach (string bbie in missingBbies)
            {
                values.Add(bbie.Substring(0, bbie.IndexOf('_')));
            }
        }
    }
}