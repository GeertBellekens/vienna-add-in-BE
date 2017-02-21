/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.constants;
using VIENNAAddIn.common;


namespace VIENNAAddIn.validator.umm.bcv
{
    internal class bTransactionVValidator : AbstractValidator
    {
        /// <summary>
        /// Validate the BusinessTransactionView
        /// </summary>
        internal override void validate(IValidationContext parentContext, string scope)
        {
            ErrorCheckingValidationContext context = new ErrorCheckingValidationContext(parentContext);

            EA.Package btv = context.Repository.GetPackageByID(Int32.Parse(scope));

            checkTV_BusinessTransactionView(context, btv);
            checkC34(context, btv);
            checkC35(context, btv);
            checkC36(context, btv);
            checkC37(context, btv);
            checkC38(context, btv);
            checkC39(context, btv);
            checkC40(context, btv);
            checkC41(context, btv);
            checkC42(context, btv);
            if (context.HasError) return;
            checkC43(context, btv);
            if (context.HasError) return;
            checkC44(context, btv);
            if (context.HasError) return;
            checkC45(context, btv);
            if (context.HasError) return;
            checkC46(context, btv);
            checkC47(context, btv);
            checkC48(context, btv);
            checkC49(context, btv);
            checkC50(context, btv);
            if (context.HasError) return;
            checkC51(context, btv);
            checkC52(context, btv);
            checkC53(context, btv);
            checkC54(context, btv);
            checkC55(context, btv);
            checkC56(context, btv);
            checkC57(context, btv);
        }


        /// <summary>
        /// Check constraint C34
        /// </summary>
        /// <param name="context"></param>
        /// <param name="btv"></param>
        private void checkC34(IValidationContext context, EA.Package btv)
        {
            int count_AR = 0;
            int count_btuc = 0;

            foreach (EA.Element e in btv.Elements)
            {
                //Count BTUC
                if (e.Stereotype == UMM.bTransactionUC.ToString())
                {
                    count_btuc++;
                }
                    //Count Authorized Roles
                else if (e.Stereotype == UMM.AuthorizedRole.ToString())
                {
                    count_AR++;
                }
                    //Invalid stereotype detected
                else
                {
                    //Ignore subelements
                    if (e.ParentID == 0)
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C34.",
                                                                           "A BusinessTransactionView MUST contain exactly one BusinessTransactionUseCase, exactly two AuthorizedRoles, and exactly two participates associations.\n\nAn element with an invalid stereotype has been detected: " +
                                                                           e.Name, "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                           btv.PackageID));
                    }
                }
            }

            //There must be exactly one business transaction use case
            if (count_btuc != 1)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C34.",
                                                                   "A BusinessTransactionView MUST contain exactly one BusinessTransactionUseCase, exactly two AuthorizedRoles, and exactly two participates associations.\n\nInvalid number of BusinessTransactionUseCases detected.",
                                                                   "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
            }

            //There must be exactly two Authorized Roles
            if (count_AR != 2)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C34.",
                                                                   "A BusinessTransactionView MUST contain exactly one BusinessTransactionUseCase, exactly two AuthorizedRoles, and exactly two participates associations.\n\nInvalid number of AuthorizedRoles detected.",
                                                                   "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
            }

            //We do not check the two participates associations here since that is taken care of in constraint 
            //C35
        }


        /// <summary>
        /// Check constraint C35
        /// </summary>
        /// <param name="context"></param>
        /// <param name="btv"></param>
        private void checkC35(IValidationContext context, EA.Package btv)
        {
            //Find the Business Transaction Use Case
            EA.Element btuc = null;

            foreach (EA.Element e in btv.Elements)
            {
                if (e.Stereotype == UMM.bTransactionUC.ToString())
                {
                    btuc = e;
                    break;
                }
            }

            if (btuc != null)
            {
                int countAssociations = 0;
                //Make sure, that the BTUC is associated with exactly two authorized roles via participates assocations
                foreach (EA.Connector con in btuc.Connectors)
                {
                    //Only associations and dependencies are allowed
                    if (
                        !(con.Type == AssociationTypes.Association.ToString() ||
                          con.Type == AssociationTypes.Dependency.ToString() ||
                          con.Type == AssociationTypes.Realisation.ToString() || con.Type == "UseCase"))
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C35.",
                                                                           "A BusinessTransactionUseCase MUST be associated with exactly two AuthorizedRoles via stereotyped binary participates associations.\n\nOnly assocations and dependencies are allowed. Invalid connection type found: " +
                                                                           con.Type + ".", "BCV",
                                                                           ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                    }
                    else if (con.Stereotype == UMM.participates.ToString())
                    {
                        //Correct connection leading from an AuthorizedRole to a business transaction use case
                        if (con.SupplierID == btuc.ElementID)
                        {
                            //Client must be of type Authorized role
                            EA.Element client = context.Repository.GetElementByID(con.ClientID);

                            if (client.Stereotype != UMM.AuthorizedRole.ToString())
                            {
                                context.AddValidationMessage(new ValidationMessage("Violation of constraint C35.",
                                                                                   "A BusinessTransactionUseCase MUST be associated with exactly two AuthorizedRoles via stereotyped binary participates associations.\n\nThe particiaptes relationship must lead from an AuthorizedRole to a BusinessTransactionUseCase.",
                                                                                   "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                                   btv.PackageID));
                            }
                            else
                            {
                                countAssociations++;
                            }
                        }
                        else
                        {
                            context.AddValidationMessage(new ValidationMessage("Violation of constraint C35.",
                                                                               "A BusinessTransactionUseCase MUST be associated with exactly two AuthorizedRoles via stereotyped binary participates associations.\n\nThe particiaptes relationship must lead from an AuthorizedRole to a BusinessTransactionUseCase.",
                                                                               "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                               btv.PackageID));
                        }
                    }
                    else if (con.Stereotype == UMM.include.ToString())
                    {
                        //do nothing here
                    }
                        //Wrong stereotype
                    else
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C35.",
                                                                           "A BusinessTransactionUseCase MUST be associated with exactly two AuthorizedRoles via stereotyped binary participates associations.\n\nInvalid or empty connection stereotype found: " +
                                                                           con.Stereotype, "BCV",
                                                                           ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                    }
                }

                //There must be exactly two participates assocations
                if (countAssociations != 2)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C35",
                                                                       "A BusinessTransactionUseCase MUST be associated with exactly two AuthorizedRoles via stereotyped binary participate associations.\n\nInvalid number of participates assocations found (" +
                                                                       countAssociations + ").", "BCV",
                                                                       ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                }
            }
        }


        /// <summary>
        /// Validate constraint C36
        /// </summary>
        /// <param name="context"></param>
        /// <param name="btv"></param>
        private void checkC36(IValidationContext context, EA.Package btv)
        {
            //Find the Business Transaction Use Case
            EA.Element btuc = null;

            foreach (EA.Element e in btv.Elements)
            {
                if (e.Stereotype == UMM.bTransactionUC.ToString())
                {
                    btuc = e;
                    break;
                }
            }

            //Business Transaction Use Case found
            if (btuc != null)
            {
                foreach (EA.Connector con in btuc.Connectors)
                {
                    //A BTUC must not be the client of an assocation of any kind
                    if (con.ClientID == btuc.ElementID)
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C36.",
                                                                           "A BusinessTransactionUseCase MUST NOT include further UseCases.\n\nA BusinessTransactionUseCase cannot be the client of any relationship (no connection must lead FROM a BusinessTransactionUseCase TO another model element). Please check the correct direction of BusinessTransactionUseCase's connections.",
                                                                           "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                           btv.PackageID));
                    }
                }
            }
        }


        /// <summary>
        /// Check constraint C37
        /// </summary>
        /// <param name="context"></param>
        /// <param name="btv"></param>
        private void checkC37(IValidationContext context, EA.Package btv)
        {
            //Find the Business Transaction Use Case
            EA.Element btuc = null;

            foreach (EA.Element e in btv.Elements)
            {
                if (e.Stereotype == UMM.bTransactionUC.ToString())
                {
                    btuc = e;
                    break;
                }
            }

            //Business Transaction Use Case found
            if (btuc != null)
            {
                bool found = false;

                //A BTUC must be included in at least one BCUC
                foreach (EA.Connector con in btuc.Connectors)
                {
                    if (con.Stereotype == UMM.include.ToString())
                    {
                        //The BTUC must be the supplier of the include relationship
                        if (con.SupplierID == btuc.ElementID)
                        {
                            //The Client of the relationship must be a bcuc
                            EA.Element e = context.Repository.GetElementByID(con.ClientID);
                            if (e.Stereotype == UMM.bCollaborationUC.ToString())
                            {
                                found = true;
                            }
                        }
                    }
                }
                //No BCUC relationship found
                if (!found)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C37.",
                                                                       "A BusinessTransactionUseCase MUST be included in at least one BusinessCollaborationUseCase. \n\nNo include relationship to a BusinessCollaborationView has been found. Go to the BusinessChoreographyView and make sure, that a connection exists.",
                                                                       "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                }
            }
        }


        /// <summary>
        /// Check constraint C38
        /// </summary>
        /// <param name="context"></param>
        /// <param name="btv"></param>
        private void checkC38(IValidationContext context, EA.Package btv)
        {
            //Find the Business Transaction Use Case
            EA.Element btuc = null;

            foreach (EA.Element e in btv.Elements)
            {
                if (e.Stereotype == UMM.bTransactionUC.ToString())
                {
                    btuc = e;
                    break;
                }
            }

            //Business Transaction Use Case found
            if (btuc != null)
            {
                //A BTUC must be included in at least one BCUC
                foreach (EA.Connector con in btuc.Connectors)
                {
                    if (con.Stereotype == "extend")
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C38.",
                                                                           "A BusinessTransactionUseCase MUST NOT be source or target of an extend association. \n\nAn 'extend' connection has been detected.",
                                                                           "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                           btv.PackageID));
                    }
                }
            }
        }


        /// <summary>
        /// Check constraint C39
        /// </summary>
        /// <param name="context"></param>
        /// <param name="btv"></param>
        private void checkC39(IValidationContext context, EA.Package btv)
        {
            //Find the Business Transaction Use Case
            EA.Element btuc = null;

            foreach (EA.Element e in btv.Elements)
            {
                if (e.Stereotype == UMM.bTransactionUC.ToString())
                {
                    btuc = e;
                    break;
                }
            }

            if (btuc != null)
            {
                String name = "";

                //Make sure, that the two Authorized Roles of a BTUC do not have the same name
                foreach (EA.Connector con in btuc.Connectors)
                {
                    //Only associations and dependencies are allowed
                    if (con.Stereotype == UMM.participates.ToString())
                    {
                        //Correct connection leading from an AuthorizedRole to a business transaction use case
                        if (con.SupplierID == btuc.ElementID)
                        {
                            //Client must be of type Authorized role
                            EA.Element client = context.Repository.GetElementByID(con.ClientID);

                            if (client.Stereotype == UMM.AuthorizedRole.ToString())
                            {
                                if (name == "")
                                {
                                    name = client.Name;
                                }
                                else
                                {
                                    //No two Authorized Roles must have the same name
                                    if (name == client.Name)
                                    {
                                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C39.",
                                                                                           "The two AuthorizedRoles within a BusinessTransactionView MUST NOT be named identically.",
                                                                                           "BCV",
                                                                                           ValidationMessage.errorLevelTypes.ERROR,
                                                                                           btv.PackageID));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Check constraint C40
        /// </summary>
        /// <param name="context"></param>
        /// <param name="btv"></param>
        private void checkC40(IValidationContext context, EA.Package btv)
        {
            //Find the Business Transaction Use Case
            EA.Element btuc = null;

            foreach (EA.Element e in btv.Elements)
            {
                if (e.Stereotype == UMM.bTransactionUC.ToString())
                {
                    btuc = e;
                    break;
                }
            }

            if (btuc != null)
            {
                int countBT = 0;
                //Is the BTUC described by a Business Transaction?
                foreach (EA.Element e in btv.Elements)
                {
                    if (e.Stereotype == UMM.bTransaction.ToString())
                    {
                        countBT++;
                    }
                }

                if (countBT != 1)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C40.",
                                                                       "A BusinessTransactionUseCase MUST be described by exactly one BusinessTransaction defined as a child element of this BusinessTransactionUseCase.\n\nFound " +
                                                                       countBT + " BusinessTransactions.", "BCV",
                                                                       ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                }
            }
        }


        /// <summary>
        /// Check constraint C41
        /// </summary>
        /// <param name="context"></param>
        /// <param name="btv"></param>
        private void checkC41(IValidationContext context, EA.Package btv)
        {
            //Get the Business Transaction
            EA.Element bta = Utility.getElementFromPackage(btv, UMM.bTransaction.ToString());
            if (bta != null)
            {
                int countPartitions = 0;
                //A business transaction must have two partitions
                foreach (EA.Element e in bta.Elements)
                {
                    if (e.Type == "ActivityPartition" && e.Stereotype == UMM.bTPartition.ToString())
                    {
                        countPartitions++;
                    }
                }

                if (countPartitions != 2)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C41.",
                                                                       "A BusinessTransaction MUST have exactly two partitions. Each of them MUST be stereotyped as BusinessTransactionPartition. \n\nMake sure that the BusinessTransactionPartition is located underneath the BusinessTransaction in the TreeView on the right hand side.",
                                                                       "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                }
            }
        }


        /// <summary>
        /// Check constraing C42
        /// </summary>
        /// <param name="context"></param>
        /// <param name="btv"></param>
        private void checkC42(IValidationContext context, EA.Package btv)
        {
            //Get the Business Transaction
            EA.Element bta = Utility.getElementFromPackage(btv, UMM.bTransaction.ToString());
            if (bta != null)
            {
                List<EA.Element> partitions = new List<EA.Element>();

                //Find the two partitions
                foreach (EA.Element e in bta.Elements)
                {
                    if (e.Type == "ActivityPartition" && e.Stereotype == UMM.bTPartition.ToString())
                    {
                        partitions.Add(e);
                    }
                }

                //Invalid number of partitions
                if (partitions.Count != 2)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C42.",
                                                                       "One of the two BusinessTransactionPartitions MUST contain one RequestingBusinessAction and the other one MUST contain one RespondingBusinessAction. \n\nCannot validate constraint due to invalid number of partitions (" +
                                                                       partitions.Count +
                                                                       "). There must be exactly two partitions in a BusinessTransaction.",
                                                                       "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                }
                else
                {
                    int i = 0;
                    bool bothInTheSame = false;
                    int numberReq = 0;
                    int numberRes = 0;


                    foreach (EA.Element partition in partitions)
                    {
                        i++;
                        foreach (EA.Element element in btv.Elements)
                        {
                            if (element.ParentID == partition.ElementID)
                            {
                                if (element.Stereotype == UMM.ReqAction.ToString())
                                {
                                    numberReq++;
                                }
                                if (element.Stereotype == UMM.ResAction.ToString())
                                {
                                    numberRes++;
                                }
                            }
                        }

                        //Both found in the same partition
                        if (numberReq == 1 && numberRes == 1 && !(i == 2))
                        {
                            context.AddValidationMessage(new ValidationMessage("Violation of constraint C42.",
                                                                               "One of the two BusinessTransactionPartitions MUST contain one RequestingBusinessAction and the other one MUST contain one RespondingBusinessAction. \n\nBoth, RequestingBusinessAction and RespondingBusinessAction have been found in the same partition.",
                                                                               "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                               btv.PackageID));
                            bothInTheSame = true;
                            break;
                        }
                    }

                    if (!bothInTheSame)
                    {
                        //Neither/nor found                              
                        if (numberReq == 0 && numberRes == 0)
                        {
                            context.AddValidationMessage(new ValidationMessage("Violation of constraint C42.",
                                                                               "One of the two BusinessTransactionPartitions MUST contain one RequestingBusinessAction and the other one MUST contain one RespondingBusinessAction. \n\nNeither the RequestingBusinessAction nor the RespondingBusinessAction have been found.",
                                                                               "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                               btv.PackageID));
                        }
                        else if (numberReq == 0)
                        {
                            context.AddValidationMessage(new ValidationMessage("Violation of constraint C42.",
                                                                               "One of the two BusinessTransactionPartitions MUST contain one RequestingBusinessAction and the other one MUST contain one RespondingBusinessAction. \n\nThe RequestingBusinessAction has not been found.",
                                                                               "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                               btv.PackageID));
                        }
                        else if (numberRes == 0)
                        {
                            context.AddValidationMessage(new ValidationMessage("Violation of constraint C42.",
                                                                               "One of the two BusinessTransactionPartitions MUST contain one RequestingBusinessAction and the other one MUST contain one RespondingBusinessAction. \n\nThe RespondingBusinessAction has not been found.",
                                                                               "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                               btv.PackageID));
                        }
                        else if (numberReq > 1)
                        {
                            context.AddValidationMessage(new ValidationMessage("Violation of constraint C42.",
                                                                               "One of the two BusinessTransactionPartitions MUST contain one RequestingBusinessAction and the other one MUST contain one RespondingBusinessAction. \n\nMore than one RequestingBusinessAction has been found.",
                                                                               "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                               btv.PackageID));
                        }
                        else if (numberRes > 1)
                        {
                            context.AddValidationMessage(new ValidationMessage("Violation of constraint C42.",
                                                                               "One of the two BusinessTransactionPartitions MUST contain one RequestingBusinessAction and the other one MUST contain one RespondingBusinessAction. \n\nMore than one RespondingBusinessAction has been found.",
                                                                               "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                               btv.PackageID));
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Validate constraint C43
        /// </summary>
        /// <param name="context"></param>
        /// <param name="btv"></param>
        private void checkC43(IValidationContext context, EA.Package btv)
        {
            List<EA.Element> partitions = new List<EA.Element>();
            EA.Element bta = Utility.getElementFromPackage(btv, UMM.bTransaction.ToString());

            //Find the two partitions
            foreach (EA.Element e in bta.Elements)
            {
                if (e.Type == "ActivityPartition" && e.Stereotype == UMM.bTPartition.ToString())
                {
                    partitions.Add(e);
                }
            }

            //Invalid number of partitions
            if (partitions.Count != 2)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C44.",
                                                                   "A BusinessTransactionPartition MUST have a classifier, which MUST be one of the associated AuthorizedRoles of the corresponding BusinessTransactionUseCase.\n\nCannot validate constraint due to invalid number of partitions (" +
                                                                   partitions.Count +
                                                                   "). There must be exactly two partitions in a BusinessTransaction.",
                                                                   "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
            }
            else
            {
                //Get the two authorized roles of the business transaction use case
                //Find the Business Transaction Use Case

                EA.Element btuc = Utility.getElementFromPackage(btv, UMM.bTransactionUC.ToString());


                //No BTUC found
                if (btuc == null)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C44.",
                                                                       "A BusinessTransactionPartition MUST have a classifier, which MUST be one of the associated AuthorizedRoles of the corresponding BusinessTransactionUseCase.\n\nCannot validate constraint because no BusinessTransactionUseCase could be found.",
                                                                       "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                }
                else
                {
                    //Get the two authorized roles
                    List<EA.Element> authorizedRoles = this.getAuthorizedRolesFromBTUC(context, btuc);
                    if (authorizedRoles.Count != 2)
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C44.",
                                                                           "A BusinessTransactionPartition MUST have a classifier, which MUST be one of the associated AuthorizedRoles of the corresponding BusinessTransactionUseCase.\n\nCannot validate constraint because an invalid number of AuthorizedRoles has been found.",
                                                                           "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                           btv.PackageID));
                    }
                    else
                    {
                        foreach (EA.Element partition in partitions)
                        {
                            if (!(partition.ClassifierID == authorizedRoles[0].ElementID ||
                                  partition.ClassifierID == authorizedRoles[1].ElementID))
                            {
                                context.AddValidationMessage(new ValidationMessage("Violation of constraint C44.",
                                                                                   "A BusinessTransactionPartition MUST have a classifier, which MUST be one of the associated AuthorizedRoles of the corresponding BusinessTransactionUseCase.\n\nMissing or wrong classifier detected. Make sure, that every BusinessTransactionPartition is classified correctly.",
                                                                                   "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                                   btv.PackageID));
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Validating constraint C44
        /// </summary>
        /// <param name="context"></param>
        /// <param name="btv"></param>
        private void checkC44(IValidationContext context, EA.Package btv)
        {
            List<EA.Element> partitions = new List<EA.Element>();

            //Find the two partitions
            foreach (EA.Element e in btv.Elements)
            {
                if (e.Type == "ActivityPartition" && e.Stereotype == UMM.bTPartition.ToString())
                {
                    partitions.Add(e);
                }
            }

            //There must be two partitions
            if (partitions.Count != 2)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C44.",
                                                                   "The two BusinessTransactionPartitions MUST have different classifiers. \n\nCannot validate the constraint because an invalid number of partitions has been found (" +
                                                                   partitions.Count +
                                                                   "). There must be exactly two partitions in a business transaction.",
                                                                   "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
            }
            else
            {
                //Check whether the two partitions have the same classifier
                if (partitions[0].ClassifierID == partitions[1].ClassifierID)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C44.",
                                                                       "The two BusinessTransactionPartitions MUST have different classifiers.",
                                                                       "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                }
            }
        }


        /// <summary>
        /// Validate constraint C45
        /// </summary>
        /// <param name="context"></param>
        /// <param name="btv"></param>
        private void checkC45(IValidationContext context, EA.Package btv)
        {
            //Get the RequestingBusinessTransactionPartition
            List<EA.Element> partitions = new List<EA.Element>();

            //Find the two partitions
            foreach (EA.Element e in btv.Elements)
            {
                if (e.Type == "ActivityPartition" && e.Stereotype == UMM.bTPartition.ToString())
                {
                    partitions.Add(e);
                }
            }

            //There must be two partitions
            if (partitions.Count != 2)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C45.",
                                                                   "The BusinessTransactionPartition containing the RequestingBusinessAction MUST contain two or more FinalStates. Each of the FinalStates MAY have a BusinessEntitySharedState as predecessor. One of the FinalStates SHOULD reflect a ControlFailure – this FinalState SHOULD NOT have a predecessing SharedBusinessEntityState.\n\nInvalid number of partitions found. There must be exactly two partitions.",
                                                                   "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
            }
            else
            {
                //Get the partition containing the requesting action
                EA.Element requestingPartition = null;
                foreach (EA.Element partition in partitions)
                {
                    foreach (EA.Element e in btv.Elements)
                    {
                        if (e.ParentID == partition.ElementID && e.Stereotype == UMM.ReqAction.ToString())
                        {
                            requestingPartition = partition;
                            break;
                        }
                    }
                }

                if (requestingPartition == null)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C45.",
                                                                       "The BusinessTransactionPartition containing the RequestingBusinessAction MUST contain two or more FinalStates. Each of the FinalStates MAY have a BusinessEntitySharedState as predecessor. One of the FinalStates SHOULD reflect a ControlFailure – this FinalState SHOULD NOT have a predecessing SharedBusinessEntityState.\n\nUnable to find a partition containing the RequestingBusinessAction.",
                                                                       "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                }
                else
                {
                    //Iterate over the elements of the Requesting Partition
                    int countFinalStates = 0;
                    int countControlFailures = 0;

                    foreach (EA.Element e in btv.Elements)
                    {
                        if (e.ParentID == requestingPartition.ElementID)
                        {
                            if (e.Type == "StateNode")
                            {
                                if (e.Subtype == 101)
                                {
                                    countFinalStates++;

                                    //Final state found - does the final state has a BusinessEntityState as predecessor?
                                    foreach (EA.Connector con in e.Connectors)
                                    {
                                        EA.Element supplier = context.Repository.GetElementByID(con.ClientID);

                                        //A ControlFailure should not have a preceeding bESharedState
                                        if (e.Name == "ControlFailure")
                                        {
                                            if (supplier.Stereotype == UMM.bESharedState.ToString())
                                            {
                                                context.AddValidationMessage(new ValidationMessage("Info for constraint C45.",
                                                                                                   "The BusinessTransactionPartition containing the RequestingBusinessAction MUST contain two or more FinalStates. Each of the FinalStates MAY have a BusinessEntitySharedState as predecessor. One of the FinalStates SHOULD reflect a ControlFailure – this FinalState SHOULD NOT have a predecessing SharedBusinessEntityState. \n\nThe FinalState ControlFailure has a predecessing SharedBusinessEntityState.",
                                                                                                   "BCV",
                                                                                                   ValidationMessage.errorLevelTypes.
                                                                                                       WARN, btv.PackageID));
                                            }
                                        }
                                        else
                                        {
                                            if (supplier.Stereotype != UMM.bESharedState.ToString())
                                            {
                                                context.AddValidationMessage(new ValidationMessage("Info for constraint C45.",
                                                                                                   "The BusinessTransactionPartition containing the RequestingBusinessAction MUST contain two or more FinalStates. Each of the FinalStates MAY have a BusinessEntitySharedState as predecessor. One of the FinalStates SHOULD reflect a ControlFailure – this FinalState SHOULD NOT have a predecessing SharedBusinessEntityState. \n\nFinalState " +
                                                                                                   e.Name +
                                                                                                   " does not have a preceeding BusinessEntitySharedState.",
                                                                                                   "BCV",
                                                                                                   ValidationMessage.errorLevelTypes.
                                                                                                       INFO, btv.PackageID));
                                            }
                                        }
                                    }

                                    //No Connectors found
                                    if (e.Connectors.Count == 0)
                                    {
                                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C45.",
                                                                                           "The BusinessTransactionPartition containing the RequestingBusinessAction MUST contain two or more FinalStates. Each of the FinalStates MAY have a BusinessEntitySharedState as predecessor. One of the FinalStates SHOULD reflect a ControlFailure – this FinalState SHOULD NOT have a predecessing SharedBusinessEntityState. \n\nThe FinalState " +
                                                                                           e.Name + " does not have any connections.",
                                                                                           "BCV",
                                                                                           ValidationMessage.errorLevelTypes.ERROR,
                                                                                           btv.PackageID));
                                    }


                                    if (e.Name == "ControlFailure")
                                        countControlFailures++;
                                }
                            }
                        }
                    }

                    if (countFinalStates < 2)
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C45.",
                                                                           "The BusinessTransactionPartition containing the RequestingBusinessAction MUST contain two or more FinalStates. Each of the FinalStates MAY have a BusinessEntitySharedState as predecessor. One of the FinalStates SHOULD reflect a ControlFailure – this FinalState SHOULD NOT have a predecessing SharedBusinessEntityState. \n\nInvalid number of Final States found: " +
                                                                           countFinalStates, "BCV",
                                                                           ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                    }
                    else
                    {
                        context.AddValidationMessage(new ValidationMessage("Info for constraint C45.",
                                                                           "The BusinessTransactionPartition containing the RequestingBusinessAction MUST contain two or more FinalStates. Each of the FinalStates MAY have a BusinessEntitySharedState as predecessor. One of the FinalStates SHOULD reflect a ControlFailure – this FinalState SHOULD NOT have a predecessing SharedBusinessEntityState. \n\nFound " +
                                                                           countFinalStates + " FinalStates.", "BCV",
                                                                           ValidationMessage.errorLevelTypes.INFO, btv.PackageID));
                    }

                    if (countControlFailures != 1)
                    {
                        context.AddValidationMessage(new ValidationMessage("Info for constraint C45.",
                                                                           "The BusinessTransactionPartition containing the RequestingBusinessAction MUST contain two or more FinalStates. Each of the FinalStates MAY have a BusinessEntitySharedState as predecessor. One of the FinalStates SHOULD reflect a ControlFailure – this FinalState SHOULD NOT have a predecessing SharedBusinessEntityState. \n\nNo FinalState ControlFailure found.",
                                                                           "BCV", ValidationMessage.errorLevelTypes.WARN,
                                                                           btv.PackageID));
                    }
                }
            }
        }


        /// <summary>
        /// Check constraint C46
        /// </summary>
        /// <param name="context"></param>
        /// <param name="btv"></param>
        private void checkC46(IValidationContext context, EA.Package btv)
        {
            //Get the Requesting Action
            EA.Element requestingAction = Utility.getElementFromPackage(btv, UMM.ReqAction.ToString());

            if (requestingAction == null)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C46.",
                                                                   "A RequestingBusinessAction MUST embed exactly one RequestingInformationPin.\n\nNo RequestingBusinessAction could be found. ",
                                                                   "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
            }
            else
            {
                int countPin = 0;
                foreach (EA.Element e in requestingAction.EmbeddedElements)
                {
                    if (e.Type == "ActionPin" && e.Stereotype == UMM.ReqInfPin.ToString())
                    {
                        countPin++;
                    }
                }
                if (countPin != 1)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C46.",
                                                                       "A RequestingBusinessAction MUST embed exactly one RequestingInformationPin.\n\nInvalid number of RequestingInformationPins found: " +
                                                                       countPin, "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                       btv.PackageID));
                }
            }
        }


        /// <summary>
        /// Check constraint C47
        /// </summary>
        /// <param name="context"></param>
        /// <param name="btv"></param>
        private void checkC47(IValidationContext context, EA.Package btv)
        {
            //Get the Responding Action
            Element respondingAction = Utility.getElementFromPackage(btv, UMM.ResAction.ToString());

            if (respondingAction == null)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C47.",
                                                                   "A RespondingBusinessAction MUST embed exactly one RequestingInformationPin. \n\nRespondingBusinessAction not found.",
                                                                   "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
            }
            else
            {
                int countPin = 0;
                foreach (EA.Element e in respondingAction.EmbeddedElements)
                {
                    if (e.Type == "ActionPin" && e.Stereotype == UMM.ReqInfPin.ToString())
                    {
                        countPin++;
                    }
                }


                if (countPin != 1)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C47.",
                                                                       "A RespondingBusinessAction MUST embed exactly one RequestingInformationPin. \n\nInvalid number of RequestingInformationPins found: " +
                                                                       countPin, "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                       btv.PackageID));
                }
            }
        }


        /// <summary>
        /// Check constraint C48
        /// </summary>
        /// <param name="context"></param>
        /// <param name="btv"></param>
        private void checkC48(IValidationContext context, EA.Package btv)
        {
            //Get the BusinessTransaction
            EA.Element bt = Utility.getElementFromPackage(btv, UMM.bTransaction.ToString());

            //No BusinessTransactionfound
            if (bt == null)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C48.",
                                                                   "If the tagged value businessTransactionType of the BusinessTransaction is either Request/Response, Query/Response, Request/Confirm, or CommercialTransaction, then the RequestingBusinessAction must embed one to many RespondingInformationPins and the RespondingBusinessAction must embed one to many RespondingInformationPins.\n\n",
                                                                   "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
            }
            else
            {
                //Get the TaggedValue businessTransactionType
                bool found = false;
                foreach (EA.TaggedValue tvalue in bt.TaggedValues)
                {
                    if (tvalue.Name == "businessTransactionType")
                    {
                        //Check the value
                        if (tvalue.Value == "Request/Response" || tvalue.Value == "Query/Response" ||
                            tvalue.Value == "Request/Confirm" || tvalue.Value == "Commercial Transaction")
                        {
                            found = true;
                            break;
                        }
                    }
                }

                if (found)
                {
                    //Get the Requesting Action
                    EA.Element requestingAction = Utility.getElementFromPackage(btv, UMM.ReqAction.ToString());

                    if (requestingAction != null)
                    {
                        //There must be one to many RespondingInformationPins
                        int countPins = 0;
                        foreach (EA.Element e in requestingAction.EmbeddedElements)
                        {
                            if (e.Type == "ActionPin" && e.Stereotype == UMM.ResInfPin.ToString())
                            {
                                countPins++;
                            }
                        }

                        if (countPins < 1)
                        {
                            context.AddValidationMessage(new ValidationMessage("Violation of constraint C48.",
                                                                               "If the tagged value businessTransactionType of the BusinessTransaction is either Request/Response, Query/Response, Request/Confirm, or CommercialTransaction, then the RequestingBusinessAction must embed one to many RespondingInformationPins and the RespondingBusinessAction must embed one to many RespondingInformationPins.\n\nRequestingBusinessAction has invalid number of RespondingInformationPins: " +
                                                                               countPins, "BCV",
                                                                               ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                        }
                    }
                    else
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C48.",
                                                                           "If the tagged value businessTransactionType of the BusinessTransaction is either Request/Response, Query/Response, Request/Confirm, or CommercialTransaction, then the RequestingBusinessAction must embed one to many RespondingInformationPins and the RespondingBusinessAction must embed one to many RespondingInformationPins.\n\nUnable to find RequestingBusinessAction.",
                                                                           "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                           btv.PackageID));
                    }

                    //Get the Responding Action
                    EA.Element respondingAction = Utility.getElementFromPackage(btv, UMM.ResAction.ToString());

                    if (respondingAction != null)
                    {
                        //There must be one to many RespondingInformationPins
                        int countPins = 0;
                        foreach (EA.Element e in respondingAction.EmbeddedElements)
                        {
                            if (e.Type == "ActionPin" && e.Stereotype == UMM.ResInfPin.ToString())
                            {
                                countPins++;
                            }
                        }

                        if (countPins < 1)
                        {
                            context.AddValidationMessage(new ValidationMessage("Violation of constraint C48.",
                                                                               "If the tagged value businessTransactionType of the BusinessTransaction is either Request/Response, Query/Response, Request/Confirm, or CommercialTransaction, then the RequestingBusinessAction must embed one to many RespondingInformationPins and the RespondingBusinessAction must embed one to many RespondingInformationPins.\n\nRespondingBusinessAction has invalid number of RespondingInformationPins: " +
                                                                               countPins, "BCV",
                                                                               ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                        }
                    }
                    else
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C48.",
                                                                           "If the tagged value businessTransactionType of the BusinessTransaction is either Request/Response, Query/Response, Request/Confirm, or CommercialTransaction, then the RequestingBusinessAction must embed one to many RespondingInformationPins and the RespondingBusinessAction must embed one to many RespondingInformationPins.\n\nUnable to find RespondingBusinessAction.",
                                                                           "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                           btv.PackageID));
                    }
                }
            }
        }


        /// <summary>
        /// Check constraint C49
        /// </summary>
        /// <param name="context"></param>
        /// <param name="btv"></param>
        private void checkC49(IValidationContext context, EA.Package btv)
        {
            //Get the BusinessTransaction
            EA.Element bt = Utility.getElementFromPackage(btv, UMM.bTransaction.ToString());

            //No BusinessTransaction found
            if (bt == null)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C49.",
                                                                   "If the tagged value businessTransactionType of the BusinessTransaction is either Notification or InformationDistribution, then both, the RequestingBusinessAction and the RespondingBusinessAction, MUST NOT embed a RespondingInformationPin .\n\nBusinessTransaction could not be found.",
                                                                   "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
            }
            else
            {
                //Get the TaggedValue businessTransactionType
                bool found = false;
                foreach (EA.TaggedValue tvalue in bt.TaggedValues)
                {
                    if (tvalue.Name == "businessTransactionType")
                    {
                        //Check the value
                        if (tvalue.Value == "Notification" || tvalue.Value == "Information Distribution")
                        {
                            found = true;
                            break;
                        }
                    }
                }

                if (found)
                {
                    //Get the Requesting Action
                    EA.Element requestingAction = Utility.getElementFromPackage(btv, UMM.ReqAction.ToString());

                    if (requestingAction != null)
                    {
                        //There must not be any RespondingInformationPins
                        int countPins = 0;
                        foreach (EA.Element e in requestingAction.EmbeddedElements)
                        {
                            if (e.Type == "ActionPin" && e.Stereotype == UMM.ResInfPin.ToString())
                            {
                                countPins++;
                            }
                        }

                        if (countPins != 0)
                        {
                            context.AddValidationMessage(new ValidationMessage("Violation of constraint C49.",
                                                                               "If the tagged value businessTransactionType of the BusinessTransaction is either Notification or InformationDistribution, then both, the RequestingBusinessAction and the RespondingBusinessAction, MUST NOT embed a RespondingInformationPin .\n\nFound " +
                                                                               countPins +
                                                                               " RespondingInformationPins on the RequestingBusinessAction.",
                                                                               "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                               btv.PackageID));
                        }
                    }
                    else
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C49.",
                                                                           "If the tagged value businessTransactionType of the BusinessTransaction is either Notification or InformationDistribution, then both, the RequestingBusinessAction and the RespondingBusinessAction, MUST NOT embed a RespondingInformationPin .\n\nRequestingBusinessAction could not be found.",
                                                                           "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                           btv.PackageID));
                    }

                    //Get the Responding Action
                    EA.Element respondingAction = Utility.getElementFromPackage(btv, UMM.ResAction.ToString());

                    if (respondingAction != null)
                    {
                        //There must be one to many RespondingInformationPins
                        int countPins = 0;
                        foreach (EA.Element e in respondingAction.EmbeddedElements)
                        {
                            if (e.Type == "ActionPin" && e.Stereotype == UMM.ResInfPin.ToString())
                            {
                                countPins++;
                            }
                        }

                        if (countPins != 0)
                        {
                            context.AddValidationMessage(new ValidationMessage("Violation of constraint C49.",
                                                                               "If the tagged value businessTransactionType of the BusinessTransaction is either Notification or InformationDistribution, then both, the RequestingBusinessAction and the RespondingBusinessAction, MUST NOT embed a RespondingInformationPin .\n\nFound " +
                                                                               countPins +
                                                                               " RespondingInformationPins on the RespondingBusinessAction.",
                                                                               "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                               btv.PackageID));
                        }
                    }
                    else
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C49.",
                                                                           "If the tagged value businessTransactionType of the BusinessTransaction is either Notification or InformationDistribution, then both, the RequestingBusinessAction and the RespondingBusinessAction, MUST NOT embed a RespondingInformationPin .\n\nRespondingBusinessAction could not be found.",
                                                                           "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                           btv.PackageID));
                    }
                }
            }
        }


        /// <summary>
        /// Check constraint C50
        /// </summary>
        /// <param name="context"></param>
        /// <param name="btv"></param>
        private void checkC50(IValidationContext context, EA.Package btv)
        {
            //Get the BusinessTransaction
            EA.Element bt = Utility.getElementFromPackage(btv, UMM.bTransaction.ToString());

            //No BusinessTransaction found
            if (bt == null)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C50.",
                                                                   "A RequestingBusinessAction and a RespondingBusinessAction MUST embed same number of RespondingInformationPins. \n\nUnable to find BusinessTransaction.",
                                                                   "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
            }
            else
            {
                //Get the Requesting Action
                Element requestingAction = Utility.getElementFromPackage(btv, UMM.ReqAction.ToString());

                int countReqPins = 0;
                int countResPins = 0;

                if (requestingAction != null)
                {
                    foreach (EA.Element e in requestingAction.EmbeddedElements)
                    {
                        if (e.Type == "ActionPin" && e.Stereotype == UMM.ResInfPin.ToString())
                        {
                            countReqPins++;
                        }
                    }
                }
                else
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C50.",
                                                                       "A RequestingBusinessAction and a RespondingBusinessAction MUST embed same number of RespondingInformationPins. \n\nUnable to find RequestingBusinessAction.",
                                                                       "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                }

                //Get the Responding Action
                Element respondingAction = Utility.getElementFromPackage(btv, UMM.ResAction.ToString());

                if (respondingAction != null)
                {
                    foreach (EA.Element e in respondingAction.EmbeddedElements)
                    {
                        if (e.Type == "ActionPin" && e.Stereotype == UMM.ResInfPin.ToString())
                        {
                            countResPins++;
                        }
                    }
                }
                else
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C50.",
                                                                       "A RequestingBusinessAction and a RespondingBusinessAction MUST embed same number of RespondingInformationPins. \n\nUnable to find RespondingBusinessAction.",
                                                                       "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                }

                if (countReqPins != countResPins)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C50.",
                                                                       "A RequestingBusinessAction and a RespondingBusinessAction MUST embed same number of RespondingInformationPins. \n\nFound " +
                                                                       countReqPins +
                                                                       " RespondingInformationPins on the RequestingBusinessAction and " +
                                                                       countReqPins +
                                                                       " RespondingInformationPins on the RespondingBusinessAction.",
                                                                       "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                }
            }
        }


        /// <summary>
        /// Check constraint C51
        /// </summary>
        /// <param name="context"></param>
        /// <param name="btv"></param>
        private void checkC51(IValidationContext context, EA.Package btv)
        {
            //Get the BusinessTransaction
            EA.Element bt = Utility.getElementFromPackage(btv, UMM.bTransaction.ToString());

            //No BusinessTransaction found
            if (bt == null)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C51.",
                                                                   "he RequestingInformationPin of the RequestingBusinessAction MUST be connected with the RequestingInformationPin of the RespondingBusinessAction using an object flow relationship leading from the RequestingBusinessAction to the RespondingBusinessAction.\n\nUnable to find BusinessTransaction.",
                                                                   "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
            }
            else
            {
                //Get the Requesting Action
                Element requestingAction = Utility.getElementFromPackage(btv, UMM.ReqAction.ToString());
                EA.Element requestingInformationPin = null;


                if (requestingAction != null)
                {
                    foreach (EA.Element e in requestingAction.EmbeddedElements)
                    {
                        if (e.Type == "ActionPin" && e.Stereotype == UMM.ReqInfPin.ToString())
                        {
                            requestingInformationPin = e;
                            break;
                        }
                    }


                    if (requestingInformationPin != null)
                    {
                        bool found = false;
                        foreach (EA.Connector con in requestingInformationPin.Connectors)
                        {
                            if (con.Type == "ObjectFlow")
                            {
                                EA.Element supplier = context.Repository.GetElementByID(con.SupplierID);
                                if (supplier.Stereotype == UMM.ReqInfPin.ToString())
                                {
                                    //The parent of the supplier must be the RespondingBusinessAction
                                    EA.Element respondingAction = Utility.getElementFromPackage(btv,
                                                                                                UMM.ResAction.ToString());
                                    if (supplier.ParentID == respondingAction.ElementID)
                                    {
                                        found = true;
                                    }
                                }
                            }
                        }

                        if (!found)
                        {
                            context.AddValidationMessage(new ValidationMessage("Violation of constraint C51.",
                                                                               "The RequestingInformationPin of the RequestingBusinessAction MUST be connected with the RequestingInformationPin of the RespondingBusinessAction using an object flow relationship leading from the RequestingBusinessAction to the RespondingBusinessAction.\n\n",
                                                                               "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                               btv.PackageID));
                        }
                    }
                        //No Requesting Information Pin on the RequestingBusinessAction
                    else
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C51.",
                                                                           "The RequestingInformationPin of the RequestingBusinessAction MUST be connected with the RequestingInformationPin of the RespondingBusinessAction using an object flow relationship leading from the RequestingBusinessAction to the RespondingBusinessAction.\n\nUnable to find RequestingInformationPin on RequestingBusinessAction.",
                                                                           "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                           btv.PackageID));
                    }
                }
                else
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C51.",
                                                                       "The RequestingInformationPin of the RequestingBusinessAction MUST be connected with the RequestingInformationPin of the RespondingBusinessAction using an object flow relationship leading from the RequestingBusinessAction to the RespondingBusinessAction.\n\nUnable to find RequestingBusinessAction.",
                                                                       "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                }
            }

            return;
        }


        /// <summary>
        /// Check constraint C52
        /// </summary>
        /// <param name="context"></param>
        /// <param name="btv"></param>
        private void checkC52(IValidationContext context, EA.Package btv)
        {
            //Get the BusinessTransaction
            EA.Element bt = Utility.getElementFromPackage(btv, UMM.bTransaction.ToString());

            //No BusinessTransaction found
            if (bt == null)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C52.",
                                                                   "Each RespondingInformationPin of the RespondingBusinessAction MUST be connected with exactly one RespondingInformationPin of the RequestingBusinessAction using an object flow relationship leading from the RespondingBusinessAction to the RequestingBusinessAction.\n\nUnable to find BusinessTransaction.",
                                                                   "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
            }
            else
            {
                //Get the Responding Action
                Element respondingAction = Utility.getElementFromPackage(btv, UMM.ResAction.ToString());


                if (respondingAction != null)
                {
                    foreach (EA.Element e in respondingAction.EmbeddedElements)
                    {
                        if (e.Type == "ActionPin" && e.Stereotype == UMM.ResInfPin.ToString())
                        {
                            bool hasConnector = false;

                            foreach (EA.Connector con in e.Connectors)
                            {
                                if (con.Type == "ObjectFlow")
                                {
                                    EA.Element supplier = context.Repository.GetElementByID(con.SupplierID);
                                    if (supplier.Stereotype == UMM.ResInfPin.ToString())
                                    {
                                        //The parent of the supplier must be the RequestingBusinessAction
                                        EA.Element requestingAction = Utility.getElementFromPackage(btv,
                                                                                                    UMM.ReqAction.
                                                                                                        ToString());
                                        if (supplier.ParentID == requestingAction.ElementID)
                                        {
                                            hasConnector = true;
                                        }
                                    }
                                }
                            }

                            if (!hasConnector)
                            {
                                context.AddValidationMessage(new ValidationMessage("Violation of constraint C52.",
                                                                                   "Each RespondingInformationPin of the RespondingBusinessAction MUST be connected with exactly one RespondingInformationPin of the RequestingBusinessAction using an object flow relationship leading from the RespondingBusinessAction to the RequestingBusinessAction.",
                                                                                   "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                                   btv.PackageID));
                                break;
                            }
                        }
                    }
                }
                else
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C52.",
                                                                       "Each RespondingInformationPin of the RespondingBusinessAction MUST be connected with exactly one RespondingInformationPin of the RequestingBusinessAction using an object flow relationship leading from the RespondingBusinessAction to the RequestingBusinessAction.\n\nUnable to find RespondingBusinessAction.",
                                                                       "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                }
            }

            return;
        }


        /// <summary>
        /// Check constraint C53
        /// </summary>
        /// <param name="context"></param>
        /// <param name="btv"></param>
        private void checkC53(IValidationContext context, EA.Package btv)
        {
            List<EA.Element> partitions = new List<EA.Element>();

            //Get the partition containing the requesting action
            EA.Element requestingPartition = null;

            //Find the two partitions
            foreach (EA.Element e in btv.Elements)
            {
                if (e.Type == "ActivityPartition" && e.Stereotype == UMM.bTPartition.ToString())
                {
                    partitions.Add(e);
                }
            }

            foreach (EA.Element partition in partitions)
            {
                foreach (EA.Element e in btv.Elements)
                {
                    if (e.ParentID == partition.ElementID && e.Stereotype == UMM.ReqAction.ToString())
                    {
                        requestingPartition = partition;
                        break;
                    }
                }
            }

            if (requestingPartition == null)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C53.",
                                                                   "If a BusinessTransactionPartition contains SharedBusinessEntityStates, each SharedBusinessEntityState MUST be the target of exactly one control flow relationship starting from the RequestingBusinessAction and MUST be the source of exactly one control flow relationship targeting a FinalState. \n\nUnable to find RequestingBusinessTransactionPartition.",
                                                                   "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
            }
            else
            {
                //Get the SharedBusinessEntityStates
                IList<EA.Element> sharedStates = Utility.getAllElements(btv, new List<EA.Element>(),
                                                                        UMM.bESharedState.ToString());

                foreach (EA.Element e in sharedStates)
                {
                    //The shared entity state must be located in the requesting partition
                    if (e.ParentID == requestingPartition.ElementID)
                    {
                        int countConnectionsTo = 0;
                        int countConnectionsFrom = 0;

                        //Check if there is exactly one connector leading from a requesting business action
                        //to the besharedstate
                        foreach (EA.Connector con in e.Connectors)
                        {
                            //Get the client which MUST be a requesting business action

                            EA.Element client = context.Repository.GetElementByID(con.ClientID);
                            EA.Element supplier = context.Repository.GetElementByID(con.SupplierID);

                            //Count the connections from requesting business actions
                            if (client.Stereotype == UMM.ReqAction.ToString())
                            {
                                countConnectionsTo++;
                            }

                            //Count the connections to final states
                            if (supplier.Subtype == 101)
                            {
                                countConnectionsFrom++;
                            }
                        }
                        if (countConnectionsTo != 1)
                        {
                            String sharedBusinessEntityStateName = "undefined";
                            if (e.ClassfierID != 0)
                            {
                                sharedBusinessEntityStateName = context.Repository.GetElementByID(e.ClassifierID).Name;
                            }

                            context.AddValidationMessage(new ValidationMessage("Violation of constraint C53.",
                                                                               "If a BusinessTransactionPartition contains SharedBusinessEntityStates, each SharedBusinessEntityState MUST be the target of exactly one control flow relationship starting from the RequestingBusinessAction and MUST be the source of exactly one control flow relationship targeting a FinalState. \n\nFound " +
                                                                               countConnectionsTo +
                                                                               " ControlFlows leading from the RequestingBusinessAction to SharedBusinessEntityState " +
                                                                               sharedBusinessEntityStateName, "BCV",
                                                                               ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                        }

                        if (countConnectionsFrom != 1)
                        {
                            String sharedBusinessEntityStateName = "undefined";
                            if (e.ClassfierID != 0)
                            {
                                sharedBusinessEntityStateName = context.Repository.GetElementByID(e.ClassifierID).Name;
                            }

                            context.AddValidationMessage(new ValidationMessage("Violation of constraint C53.",
                                                                               "If a BusinessTransactionPartition contains SharedBusinessEntityStates, each SharedBusinessEntityState MUST be the target of exactly one control flow relationship starting from the RequestingBusinessAction and MUST be the source of exactly one control flow relationship targeting a FinalState. \n\nSharedBusinessEntityState " +
                                                                               sharedBusinessEntityStateName +
                                                                               " does not have a connection to a final state.", "BCV",
                                                                               ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Check constraint C54
        /// </summary>
        /// <param name="context"></param>
        /// <param name="btv"></param>
        private void checkC54(IValidationContext context, EA.Package btv)
        {
            List<EA.Element> finalStates = new List<EA.Element>();

            //Get all final states
            foreach (EA.Element e in btv.Elements)
            {
                if (e.Subtype == 101)
                {
                    finalStates.Add(e);
                }
            }


            foreach (EA.Element finalState in finalStates)
            {
                int countConnections = 0;

                foreach (EA.Connector con in finalState.Connectors)
                {
                    //Get the client of the connection
                    EA.Element client = context.Repository.GetElementByID(con.ClientID);
                    if (client.Stereotype == UMM.ReqAction.ToString() ||
                        client.Stereotype == UMM.bESharedState.ToString())
                    {
                        countConnections++;
                    }
                }

                if (countConnections < 1)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C54.",
                                                                       "Each FinalState MUST be the target of one to many control flow relationships starting from the RequestingBusinessAction or from a SharedBusinessEntityState. \n\nUnable to find a connection.",
                                                                       "BCV", ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                }
            }
        }


        /// <summary>
        /// Check constraint C55
        /// </summary>
        /// <param name="context"></param>
        /// <param name="btv"></param>
        private void checkC55(IValidationContext context, EA.Package btv)
        {
            //Get the RequestingInformationPins
            IList<EA.Element> reqInfPins = Utility.getAllElements(btv, new List<EA.Element>(), UMM.ReqInfPin.ToString());
            //Each Pin must have a classifier which MUST be an InformationEnveleope or a subtype thereof

            EA.Element classifier;

            foreach (EA.Element e in reqInfPins)
            {
                bool classifierFound = false;
                classifier = null;

                if (e.ClassifierID != 0)
                    classifier = context.Repository.GetElementByID(e.ClassifierID);

                if (classifier != null)
                {
                    //Is the classifier of type InformationEnvelope?
                    if (classifier.Stereotype == UMM.InfEnvelope.ToString())
                    {
                        classifierFound = true;
                    }
                        //Is the classifier a subtype of an InformationEnvelope?
                    else
                    {
                        foreach (EA.Element el in classifier.BaseClasses)
                        {
                            if (el.Stereotype == UMM.InfEnvelope.ToString())
                            {
                                classifierFound = true;
                                break;
                            }
                        }
                    }
                }

                //Classifier found?
                if (!classifierFound)
                {
                    String errorneousClassifier = "";
                    if (classifier != null)
                        errorneousClassifier = classifier.Name;

                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C55.",
                                                                       "Each RequestingInformationPin and each RespondingInformationPin MUST have a classifier, this classifier MUST be an InformationEnvelope or a subtype defined in an extension/specialization module. \n\nUnable to find a classifier for a RequestingInformationPin on the element " +
                                                                       context.Repository.GetElementByID(e.ParentID).Name +
                                                                       " or classifier is invalid. Make sure that the InformationPins have the appropriate classifiers and that classifiers have the appropriate stereotype (InfEnvelope or subtypes thereof). Invalid classifier: " +
                                                                       errorneousClassifier, "BCV",
                                                                       ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                }
            }


            //Get the RespondingInformationPins
            IList<EA.Element> resInfPins = Utility.getAllElements(btv, new List<EA.Element>(), UMM.ResInfPin.ToString());
            //Each Pin must have a classifier which MUST be an InformationEnveleope or a subtype thereof
            foreach (EA.Element e in resInfPins)
            {
                bool classifierFound = false;
                classifier = null;

                if (e.ClassifierID != 0)
                    classifier = context.Repository.GetElementByID(e.ClassifierID);

                if (classifier != null)
                {
                    //Is the classifier of type InformationEnvelope?
                    if (classifier.Stereotype == UMM.InfEnvelope.ToString())
                    {
                        classifierFound = true;
                    }
                        //Is the classifier a subtype of an InformationEnvelope?
                    else
                    {
                        foreach (EA.Element el in classifier.BaseClasses)
                        {
                            if (el.Stereotype == UMM.InfEnvelope.ToString())
                            {
                                classifierFound = true;
                                break;
                            }
                        }
                    }
                }

                //Classifier found?
                if (!classifierFound)
                {
                    String errorneousClassifier = "";
                    if (classifier != null)
                        errorneousClassifier = classifier.Name;

                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C55.",
                                                                       "Each RequestingInformationPin and each RespondingInformationPin MUST have a classifier, this classifier MUST be an InformationEnvelope or a subtype defined in an extension/specialization module. \n\nUnable to find a classifier for a RespondingInformationPin on the element " +
                                                                       context.Repository.GetElementByID(e.ParentID) +
                                                                       ".Name Make sure that the InformationPins have the appropriate classifiers and that classifiers have the appropriate stereotype (InfEnvelope or subtypes thereof). Invalid classifier: " +
                                                                       errorneousClassifier, "BCV",
                                                                       ValidationMessage.errorLevelTypes.ERROR, btv.PackageID));
                }
            }
        }


        /// <summary>
        /// Check constraint C56
        /// </summary>
        /// <param name="context"></param>
        /// <param name="btv"></param>
        private void checkC56(IValidationContext context, EA.Package btv)
        {
            //Get the RequestingInformationPins
            IList<EA.Element> reqInfPins = Utility.getAllElements(btv, new List<EA.Element>(), UMM.ReqInfPin.ToString());

            List<int> checkedElement = new List<int>();

            //Each reqInfPin must be connected to another RequestingInformationPin using an object flow and
            //the classifiers of both reqinfpins must be the same
            foreach (EA.Element e in reqInfPins)
            {
                foreach (EA.Connector con in e.Connectors)
                {
                    if (con.Type == "ObjectFlow")
                    {
                        if (!checkedElement.Contains(con.ConnectorID))
                        {
                            //This pin is a client of an object flow
                            if (con.ClientID == e.ElementID)
                            {
                                EA.Element supplier = context.Repository.GetElementByID(con.SupplierID);
                                if (supplier.ClassifierID != e.ClassifierID)
                                {
                                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C56.",
                                                                                       "Two RequestingInformationPins which are connected using an object flow MUST have the same classifier.",
                                                                                       "BDV", ValidationMessage.errorLevelTypes.ERROR,
                                                                                       btv.PackageID));
                                }
                            }
                                //This pin is the supplier of an object flow
                            else
                            {
                                EA.Element client = context.Repository.GetElementByID(con.ClientID);
                                if (client.ClassifierID != e.ClassifierID)
                                {
                                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C56.",
                                                                                       "Two RequestingInformationPins which are connected using an object flow MUST have the same classifier.",
                                                                                       "BDV", ValidationMessage.errorLevelTypes.ERROR,
                                                                                       btv.PackageID));
                                }
                            }
                            checkedElement.Add(con.ConnectorID);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Check constraint C57
        /// </summary>
        /// <param name="context"></param>
        /// <param name="btv"></param>
        private void checkC57(IValidationContext context, EA.Package btv)
        {
            //Get the RespondingInformationPins
            IList<EA.Element> resInfPins = Utility.getAllElements(btv, new List<EA.Element>(), UMM.ResInfPin.ToString());

            List<int> checkedElement = new List<int>();

            //Each resInfPin must be connected to another RespondingInformationPin using an object flow and
            //the classifiers of both resinfpins must be the same
            foreach (EA.Element e in resInfPins)
            {
                foreach (EA.Connector con in e.Connectors)
                {
                    if (con.Type == "ObjectFlow")
                    {
                        if (!checkedElement.Contains(con.ConnectorID))
                        {
                            //This pin is a client of an object flow
                            if (con.ClientID == e.ElementID)
                            {
                                EA.Element supplier = context.Repository.GetElementByID(con.SupplierID);
                                if (supplier.ClassifierID != e.ClassifierID)
                                {
                                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C57.",
                                                                                       "Two RespondingInformationPins which are connected using an object flow MUST have the same classifier.",
                                                                                       "BDV", ValidationMessage.errorLevelTypes.ERROR,
                                                                                       btv.PackageID));
                                }
                            }
                                //This pin is the supplier of an object flow
                            else
                            {
                                EA.Element client = context.Repository.GetElementByID(con.ClientID);
                                if (client.ClassifierID != e.ClassifierID)
                                {
                                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C57.",
                                                                                       "Two RespondingInformationPins which are connected using an object flow MUST have the same classifier.",
                                                                                       "BDV", ValidationMessage.errorLevelTypes.ERROR,
                                                                                       btv.PackageID));
                                }
                            }
                            checkedElement.Add(con.ConnectorID);
                        }
                    }
                }
            }
            return;
        }


        /// <summary>
        /// Get the two Authorized Roles associated with a business transaction use case
        /// </summary>
        /// <param name="context"></param>
        /// <param name="btuc"></param>
        /// <returns></returns>
        private List<Element> getAuthorizedRolesFromBTUC(IValidationContext context, Element btuc)
        {
            List<EA.Element> authorizedRoles = new List<EA.Element>();

            //Make sure, that the BTUC is associated with exactly two authorized roles via participates assocations
            foreach (EA.Connector con in btuc.Connectors)
            {
                //Only associations and dependencies are allowed
                if (con.Stereotype == UMM.participates.ToString())
                {
                    //Correct connection leading from an AuthorizedRole to a business transaction use case
                    if (con.SupplierID == btuc.ElementID)
                    {
                        //Client must be of type Authorized role
                        EA.Element client = context.Repository.GetElementByID(con.ClientID);

                        if (client.Stereotype == UMM.AuthorizedRole.ToString())
                        {
                            authorizedRoles.Add(client);
                        }
                    }
                }
            }
            return authorizedRoles;
        }


        /// <summary>
        /// Check the tagged values of the BusinessTransactionView
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        private void checkTV_BusinessTransactionView(IValidationContext context, EA.Package p)
        {
            //Check the TaggedValues of the BusinessTransactionView package
            new TaggedValueValidator().validatePackage(context, p);

            //Check the TaggedValues of the BusinessTransactionUseCases
            IList<EA.Element> btucs = Utility.getAllElements(p, new List<EA.Element>(), UMM.bTransactionUC.ToString());
            foreach (EA.Element btuc in btucs)
            {
                new TaggedValueValidator().validateElement(context, btuc);
            }

            //Check the tagged values ot the bTransactions
            IList<EA.Element> bts = Utility.getAllElements(p, new List<EA.Element>(), UMM.bTransaction.ToString());
            foreach (EA.Element bt in bts)
            {
                new TaggedValueValidator().validateElement(context, bt);
            }

            //Check the tagged value of the RequestingActions and RespondingActions
            IList<EA.Element> reqActions = Utility.getAllElements(p, new List<EA.Element>(), UMM.ReqAction.ToString());
            IList<EA.Element> resActions = Utility.getAllElements(p, new List<EA.Element>(), UMM.ResAction.ToString());
            foreach (EA.Element reqAction in reqActions)
            {
                new TaggedValueValidator().validateElement(context, reqAction);
            }
            foreach (EA.Element resAction in resActions)
            {
                new TaggedValueValidator().validateElement(context, resAction);
            }


            //Get the Requesting/Responding Action
            Element requestingAction = Utility.getElementFromPackage(p, UMM.ReqAction.ToString());
            Element respondingAction = Utility.getElementFromPackage(p, UMM.ResAction.ToString());

            if (requestingAction != null)
            {
                foreach (EA.Element el in requestingAction.EmbeddedElements)
                {
                    if (el.Type == "ActionPin" &&
                        (el.Stereotype == UMM.ReqInfPin.ToString() || el.Stereotype == UMM.ResInfPin.ToString()))
                    {
                        new TaggedValueValidator().validateElement(context, el);
                    }
                }
            }

            if (respondingAction != null)
            {
                foreach (EA.Element el in respondingAction.EmbeddedElements)
                {
                    if (el.Type == "ActionPin" &&
                        (el.Stereotype == UMM.ResInfPin.ToString() || el.Stereotype == UMM.ReqInfPin.ToString()))
                    {
                        new TaggedValueValidator().validateElement(context, el);
                    }
                }
            }
        }
    }
}