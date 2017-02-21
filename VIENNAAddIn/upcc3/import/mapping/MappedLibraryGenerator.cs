using System.Collections.Generic;
using System.Linq;
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.BLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.DocLibrary;
using VIENNAAddIn.Utils;

namespace VIENNAAddIn.upcc3.import.mapping
{
    /// <summary>
    /// This Class manages as a last step of the generic XML Schema importer the generation of proper 
    /// BIE and BIE Doc elements in the corresponding UPCC libraries.
    /// </summary>
    public class MappedLibraryGenerator
    {
        private readonly IBLibrary bLibrary;
        private readonly string docLibraryName;
        private readonly string bieLibraryName;
        private readonly string bdtLibraryName;
        private readonly string qualifier;
        private readonly string rootElementName;
        private readonly SchemaMapping schemaMapping;
        private readonly List<BdtSpec> bdtSpecs = new List<BdtSpec>();
        private readonly List<AbieSpec> abieSpecs = new List<AbieSpec>();
        private readonly Dictionary<string, List<AsbieToGenerate>> asbiesToGenerate = new Dictionary<string, List<AsbieToGenerate>>();
        private readonly List<MaSpec> maSpecs = new List<MaSpec>();
        private readonly Dictionary<string, List<AsmaToGenerate>> asmasToGenerate = new Dictionary<string, List<AsmaToGenerate>>();
        private IBdtLibrary bdtLibrary;
        private IBieLibrary bieLibrary;
        private IDocLibrary docLibrary;

        public MappedLibraryGenerator(SchemaMapping schemaMapping, IBLibrary bLibrary, string docLibraryName, string bieLibraryName, string bdtLibraryName, string qualifier, string rootElementName)
        {
            this.bLibrary = bLibrary;
            this.docLibraryName = docLibraryName;
            this.bieLibraryName = bieLibraryName;
            this.bdtLibraryName = bdtLibraryName;
            this.qualifier = qualifier;
            this.rootElementName = rootElementName;
            this.schemaMapping = schemaMapping;
        }

        /// <summary>
        /// Retrieves the BDT based on the given CDT from the BDT library. If the BDT does not yet exist, it is created.
        /// </summary>
        /// <param name="bccMapping"></param>
        /// <returns></returns>
        private IBdt GetBdt(AttributeOrSimpleElementOrComplexElementToBccMapping bccMapping)
        {
            IMapping bccTypeMapping = bccMapping.BccTypeMapping;
            return bdtLibrary.GetBdtByName(bccTypeMapping.BIEName);
        }

        /// <summary>
        /// Retrieves the BDT based on the given CDT from the BDT library. If the BDT does not yet exist, it is created.
        /// </summary>
        /// <param name="splitMapping"></param>
        /// <param name="targetBcc"></param>
        /// <returns></returns>
        private IBdt GetBdt(SplitMapping splitMapping, IBcc targetBcc)
        {
            SimpleTypeToCdtMapping cdtMapping = splitMapping.GetCdtMappingForTargetBcc(targetBcc);
            return bdtLibrary.GetBdtByName(cdtMapping.BIEName);
        }

        /// <summary>
        /// Takes the names of the libraries to be created as well as a qualifier as input and creates the
        /// libraries.
        /// </summary>
        public void GenerateLibraries()
        {
            bdtLibrary = bLibrary.CreateBdtLibrary(new BdtLibrarySpec
                                                   {
                                                       Name = bdtLibraryName
                                                   });
            bieLibrary = bLibrary.CreateBieLibrary(new BieLibrarySpec
                                                   {
                                                       Name = bieLibraryName
                                                   });
            docLibrary = bLibrary.CreateDocLibrary(new DocLibrarySpec
                                                   {
                                                       Name = docLibraryName
                                                   });
            GenerateBdtsAndAbiesAndMas();

            GenerateRootABIE();
        }


        private void GenerateBdtsAndAbiesAndMas()
        {
            foreach (var simpleTypeMapping in schemaMapping.GetSimpleTypeMappings())
            {
                GenerateBdtSpecsFromSimpleTypeMapping(simpleTypeMapping);
            }
            foreach (var complexTypeMapping in schemaMapping.GetComplexTypeMappings())
            {
                GenerateBdtSpecsFromComplexTypeMapping(complexTypeMapping);
            }

            CreateBdts();
            
            foreach (var complexTypeMapping in schemaMapping.GetComplexTypeMappings())
            {
                GenerateAbieAndMaSpecsFromComplexTypeMapping(complexTypeMapping);
            }

            CreateAbies();
            CreateMas();
        }

        private void CreateBdts()
        {
            foreach (var bdtSpec in bdtSpecs)
            {
                bdtLibrary.CreateBdt(bdtSpec);            
            }
        }

        private void CreateAbies()
        {
            var abies = new List<IAbie>();
            foreach (var abieSpec in abieSpecs)
            {
                abies.Add(bieLibrary.CreateAbie(abieSpec));
            }
            foreach (var abie in abies)
            {
                foreach (var asbieSpec in GenerateAsbieSpecsForAbie(abie))
                {
                    abie.CreateAsbie(asbieSpec);
                }
            }
        }

        private List<AsbieSpec> GenerateAsbieSpecsForAbie(IAbie abie)
        {
            return asbiesToGenerate.GetAndCreate(abie.Name).ConvertAll(asbieToGenerate => asbieToGenerate.GenerateSpec());
        }

        private List<AsmaSpec> GenerateAsmaSpecsForMa(IMa ma)
        {
            return asmasToGenerate.GetAndCreate(ma.Name).ConvertAll(asmaToGenerate => asmaToGenerate.GenerateSpec());
        }

        private void CreateMas()
        {
            var mas = new List<IMa>();

            foreach (var maSpec in maSpecs)
            {
                mas.Add(docLibrary.CreateMa(maSpec));
            }

            foreach (var ma in mas)
            {
                foreach (var asmaSpec in GenerateAsmaSpecsForMa(ma))
                {
                    ma.CreateAsma(asmaSpec);
                }
            }
        }

        private void GenerateBdtSpecsFromSimpleTypeMapping(SimpleTypeToCdtMapping simpleTypeMapping)
        {
            // NOTE: at this point there is only one simple type mapping which is the "SimpleTypeToCdtMapping".             
            var bdtSpec = BdtSpec.CloneCdt(simpleTypeMapping.TargetCDT, simpleTypeMapping.BIEName);
            bdtSpec.Sups = new List<BdtSupSpec>();
            bdtSpecs.Add(bdtSpec);
        }

        private void GenerateBdtSpecsFromComplexTypeMapping(ComplexTypeMapping complexTypeMapping)
        {
            if (complexTypeMapping is ComplexTypeToCdtMapping)
            {
                ComplexTypeToCdtMapping cdtMapping = (ComplexTypeToCdtMapping) complexTypeMapping;

                List<BdtSupSpec> supSpecs = new List<BdtSupSpec>();

                foreach (AttributeOrSimpleElementToSupMapping supMapping in cdtMapping.GetSupMappings())
                {
                    BdtSupSpec bdtSupSpec = BdtSpec.CloneCdtSup(supMapping.Sup);
                    bdtSupSpec.Name = supMapping.BIEName;
                    supSpecs.Add(bdtSupSpec);
                }

                BdtSpec bdtSpec = BdtSpec.CloneCdt(complexTypeMapping.TargetCdt, complexTypeMapping.BIEName);
                
                bdtSpec.Sups = supSpecs;

                bdtSpecs.Add(bdtSpec);
            }
        }

        private void GenerateAbieAndMaSpecsFromComplexTypeMapping(ComplexTypeMapping complexTypeMapping)
        {
            if (complexTypeMapping is ComplexTypeToAccMapping)
            {
                GenerateAbieSpec(complexTypeMapping, complexTypeMapping.TargetACCs.ElementAt(0), complexTypeMapping.BIEName);
            }
            else if (complexTypeMapping is ComplexTypeToMaMapping)
            {
                string maName = complexTypeMapping.ComplexTypeName;

                maSpecs.Add(new MaSpec
                {
                    Name = maName,
                });

                List<AsmaToGenerate> asmasToGenerateForThisMa = asmasToGenerate.GetAndCreate(maName);

                foreach (IAcc acc in complexTypeMapping.TargetACCs)
                {
                    var accABIESpec = GenerateAbieSpec(complexTypeMapping, acc, complexTypeMapping.ComplexTypeName + "_" + acc.Name);
                    asmasToGenerateForThisMa.Add(new AsmaToGenerate(bieLibrary, acc.Name, accABIESpec.Name));
                }

                foreach (var asmaMapping in complexTypeMapping.AsmaMappings)
                {
                    if (asmaMapping.TargetMapping is ComplexTypeToAccMapping)
                    {
                        asmasToGenerateForThisMa.Add(new AsmaToGenerate(bieLibrary, asmaMapping.BIEName,
                                                                        asmaMapping.TargetMapping.BIEName));
                    }
                    else
                    {
                        asmasToGenerateForThisMa.Add(new AsmaToGenerate(docLibrary, asmaMapping.BIEName,
                                                                        asmaMapping.TargetMapping.BIEName));
                    }
                }                
            }
        }

        private AbieSpec GenerateAbieSpec(ComplexTypeMapping complexTypeMapping, IAcc acc, string name)
        {
            List<BbieSpec> bbieSpecs = new List<BbieSpec>();
            bbieSpecs.AddRange(GenerateBbieSpecs(complexTypeMapping.BccMappings(acc)));
            bbieSpecs.AddRange(GenerateBbieSpecs(complexTypeMapping.SplitMappings(), acc));

            var abieSpec = new AbieSpec
                           {
                               BasedOn = acc,
                               Name = name,
                               Bbies = bbieSpecs,
                           };

            abieSpecs.Add(abieSpec);
            asbiesToGenerate.GetAndCreate(name).AddRange(DetermineAsbiesToGenerate(complexTypeMapping.AsccMappings(acc)));
            return abieSpec;
        }

        private IEnumerable<AsbieToGenerate> DetermineAsbiesToGenerate(IEnumerable<ComplexElementToAsccMapping> asccMappings)
        {
            foreach (var asccMapping in asccMappings)
            {
                yield return new AsbieToGenerate(bieLibrary, asccMapping.Ascc, asccMapping.BIEName, asccMapping.TargetMapping.BIEName);
            }
        }

        private IEnumerable<BbieSpec> GenerateBbieSpecs(IEnumerable<AttributeOrSimpleElementOrComplexElementToBccMapping> bccMappings)
        {
            foreach (var bccMapping in bccMappings)
            {
                var bcc = bccMapping.Bcc;
                var bbieSpec = BbieSpec.CloneBcc(bcc, GetBdt(bccMapping));
                bbieSpec.Name = bccMapping.BIEName;
                yield return bbieSpec;
            }
        }

        private IEnumerable<BbieSpec> GenerateBbieSpecs(IEnumerable<SplitMapping> splitMappings, IAcc targetAcc)
        {
            foreach (var splitMapping in splitMappings)
            {
                foreach (IBcc bcc in splitMapping.TargetBccs)
                {
                    if (bcc.Acc.Id == targetAcc.Id)
                    {
                        var bbieSpec = BbieSpec.CloneBcc(bcc, GetBdt(splitMapping, bcc));
                        bbieSpec.Name = splitMapping.GetBbieName(bcc);
                        yield return bbieSpec;
                    }
                }
            }
        }

        private void GenerateRootABIE()
        {
            var rootElementMapping = schemaMapping.RootElementMapping;
            if (rootElementMapping is AsmaMapping)
            {
                AsmaMapping asmaMapping = (AsmaMapping) rootElementMapping;
                var ma = docLibrary.CreateMa(new MaSpec
                {
                    Name = qualifier + "_" + rootElementName,
                });
                AsmaSpec asmaSpec = new AsmaSpec
                                             {
                                                 Name = asmaMapping.SourceElementName,
                                             };
                if (asmaMapping.TargetMapping is ComplexTypeToAccMapping)
                {
                    asmaSpec.AssociatedBieAggregator =
                        new BieAggregator(bieLibrary.GetAbieByName(asmaMapping.TargetMapping.BIEName));
                }
                else
                {
                    asmaSpec.AssociatedBieAggregator =
                        new BieAggregator(docLibrary.GetMaByName(asmaMapping.TargetMapping.BIEName));
                }
                ma.CreateAsma(asmaSpec);
            }
            else if (rootElementMapping is AttributeOrSimpleElementOrComplexElementToBccMapping)
            {
                var bccMapping = (AttributeOrSimpleElementOrComplexElementToBccMapping)rootElementMapping;
                var abie = bieLibrary.CreateAbie(new AbieSpec
                {
                    BasedOn = bccMapping.Acc,
                    Name = qualifier + "_" + bccMapping.Acc.Name,
                    Bbies = new List<BbieSpec>(GenerateBbieSpecs(new List<AttributeOrSimpleElementOrComplexElementToBccMapping> { bccMapping })),
                });
                var ma = docLibrary.CreateMa(new MaSpec
                {
                    Name = qualifier + "_" + rootElementName,
                });
                ma.CreateAsma(new AsmaSpec
                {
                    Name = abie.Name,
                    AssociatedBieAggregator = new BieAggregator(abie),
                });
            }
            else
            {
                throw new MappingError("Root element can only be mapped to BCC, but is mapped to something else.");
            }
        }

    }

    internal class AsmaToGenerate
    {
        private readonly string name;
        private readonly string associatedBieName;
        private readonly IBieLibrary bieLibrary;
        private readonly IDocLibrary docLibrary;

        public AsmaToGenerate(IBieLibrary bieLibrary, string name, string associatedAbieName)
        {
            this.bieLibrary = bieLibrary;
            this.name = name;
            associatedBieName = associatedAbieName;
        }

        public AsmaToGenerate(IDocLibrary docLibrary, string name, string associatedMaName)
        {
            this.docLibrary = docLibrary;
            this.name = name;
            associatedBieName = associatedMaName;
        }

        internal AsmaSpec GenerateSpec()
        {
            AsmaSpec asmaSpec = new AsmaSpec
                                {
                                    Name = name,
                                    AssociatedBieAggregator = new BieAggregator(bieLibrary != null ? (object) bieLibrary.GetAbieByName(associatedBieName) : docLibrary.GetMaByName(associatedBieName)),
                                };
            return asmaSpec;
        }
    }

    internal class AsbieToGenerate
    {
        private readonly IBieLibrary bieLibrary;
        private readonly IAscc ascc;
        private readonly string asbieName;
        private readonly string associatedAbieName;

        public AsbieToGenerate(IBieLibrary bieLibrary, IAscc ascc, string asbieName, string associatedAbieName)
        {
            this.bieLibrary = bieLibrary;
            this.ascc = ascc;
            this.asbieName = asbieName;
            this.associatedAbieName = associatedAbieName;
        }

        internal AsbieSpec GenerateSpec()
        {
            return AsbieSpec.CloneAscc(ascc, asbieName, bieLibrary.GetAbieByName(associatedAbieName));
        }
    }
}