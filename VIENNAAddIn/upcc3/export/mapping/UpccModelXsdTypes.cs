using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.DocLibrary;

namespace VIENNAAddIn.upcc3.export.mapping
{
    public class UpccModelXsdTypes
    {
        private readonly Dictionary<string, XsdType> xsdTypes;
        private readonly HashSet<int> alreadyCompiledTypes;

        public UpccModelXsdTypes(IDocLibrary docLibrary)
        {
            xsdTypes = new Dictionary<string, XsdType>();
            alreadyCompiledTypes = new HashSet<int>();

            Compile(docLibrary);
        }

        public int Count
        {
            get { return xsdTypes.Count; }
        }

        public IEnumerable<XsdType> XsdTypes
        {
            get { return xsdTypes.Values; }
        }

        public bool ContainsXsdType(string xsdTypeName)
        {
            return xsdTypes.ContainsKey(xsdTypeName);
        }

        public int NumberOfChildren(string xsdTypeName)
        {
            XsdType xsdType;

            if (xsdTypes.TryGetValue(xsdTypeName, out xsdType))
            {
                if (xsdType != null)
                {
                    return xsdType.NumberOfChildren;    
                }                
            }

            return 0;
        }

        public bool XsdTypeContainsChild(string xsdTypeName, string childName)
        {
            XsdType xsdType;

            if (xsdTypes.TryGetValue(xsdTypeName, out xsdType))
            {
                if (xsdType != null)
                {
                    if (xsdType.HasChild(childName))
                    {
                        return true;
                    }
                }                
            }            
            
            return false;
        }
        
        
        private void Compile(IDocLibrary docLibrary)
        {
            IMa documentRoot = docLibrary.DocumentRoot;
            IAsma asma = documentRoot.Asmas.FirstOrDefault();
            BieAggregator bieAggregator = asma.AssociatedBieAggregator;

            if (bieAggregator.IsMa)
            {
                CompileXsdTypesFromMa(bieAggregator.Ma);    
            }
            else if (bieAggregator.IsAbie)
            {
                CompileXsdTypesFromAbie(bieAggregator.Abie);
            }
            else
            {
                throw new Exception("The ASMA " + asma.Name + " of the Document Root Element " + documentRoot.Name + " neither aggregates a MA nor an ABIE.");
            }            
        }

        private void CompileXsdTypesFromMa(IMa ma)
        {
            if (!alreadyCompiledTypes.Contains(ma.Id))
            {
                alreadyCompiledTypes.Add(ma.Id);

                XsdType xsdType = AddXsdType(ma.Name);

                foreach (IAsma asma in ma.Asmas)
                {
                    BieAggregator bieAggregator = asma.AssociatedBieAggregator;

                    if (bieAggregator.IsMa)
                    {
                        xsdType.AddChild(asma.Name);

                        CompileXsdTypesFromMa(bieAggregator.Ma);
                    }
                    else if (bieAggregator.IsAbie)
                    {
                        //if (bieAggregator.Name == "PaymentConditionsType_PaymentTerms")                            
                        //{
                        //    int y = 1;
                        //}
                        
                        // BUG FIX:
                        xsdType.AddChild(asma.Name);

                        // In case that trough the ASMA aggregated element is an ABIE, 
                        // then the ASMA is not an actual complex element of the MA's 
                        // complex type.
                        CompileXsdTypesFromAbie(bieAggregator.Abie);
                    }
                    else
                    {
                        throw new Exception("The ASMA " + asma.Name + " of the MA " + ma.Name +
                                            " neither aggregates a MA nor an ABIE.");
                    }
                }
            }
        }

        private void CompileXsdTypesFromAbie(IAbie abie)
        {
            if (!alreadyCompiledTypes.Contains(abie.Id))
            {
                alreadyCompiledTypes.Add(abie.Id);

                XsdType xsdType = AddXsdType(abie.Name);

                foreach (IBbie bbie in abie.Bbies)
                {
                    xsdType.AddChild(bbie.Name);

                    CompileXsdTypesFromBdt(bbie.Bdt);
                }

                foreach (IAsbie asbie in abie.Asbies)
                {
                    xsdType.AddChild(asbie.Name);

                    CompileXsdTypesFromAbie(asbie.AssociatedAbie);
                }
            }
        }

        private void CompileXsdTypesFromBdt(IBdt bdt)
        {
            if (!alreadyCompiledTypes.Contains(bdt.Id))
            {
                alreadyCompiledTypes.Add(bdt.Id);

                XsdType xsdType = AddXsdType(bdt.Name);

                foreach (IBdtSup sup in bdt.Sups)
                {
                    xsdType.AddChild(sup.Name);
                }
            }
        }

        private XsdType AddXsdType(string upccName)
        {
            string xsdName = XsdType.ExtractXsdName(upccName);

            XsdType xsdType;

            if (!(xsdTypes.TryGetValue(xsdName, out xsdType)))
            {
                xsdType = new XsdType(upccName);
                xsdTypes.Add(xsdType.XsdName, xsdType);                
            }

            return xsdType;
        }
    }

    public class XsdType
    {
        private readonly HashSet<string> children;

        public XsdType(string upccName)
        {
            XsdName = ExtractXsdName(upccName);
            children = new HashSet<string>();
        }

        public string XsdName { get; private set; }

        public void AddChild(string upccNameOfChild)
        {
            children.Add(ExtractXsdName(upccNameOfChild));
        }

        public int NumberOfChildren
        {
            get { return children.Count; }
        }

        public bool HasChild(string xsdNameOfChild)
        {
            if (children.Contains(xsdNameOfChild))
            {
                return true;
            }
            
            return false;
        }

        public static string ExtractXsdName(string upccName)
        {
            int indexOfUnderscore = upccName.IndexOf('_');

            if (indexOfUnderscore < 0)
            {
                return upccName;
            }

            return upccName.Substring(0, indexOfUnderscore);
        }
    }
}