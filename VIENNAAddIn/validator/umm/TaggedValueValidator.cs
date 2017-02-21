/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections.Generic;
using VIENNAAddIn.constants;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace VIENNAAddIn.validator.umm
{
    class TaggedValueValidator : AbstractValidator
    {
        internal override void validate(IValidationContext context, string scope)
        {
        }

        /// <summary>
        /// Validate the tagged values of the package 
        /// </summary>
        /// <returns></returns>
        internal void validatePackage(IValidationContext context, EA.Package package)
        {
            //Get the stereotype of the package            
            String stereotype = package.Element.Stereotype;
            
            //The top views
            if (stereotype == UMM.bRequirementsV.ToString() || stereotype == UMM.bChoreographyV.ToString()
                || stereotype == UMM.bInformationV.ToString() || stereotype == UMM.bCollModel.ToString() ||
                stereotype == UMM.bDomainV.ToString() || stereotype == UMM.bPartnerV.ToString() || 
                stereotype == UMM.bEntityV.ToString() || stereotype == UMM.bArea.ToString() || stereotype == UMM.ProcessArea.ToString()
                || stereotype == UMM.bDataV.ToString()|| stereotype == UMM.bChoreographyV.ToString() ||
                stereotype == UMM.bCollaborationV.ToString() || stereotype == UMM.bTransactionV.ToString() ||
                stereotype == UMM.bRealizationV.ToString() || stereotype == UMM.bInformationV.ToString())
            {
                validatePackageTaggedValues(context, package);
            }
        }


        /// <summary>
        /// Validate the tagged values of the element 
        /// </summary>
        internal void validateElement(IValidationContext context, EA.Element element)
        {
            //Get the stereotype of the element
            String stereotype = element.Stereotype;
            //Get the containing pckage
            EA.Package package = context.Repository.GetPackageByID(element.PackageID);

            //Business Process Use Case / BTUC / BCUC
            if (stereotype == UMM.bProcessUC.ToString() || stereotype == UMM.bTransactionUC.ToString() || stereotype == UMM.bCollaborationUC.ToString())
            {
                validateBPUC_BTUC_BCUC_TaggedValues(context, element, package);
            }
            //Stakeholder and BusinessPartner
            else if (stereotype == UMM.Stakeholder.ToString() || stereotype == UMM.bPartner.ToString())
            {
                validateBusinessPartnerAndStakeholder(context, element, package);
            }
            //BusinessTransaction
            else if (stereotype == UMM.bTransaction.ToString())
            {
                validateBusinessTransaction(context, element, package);
            }
            //ReqAction/ResAction
            else if (stereotype == UMM.ReqAction.ToString() || stereotype == UMM.ResAction.ToString())
            {
                validateBusinessActions(context, element, package);
            }
            //ReqInfPin/ResInfPin
            else if (stereotype == UMM.ResInfPin.ToString() || stereotype == UMM.ReqInfPin.ToString())
            {
                validateInformationPins(context, element, package);
            }
            //BusinessTransactionActions
            else if (stereotype == UMM.bTransactionAction.ToString())
            {
                validateBusinessTransactionAction(context, element, package);
            }
        }




        /// <summary>
        /// Validate the tagged values of participates and isOfInterestTo connectors of the passed
        /// element in the containing package
        /// </summary>
        internal void validateConnectors(IValidationContext context, List<EA.Connector> connectors)
        {            
            foreach (EA.Connector con in connectors)
            {
                bool foundInternest = false;
                foreach (EA.ConnectorTag ctag in con.TaggedValues)
                {
                    if (ctag.Name == UMMTaggedValues.interest.ToString())
                    {
                        foundInternest = true;
                    }
                }

                if (!foundInternest)
                {
                    //Get source
                    EA.Element source = context.Repository.GetElementByID(con.ClientID);
                    EA.Element target = context.Repository.GetElementByID(con.SupplierID);
                    EA.Package package = context.Repository.GetPackageByID(source.PackageID);

                    context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getConnectorError("interest", con, source, target, package), "BRV", ValidationMessage.errorLevelTypes.WARN, package.PackageID));
                }
            }
        }

        /// <summary>
        /// Validate BusinessTransactionAction
        /// </summary>
        /// <param name="context"></param>
        /// <param name="e"></param>
        /// <param name="package"></param>
        private void validateBusinessTransactionAction(IValidationContext context, EA.Element e, EA.Package package)
        {
            bool foundTimeToPerform = false;
            bool foundIsConcurrent = false;

            String valueTimeToPerform = "";
            String valueIsConcurrent = "";


            foreach (EA.TaggedValue tv in e.TaggedValues)
            {
                String n = tv.Name;
                String v = tv.Value;

                if (n == UMMTaggedValues.timeToPerform.ToString())
                {
                    foundTimeToPerform = true;
                    valueTimeToPerform = v;
                }
                else if (n == UMMTaggedValues.isConcurrent.ToString())
                {
                    foundIsConcurrent = true;
                    valueIsConcurrent = v;
                }
            }


            if (!foundTimeToPerform)
            {
                context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getElementError("timeToPerform", package, e), "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
            }
            else
            {
                if (!isDuration(valueTimeToPerform))
                {
                    context.AddValidationMessage(new ValidationMessage("Wrong tagged value.", "Tagged value timeToPerform of element " + e.Name + " in package " + package.Name + " has an invalid value. \n\nAllowed values are xsd:duration or null. E.g P5Y2M10DT15H4M3S represents 5 years, 2 months, 10 days, 15 hours, 4 mintues and 3 seconds.", "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
                }
            }


            if (!foundIsConcurrent)
            {
                context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getElementError("isConcurrent", package, e), "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
            }
            else
            {
                if (!isBoolean(valueIsConcurrent))
                {
                    context.AddValidationMessage(new ValidationMessage("Wrong tagged value.", "Tagged value isConcurrent of element " + e.Name + " in package " + package.Name + " has an invalid value. \n\nAllowed values are true and false.", "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
                }
            }
        }




        /// <summary>
        /// Validate BusinessActions
        /// </summary>
        /// <param name="context"></param>
        /// <param name="e"></param>
        /// <param name="package"></param>
        private void validateInformationPins(IValidationContext context, EA.Element e, EA.Package package)
        {
            bool foundIsConfidential = false;
            bool foundIsTamperProof = false;
            bool foundIsAuthenticated = false;

            String valueIsConfidential = "";
            String valueIsTamperProof = "";
            String valueIsAuthenticated = "";

            EA.Element element = context.Repository.GetElementByID(e.ParentID);


            foreach (EA.TaggedValue tv in e.TaggedValues)
            {
                String n = tv.Name;
                String v = tv.Value;

                if (n == UMMTaggedValues.isConfidential.ToString())
                {
                    foundIsConfidential = true;
                    valueIsConfidential = v;
                }
                else if (n == UMMTaggedValues.isTamperProof.ToString())
                {
                    foundIsTamperProof = true;
                    valueIsTamperProof = v;
                }
                else if (n == UMMTaggedValues.isAuthenticated.ToString())
                {
                    foundIsAuthenticated = true;
                    valueIsAuthenticated = v;
                }
            }

            if (!foundIsConfidential)
            {
                context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getElementError("isConfidential", package, element), "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
            }
            else
            {
                if (!isBoolean(valueIsConfidential))
                {
                    context.AddValidationMessage(new ValidationMessage("Wrong tagged value.", "Tagged value isConfidential of element " + element.Name + " in package " + package.Name + " has an invalid value. \n\nAllowed values are true and false.", "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
                }
            }


            if (!foundIsTamperProof)
            {
                context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getElementError("isTamperProof", package, element), "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
            }
            else
            {
                if (!isBoolean(valueIsTamperProof))
                {
                    context.AddValidationMessage(new ValidationMessage("Wrong tagged value.", "Tagged value isTamperProof of element " + element.Name + " in package " + package.Name + " has an invalid value. \n\nAllowed values are true and false.", "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
                }
            }

            if (!foundIsAuthenticated)
            {
                context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getElementError("isAuthenticated", package, element), "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
            }
            else
            {
                if (!isBoolean(valueIsAuthenticated))
                {
                    context.AddValidationMessage(new ValidationMessage("Wrong tagged value.", "Tagged value isAuthenticated of element " + element.Name + " in package " + package.Name + " has an invalid value. \n\nAllowed values are true and false.", "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
                }
            }
        }



        /// <summary>
        /// Validate BusinessActions
        /// </summary>
        /// <param name="context"></param>
        /// <param name="element"></param>
        /// <param name="package"></param>
        private void validateBusinessActions(IValidationContext context, EA.Element element, EA.Package package)
        {
            bool foundIsAuthorizationRequired = false;
            bool foundIsNonRepudationRequired = false;
            bool foundIsNonRepudationReceiptRequired = false;
            bool foundTimeToAcknowledgeReceipt = false;
            bool foundTimeToAcknowledgeProcessing = false;
            bool foundIsIntelligibleCheckRequired = false;

            bool foundTimeToRespond = false;
            bool foundRetryCount = false;


            String valueIsAuthorizationRequired = "";
            String valueIsNonRepudationRequired = "";
            String valueIsNonRepudationReceiptRequired = "";
            String valueTimeToAcknowledgeReceipt = "";
            String valueTimeToAcknowledgeProcessing = "";
            String valueIsIntelligibleCheckRequired = "";
            String valueTimeToRespond = "";
            String valueRetrycount = "";


            foreach (EA.TaggedValue tv in element.TaggedValues)
            {
                String n = tv.Name;
                String v = tv.Value;

                if (n == UMMTaggedValues.isAuthorizationRequired.ToString())
                {
                    foundIsAuthorizationRequired = true;
                    valueIsAuthorizationRequired = v;
                }
                else if (n == UMMTaggedValues.isNonRepudiationRequired.ToString())
                {
                    foundIsNonRepudationRequired = true;
                    valueIsNonRepudationRequired = v;
                }
                else if (n == UMMTaggedValues.isNonRepudiationReceiptRequired.ToString())
                {
                    foundIsNonRepudationReceiptRequired = true;
                    valueIsNonRepudationReceiptRequired = v;

                }
                else if (n == UMMTaggedValues.timeToAcknowledgeReceipt.ToString())
                {
                    foundTimeToAcknowledgeReceipt = true;
                    valueTimeToAcknowledgeReceipt = v;
                }
                else if (n == UMMTaggedValues.timeToAcknowledgeProcessing.ToString())
                {
                    foundTimeToAcknowledgeProcessing = true;
                    valueTimeToAcknowledgeProcessing = v;
                }
                else if (n == UMMTaggedValues.isIntelligibleCheckRequired.ToString())
                {
                    foundIsIntelligibleCheckRequired = true;
                    valueIsIntelligibleCheckRequired = v;
                }
                else if (n == UMMTaggedValues.timeToRespond.ToString())
                {
                    foundTimeToRespond = true;
                    valueTimeToRespond = v;
                }
                else if (n == UMMTaggedValues.retryCount.ToString())
                {
                    foundRetryCount = true;
                    valueRetrycount = v;
                }
            }


            if (!foundIsAuthorizationRequired)
            {
                context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getElementError("isAuthorizationRequired", package, element), "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
            }
            else {
                if (!isBoolean(valueIsAuthorizationRequired))
                {
                    context.AddValidationMessage(new ValidationMessage("Wrong tagged value.", "Tagged value isAuthorizationRequired of element " + element.Name + " in package " + package.Name + " has an invalid value. \n\nAllowed values are true and false.", "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
                }
            }


            if (!foundIsNonRepudationRequired)
            {
                context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getElementError("isNonRepudiationRequired", package, element), "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
            }
            else
            {
                if (!isBoolean(valueIsNonRepudationRequired))
                {
                    context.AddValidationMessage(new ValidationMessage("Wrong tagged value.", "Tagged value isNonRepudiationRequired of element " + element.Name + " in package " + package.Name + " has an invalid value. \n\nAllowed values are true and false.", "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
                }
            }

            if (!foundIsNonRepudationReceiptRequired)
            {
                context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getElementError("isNonRepudiationReceiptRequired", package, element), "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
            }
            else
            {
                if (!isBoolean(valueIsNonRepudationReceiptRequired))
                {
                    context.AddValidationMessage(new ValidationMessage("Wrong tagged value.", "Tagged value isNonRepudiationReceiptRequired of element " + element.Name + " in package " + package.Name + " has an invalid value. \n\nAllowed values are true and false.", "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
                }
            }

            if (!foundTimeToAcknowledgeReceipt)
            {
                context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getElementError("timeToAcknowledgeReceipt", package, element), "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
            }
            else
            {
                if (!isDuration(valueTimeToAcknowledgeReceipt))
                {
                    context.AddValidationMessage(new ValidationMessage("Wrong tagged value.", "Tagged value timeToAcknowledgeReceipt of element " + element.Name + " in package " + package.Name + " has an invalid value. \n\nAllowed values are xsd:duration or null. E.g P5Y2M10DT15H4M3S represents 5 years, 2 months, 10 days, 15 hours, 4 mintues and 3 seconds.", "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
                }
            }

            if (!foundTimeToAcknowledgeProcessing)
            {
                context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getElementError("timeToAcknowledgeProcessing", package, element), "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
            }
            else
            {
                if (!isDuration(valueTimeToAcknowledgeProcessing))
                {
                    context.AddValidationMessage(new ValidationMessage("Wrong tagged value.", "Tagged value timeToAcknowledgeProcessing of element " + element.Name + " in package " + package.Name + " has an invalid value. \n\nAllowed values are xsd:duration or null. E.g P5Y2M10DT15H4M3S represents 5 years, 2 months, 10 days, 15 hours, 4 mintues and 3 seconds.", "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
                }
            }

            if (!foundIsIntelligibleCheckRequired)
            {
                context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getElementError("isIntelligibleCheckRequired", package, element), "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
            }
            else
            {
                if (!isBoolean(valueIsIntelligibleCheckRequired))
                {
                    context.AddValidationMessage(new ValidationMessage("Wrong tagged value.", "Tagged value isIntelligibleCheckRequired of element " + element.Name + " in package " + package.Name + " has an invalid value. \n\nAllowed values are true and false.", "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
                }
            }

            //A RequestingAction has two additional taggedvalues in comparison to a RespondingAction
            if (element.Stereotype == UMM.ReqAction.ToString())
            {

                if (!foundTimeToRespond)
                {
                    context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getElementError("timeToRespond", package, element), "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
                }
                else
                {
                    if (!isDuration(valueTimeToRespond))
                    {
                        context.AddValidationMessage(new ValidationMessage("Wrong tagged value.", "Tagged value timeToRespond of element " + element.Name + " in package " + package.Name + " has an invalid value. \n\nAllowed values are xsd:duration or null. E.g P5Y2M10DT15H4M3S represents 5 years, 2 months, 10 days, 15 hours, 4 mintues and 3 seconds.", "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
                    }
                }

                if (!foundRetryCount)
                {
                    context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getElementError("retryCount", package, element), "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
                }
                else
                {
                    if (!isPositiveInteger(valueRetrycount))
                    {
                        context.AddValidationMessage(new ValidationMessage("Wrong tagged value.", "Tagged value retryCount of element " + element.Name + " in package " + package.Name + " has an invalid value. \n\nRetryCount must be a figure greater or equal zero.", "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
                    }
                }

            }
        }




        /// <summary>
        /// Validate a abusiness transaction
        /// </summary>
        /// <param name="context"></param>
        /// <param name="element"></param>
        /// <param name="package"></param>
        private void validateBusinessTransaction(IValidationContext context, EA.Element element, EA.Package package)
        {
            bool foundBusinessTransactionType = false;
            bool foundIsSecureTransportRequired = false;
            String valueBusinessTransactionType = "";
            String valueIsSecureTransportRequired = "";

            foreach (EA.TaggedValue tv in element.TaggedValues)
            {
                if (tv.Name == UMMTaggedValues.businessTransactionType.ToString())
                {
                    foundBusinessTransactionType = true;
                    valueBusinessTransactionType = tv.Value;
                }
                else if (tv.Name == UMMTaggedValues.isSecureTransportRequired.ToString()) {
                    foundIsSecureTransportRequired = true;
                    valueIsSecureTransportRequired = tv.Value;
                }
            }

            if (!foundBusinessTransactionType)
            {
                context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getElementError("businessTransactionType", package, element), "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
            }
            else
            {
                //Found the tagged value - check if the values are ok
                if (!(valueBusinessTransactionType == "CommercialTransaction" || 
                    valueBusinessTransactionType == "Request/Confirm" || valueBusinessTransactionType == "Query/Response" 
                    || valueBusinessTransactionType == "Request/Response" 
                    || valueBusinessTransactionType == "Notification" 
                    || valueBusinessTransactionType == "InformationDistribution"))
                {
                    context.AddValidationMessage(new ValidationMessage("Wrong tagged value.", "Tagged value businessTransactionType of element " + element.Name + " in package " + package.Name + " has an invalid value. \n\nAllowed values are: Commercial Transaction, Request/Confirm, Query/Response, Request/Response, Notification, Information Distribution.", "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
                }
            }

            if (!foundIsSecureTransportRequired)
            {
                context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getElementError("isSecureTransportRequired", package, element), "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
            }
            else
            {
                if (!isBoolean(valueIsSecureTransportRequired))
                {
                    context.AddValidationMessage(new ValidationMessage("Wrong tagged value.", "Tagged value isSecureTransportRequired of element " + element.Name + " in package " + package.Name + " has an invalid value. \n\nAllowed values are true and false.", "BCV", ValidationMessage.errorLevelTypes.ERROR, package.PackageID));
                }
            }
        }


        /// <summary>
        /// Validate the Stakeholder
        /// </summary>
        /// <param name="context"></param>
        /// <param name="element"></param>
        /// <param name="package"></param>
        private void validateBusinessPartnerAndStakeholder(IValidationContext context, EA.Element element, EA.Package package)
        {
            bool foundInterest = false;

            foreach (EA.TaggedValue tv in element.TaggedValues)
            {
                if (tv.Name == UMMTaggedValues.interest.ToString())
                {
                    foundInterest = true;
                }
            }

            if (!foundInterest)
            {
                context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getElementError("interest", package, element), "BRV", ValidationMessage.errorLevelTypes.WARN, package.PackageID));
            }
        }


        /// <summary>
        /// Validate tagged values of an element
        /// </summary>
        /// <param name="context"></param>
        /// <param name="element"></param>
        /// <param name="package"></param>
        private void validateBPUC_BTUC_BCUC_TaggedValues(IValidationContext context, EA.Element element, EA.Package package)
        {
            bool foundDefinition = false;
            bool foundBeginsWhen = false;
            bool foundPreCondition = false;
            bool foundEndsWhen = false;
            bool foundPostCondition = false;
            bool foundExceptions = false;
            bool foundAction = false;
            

            foreach (EA.TaggedValue tv in element.TaggedValues)
            {
                String n = tv.Name;

                if (n == UMMTaggedValues.definition.ToString())
                    foundDefinition = true;
                if (n == UMMTaggedValues.beginsWhen.ToString())
                    foundBeginsWhen = true;
                if (n == UMMTaggedValues.preCondition.ToString())
                    foundPreCondition = true;
                if (n == UMMTaggedValues.endsWhen.ToString())
                    foundEndsWhen = true;
                if (n == UMMTaggedValues.postCondition.ToString())
                    foundPostCondition = true;
                if (n == UMMTaggedValues.exceptions.ToString())
                    foundExceptions = true;
                if (n == UMMTaggedValues.actions.ToString())
                    foundAction = true;                

            }

            if (!foundDefinition)
            {
                context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getElementError("definition", package, element), "BRV", ValidationMessage.errorLevelTypes.WARN, package.PackageID));
            }

            if (!foundBeginsWhen)
            {
                context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getElementError("beginsWhen", package, element), "BRV", ValidationMessage.errorLevelTypes.WARN, package.PackageID));
            }

            if (!foundPreCondition)
            {
                context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getElementError("precondition", package, element), "BRV", ValidationMessage.errorLevelTypes.WARN, package.PackageID));
            }

            if (!foundEndsWhen)
            {
                context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getElementError("endsWhen", package, element), "BRV", ValidationMessage.errorLevelTypes.WARN, package.PackageID));
            }

            if (!foundPostCondition)
            {
                context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getElementError("postCondition", package, element), "BRV", ValidationMessage.errorLevelTypes.WARN, package.PackageID));
            }

            if (!foundExceptions)
            {
                context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getElementError("exceptions", package, element), "BRV", ValidationMessage.errorLevelTypes.WARN, package.PackageID));
            }

            if (!foundAction)
            {
                context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getElementError("actions", package, element), "BRV", ValidationMessage.errorLevelTypes.WARN, package.PackageID));
            }
        }

        /// <summary>
        /// Validate the tagged values of a package
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        internal void validatePackageTaggedValues(IValidationContext context, EA.Package p)
        {            
            bool foundJustification = false;
            bool foundBusinessTerm = false;
            bool foundCopyright = false;
            bool foundOwner = false;
            bool foundReference = false;
            bool foundStatus = false;
            bool foundURI = false;
            bool foundVersion = false;

            bool foundObjective = false;
            bool foundScope = false;
            bool foundBusinessOpportunity = false;


            foreach (EA.TaggedValue t in p.Element.TaggedValues)
            {
                String n = t.Name;

                if (n == UMMTaggedValues.justification.ToString())
                    foundJustification = true;
                else if (n == UMMTaggedValues.businessTerm.ToString())
                    foundBusinessTerm = true;
                else if (n == UMMTaggedValues.copyright.ToString())
                    foundCopyright = true;
                else if (n == UMMTaggedValues.owner.ToString())
                    foundOwner = true;
                else if (n == UMMTaggedValues.reference.ToString())
                    foundReference = true;
                else if (n == UMMTaggedValues.status.ToString())
                    foundStatus = true;
                else if (n == UMMTaggedValues.URI.ToString())
                    foundURI = true;
                else if (n == UMMTaggedValues.version.ToString())
                    foundVersion = true;
                else if (n == UMMTaggedValues.objective.ToString())
                    foundObjective = true;
                else if (n == UMMTaggedValues.scope.ToString())
                    foundScope = true;
                else if (n == UMMTaggedValues.businessOpportunity.ToString())
                    foundBusinessOpportunity = true;
            }

            //Raise an error if a taggedvalue is missing

            //Justification is a tagged value only occurring in  the bCollModel
            if (p.Element.Stereotype == UMM.bCollModel.ToString() && !foundJustification)
            {
                context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getPackageError("justification", p), "BCM", ValidationMessage.errorLevelTypes.WARN, p.PackageID));
            }

            //Objective is a tagged value only occurring in a BusinessArea or ProcessArea
            if ((p.Element.Stereotype == UMM.bArea.ToString() || p.Element.Stereotype == UMM.ProcessArea.ToString()) && !foundObjective)
            {
                context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getPackageError("objective", p), "BRV", ValidationMessage.errorLevelTypes.WARN, p.PackageID));
            }

            //Scope is a tagged value only occurring in a BusinessArea or ProcessArea
            if ((p.Element.Stereotype == UMM.bArea.ToString() || p.Element.Stereotype == UMM.ProcessArea.ToString()) && !foundScope)
            {
                context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getPackageError("scope", p), "BRV", ValidationMessage.errorLevelTypes.WARN, p.PackageID));
            }

            //businessOpportunity is a tagged value only occurring in a BusinessArea or ProcessArea
            if ((p.Element.Stereotype == UMM.bArea.ToString() || p.Element.Stereotype == UMM.ProcessArea.ToString()) && !foundBusinessOpportunity)
            {
                context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getPackageError("businessOpportunity", p), "BRV", ValidationMessage.errorLevelTypes.WARN, p.PackageID));
            }

            if (!foundBusinessTerm)
            {
                context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getPackageError("businessTerm", p), "BCM", ValidationMessage.errorLevelTypes.WARN, p.PackageID));
            }

            if (!foundCopyright)
            {
                context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getPackageError("copyright", p), "BCM", ValidationMessage.errorLevelTypes.WARN, p.PackageID));
            }

            if (!foundOwner)
            {
                context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getPackageError("owner", p), "BCM", ValidationMessage.errorLevelTypes.WARN, p.PackageID));
            }

            if (!foundReference)
            {
                context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getPackageError("reference", p), "BCM", ValidationMessage.errorLevelTypes.WARN, p.PackageID));
            }

            if (!foundStatus)
            {
                context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getPackageError("status", p), "BCM", ValidationMessage.errorLevelTypes.WARN, p.PackageID));
            }

            if (!foundURI)
            {
                context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getPackageError("URI", p), "BCM", ValidationMessage.errorLevelTypes.WARN, p.PackageID));
            }

            if (!foundVersion)
            {
                context.AddValidationMessage(new ValidationMessage("Missing tagged value.", getPackageError("version", p), "BCM", ValidationMessage.errorLevelTypes.WARN, p.PackageID));
            }
        }

        /// <summary>
        /// Returns the package error
        /// </summary>
        /// <param name="taggedValue"></param>
        /// <param name="package"></param>
        /// <returns></returns>
        private String getPackageError(String taggedValue, EA.Package package)
        {
            return "Tagged value " + taggedValue + " of package " + package.Name + " <<" +package .Element.Stereotype+ ">> is missing.";
        }



        /// <summary>
        /// Returns the package error
        /// </summary>
        /// <param name="taggedValue"></param>
        /// <param name="package"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        private String getElementError(String taggedValue, EA.Package package, EA.Element element)
        {
            return "Tagged value " + taggedValue + " of element " + element.Name + " in package "+ package.Name +" <<" + package.Element.Stereotype + ">> is missing.";
        }


        /// <summary>
        /// Return the error message for a connector
        /// </summary>
        /// <param name="taggedValue"></param>
        /// <param name="con"></param>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="package"></param>
        /// <returns></returns>
        private String getConnectorError(String taggedValue, EA.Connector con, EA.Element source, EA.Element target, EA.Package package)
        {
            
            return "Tagged value " + taggedValue + " of connector with stereotype " + con.Stereotype + " between " + source.Name + " and " + target.Name + " in package " + package.Name + " <<" + package.Element.Stereotype + ">> is missing.";

        }



        /// <summary>
        /// Returns true if the passed string is either true or false
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool isBoolean(String s)
        {
            if (s != null)
            {
                if (s.ToLower().Trim() == "true" || s.ToLower().Trim() == "false")
                    return true;
            }

            return false;
        }


        /// <summary>
        /// Returns true if s is an Integer >= 0 or NULL
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static bool isPositiveInteger(String s)
        {
            if (s == null || s.ToLower() == "null")
                return true;


            try
            {
                Int32 i = Int32.Parse(s);
                if (i < 0)
                    return false;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            return false;

        }



        /// <summary>
        /// Check if the passed TaggedValue is of type duration
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool isDuration(String s)
        {
            if (s == null || s.ToLower() == "null")
            {
                return true;
            }
            try
            {
                TimeSpan t = SoapDuration.Parse(s);
                /* if a xsd:duration value is not parsable a zero TimeSpan is returned
                     * which means that the value isnt a correct xsd:duration */
                if (!t.Equals(TimeSpan.Zero))
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
