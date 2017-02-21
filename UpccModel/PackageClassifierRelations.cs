using System.Collections.Generic;

namespace Upcc
{
    internal class PackageClassifierRelations
    {
        internal PackageClassifierRelations(Packages packages, DataTypes dataTypes, Classes classes, Enumerations enumerations)
        {
            All = new[]
                  {
                      new MetaPackageClassifierRelation
                      {
                          PackageType = packages.PrimLibrary,
                          PackageRole = "PrimLibrary",
                          ClassifierCardinality = MetaCardinality.Many,
                          ClassifierType = dataTypes.Prim,
                          ClassifierRole = "Prims",
                      },
                      new MetaPackageClassifierRelation
                      {
                          PackageType = packages.EnumLibrary,
                          PackageRole = "EnumLibrary",
                          ClassifierCardinality = MetaCardinality.Many,
                          ClassifierType = enumerations.Enum,
                          ClassifierRole = "Enums",
                      },
                      new MetaPackageClassifierRelation
                      {
                          PackageType = packages.EnumLibrary,
                          PackageRole = "EnumLibrary",
                          ClassifierCardinality = MetaCardinality.Many,
                          ClassifierType = dataTypes.IdScheme,
                          ClassifierRole = "IdSchemes",
                      },
                      new MetaPackageClassifierRelation
                      {
                          PackageType = packages.CdtLibrary,
                          PackageRole = "CdtLibrary",
                          ClassifierCardinality = MetaCardinality.Many,
                          ClassifierType = classes.Cdt,
                          ClassifierRole = "Cdts",
                      },
                      new MetaPackageClassifierRelation
                      {
                          PackageType = packages.CcLibrary,
                          PackageRole = "CcLibrary",
                          ClassifierCardinality = MetaCardinality.Many,
                          ClassifierType = classes.Acc,
                          ClassifierRole = "Accs",
                      },
                      new MetaPackageClassifierRelation
                      {
                          PackageType = packages.BdtLibrary,
                          PackageRole = "BdtLibrary",
                          ClassifierCardinality = MetaCardinality.Many,
                          ClassifierType = classes.Bdt,
                          ClassifierRole = "Bdts",
                      },
                      new MetaPackageClassifierRelation
                      {
                          PackageType = packages.BieLibrary,
                          PackageRole = "BieLibrary",
                          ClassifierCardinality = MetaCardinality.Many,
                          ClassifierType = classes.Abie,
                          ClassifierRole = "Abies",
                      },
                      new MetaPackageClassifierRelation
                      {
                          PackageType = packages.DocLibrary,
                          PackageRole = "DocLibrary",
                          ClassifierCardinality = MetaCardinality.Many,
                          ClassifierType = classes.Ma,
                          ClassifierRole = "Mas",
                      },
                  };
        }

        internal IEnumerable<MetaPackageClassifierRelation> All { get; private set; }
    }
}