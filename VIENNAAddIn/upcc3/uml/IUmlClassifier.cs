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
        IEnumerable<IUmlAttribute> Attributes{get;}
        
        IUmlTaggedValue GetTaggedValue(string name);
        

        IEnumerable<IUmlDependency> GetDependenciesByStereotype(string stereotype);
        IUmlDependency GetFirstDependencyByStereotype(string stereotype);
        IUmlDependency CreateDependency(UmlDependencySpec spec);
        
        IEnumerable<IUmlAttribute> GetAttributesByStereotype(string stereotype);
        
        IUmlAttribute GetFirstAttributeByStereotype(string stereotype);
        IUmlAttribute CreateAttribute(UmlAttributeSpec spec);
        IUmlAttribute UpdateAttribute(IUmlAttribute attribute, UmlAttributeSpec spec);
        void RemoveAttribute(IUmlAttribute attribute);

        IEnumerable<IUmlAssociation> GetAssociationsByStereotype(string stereotype);
        IUmlAssociation CreateAssociation(UmlAssociationSpec spec);
        IUmlAssociation UpdateAssociation(IUmlAssociation association, UmlAssociationSpec spec);
        void RemoveAssociation(IUmlAssociation association);
    }
}