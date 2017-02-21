using System.Collections.Generic;
using VIENNAAddInUtils;

namespace VIENNAAddIn.upcc3.uml
{
    public interface IUmlRepository
    {
        IEnumerable<IUmlPackage> GetPackagesByStereotype(params string[] stereotypes);
        IUmlPackage GetPackageById(int id);
        IUmlPackage GetPackageByPath(Path path);

        IUmlDataType GetDataTypeById(int id);
        IUmlDataType GetDataTypeByPath(Path path);

        IUmlEnumeration GetEnumerationById(int id);
        IUmlEnumeration GetEnumerationByPath(Path path);

        IUmlClass GetClassById(int id);
        IUmlClass GetClassByPath(Path path);

        IEnumerable<Path> GetRootLocations();
        IUmlPackage CreateRootPackage(Path rootLocation, UmlPackageSpec spec);
    }
}