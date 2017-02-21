/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections.Generic;
using VIENNAAddIn.constants;
using VIENNAAddIn.common;

namespace VIENNAAddIn.validator.umm.bcv
{
    class bRealizationVValidator : AbstractValidator
    {
        /// <summary>
        /// Validate the BusinessRealizationView
        /// </summary>
        internal override void validate(IValidationContext parentContext, string scope)
        {
            ErrorCheckingValidationContext context = new ErrorCheckingValidationContext(parentContext);
            var brv = context.Repository.GetPackageByID(Int32.Parse(scope));
            checkTV_BusinessRealizationView(context, brv);
            checkC92(context, brv);
            if (context.HasError) return;
            checkC93(context, brv);
            checkC94(context, brv);
            checkC95(context, brv);
            checkC96(context, brv);
            checkC97(context, brv);
            checkC98(context, brv);
            checkC99(context, brv);
        }


        /// <summary>
        /// Check constraint C92
        /// </summary>
        /// <param name="context"></param>
        /// <param name="brv"></param>
        private void checkC92(IValidationContext context, EA.Package brv)
        {
            int count_BR = 0;
            int count_AR = 0;


            foreach (EA.Element e in brv.Elements)
            {

                //Count the BusinessRealizations
                if (e.Stereotype == UMM.bRealization.ToString())
                {
                    count_BR++;
                }
                else if (e.Stereotype == UMM.AuthorizedRole.ToString())
                {
                    count_AR++;
                }
            }

            if (count_BR != 1)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C92.", "A BusinessRealizationView MUST contain exactly one BusinessRealization, two to many AuthorizedRoles, and two to many participates associations. \n\nFound " + count_BR + " BusinessRealizations.", "BCV", ValidationMessage.errorLevelTypes.ERROR, brv.PackageID));
            }

            if (count_AR < 2)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C92.", "A BusinessRealizationView MUST contain exactly one BusinessRealization, two to many AuthorizedRoles, and two to many participates associations. \n\nFound " + count_AR + " AuthorizedRoles.", "BCV", ValidationMessage.errorLevelTypes.ERROR, brv.PackageID));
            }

            //We do not check the participates associations here but in the next constraint
        }


        /// <summary>
        /// Check constraint C93
        /// 
        /// A BusinessRealization MUST be associated with two to many AuthorizedRoles via stereotyped binary participates associations.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="brv"></param>
        /// <returns></returns>
        private void checkC93(IValidationContext context, EA.Package brv)
        {
            //Get the BusinessRealization
            EA.Element br = Utility.getElementFromPackage(brv, UMM.bRealization.ToString());

            if (br != null)
            {

                int countConnections = 0;
                foreach (EA.Connector con in br.Connectors)
                {

                    if (con.Stereotype == UMM.participates.ToString())
                    {
                        //The realization must be the supplier
                        if (con.SupplierID == br.ElementID)
                        {
                            EA.Element client = context.Repository.GetElementByID(con.ClientID);
                            if (client.Stereotype == UMM.AuthorizedRole.ToString())
                            {
                                countConnections++;

                            }
                        }
                    }
                }

                if (countConnections < 2)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C93.", "A BusinessRealization MUST be associated with two to many AuthorizedRoles via stereotyped binary participates associations.\n\nInvalid number of assocations found: " + countConnections, "BCV", ValidationMessage.errorLevelTypes.ERROR, brv.PackageID));
                }
            }
        }



        /// <summary>
        /// Check constraint C94
        /// A BusinessRealization MUST be the source of exactly one realization 
        /// dependency to a BusinessCollaborationUseCase.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="brv"></param>
        /// <returns></returns>
        private void checkC94(IValidationContext context, EA.Package brv)
        {
            //Get the BusinessRealization
            EA.Element br = Utility.getElementFromPackage(brv, UMM.bRealization.ToString());

            if (br != null)
            {

                int count = 0;
                //Get the dependencies
                foreach (EA.Connector con in br.Connectors)
                {
                    if (con.Type == "Realisation" && con.Stereotype == "realizes")
                    {

                        if (con.ClientID == br.ElementID)
                        {
                            EA.Element supplier = context.Repository.GetElementByID(con.SupplierID);
                            if (supplier.Stereotype == UMM.bCollaborationUC.ToString())
                            {
                                count++;
                            }
                        }
                    }
                }

                if (count != 1)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C94.", "A BusinessRealization MUST be the source of exactly one realization dependency to a BusinessCollaborationUseCase. \n\nFound invalid number of realize dependencies: " + count, "BCV", ValidationMessage.errorLevelTypes.ERROR, brv.PackageID));
                }
            }
        }


        /// <summary>
        /// Check constraint C95
        /// A BusinessRealization MUST NOT be the 
        /// source or target of an include or extends association.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="brv"></param>
        /// <returns></returns>
        private void checkC95(IValidationContext context, EA.Package brv)
        {
            //Get the BusinessRealization
            EA.Element br = Utility.getElementFromPackage(brv, UMM.bRealization.ToString());

            if (br != null)
            {
                foreach (EA.Connector con in br.Connectors)
                {

                    if (con.Stereotype == "extend" || con.Stereotype == "include")
                    {
                        context.AddValidationMessage(new ValidationMessage("Violation of constraint C95.", "A BusinessRealization MUST NOT be the source or target of an include or extends association. \n\nBusinessRealization " + br.Name + " has invalid connectors.", "BCV", ValidationMessage.errorLevelTypes.ERROR, brv.PackageID));
                    }
                }


            }
        }


        /// <summary>
        /// Check constraint C96
        /// All dependencies from/to an AuthorizedRole must be stereotyped as mapsTo.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="brv"></param>
        /// <returns></returns>
        private void checkC96(IValidationContext context, EA.Package brv)
        {
            //Get the Authorized Roles
            IList<EA.Element> aroles = Utility.getAllElements(brv, new List<EA.Element>(), UMM.AuthorizedRole.ToString());

            foreach (EA.Element ar in aroles)
            {
                //Get the dependencies
                foreach (EA.Connector con in ar.Connectors)
                {

                    if (con.Type == "Dependency")
                    {
                        if (con.Stereotype != UMM.mapsTo.ToString())
                        {
                            context.AddValidationMessage(new ValidationMessage("Violation of constraint C96.", "All dependencies from/to an AuthorizedRole must be stereotyped as mapsTo.\n\nBusiness Realization " + brv.Name + " has invalid connectors.", "BDV", ValidationMessage.errorLevelTypes.ERROR, brv.PackageID));
                        }
                    }
                }
            }
        }



        /// <summary>
        /// Check constraint C97
        /// An AuthorizedRole, which participates in a BusinessRealization, must be 
        /// the target of exactly one mapsTo dependency starting from a BusinessPartner.
        /// Furthermore the AuthorizedRole, which participates in the BusinessRealization
        /// must be the source of exactly one mapsTo dependency targeting an AuthorizedRole 
        /// participating in a BusinessCollaborationUseCase.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="brv"></param>
        /// <returns></returns>
        private void checkC97(IValidationContext context, EA.Package brv)
        {
            //Get the Authorized Roles
            IList<EA.Element> aroles = Utility.getAllElements(brv, new List<EA.Element>(), UMM.AuthorizedRole.ToString());

            foreach (EA.Element ar in aroles)
            {
                int countTo = 0;
                int countFrom = 0;
                bool doesParticipate = doesParticipateInBR(context, ar);


                foreach (EA.Connector con in ar.Connectors)
                {

                    if (doesParticipate)
                    {

                        if (con.Type == "Dependency" && con.Stereotype == UMM.mapsTo.ToString())
                        {
                            //Count the mapsTo leading to this authorized role
                            if (con.SupplierID == ar.ElementID)
                            {

                                EA.Element client = context.Repository.GetElementByID(con.ClientID);
                                if (client.Stereotype == UMM.bPartner.ToString())
                                {
                                    countTo++;
                                }
                            }
                            //Count the mapsTo emanating from this authorized role
                            if (con.ClientID == ar.ElementID)
                            {
                                EA.Element supplier = context.Repository.GetElementByID(con.SupplierID);
                                if (supplier.Stereotype == UMM.AuthorizedRole.ToString())
                                {
                                    if (doesParticipateinBC(context, supplier))
                                    {
                                        countFrom++;
                                    }
                                }


                            }
                        }




                    }
                }

                if (doesParticipate && countTo != 1)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C97.", "An AuthorizedRole, which participates in a BusinessRealization, must be the target of exactly one mapsTo dependency starting from a BusinessPartner. Furthermore the AuthorizedRole, which participates in the BusinessRealization must be the source of exactly one mapsTo dependency targeting an AuthorizedRole participating in a BusinessCollaborationUseCase. \n\nAuthorized Role " + ar.Name + " has an invalid number of incoming mapsTo dependencies: " + countTo, "BCV", ValidationMessage.errorLevelTypes.ERROR, brv.PackageID));
                }

                if (doesParticipate && countFrom != 1)
                {
                    context.AddValidationMessage(new ValidationMessage("Violation of constraint C97.", "An AuthorizedRole, which participates in a BusinessRealization, must be the target of exactly one mapsTo dependency starting from a BusinessPartner. Furthermore the AuthorizedRole, which participates in the BusinessRealization must be the source of exactly one mapsTo dependency targeting an AuthorizedRole participating in a BusinessCollaborationUseCase. \n\nAuthorized Role " + ar.Name + " has an invalid number of outgoing mapsTo dependencies: " + countFrom, "BCV", ValidationMessage.errorLevelTypes.ERROR, brv.PackageID));
                }
            }
        }



        /// <summary>
        /// Check constraint C98
        /// AuthorizedRoles in a BusinessRealizationView must have a unique 
        /// name within the scope of the package, they are located in
        /// </summary>
        /// <param name="context"></param>
        /// <param name="brv"></param>
        /// <returns></returns>
        private void checkC98(IValidationContext context, EA.Package brv)
        {
            //Get the Authorized Roles
            IList<EA.Element> aroles = Utility.getAllElements(brv, new List<EA.Element>(), UMM.AuthorizedRole.ToString());


            foreach (EA.Element ar in aroles)
            {
                String name = ar.Name;
                foreach (EA.Element ar1 in aroles)
                {
                    if (ar1 != ar)
                    {
                        if (ar1.Name == name)
                        {
                            context.AddValidationMessage(new ValidationMessage("Violation of constraint C98.", "AuthorizedRoles in a BusinessRealizationView must have a unique name within the scope of the package, they are located in. \n\nThe following name is used multiple times: " + ar.Name, "BCV", ValidationMessage.errorLevelTypes.ERROR, brv.PackageID));
                            return;
                        }
                    }
                }
            }
        }



        /// <summary>
        /// Check constraint C99
        /// 
        /// The number of AuthorizedRoles participating in a BusinessCollaborationUseCase 
        /// MUST match the number of AuthorizedRoles participating in the BusinessRealization 
        /// realizing this BusinessCollaborationUseCase
        /// </summary>
        /// <param name="context"></param>
        /// <param name="brv"></param>
        /// <returns></returns>
        private void checkC99(IValidationContext context, EA.Package brv)
        {
            //Get the Authorized Roles
            IList<EA.Element> aroles = Utility.getAllElements(brv, new List<EA.Element>(), UMM.AuthorizedRole.ToString());

            int countBR = 0;
            int countBC = 0;

            //Count the AR of the BusinessRealization
            foreach (EA.Element ar in aroles)
            {

                if (doesParticipateInBR(context, ar))
                {
                    countBR++;
                }
            }

            //Count the AR of the BCUC
            //Get the BusinessRealization first
            EA.Element bcuc = null;
            EA.Element br = Utility.getElementFromPackage(brv, UMM.bRealization.ToString());
            if (br != null)
            {
                foreach (EA.Connector con in br.Connectors)
                {
                    if (con.Type == "Realisation" && con.Stereotype == "realizes")
                    {
                        if (con.ClientID == br.ElementID)
                        {
                            EA.Element supplier = context.Repository.GetElementByID(con.SupplierID);
                            if (supplier.Stereotype == UMM.bCollaborationUC.ToString())
                            {
                                bcuc = supplier;
                                break;
                            }
                        }

                    }
                }

                if (bcuc != null)
                {
                    foreach (EA.Connector con in bcuc.Connectors)
                    {
                        if (con.SupplierID == bcuc.ElementID && con.Stereotype == UMM.participates.ToString())
                        {
                            EA.Element client = context.Repository.GetElementByID(con.ClientID);
                            if (client.Stereotype == UMM.AuthorizedRole.ToString())
                            {
                                countBC++;
                            }
                        }

                    }


                }

            }


            if (countBC != countBR)
            {
                context.AddValidationMessage(new ValidationMessage("Violation of constraint C99.", "The number of AuthorizedRoles participating in a BusinessCollaborationUseCase MUST match the number of AuthorizedRoles participating in the BusinessRealization realizing this BusinessCollaborationUseCase. \n\n" + countBR + " AuthorizedRoles participate in the BusinessRealization and " + countBC + " AuthorizedRoles participate in the BusinessCollaborationUseCase.", "BRV", ValidationMessage.errorLevelTypes.ERROR, brv.PackageID));
            }
        }



        /// <summary>
        /// Returns true if the given Element e has a participates assocation to a
        /// BusinessCollaborationUseCase
        /// </summary>
        /// <param name="context"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool doesParticipateinBC(IValidationContext context, EA.Element e)
        {


            foreach (EA.Connector con in e.Connectors)
            {

                if (con.ClientID == e.ElementID && con.Stereotype == UMM.participates.ToString())
                {
                    EA.Element supplier = context.Repository.GetElementByID(con.SupplierID);
                    if (supplier.Stereotype == UMM.bCollaborationUC.ToString())
                        return true;

                }

            }



            return false;
        }



        /// <summary>
        /// Returns true if the given Element e has a participates assocation to
        /// a BusinessREalization
        /// </summary>
        /// <param name="context"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool doesParticipateInBR(IValidationContext context, EA.Element e)
        {

            foreach (EA.Connector con in e.Connectors)
            {

                if (con.ClientID == e.ElementID && con.Stereotype == UMM.participates.ToString())
                {
                    EA.Element supplier = context.Repository.GetElementByID(con.SupplierID);
                    if (supplier.Stereotype == UMM.bRealization.ToString())
                        return true;

                }

            }

            return false;
        }

        /// <summary>
        /// Check the tagged values of the BusinessRealizationView
        /// </summary>
        /// <param name="context"></param>
        /// <param name="p"></param>
        private void checkTV_BusinessRealizationView(IValidationContext context, EA.Package p)
        {
            //Check the TaggedValues of the BusinessRealizationView package
            new TaggedValueValidator().validatePackage(context, p);
        }

    }
}
