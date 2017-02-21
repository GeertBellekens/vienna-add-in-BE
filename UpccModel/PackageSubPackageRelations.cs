using System.Collections.Generic;

namespace Upcc
{
    internal class PackageSubPackageRelations
    {
        internal PackageSubPackageRelations(Packages packages)
        {
            All = new[]
                  {
                      new MetaSubPackageRelation
                      {
                          ParentPackageCardinality = MetaCardinality.ZeroOrOne,
                          ParentPackageType = packages.BLibrary,
                          ParentPackageRole = "Parent",
                          SubPackageType = packages.BLibrary,
                          SubPackageRole = "BLibraries",
                      },
                      new MetaSubPackageRelation
                      {
                          ParentPackageCardinality = MetaCardinality.One,
                          ParentPackageType = packages.BLibrary,
                          ParentPackageRole = "BLibrary",
                          SubPackageType = packages.PrimLibrary,
                          SubPackageRole = "PrimLibraries",
                      },
                      new MetaSubPackageRelation
                      {
                          ParentPackageCardinality = MetaCardinality.One,
                          ParentPackageType = packages.BLibrary,
                          ParentPackageRole = "BLibrary",
                          SubPackageType = packages.EnumLibrary,
                          SubPackageRole = "EnumLibraries",
                      },
                      new MetaSubPackageRelation
                      {
                          ParentPackageCardinality = MetaCardinality.One,
                          ParentPackageType = packages.BLibrary,
                          ParentPackageRole = "BLibrary",
                          SubPackageType = packages.CdtLibrary,
                          SubPackageRole = "CdtLibraries",
                      },
                      new MetaSubPackageRelation
                      {
                          ParentPackageCardinality = MetaCardinality.One,
                          ParentPackageType = packages.BLibrary,
                          ParentPackageRole = "BLibrary",
                          SubPackageType = packages.CcLibrary,
                          SubPackageRole = "CcLibraries",
                      },
                      new MetaSubPackageRelation
                      {
                          ParentPackageCardinality = MetaCardinality.One,
                          ParentPackageType = packages.BLibrary,
                          ParentPackageRole = "BLibrary",
                          SubPackageType = packages.BdtLibrary,
                          SubPackageRole = "BdtLibraries",
                      },
                      new MetaSubPackageRelation
                      {
                          ParentPackageCardinality = MetaCardinality.One,
                          ParentPackageType = packages.BLibrary,
                          ParentPackageRole = "BLibrary",
                          SubPackageType = packages.BieLibrary,
                          SubPackageRole = "BieLibraries",
                      },
                      new MetaSubPackageRelation
                      {
                          ParentPackageCardinality = MetaCardinality.One,
                          ParentPackageType = packages.BLibrary,
                          ParentPackageRole = "BLibrary",
                          SubPackageType = packages.DocLibrary,
                          SubPackageRole = "DocLibraries",
                      },
                  };
        }

        internal IEnumerable<MetaSubPackageRelation> All { get; private set; }
    }
}