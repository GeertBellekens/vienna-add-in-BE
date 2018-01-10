using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using EA;
using VIENNAAddInUtils;

namespace VIENNAAddIn
{
    public static class EARepositoryExtensions
    {
        public static Package AddModel(this Repository repository, string name, Action<Package> initModel)
        {
            var model = (Package) repository.Models.AddNew(name, "Package");
            model.Update();
            initModel(model);
            model.Update();
            return model;
        }

        public static T Resolve<T>(this Repository repository, Path path) where T : class
        {
            if (path.Length == 0)
            {
                return default(T);
            }
            foreach (Package model in repository.Models)
            {
                if (model.Name == path.FirstPart)
                {
                    return Resolve<T>(model, path.Rest);
                }
            }
            return default(T);
        }

        private static T Resolve<T>(Package package, Path path) where T : class
        {
            if (package == null)
            {
                return default(T);
            }
            if (path.Length == 0)
            {
                return package as T;
            }
            string firstPart = path.FirstPart;
            if (path.Length == 1)
            {
                // this is the last element
                if (typeof (T) == typeof (Package))
                {
                    return package.PackageByName(firstPart) as T;
                }
                if (typeof (T) == typeof (Element))
                {
                    return package.ElementByName(firstPart) as T;
                }
                var o = package.PackageByName(firstPart) as T;
                if (o != default(T))
                {
                    return o;
                }
                return package.ElementByName(firstPart) as T;
            }
            return Resolve<T>(package.PackageByName(firstPart), path.Rest);
        }

        private static Package PackageByName(this Package package, string name)
        {
            foreach (Package child in package.Packages)
            {
                if (child.Name == name)
                {
                    return child;
                }
            }
            return null;
        }

        private static Element ElementByName(this Package package, string name)
        {
            foreach (Element element in package.Elements)
            {
                if (element.Name == name)
                {
                    return element;
                }
            }
            return null;
        }

        internal static string DetermineScope(this Repository repository)
        {
            Object obj;
            switch (repository.GetTreeSelectedItem(out obj))
            {
                case ObjectType.otPackage:
                    return ((Package) obj).PackageID.ToString();
                case ObjectType.otDiagram:
                    return ((Diagram) obj).PackageID.ToString();
                case ObjectType.otElement:
                    return ((Element) obj).PackageID.ToString();
                default:
                    return string.Empty;
            }
        }

        internal static int DetermineDiagramID(this Repository repository)
        {
            Object obj;
            repository.GetTreeSelectedItem(out obj);
            return ((Diagram) obj).DiagramID;
        }

        internal static bool IsUmm2Model(this Repository repository)
        {
            return repository != null;
        }

        /// <summary>
        /// defines a normal EA model as an UMM2 model. An "Issue" is added to the repository object,
        /// which marks the model permanently until the user chooses to revert the setting. Also
        /// an MDG file is loaded, which contains the UMM2 Profile and the UMM2 Standard Transaction
        /// Patterns
        /// </summary>
        /// <param name="repository">the model, which should be marked as UMM2 model</param>
        /// <returns>true if the model can be successfully marked and the relevant 
        /// MDG file (Profiles, Patterns) can be loaded successfully</returns>
        private static void SetAsUMM2Model(this Repository repository)
        {
            var pIssues = (ProjectIssues) repository.Issues.AddNew("UMM2Model", "Issue");
            pIssues.Update();
            repository.Issues.Refresh();

            try
            {
                MessageBox.Show("This Model is now defined as an UMM2/UPCC3 Model", "AddIn");
            }
            catch (Exception e)
            {
                MessageBox.Show("The following exception occured while loading the MDG File: " + e.Message,
                                "AddIn Error");
                repository.UnSetAsUMM2Model();
            }
        }

        /// <summary>
        /// Unmark an EA Model, which has previously defined as an UMM2 Model. This operation
        /// also unloads the MDG technology file, which contains the UMM2 Profile and the UMM2
        /// Standard transaction patterns
        /// </summary>
        /// <param name="repository">the model which shouldnt be marked as UMM2 Model any longer</param>
        /// <returns>true, if the model can be successfully unmarked</returns>
        private static void UnSetAsUMM2Model(this Repository repository)
        {
            Collection pIssues = repository.Issues;
            for (short i = 0; i < pIssues.Count; i++)
            {
                var pIssue = (ProjectIssues) pIssues.GetAt(i);
                if (pIssue.Name.Equals("UMM2Model"))
                {
                    pIssues.DeleteAt(i, true);
                    MessageBox.Show("Model is not defined as an UMM2/UPCC3 Model any longer", "AddIn");
                    break;
                }
            }
        }

        internal static void ToggleUmm2ModelState(this Repository repository)
        {
            try
            {
                if (repository.IsUmm2Model())
                {
                    repository.UnSetAsUMM2Model();
                }
                else
                {
                    repository.SetAsUMM2Model();
                }
            }
            catch (COMException)
            {
                MessageBox.Show("Please open a model first", "AddIn Error");
            }
        }
    }
}