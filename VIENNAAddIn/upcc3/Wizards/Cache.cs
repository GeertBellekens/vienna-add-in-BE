// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using CctsRepository.DocLibrary;

namespace VIENNAAddIn.upcc3.Wizards
{
    public class cItem
    {
        public cItem()
        {
            Name = "";
            Id = 0;
        }

        public cItem(string initName, int initId)
        {
            Name = initName;
            Id = initId;
        }

        public string Name { get; set; }
        public int Id { get; set; }
    }

    public class cCheckItem : cItem
    {
        public cCheckItem()
        {
            State = CheckState.Unchecked;
        }

        public cCheckItem(string initName, int initId, CheckState initState)
            : base(initName, initId)
        {
            State = initState;
        }

        public virtual CheckState State { get; set; }
    }

    public class cABIE : cItem
    {
        public cABIE()
        {
        }

        public cABIE(string initName, int initId, int initBasedOn)
            : base(initName, initId)
        {
            BasedOn = initBasedOn;
        }

        public int BasedOn { get; set; }
    }

    public class cCON : cCheckItem
    {
        public cCON()
        {
        }

        public cCON(string initName, int initId, CheckState initState)
            : base(initName, initId, initState)
        {
        }
    }

    public class cSUP : cCheckItem
    {
        public cSUP()
        {
        }

        public cSUP(string initName, int initId, CheckState initState)
            : base(initName, initId, initState)
        {
        }
    }

    public class cCDT : cItem
    {
        public cCDT()
        {
            CON = new cCON();
            SUPs = new Dictionary<string, cSUP>();
        }

        public cCDT(string initName, int initId)
            : base(initName, initId)
        {
            CON = new cCON();
            SUPs = new Dictionary<string, cSUP>();
        }

        public cCDT(string initName, int initId, cCON initCON, IDictionary<string, cSUP> initSUPs)
            : base(initName, initId)
        {
            CON = initCON;
            SUPs = initSUPs;
        }

        public cCON CON { get; set; }
        public IDictionary<string, cSUP> SUPs { get; set; }
        public CheckState AllSUPs { get; set; }

        public void LoadCONAndSUPs(ICctsRepository repository)
        {
            if ((CON.Name.Equals("") && SUPs.Count < 1))
            {
                int cdtId = Id;
                ICdt cdt = repository.GetCdtById(cdtId);

                CON.Name = cdt.Con.Name;
                CON.Id = cdt.Con.Id;
                CON.State = CheckState.Checked;

                foreach (var sup in cdt.Sups)
                {
                    SUPs.Add(sup.Name, new cSUP(sup.Name, sup.Id, CheckState.Unchecked));
                }
            }
        }
    }

    public class cCDTLibrary : cItem
    {
        public cCDTLibrary()
        {
            CDTs = new Dictionary<string, cCDT>();
        }

        public cCDTLibrary(string initName, int initId, IDictionary<string, cCDT> initCDTs)
            : base(initName, initId)
        {
            CDTs = initCDTs;
        }

        public cCDTLibrary(string initName, int initId)
            : base(initName, initId)
        {
            CDTs = new Dictionary<string, cCDT>();
        }

        public IDictionary<string, cCDT> CDTs { get; set; }

        public void LoadCDTs(ICctsRepository repository)
        {
            if (CDTs.Count == 0)
            {
                ICdtLibrary cdtl = repository.GetCdtLibraryById(Id);

                foreach (ICdt cdt in cdtl.Cdts)
                {
                    if (CDTs.ContainsKey(cdt.Name))
                    {
                        CDTs.Clear();
                        throw new CacheException("The wizard encountered two CDTs within one CDT library having identical names. Please verify your model!");
                    }

                    CDTs.Add(cdt.Name, new cCDT(cdt.Name, cdt.Id));
                }

                if (CDTs.Count == 0)
                {
                    throw new CacheException(
                        "The CC library did not contain any CDTs. Please make sure at least one CDT is present in the library before proceeding with the wizard.");
                }
            }
        }
    }

    public class cBBIE : cCheckItem
    {
        public cBBIE()
        {
            BDTs = new List<cBDT>();
        }

        public cBBIE(string initName, int initId, int initIdOfUnderlyingType, CheckState initState, IList<cBDT> initBDTs)
            : base(initName, initId, initState)
        {
            BDTs = initBDTs;
            IdOfUnderlyingType = initIdOfUnderlyingType;
        }

        public cBBIE(string initName, int initId, int initIdOfUnderlyingType, CheckState initState)
            : base(initName, initId, initState)
        {
            BDTs = new List<cBDT>();
            IdOfUnderlyingType = initIdOfUnderlyingType;
        }

        public IList<cBDT> BDTs { get; set; }
        public int IdOfUnderlyingType { get; set; }

        
        public void SearchAndAssignRelevantBDTs(int cdtid, IDictionary<string, cBDTLibrary> bdtls, CheckState check)
        {
            BDTs = GetRelevantBDTs(cdtid, bdtls, check);
        }

        public IList<cBDT> GetRelevantBDTs(int cdtid, IDictionary<string, cBDTLibrary> bdtls, CheckState check)
        {
            IList<cBDT> relevantBdts = new List<cBDT>();

            foreach (cBDTLibrary bdtl in bdtls.Values)
            {
                foreach (cBDT bdt in bdtl.BDTs.Values)
                {
                    if (bdt.BasedOn == cdtid)
                    {
                        relevantBdts.Add(new cBDT(bdt.Name, bdt.Id, bdt.BasedOn, check));
                    }
                }
            }

            relevantBdts.Add(new cBDT("Create new BDT", -1, cdtid, CheckState.Unchecked));

            return relevantBdts;
        }
    }

    public class cBDT : cCheckItem
    {
        public cBDT()
        {
            State = CheckState.Unchecked;
        }

        public cBDT(string initName, int initId, int initBasedOn, CheckState initState)
            : base(initName, initId, initState)
        {
            BasedOn = initBasedOn;
        }

        public int BasedOn { get; set; }
    }

    public class cBCC : cCheckItem
    {
        public cBCC()
        {
            BBIEs = new Dictionary<string, cBBIE>();
        }

        public cBCC(string initName, int initId, int initType, CheckState initState)
            : base(initName, initId, initState)
        {
            BBIEs = new Dictionary<string, cBBIE>();
            Type = initType;
        }

        public cBCC(string initName, int initId, int initType, CheckState initState, IDictionary<string, cBBIE> initBBIEs)
            : base(initName, initId, initState)
        {
            BBIEs = initBBIEs;
            Type = initType;
        }

        public IDictionary<string, cBBIE> BBIEs { get; set; }

        public int Type { get; set; }
    }

    public class cASCC : cCheckItem
    {
        public cASCC(string initName, int initId, CheckState initState, IDictionary<string, cABIE> initABIEs)
            : base(initName, initId, initState)
        {
            ABIEs = initABIEs;
        }

        public IDictionary<string, cABIE> ABIEs { get; set; }
    }

    public class cACC : cCheckItem
    {
        public cACC()
        {
            BCCs = new Dictionary<string, cBCC>();
            ASCCs = new Dictionary<string, cASCC>();
        }

        public cACC(string initName, int initId, CheckState initState)
            : base(initName, initId, initState)
        {
            BCCs = new Dictionary<string, cBCC>();
            ASCCs = new Dictionary<string, cASCC>();            
        }


        public cACC(string initName, int initId, IDictionary<string, cBCC> initBCCs, IDictionary<string, cASCC> initASCCs, CheckState initState)
            : base(initName, initId, initState)
        {
            BCCs = initBCCs;
            ASCCs = initASCCs;
        }

        public IDictionary<string, cBCC> BCCs { get; set; }
        public IDictionary<string, cASCC> ASCCs { get; set; }

        public void LoadBCCsAndCreateDefaults(ICctsRepository repository, IDictionary<string, cBDTLibrary> bdtls)
        {
            if (!HasBCCs())
            {
                IAcc acc = repository.GetAccById(Id);

                foreach (IBcc bcc in acc.Bccs)
                {
                    if (BCCs.ContainsKey(bcc.Name))
                    {
                        BCCs.Clear();
                        throw new CacheException("The wizard encountered two BCCs within one ACC having identical names. Please verify your model!");
                    }

                    BCCs.Add(bcc.Name, new cBCC(bcc.Name, bcc.Id, bcc.Cdt.Id, CheckState.Unchecked));

                    BCCs[bcc.Name].BBIEs.Add(bcc.Name, new cBBIE(bcc.Name, -1, bcc.Cdt.Id, CheckState.Unchecked));

                    BCCs[bcc.Name].BBIEs[bcc.Name].SearchAndAssignRelevantBDTs(bcc.Cdt.Id, bdtls, CheckState.Unchecked);
                }                
            }
        }

        public void LoadBCCsAndBBIEs(ICctsRepository repository, IDictionary<string, cBDTLibrary> bdtls, IAbie abie)
        {
            if (!HasBCCs())
            {
                IAcc acc = repository.GetAccById(Id);

                foreach (IBcc bcc in acc.Bccs)
                {
                    if (BCCs.ContainsKey(bcc.Name))
                    {
                        BCCs.Clear();
                        throw new CacheException("The wizard encountered two BCCs within one ACC having identical names. Please verify your model!");
                    }
                    
                    BCCs.Add(bcc.Name, new cBCC(bcc.Name, bcc.Id, bcc.Cdt.Id, CheckState.Unchecked));

                    int bbieCount = 0;
                    foreach (IBbie bbie in abie.Bbies)
                    {
                        if (bbie.Name.Contains(bcc.Name))
                        {                            
                            BCCs[bcc.Name].State = CheckState.Checked;
                            BCCs[bcc.Name].BBIEs.Add(bbie.Name,
                                                     new cBBIE(bbie.Name, -1, bcc.Cdt.Id, CheckState.Checked));
                            BCCs[bcc.Name].BBIEs[bbie.Name].SearchAndAssignRelevantBDTs(bcc.Cdt.Id, bdtls, CheckState.Checked);

                            bbieCount++;
                        }
                    }
                    if (bbieCount == 0)
                    {
                        BCCs[bcc.Name].BBIEs.Add(bcc.Name, new cBBIE(bcc.Name, -1, bcc.Cdt.Id, CheckState.Unchecked));
                        BCCs[bcc.Name].BBIEs[bcc.Name].SearchAndAssignRelevantBDTs(bcc.Cdt.Id, bdtls, CheckState.Unchecked);
                    }                   
                }
            }
        }

        public void LoadASCCs(ICctsRepository repository, IDictionary<string, cBIELibrary> biels)
        {
            // TODO: temporarily disabled to forace ASCCs
            //if (!HasASCCs())
            //{
                ASCCs.Clear();

                IAcc acc = repository.GetAccById(Id);

                foreach (IAscc ascc in acc.Asccs)
                {
                    IDictionary<string, cABIE> relevantABIEs = new Dictionary<string, cABIE>();

                    foreach (cBIELibrary biel in biels.Values)
                    {
                        foreach (cABIE abie in biel.ABIEs.Values)
                        {
                            if (abie.BasedOn == ascc.AssociatedAcc.Id)
                            {
                                relevantABIEs.Add(abie.Name, new cABIE(abie.Name, abie.Id, abie.BasedOn));
                            }
                        }
                    }

                    if (relevantABIEs.Count > 0)
                    {
                        if (!(ascc.Name.Equals("")))
                        {                            
                            if (!ASCCs.ContainsKey(ascc.Name))
                            {
                                //todo: change Checkstate to real state of potential ASBIE
                                ASCCs.Add(ascc.Name, new cASCC(ascc.Name, ascc.Id, CheckState.Unchecked, relevantABIEs));
                            }
                            else
                            {
                                throw new CacheException("The wizard encountered two ASCCs having identical target role names. Please make sure that all ASCCs for a particular ACC have unique target role names before proceeding with the wizard.");
                            }                                                    
                        }
                        else
                        {
                            throw new CacheException("The wizard encountered an association whose target role name is not set properly. Please make sure that all ASCC's target names are set properly before proceeding with the wizard.");
                        }
                    }
                }
            //}
        }

        public bool HasBCCs()
        {
            if (BCCs.Count > 0)
            {
                return true;
            }

            return false;
        }

        public bool HasASCCs()
        {
            if (ASCCs.Count > 0)
            {
                return true;
            }

            return false;
        }  
    }

    public class cCCLibrary : cItem
    {
        public cCCLibrary()
        {
            ACCs = new Dictionary<string, cACC>();
        }

        public cCCLibrary(string initName, int initId)
            : base(initName, initId)
        {
            ACCs = new Dictionary<string, cACC>();
        }


        public cCCLibrary(string initName, int initId, IDictionary<string, cACC> initACCs)
            : base(initName, initId)
        {
            ACCs = initACCs;
        }

        public IDictionary<string, cACC> ACCs { get; set; }

        public void LoadACCs(ICctsRepository repository)
        {
            if (ACCs.Count == 0)
            {
                ICcLibrary ccl = repository.GetCcLibraryById(Id);

                foreach (IAcc acc in ccl.Accs)
                {
                    if (ACCs.ContainsKey(acc.Name))
                    {
                        ACCs.Clear();
                        throw new CacheException("The wizard encountered two ACCs having identical names. Please verify your model!");
                    }

                    ACCs.Add(acc.Name, new cACC(acc.Name, acc.Id, CheckState.Unchecked));
                }
            }

            if (ACCs.Count == 0)
            {
                throw new CacheException("The CC library did not contain any ACCs. Please make sure at least one ACC is present in the library before proceeding with the wizard.");
            }
        }
    }

    public class cBDTLibrary : cItem
    {
        public cBDTLibrary()
        {
            BDTs = new Dictionary<string, cBDT>();
        }

        public cBDTLibrary(string initName, int initId)
            : base(initName, initId)
        {
            BDTs = new Dictionary<string, cBDT>();
        }

        public cBDTLibrary(string initName, int initId, IDictionary<string, cBDT> initBDTs)
            : base(initName, initId)
        {
            BDTs = initBDTs;
        }

        public IDictionary<string, cBDT> BDTs { get; set; }
    }

    public class cBIELibrary : cItem
    {
        public cBIELibrary()
        {
            ABIEs = new Dictionary<string, cABIE>();
        }

        public cBIELibrary(string initName, int initId)
            : base(initName, initId)
        {
            ABIEs = new Dictionary<string, cABIE>();
        }

        public cBIELibrary(string initName, int initId, IDictionary<string, cABIE> initABIEs)
            : base(initName, initId)
        {
            ABIEs = initABIEs;
        }

        public IDictionary<string, cABIE> ABIEs { get; set; }
    }
  
    public class cDOC : cCheckItem
    {
        public cDOC(string initName, int initId, CheckState initState, string baseUrn, string initTargetNSPrefix,cBIV biv) : base(initName, initId, initState)
        {            
            BaseUrn = baseUrn;
            TargetNamespacePrefix = initTargetNSPrefix;
            BIV = biv;
            if (biv.DocL !=null && biv.DocL.DocumentRoot != null)
            {
            	TargetNamespace = BaseUrn + "-" + biv.DocL.DocumentRoot.Name;
            }
        }
        public string BaseUrn { get; set; }
        public string TargetNamespace { get; set; }
        public string TargetNamespacePrefix { get; set; }
        public string OutputDirectory { get; set; }
        public cBIV BIV {get;private set;}
    }

    public class cBIV : cItem
    {
    	cDOC _DOC;
		IDocLibrary _DocL;
		IMa _Document;
        public cBIV(string initName, int initId,ICctsRepository repo) : base(initName, initId)
        {
        	this.Repository = repo;
        }
        public ICctsRepository Repository {get; private set;}
        public cDOC DOC 
        {
			get 
			{
				if (_DOC == null) LoadDOC(Repository);
				return _DOC;
			}
			private set 
			{
				_DOC = value;
			}
		}
		
        public IDocLibrary DocL
        {
			get 
			{
				if (_DocL == null) LoadDOC(Repository);
				return _DocL;
			}
			private set 
			{
				_DocL = value;
			}
		}
		
        public IMa Document 
        {
			get 
			{
				if (_Document == null) LoadDOC(Repository);
				return _Document;
			}
			private set 
			{
				_Document = value;
			}
		}

        private void LoadDOC(ICctsRepository repository)
        {
            DocL = repository.GetDocLibraryById(Id);
            Document = DocL.DocumentRoot;
            if (_Document != null && _DocL != null)
            {
                DOC = new cDOC(Document.Name, Document.Id, CheckState.Unchecked, DocL.BaseURN, DocL.NamespacePrefix,this);
            }
            else
            {
            	DOC = null;
            }
        }
    }


    public class Cache
    {
        public IDictionary<string, cCCLibrary> CCLs { get; set; }
        public IDictionary<string, cCDTLibrary> CDTLs { get; set; }
        public IDictionary<string, cBDTLibrary> BDTLs { get; set; }
        public IDictionary<string, cBIELibrary> BIELs { get; set; }
        public IDictionary<string, cBIV> BIVs { get; set; }

        public Cache()
        {
            CCLs = new Dictionary<string, cCCLibrary>();
            CDTLs = new Dictionary<string, cCDTLibrary>();
            BDTLs = new Dictionary<string, cBDTLibrary>();
            BIELs = new Dictionary<string, cBIELibrary>();
            BIVs = new Dictionary<string, cBIV>();
        }

        public void EmptyCache()
        {
            CCLs.Clear();
            CDTLs.Clear();
            BDTLs.Clear();
            BIELs.Clear();
        }

        public bool PathIsValid(int typeOfPath, string[] path)
        {
            switch(typeOfPath)
            {
                case CacheConstants.PATH_BCCs:
                    {
                        if ((path.Length > 0) && !(CCLs.ContainsKey(path[0])))
                        {
                            return false;
                        }

                        if ((path.Length > 1) && !(CCLs[path[0]].ACCs.ContainsKey(path[1])))
                        {
                            return false;
                        }

                        if ((path.Length > 2) && !(CCLs[path[0]].ACCs[path[1]].BCCs.ContainsKey(path[2])))
                        {
                            return false;
                        }

                        if ((path.Length > 3) && !(CCLs[path[0]].ACCs[path[1]].BCCs[path[2]].BBIEs.ContainsKey(path[3])))
                        {
                            return false;
                        }

                        if (path.Length > 4)
                        {
                            int countMatchingBDTs = 0;

                            foreach (cBDT bdt in CCLs[path[0]].ACCs[path[1]].BCCs[path[2]].BBIEs[path[3]].BDTs)
                            {
                                if (bdt.Name.Equals(path[4]))
                                {
                                    countMatchingBDTs++;
                                }
                            }

                            if (countMatchingBDTs < 1)
                            {
                                return false;
                            }
                        }                        
                    }
                    break;

                case CacheConstants.PATH_ASCCs:
                    {
                        if ((path.Length > 0) && !(CCLs.ContainsKey(path[0])))
                        {
                            return false;
                        }

                        if ((path.Length > 1) && !(CCLs[path[0]].ACCs.ContainsKey(path[1])))
                        {
                            return false;
                        }

                        if ((path.Length > 2) && !(CCLs[path[0]].ACCs[path[1]].ASCCs.ContainsKey(path[2])))
                        {
                            return false;
                        }
                        
                    }
                    break;

                case CacheConstants.PATH_BDTLs:
                    {
                        if ((path.Length > 0) && !(BDTLs.ContainsKey(path[0])))
                        {
                            return false;
                        }
                    }
                    break;

                case CacheConstants.PATH_BIELs:
                    {
                        if ((path.Length > 0) && !(BIELs.ContainsKey(path[0])))
                        {
                            return false;
                        }                        
                    }
                    break;

                case CacheConstants.PATH_CDTs:
                    {
                        if ((path.Length > 0) && !(CDTLs.ContainsKey(path[0])))
                        {
                            return false;
                        }

                        if ((path.Length > 1) && !(CDTLs[path[0]].CDTs.ContainsKey(path[1])))
                        {
                            return false;
                        }

                        if ((path.Length > 2) && !(CDTLs[path[0]].CDTs[path[1]].SUPs.ContainsKey(path[2])))
                        {
                            return false;
                        }
                    }
                    break;
            }

            return true;
        }

        public void LoadCCLs(ICctsRepository repository)
        {
            foreach (ICcLibrary ccl in repository.GetCcLibraries())
            {
                if (CCLs.ContainsKey(ccl.Name))
                {
                    EmptyCache();
                    throw new CacheException("The wizard encountered two CC libraries having identical names. Please make sure that all CC libraries within the model have unique names before proceeding with the wizard.");
                }

                CCLs.Add(ccl.Name, new cCCLibrary(ccl.Name, ccl.Id));
            }

            if (CCLs.Count == 0)
            {
                throw new CacheException("The repository did not contain any CC libraries. Please make sure at least one CC library is present before proceeding with the wizard.");
            }
        }

        public void LoadCDTLs(ICctsRepository repository)
        {
            foreach (ICdtLibrary cdtl in repository.GetCdtLibraries())
            {                
                if (CDTLs.ContainsKey(cdtl.Name))
                {
                    CDTLs.Clear();
                    throw new CacheException("The wizard encountered two CDT libraries having identical names. Please make sure that all CDT libraries within the model have unique names before proceeding with the wizard.");
                }
                CDTLs.Add(cdtl.Name, new cCDTLibrary(cdtl.Name, cdtl.Id));
            }

            if (CDTLs.Count == 0)
            {
                throw new CacheException("The repository did not contain any CDT libraries. Please make sure at least one CDT library is present before proceeding with the wizard.");
            }
        }

        
        public void LoadBIELsAndTheirABIEs(ICctsRepository repository)
        {
            foreach (IBieLibrary biel in repository.GetBieLibraries())
            {
                if (BIELs.ContainsKey(biel.Name))
                {
                    BIELs.Clear();
                    throw new CacheException("The wizard encountered two BIE libraries having identical names. Please make sure that all BIE libraries within the model have unique names before proceeding with the wizard.");
                }

                BIELs.Add(biel.Name, new cBIELibrary(biel.Name, biel.Id));

                foreach (IAbie abie in biel.Abies)
                {
                    if (BIELs[biel.Name].ABIEs.ContainsKey(abie.Name))
                    {
                        BIELs[biel.Name].ABIEs.Clear();
                        throw new CacheException("The wizard encountered two ABIEs within one BIE library having identical names. Please make sure that all ABIEs within each BIE library have unique names before proceeding with the wizard.");
                    }

                    BIELs[biel.Name].ABIEs.Add(abie.Name, new cABIE(abie.Name, abie.Id, abie.BasedOn.Id));
                }
            }

            if (BIELs.Count == 0)
            {
                throw new CacheException("The repository did not contain any BIE libraries. Please make sure at least one BIE library is present before proceeding with the wizard.");
            }
        }

        public void LoadBDTLsAndTheirBDTs(ICctsRepository repository)
        {
            foreach (IBdtLibrary bdtl in repository.GetBdtLibraries())
            {
                if (BDTLs.ContainsKey(bdtl.Name))
                {
                    BDTLs.Clear();
                    throw new CacheException("The wizard encountered two BDT libraries having identical names. Please make sure that all BDT libraries within the model have unique names before proceeding with the wizard.");
                }

                BDTLs.Add(bdtl.Name, new cBDTLibrary(bdtl.Name, bdtl.Id));

                foreach (IBdt bdt in bdtl.Bdts)
                {
                    if (BDTLs[bdtl.Name].BDTs.ContainsKey(bdt.Name))
                    {
                        BDTLs[bdtl.Name].BDTs.Clear();
                        throw new CacheException("The wizard encountered two BDTs within one BDT library having identical names. Please make sure that all BDTs within each BDT library have unique names before proceeding with the wizard.");
                    }

                    BDTLs[bdtl.Name].BDTs.Add(bdt.Name, new cBDT(bdt.Name, bdt.Id, bdt.BasedOn.Id, CheckState.Unchecked));
                }
            }

            if (BDTLs.Count == 0)
            {
                throw new CacheException("The repository did not contain any BDT libraries. Please make sure at least one BDT library is present before proceeding with the wizard.");
            }
        }        
		
        public void LoadBIVs(ICctsRepository repository, EA.Package selectedPackage)
        {
        	BIVs.Clear();
        	foreach (var docLibrary in repository.GetDocLibraries(selectedPackage.PackageID)) 
        	{
        		if (!BIVs.ContainsKey(docLibrary.Name))
        		{
        			BIVs.Add(docLibrary.Name, new cBIV(docLibrary.Name, docLibrary.Id,repository));
        		}
        	}
        	if (BIVs.Count == 0)
        	{
        		throw new CacheException(string.Format("No DOClibraries found in package '{0}'", selectedPackage.Name));
        	}
        }
        public void LoadBIVs(ICctsRepository repository)
        {
            foreach (IDocLibrary docl in repository.GetDocLibraries())
            {
                if (BIVs.ContainsKey(docl.Name))
                {
                    BIVs.Clear();
                    throw new CacheException("The wizard encountered two BIVs having identical names. Please make sure that all BIVs within the model have unique names before proceeding with the wizard.");
                }

                BIVs.Add(docl.Name, new cBIV(docl.Name, docl.Id,repository));
            }

            if (BIVs.Count == 0)
            {
                throw new CacheException("The repository did not contain any Business Information Views (BIVs) or any DOCLibraries. Please make sure at least one BIV or one DOCLibrary is present before proceeding with the wizard.");
            }
        }
    }

    public class CacheConstants
    {
        public const int PATH_BCCs = 1;
        public const int PATH_ASCCs = 2;
        public const int PATH_BDTLs = 3;
        public const int PATH_BIELs = 4;
        public const int PATH_CDTs = 5;
    }

    public class CacheException : Exception
    {
        public CacheException(string message)
            : base(message)
        {
        }
    }
}