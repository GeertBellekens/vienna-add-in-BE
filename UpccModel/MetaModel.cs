using System;
using System.Collections.Generic;
using System.Linq;

namespace Upcc
{
    /// <summary>
    /// <para>This class is an implementation of the UPCC meta model based on UPCC 3.0 ODP 5 "Public Review" from 2009-10-15.</para>
    /// 
    /// <para><b>Deviations from UPCC:</b></para>
    /// 
    /// <para>
    /// We tried to implement this meta model as close to the original UPCC specification as possible (e.g. the UML meta classes are represented in the meta model code). However, we
    /// had to add some work-arounds due to the nature of certain parts of the specification. In particular, three areas are problematic:
    /// 
    /// <list type="bullet">
    /// 
    /// <item>
    /// <b>CON and SUP</b> attributes have different tagged values for CDT and BDT classes. Therefore, we had to define separate concepts, namely CDT-CON/CDT-SUP and BDT-CON/BDT-SUP for
    /// CDT and BDT, respectively. The major problem with this is that the meaning of CON and SUP stereotypes is context dependent. While this work-around works, a better solution
    /// would be to have separate stereotypes for the two concepts (e.g. CDT-CON/SUP and BDT-CON/SUP).
    /// </item>
    /// 
    /// <item>
    /// <b>PRIM, ENUM and IDSCHEME</b> are all candidate types for CON and SUP attributes. The conceptual view of the UPCC defines ENUM and IDSCHEME as sub-classes of PRIM, but
    /// this cannot really be implemented, because the three concepts are fundamentally different. Therefore, we introduce the concept of a (Meta-)MultiType, which is basically
    /// a container for an object of any of a set of possible types. The BasicType multi-type can be either a PRIM or an ENUM or an IDSCHEME. CON and SUP attributes declare the BasicType 
    /// multi-type as their type. The multi-type concept is more appropriate (and more easy to generate code with) than the previously attempted inheritance-based approach (where BasicType
    /// was a common abstract base class for PRIM, ENUM and IDSCHEME).
    /// </item>
    /// 
    /// <item>
    /// The associated element of an <b>ASMA</b> can be either an ABIE or a MA. This problem is similar to the PRIM/ENUM/IDSCHEME problem outlined above and also solved via the use
    /// of a multi-type (BieAggregator).
    /// </item>
    /// </list>
    /// </para>
    /// 
    /// <para><b>Implementation:</b></para>
    /// 
    /// <para>
    /// The meta model is implemented as a static repository/facade, based on a singleton instance of the meta model. There is also no interface 
    /// for the meta model. This implementation has been chosen for the following reasons:
    /// 
    /// <list type="bullet">
    /// <item>The meta model is statically defined data (we just happen to use C# for defining this data) and a change requires a re-build of the assembly.</item>
    /// <item>The data is never modified at runtime (i.e. by the templates that use the model for generating artifacts).</item>
    /// <item>There is no need to mock the meta model (thus no interface required).</item>
    /// <item>This implementation increases the generation speed of the templates, because the meta model is instantiated only once when the first template accesses the meta model.</item>
    /// <item>The facade allows us to change the implementation of the meta model at any time without affecting the templates (which are rather hard to refactor do to mediocre editor support).</item>
    /// <item>The facade is a straightforward place to add new getter methods for retrieving meta model elements.</item>
    /// </list>
    /// </para>
    /// </summary>
    public class MetaModel
    {
        private readonly MultiTypes multiTypes;
        private readonly Associations associations;
        private readonly Attributes attributes;
        private readonly Classes classes;
        private readonly DataTypes dataTypes;
        private readonly Dependencies dependencies;
        private readonly AttributeDependencies attributeDependencies;
        private readonly AssociationDependencies associationDependencies;
        private readonly EnumerationLiterals enumerationLiterals;
        private readonly Enumerations enumerations;
        private readonly PackageClassifierRelations packageClassifierRelations;
        private readonly PackageSubPackageRelations packageSubPackageRelations;
        private readonly Packages packages;
        private readonly TaggedValues taggedValues;

        static MetaModel()
        {
            Instance = new MetaModel();
        }

        private MetaModel()
        {
            taggedValues = new TaggedValues();

            packages = new Packages(taggedValues);

            enumerations = new Enumerations(taggedValues);
            dataTypes = new DataTypes(taggedValues);
            classes = new Classes(taggedValues);
            multiTypes = new MultiTypes(classes, dataTypes, enumerations);

            attributes = new Attributes(taggedValues, classes, multiTypes);
            enumerationLiterals = new EnumerationLiterals(taggedValues, enumerations);
            associations = new Associations(taggedValues, classes, multiTypes);
            dependencies = new Dependencies(classes, dataTypes, enumerations);
            attributeDependencies = new AttributeDependencies(attributes);
            associationDependencies = new AssociationDependencies(associations);

            packageSubPackageRelations = new PackageSubPackageRelations(packages);
            packageClassifierRelations = new PackageClassifierRelations(packages, dataTypes, classes, enumerations);
        }

        private static readonly MetaModel Instance;

        public static MetaPackage BLibrary
        {
            get { return Instance.packages.BLibrary; }
        }

        public static MetaPackage PrimLibrary
        {
            get { return Instance.packages.PrimLibrary; }
        }

        public static MetaPackage EnumLibrary
        {
            get { return Instance.packages.EnumLibrary; }
        }

        public static MetaPackage CdtLibrary
        {
            get { return Instance.packages.CdtLibrary; }
        }

        public static MetaPackage CcLibrary
        {
            get { return Instance.packages.CcLibrary; }
        }

        public static MetaPackage BdtLibrary
        {
            get { return Instance.packages.BdtLibrary; }
        }

        public static MetaPackage BieLibrary
        {
            get { return Instance.packages.BieLibrary; }
        }

        public static MetaPackage DocLibrary
        {
            get { return Instance.packages.DocLibrary; }
        }

        public static MetaMultiType BasicType
        {
            get { return Instance.multiTypes.BasicType; }
        }

        public static MetaMultiType BieAggregator
        {
            get { return Instance.multiTypes.BieAggregator; }
        }

        public static MetaDataType Prim
        {
            get { return Instance.dataTypes.Prim; }
        }

        public static MetaDataType IdScheme
        {
            get { return Instance.dataTypes.IdScheme; }
        }

        public static MetaEnumeration Enum
        {
            get { return Instance.enumerations.Enum; }
        }

        public static MetaClass Cdt
        {
            get { return Instance.classes.Cdt; }
        }

        public static MetaClass Bdt
        {
            get { return Instance.classes.Bdt; }
        }

        public static MetaClass Acc
        {
            get { return Instance.classes.Acc; }
        }

        public static MetaClass Abie
        {
            get { return Instance.classes.Abie; }
        }

        public static MetaClass Ma
        {
            get { return Instance.classes.Ma; }
        }

        public static MetaAttribute Bbie
        {
            get { return Instance.attributes.Bbie; }
        }

        public static MetaAttribute Bcc
        {
            get { return Instance.attributes.Bcc; }
        }

        public static MetaAttribute BdtCon
        {
            get { return Instance.attributes.BdtCon; }
        }

        public static MetaAttribute BdtSup
        {
            get { return Instance.attributes.BdtSup; }
        }

        public static MetaAttribute CdtCon
        {
            get { return Instance.attributes.CdtCon; }
        }

        public static MetaAttribute CdtSup
        {
            get { return Instance.attributes.CdtSup; }
        }

        public static MetaAssociation Asma
        {
            get { return Instance.associations.Asma; }
        }

        public static MetaAssociation Asbie
        {
            get { return Instance.associations.Asbie; }
        }

        public static MetaAssociation Ascc
        {
            get { return Instance.associations.Ascc; }
        }

        public static MetaEnumerationLiteral CodelistEntry
        {
            get { return Instance.enumerationLiterals.CodelistEntry; }
        }

        public static IEnumerable<MetaClass> GetAllClasses()
        {
            return Instance.classes.All;
        }

        public static IEnumerable<MetaMultiType> GetAllMultiTypes()
        {
            return Instance.multiTypes.All;
        }

        public static IEnumerable<MetaDataType> GetAllDataTypes()
        {
            return Instance.dataTypes.All;
        }

        public static IEnumerable<MetaEnumeration> GetAllEnumerations()
        {
            return Instance.enumerations.All;
        }

        public static IEnumerable<MetaClassifier> GetAllConcreteClassifiers()
        {
            foreach (var dataType in GetAllDataTypes())
            {
                yield return dataType;
            }
            foreach (var enumeration in GetAllEnumerations())
            {
                yield return enumeration;
            }
            foreach (var @class in GetAllClasses())
            {
                yield return @class;
            }
        }

        public static IEnumerable<MetaPackage> GetAllPackages()
        {
            return Instance.packages.All;
        }

        public static IEnumerable<MetaTaggedValue> GetAllTaggedValues()
        {
            return Instance.taggedValues.All;
        }

        /// <summary>
        /// Returns the package-to-package containment relations specifying the possible meta-sub-packages of the given meta package.
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public static IEnumerable<MetaSubPackageRelation> GetSubPackageRelationsFor(MetaPackage package)
        {
            foreach (MetaSubPackageRelation relation in Instance.packageSubPackageRelations.All)
            {
                if (relation.ParentPackageType == package)
                {
                    yield return relation;
                }
            }
        }

        public static bool HasSubPackages(MetaPackage package)
        {
            return GetSubPackageRelationsFor(package).Count() > 0;
        }

        /// <summary>
        /// Returns the package-to-package containment relations specifying the possible parent packages of the given meta package.
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public static IEnumerable<MetaSubPackageRelation> GetParentPackageRelationsFor(MetaPackage package)
        {
            foreach (MetaSubPackageRelation relation in Instance.packageSubPackageRelations.All)
            {
                if (relation.SubPackageType == package)
                {
                    yield return relation;
                }
            }
        }

        public static bool HasParentPackages(MetaPackage package)
        {
            return GetParentPackageRelationsFor(package).Count() > 0;
        }

        public static IEnumerable<MetaPackageClassifierRelation> GetClassifierRelationsFor(MetaPackage package)
        {
            foreach (MetaPackageClassifierRelation relation in Instance.packageClassifierRelations.All)
            {
                if (relation.PackageType == package)
                {
                    yield return relation;
                }
            }
        }

        public static bool HasClassifiers(MetaPackage package)
        {
            return GetClassifierRelationsFor(package).Count() > 0;
        }

        public static IEnumerable<MetaPackageClassifierRelation> GetPackageRelationsFor(MetaClassifier classifier)
        {
            foreach (MetaPackageClassifierRelation relation in Instance.packageClassifierRelations.All)
            {
                if (relation.ClassifierType == classifier)
                {
                    yield return relation;
                }
            }
        }

        public static bool HasPackages(MetaClassifier classifier)
        {
            return GetPackageRelationsFor(classifier).Count() > 0;
        }

        public static IEnumerable<MetaAttribute> GetAttributesFor(MetaClassifier classifier)
        {
            foreach (MetaAttribute attribute in Instance.attributes.All)
            {
                if (attribute.ContainingClassifierType == classifier)
                {
                    yield return attribute;
                }
            }
        }

        public static bool HasAttributes(MetaClassifier classifier)
        {
            return GetAttributesFor(classifier).Count() > 0;
        }

        public static MetaAttribute GetAttributeByName(string name)
        {
            foreach (MetaAttribute attribute in Instance.attributes.All)
            {
                if (attribute.AttributeName == name)
                {
                    return attribute;
                }
            }
            throw new Exception("Attribute '" + name + "' not defined.");
        }

        public static IEnumerable<MetaEnumerationLiteral> GetEnumerationLiteralsFor(MetaClassifier enumeration)
        {
            foreach (MetaEnumerationLiteral enumerationLiteral in Instance.enumerationLiterals.All)
            {
                if (enumerationLiteral.ContainingEnumerationType == enumeration)
                {
                    yield return enumerationLiteral;
                }
            }
        }

        public static bool HasEnumerationLiterals(MetaClassifier classifier)
        {
            return GetEnumerationLiteralsFor(classifier).Count() > 0;
        }

        public static IEnumerable<MetaAssociation> GetAssociationsForAssociatingClassifier(MetaClassifier classifier)
        {
            foreach (var relation in Instance.associations.All)
            {
                if (relation.AssociatingClassifierType == classifier)
                {
                    yield return relation;
                }
            }
        }

        public static bool IsAssociatingClassifier(MetaClassifier classifier)
        {
            return GetAssociationsForAssociatingClassifier(classifier).Count() > 0;
        }

        public static IEnumerable<MetaAssociation> GetAssociationsForAssociatedClassifier(MetaClassifier classifier)
        {
            foreach (var relation in Instance.associations.All)
            {
                if (relation.AssociatedClassifierType == classifier)
                {
                    yield return relation;
                }
            }
        }

        public static bool IsAssociatedClassifier(MetaClassifier classifier)
        {
            return GetAssociationsForAssociatedClassifier(classifier).Count() > 0;
        }

        public static IEnumerable<MetaDependency> GetDependenciesFor(MetaClassifier classifier)
        {
            foreach (MetaDependency dependency in Instance.dependencies.All)
            {
                if (dependency.SourceClassifierType == classifier)
                {
                    yield return dependency;
                }
            }
        }

        public static bool HasDependencies(MetaClassifier classifier)
        {
            return GetDependenciesFor(classifier).Count() > 0;
        }

        public static IEnumerable<MetaAttributeDependency> GetDependenciesFor(MetaAttribute attribute)
        {
            foreach (var dependency in Instance.attributeDependencies.All)
            {
                if (dependency.SourceAttribute == attribute)
                {
                    yield return dependency;
                }
            }
        }

        public static bool HasDependencies(MetaAttribute attribute)
        {
            return GetDependenciesFor(attribute).Count() > 0;
        }

        public static IEnumerable<MetaAssociationDependency> GetDependenciesFor(MetaAssociation association)
        {
            foreach (var dependency in Instance.associationDependencies.All)
            {
                if (dependency.SourceAssociation == association)
                {
                    yield return dependency;
                }
            }
        }

        public static bool HasDependencies(MetaAssociation association)
        {
            return GetDependenciesFor(association).Count() > 0;
        }
    }
}