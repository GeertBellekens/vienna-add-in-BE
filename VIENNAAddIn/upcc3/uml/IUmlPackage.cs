using System.Collections;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.uml
{
    public interface IUmlPackage
    {
        int Id { get; }
        string Name { get; }
        string Stereotype { get; }
        string[] Stereotypes {get;}
        IEnumerable<IUmlPackage> Packages {get;}

        IUmlPackage Parent { get; }

        IEnumerable<IUmlClass> Classes { get; }
        IEnumerable<IUmlDataType> DataTypes { get; }
        IEnumerable<IUmlEnumeration> Enumerations { get; }

        IUmlTaggedValue GetTaggedValue(string name);

        IEnumerable<IUmlClass> GetClassesByStereotype(string stereotype);
        IUmlClass CreateClass(UmlClassSpec spec);
        IUmlClass UpdateClass(IUmlClass umlClass, UmlClassSpec spec);
        void RemoveClass(IUmlClass umlClass);

        IEnumerable<IUmlDataType> GetDataTypesByStereotype(string stereotype);
        IUmlDataType CreateDataType(UmlDataTypeSpec spec);
        IUmlDataType UpdateDataType(IUmlDataType dataType, UmlDataTypeSpec spec);
        void RemoveDataType(IUmlDataType dataType);

        IEnumerable<IUmlEnumeration> GetEnumerationsByStereotype(string stereotype);
        IUmlEnumeration CreateEnumeration(UmlEnumerationSpec spec);
        IUmlEnumeration UpdateEnumeration(IUmlEnumeration umlEnumeration, UmlEnumerationSpec spec);
        void RemoveEnumeration(IUmlEnumeration umlEnumeration);

        IEnumerable<IUmlPackage> GetPackagesByStereotype(string stereotype);
        IUmlPackage CreatePackage(UmlPackageSpec spec);
        IUmlPackage UpdatePackage(IUmlPackage umlPackage, UmlPackageSpec spec);
        void RemovePackage(IUmlPackage umlPackage);
    }
}