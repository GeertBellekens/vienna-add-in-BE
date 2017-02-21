using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.uml
{
    public interface IUmlClass : IUmlClassifier
    {
        IEnumerable<IUmlAttribute> GetAttributesByStereotype(string stereotype);
        IEnumerable<IUmlClass> GetClassesByStereotype(string stereotype);
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