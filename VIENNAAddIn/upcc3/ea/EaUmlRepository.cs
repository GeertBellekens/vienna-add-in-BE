using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.uml;
using VIENNAAddInUtils;

namespace VIENNAAddIn.upcc3.ea
{
    public class EaUmlRepository : IUmlRepository
    {
        private readonly Repository eaRepository;

        public EaUmlRepository(Repository eaRepository)
        {
            this.eaRepository = eaRepository;
        }

        #region IUmlRepository Members

        public IEnumerable<IUmlPackage> GetPackagesByStereotype(params string[] stereotypes)
        {
            var stereotypeSet = new HashSet<string>(stereotypes);
            var eaPackages = new List<Package>();
            foreach (Package eaModel in eaRepository.Models)
            {
                EnumeratePackages(eaModel, eaPackages);
            }
            foreach (Package eaPackage in eaPackages)
            {
                if (eaPackage.Element != null && stereotypeSet.Contains(eaPackage.Element.Stereotype))
                {
                    yield return new EaUmlPackage(eaRepository, eaPackage);
                }
            }
        }

        public IUmlPackage GetPackageById(int id)
        {
            return new EaUmlPackage(eaRepository, eaRepository.GetPackageByID(id));
        }

        public IUmlPackage GetPackageByPath(Path path)
        {
            return new EaUmlPackage(eaRepository, eaRepository.Resolve<Package>(path));
        }

        public IUmlDataType GetDataTypeById(int id)
        {
            return new EaUmlClassifier(eaRepository, eaRepository.GetElementByID(id));
        }

        public IUmlDataType GetDataTypeByPath(Path path)
        {
            return new EaUmlClassifier(eaRepository, eaRepository.Resolve<Element>(path));
        }

        public IUmlEnumeration GetEnumerationById(int id)
        {
            return new EaUmlClassifier(eaRepository, eaRepository.GetElementByID(id));
        }

        public IUmlEnumeration GetEnumerationByPath(Path path)
        {
            return new EaUmlClassifier(eaRepository, eaRepository.Resolve<Element>(path));
        }

        public IUmlClass GetClassById(int id)
        {
            return new EaUmlClassifier(eaRepository, eaRepository.GetElementByID(id));
        }

        public IUmlClass GetClassByPath(Path path)
        {
            return new EaUmlClassifier(eaRepository, eaRepository.Resolve<Element>(path));
        }

        public IEnumerable<Path> GetRootLocations()
        {
            foreach (Package eaModel in eaRepository.Models)
            {
                yield return eaModel.Name;
                foreach (Package rootPackage in eaModel.Packages)
                {
                    if (rootPackage.Element.Stereotype == "bInformationV")
                    {
                        yield return (Path) eaModel.Name / rootPackage.Name;
                    }
                }
            }
        }

        public IUmlPackage CreateRootPackage(Path rootLocation, UmlPackageSpec spec)
        {
            var rootLocationPackage = eaRepository.Resolve<Package>(rootLocation);
            if (rootLocationPackage == null)
            {
                throw new ArgumentException("Invalid root location: " + rootLocation);
            }

            var eaPackage = (Package)rootLocationPackage.Packages.AddNew(spec.Name, string.Empty);
            eaPackage.Update();
            eaPackage.ParentID = rootLocationPackage.PackageID;

            var package = new EaUmlPackage(eaRepository, eaPackage);
            package.Initialize(spec);
            return package;
        }

        #endregion

        private static void EnumeratePackages(Package root, List<Package> packageList)
        {
            packageList.Add(root);
            foreach (Package package in root.Packages)
            {
                EnumeratePackages(package, packageList);
            }
        }
    }
}