using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.uml
{
    public interface IUmlClassifier
    {
        int Id { get; }
        string GUID { get; }
        string Name { get; }
        IUmlPackage Package { get; }
        string Stereotype { get; }
        string[] Stereotypes {get;}
        IUmlTaggedValue GetTaggedValue(string name);

        IEnumerable<IUmlDependency> GetDependenciesByStereotype(string stereotype);
        IUmlDependency GetFirstDependencyByStereotype(string stereotype);
        IUmlDependency CreateDependency(UmlDependencySpec spec);
    }
}