// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Xml;
using CctsRepository;
using CctsRepository.BieLibrary;
using CctsRepository.DocLibrary;
using VIENNAAddIn.upcc3.Wizards.dev.binding;
using VIENNAAddIn.upcc3.Wizards.dev.cache;
using VIENNAAddIn.upcc3.Wizards.dev.util;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.subsettingmodel
{
    public class TemporarySubSettingModel : TemporaryModel, INotifyPropertyChanged
    {
        #region Backing Fields for Binding Properties

        private List<CheckableTreeViewItem> mCandidateAbieItems;
        private List<string> mCandidateDocLibraryNames;
        private List<CheckableItem> mCandidateRootElementItems;
        private List<string> mCandidateRootElementNames;
        private List<IOConnectionItem> mPotentialAsbieItems;
        private List<CheckableItem> mPotentialBbieItems;
        private string mRootElementName;

        #endregion

        #region Class Fields

        private readonly CcCache ccCache;
        private readonly List<CandidateDocLibrary> mCandidateDocLibraries;
        private int idCounter;
        private CandidateRootElement rootElement;
        public string namespacePrefix { get; set; }
        public string targetNamespace { get; set; }
        public string outputDirectory { get; set; }

        #endregion

        public TemporarySubSettingModel(ICctsRepository cctsRepository)
        {
            mCandidateAbieItems = new List<CheckableTreeViewItem>();

            ccCache = CcCache.GetInstance(cctsRepository);

            mCandidateDocLibraries =
                new List<CandidateDocLibrary>(
                    ccCache.GetDocLibraries().ConvertAll(doclib => new CandidateDocLibrary(doclib)));
            CandidateDocLibraryNames =
                new List<string>(mCandidateDocLibraries.ConvertAll(doclib => doclib.OriginalDocLibrary.Name));
            // Populate the model with the appropriate DOC library which contains the root MA.
        }
        
        #region Binding Properties

        public string currentDocLibrary;

        public string RootElement
        {
            set
            {
                mRootElementName = value;
                OnPropertyChanged(BindingPropertyNames.TemporarySubSettingModel.RootElement);
            }
            get { return mRootElementName; }
        }

        public List<IOConnectionItem> PotentialAsbieItems
        {
            set
            {
                mPotentialAsbieItems = value;
                OnPropertyChanged(BindingPropertyNames.TemporarySubSettingModel.PotentialAsbieItems);
            }
            get { return mPotentialAsbieItems; }
        }

        public List<string> CandidateDocLibraryNames
        {
            set
            {
                mCandidateDocLibraryNames = value;
                OnPropertyChanged(BindingPropertyNames.TemporarySubSettingModel.CandidateDocLibraryNames);
            }

            get { return mCandidateDocLibraryNames; }
        }

        public List<string> CandidateRootElementNames
        {
            set
            {
                mCandidateRootElementNames = value;
                OnPropertyChanged(BindingPropertyNames.TemporarySubSettingModel.CandidateRootElementNames);
            }

            get { return mCandidateRootElementNames; }
        }

        public List<CheckableItem> PotentialBbieItems
        {
            set
            {
                mPotentialBbieItems = value;
                OnPropertyChanged(BindingPropertyNames.TemporarySubSettingModel.PotentialBbieItems);
            }

            get { return mPotentialBbieItems; }
        }

        public List<CheckableItem> CandidateRootElementItems
        {
            set
            {
                mCandidateRootElementItems = value;
                OnPropertyChanged(BindingPropertyNames.TemporarySubSettingModel.CandidateRootElementItems);
            }

            get { return mCandidateRootElementItems; }
        }

        public ObservableCollection<CheckableTreeViewItem> CandidateAbieItems
        {
            set
            {
                mCandidateAbieItems = new List<CheckableTreeViewItem>(value);
                OnPropertyChanged(BindingPropertyNames.TemporarySubSettingModel.CandidateAbieItems);
            }

            get { return new ObservableCollection<CheckableTreeViewItem>(mCandidateAbieItems); }
        }

        #endregion

        #region buildCheckabelTreeView

        private List<CheckableTreeViewItem> buildCheckableTreeView(CandidateRootElement ma)
        {
            var checkableTreeViewItems = new List<CheckableTreeViewItem>();
            foreach (IAsma asma in ma.OriginalMa.Asmas)
            {
                if (asma.AssociatedBieAggregator.IsMa)
                {
                    var currentItem = new CheckableTreeViewItem(asma.AssociatedBieAggregator.Ma.Name, idCounter++)
                                          {Parent = null};
                    currentItem.Children = findChildren(currentItem, asma.AssociatedBieAggregator.Ma);

                    checkableTreeViewItems.Add(currentItem);
                }
                else if (asma.AssociatedBieAggregator.IsAbie)
                {
                    rootElement.CandidateAbies.Add(new CandidateAbie(asma.AssociatedBieAggregator.Abie,
                                                                     AbieFindChildren(asma.AssociatedBieAggregator.Abie)));
                    var currentItem = new CheckableTreeViewItem(true, asma.AssociatedBieAggregator.Abie.Name,
                                                                idCounter++)
                                          {Parent = null};
                    currentItem.Children = findChildren(currentItem, asma.AssociatedBieAggregator.Abie);

                    checkableTreeViewItems.Add(currentItem);
                }
            }
            //also visualize Root Element in Tree!
            var root = new CheckableTreeViewItem(ma.OriginalMa.Name, idCounter++)
                           {
                               Children = new ObservableCollection<CheckableTreeViewItem>(checkableTreeViewItems)
                           };
            return new List<CheckableTreeViewItem> {root};
        }

        private ObservableCollection<CheckableTreeViewItem> findChildren(CheckableTreeViewItem parent, IMa root)
        {
            var tempList = new ObservableCollection<CheckableTreeViewItem>();
            IEnumerator enumerator = root.Asmas.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var currentAsma = (IAsma) enumerator.Current;
                if (currentAsma.AssociatedBieAggregator.IsAbie)
                {
                    IAbie currentAbie = currentAsma.AssociatedBieAggregator.Abie;
                    if (findAbie(currentAbie.Name, rootElement.CandidateAbies) == null)
                    {
                        rootElement.CandidateAbies.Add(new CandidateAbie(currentAbie,
                                                                         AbieFindChildren(currentAbie)));
                    }
                    var currentItem = new CheckableTreeViewItem(true, currentAbie.Name, idCounter++);
                    currentItem.Children = findChildren(currentItem, currentAbie);
                    currentItem.Parent = parent;
                    tempList.Add(currentItem);
                }
                else if (currentAsma.AssociatedBieAggregator.IsMa)
                {
                    IMa currentMa = currentAsma.AssociatedBieAggregator.Ma;

                    var currentItem = new CheckableTreeViewItem(currentMa.Name, idCounter++);
                    currentItem.Children = findChildren(currentItem, currentMa);
                    currentItem.Parent = parent;
                    tempList.Add(currentItem);
                }
            }
            return tempList;
        }

        private ObservableCollection<CheckableTreeViewItem> findChildren(CheckableTreeViewItem parent, IAbie root)
        {
            var tempList = new ObservableCollection<CheckableTreeViewItem>();
            IEnumerator enumerator = root.Asbies.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var currentAsbie = (IAsbie) enumerator.Current;
                IAbie currentAbie = currentAsbie.AssociatedAbie;
                var currentItem = new CheckableTreeViewItem(true, currentAbie.Name, idCounter++);
                currentItem.Children = findChildren(currentItem, currentAbie);
                currentItem.Parent = parent;
                tempList.Add(currentItem);
            }
            return tempList;
        }

        private static List<CandidateAbie> AbieFindChildren(IAbie root)
        {
            var tempList = new List<CandidateAbie>();
            IEnumerator enumerator = root.Asbies.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var currentAsbie = (IAsbie) enumerator.Current;
                IAbie currentAbie = currentAsbie.AssociatedAbie;
                tempList.Add(new CandidateAbie(currentAbie, AbieFindChildren(currentAbie)));
            }
            return tempList;
        }

        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(Enum fieldName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(fieldName.ToString()));
            }
        }

        #endregion

        public void SetSelectedCandidateDocLibrary(string selectedDocLibrary)
        {
            foreach (CandidateDocLibrary candidateDocLibrary in mCandidateDocLibraries)
            {
                if (candidateDocLibrary.OriginalDocLibrary.Name.Equals(selectedDocLibrary))
                {
                    candidateDocLibrary.Selected = true;
                    rootElement = new CandidateRootElement(candidateDocLibrary.OriginalDocLibrary.DocumentRoot)
                                      {CandidateAbies = new List<CandidateAbie>()};
                    RootElement = rootElement.OriginalMa.Name;
                    mCandidateAbieItems.Clear();
                    mCandidateAbieItems = buildCheckableTreeView(rootElement);
                    CandidateAbieItems = new ObservableCollection<CheckableTreeViewItem>(mCandidateAbieItems);
                    return;
                    //override selection of root Element!
                    //CandidateRootElementNames = new List<string>(candidateDocLibrary.CandidateRootElements.ConvertAll(new Converter<CandidateRootElement, string>(CandidateRootElementToString)));                        
                }
                candidateDocLibrary.Selected = false;
            }
        }

        public void SetSelectedCandidateRootElement(string selectedRootElement)
        {
            mCandidateAbieItems.Clear();
            foreach (CandidateDocLibrary candidateDocLibrary in mCandidateDocLibraries)
            {
                if (candidateDocLibrary.Selected)
                {
                    foreach (CandidateRootElement candidateRootElement in candidateDocLibrary.CandidateRootElements)
                    {
                        if (candidateRootElement.OriginalMa.Name.Equals(selectedRootElement))
                        {
                            candidateRootElement.Selected = true;
                            //fills CandidateAbieItems with hierachical content
                            buildCheckableTreeView(candidateRootElement);
                        }
                        else
                        {
                            candidateRootElement.Selected = false;
                        }
                    }
                }
            }
        }

        public void SetCheckedPotentialBbie(CheckableItem checkableItem)
        {
            CandidateAbie abie = FindSelectedAbie(rootElement.CandidateAbies);

            foreach (PotentialBbie potentialBbie in abie.PotentialBbies)
            {
                if (potentialBbie.Name.Equals(checkableItem.Text))
                {
                    potentialBbie.Checked = checkableItem.Checked;
                }
            }
        }

        public void SetSelectedCandidateAbie(CheckableTreeViewItem checkableTreeViewItem)
        {
            mPotentialBbieItems = new List<CheckableItem>();
            CandidateAbie abie = findAbie(checkableTreeViewItem.Text, rootElement.CandidateAbies);
            if (abie != null)
            {
                if (abie.PotentialBbies == null)
                {
                    var bbies = new List<IBbie>(abie.OriginalAbie.Bbies);
                    abie.PotentialBbies =
                        new List<PotentialBbie>(
                            bbies.ConvertAll(potBbie => new PotentialBbie(potBbie.Name)));
                }
                if (abie.PotentialAsbies == null)
                {
                    var asbies = new List<IAsbie>(abie.OriginalAbie.Asbies);
                    abie.PotentialAsbies =
                        new List<PotentialAsbie>(
                            asbies.ConvertAll(
                                potAsbie =>
                                new PotentialAsbie(potAsbie.Name, (potAsbie.AssociatingAbie.Equals(potAsbie)))));
                }
                mPotentialBbieItems =
                    new List<CheckableItem>(
                        abie.PotentialBbies.ConvertAll(
                            potBbie => new CheckableItem(potBbie.Checked, potBbie.Name, true, false, Cursors.Arrow)));

                mPotentialAsbieItems =
                    new List<IOConnectionItem>(
                        abie.PotentialAsbies.ConvertAll(
                            potAsbie => new IOConnectionItem(potAsbie.Name, potAsbie.Incoming, potAsbie.Checked)));

                abie.Selected = true;
                UnSelectAllButAbie(abie.Name, rootElement.CandidateAbies);
            }
            PotentialBbieItems = mPotentialBbieItems;
            PotentialAsbieItems = mPotentialAsbieItems;
        }

        private static CandidateAbie FindSelectedAbie(IEnumerable<CandidateAbie> abies)
        {
            CandidateAbie returnAbie = null;
            if (abies != null)
            {
                foreach (CandidateAbie abie in abies)
                {
                    if (abie.Selected)
                    {
                        return abie;
                    }
                    returnAbie = FindSelectedAbie(abie.PotentialAbies);
                    if (returnAbie != null)
                    {
                        if (returnAbie.Selected)
                        {
                            return returnAbie;
                        }
                    }
                }
            }
            return returnAbie;
        }

        private static void UnSelectAllButAbie(string selectedAbie, IEnumerable<CandidateAbie> abies)
        {
            if (abies != null)
            {
                foreach (CandidateAbie abie in abies)
                {
                    if (!abie.Name.Equals(selectedAbie))
                    {
                        abie.Selected = false;
                    }
                    UnSelectAllButAbie(selectedAbie, abie.PotentialAbies);
                }
            }
        }

        private static CandidateAbie findAbie(string selectedAbie, IEnumerable<CandidateAbie> abies)
        {
            CandidateAbie returnAbie = null;
            if (abies != null)
            {
                foreach (CandidateAbie abie in abies)
                {
                    if (abie.Name.Equals(selectedAbie))
                    {
                        return abie;
                    }
                    returnAbie = findAbie(selectedAbie, abie.PotentialAbies);
                    if (returnAbie != null)
                    {
                        return returnAbie;
                    }
                }
            }
            return returnAbie;
        }

        public void SetCheckedCandidateAbie(CheckableTreeViewItem checkableTreeViewItem)
        {
            if (checkableTreeViewItem != null)
            {
                CandidateAbie abie = findAbie(checkableTreeViewItem.Text, rootElement.CandidateAbies);
                if (abie == null)
                {
                    return; // it's ok.. we found an MA
                }
                //check all related Nodes from tree if checked is true
                if (checkableTreeViewItem.Checked)
                {
                    checkAllParents(checkableTreeViewItem);
                }
                else
                {
                    checkChildren(checkableTreeViewItem);
                }
                //checkChildren(result, checkedValue); change from 08.04.2010 => only uncheck children

                if (abie.PotentialBbies == null)
                {
                    var bbies = new List<IBbie>(abie.OriginalAbie.Bbies);
                    abie.PotentialBbies =
                        new List<PotentialBbie>(bbies.ConvertAll(potBbie => new PotentialBbie(potBbie.Name)));
                }

                foreach (PotentialBbie bbie in abie.PotentialBbies)
                {
                    bbie.Checked = checkableTreeViewItem.Checked;
                }

                abie.Checked = checkableTreeViewItem.Checked;
                CandidateAbieItems = new ObservableCollection<CheckableTreeViewItem>(mCandidateAbieItems);
            }
        }

        private void checkAllParents(CheckableTreeViewItem itemToCheck)
        {
            if (itemToCheck.Parent != null)
            {
                itemToCheck.Parent.Checked = itemToCheck.Checked;
                SetCheckedCandidateAbie(itemToCheck.Parent);
                //if (itemToCheck.Checked)
                //{
                //    var abie = findAbie(itemToCheck.Parent.Text, rootElement.CandidateAbies);
                //    if (abie != null)
                //    {
                //        abie.Checked = itemToCheck.Checked;
                //        itemToCheck.Parent.Checked = itemToCheck.Checked;
                //    }
                //}
                //checkAllParents(itemToCheck.Parent);
            }
        }

        private void checkChildren(CheckableTreeViewItem itemToCheck)
        {
            foreach (CheckableTreeViewItem child in itemToCheck.Children)
            {
                child.Checked = itemToCheck.Checked;
                SetCheckedCandidateAbie(child);
                //var abie = findAbie(child.Text, rootElement.CandidateAbies);
                //abie.Checked = itemToCheck.Checked;
                //child.Checked = itemToCheck.Checked;
                //if (child.Children != null)
                //{
                //    checkChildren(child);
                //}
            }
        }

        private static List<CheckableTreeViewItem> findCheckableTreeViewItems(string selectedAbie,
                                                                              IEnumerable<CheckableTreeViewItem>
                                                                                  listToSearch)
        {
            var returnList = new List<CheckableTreeViewItem>();
            foreach (CheckableTreeViewItem candidateAbieItem in listToSearch)
            {
                if (candidateAbieItem.Text.Equals(selectedAbie))
                {
                    returnList.Add(candidateAbieItem);
                }
                if (candidateAbieItem.Children != null)
                {
                    returnList.AddRange(findCheckableTreeViewItems(selectedAbie, candidateAbieItem.Children));
                }
            }
            return returnList;
        }


        public void SetNoSelectedCandidateAbie()
        {
            CandidateAbieItems = new ObservableCollection<CheckableTreeViewItem>();
            PotentialBbieItems = new List<CheckableItem>();
        }

        public void createSubSet()
        {
            var abiesToRemove = new List<String>();
            var bbiesToRemove = new Dictionary<String, List<String>>();
            var asbiesToRemove = new Dictionary<String, List<String>>();

            followAsmasAndRemoveUnused(rootElement.OriginalMa);
            foreach (IBieLibrary bieLibrary in ccCache.GetBieLibraries())
            {
                var abies = new List<IAbie>(bieLibrary.Abies);
                foreach (IAbie abie in abies)
                {
                    List<CheckableTreeViewItem> result = findCheckableTreeViewItems(abie.Name, mCandidateAbieItems);
                    if (result.Count == 0)
                    {
                        abiesToRemove.Add(abie.Name);
                        bieLibrary.RemoveAbie(abie);
                    }
                    else
                    {
                        if (testIfAllCheckableTreeViewItemsAreUnchecked(result))
                        {
                            abiesToRemove.Add(abie.Name);
                            bieLibrary.RemoveAbie(abie);
                        }
                        else
                        {
                            CandidateAbie candidateAbie = findAbie(abie.Name, rootElement.CandidateAbies);
                            //update asbies
                            if (candidateAbie.PotentialAsbies != null)
                                //if the potentialAsbies are null the user hasn't even looked at them.. nothing to do!
                            {
                                if (candidateAbie.PotentialAsbies.Count == 0)
                                //for now this is almost always 0.. so we will examine children also!
                                {
                                    foreach (CheckableTreeViewItem checkableTreeViewItem in result)
                                    {
                                        if (checkableTreeViewItem.Checked)
                                        {
                                            //remove unchecked Bbies, only if the item is not unchecked!
                                            var actualBbies = new List<IBbie>(abie.Bbies);
                                            var tempBbies = new List<String>();
                                            if (candidateAbie.PotentialBbies != null)
                                            {
                                                foreach (IBbie bbie in actualBbies)
                                                {
                                                    foreach (PotentialBbie potentialBbie in candidateAbie.PotentialBbies)
                                                    {
                                                        if (bbie.Name.Equals(potentialBbie.Name))
                                                        {
                                                            if (!potentialBbie.Checked)
                                                            {
                                                                tempBbies.Add(bbie.Name + bbie.Bdt.Name);
                                                                abie.RemoveBbie(bbie);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            bbiesToRemove.Add(abie.Name,tempBbies);
                                        }
                                        //examine children to find obsolete asbies
                                        foreach (CheckableTreeViewItem child in checkableTreeViewItem.Children)
                                        {
                                            if (!child.Checked)
                                            {
                                                var tmpAsbies = new List<String>();
                                                foreach (IAsbie asbie in candidateAbie.OriginalAbie.Asbies)
                                                {
                                                    if (asbie.AssociatedAbie.Name.Equals(child.Text))
                                                    {
                                                        tmpAsbies.Add(asbie.Name);
                                                        candidateAbie.OriginalAbie.RemoveAsbie(asbie);
                                                    }
                                                }
                                                asbiesToRemove.Add(abie.Name,tmpAsbies);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    IEnumerable<IAsbie> originalAsbies = candidateAbie.OriginalAbie.Asbies;
                                    var tmpAsbies = new List<String>();
                                    foreach (IAsbie asbie in originalAsbies)
                                    {
                                        foreach (PotentialAsbie potentialAsbie in candidateAbie.PotentialAsbies)
                                        {
                                            if (potentialAsbie.Name.Equals(asbie.Name))
                                            {
                                                if (!potentialAsbie.Checked)
                                                {
                                                    tmpAsbies.Add(asbie.Name);
                                                    candidateAbie.OriginalAbie.RemoveAsbie(asbie);
                                                }
                                            }
                                        }
                                    }
                                    asbiesToRemove.Add(abie.Name,tmpAsbies);
                                }
                            }
                        }
                    }
                }
            }
            if (!namespacePrefix.Equals(String.Empty) && !targetNamespace.Equals(String.Empty))
            {
                writeSchematronRules(abiesToRemove, bbiesToRemove, asbiesToRemove);
            }
        }

        public void writeSchematronRules(List<String> abiesToRemove, Dictionary<String, List<String>> bbiesToRemove, Dictionary<String, List<String>> asbiesToRemove)
        {
            var schematronRulesFile = new XmlDocument();
            var xmldecl = schematronRulesFile.CreateXmlDeclaration("1.0", null, null);
            xmldecl.Encoding = "UTF-8";
            var root = schematronRulesFile.CreateElement("iso", "schema", "http://purl.oclc.org/dsdl/schematron");
            root.SetAttribute("xmlns", "http://purl.oclc.org/dsdl/schematron");
            root.SetAttribute("xmlns:iso", "http://purl.oclc.org/dsdl/schematron");
            root.SetAttribute("xmlns:sch", "http://www.ascc.net/xml/schematron");
            root.SetAttribute("schemaVersion", "ISO19757-3");
            
            var ns = schematronRulesFile.CreateElement("iso", "ns", "http://purl.oclc.org/dsdl/schematron");
            ns.SetAttribute("prefix", namespacePrefix);
            ns.SetAttribute("uri", targetNamespace);
            root.AppendChild(ns);

            var title = schematronRulesFile.CreateElement("iso", "title", "http://purl.oclc.org/dsdl/schematron");
            title.AppendChild(schematronRulesFile.CreateTextNode("Test ISO schematron file. Introduction mode"));
            root.AppendChild(title);
            
            var abiePattern = schematronRulesFile.CreateElement("iso", "pattern", "http://purl.oclc.org/dsdl/schematron");
            abiePattern.SetAttribute("id", "abie.restrictions");
            var insideTitle = schematronRulesFile.CreateElement("iso", "title", "http://purl.oclc.org/dsdl/schematron");
            insideTitle.AppendChild(schematronRulesFile.CreateTextNode("Checking for ABIE restrictions derived from Subsetting..."));
            abiePattern.AppendChild(insideTitle);

            var abieRule = schematronRulesFile.CreateElement("iso", "rule", "http://purl.oclc.org/dsdl/schematron");
            abieRule.SetAttribute("context", "*");
            foreach (var abieName in abiesToRemove)
            {
                var report = schematronRulesFile.CreateElement("iso", "report", "http://purl.oclc.org/dsdl/schematron");
                report.SetAttribute("test", namespacePrefix+":"+abieName);
                report.AppendChild(schematronRulesFile.CreateTextNode("Element "+abieName+" has been removed during Subsetting and is therefor not allowed to be used here."));
                abieRule.AppendChild(report);
            }
            abiePattern.AppendChild(abieRule);
            root.AppendChild(abiePattern);

            var bbiePattern = schematronRulesFile.CreateElement("iso", "pattern", "http://purl.oclc.org/dsdl/schematron");
            bbiePattern.SetAttribute("id", "bbie.restrictions");
            var bbieTitle = schematronRulesFile.CreateElement("iso", "title", "http://purl.oclc.org/dsdl/schematron");
            bbieTitle.AppendChild(schematronRulesFile.CreateTextNode("Checking for BBIE restrictions derived from Subsetting..."));
            bbiePattern.AppendChild(bbieTitle);

            foreach (KeyValuePair<string, List<string>> keyValuePair in bbiesToRemove)
            {
                var bbieRule = schematronRulesFile.CreateElement("iso", "rule", "http://purl.oclc.org/dsdl/schematron");
                bbieRule.SetAttribute("context", namespacePrefix + ":" + keyValuePair.Key);
                foreach (var bbieName in keyValuePair.Value)
                {
                    var report = schematronRulesFile.CreateElement("iso", "report", "http://purl.oclc.org/dsdl/schematron");
                    report.SetAttribute("test", namespacePrefix + ":" + bbieName);
                    report.AppendChild(schematronRulesFile.CreateTextNode("Attribute " + bbieName + " has been removed during Subsetting and is therefor not allowed to be used here."));
                    bbieRule.AppendChild(report);
                }
                bbiePattern.AppendChild(bbieRule);
            }
            
            root.AppendChild(bbiePattern);

            schematronRulesFile.AppendChild(root);
            schematronRulesFile.InsertBefore(xmldecl, root);
            schematronRulesFile.Save(@outputDirectory+"\\"+RootElement+".sch");
        }

        private static bool testIfAllCheckableTreeViewItemsAreUnchecked(IEnumerable<CheckableTreeViewItem> listToSearch)
        {
            foreach (CheckableTreeViewItem checkableTreeViewItem in listToSearch)
            {
                if (checkableTreeViewItem.Checked)
                {
                    return false;
                }
            }
            return true;
        }

        private void followAsmasAndRemoveUnused(IMa rootMa)
        {
            foreach (IAsma asma in rootMa.Asmas)
            {
                if (asma.AssociatedBieAggregator.IsMa)
                {
                    followAsmasAndRemoveUnused(asma.AssociatedBieAggregator.Ma);
                }
                if (asma.AssociatedBieAggregator.IsAbie)
                {
                    List<CheckableTreeViewItem> result =
                        findCheckableTreeViewItems(asma.AssociatedBieAggregator.Abie.Name,
                                                   mCandidateAbieItems);
                    if (result == null)
                    {
                        rootMa.RemoveAsma(asma);
                    }
                    else
                    {
                        if (testIfAllCheckableTreeViewItemsAreUnchecked(result))
                        {
                            rootMa.RemoveAsma(asma);
                        }
                    }
                }
            }
        }


        public void SetCheckedPotentialAsbie(IOConnectionItem checkedAsbie)
        {
            CandidateAbie abie = FindSelectedAbie(rootElement.CandidateAbies);

            foreach (PotentialAsbie potentialAsbie in abie.PotentialAsbies)
            {
                if (potentialAsbie.Name.Equals(checkedAsbie.Text))
                {
                    potentialAsbie.Checked = checkedAsbie.Checked;
                }
            }
        }


        //public bool ContainsValidConfiguration()
        //{
        //    foreach (CandidateCcLibrary candidateCcLibrary in mCandidateCcLibraries)
        //    {
        //        if (candidateCcLibrary.Selected)
        //        {
        //            foreach (CandidateAcc candidateAcc in candidateCcLibrary.CandidateAccs)
        //            {
        //                if (candidateAcc.Selected)
        //                {
        //                    foreach (CandidateBcc candidateBcc in candidateAcc.CandidateBccs)
        //                    {
        //                        if (candidateBcc.Checked)
        //                        {
        //                            foreach (PotentialBbie potentialBbie in candidateBcc.PotentialBbies)
        //                            {
        //                                if (potentialBbie.Checked)
        //                                {
        //                                    foreach (PotentialBdt potentialBdt in potentialBbie.PotentialBdts)
        //                                    {
        //                                        if (potentialBdt.Checked) 
        //                                        {
        //                                            return true;
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return false;
        //}
    }
}