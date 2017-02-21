using EA;
using VIENNAAddIn;
using VIENNAAddIn.upcc3.ea;
using VIENNAAddInUnitTests.TestRepository;
using Stereotype=VIENNAAddIn.upcc3.Stereotype;

namespace VIENNAAddInUnitTests.SynchTaggedValuesTest
{
    public class SynchTaggedValuesTestRepository : EARepository
    {
        public SynchTaggedValuesTestRepository()
        {
            Element prim = null;
            Element acc = null;
            Element abie = null;
            Package model = this.AddModel("Model", m => { });
            Package bLibrary = model.AddPackage(
                "bLibrary", bLib => { bLib.Element.Stereotype = Stereotype.bLibrary; }
                );
            bLibrary.AddPackage("PRIMLibrary", primLib =>
                                               {
                                                   primLib.Element.Stereotype = Stereotype.PRIMLibrary;
                                                   prim = primLib.AddPRIM("PRIM");
                                               });
            bLibrary.AddPackage("ENUMLibrary", enumLib =>
                                               {
                                                   enumLib.Element.Stereotype = Stereotype.ENUMLibrary;
                                                   Element @enum = enumLib.AddENUM("ENUM", prim);
                                                   @enum.AddAttribute("CodelistEntry", "string").With(a => a.Stereotype = Stereotype.CodelistEntry);
                                                   enumLib.AddClass("IDSCHEME").With(c => c.Stereotype = Stereotype.IDSCHEME);
                                               });
            bLibrary.AddPackage("CDTLibrary", cdtLib =>
                                              {
                                                  cdtLib.Element.Stereotype = Stereotype.CDTLibrary;
                                                  Element cdt = cdtLib.AddCDT("CDT");
                                                  cdt.AddAttribute("CON", prim).With((a => a.Stereotype = Stereotype.CON));
                                                  cdt.AddSUP(prim, "SUP");
                                              });
            bLibrary.AddPackage("CCLibrary", ccLib =>
                                             {
                                                 ccLib.Element.Stereotype = Stereotype.CCLibrary;
                                                 acc = ccLib.AddACC("ACC");
                                                 acc.AddBCC(prim, "BCC");
                                             });
            bLibrary.AddPackage("BDTLibrary", bdtLib =>
                                              {
                                                  bdtLib.Element.Stereotype = Stereotype.BDTLibrary;
                                                  Element bdt = bdtLib.AddBDT("BDT");
                                                  bdt.AddAttribute("CON", prim).With((a => a.Stereotype = Stereotype.CON));
                                                  bdt.AddSUP(prim, "SUP");
                                              });
            bLibrary.AddPackage("BIELibrary", bieLib =>
                                              {
                                                  bieLib.Element.Stereotype = Stereotype.BIELibrary;
                                                  abie = bieLib.AddABIE("ABIE");
                                                  abie.AddBBIE(prim, "BBIE");
                                              });
            bLibrary.AddPackage("DOCLibrary", bieLib => { bieLib.Element.Stereotype = Stereotype.DOCLibrary; });
            acc.AddASCC(acc, "ASCC");
            abie.AddASBIE(abie, "ASBIE", EaAggregationKind.Shared);
        }
    }
}