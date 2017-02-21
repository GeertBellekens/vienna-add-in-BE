/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.common;
using VIENNAAddIn.constants;

namespace VIENNAAddIn.validator.umm.bcv
{
    internal class bCollaborationVValidator : AbstractValidator
    {
        /// <summary>
        /// Validate the Business Collaboration View
        /// </summary>
        internal override void validate(IValidationContext parentContext, string scope)
        {
            var context = new ErrorCheckingValidationContext(parentContext);

            Package bcv = context.Repository.GetPackageByID(Int32.Parse(scope));

            checkTV_BusinessCollaborationView(context, bcv);
            checkC58(context, bcv);
            checkC59(context, bcv);
            checkC60(context, bcv);
            checkC61(context, bcv);
            checkC62(context, bcv);
            checkC63(context, bcv);
            if (context.HasError) return;
            checkC64(context, bcv);
            if (context.HasError) return;
            checkC65(context, bcv);
            if (context.HasError) return;
            checkC66(context, bcv);
            checkC67(context, bcv);
            checkC68(context, bcv);
            if (context.HasError) return;
            checkC69(context, bcv);
            if (context.HasError) return;
            checkC70(context, bcv);
            checkC71(context, bcv);
            checkC72(context, bcv);
            checkC73(context, bcv);
            checkC74(context, bcv);
            if (context.HasError) return;
            checkC75(context, bcv);
            checkC76(context, bcv);
            checkC77(context, bcv);
            if (context.HasError) return;
            checkC78(context, bcv);
            checkC79(context, bcv);
            if (context.HasError) return;
            checkC80(context, bcv);
            checkC81(context, bcv);
            checkC82(context, bcv);
            checkC83(context, bcv);
            checkC84(context, bcv);
            checkC85(context, bcv);
            checkC86(context, bcv);
            checkC87(context, bcv);
            checkC88(context, bcv);
            checkC89(context, bcv);
            checkC90(context, bcv);
            if (context.HasError) return;
            checkC91(context, bcv);
        }


        /// <summary>
        /// Check constraint c58
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bcv"></param>
        private void checkC58(IValidationContext context, Package bcv)
        {
            int count_BCUC = 0;

            foreach (Element e in bcv.Elements)
            {
                if (e.Stereotype == UMM.bCollaborationUC.ToString())
                {
                    count_BCUC++;
                }
            }

            if (count_BCUC != 1)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C58.",
                                                                   "A BusinessCollaborationView MUST contain exactly one BusinessCollaborationUseCase. \n\nFound " +
                                                                   count_BCUC + " BusinessCollaborationUseCases.", "BCV",
                                                                   ValidationMessage.errorLevelTypes.ERROR,
                                                                   bcv.PackageID));
            }
        }


        /// <summary>
        /// Check constraint c59
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bcv"></param>
        private void checkC59(IValidationContext context, Package bcv)
        {
            int count_AR = 0;

            foreach (Element e in bcv.Elements)
            {
                if (e.Stereotype == UMM.AuthorizedRole.ToString())
                {
                    count_AR++;
                }
            }

            if (count_AR < 2)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C59.",
                                                                   "A BusinessCollaborationView MUST contain two to many AuthorizedRoles. \n\nFound " +
                                                                   count_AR + " Authorized Roles.", "BCV",
                                                                   ValidationMessage.errorLevelTypes.ERROR,
                                                                   bcv.PackageID));
            }
        }


        /// <summary>
        /// Check constraint C60
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bcv"></param>
        private void checkC60(IValidationContext context, Package bcv)
        {
            Element bcuc = Utility.getElementFromPackage(bcv, UMM.bCollaborationUC.ToString());

            if (bcuc == null)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C60.",
                                                                   "A BusinessCollaborationUseCase MUST have two to many participates associations to AuthorizedRoles contained in the same BusinessCollaborationView. \n\nUnable to find BusinessCollaborationUseCase.",
                                                                   "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                   bcv.PackageID));
            }
            else
            {
                int count_ParticipatesAssocations = 0;
                //Iterate of the different assocations of the BCUC
                foreach (Connector con in bcuc.Connectors)
                {
                    if (con.Type == "Association" && con.Stereotype == UMM.participates.ToString())
                    {
                        //The client must be an AuthorizedRole
                        Element client = context.Repository.GetElementByID(con.ClientID);

                        if (client.Stereotype != UMM.AuthorizedRole.ToString())
                        {
                            context.AddValidationMessage(new ValidationMessage("Violation of constraint C60.",
                                                                               "A BusinessCollaborationUseCase MUST have two to many participates associations to AuthorizedRoles contained in the same BusinessCollaborationView. \n\nFound a participates assocation which is either wrong-directed (it must lead from an Authorized Role to a Businss Collaboration Use Case) or connected to a wrong element. Wrong source element of the participates assocation: " +
                                                                               client.Name, "BCV",
                                                                               ValidationMessage.errorLevelTypes.ERROR,
                                                                               bcv.PackageID));
                        }
                            //Found an Authorized Role - is the Authorized Role from the Business Collaboration View
                        else if (client.PackageID != bcv.PackageID)
                        {
                            context.AddValidationMessage(new ValidationMessage("Violation of constraint C60.",
                                                                               "A BusinessCollaborationUseCase MUST have two to many participates associations to AuthorizedRoles contained in the same BusinessCollaborationView. \n\nThe Authorized Role " +
                                                                               client.Name +
                                                                               " of the BusinessCollaborationUseCase " +
                                                                               bcuc.Name +
                                                                               "is from a different BusinessCollaborationView.",
                                                                               "BCV",
                                                                               ValidationMessage.errorLevelTypes.ERROR,
                                                                               bcv.PackageID));
                        }
                        count_ParticipatesAssocations++;
                    }
                }

                if (count_ParticipatesAssocations < 2)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C60.",
                                                                       "A BusinessCollaborationUseCase MUST have two to many participates associations to AuthorizedRoles contained in the same BusinessCollaborationView. \n\nInvalid number of participates assocations found: " +
                                                                       count_ParticipatesAssocations, "BCV",
                                                                       ValidationMessage.errorLevelTypes.ERROR,
                                                                       bcv.PackageID));
                }
            }
        }


        /// <summary>
        /// Check constraint C61
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bcv"></param>
        private void checkC61(IValidationContext context, Package bcv)
        {
            Element bcuc = Utility.getElementFromPackage(bcv, UMM.bCollaborationUC.ToString());

            if (bcuc == null)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C61.",
                                                                   "Each AuthorizedRole contained in the BusinessCollaborationView MUST have exactly one participates association to the BusinessCollaborationUseCase included in the same BusinessCollaborationView.\n\nUnable to find BusinessCollaborationUseCase.",
                                                                   "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                   bcv.PackageID));
            }
            else
            {
                //Get the Authorized Roles
                IList<Element> ars = Utility.getAllElements(bcv, new List<Element>(),
                                                            UMM.AuthorizedRole.ToString());

                if (ars != null)
                {
                    foreach (Element ar in ars)
                    {
                        int countAssocations = 0;

                        //Iterate of the connectors of the AR and get the participates associations
                        foreach (Connector con in ar.Connectors)
                        {
                            if (con.Type == "Association" && con.Stereotype == UMM.participates.ToString())
                            {
                                Element client = context.Repository.GetElementByID(con.ClientID);
                                Element supplier = context.Repository.GetElementByID(con.SupplierID);

                                //The client must be of type Authorized Role
                                if (client.Stereotype != UMM.AuthorizedRole.ToString())
                                {
                                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C61.",
                                                                                       "Each AuthorizedRole contained in the BusinessCollaborationView MUST have exactly one participates association to the BusinessCollaborationUseCase included in the same BusinessCollaborationView.\n\nMissing assocation for AuthorizedRole " +
                                                                                       ar.Name +
                                                                                       ". Make sure, that the participates assocation has the correct direction (from the AuthorizedRole to the BusinessCollaborationUseCase).",
                                                                                       "BCV",
                                                                                       ValidationMessage.errorLevelTypes
                                                                                           .ERROR, bcv.PackageID));
                                }
                                else if (supplier.ElementID != bcuc.ElementID)
                                {
                                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C61.",
                                                                                       "Each AuthorizedRole contained in the BusinessCollaborationView MUST have exactly one participates association to the BusinessCollaborationUseCase included in the same BusinessCollaborationView.\n\nAssocation of AuthorizedRole " +
                                                                                       ar.Name +
                                                                                       " is leading to a BusinessCollaborationUseCase from a different BusinessCollaborationView.",
                                                                                       "BCV",
                                                                                       ValidationMessage.errorLevelTypes
                                                                                           .ERROR, bcv.PackageID));
                                }
                                    //AR and BCUC must be in the same package
                                else
                                {
                                    if (ar.PackageID != bcuc.PackageID)
                                    {
                                        context.AddValidationMessage(
                                            new ValidationMessage("Violation of constraint C61.",
                                                                  "Each AuthorizedRole contained in the BusinessCollaborationView MUST have exactly one participates association to the BusinessCollaborationUseCase included in the same BusinessCollaborationView.\n\nAuthorizedRole and BusinessCollaborationUseCase must be in the same package. AuthorizedRole " +
                                                                  ar.Name + " is contained in package " +
                                                                  context.Repository.GetPackageByID(ar.PackageID).Name +
                                                                  " and BusinessCollaborationUseCase" + bcuc.Name +
                                                                  " is contained in package " + bcv.Name + ".", "BCV",
                                                                  ValidationMessage.errorLevelTypes.ERROR, bcv.PackageID));
                                    }
                                }
                                countAssocations++;
                            }
                        }

                        //Each Authorized Role muast have exactly one participates assocation
                        if (countAssocations != 1)
                        {
                            context.AddValidationMessage(new ValidationMessage("Violation of constraint C61.",
                                                                               "Each AuthorizedRole contained in the BusinessCollaborationView MUST have exactly one participates association to the BusinessCollaborationUseCase included in the same BusinessCollaborationView.\n\nAuthorizedRole " +
                                                                               ar.Name +
                                                                               " has an invalid number of participates assocations: " +
                                                                               countAssocations, "BCV",
                                                                               ValidationMessage.errorLevelTypes.ERROR,
                                                                               bcv.PackageID));
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Check constraint C62
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bcv"></param>
        private void checkC62(IValidationContext context, Package bcv)
        {
            Element bcuc = Utility.getElementFromPackage(bcv, UMM.bCollaborationUC.ToString());

            if (bcuc == null)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C62.",
                                                                   "A BusinessCollaborationUseCase MUST have one to many include relationships to another BusinessCollaborationUseCase or to a BusinessTransactionUseCase.\n\nUnable to find BusinessCollaborationUseCase.",
                                                                   "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                   bcv.PackageID));
            }
            else
            {
                int countIncludes = 0;
                foreach (Connector con in bcuc.Connectors)
                {
                    if (con.Stereotype == UMM.include.ToString())
                    {
                        Element supplier = context.Repository.GetElementByID(con.SupplierID);
                        if (supplier.Stereotype == UMM.bTransactionUC.ToString())
                        {
                            countIncludes++;
                        }
                        else if (supplier.Stereotype == UMM.bCollaborationUC.ToString())
                        {
                            countIncludes++;
                        }
                    }
                }

                //There must be one to many includes
                if (countIncludes < 1)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C62.",
                                                                       "A BusinessCollaborationUseCase MUST have one to many include relationships to another BusinessCollaborationUseCase or to a BusinessTransactionUseCase.\n\nFound " +
                                                                       countIncludes + " include relationships.", "BCV",
                                                                       ValidationMessage.errorLevelTypes.ERROR,
                                                                       bcv.PackageID));
                }
            }
        }


        /// <summary>
        /// Check constraint C63
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bcv"></param>
        private void checkC63(IValidationContext context, Package bcv)
        {
            Element bcuc = Utility.getElementFromPackage(bcv, UMM.bCollaborationUC.ToString());

            if (bcuc == null)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C63.",
                                                                   "Exactly one BusinessCollaborationProtocol MUST be placed beneath each BusinessCollaborationUseCase. \n\nUable to find BusinessCollaborationUseCase.",
                                                                   "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                   bcv.PackageID));
            }
            else
            {
                int countProtocols = 0;
                //Get the number of BusinessCollaborationProtocols
                foreach (Element e in bcv.Elements)
                {
                    if (e.Stereotype == UMM.bCollaborationProtocol.ToString() && e.ParentID == bcuc.ElementID)
                    {
                        countProtocols++;
                    }
                }

                if (countProtocols != 1)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C63.",
                                                                       "Exactly one BusinessCollaborationProtocol MUST be placed beneath each BusinessCollaborationUseCase. \n\nFound " +
                                                                       countProtocols +
                                                                       " BusinessCollaborationProtocols.", "BCV",
                                                                       ValidationMessage.errorLevelTypes.ERROR,
                                                                       bcv.PackageID));
                }
            }
        }


        /// <summary>
        /// Check constraint C64
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bcv"></param>
        private void checkC64(IValidationContext context, Package bcv)
        {
            //Get the BusinessCollaborationProtocol
            Element bcpr = Utility.getElementFromPackage(bcv, UMM.bCollaborationProtocol.ToString());
            if (bcpr == null)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C64.",
                                                                   "A BusinessCollaborationProtocol MUST contain one to many BusinessTransactionActions and/or BusinessCollaborationAction.\n\nUnable to find BusinessCollaborationProtocol.",
                                                                   "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                   bcv.PackageID));
            }
            else
            {
                int countAction = 0;
                //Iterate over the elements and get all subelements of the business collaboration protocol
                foreach (Element el in bcv.Elements)
                {
                    //Count the included BusinessCollaborationActions and BusinessTransactionActions
                    if (el.ParentID == bcpr.ElementID)
                    {
                        if (el.Stereotype == UMM.bCollaborationAction.ToString() ||
                            el.Stereotype == UMM.bTransactionAction.ToString())
                        {
                            countAction++;
                        }
                    }
                }
                if (countAction < 1)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C64.",
                                                                       "A BusinessCollaborationProtocol MUST contain one to many BusinessTransactionActions and/or BusinessCollaborationAction.\n\nNo Actions found.",
                                                                       "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                       bcv.PackageID));
                }
            }
        }


        /// <summary>
        /// Check constraint C65
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bcv"></param>
        private void checkC65(IValidationContext context, Package bcv)
        {
            //Get all BusinessTransactionActions
            IList<Element> btas = Utility.getAllElements(bcv, new List<Element>(),
                                                         UMM.bTransactionAction.ToString());

            if (btas != null && btas.Count != 0)
            {
                foreach (Element bta in btas)
                {
                    Element classifier = null;

                    if (bta.ClassifierID != 0)
                        classifier = context.Repository.GetElementByID(bta.ClassifierID);

                    if (classifier == null || classifier.Stereotype != UMM.bTransaction.ToString())
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C65.",
                                                                           "Each BusinessTransactionAction MUST call exactly one BusinessTransaction\n\nBusinessTransactionAction " +
                                                                           bta.Name +
                                                                           " does not call a BusinessTransaction.",
                                                                           "BCV",
                                                                           ValidationMessage.errorLevelTypes.ERROR,
                                                                           bcv.PackageID));
                    }
                }
            }
            else
            {
                context.AddValidationMessage(new ValidationMessage("Info for constraint C65.",
                                                                   "Each BusinessTransactionAction MUST call exactly one BusinessTransaction\n\nNo BusinessTransactionActions found.",
                                                                   "BCV", ValidationMessage.errorLevelTypes.INFO,
                                                                   bcv.PackageID));
            }
        }


        /// <summary>
        /// Check constraint C66
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bcv"></param>
        private void checkC66(IValidationContext context, Package bcv)
        {
            //Get all BusinessTransactionActions
            IList<Element> btas = Utility.getAllElements(bcv, new List<Element>(),
                                                         UMM.bTransactionAction.ToString());

            if (btas != null && btas.Count != 0)
            {
                foreach (Element bta in btas)
                {
                    Element classifier = null;

                    if (bta.ClassifierID != 0)
                        classifier = context.Repository.GetElementByID(bta.ClassifierID);

                    //No Business Transaction found
                    if (classifier == null || classifier.Stereotype != UMM.bTransaction.ToString())
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C66.",
                                                                           "Each BusinessTransaction called by a BusinessTransactionAction MUST be placed beneath a BusinessTransactionUseCase which is included in the BusinessCollaborationUseCase that covers the corresponding BusinessCollaborationProtocol. \n\nUnable to find BusinessTransaction for BusinessTransactionAction " +
                                                                           bta.Name, "BCV",
                                                                           ValidationMessage.errorLevelTypes.ERROR,
                                                                           bcv.PackageID));
                    }
                    else
                    {
                        //Get the business transaction use case under which the business transaction is placed
                        Package btv = context.Repository.GetPackageByID(classifier.PackageID);
                        Element btuc = null;
                        foreach (Element e in btv.Elements)
                        {
                            if (e.Stereotype == UMM.bTransactionUC.ToString())
                            {
                                btuc = e;
                                break;
                            }
                        }

                        //No Business Transaction Use Case found
                        if (btuc == null)
                        {
                            context.AddValidationMessage(new ValidationMessage("Violation of constraint C66.",
                                                                               "Each BusinessTransaction called by a BusinessTransactionAction MUST be placed beneath a BusinessTransactionUseCase which is included in the BusinessCollaborationUseCase that covers the corresponding BusinessCollaborationProtocol. \n\nUnable to find BusinessTransactionUseCase for BusinessTransactionAction " +
                                                                               bta.Name, "BCV",
                                                                               ValidationMessage.errorLevelTypes.ERROR,
                                                                               bcv.PackageID));
                        }
                        else
                        {
                            //Get the BusinessCollaborationUseCase of this package
                            Element bcuc = Utility.getElementFromPackage(bcv, UMM.bCollaborationUC.ToString());
                            if (bcuc == null)
                            {
                                context.AddValidationMessage(new ValidationMessage("Violation of constraint C66.",
                                                                                   "Each BusinessTransaction called by a BusinessTransactionAction MUST be placed beneath a BusinessTransactionUseCase which is included in the BusinessCollaborationUseCase that covers the corresponding BusinessCollaborationProtocol. \n\nUnable to find BusinessCollaborationUseCase in which the BusinessTransactionUseCase of the BusinessTransaction " +
                                                                                   bta.Name + " should be included.",
                                                                                   "BCV",
                                                                                   ValidationMessage.errorLevelTypes.
                                                                                       ERROR, bcv.PackageID));
                            }
                            else
                            {
                                //Does the BusinessCollaborationUseCase include the BTUC
                                bool found = false;
                                foreach (Connector con in bcuc.Connectors)
                                {
                                    if (con.Stereotype == UMM.include.ToString())
                                    {
                                        Element client = context.Repository.GetElementByID(con.ClientID);
                                        Element supplier = context.Repository.GetElementByID(con.SupplierID);

                                        if (client.ElementID == bcuc.ElementID &&
                                            supplier.ElementID == btuc.ElementID)
                                        {
                                            found = true;
                                        }
                                    }
                                }

                                if (!found)
                                {
                                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C66.",
                                                                                       "Each BusinessTransaction called by a BusinessTransactionAction MUST be placed beneath a BusinessTransactionUseCase which is included in the BusinessCollaborationUseCase that covers the corresponding BusinessCollaborationProtocol. \n\nThe BusinessTransactionUseCase " +
                                                                                       btuc.Name +
                                                                                       " is not included by the BusinessCollaborationUseCase " +
                                                                                       bcuc.Name +
                                                                                       ". If you want to use a BusinessTransaction because it is called by a BusinessTransactionAction in a BusinessCollaborationView, the following requirement has to be met: \n1. The BusinessTransactionUseCase under which the BusinessTransaction is placed MUST be included by the BusinessCollaborationUseCase of the BusinessCollaborationView where you want to use the BusinessTransaction.",
                                                                                       "BCV",
                                                                                       ValidationMessage.errorLevelTypes
                                                                                           .ERROR, bcv.PackageID));
                                }
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Check constraint C67
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bcv"></param>
        private void checkC67(IValidationContext context, Package bcv)
        {
            //Get all BusinessCollaborationActions
            IList<Element> bcas = Utility.getAllElements(bcv, new List<Element>(),
                                                         UMM.bCollaborationAction.ToString());

            if (bcas != null && bcas.Count != 0)
            {
                foreach (Element bca in bcas)
                {
                    Element classifier = null;

                    if (bca.ClassifierID != 0)
                        classifier = context.Repository.GetElementByID(bca.ClassifierID);

                    //No Business CollaborationProtocol found
                    if (classifier == null || classifier.Stereotype != UMM.bCollaborationProtocol.ToString())
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C67.",
                                                                           "Each BusinessCollaborationProtocol called by a BusinessCollaborationAction MUST be placed beneath a BusinessCollaborationProtocolUseCase which is included in the BusinessCollaborationUseCase that covers the corresponding BusinessCollaborationProtocol.\n\nUnable to find BusinessCollaborationProtocol for BusinessCollaborationAction " +
                                                                           bca.Name, "BCV",
                                                                           ValidationMessage.errorLevelTypes.ERROR,
                                                                           bcv.PackageID));
                    }
                    else
                    {
                        //Get the business collaboration use case under which the business collaboration protocol is placed
                        Package bcv1 = context.Repository.GetPackageByID(classifier.PackageID);
                        Element includedBCUC = null;
                        foreach (Element e in bcv1.Elements)
                        {
                            if (e.Stereotype == UMM.bCollaborationUC.ToString())
                            {
                                includedBCUC = e;
                                break;
                            }
                        }

                        //No Business Collaboration Use Case found
                        if (includedBCUC == null)
                        {
                            context.AddValidationMessage(new ValidationMessage("Violation of constraint C67.",
                                                                               "Each BusinessCollaborationProtocol called by a BusinessCollaborationAction MUST be placed beneath a BusinessCollaborationProtocolUseCase which is included in the BusinessCollaborationUseCase that covers the corresponding BusinessCollaborationProtocol.\n\nUnable to find BusinessCollaborationUseCase for BusinessCollaborationAction " +
                                                                               bca.Name, "BCV",
                                                                               ValidationMessage.errorLevelTypes.ERROR,
                                                                               bcv.PackageID));
                        }
                        else
                        {
                            //Get the BusinessCollaborationUseCase of this package
                            Element bcuc = Utility.getElementFromPackage(bcv, UMM.bCollaborationUC.ToString());
                            if (bcuc == null)
                            {
                                context.AddValidationMessage(new ValidationMessage("Violation of constraint C67.",
                                                                                   "Each BusinessCollaborationProtocol called by a BusinessCollaborationAction MUST be placed beneath a BusinessCollaborationProtocolUseCase which is included in the BusinessCollaborationUseCase that covers the corresponding BusinessCollaborationProtocol.\n\nUnable to find BusinessCollaborationUseCase in which the BusinessCollaborationUseCase of the BusinessCollaborationAction " +
                                                                                   bca.Name + " should be included.",
                                                                                   "BCV",
                                                                                   ValidationMessage.errorLevelTypes.
                                                                                       ERROR, bcv.PackageID));
                            }
                            else
                            {
                                //Does the BusinessCollaborationUseCase include the BTUC
                                bool found = false;
                                foreach (Connector con in bcuc.Connectors)
                                {
                                    if (con.Stereotype == UMM.include.ToString())
                                    {
                                        Element client = context.Repository.GetElementByID(con.ClientID);
                                        Element supplier = context.Repository.GetElementByID(con.SupplierID);

                                        if (client.ElementID == bcuc.ElementID &&
                                            supplier.ElementID == includedBCUC.ElementID)
                                        {
                                            found = true;
                                        }
                                    }
                                }

                                if (!found)
                                {
                                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C67.",
                                                                                       "Each BusinessCollaborationProtocol called by a BusinessCollaborationAction MUST be placed beneath a BusinessCollaborationProtocolUseCase which is included in the BusinessCollaborationUseCase that covers the corresponding BusinessCollaborationProtocol.\n\nThe BusinessCollaborationUseCase " +
                                                                                       includedBCUC.Name +
                                                                                       " is not included by the BusinessCollaborationUseCase " +
                                                                                       bcuc.Name +
                                                                                       ".If you want to use a BusinessCollaborationProtocol because it is called by a BusinessCollaborationAction in a BusinessCollaborationView, the following requirement has to be met:\n1. The BusinessCollaborationUseCase under which the BusinessCollaborationProtocol is placed MUST be included by the BusinessCollaborationUseCase of the BusinessCollaborationView where you want to use the BusinessCollaboration.",
                                                                                       "BCV",
                                                                                       ValidationMessage.errorLevelTypes
                                                                                           .ERROR, bcv.PackageID));
                                }
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Check constraint C68
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bcv"></param>
        private void checkC68(IValidationContext context, Package bcv)
        {
            //Get the BusinessCollaborationProtocol
            Element bcpr = Utility.getElementFromPackage(bcv, UMM.bCollaborationProtocol.ToString());

            if (bcpr == null)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C68.",
                                                                   "A BusinessCollaborationProtocol MUST contain two to many BusinessCollaborationPartions. \n\nUnable to find BusinessCollaborationProtocol",
                                                                   "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                   bcv.PackageID));
            }
            else
            {
                int countPartitions = 0;

                foreach (Element e in bcv.Elements)
                {
                    if (e.ParentID == bcpr.ElementID && e.Stereotype == UMM.bCPartition.ToString())
                    {
                        countPartitions++;
                    }
                }

                if (countPartitions < 2)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C68.",
                                                                       "A BusinessCollaborationProtocol MUST contain two to many BusinessCollaborationPartions. \n\nFound invalid number of BusinessCollaborationPartitions: " +
                                                                       countPartitions, "BCV",
                                                                       ValidationMessage.errorLevelTypes.ERROR,
                                                                       bcv.PackageID));
                }
            }
        }


        /// <summary>
        /// Validate constraint C69
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bcv"></param>
        private void checkC69(IValidationContext context, Package bcv)
        {
            //Get the number of partitions
            IList<Element> partitions = Utility.getAllElements(bcv, new List<Element>(),
                                                               UMM.bCPartition.ToString());
            //Get the number of Authorized Roles
            IList<Element> authorizedRoles = Utility.getAllElements(bcv, new List<Element>(),
                                                                    UMM.AuthorizedRole.ToString());

            if (partitions.Count != authorizedRoles.Count)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C69.",
                                                                   "The number of AuthorizedRoles in the BusinessCollaborationView MUST match the number of BusinessCollaborationPartitions in the BusinessCollaborationProtocol which is placed beneath the BusinessCollaborationUseCase of the same BusinessCollaborationView. \n\nFound " +
                                                                   partitions.Count +
                                                                   " BusinessCollaborationPartitions and " +
                                                                   authorizedRoles.Count + " AuthorizedRoles.", "BCV",
                                                                   ValidationMessage.errorLevelTypes.ERROR,
                                                                   bcv.PackageID));
                return;
            }

            //Get the BusinessCollaborationProtocol
            Element bcpr = Utility.getElementFromPackage(bcv, UMM.bCollaborationProtocol.ToString());

            if (bcpr == null)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C69.",
                                                                   "The number of AuthorizedRoles in the BusinessCollaborationView MUST match the number of BusinessCollaborationPartitions in the BusinessCollaborationProtocol which is placed beneath the BusinessCollaborationUseCase of the same BusinessCollaborationView. \n\nUnable to find BusinessCollaborationProtocol.",
                                                                   "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                   bcv.PackageID));
            }
            else
            {
                //Check if all the partitions are located underneath the BusinessCollaboration Protocol
                int countPartitions = 0;
                foreach (Element e in bcv.Elements)
                {
                    if (e.ParentID == bcpr.ElementID && e.Stereotype == UMM.bCPartition.ToString())
                    {
                        countPartitions++;
                    }
                }

                //The number of all partitions in the BusinessCollaborationView and the number of partitions underneath
                //the businesscollaborationprotocol must be the same
                if (countPartitions != partitions.Count)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C69.",
                                                                       "The number of AuthorizedRoles in the BusinessCollaborationView MUST match the number of BusinessCollaborationPartitions in the BusinessCollaborationProtocol which is placed beneath the BusinessCollaborationUseCase of the same BusinessCollaborationView. \n\nAll BusinesscollaborationPartitions must be placed beneath the BusinessCollaborationUseCase.",
                                                                       "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                       bcv.PackageID));
                }
            }
        }


        /// <summary>
        /// Check constraint C70
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bcv"></param>
        private void checkC70(IValidationContext context, Package bcv)
        {
            //Get all Authorized Roles
            IList<Element> ar = Utility.getAllElements(bcv, new List<Element>(), UMM.AuthorizedRole.ToString());
            //Get all Partitions
            IList<Element> partitions = Utility.getAllElements(bcv, new List<Element>(),
                                                               UMM.bCPartition.ToString());

            //Check for each AR if it is assigned to a partitions
            foreach (Element arole in ar)
            {
                bool found = false;
                foreach (Element partition in partitions)
                {
                    if (partition.ClassifierID == arole.ElementID)
                    {
                        found = true;
                        break;
                    }
                }

                //No partition assigned to this authorized role
                if (!found)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C70.",
                                                                       "Each AuthorizedRole in the BusinessCollaborationView MUST be assigned to a BusinessCollaborationPartition in the BusinessCollaborationProtocol which is placed beneath the BusinessCollaborationUseCase of the same BusinessCollaborationView.\n\nNo BusinessCollaborationPartition is assigned to the AuthorizedRole " +
                                                                       arole.Name +
                                                                       ". Drag and drop the BusinessCollaborationPartition onto the diagram canvas, right click on it, choose Advanced > Instance Classifier and select the correct Authorized Role.",
                                                                       "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                       bcv.PackageID));
                }
            }
        }


        /// <summary>
        /// Check constraint C71
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bcv"></param>
        private void checkC71(IValidationContext context, Package bcv)
        {
            //Get all Authorized Roles
            IList<Element> ar = Utility.getAllElements(bcv, new List<Element>(), UMM.AuthorizedRole.ToString());
            //Get all Partitions
            IList<Element> partitions = Utility.getAllElements(bcv, new List<Element>(),
                                                               UMM.bCPartition.ToString());

            //Check if each partition is classified by excactly one Authorized role
            foreach (Element partition in partitions)
            {
                bool found = false;
                foreach (Element arole in ar)
                {
                    if (partition.ClassifierID == arole.ElementID)
                    {
                        found = true;
                    }
                }

                if (!found)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C71.",
                                                                       "Each BusinessCollaborationPartition MUST be classified by exactly one AuthorizedRole included in the same BusinessCollaborationView as the BusinessCollaborationUseCase covering the BusinessCollaborationProtocol containing this BusinessCollaborationPartition.\n\nFound a partition without a classifier. Drag and drop the BusinessCollaborationPartition onto the diagram canvas, right click on it, choose Advanced > Instance Classifier and select the correct Authorized Role.",
                                                                       "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                       bcv.PackageID));
                }
            }
        }


        /// <summary>
        /// Check constraint C72
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bcv"></param>
        private void checkC72(IValidationContext context, Package bcv)
        {
            //Get the BusinessCollaborationPartitions            
            IList<Element> partitions = Utility.getAllElements(bcv, new List<Element>(),
                                                               UMM.bCPartition.ToString());

            foreach (Element e in bcv.Elements)
            {
                foreach (Element partition in partitions)
                {
                    if (e.ParentID == partition.ElementID)
                    {
                        if (e.Stereotype != UMM.bNestedCollaboration.ToString())
                        {
                            context.AddValidationMessage(new ValidationMessage("Violation of constraint C72.",
                                                                               "A BusinessCollaborationPartition MUST be either empty or contain one to many NestedBusinessCollaborations.\n\nInvalid element detected: " +
                                                                               e.Name, "BCV",
                                                                               ValidationMessage.errorLevelTypes.ERROR,
                                                                               bcv.PackageID));
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Check constraint C73
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bcv"></param>
        private void checkC73(IValidationContext context, Package bcv)
        {
            //Get all BusinessTransactionActions
            IList<Element> btactions = Utility.getAllElements(bcv, new List<Element>(),
                                                              UMM.bTransactionAction.ToString());


            foreach (Element e in btactions)
            {
                int countInitialFlows = 0;

                //Each BusinessTransactionAction MUST be the target of exactly one IntialFlow which source
                //MUST be a BusinessCollaborationPartition
                foreach (Connector con in e.Connectors)
                {
                    if (con.Stereotype == UMM.initFlow.ToString())
                    {
                        Element client = context.Repository.GetElementByID(con.ClientID);
                        Element supplier = context.Repository.GetElementByID(con.SupplierID);


                        //The business transaction action must be the supplier
                        if (supplier.ElementID == e.ElementID)
                        {
                            //Analyze the Client which must be the partition
                            if (client.Stereotype == UMM.bCPartition.ToString())
                            {
                                //The partition must be in this business collaboration view
                                if (client.PackageID != bcv.PackageID)
                                {
                                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C73.",
                                                                                       "Each BusinessTransactionAction MUST be the target of exactly one InitialFlow which source MUST be a BusinessCollaborationPartition.\n\nBusinessTransactionAction " +
                                                                                       e.Name +
                                                                                       " is connected to a BusinessCollaborationPartition which is part of another BusinessCollaborationView.",
                                                                                       "BCV",
                                                                                       ValidationMessage.errorLevelTypes
                                                                                           .ERROR, bcv.PackageID));
                                }
                                else
                                {
                                    countInitialFlows++;
                                }
                            }
                            else
                            {
                                //add error
                                context.AddValidationMessage(new ValidationMessage("Violation of constraint C73.",
                                                                                   "Each BusinessTransactionAction MUST be the target of exactly one InitialFlow which source MUST be a BusinessCollaborationPartition.\n\nInvalid connection to BusinessTransactionAction " +
                                                                                   e.Name +
                                                                                   " detected. Please check the source of the connection and make sure that it is a BusinessCollaborationPartition.",
                                                                                   "BCV",
                                                                                   ValidationMessage.errorLevelTypes.
                                                                                       ERROR, bcv.PackageID));
                            }
                        }
                    }
                }

                if (countInitialFlows != 1)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C73.",
                                                                       "Each BusinessTransactionAction MUST be the target of exactly one InitialFlow which source MUST be a BusinessCollaborationPartition.\n\nInvalid number of connections found for BusinessTransactionAction " +
                                                                       e.Name + ": " + countInitialFlows, "BCV",
                                                                       ValidationMessage.errorLevelTypes.ERROR,
                                                                       bcv.PackageID));
                }
            }
        }


        /// <summary>
        /// Check constraint C74
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bcv"></param>
        private void checkC74(IValidationContext context, Package bcv)
        {
            //Get all BusinessTransactionActions
            IList<Element> btactions = Utility.getAllElements(bcv, new List<Element>(),
                                                              UMM.bTransactionAction.ToString());


            foreach (Element e in btactions)
            {
                int countInitialFlows = 0;

                //Each BusinessTransactionAction must be the source of exactly one INtital Flow which target
                //MUSt be either a business collaboration partition or a neseted business collaboration
                foreach (Connector con in e.Connectors)
                {
                    if (con.Stereotype == UMM.initFlow.ToString())
                    {
                        Element client = context.Repository.GetElementByID(con.ClientID);
                        Element supplier = context.Repository.GetElementByID(con.SupplierID);


                        //The business transaction action must be the supplier
                        if (client.ElementID == e.ElementID)
                        {
                            //Analyze the Supplier which must be the partition
                            if (supplier.Stereotype == UMM.bCPartition.ToString() ||
                                supplier.Stereotype == UMM.bNestedCollaboration.ToString())
                            {
                                //The partition must be in this business collaboration view
                                if (supplier.PackageID != bcv.PackageID)
                                {
                                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C74.",
                                                                                       "Each BusinessTransactionAction MUST be the source of exactly one InitialFlow which target MUST be either a BusinessCollaborationPartition or a NestedBusinessCollaboration. \n\nBusinessTransactionAction " +
                                                                                       e.Name +
                                                                                       " is connected to a BusinessCollaborationPartition which is part of another BusinessCollaborationView.",
                                                                                       "BCV",
                                                                                       ValidationMessage.errorLevelTypes
                                                                                           .ERROR, bcv.PackageID));
                                }
                                else
                                {
                                    countInitialFlows++;
                                }
                            }
                        }
                    }
                }

                if (countInitialFlows != 1)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C74.",
                                                                       "Each BusinessTransactionAction MUST be the source of exactly one InitialFlow which target MUST be either a BusinessCollaborationPartition or a NestedBusinessCollaboration. \n\nInvalid number of connections found for BusinessTransactionAction " +
                                                                       e.Name + ": " + countInitialFlows, "BCV",
                                                                       ValidationMessage.errorLevelTypes.ERROR,
                                                                       bcv.PackageID));
                }
            }
        }


        /// <summary>
        /// Check constraint C75
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bcv"></param>
        private void checkC75(IValidationContext context, Package bcv)
        {
            //Get all BusinessTransactionActions
            IList<Element> btactions = Utility.getAllElements(bcv, new List<Element>(),
                                                              UMM.bTransactionAction.ToString());


            foreach (Element e in btactions)
            {
                Element supplierPartition = null;
                Element clientPartition = null;

                //Each BusinessTransactionAction must be the source of exactly one INtital Flow which target
                //MUSt be either a business collaboration partition or a neseted business collaboration
                foreach (Connector con in e.Connectors)
                {
                    if (con.Stereotype == UMM.initFlow.ToString())
                    {
                        Element client = context.Repository.GetElementByID(con.ClientID);
                        Element supplier = context.Repository.GetElementByID(con.SupplierID);

                        //The BusinessTransactionAction is the Client                       
                        if (client.ElementID == e.ElementID)
                        {
                            //Analyze the Supplier which must be the partition or a NestedCollaboration
                            if (supplier.Stereotype == UMM.bCPartition.ToString() ||
                                client.Stereotype == UMM.bNestedCollaboration.ToString())
                            {
                                supplierPartition = supplier;
                            }
                        }
                            //The business transaction action is the supplier
                        else if (supplier.ElementID == e.ElementID)
                        {
                            //Analyze the Client which must be the partition or a Nested Collaboration
                            if (client.Stereotype == UMM.bCPartition.ToString() ||
                                client.Stereotype == UMM.bNestedCollaboration.ToString())
                            {
                                clientPartition = client;
                            }
                        }
                    }
                }

                if (supplierPartition != null && clientPartition != null)
                {
                    if (supplierPartition.ElementID == clientPartition.ElementID)
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C75.",
                                                                           "The InitialFlow sourcing from a BusinessTransactionAction and the InitialFlow targeting a BusinessTransactionAction MUST NOT be targeting to / sourcing from the same BusinessCollaborationPartition, nor targeting to a NestedBusinessCollaboration within the same BusinessCollaborationPartition. \n\nPlease check the connections of BusinessTransactionAction " +
                                                                           e.Name, "BCV",
                                                                           ValidationMessage.errorLevelTypes.ERROR,
                                                                           bcv.PackageID));
                    }
                }
            }
        }


        /// <summary>
        /// Check constraint C76
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bcv"></param>
        private void checkC76(IValidationContext context, Package bcv)
        {
            //Get all BusinessTransactionActions
            IList<Element> btactions = Utility.getAllElements(bcv, new List<Element>(),
                                                              UMM.bTransactionAction.ToString());


            //Get the calling BusinessTransactions
            foreach (Element btaction in btactions)
            {
                //Get the business transaction
                Element bt = null;
                if (btaction.ClassifierID != 0)
                {
                    bt = context.Repository.GetElementByID(btaction.ClassifierID);
                }

                //Unable to find the Business Transaction - terminate and return
                if (bt == null)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C76.",
                                                                       "If a BusinessTransactionAction calls a two-way BusinessTransaction, this BusinessTransactionAction MUST be the source of exactly one RespondingFlow which target MUST be a BusinessCollaborationPartition. \n\nUnable to find BusinessTransaction.",
                                                                       "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                       bcv.PackageID));
                    break;
                }

                bool twoWay = false;
                foreach (TaggedValue tvalue in bt.TaggedValues)
                {
                    if (tvalue.Name == "businessTransactionType")
                    {
                        //Check the value
                        if (tvalue.Value == "Request/Response" || tvalue.Value == "Query/Response" ||
                            tvalue.Value == "Request/Confirm" || tvalue.Value == "CommercialTransaction")
                        {
                            twoWay = true;
                            break;
                        }
                    }
                }

                //Two Way Transaction?
                if (twoWay)
                {
                    //Go through the connectors and check, if this business transaction action is the source of
                    //exactly one repsonding flow which target must be a businessdollaboration patrtition
                    int countReFlow = 0;
                    foreach (Connector con in btaction.Connectors)
                    {
                        Element client = context.Repository.GetElementByID(con.ClientID);
                        Element supplier = context.Repository.GetElementByID(con.SupplierID);

                        if (con.Stereotype == UMM.reFlow.ToString())
                        {
                            //connector emanating from this business transaction action
                            if (client.ElementID == btaction.ElementID)
                            {
                                if (supplier.Stereotype == UMM.bCPartition.ToString())
                                {
                                    countReFlow++;
                                }
                                else
                                {
                                    //Invalid supplier of the reflow
                                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C76.",
                                                                                       "If a BusinessTransactionAction calls a two-way BusinessTransaction, this BusinessTransactionAction MUST be the source of exactly one RespondingFlow which target MUST be a BusinessCollaborationPartition. \n\nInvalid target of reflow connector.",
                                                                                       "BCV",
                                                                                       ValidationMessage.errorLevelTypes
                                                                                           .ERROR, bcv.PackageID));
                                }
                            }
                        }
                    }

                    if (countReFlow != 1)
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C76.",
                                                                           "If a BusinessTransactionAction calls a two-way BusinessTransaction, this BusinessTransactionAction MUST be the source of exactly one RespondingFlow which target MUST be a BusinessCollaborationPartition. \n\nInvalid number of reflow connections: " +
                                                                           countReFlow, "BCV",
                                                                           ValidationMessage.errorLevelTypes.ERROR,
                                                                           bcv.PackageID));
                    }
                }
            }
        }


        /// <summary>
        /// Check constraint C77
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bcv"></param>
        private void checkC77(IValidationContext context, Package bcv)
        {
            //Get all BusinessTransactionActions
            IList<Element> btactions = Utility.getAllElements(bcv, new List<Element>(),
                                                              UMM.bTransactionAction.ToString());


            //Get the calling BusinessTransactions
            foreach (Element btaction in btactions)
            {
                //Get the business transaction
                Element bt = null;
                if (btaction.ClassifierID != 0)
                {
                    bt = context.Repository.GetElementByID(btaction.ClassifierID);
                }

                //Unable to find the Business Transaction - terminate and return
                if (bt == null)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C77.",
                                                                       "If a BusinessTransactionAction calls a two-way BusinessTransaction, this BusinessTransactionAction MUST be the target of exactly one RespondingFlow which source MUST be either a BusinessCollaborationPartition or a NestedBusinessCollaboration. \n\nUnable to find BusinessTransaction.",
                                                                       "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                       bcv.PackageID));
                    break;
                }

                bool twoWay = false;
                foreach (TaggedValue tvalue in bt.TaggedValues)
                {
                    if (tvalue.Name == "businessTransactionType")
                    {
                        //Check the value
                        if (tvalue.Value == "Request/Response" || tvalue.Value == "Query/Response" ||
                            tvalue.Value == "Request/Confirm" || tvalue.Value == "CommercialTransaction")
                        {
                            twoWay = true;
                            break;
                        }
                    }
                }

                //Two Way Transaction?
                if (twoWay)
                {
                    //Go through the connectors of the business transaction action and check if it the target of exactly
                    //one responding flow which source must be either a business collaboration partition of a nestedpartition
                    int countReFlow = 0;
                    foreach (Connector con in btaction.Connectors)
                    {
                        Element client = context.Repository.GetElementByID(con.ClientID);
                        Element supplier = context.Repository.GetElementByID(con.SupplierID);

                        if (con.Stereotype == UMM.reFlow.ToString())
                        {
                            //connector leading to this business transaction action
                            if (supplier.ElementID == btaction.ElementID)
                            {
                                if (client.Stereotype == UMM.bCPartition.ToString() ||
                                    client.Stereotype == UMM.bNestedCollaboration.ToString())
                                {
                                    countReFlow++;
                                }
                                else
                                {
                                    //Invalid source of the reflow
                                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C77.",
                                                                                       "If a BusinessTransactionAction calls a two-way BusinessTransaction, this BusinessTransactionAction MUST be the target of exactly one RespondingFlow which source MUST be either a BusinessCollaborationPartition or a NestedBusinessCollaboration. \n\nInvalid source of reflow connector.",
                                                                                       "BCV",
                                                                                       ValidationMessage.errorLevelTypes
                                                                                           .ERROR, bcv.PackageID));
                                }
                            }
                        }
                    }

                    if (countReFlow != 1)
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C77.",
                                                                           "If a BusinessTransactionAction calls a two-way BusinessTransaction, this BusinessTransactionAction MUST be the target of exactly one RespondingFlow which source MUST be either a BusinessCollaborationPartition or a NestedBusinessCollaboration. \n\nInvalid number of reflow connections: " +
                                                                           countReFlow, "BCV",
                                                                           ValidationMessage.errorLevelTypes.ERROR,
                                                                           bcv.PackageID));
                    }
                }
            }
        }


        /// <summary>
        /// Check constraint C78
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bcv"></param>
        private void checkC78(IValidationContext context, Package bcv)
        {
            //Get all BusinessTransactionActions
            IList<Element> btactions = Utility.getAllElements(bcv, new List<Element>(),
                                                              UMM.bTransactionAction.ToString());


            foreach (Element e in btactions)
            {
                Element supplierPartition = null;
                Element clientPartition = null;

                //Each BusinessTransactionAction must be the source of exactly one INtital Flow which target
                //MUSt be either a business collaboration partition or a neseted business collaboration
                foreach (Connector con in e.Connectors)
                {
                    if (con.Stereotype == UMM.reFlow.ToString())
                    {
                        Element client = context.Repository.GetElementByID(con.ClientID);
                        Element supplier = context.Repository.GetElementByID(con.SupplierID);

                        //The BusinessTransactionAction is the Client                       
                        if (client.ElementID == e.ElementID)
                        {
                            //Analyze the Supplier which must be the partition or a NestedCollaboration
                            if (supplier.Stereotype == UMM.bCPartition.ToString() ||
                                client.Stereotype == UMM.bNestedCollaboration.ToString())
                            {
                                supplierPartition = supplier;
                            }
                        }
                            //The business transaction action is the supplier
                        else if (supplier.ElementID == e.ElementID)
                        {
                            //Analyze the Client which must be the partition or a Nested Collaboration
                            if (client.Stereotype == UMM.bCPartition.ToString() ||
                                client.Stereotype == UMM.bNestedCollaboration.ToString())
                            {
                                clientPartition = client;
                            }
                        }
                    }
                }

                if (supplierPartition != null && clientPartition != null)
                {
                    if (supplierPartition.ElementID == clientPartition.ElementID)
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C78.",
                                                                           "The RespondingFlow sourcing from a BusinessTransactionAction and the RespondingFlow targeting a BusinessTransactionAction MUST NOT be targeting to /sourcing from the same BusinessCollaborationPartition, nor targeting to a NestedBusinessCollaboration within the same BusinessCollaborationPartition.\n\nPlease check the connections of BusinessTransactionAction " +
                                                                           e.Name, "BCV",
                                                                           ValidationMessage.errorLevelTypes.ERROR,
                                                                           bcv.PackageID));
                    }
                }
            }
        }


        /// <summary>
        /// Check constraint C79
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bcv"></param>
        private void checkC79(IValidationContext context, Package bcv)
        {
            //Get all BusinessTransactionActions
            IList<Element> btactions = Utility.getAllElements(bcv, new List<Element>(),
                                                              UMM.bTransactionAction.ToString());


            //Get the calling BusinessTransactions
            foreach (Element btaction in btactions)
            {
                //Get the business transaction
                Element bt = null;
                if (btaction.ClassifierID != 0)
                {
                    bt = context.Repository.GetElementByID(btaction.ClassifierID);
                }

                //Unable to find the Business Transaction - terminate and return
                if (bt == null)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C79.",
                                                                       "If a BusinessTransactionAction calls a one-way BusinessTransaction, this BusinessTransactionAction MUST NOT be the source of a RespondingFlow and MUST NOT be the target of a RespondingFlow. \n\nUnable to find BusinessTransaction.",
                                                                       "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                       bcv.PackageID));
                    break;
                }

                bool oneWay = false;
                foreach (TaggedValue tvalue in bt.TaggedValues)
                {
                    if (tvalue.Name == "businessTransactionType")
                    {
                        //Check the value
                        if (tvalue.Value == "Notification" || tvalue.Value == "InformationDistribution")
                        {
                            oneWay = true;
                            break;
                        }
                    }
                }

                //One Way Transaction?
                if (oneWay)
                {
                    //Since the Business Transaction is a Onw way one, There must not be any reflows
                    foreach (Connector con in btaction.Connectors)
                    {
                        if (con.Stereotype == UMM.reFlow.ToString())
                        {
                            context.AddValidationMessage(new ValidationMessage("Violation of constraint C79.",
                                                                               "If a BusinessTransactionAction calls a one-way BusinessTransaction, this BusinessTransactionAction MUST NOT be the source of a RespondingFlow and MUST NOT be the target of a RespondingFlow. \n\nInvalid reflow detected on BusinessTransactionAction " +
                                                                               btaction.Name, "BCV",
                                                                               ValidationMessage.errorLevelTypes.ERROR,
                                                                               bcv.PackageID));
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Check constraint C80
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bcv"></param>
        private void checkC80(IValidationContext context, Package bcv)
        {
            //Get all BusinessTransactionActions
            IList<Element> btactions = Utility.getAllElements(bcv, new List<Element>(),
                                                              UMM.bTransactionAction.ToString());


            //Get the calling BusinessTransactions
            foreach (Element btaction in btactions)
            {
                int respondingClient = 0;
                int initSupplier = 0;

                //Is there a responding flow targeting this business transaction action?
                foreach (Connector con in btaction.Connectors)
                {
                    if (con.Stereotype == UMM.reFlow.ToString())
                    {
                        if (con.SupplierID == btaction.ElementID)
                        {
                            respondingClient = con.ClientID;
                        }
                    }

                    if (con.Stereotype == UMM.initFlow.ToString())
                    {
                        if (con.ClientID == btaction.ElementID)
                        {
                            initSupplier = con.SupplierID;
                        }
                    }
                }

                if (respondingClient != 0 && initSupplier != 0)
                {
                    if (respondingClient != initSupplier)
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C80.",
                                                                           "The RespondingFlow targeting a BusinessTransactionAction must start from the BusinessCollaborationPartition / NestedBusinessCollaboration which is the target of the InitialFlow starting from the same BusinessTransactionAction.\n\nPlease check the connections of the BusinessTransactionAction " +
                                                                           btaction.Name, "BCV",
                                                                           ValidationMessage.errorLevelTypes.ERROR,
                                                                           bcv.PackageID));
                    }
                }
            }
        }


        /// <summary>
        /// Check constraint C81
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bcv"></param>
        private void checkC81(IValidationContext context, Package bcv)
        {
            //Get all BusinessTransactionActions
            IList<Element> btactions = Utility.getAllElements(bcv, new List<Element>(),
                                                              UMM.bTransactionAction.ToString());


            //Get the calling BusinessTransactions
            foreach (Element btaction in btactions)
            {
                int respondingSupplier = 0;
                int initClient = 0;

                //Is there a responding flow starting from the business transaction
                foreach (Connector con in btaction.Connectors)
                {
                    if (con.Stereotype == UMM.reFlow.ToString())
                    {
                        if (con.ClientID == btaction.ElementID)
                        {
                            respondingSupplier = con.SupplierID;
                        }
                    }

                    if (con.Stereotype == UMM.initFlow.ToString())
                    {
                        if (con.SupplierID == btaction.ElementID)
                        {
                            initClient = con.ClientID;
                        }
                    }
                }

                if (respondingSupplier != 0 && initClient != 0)
                {
                    if (respondingSupplier != initClient)
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C81.",
                                                                           "The RespondingFlow starting from a BusinessTransactionAction must target the BusinessCollaborationPartition which is the source of the InitialFlow targeting to the same BusinessTransactionAction..\n\nPlease check the connections of the BusinessTransactionAction " +
                                                                           btaction.Name, "BCV",
                                                                           ValidationMessage.errorLevelTypes.ERROR,
                                                                           bcv.PackageID));
                    }
                }
            }
        }


        /// <summary>
        /// Check constraint C82
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bcv"></param>
        private void checkC82(IValidationContext context, Package bcv)
        {
            //Get all NestedBusinesscollaborations (if any)
            IList<Element> nestedC = Utility.getAllElements(bcv, new List<Element>(),
                                                            UMM.bNestedCollaboration.ToString());

            //A NestedBusinessCollaboration MUST be the target of exactly one InitialFlow
            foreach (Element nc in nestedC)
            {
                int countCons = 0;
                foreach (Connector con in nc.Connectors)
                {
                    if (con.Stereotype == UMM.initFlow.ToString())
                    {
                        if (con.SupplierID == nc.ElementID)
                        {
                            countCons++;
                        }
                    }
                }

                if (countCons != 1)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C82.",
                                                                       "A NestedBusinessCollaboration MUST be the target of exactly one InitialFlow. \n\nPlease check NestedBusinessCollaboration " +
                                                                       nc.Name, "BCV",
                                                                       ValidationMessage.errorLevelTypes.ERROR,
                                                                       bcv.PackageID));
                }
            }
        }


        // <summary>
        /// Check constraint C83
        /// 
        /// A NestedBusinessCollaboration MAY be the source of a RespondingFlow, but MUST NOT be the source of more than one RespondingFlow.
        /// </summary>
        /// <param name="bcv"></param>
        private void checkC83(IValidationContext context, Package bcv)
        {
            //Get all NestedBusinesscollaborations (if any)
            IList<Element> nestedC = Utility.getAllElements(bcv, new List<Element>(),
                                                            UMM.bNestedCollaboration.ToString());

            foreach (Element nbc in nestedC)
            {
                //MUST NOT be the source of more than one RespondingFlow.
                int countCons = 0;
                foreach (Connector con in nbc.Connectors)
                {
                    if (con.Stereotype == UMM.reFlow.ToString() && con.ClientID == nbc.ElementID)
                    {
                        countCons++;
                    }
                }

                if (countCons > 1)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C83.",
                                                                       "A NestedBusinessCollaboration MAY be the source of a RespondingFlow, but MUST NOT be the source of more than one RespondingFlow.\n\nNestedBusinessCollaboration " +
                                                                       bcv.Name + " has invalid connectors.", "BCV",
                                                                       ValidationMessage.errorLevelTypes.ERROR,
                                                                       bcv.PackageID));
                }
            }
        }


        // <summary>
        /// Check constraint C84
        /// 
        ///  A BusinessCollaborationAction MUST be the target of two to many InformationFlows (UML standard: <<flow>>). 
        /// </summary>
        /// <param name="bcv"></param>
        private void checkC84(IValidationContext context, Package bcv)
        {
            //Get all BusinessCollaborationActions
            IList<Element> bcas = Utility.getAllElements(bcv, new List<Element>(),
                                                         UMM.bCollaborationAction.ToString());


            foreach (Element bca in bcas)
            {
                int countFlows = 0;
                foreach (Connector con in bca.Connectors)
                {
                    if (con.SupplierID == bca.ElementID && con.Type == "InformationFlow" && con.Stereotype == "flow")
                    {
                        countFlows++;
                    }
                }

                if (countFlows < 2)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C84.",
                                                                       "A BusinessCollaborationAction MUST be the target of two to many InformationFlows (UML standard: <<flow>>). \n\nBusinessCollaborationAction " +
                                                                       bca.Name +
                                                                       " has an invalid number of <<flow>> connectors: " +
                                                                       countFlows +
                                                                       ". Please check in the properties menu of the InformationFlow connector, if the stereotype is set correctly.",
                                                                       "BCV", ValidationMessage.errorLevelTypes.ERROR,
                                                                       bcv.PackageID));
                }
            }
        }


        /// <summary>
        /// Check constraint C85
        /// A BusinessCollaborationAction MUST not be the source of an InformationFlow.
        ///  
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bcv"></param>
        private void checkC85(IValidationContext context, Package bcv)
        {
            //Get all BusinessCollaborationActions
            IList<Element> bcas = Utility.getAllElements(bcv, new List<Element>(),
                                                         UMM.bCollaborationAction.ToString());


            foreach (Element bca in bcas)
            {
                int countFlows = 0;
                foreach (Connector con in bca.Connectors)
                {
                    if (con.Type == "InformationFlow" && con.ClientID == bca.ElementID)
                    {
                        countFlows++;
                    }
                }


                if (countFlows != 0)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C85.",
                                                                       "A BusinessCollaborationAction MUST not be the source of an InformationFlow. \n\nPlease check BusinessCollaborationAction " +
                                                                       bca.Name, "BCV",
                                                                       ValidationMessage.errorLevelTypes.ERROR,
                                                                       bcv.PackageID));
                }
            }
        }


        /// <summary>
        /// Check constraint C86
        /// A BusinessCollaborationAction MUST not be the source and MUST not be the target of an InitialFlow.
        ///  
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bcv"></param>
        private void checkC86(IValidationContext context, Package bcv)
        {
            //Get all BusinessCollaborationActions
            IList<Element> bcas = Utility.getAllElements(bcv, new List<Element>(),
                                                         UMM.bCollaborationAction.ToString());


            foreach (Element bca in bcas)
            {
                int countFlows = 0;
                foreach (Connector con in bca.Connectors)
                {
                    if (con.Stereotype == UMM.initFlow.ToString())
                    {
                        countFlows++;
                    }
                }


                if (countFlows != 0)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C86.",
                                                                       "A BusinessCollaborationAction MUST not be the source and MUST not be the target of an InitialFlow.\n\nFound " +
                                                                       countFlows +
                                                                       " InitalFlows to/from BusinessCollaborationAction " +
                                                                       bca.Name, "BCV",
                                                                       ValidationMessage.errorLevelTypes.ERROR,
                                                                       bcv.PackageID));
                }
            }
        }


        /// <summary>
        /// Check constraint C87
        /// 
        ///  
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bcv"></param>
        private void checkC87(IValidationContext context, Package bcv)
        {
            //Get all BusinessCollaborationActions
            IList<Element> bcas = Utility.getAllElements(bcv, new List<Element>(),
                                                         UMM.bCollaborationAction.ToString());


            foreach (Element bca in bcas)
            {
                int countFlows = 0;
                foreach (Connector con in bca.Connectors)
                {
                    if (con.Stereotype == UMM.reFlow.ToString())
                    {
                        countFlows++;
                    }
                }


                if (countFlows != 0)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C87.",
                                                                       "A BusinessCollaborationAction MUST not be the source and MUST not be the target of an RespondingFlow.\n\nFound " +
                                                                       countFlows +
                                                                       " RespondingFlows to/from BusinessCollaborationAction " +
                                                                       bca.Name, "BCV",
                                                                       ValidationMessage.errorLevelTypes.ERROR,
                                                                       bcv.PackageID));
                }
            }
        }


        /// <summary>
        /// Check constraint C88
        /// 
        ///A BusinessTransactionAction MUST not be the source and MUST not be the target of an InformationFlow (<<flow>>) that is neither stereotyped as InitialFlow nor as RespondingFlow.  
        /// </summary>
        /// <param name="bcv"></param>
        private void checkC88(IValidationContext context, Package bcv)
        {
            //Get all BusinessTransactionActions
            IList<Element> btas = Utility.getAllElements(bcv, new List<Element>(),
                                                         UMM.bTransactionAction.ToString());


            foreach (Element bta in btas)
            {
                int count = 0;
                //There must not be any InformationFlow from/to but InitFlow and ReFlow
                foreach (Connector con in bta.Connectors)
                {
                    if (con.Type == "InformationFlow")
                    {
                        //The stereotype flow is no specifically defined in the constraint - however we check it too
                        if (
                            !(con.Stereotype == UMM.initFlow.ToString() || con.Stereotype == UMM.reFlow.ToString() ||
                              con.Stereotype == "flow"))
                        {
                            count++;
                        }
                    }
                }

                if (count != 0)
                {
                    context.AddValidationMessage(new ValidationMessage("Violoation of constraint C88.",
                                                                       "A BusinessTransactionAction MUST not be the source and MUST not be the target of an InformationFlow (<<flow>>) that is neither stereotyped as InitialFlow nor as RespondingFlow.  \n\nBusinessTransactionAction " +
                                                                       bta.Name + " has invalid connectors.", "BCV",
                                                                       ValidationMessage.errorLevelTypes.ERROR,
                                                                       bcv.PackageID));
                }
            }
        }


        /// <summary>
        /// Check constraint C89
        /// 
        ///A NestedBusinessCollaboration MUST not be the source and MUST not be the target of an InformationFlow that targets to / sources from a BusinessCollaborationAction.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bcv"></param>
        private void checkC89(IValidationContext context, Package bcv)
        {
            //Get all NestedBusinessCollaboration
            IList<Element> nbcs = Utility.getAllElements(bcv, new List<Element>(),
                                                         UMM.bNestedCollaboration.ToString());


            foreach (Element nbc in nbcs)
            {
                foreach (Connector con in nbc.Connectors)
                {
                    if (con.Type == "InformationFlow")
                    {
                        //NestedBusinessCollaboration is the client
                        if (con.ClientID == nbc.ElementID)
                        {
                            //the supplier must not be a businesscollaborationaction
                            Element supplier = context.Repository.GetElementByID(con.SupplierID);
                            if (supplier.Stereotype == UMM.bCollaborationAction.ToString())
                            {
                                //Raise an error
                                context.AddValidationMessage(new ValidationMessage("Violation of constraint C89.",
                                                                                   "A NestedBusinessCollaboration MUST not be the source and MUST not be the target of an InformationFlow that targets to / sources from a BusinessCollaborationAction. \n\nPlease check NestedBusinessCollaboration " +
                                                                                   nbc.Name + " for invalid connectors.",
                                                                                   "BCV",
                                                                                   ValidationMessage.errorLevelTypes.
                                                                                       ERROR, bcv.PackageID));
                            }
                        }
                            //NestedBusinessCollaboration is the supplier
                        else
                        {
                            //the client must not be a businesscollaborationaction
                            Element client = context.Repository.GetElementByID(con.ClientID);
                            if (client.Stereotype == UMM.bCollaborationAction.ToString())
                            {
                                //Raise an error
                                context.AddValidationMessage(new ValidationMessage("Violation of constraint C89.",
                                                                                   "A NestedBusinessCollaboration MUST not be the source and MUST not be the target of an InformationFlow that targets to / sources from a BusinessCollaborationAction. \n\nPlease check NestedBusinessCollaboration " +
                                                                                   nbc.Name + " for invalid connectors.",
                                                                                   "BCV",
                                                                                   ValidationMessage.errorLevelTypes.
                                                                                       ERROR, bcv.PackageID));
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Check constraint C90
        /// The number of InformationFlows targeting a BusinessCollaborationAction MUST match the number of BusinessCollaborationPartitions contained in the BusinessCollaborationProtocol that is called by this BusinessCollaborationAction.
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bcv"></param>
        private void checkC90(IValidationContext context, Package bcv)
        {
            //Get all BusinessCollaborationActions
            IList<Element> bcas = Utility.getAllElements(bcv, new List<Element>(),
                                                         UMM.bCollaborationAction.ToString());

            foreach (Element bca in bcas)
            {
                //Get the business collaboration
                Element bc = null;
                if (bca.ClassifierID != 0)
                {
                    bc = context.Repository.GetElementByID(bca.ClassifierID);
                }

                if (bc == null)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C90.",
                                                                       "The number of InformationFlows targeting a BusinessCollaborationAction MUST match the number of BusinessCollaborationPartitions contained in the BusinessCollaborationProtocol that is called by this BusinessCollaborationAction. \n\nUnable to find the BusinessChoreography which is called by the BusinessCollaborationAction " +
                                                                       bca.Name, "BCV",
                                                                       ValidationMessage.errorLevelTypes.ERROR,
                                                                       bcv.PackageID));
                }
                else
                {
                    //Got the business collaboration - get the package and count
                    //the number of BusinessCollaborationPartitions
                    Package bcPackage = context.Repository.GetPackageByID(bc.PackageID);
                    int countPartitions = 0;
                    foreach (Element e in bcPackage.Elements)
                    {
                        if (e.Stereotype == UMM.bCPartition.ToString())
                        {
                            countPartitions++;
                        }
                    }

                    //Get the number of InformationFlows targeting the BusinessCollaborationAction
                    int numberOfFlows = 0;

                    foreach (Connector con in bca.Connectors)
                    {
                        if (con.Type == "InformationFlow" && con.Stereotype == "flow")
                        {
                            if (con.SupplierID == bca.ElementID)
                            {
                                numberOfFlows++;
                            }
                        }
                    }


                    //Compare the two numbers
                    if (countPartitions != numberOfFlows)
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C90.",
                                                                           "The number of InformationFlows targeting a BusinessCollaborationAction MUST match the number of BusinessCollaborationPartitions contained in the BusinessCollaborationProtocol that is called by this BusinessCollaborationAction. \n\nThe BusinessCollaborationAction " +
                                                                           bca.Name + " is targeted by " + numberOfFlows +
                                                                           " InformationFlows. The number of partitions in the Business Collaboration the BusinessCollaborationAction is calling is " +
                                                                           countPartitions +
                                                                           "\n\nMake sure, that the InformationFlows to the BusinessCollaborationAction are stereotyped correctly (<<flow>>).",
                                                                           "BCV",
                                                                           ValidationMessage.errorLevelTypes.ERROR,
                                                                           bcv.PackageID));
                    }
                }
            }
        }


        /// <summary>
        /// Check constraint C91
        /// 
        ///Either an AuthorizedRole classifying a BusinessCollaborationPartition 
        ///that is the source of an InformationFlow (UML standard: <<flow>>) targeting a 
        ///BusinessCollaborationAction MUST match an AuthorizedRole classifying a 
        ///BusinessCollaborationPartition in the BusinessCollaborationProtocol that is 
        ///called by this BusinessCollaborationAction or the InformationFlow must be 
        ///classified by an AuthorizedRole classifying a BusinessCollaborationPartition 
        ///in the BusinessCollaborationProtocol that is called by this 
        ///BusinessCollaborationAction.
        ///
        /// </summary>
        /// <param name="bcv"></param>
        private void checkC91(IValidationContext context, Package bcv)
        {
            //Check if there are any BusinesscollaborationActions
            IList<Element> bcas = Utility.getAllElements(bcv, new List<Element>(),
                                                         UMM.bCollaborationAction.ToString());


            if (bcas != null && bcas.Count != 0)
            {
                foreach (Element bca in bcas)
                {
                    var authorizedRolesofCalledCollaboration = new List<Element>();

                    //Get the BusinessCollaboration which the BusinessCollaborationAction is calling

                    if (bca.ClassfierID != 0)
                    {
                        Element cf = context.Repository.GetElementByID(bca.ClassifierID);
                        Package cfpackage = context.Repository.GetPackageByID(cf.PackageID);
                        foreach (Element el in cfpackage.Elements)
                        {
                            if (el.Stereotype == UMM.AuthorizedRole.ToString())
                            {
                                authorizedRolesofCalledCollaboration.Add(el);
                            }
                        }
                    }


                    var analyzed = new List<Int32>();

                    foreach (Connector con in bca.Connectors)
                    {
                        if (con.SupplierID == bca.ElementID)
                        {
                            if (con.Type == "InformationFlow" && con.Stereotype == "flow")
                            {
                                //Get the partition where the flow is coming from
                                Element partition = context.Repository.GetElementByID(con.ClientID);
                                if (partition.Stereotype == UMM.bCPartition.ToString())
                                {
                                    if (partition.ClassfierID != 0)
                                    {
                                        //Get the Authorized Role, whcih is classifying the partition
                                        Element classifier = context.Repository.GetElementByID(partition.ClassfierID);

                                        if (!analyzed.Contains(classifier.ElementID))
                                        {
                                            bool found = false;
                                            foreach (Element e in authorizedRolesofCalledCollaboration)
                                            {
                                                if (classifier.Name == e.Name)
                                                {
                                                    found = true;
                                                    break;
                                                }
                                            }

                                            if (!found)
                                            {
                                                context.AddValidationMessage(
                                                    new ValidationMessage("Violation of constraint C91.",
                                                                          "Either an AuthorizedRole classifying a BusinessCollaborationPartition that is the source of an InformationFlow (UML standard: <<flow>>) targeting a BusinessCollaborationAction MUST match an AuthorizedRole classifying a BusinessCollaborationPartition in the BusinessCollaborationProtocol that is called by this BusinessCollaborationAction or the InformationFlow must be classified by an AuthorizedRole classifying a BusinessCollaborationPartition in the BusinessCollaborationProtocol that is called by this BusinessCollaborationAction. \n\nFor the Authorized Role " +
                                                                          classifier.Name +
                                                                          " no equivalent Authorized Role in the Business Collaboration that is called by the BusinessColaborationAction " +
                                                                          bca.Name + " could be found.", "BCV",
                                                                          ValidationMessage.errorLevelTypes.ERROR,
                                                                          bcv.PackageID));
                                            }

                                            analyzed.Add(classifier.ElementID);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check the tagged values of the BusinessCollaborationView
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        private void checkTV_BusinessCollaborationView(IValidationContext context, Package p)
        {
            //Check the TaggedValues of the BusinessCollaborationView package
            new TaggedValueValidator().validatePackage(context, p);

            //Check the TaggedValues of the BusinessCollaborationUseCase
            IList<Element> bcucs = Utility.getAllElements(p, new List<Element>(), UMM.bCollaborationUC.ToString());
            foreach (Element bcuc in bcucs)
            {
                new TaggedValueValidator().validateElement(context, bcuc);
            }

            //Check the TaggedValues of the BusinessTransactionActions
            IList<Element> btas = Utility.getAllElements(p, new List<Element>(), UMM.bTransactionAction.ToString());
            foreach (Element bta in btas)
            {
                new TaggedValueValidator().validateElement(context, bta);
            }
        }
    }
}