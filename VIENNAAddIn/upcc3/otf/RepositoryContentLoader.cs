using System;
using System.Collections.Generic;
using EA;

namespace VIENNAAddIn.upcc3.otf
{
    public class RepositoryContentLoader
    {
        private readonly Repository eaRepository;
        private readonly Dictionary<int, int> packageIdsByPackageElementId = new Dictionary<int, int>();

        public RepositoryContentLoader(Repository eaRepository)
        {
            this.eaRepository = eaRepository;
        }

        public event Action<RepositoryItem> ItemLoaded;

        public void LoadRepositoryContent()
        {
            foreach (Package model in eaRepository.Models)
            {
                LoadPackageRecursively(model);
            }
        }

        private void LoadPackageRecursively(Package package)
        {
            LoadPackage(package);
            foreach (Package subPackage in package.Packages)
            {
                LoadPackageRecursively(subPackage);
            }
            foreach (Element element in package.Elements)
            {
                LoadElement(element);
            }
        }

        private void LoadElement(Element element)
        {
            var elementId = element.ElementID;
            int packageId;
            if (packageIdsByPackageElementId.TryGetValue(elementId, out packageId))
            {
                LoadPackageByID(packageId);
            }
            else
            {
                if (ItemLoaded != null)
                {
                    ItemLoaded(RepositoryItem.FromElement(element));
                }
            }
        }

        private void LoadPackage(Package package)
        {
            if (package.Element != null)
            {
                var packageElementId = package.Element.ElementID;
                packageIdsByPackageElementId[packageElementId] = package.PackageID;
            }
            if (ItemLoaded != null)
            {
                ItemLoaded(RepositoryItem.FromPackage(package));
            }
        }

        public void LoadPackageByID(int id)
        {
            LoadPackage(eaRepository.GetPackageByID(id));
        }

        public void LoadElementByID(int id)
        {
            LoadElement(eaRepository.GetElementByID(id));
        }

        private void LoadElementByGUID(string guid)
        {
            LoadElement(eaRepository.GetElementByGuid(guid));
        }

        private void LoadPackageByGUID(string guid)
        {
            LoadPackage(eaRepository.GetPackageByGuid(guid));
        }

        public void LoadItemByGUID(ObjectType objectType, string guid)
        {
            switch (objectType)
            {
                case ObjectType.otPackage:
                    LoadPackageByGUID(guid);
                    break;
                case ObjectType.otElement:
                    LoadElementByGUID(guid);
                    break;
            }
        }

        public void ItemDeleted(ItemId id)
        {
            if (id.Type == ItemId.ItemType.Package)
            {
                int packageElementId;
                if (FindPackageElementId(id, out packageElementId))
                {
                    packageIdsByPackageElementId.Remove(packageElementId);
                }
            }
        }

        private bool FindPackageElementId(ItemId id, out int packageElementId)
        {
            foreach (KeyValuePair<int, int> keyValuePair in packageIdsByPackageElementId)
            {
                if (keyValuePair.Value == id.Value)
                {
                    packageElementId = keyValuePair.Key;
                    return true;
                }
            }
            packageElementId = 0;
            return false;
        }
    }
}