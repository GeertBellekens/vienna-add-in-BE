/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VIENNAAddIn.constants
{ 

    internal enum UMMTaggedValues
    {

        URI,
        actions,
        beginsWhen,
        businessOpportunity,
        businessTerm,
        businessTransactionType,
        copyright,
        definition,
        endsWhen,
        exceptions,
        interest,
        isAuthenticated,
        isAuthorizationRequired,
        isConcurrent,
        isConfidential,
        isIntelligibleCheckRequired,
        isNonRepudiationReceiptRequired,
        isNonRepudiationRequired,
        isSecureTransportRequired,
        isTamperProof,
        justification,
        objective,
        owner,
        postCondition,
        preCondition,
        reference,
        retryCount,
        scope,
        status,
        timeToAcknowledgeProcessing,
        timeToAcknowledgeReceipt,
        timeToPerform,
        timeToRespond,
        version,
    
    }


    /// <sUMM2ary>
    /// Represents the Types of Elements occuring in an Enterprise
    /// Architect model
    /// </sUMM2ary>
    internal enum EA_Element
    {
        Action,
        Activity,
        ActivityPartition,
        ActivityRegion,
        Actor,
        Aggregation,
        Artifact,
        Association,
        Boundary,
        Change,
        Class,
        Collaboration,
        Component,
        Constraint,
        Decision,
        DeploymentSpecification,
        DiagramFrame,
        EmbeddedElement,
        Entity,
        EntryPoint,
        Event,
        ExceptionHandler,
        ExitPoint,
        ExpansionNode,
        ExpansionRegion,
        GUIElement,
        InteractionFragment,
        InteractionOccurrence,
        InteractionState,
        Interface,
        InterruptibleActivityRegion,
        Issue,
        Node,
        Note,
        Object,
        Package,
        Parameter,
        Part,
        Port,
        ProvidedInterface,
        Report,
        RequiredInterface,
        Requirement,
        Screen,
        Sequence,
        State,
        StateNode,
        Synchronization,
        Text,
        TimeLine,
        UMLDiagram,
        UseCase


    }



    internal enum UPCC
    {
        ACC,
        ASCC,
        BCC,
        ABIE,
        BBIE,
        ASBIE,
        bLibrary,
        BDT,
        CDT,
        CON,
        SUP,
        ENUM,
        PRIM,
        BDTLibrary,
        BIELibrary,
        CCLibrary,
        CDTLibrary,
        DOCLibrary,
        ENUMLibrary,
        PRIMLibrary,
        isEquivalentTo,
        basedOn
        
    }

    internal enum UPCC_TV
    {
        dictionaryEntryName,
        uniqueIdentifier,
        agencyName,
        agencyIdentifier,
        definition
    }



    internal enum UPCC_Packages
    {
        bLibrary,
        BDTLibrary,
        BIELibrary,
        CCLibrary,
        CDTLibrary,
        DOCLibrary,
        ENUMLibrary,
        PRIMLibrary

    }

    public enum UMM
    {

        AuthorizedRole,
        BusinessAction,
        InfEnvelope,
        InfPin,
        ProcessArea,
        ReqAction,
        ReqInfPin,
        ResAction,
        ResInfPin,
        Stakeholder,
        bArea,
        bCPartition,
        bCategory,
        bChoreographyV,
        bCollModel,
        bCollaborationAction,
        bCollaborationProtocol,
        bCollaborationUC,
        bCollaborationV,
        bDataV,
        bDomainV,
        bEInternalState,
        bESharedState,
        bEntity,
        bEState,
        bEntityV,
        bInformation,
        bInformationV,
        bNestedCollaboration,
        bTPartition,
        bPartner,
        bPartnerV,
        bProcess,
        bProcessAction,
        bProcessUC,
        bRealization,
        bRealizationUC,
        bRealizationV,
        bRequirementsV,
        bTransaction,
        bTransactionAction,
        bTransactionUC,
        bTransactionV,
        participates,
        include,
        initFlow,
        isOfInterestTo,
        mapsTo,
        reFlow


    }


    internal enum AssociationTypes
    {
        Association,
        ControlFlow,
        Dependency,
        Aggregation,
        Extends,
        Realisation,
        include,
        Includes,
        NoteLink,
        ObjectFlow,
        StateFlow,
        UseCase
    }

    internal enum BusinessTransactionPattern
    {
        CommercialTransaction,
        RequestConfirm,
        QueryResponse,
        RequestResponse,
        Notification,
        InformationDistribution
    }
    internal enum CBPC_BusinessAreas
    {
        ProcurementSales,
        Design,
        Manufacture,
        Logistics,
        RecruitmentTraining,
        FinancialServices,
        Regulation,
        HealthCare
    }
    internal enum CBPC_ProcessAreas
    {
        Planning,
        Identification,
        Negotiation,
        Actualization,
        PostActualization
    }

    public enum CCTS_Types
    {

        ABIE,
        ACC,
        ASBIE,
        ASCC,
        BBIE,
        BCC,
        BCSS,
        BIE,
        BIELibrary,
        bLibrary,
        BusinessLibrary,
        CC,
        CCLibrary,
        CCTS,
        CDT,
        CDTLibrary,
        CON,
        DOCLibrary,
        ENUM,
        ENUMLibrary,
        PRIM,
        PRIMLibrary,
        QDT,
        QDTLibrary,                
        BDTLibrary,
        SUP,
        basedOn,
        BDT
    }

    internal enum CCTS_PackageType
    {
        BusinessInformationView,
        BusinessSamplesView,
        BusinessDataView,
        BusinessDataTypeView,
        CoreDataView,
        CoreDataTypeView,
        BusinessCodeListView,
        BusinessDocumentView,
        BusinessLibrary,
        BIELibrary,
        CCLibrary,
        CDTLibrary,
        DOCLibrary,
        ENUMLibrary,
        PRIMLibrary,
        QDTLibrary
    }

    /// <sUMM2ary>
    /// Tagged values of CC and BIE
    /// </sUMM2ary>
    public enum CCTS_TV
    {
        Acronym,
        AssociatedObjectClassQualifierTerm,
        AssociatedObjectClassTerm,
        AssociationType,
        baseURN,
        baseURL,
        BusinessProcessContextValue,
        BusinessProcessRoleContextValue,
        BusinessTerm,
        Cardinality,
        DataTypeQualifierTerm,
        Definition,
        DictionaryEntryName,
        Documentation,
        Example,
        GeopoliticalOrRegionContextValue,
        IndustryContextValue,
        Name,
        OASISMap,
        ObjectClassQualifierTerm,
        ObjectClassTerm,
        OfficialConstraintContextValue,
        PrimaryRepresentationTerm,
        PrimitiveType,
        ProductContextValue,
        PropertyQualifierTerm,
        PropertyTerm,
        RepresentationLayout,
        Status,
        SupportingRoleContextValue,
        SystemCapabilitiesContextValue,
        UniqueID,
        UniqueIdentifier,
        UsageRule,
        Version
    }



    // all stereotypes which owns tagged values
    public enum StereotypeOwnTaggedValues
    {
        BusinessDomainView,
        BusinessCategory,
        ProcessArea,
        BusinessArea,
        BusinessProcess,
        Stakeholder,
        BusinessPartner,
        BusinessRequirementsView,
        BusinessProcessView,
        BusinessEntityView,
        CollaborationRequirementsView,
        TransactionRequirementsView,
        CollaborationRealizationView,
        BusinessCollaborationUseCase,
        BusinessTransactionUseCase,
        BusinessTransactionView,
        BusinessChoreographyView,
        BusinessInteraction,
        BusinessInteractionView,
        BusinessInformationView,
        BusinessTransactionActivity,
        RequestingBusinessActivity,
        RespondingBusinessActivity,
        RequestingInformationEnvelope,
        RespondingInformationEnvelope,
        BusinessTransaction,
        InformationEntity,
        InformationEnvelope
    }
    internal enum BusinessActionTV
    {
        isAuthorizationRequired,
        isNonRepudiationRequired,
        isNonRepudiationReceiptRequired,
        timeToAcknowledgeReceipt,
        timeToAcknowledgeProcessing,
        isIntelligibleCheckRequired,
    }

    internal enum BusinessLibraryTV
    {
        businessTerm,
        copyright,
        owner,
        reference,
        status,
        URI,
        version
    }

    internal enum BusinessCategoryTV
    {
        objective,
        scope,
        businessOpportunity
    }

    internal enum StakeholderTV
    {
        interest
    }

    internal enum BusinessProcessTV
    {
        actions
    }

    internal enum BusinessTransactionActivityTV
    {
        timeToPerform,
        isConcurrent
    }

    internal enum BusinessInteractionTV
    {
        BusinessTransactionPattern,
        isSecureTransportRequired
    }

    internal enum RequestingBusinessActivityTV
    {
        timeToAcknowledgeReceipt,
        timeToAcknowledgeProcessing,
        timeToRespond,
        isAuthorizationRequired,
        isNonRepudiationRequired,
        isNonRepudiationReceiptRequired,
        retryCount,
        isIntelligibleCheckRequired,
    }

    internal enum RespondingBusinessActivityTV
    {
        timeToAcknowledgeReceipt,
        timeToAcknowledgeProcessing,
        isAuthorizationRequired,
        isNonRepudiationRequired,
        isIntelligibleCheckRequired,
        isNonRepudiationReceiptRequired

    }

    internal enum InformationEntityTV
    {
        isConfidential,
        isTamperProof,
        isAuthenticated
    }

    

   

   




}
