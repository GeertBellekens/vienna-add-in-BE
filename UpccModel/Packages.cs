using System.Collections.Generic;
using System.Reflection;

namespace Upcc
{
    internal class Packages
    {
        internal readonly MetaPackage BdtLibrary;
        internal readonly MetaPackage BieLibrary;
        internal readonly MetaPackage BLibrary;
        internal readonly MetaPackage CcLibrary;
        internal readonly MetaPackage CdtLibrary;
        internal readonly MetaPackage DocLibrary;
        internal readonly MetaPackage EnumLibrary;
        internal readonly MetaPackage PrimLibrary;

        internal Packages(TaggedValues taggedValues)
        {
            var bLibraryTaggedValues = new[]
                                       {
                                           taggedValues.BusinessTerm,
                                           taggedValues.Copyright,
                                           taggedValues.Owner,
                                           taggedValues.Reference,
                                           taggedValues.Status,
                                           taggedValues.UniqueIdentifier,
                                           taggedValues.VersionIdentifier,
                                       };

            MetaTaggedValue[] elementLibraryTaggedValues = new List<MetaTaggedValue>(bLibraryTaggedValues)
                                                           {
                                                               taggedValues.BaseUrn,
                                                               taggedValues.NamespacePrefix,
                                                           }.ToArray();

            PrimLibrary = new MetaPackage
                          {
                              Name = "PrimLibrary",
                              Stereotype = MetaStereotype.PRIMLibrary,
                              TaggedValues = elementLibraryTaggedValues,
                          };

            EnumLibrary = new MetaPackage
                          {
                              Name = "EnumLibrary",
                              Stereotype = MetaStereotype.ENUMLibrary,
                              TaggedValues = elementLibraryTaggedValues,
                          };

            CdtLibrary = new MetaPackage
                         {
                             Name = "CdtLibrary",
                             Stereotype = MetaStereotype.CDTLibrary,
                             TaggedValues = elementLibraryTaggedValues,
                         };

            CcLibrary = new MetaPackage
                        {
                            Name = "CcLibrary",
                            Stereotype = MetaStereotype.CCLibrary,
                            TaggedValues = elementLibraryTaggedValues,
                        };

            BdtLibrary = new MetaPackage
                         {
                             Name = "BdtLibrary",
                             Stereotype = MetaStereotype.BDTLibrary,
                             TaggedValues = elementLibraryTaggedValues,
                         };

            BieLibrary = new MetaPackage
                         {
                             Name = "BieLibrary",
                             Stereotype = MetaStereotype.BIELibrary,
                             TaggedValues = elementLibraryTaggedValues,
                         };

            DocLibrary = new MetaPackage
                         {
                             Name = "DocLibrary",
                             Stereotype = MetaStereotype.DOCLibrary,
                             TaggedValues = elementLibraryTaggedValues,
                         };

            BLibrary = new MetaPackage
                       {
                           Name = "BLibrary",
                           Stereotype = MetaStereotype.bLibrary,
                           TaggedValues = bLibraryTaggedValues,
                       };
        }

        internal IEnumerable<MetaPackage> All
        {
            get
            {
                foreach (FieldInfo field in GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
                {
                    yield return (MetaPackage) field.GetValue(this);
                }
            }
        }
    }
}