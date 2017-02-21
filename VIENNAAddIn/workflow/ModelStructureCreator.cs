/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Windows.Forms;
using VIENNAAddIn.constants;

namespace VIENNAAddIn.WorkFlow
{
    /// <summary>
    /// Summary description for InitialModelStructureCreator.
    /// </summary>
    internal class ModelStructureCreator
    {
        private EA.Repository repository;

        internal ModelStructureCreator(EA.Repository repository)
        {
            this.repository = repository;
        }
        /// <summary>
        /// creates the initial structure of an UMM model
        /// </summary>
        internal void create(String modelName, String nameBRV, String nameBCV, String nameBIV, bool createBRV)
        {
            try
            { 
                
                EA.Collection currentModels = this.repository.Models;
                foreach (EA.Package p in currentModels)
                {
                    currentModels.DeleteAt(0, true);
                }
                // create a new model with the specified name
                EA.Package newModel = (EA.Package)currentModels.AddNew(modelName, "");
                newModel.Update();
                currentModels.Refresh();


                // a brv package is not mandatory.                  
                if (createBRV)
                {
                    EA.Package brv = (EA.Package)newModel.Packages.AddNew(nameBRV, "");
                    brv.Update();
                    brv = this.populateUMMView(brv, UMM.bRequirementsV);

                    //Add the three subviews
                    EA.Package bev = addViewPackage(brv, UMM.bEntityV, modelName, EA_Element.Class);
                    EA.Package bpv = addViewPackage(brv, UMM.bPartnerV, modelName, EA_Element.UseCase);
                    EA.Package bdv = addViewPackage(brv, UMM.bDomainV, modelName, EA_Element.Package);


                }


                // add the bcv package
                EA.Package bcv = (EA.Package)newModel.Packages.AddNew(nameBCV, "");
                bcv.Update();
                bcv = this.populateUMMView(bcv, UMM.bChoreographyV);

                //Add the three subviews
                EA.Package brealv = addViewPackage(bcv, UMM.bRealizationV, modelName, EA_Element.UseCase);
                EA.Package bcollv = addViewPackage(bcv, UMM.bCollaborationV, modelName, EA_Element.UseCase);
                EA.Package btv = addViewPackage(bcv, UMM.bTransactionV, modelName, EA_Element.UseCase);



                // add the biv package
                EA.Package biv = (EA.Package)newModel.Packages.AddNew(nameBIV, "");
                biv.Update();
                biv = this.populateUMMView(biv, UMM.bInformationV);
                         


                // refresh the complete model treeview
                repository.RefreshModelView(0);


            }
            // if an error occurs, clean up the model to avoid an undefined state
            catch (Exception e)
            {
                EA.Collection currentModels = this.repository.Models;
                for (short i = 0; i < currentModels.Count; i++)
                {
                    currentModels.Delete(i);
                }
                repository.RefreshModelView(0);
                throw new Exception(e.Message, e);
            }
        }
        /// <summary>
        /// Sets a normal EA.Package as an UMM view package. This means, that a stereotype is 
        /// applied and also the taggedvalues, which every "UMM view package" inherits
        /// from "BusinessLibrary" from UMM base. Additionally a package diagram is created if 
        /// the package is stereotyped as BDV, BRV or BTV 
        /// </summary>
        /// <param name="package">the EA.Package which should be modified to an UMM "view package"</param>
        /// <param name="stereotype">the stereotype, which should be applied</param>
        /// <returns>a reference to the modified package</returns>
        private EA.Package populateUMMView(EA.Package package, UMM stereotype)
        {
            // set the stereotype 
            package.Element.Stereotype = stereotype.ToString();
            // add the tagged values
            foreach (string tv_string in Enum.GetNames(typeof(BusinessLibraryTV)))
            {
                EA.TaggedValue tv = (EA.TaggedValue)package.Element.TaggedValues.AddNew(tv_string, "");
                tv.Update();
            }
            /* if the stereotype is BRV or BCV the package is own of the core views. 
             * then for a more convinient modeling, a package diagram is added */
            if (stereotype.Equals(UMM.bRequirementsV) || stereotype.Equals(UMM.bChoreographyV))
            {
                EA.Diagram diagram = (EA.Diagram)package.Diagrams.AddNew(package.Name, EA_Element.Package.ToString());
                diagram.ShowDetails = 0;
                diagram.Update();
            }
            //For the BIV we add a class diagram
            else if (stereotype.Equals(UMM.bInformationV))
            {
                EA.Diagram diagram = (EA.Diagram)package.Diagrams.AddNew(package.Name, EA_Element.Class.ToString());
                diagram.ShowDetails = 0;
                diagram.Update();
            }

            package.Update();            
            return package;
        }




        /// <summary>
        /// adds a package with the given Stereotype as a Subpackage to the given package
        /// </summary>
        /// <param name="package">The package, which is the parent of the newly created package</param>
        /// <param name="cStereotype">The stereotype of which the newly created package should be type of</param>
        /// <param name="name">The name of the package to add</param>
        /// 
        internal EA.Package addViewPackage(EA.Package package, UMM cStereotype, String name, EA_Element diagramType)
        {
            // at first add the new package to the parent package
            EA.Package childPackage = (EA.Package)package.Packages.AddNew(name, "");
            childPackage.Update();
            childPackage = this.populateUMMView(childPackage, cStereotype);
            /* also add the child package to the package diagram contained in the parent
             * package, if wanted. if the package diagram doenst exist than create it */


            EA.Diagram packageDiagram = (EA.Diagram)childPackage.Diagrams.AddNew(childPackage.Name, diagramType.ToString());
            packageDiagram.ShowDetails = 0;
            packageDiagram.Update();
   

            /* now add the child package to the package diagram of the parent */
            EA.Collection diagramObjects = packageDiagram.DiagramObjects;
            EA.DiagramObject diagramObject = (EA.DiagramObject)diagramObjects.AddNew("l=200;r=400;t=200;b=600;", "");
            diagramObject.ElementID = childPackage.Element.ElementID;
            diagramObject.Update();
            
            repository.RefreshModelView(package.PackageID);
            return childPackage;
        }


        ///// <summary>
        ///// adds a BusinessChoreography to the BusinessChoreographyView
        ///// or a BusinessInteraction to the BusinessInteractionView
        ///// </summary>
        ///// <param name="parentPackage">the parent package where the new class should be a child of
        ///// (may be a BusinessChoreographyView or a BusinessInteractionView)</param>
        //internal BTAInteractioAndDiagram addPersistentBTVClassAndActivityGraph(EA.Package parentPackage)
        //{

        //    String parentPackageStereotype = parentPackage.Element.Stereotype;
        //    String classStereotype = "";
        //    String diagramStereotype = "";
        //    if (parentPackageStereotype.Equals(UMM_Stereotype.BusinessChoreographyView.ToString()))
        //    {
        //        classStereotype = UMM_Stereotype.BusinessChoreography.ToString();
        //        diagramStereotype = UMM_Stereotype.BusinessCollaborationProtocol.ToString();
        //    }
        //    else if (parentPackageStereotype.Equals(UMM_Stereotype.BusinessInteractionView.ToString()))
        //    {
        //        classStereotype = UMM_Stereotype.BusinessInteraction.ToString();
        //        diagramStereotype = UMM_Stereotype.BusinessTransaction.ToString();
        //    }
        //    EA.Element newPersistentClass = (EA.Element)parentPackage.Elements.AddNew(parentPackage.Name, EA_Element.Class.ToString());
        //    newPersistentClass.Stereotype = classStereotype;
        //    //kristina add
        //    if (parentPackageStereotype.Equals(UMM_Stereotype.BusinessInteractionView.ToString()))
        //    {
        //        foreach (string tv_string in Enum.GetNames(typeof(BusinessInteractionTV)))
        //        {
        //            EA.TaggedValue tv = (EA.TaggedValue)newPersistentClass.TaggedValues.AddNew(tv_string, "");
        //            tv.Update();
        //        }
        //    }
        //    //end Kristina add

        //    newPersistentClass.Update();
        //    /* add the corresponding diagram that describes the behavior of the class */
        //    EA.Diagram subdiagram = (EA.Diagram)newPersistentClass.Diagrams.AddNew(newPersistentClass.Name, "Activity");
        //    subdiagram.Stereotype = diagramStereotype;
        //    // dont show the diagram details
        //    subdiagram.ShowDetails = 0;
        //    subdiagram.Update();


        //    BTAInteractioAndDiagram workAround = new BTAInteractioAndDiagram(newPersistentClass, subdiagram);
        //    repository.RefreshModelView(parentPackage.PackageID);
        //    return workAround;
        //}
    }
}
