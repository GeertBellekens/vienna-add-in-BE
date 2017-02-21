using System;
using System.Collections.Generic;
using System.Linq;
using CctsRepository;
using CctsRepository.BieLibrary;
using CctsRepository.DocLibrary;
using VIENNAAddIn.upcc3.export.mapping;

namespace VIENNAAddIn.upcc3.export.transformer
{
    public static class Transformer
    {
        private static readonly HashSet<int> abiesToBeRemoved = new HashSet<int>();
        private static readonly HashSet<int> masToBeRemoved = new HashSet<int>();
        private static readonly HashSet<int> alreadyPrunedItems = new HashSet<int>();

        public static void Transform(IBieLibrary sourceBieLibrary, IBieLibrary targetBieLibrary, IDocLibrary sourceDocLibrary, IDocLibrary targetDocLibrary)
        {
            // ************** ANSATZ 1 **************************            
            // 0. BDTs und ASBIEs werden derzeit ignoriert.

            HashSet<int> existingBccs = new HashSet<int>();
            
            // 1. Hashset von allen BCCs => dazu muessen wir:
            //    => in der source BIE (i.e. ebInterface) library alle ABIEs durchlaufen, darin die BBIEs durchlaufen, 
            //       und fuer jeden BBIE den basedOn BCC im Hashset speichern

            foreach (IAbie abie in sourceBieLibrary.Abies)
            {
                foreach (IBbie bbie in abie.Bbies)
                {
                    existingBccs.Add(bbie.BasedOn.Id);
                }
            }

            // 2. Durchlaufen aller ABIEs in der BIE Library von target (i.e. UBL), fuer jeden ABIE die BBIEs durchlaufen und
            //    fuer jeden BBIE folgenden Schritt durchführen
            //    => ist der BCC, auf welchem der BBIE basiert im Hashset vorhanden?
            //         nein => loeschen des BBIEs
            //         ja => nix tun
            //
            //    The same alrogithm is used for BDT/SUP as well as ASBIEs.

            foreach (IAbie abie in targetBieLibrary.Abies)
            {
                List<IBbie> bbies = new List<IBbie>(abie.Bbies);

                foreach (IBbie bbie in bbies)
                {
                    if (!existingBccs.Contains(bbie.BasedOn.Id))
                    {
                        abie.RemoveBbie(bbie);
                    }
                }
            }

            // 3. Durchlaufen des DOC Library Baumes, und bei den Blättern beginnend: existiert ein BBIE oder an ASBIE innherhalb
            //    des aktuellen ABIEs? 
            //
            //    Nein? Dann wird der ABIE gelöscht. Es müssen auch die ASMAs oder ASBIEs welche auf den ABIE zeigen, entfernt werden.

            PruneUpccModel(targetDocLibrary, targetBieLibrary);


            // ************** ANSATZ 2 **************************
            // Prinzipiell ist der Algorithmus gleich nur dass in diesem Ansatz nicht ueber die BIE Libraries iteriert wird
            // sondern rekursiv ueber die DOC Baeume.
        }

        private static void PruneUpccModel(IDocLibrary docLibrary, IBieLibrary bieLibrary)
        {
            abiesToBeRemoved.Clear();
            masToBeRemoved.Clear();
            alreadyPrunedItems.Clear();

            PruneMa(docLibrary.DocumentRoot);

            List<IAbie> abies = new List<IAbie>(bieLibrary.Abies);
            foreach (IAbie abie in abies)
            {
                if (abiesToBeRemoved.Contains(abie.Id))
                {
                    bieLibrary.RemoveAbie(abie);
                }
            }

            List<IMa> mas = new List<IMa>(docLibrary.Mas);
            foreach (IMa ma in mas)
            {
                if (masToBeRemoved.Contains(ma.Id))
                {
                    docLibrary.RemoveMa(ma);
                }
            }
        }

        private static bool PruneMa(IMa ma)
        {
            if (!alreadyPrunedItems.Contains(ma.Id))
            {
                alreadyPrunedItems.Add(ma.Id);

                List<IAsma> asmas = new List<IAsma>(ma.Asmas);

                foreach (IAsma asma in asmas)
                {
                    if (asma.AssociatedBieAggregator.IsMa)
                    {
                        IMa associatedMa = asma.AssociatedBieAggregator.Ma;

                        if (PruneMa(associatedMa))
                        {
                            masToBeRemoved.Add(associatedMa.Id);
                            ma.RemoveAsma(asma);
                        }
                    }
                    else if (asma.AssociatedBieAggregator.IsAbie)
                    {
                        IAbie abie = asma.AssociatedBieAggregator.Abie;

                        if (PruneAbie(abie))
                        {
                            abiesToBeRemoved.Add(abie.Id);
                            ma.RemoveAsma(asma);
                        }
                    }
                }

                if (ma.Asmas.Count() == 0)
                {
                    return true;
                }

                return false;
            }
            return masToBeRemoved.Contains(ma.Id);
        }

        private static bool PruneAbie(IAbie abie)
        {
            if (!alreadyPrunedItems.Contains(abie.Id))
            {
                alreadyPrunedItems.Add(abie.Id);

                List<IAsbie> asbies = new List<IAsbie>(abie.Asbies);

                foreach (IAsbie asbie in asbies)
                {
                    IAbie associatedAbie = asbie.AssociatedAbie;

                    if (PruneAbie(associatedAbie))
                    {
                        abiesToBeRemoved.Add(associatedAbie.Id);
                        abie.RemoveAsbie(asbie);
                    }                
                }

                if ((abie.Bbies.Count() + abie.Asbies.Count()) == 0)
                {
                    return true;
                }

                return false;
            }
            return abiesToBeRemoved.Contains(abie.Id);
        }
    }
}