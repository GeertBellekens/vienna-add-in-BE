using CctsRepository.DocLibrary;
using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.repo.DocLibrary
{
    internal partial class UpccDocLibrary
    {
        public IMa DocumentRoot
        {
            get
            {
                var mas = new List<IMa>(Mas);
                // collect ASMAs
                var asmas = new List<IAsma>();
                foreach (var ma in Mas)
                {
                    if (ma.Asmas != null)
                    {
                        asmas.AddRange(ma.Asmas);
                    }
                }
                // remove all MAs that are associated via an ASMA
                foreach (var asma in asmas)
                {
                    if (asma.AssociatedBieAggregator.IsMa)
                    {
                        mas.Remove(asma.AssociatedBieAggregator.Ma);
                    }
                }
                return mas.Count == 0 ? null : mas[0];
            }
        }

        public IEnumerable<IMa> NonRootMas
        {
            get
            {
                var mas = new List<IMa>(Mas);
                mas.Remove(DocumentRoot);
                return mas;
            }
        }
    }
}
