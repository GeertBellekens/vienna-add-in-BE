/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections.Generic;
using VIENNAAddIn.common;
using VIENNAAddIn.constants;

namespace VIENNAAddIn.validator.umm.brv
{
    internal class bEntityVValidator : AbstractValidator
    {
        /// <summary>
        /// Validate the BusinessEntityView
        /// </summary>
        internal override void validate(IValidationContext context, string scope)
        {
            EA.Package bev = context.Repository.GetPackageByID(Int32.Parse(scope));
            checkTV_BusinessEntityView(context, bev);
            checkC23(context, bev);
            checkC24(context, bev);
            checkC25(context, bev);
            checkC26(context, bev);
            checkC27(context, bev);
            checkC28(context, bev);
            checkC29(context, bev);
        }


        /// <summary>
        /// Check constraint C23
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bev"></param>
        private void checkC23(IValidationContext context, EA.Package bev)
        {
            //Iterate recursively through the element of the business entity view and search for business entities,
            //since we have to consider the possibility, that users structure their entities in subfolders
            IList<EA.Element> elements = Utility.getAllElements(bev, new List<EA.Element>(), UMM.bEntity.ToString());

            //A BusinessEntityView must contain one to many business entities
            if (elements.Count < 1)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C23.",
                                                                   "A BusinessEntityView MUST contain one to many BusinessEntities. \n\nThe BusinessEntityView " +
                                                                   bev.Name + " does not contain any BusinessEntities.", "BRV",
                                                                   ValidationMessage.errorLevelTypes.ERROR, bev.PackageID));
            }
            else
            {
                context.AddValidationMessage(new ValidationMessage("Info to constraint C23.",
                                                                   "A BusinessEntityView MUST contain one to many BusinessEntities. \n\nThe BusinessEntitiyView " +
                                                                   bev.Name + " contains " + elements.Count + " BusinessEntities.",
                                                                   "BRV", ValidationMessage.errorLevelTypes.INFO, bev.PackageID));
            }
        }


        /// <summary>
        /// Check constraint C24
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bev"></param>
        private void checkC24(IValidationContext context, EA.Package bev)
        {
            //Iterate recursively through the element of the business entity view and search for business entities,
            //since we have to consider the possibility, that users structure their entities in subfolders
            IList<EA.Element> elements = Utility.getAllElements(bev, new List<EA.Element>(), UMM.bEntity.ToString());

            //Generate an info message for every business entity that is described by a UML State Diagram
            foreach (EA.Element businessEntity in elements)
            {
                int countStateDiagrams = 0;
                bool invalidDiagramDetected = false;

                foreach (EA.Diagram diagram in businessEntity.Diagrams)
                {
                    if (diagram.Type == "Statechart")
                    {
                        countStateDiagrams++;
                    }
                    else
                    {
                        //Invalid diagram detected
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C24.",
                                                                           "A BusinessEntity SHOULD have zero to one UML State Diagram that describe its lifecycle. \n\nInvalid diagram type detected under business entity " +
                                                                           businessEntity.Name + ".", "BRV",
                                                                           ValidationMessage.errorLevelTypes.ERROR, bev.PackageID));
                        invalidDiagramDetected = true;
                        break;
                    }
                }

                //In case no invalid diagram was detected check how often a State diagram occurred under a
                //given business entity
                if (!invalidDiagramDetected)
                {
                    //Error - more than one state diagram detected
                    if (countStateDiagrams > 1)
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C24.",
                                                                           "A BusinessEntity SHOULD have zero to one UML State Diagram that describe its lifecycle. \n\nInvalid number of UML state diagrams detected under business entity " +
                                                                           businessEntity.Name + ".", "BRV",
                                                                           ValidationMessage.errorLevelTypes.ERROR, bev.PackageID));
                    }
                        //No UML State Diagram detected - show a warning message
                    else if (countStateDiagrams == 0)
                    {
                        context.AddValidationMessage(new ValidationMessage("Warning for constraint C24.",
                                                                           "A BusinessEntity SHOULD have zero to one UML State Diagram that describe its lifecycle. \n\nBusiness entity " +
                                                                           businessEntity.Name +
                                                                           " is not described by a UML State Diagram.", "BRV",
                                                                           ValidationMessage.errorLevelTypes.WARN, bev.PackageID));
                    }
                        //No UML State Diagram detected - show a warning message
                    else
                    {
                        context.AddValidationMessage(new ValidationMessage("Info for constraint C24.",
                                                                           "A BusinessEntity SHOULD have zero to one UML State Diagram that describe its lifecycle. \n\nBusiness entity " +
                                                                           businessEntity.Name +
                                                                           " is described by a UML State Diagram.", "BRV",
                                                                           ValidationMessage.errorLevelTypes.INFO, bev.PackageID));
                    }
                }
            }
        }


        /// <summary>
        /// Check constraint C25
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bev"></param>
        private void checkC25(IValidationContext context, EA.Package bev)
        {
            //Iterate recursively through the element of the business entity view and search for business entities,
            //since we have to consider the possibility, that users structure their entities in subfolders
            IList<EA.Element> elements = Utility.getAllElements(bev, new List<EA.Element>(), UMM.bEntity.ToString());

            //Generate an info message for every business entity that is described by a UML State Diagram
            foreach (EA.Element businessEntity in elements)
            {
                //Is this BusinessEntity refined by a diagram?
                int countDiagram = businessEntity.Diagrams.Count;

                //This constraint only applies if there are UML state diagrams describing the lifecycle of a 
                //business entity
                if (countDiagram > 0)
                {
                    int countEntityStates = 0;
                    //There must be at least one business entity state 
                    foreach (EA.Element subelement in businessEntity.Elements)
                    {
                        if (subelement.Stereotype == UMM.bEState.ToString())
                        {
                            countEntityStates++;
                        }
                    }


                    //Raise an error if no entity state is found
                    if (countEntityStates < 1)
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C25.",
                                                                           "A UML State Diagram describing the lifecycle of a BusinessEntity MUST contain one to many BusinessEntityStates. \n\nThe state diagram underneath the business entity " +
                                                                           businessEntity.Name +
                                                                           " does not contain BusinessEntityStates.", "BRV",
                                                                           ValidationMessage.errorLevelTypes.ERROR, bev.PackageID));
                    }
                    else
                    {
                        context.AddValidationMessage(new ValidationMessage("Info for constraint C25.",
                                                                           "A UML State Diagram describing the lifecycle of a BusinessEntity MUST contain one to many BusinessEntityStates. \n\nThe state diagram underneath the business entity " +
                                                                           businessEntity.Name + " has " + countEntityStates +
                                                                           " BusinessEntityStates.", "BRV",
                                                                           ValidationMessage.errorLevelTypes.INFO, bev.PackageID));
                    }
                }
            }
        }


        /// <summary>
        /// Check constraint C26
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bev"></param>
        private void checkC26(IValidationContext context, EA.Package bev)
        {
            //Get all business data views
            IList<EA.Package> businessDataViews = Utility.getAllSubPackagesWithGivenStereotypeRecursively(bev,
                                                                                                          new List
                                                                                                              <
                                                                                                              EA.Package
                                                                                                              >(),
                                                                                                          UMM.bDataV.
                                                                                                              ToString());

            //Get all subpackage of the the BusinessEntityView
            IList<EA.Package> allSubpackagesofBEV = Utility.getAllSubPackagesRecursively(bev, new List<EA.Package>());

            //Both counts must be the same, otherwise wrong packages have been found
            if (businessDataViews.Count != allSubpackagesofBEV.Count)
            {
                String wrongPackages = "";
                foreach (EA.Package wrongpackage in allSubpackagesofBEV)
                {
                    if (Utility.getStereoTypeFromPackage(wrongpackage) != UMM.bDataV.ToString())
                    {
                        wrongPackages += " : " + wrongpackage.Name;
                    }
                }

                context.AddValidationMessage(new ValidationMessage("Violation of constraint C26.",
                                                                   "A BusinessEntityView MAY contain zero to many BusinessDataView that describe its conceptual design. \n\nThe following invalid packages have been found: " +
                                                                   wrongPackages, "BRV", ValidationMessage.errorLevelTypes.ERROR,
                                                                   bev.PackageID));
            }
        }


        /// <summary>
        /// Check constraint C27
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bev"></param>
        private void checkC27(IValidationContext context, EA.Package bev)
        {
            //Get all business data views
            IList<EA.Package> businessDataViews = Utility.getAllSubPackagesWithGivenStereotypeRecursively(bev,
                                                                                                          new List
                                                                                                              <
                                                                                                              EA.Package
                                                                                                              >(),
                                                                                                          UMM.bDataV.
                                                                                                              ToString());


            //The parent package of every business data view must be a business entity view
            foreach (EA.Package businessDataView in businessDataViews)
            {
                EA.Package parent = context.Repository.GetPackageByID(businessDataView.ParentID);
                if (Utility.getStereoTypeFromPackage(parent) != UMM.bEntityV.ToString())
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C27.",
                                                                       "The parent of a BusinessDataView MUST be a BusinessEntityView. \n\nBusinessDataView " +
                                                                       businessDataView.Name +
                                                                       " does not have a BusinessEntityView as parent.", "BRV",
                                                                       ValidationMessage.errorLevelTypes.ERROR, bev.PackageID));
                }
            }
        }


        /// <summary>
        /// Check constraint C28
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bev"></param>
        private void checkC28(IValidationContext context, EA.Package bev)
        {
            //Get all business data views
            IList<EA.Package> businessDataViews = Utility.getAllSubPackagesWithGivenStereotypeRecursively(bev,
                                                                                                          new List
                                                                                                              <
                                                                                                              EA.Package
                                                                                                              >(),
                                                                                                          UMM.bDataV.
                                                                                                              ToString());


            //A BusinessDataView should use a UML Class Diagram
            foreach (EA.Package businessDataView in businessDataViews)
            {
                foreach (EA.Diagram diagram in businessDataView.Diagrams)
                {
                    if (diagram.Type != "Logical")
                    {
                        context.AddValidationMessage(new ValidationMessage("Warning for constraint C28.",
                                                                           "A BusinessDataView SHOULD use a UML Class Diagram to describe the conceptual design of a BusinessEntity. \n\nThe business data view " +
                                                                           businessDataView.Name +
                                                                           " uses a diagram type which is not recommended (" +
                                                                           diagram.Type + ").", "BRV",
                                                                           ValidationMessage.errorLevelTypes.WARN, bev.PackageID));
                    }
                }
            }
        }


        /// <summary>
        /// Check constraint C29
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bev"></param>
        private void checkC29(IValidationContext context, EA.Package bev)
        {
            //Get all business data views
            IList<EA.Package> businessDataViews = Utility.getAllSubPackagesWithGivenStereotypeRecursively(bev,
                                                                                                          new List
                                                                                                              <
                                                                                                              EA.Package
                                                                                                              >(),
                                                                                                          UMM.bDataV.
                                                                                                              ToString());


            //A BusinessDataView should contain one to many classes
            foreach (EA.Package businessDataView in businessDataViews)
            {
                int countClasses = 0;
                bool invalidfound = false;
                foreach (EA.Element subelement in businessDataView.Elements)
                {
                    if (subelement.Type == "Class")
                    {
                        countClasses++;
                    }
                    else
                    {
                        invalidfound = true;
                    }
                }

                //Classes found?
                if (countClasses < 1)
                {
                    context.AddValidationMessage(new ValidationMessage("Warning for constraint C29.",
                                                                       "A BusinessDataView SHOULD contain one to many classes.\n\nBusinessDataView " +
                                                                       businessDataView.Name + " does not contain any classes.",
                                                                       "BRV", ValidationMessage.errorLevelTypes.WARN, bev.PackageID));
                }
                //Invalid elements found?
                if (invalidfound)
                {
                    context.AddValidationMessage(new ValidationMessage("Warning for constraint C29.",
                                                                       "A BusinessDataView SHOULD contain one to many classes.\n\nBusinessDataView" +
                                                                       businessDataView.Name +
                                                                       " contains other elements than only classes.", "BRV",
                                                                       ValidationMessage.errorLevelTypes.WARN, bev.PackageID));
                }
            }
        }


        /// <summary>
        /// Check the tagged values oth e business entity view
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        private void checkTV_BusinessEntityView(IValidationContext context, EA.Package p)
        {
            //Check the TaggedValues of the bEntityV itself
            new TaggedValueValidator().validatePackage(context, p);

            //Get the BusinessDataViews (if any)
            IList<EA.Package> bdataViews = Utility.getAllSubPackagesWithGivenStereotypeRecursively(p,
                                                                                                   new List<EA.Package>(),
                                                                                                   UMM.bDataV.ToString());
            foreach (EA.Package package in bdataViews)
            {
                new TaggedValueValidator().validatePackage(context, package);
            }
        }
    }
}