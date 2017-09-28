using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.uml
{
    public interface IUmlClass : IUmlClassifier
    {
        
        IEnumerable<IUmlClass> GetClassesByStereotype(string stereotype);

    }
}