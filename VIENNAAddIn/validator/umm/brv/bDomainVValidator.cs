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
    internal class bDomainVValidator : AbstractValidator
    {
        /// <summary>
        /// Validate the BusinessDomainView
        /// </summary>
        internal override void validate(IValidationContext context, string scope)
        {
            EA.Package p = context.Repository.GetPackageByID(Int32.Parse(scope));
            checkTV_BusinessDomainViews(context, p);
            checkC9(context, p);
            checkC10(context, p);
            checkC11(context, p);
            checkC12(context, p);
            checkC13(context, p);
            checkC14(context, p);
            checkC15(context, p);
            checkC16(context, p);
            checkC17(context, p);
            checkC18(context, p);
            checkC19(context, p);
            checkC20(context, p);
        }


        /// <summary>
        /// Check constraint C9
        /// </summary>
        private void checkC9(IValidationContext context, EA.Package bdv)
        {
            int count = 0;

            foreach (EA.Package p in bdv.Packages)
            {
                String stereotype = Utility.getStereoTypeFromPackage(p);
                if (stereotype == UMM.bArea.ToString())
                {
                    count++;
                }
                else
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C9.",
                                                                       "A BusinessDomainView MUST include one to many BusinessAreas. \n\nThe package " +
                                                                       p.Name + " has an invalid stereotype.", "BRV",
                                                                       ValidationMessage.errorLevelTypes.ERROR, p.PackageID));
                }
            }

            //Found less than 1 BusinessArea
            if (count < 1)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C9.",
                                                                   "A BusinessDomainView MUST include one to many BusinessAreas. \n\nThe package " +
                                                                   bdv.Name + " contains 0 BusinessAreas.", "BRV",
                                                                   ValidationMessage.errorLevelTypes.ERROR, bdv.PackageID));
            }
        }


        /// <summary>
        /// Check constraint C10
        /// </summary>
        private void checkC10(IValidationContext context, EA.Package bdv)
        {
            //Get a list with all the BusinessAreas in this BusinessDomainView
            IList<EA.Package> businessAreas = Utility.getAllSubPackagesWithGivenStereotypeRecursively(bdv,
                                                                                                      new List
                                                                                                          <EA.Package>(),
                                                                                                      UMM.bArea.ToString
                                                                                                          ());

            foreach (EA.Package barea in businessAreas)
            {
                //Does the business area have subpackages?
                if (barea.Packages != null && barea.Packages.Count != 0)
                {
                    int countProcessArea = 0;
                    int countBusinessArea = 0;

                    //If so - there must only be BusinessAreas or ProcessAreas
                    foreach (EA.Package subpackage in barea.Packages)
                    {
                        String stereotype = Utility.getStereoTypeFromPackage(subpackage);
                        if (!(stereotype == UMM.bArea.ToString() || stereotype == UMM.ProcessArea.ToString()))
                        {
                            context.AddValidationMessage(new ValidationMessage("Violation of constraint C10",
                                                                               "A BusinessArea MUST include one to many BusinessAreas or one to many ProcessAreas or one to many BusinessProcessUseCases. \n\nPackage " +
                                                                               subpackage.Name + " has an invalid stereotype.", "BRV",
                                                                               ValidationMessage.errorLevelTypes.ERROR,
                                                                               subpackage.PackageID));
                        }
                        else if (stereotype == UMM.bArea.ToString())
                        {
                            countBusinessArea++;
                        }
                        else if (stereotype == UMM.ProcessArea.ToString())
                        {
                            countProcessArea++;
                        }
                    }

                    //There MUST be one to many bAreas or one to many ProcessAreas
                    if (countBusinessArea < 1 && countProcessArea < 1)
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C10",
                                                                           "A BusinessArea MUST include one to many BusinessAreas or one to many ProcessAreas or one to many BusinessProcessUseCases. \n\nPackage " +
                                                                           barea.Name + " contains " + countBusinessArea +
                                                                           " BusinessAreas and " + countProcessArea +
                                                                           " ProcessAreas.", "BRV",
                                                                           ValidationMessage.errorLevelTypes.ERROR, barea.PackageID));
                    }
                }
                    //The given BusinessArea does not have any subpackages - it must have one BusinessProcessUseCase
                else
                {
                    int countBpuc = 0;

                    foreach (EA.Element element in barea.Elements)
                    {
                        if (element.Stereotype == UMM.bProcessUC.ToString())
                        {
                            countBpuc++;
                        }
                    }

                    if (countBpuc < 1)
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C10",
                                                                           "A BusinessArea MUST include one to many BusinessAreas or one to many ProcessAreas or one to many BusinessProcessUseCases. \n\nPackage " +
                                                                           barea.Name + " contains " + countBpuc +
                                                                           " BusinessProcessUseCases.", "BRV",
                                                                           ValidationMessage.errorLevelTypes.ERROR, barea.PackageID));
                    }
                }
            }
        }


        /// <summary>
        /// Check constraint C11
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bdv"></param>
        private void checkC11(IValidationContext context, EA.Package bdv)
        {
            //Get a list with all the ProcessAreas in this BusinessDomainView
            IList<EA.Package> processAreas = Utility.getAllSubPackagesWithGivenStereotypeRecursively(bdv,
                                                                                                     new List
                                                                                                         <EA.Package>(),
                                                                                                     UMM.ProcessArea.
                                                                                                         ToString());

            foreach (EA.Package processArea in processAreas)
            {
                //Given process area has subpackages - check if one of them is a another process area
                if (processArea.Packages != null && processArea.Packages.Count != 0)
                {
                    int count_subProcessAreas = 0;
                    foreach (EA.Package subpackage in processArea.Packages)
                    {
                        String stereotype = Utility.getStereoTypeFromPackage(subpackage);
                        if (stereotype == UMM.ProcessArea.ToString())
                        {
                            count_subProcessAreas++;
                        }
                            //The only subpackages allowed underneath a process area are other process areas
                        else
                        {
                            context.AddValidationMessage(new ValidationMessage("Violoation of constraint C11.",
                                                                               "A ProcessArea MUST contain one to many other ProcessAreas or one to many BusinessProcessUseCases. \n\nPackage " +
                                                                               processArea.Name +
                                                                               " contains a package with an invalid stereotype (" +
                                                                               stereotype + ").", "BRV",
                                                                               ValidationMessage.errorLevelTypes.ERROR,
                                                                               processArea.PackageID));
                        }
                    }

                    //The given processarea has subpackages - there MUST be at least one process area underneath
                    if (count_subProcessAreas < 1)
                    {
                        context.AddValidationMessage(new ValidationMessage("Violoation of constraint C11.",
                                                                           "A ProcessArea MUST contain one to many other ProcessAreas or one to many BusinessProcessUseCases. \n\nPackage " +
                                                                           processArea.Name +
                                                                           " contains subpackages but no ProcessArea could be found.",
                                                                           "BRV", ValidationMessage.errorLevelTypes.ERROR,
                                                                           processArea.PackageID));
                    }
                }
                else
                {
                    //No subpackages in this process area - hence there MUST be one to many BusinessProcessUseCases
                    int count_bpuc = 0;
                    foreach (EA.Element e in processArea.Elements)
                    {
                        if (e.Stereotype == UMM.bProcessUC.ToString())
                        {
                            count_bpuc++;
                        }
                    }

                    if (count_bpuc < 1)
                    {
                        context.AddValidationMessage(new ValidationMessage("Violoation of constraint C11.",
                                                                           "A ProcessArea MUST contain one to many other ProcessAreas or one to many BusinessProcessUseCases. \n\nPackage " +
                                                                           processArea.Name +
                                                                           " does neither contain ProcessAreas nor BusinessProcessUseCases.",
                                                                           "BRV", ValidationMessage.errorLevelTypes.ERROR,
                                                                           processArea.PackageID));
                    }
                }
            }
        }


        /// <summary>
        /// Check constraint C12
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bdv"></param>
        private void checkC12(IValidationContext context, EA.Package bdv)
        {
            //Get a list with all the BusinessProcessUseCases in this BusinessDomainView
            IList<EA.Element> bpucs = Utility.getAllElements(bdv, new List<EA.Element>(), UMM.bProcessUC.ToString());


            //Iterate over the business process use cases and check whether every use case is associated with one to many business partners
            foreach (EA.Element bpuc in bpucs)
            {
                int count_participatesAssocationsFound = 0;

                foreach (EA.Connector con in bpuc.Connectors)
                {
                    //Only associations or dependencies are allowed
                    if (
                        !(con.Type == AssociationTypes.Association.ToString() ||
                          con.Type == AssociationTypes.Dependency.ToString()))
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C12",
                                                                           "A BusinessProcessUseCase MUST be associated with one to many BusinessPartners using the participates relationship. \n\nInvalid connection (" +
                                                                           con.Type + ") to BusinessProcessUseCase " +
                                                                           bpuc.Name + " detected.", "BRV",
                                                                           ValidationMessage.errorLevelTypes.ERROR, bpuc.PackageID));
                    }
                        //Correct participates assocation detected
                    else if (con.Stereotype == UMM.participates.ToString())
                    {
                        count_participatesAssocationsFound++;

                        EA.Element client = context.Repository.GetElementByID(con.ClientID);
                        //Correct connection leading from a business partner to a business process use case
                        if (con.SupplierID == bpuc.ElementID)
                        {
                            //Client must be of type Business Partner
                            if (client.Stereotype != UMM.bPartner.ToString())
                            {
                                context.AddValidationMessage(new ValidationMessage("Violation of constraint C12",
                                                                                   "A BusinessProcessUseCase MUST be associated with one to many BusinessPartners using the participates relationship. \n\nThe particiaptes relationship must lead from a business partner to a business process use case.",
                                                                                   "BRV", ValidationMessage.errorLevelTypes.ERROR,
                                                                                   bpuc.PackageID));
                            }
                        }
                        else
                        {
                            context.AddValidationMessage(new ValidationMessage("Violation of constraint C12",
                                                                               "A BusinessProcessUseCase MUST be associated with one to many BusinessPartners using the participates relationship. \n\nA participates relationship must lead from a business partner to a business process use case.",
                                                                               "BRV", ValidationMessage.errorLevelTypes.ERROR,
                                                                               bpuc.PackageID));
                        }
                    }
                    else if (con.Stereotype == UMM.isOfInterestTo.ToString())
                    {
                        //do nothing here - will be vaidated in later constraints
                    }
                    else
                    {
                        //Invalid connection stereotype
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C12",
                                                                           "A BusinessProcessUseCase MUST be associated with one to many BusinessPartners using the participates relationship. \n\nInvalid connector to BusinessProcessUseCase " +
                                                                           bpuc.Name + " detected.", "BRV",
                                                                           ValidationMessage.errorLevelTypes.ERROR, bpuc.PackageID));
                    }
                }

                //Does this BPUC has any particpates connections?
                if (count_participatesAssocationsFound < 1)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C12",
                                                                       "A BusinessProcessUseCase MUST be associated with one to many BusinessPartners using the participates relationship. \n\nNo participates assocations found.",
                                                                       "BRV", ValidationMessage.errorLevelTypes.ERROR, bpuc.PackageID));
                }
            }
        }


        /// <summary>
        /// Check constraint C13
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bdv"></param>
        private void checkC13(IValidationContext context, EA.Package bdv)
        {
            //Get a list with all the BusinessProcessUseCases in this BusinessDomainView
            IList<EA.Element> bpucs = Utility.getAllElements(bdv, new List<EA.Element>(), UMM.bProcessUC.ToString());


            //Iterate over the business process use cases and check if the business process use case is connected to stakeholders
            foreach (EA.Element bpuc in bpucs)
            {
                foreach (EA.Connector con in bpuc.Connectors)
                {
                    //Only associations or dependencies are allowed
                    if (
                        !(con.Type == AssociationTypes.Association.ToString() ||
                          con.Type == AssociationTypes.Dependency.ToString()))
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C13",
                                                                           "A BusinessProcessUseCase MAY be associated with zero to many Stakeholders using the isOfInterestTo relationship. \n\nInvalid connection (" +
                                                                           con.Type + ") to BusinessProcessUseCase " +
                                                                           bpuc.Name + " detected.", "BRV",
                                                                           ValidationMessage.errorLevelTypes.ERROR, bpuc.PackageID));
                    }
                        //Correct isOfInterestTo assocation detected
                    else if (con.Stereotype == UMM.isOfInterestTo.ToString())
                    {
                        EA.Element client = context.Repository.GetElementByID(con.ClientID);
                        //Correct connection leading from a stakeholder to a business process use case
                        if (con.SupplierID == bpuc.ElementID)
                        {
                            //Client must be of type Stakeholder
                            if (client.Stereotype != UMM.Stakeholder.ToString())
                            {
                                context.AddValidationMessage(new ValidationMessage("Violation of constraint C13",
                                                                                   "A BusinessProcessUseCase MAY be associated with zero to many Stakeholders using the isOfInterestTo relationship. \n\nThe isOfInterestTo relationship must lead from a stakeholder to a business process use case.",
                                                                                   "BRV", ValidationMessage.errorLevelTypes.ERROR,
                                                                                   bpuc.PackageID));
                            }
                        }
                        else
                        {
                            context.AddValidationMessage(new ValidationMessage("Violation of constraint C13",
                                                                               "A BusinessProcessUseCase MAY be associated with zero to many Stakeholders using the isOfInterestTo relationship.  \n\nA participates relationship must lead from a stakeholder to a business process use case.",
                                                                               "BRV", ValidationMessage.errorLevelTypes.ERROR,
                                                                               bpuc.PackageID));
                        }
                    }
                    else if (con.Stereotype == UMM.participates.ToString())
                    {
                        //do nothing here - already validated in the last constraint
                    }
                    else
                    {
                        //Invalid connection stereotype
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C13",
                                                                           "A BusinessProcessUseCase MAY be associated with zero to many Stakeholders using the isOfInterestTo relationship.  \n\nInvalid connector to BusinessProcessUseCase " +
                                                                           bpuc.Name + " detected.", "BRV",
                                                                           ValidationMessage.errorLevelTypes.ERROR, bpuc.PackageID));
                    }
                }
            }
        }


        /// <summary>
        /// Check constraint C14
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bdv"></param>
        private void checkC14(IValidationContext context, EA.Package bdv)
        {
            //Get a list with all the BusinessProcessUseCases in this BusinessDomainView
            IList<EA.Element> bpucs = Utility.getAllElements(bdv, new List<EA.Element>(), UMM.bProcessUC.ToString());


            foreach (EA.Element bpuc in bpucs)
            {
                int parentPackageID = context.Repository.GetPackageByID(bpuc.PackageID).PackageID;

                bool found = false;
                //Is the business process use case refined by a business process?
                foreach (EA.Element subelement in bpuc.Elements)
                {
                    if (subelement.Stereotype == UMM.bProcess.ToString())
                    {
                        found = true;
                        break;
                    }
                }

                //No bProcess found - show a warn message
                if (!found)
                {
                    context.AddValidationMessage(new ValidationMessage("Warning for constraint C 14.",
                                                                       "A BusinessProcessUseCase SHOULD be refined by zero to many BusinessProcesses. \n\nThe BusinessProcessUseCase " +
                                                                       bpuc.Name + " is not refined by a BusinessProcess.", "BRV",
                                                                       ValidationMessage.errorLevelTypes.WARN, parentPackageID));
                }
                else
                {
                    context.AddValidationMessage(new ValidationMessage("Info to constraint C 14.",
                                                                       "A BusinessProcessUseCase SHOULD be refined by zero to many BusinessProcesses. \n\nThe BusinessProcessUseCase " +
                                                                       bpuc.Name + " is refined by a BusinessProcess.", "BRV",
                                                                       ValidationMessage.errorLevelTypes.INFO, parentPackageID));
                }
            }
        }


        /// <summary>
        /// Check constraint C15
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bdv"></param>
        private void checkC15(IValidationContext context, EA.Package bdv)
        {
            //Get a list with all the BusinessProcesses from the BusinessDomainView
            IList<EA.Element> bps = Utility.getAllElements(bdv, new List<EA.Element>(), UMM.bProcess.ToString());

            foreach (EA.Element bp in bps)
            {
                //Does the business process have a parent?
                if (bp.ParentID != 0)
                {
                    EA.Element el = context.Repository.GetElementByID(bp.ParentID);
                    if (el.Stereotype != UMM.bProcessUC.ToString())
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C15.",
                                                                           "A BusinessProcess MUST be modeled as a child of a BusinessProcessUseCase \n\nThe BusinessProcess " +
                                                                           bp.Name +
                                                                           " is not located underneath a BusinessProcessUseCase.",
                                                                           "BRV", ValidationMessage.errorLevelTypes.ERROR,
                                                                           bp.PackageID));
                    }
                }
                else
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C15.",
                                                                       "A BusinessProcess MUST be modeled as a child of a BusinessProcessUseCase \n\nThe BusinessProcess " +
                                                                       bp.Name +
                                                                       " is not located underneath a BusinessProcessUseCase.", "BRV",
                                                                       ValidationMessage.errorLevelTypes.ERROR, bp.PackageID));
                }
            }
        }


        /// <summary>
        /// Validate constraint C16
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bdv"></param>
        private void checkC16(IValidationContext context, EA.Package bdv)
        {
            //Get a list with all the BusinessProcessUseCases in this BusinessDomainView
            IList<EA.Element> bpucs = Utility.getAllElements(bdv, new List<EA.Element>(), UMM.bProcessUC.ToString());


            //Check which businessprocessusecase is refined by a UML Sequence Diagram
            foreach (EA.Element bpuc in bpucs)
            {
                int countSequenceDiagrams = 0;
                foreach (EA.Diagram diagram in bpuc.Diagrams)
                {
                    if (diagram.Type == "Sequence")
                    {
                        countSequenceDiagrams++;
                    }
                }

                if (countSequenceDiagrams > 0)
                {
                    context.AddValidationMessage(new ValidationMessage("Info to constraint C16.",
                                                                       "A BusinessProcessUseCase MAY be refined by zero to many UML Sequence Diagrams. \n\nThe BusinessProcessUseCase " +
                                                                       bpuc.Name + " is refined by " + countSequenceDiagrams +
                                                                       " UML Sequence Diagrams.", "BRV",
                                                                       ValidationMessage.errorLevelTypes.INFO, bpuc.PackageID));
                }
            }
        }


        /// <summary>
        /// Check constraint C17
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bdv"></param>
        private void checkC17(IValidationContext context, EA.Package bdv)
        {
            //Get a list with all the BusinessProcesses from the BusinessDomainView
            IList<EA.Element> bps = Utility.getAllElements(bdv, new List<EA.Element>(), UMM.bProcess.ToString());


            //Check which business process contains Activity Partitions
            foreach (EA.Element bp in bps)
            {
                int countPartition = 0;
                foreach (EA.Element subelement in bp.Elements)
                {
                    if (subelement.Type == "ActivityPartition")
                    {
                        countPartition++;
                    }
                }

                context.AddValidationMessage(new ValidationMessage("Info to constraint C17.",
                                                                   "A BusinessProcess MAY contain zero to many ActivityPartitions. \n\nThe BusinessProcess " +
                                                                   bp.Name + " contains " + countPartition + " ActivityPartitions.",
                                                                   "BRV", ValidationMessage.errorLevelTypes.INFO, bp.PackageID));
            }
        }


        /// <summary>
        /// Check constraint C18
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bdv"></param>
        private void checkC18(IValidationContext context, EA.Package bdv)
        {
            //Get a list with all the BusinessProcesses from the BusinessDomainView
            IList<EA.Element> bps = Utility.getAllElements(bdv, new List<EA.Element>(), UMM.bProcess.ToString());


            //Check which business process contains Activity Partitions
            foreach (EA.Element bp in bps)
            {
                int countPartition = 0;
                foreach (EA.Element subelement in bp.Elements)
                {
                    if (subelement.Type == "ActivityPartition")
                    {
                        countPartition++;
                    }
                }

                //No partition found
                if (countPartition < 1)
                {
                    int countBusinessProcessActions = 0;
                    int countInternalStates = 0;
                    int countSharedStates = 0;

                    //The business process MUST contain one or more business process actions
                    foreach (EA.Element subelement in bp.Elements)
                    {
                        //Count the business process actions - there  MUST be actions
                        if (subelement.Stereotype == UMM.bProcessAction.ToString())
                        {
                            countBusinessProcessActions++;
                        }
                            //count the internal business entity states - there MAY be states
                        else if (subelement.Stereotype == UMM.bEInternalState.ToString())
                        {
                            countInternalStates++;
                        }
                            //count the shared business entity state - there MAY be states
                        else if (subelement.Stereotype == UMM.bESharedState.ToString())
                        {
                            countSharedStates++;
                        }
                    }

                    //No business process actions
                    if (countBusinessProcessActions < 1)
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C18.",
                                                                           "A BusinessProcess, which has no ActivityPartitions, MUST contain one or more BusinessProcessActions and MAY contain zero to many InternalBusinessEntityStates and zero to many SharedBusinessEntityStates. \n\nNo BusinessProcessActions where found for the BusinessProcess " +
                                                                           bp.Name + ".", "BRV",
                                                                           ValidationMessage.errorLevelTypes.ERROR, bp.PackageID));
                    }
                    else
                    {
                        context.AddValidationMessage(new ValidationMessage("Info for constraint C18.",
                                                                           "A BusinessProcess, which has no ActivityPartitions, MUST contain one or more BusinessProcessActions and MAY contain zero to many InternalBusinessEntityStates and zero to many SharedBusinessEntityStates. \n\nThe BusinessProcess " +
                                                                           bp.Name + " contains " + countInternalStates +
                                                                           " InternalBusinessEntityStates and " + countSharedStates +
                                                                           " SharedBusinessEntityStates.", "BRV",
                                                                           ValidationMessage.errorLevelTypes.INFO, bp.PackageID));
                    }
                }

                context.AddValidationMessage(new ValidationMessage("Info to constraint C17.",
                                                                   "A BusinessProcess MAY contain zero to many ActivityPartitions. \n\nThe BusinessProcess " +
                                                                   bp.Name + " contains " + countPartition + " ActivityPartitions.",
                                                                   "BRV", ValidationMessage.errorLevelTypes.INFO, bp.PackageID));
            }
        }


        /// <summary>
        /// Check constraint C19
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bdv"></param>
        private void checkC19(IValidationContext context, EA.Package bdv)
        {
            //Get a list with all the BusinessProcesses from the BusinessDomainView
            IList<EA.Element> bps = Utility.getAllElements(bdv, new List<EA.Element>(), UMM.bProcess.ToString());


            //Check which business process contain Activity Partitions
            foreach (EA.Element bp in bps)
            {
                foreach (EA.Element subelement in bp.Elements)
                {
                    if (subelement.Type == "ActivityPartition")
                    {
                        int countBusinessProcessActions = 0;
                        int countInternalStates = 0;

                        //Activity Parition found -                         
                        foreach (EA.Element partitionSubElement in subelement.Elements)
                        {
                            //It MUST contain one to many BusinessProcessActions
                            if (partitionSubElement.Stereotype == UMM.bProcessAction.ToString())
                            {
                                countBusinessProcessActions++;
                            }
                                //It MAY contain SharedEntityStates
                            else if (partitionSubElement.Stereotype == UMM.bEInternalState.ToString())
                            {
                                countInternalStates++;
                            }
                        }

                        //No BusinessProcessActions found - error
                        if (countBusinessProcessActions < 1)
                        {
                            context.AddValidationMessage(new ValidationMessage("Violoation of constraint C19.",
                                                                               "An ActivityPartition being part of a BusinessProcess MUST contain one to many BusinessProcessActions and MAY contain zero to many InternalBusinessEntityStates. \n\nNo BusinessProcessActions where found in the ActivityPartition " +
                                                                               subelement.Name + " of the BusinessProcess " + bp.Name,
                                                                               "BRV", ValidationMessage.errorLevelTypes.ERROR,
                                                                               bp.PackageID));
                        }
                            //BusinessProcessActions found - report an INFO
                        else
                        {
                            context.AddValidationMessage(new ValidationMessage("Info for constraint C19.",
                                                                               "An ActivityPartition being part of a BusinessProcess MUST contain one to many BusinessProcessActions and MAY contain zero to many InternalBusinessEntityStates. \n\nThe ActivityParition " +
                                                                               subelement.Name + " of the BusinessProcess " + bp.Name +
                                                                               " contains " + countBusinessProcessActions +
                                                                               " BusinessProcessActions and " + countInternalStates +
                                                                               " InternalBusinessEntityStates.", "BRV",
                                                                               ValidationMessage.errorLevelTypes.INFO, bp.PackageID));
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Check constraint C20
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bdv"></param>
        private void checkC20(IValidationContext context, EA.Package bdv)
        {
            //Get a list with all the BusinessProcesses from the BusinessDomainView
            IList<EA.Element> bps = Utility.getAllElements(bdv, new List<EA.Element>(), UMM.bProcess.ToString());


            //Check which business process contain Activity Partitions
            foreach (EA.Element bp in bps)
            {
                foreach (EA.Element subelement in bp.Elements)
                {
                    if (subelement.Type == "ActivityPartition")
                    {
                        //An ActivityParition MUST not contain SharedBusinessEntityStates
                        foreach (EA.Element partitionElement in subelement.Elements)
                        {
                            if (partitionElement.Stereotype == UMM.bESharedState.ToString())
                            {
                                context.AddValidationMessage(new ValidationMessage("Violation of constraint C20.",
                                                                                   "A SharedBusinessEntityStates MUST NOT be located in an ActivityPartition. (They must be contained within the BusinessProcess even if this BusinessProcess contains ActivityPartitions.) \n\nSharedBusinessEntityState " +
                                                                                   partitionElement.Name +
                                                                                   " is locatdd in an ActivityPartition.", "BRV",
                                                                                   ValidationMessage.errorLevelTypes.ERROR,
                                                                                   bp.PackageID));
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Checks the TV of the Stereotypes from the Business Domain View
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        private void checkTV_BusinessDomainViews(IValidationContext context, EA.Package p)
        {
            //Get all BusinessAreas
            IList<EA.Package> bAreas = Utility.getAllSubPackagesWithGivenStereotypeRecursively(p, new List<EA.Package>(),
                                                                                               UMM.bArea.ToString());
            //Get all ProcessAreas
            IList<EA.Package> pAreas = Utility.getAllSubPackagesWithGivenStereotypeRecursively(p, new List<EA.Package>(),
                                                                                               UMM.ProcessArea.ToString());

            //Check the TaggedValues of the BusinessDomainView package
            new TaggedValueValidator().validatePackage(context, p);

            //Check all BusinessAreas
            foreach (EA.Package bArea in bAreas)
            {
                new TaggedValueValidator().validatePackage(context, bArea);
            }

            //Check all ProcressAreas
            foreach (EA.Package pArea in pAreas)
            {
                new TaggedValueValidator().validatePackage(context, pArea);
            }

            //Get all BusinessProcessUseCases recursively from the Business Domain View
            IList<EA.Element> bPUCs = Utility.getAllElements(p, new List<EA.Element>(), UMM.bProcessUC.ToString());
            List<EA.Connector> connectors = new List<EA.Connector>();
            foreach (EA.Element bPUC in bPUCs)
            {
                new TaggedValueValidator().validateElement(context, bPUC);
                foreach (EA.Connector con in bPUC.Connectors)
                {
                    connectors.Add(con);
                }
            }

            //Validate the participates and isOfInterestTo connectors
            new TaggedValueValidator().validateConnectors(context, connectors);
        }
    }
}