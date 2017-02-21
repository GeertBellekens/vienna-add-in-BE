/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Services.Description;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Schema;
using EA;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.ea;
using VIENNAAddIn.upcc3.uml;
using VIENNAAddInWpfUserControls;

namespace VIENNAAddIn.upcc3.Wizards.dev.ui
{
    /// <summary>
    /// Interaction logic for WSDLGenerator.xaml
    /// </summary>
    public partial class WSDLGenerator
    {
        private readonly EaUmlRepository eaUmlRepo;
        private readonly Repository repo;
        private IUmlPackage currentChoreographyView;
        private IUmlPackage currentCollaborationView;
        private string outputDirectory;
        private ServiceDescription serviceDescription;
        private bool containsInnerXML;

        public WSDLGenerator(Repository repository)
        {
            repo = repository;
            InitializeComponent();
            eaUmlRepo = new EaUmlRepository(repo);
            foreach (var umlPackage in eaUmlRepo.GetPackagesByStereotype("bChoreographyV"))
            {
                comboBoxBusinessChoreographyView.Items.Add(umlPackage.Name);
            }
        }

        public static void ShowForm(AddInContext context)
        {
            new WSDLGenerator(context.EARepository).Show();
        }

        /// <summary>
        /// Handles the selectionChanged event for the comboBoxBusinessChoreographyView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxBusinessChoreographyView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (IUmlPackage umlPackage in eaUmlRepo.GetPackagesByStereotype("bChoreographyV"))
            {
                if (umlPackage.Name.Equals(comboBoxBusinessChoreographyView.SelectedItem))
                {
                    currentChoreographyView = umlPackage;
                    foreach (var package in umlPackage.GetPackagesByStereotype("bCollaborationV"))
                    {
                        comboBoxBusinessCollaborationView.Items.Add(package.Name);
                    }
                }
            }
            comboBoxBusinessCollaborationView.IsEnabled = true;
            checkFormState();
        }

        /// <summary>
        /// Enable generation button only if all fields are filled out
        /// </summary>
        private void checkFormState()
        {
            if (comboBoxBusinessCollaborationView.SelectedItem != null)
            {
                if (!comboBoxBusinessCollaborationView.SelectedItem.Equals(string.Empty) &&
                    !comboBoxBusinessChoreographyView.SelectedItem.Equals(string.Empty) &&
                    textboxOutputDirectory.DirectoryName != null)
                {
                    buttonGenerate.IsEnabled = true;
                }
            }

            else
            {
                buttonGenerate.IsEnabled = false;
            }
        }

        /// <summary>
        /// Close the current Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the DirectoryNameChanged event for the textboxOutputDirectory.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textboxOutputDirectory_DirectoryNameChanged(object sender, RoutedEventArgs e)
        {
            outputDirectory = textboxOutputDirectory.DirectoryName;
            checkFormState();
        }

        /// <summary>
        /// Handles the selectionChanged event for the comboBoxBusinessCollaborationView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxBusinessCollaborationView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var umlPackage in currentChoreographyView.GetPackagesByStereotype("bCollaborationV"))
            {
                if (umlPackage.Name.Equals(comboBoxBusinessCollaborationView.SelectedItem))
                {
                    currentCollaborationView = umlPackage;
                }
            }
            checkFormState();

            targetXSDs.Items.Clear();
            foreach (var keyValuePair in retrieveMessageNames())
            {
                targetXSDs.Items.Add(new RetrievedMessage(keyValuePair.Key, "", ""));
            }
            if (targetXSDs.Items.IsEmpty)
            {
                MessageBox.Show("Your model either contains no messages or it does not comply to the UMM 2.0 Standard.\n"+
                    "Please note that each bTransaction and bCollaborationProtocol must contain one InitFlow containing 'Initial' in its name.\n"+
                    "(More information can be found at http://umm-dev.org) ","Warning! Unable to proceed.");
                buttonGenerate.IsEnabled = false;
            }
        }

        /// <summary>
        /// Reports the current status of the application to the graphical user interface.
        /// </summary>
        /// <param name="status">A string containing the status to be reported to the GUI.</param>
        public void reportStatus(string status)
        {
            textBoxStatus.Text = status + Environment.NewLine + textBoxStatus.Text;
        }


        /// <summary>
        /// Handles the click event for the buttonGenerate.
        /// Currently all application logic is implemented inside.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonGenerate_Click(object sender, RoutedEventArgs e)
        {
            containsInnerXML = false;
            foreach(var bcPartition in getbcPartitions()){
                        serviceDescription = new ServiceDescription();
                        var collaborationRoleName =
                            removeWhiteSpaces(repo.GetElementByID(repo.GetElementByID(bcPartition.Id).ClassifierID).Name);

                        //the collaborationRoleName + .wsdl is the actual Filename of the final WSDL
                        reportStatus("creating File: " + collaborationRoleName + ".wsdl");

                        var flow = (Connector) repo.GetElementByID(bcPartition.Id).Connectors.GetAt(0);

                        #region Initiator

                        if (flow.ClientID.Equals(bcPartition.Id))
                        {
                            var visitedTransactions = new List<int>();

                            //this is the initator because it contains the first outgoing initflow
                            foreach (
                                Connector connector in repo.GetElementByID(bcPartition.Id).Connectors)
                            {
                                if (connector.ClientID.Equals(bcPartition.Id)) continue;
                                if (visitedTransactions.Contains(connector.ClientID)) continue;
                                visitedTransactions.Add(connector.ClientID);
                                var targetTransaktion = repo.GetElementByID(connector.ClientID);
                                var realTransaktion =
                                    repo.GetElementByID(targetTransaktion.ClassfierID);
                                reportStatus("Discovered portType: " + realTransaktion.Name);
                                var portType = new PortType {Name = removeWhiteSpaces(realTransaktion.Name)};

                                //every transaction is a portType in the WSDL
                                foreach (var child in getElementsByStereoType(realTransaktion.Elements,"bTPartition"))
                                {
                                    if (!isInitiator(child)) continue;
                                    foreach (var action in getElementsByStereoType(child.Elements,"ReqAction"))
                                    {
                                        //this action is the operation name in the final WSDL
                                        reportStatus(
                                            "Discovered operation: ReceiveResponseFor_" +
                                            action.Name);
                                        var operation = new Operation
                                                            {
                                                                Name =
                                                                    "ReceiveResponseFor_" +
                                                                    removeWhiteSpaces(action.Name)
                                                            };
                                        var pins =
                                            new List<Element>(getElementsByStereoType(action.Elements, "ResInfPin"));
                                        //if there is only one incoming pin, nor further actions have to be taken, just create the according input message
                                        if (pins.Count.Equals(1))
                                        {
                                            var inputMessageValue =
                                                removeWhiteSpaces(repo.GetElementByID(pins[0].ClassfierID).Name);

                                            //this is the input message
                                            var inputMessage = assembleInputMessage(inputMessageValue);
                                            
                                            operation.Messages.Add(inputMessage);
                                            serviceDescription.Messages.Add(assembleMessageWithPart(inputMessageValue));
                                        }
                                            //otherwise we have to create a inner XML schema to enable a XSD Choice of the actual data transmitted
                                        else
                                        {
                                            serviceDescription.Types.Schemas.Add(generateInnerXmlSchema(pins,
                                                                                                        action.Name));
                                            containsInnerXML = true;
                                            var inputMessage =
                                                assembleInputMessage(removeWhiteSpaces(action.Name) + "Response");
                                            operation.Messages.Add(inputMessage);
                                            serviceDescription.Messages.Add(assembleMessageWithPartForInnerXml(action.Name));
                                        }
                                        portType.Operations.Add(operation);
                                        portType = addDefaultOperations(portType, collaborationRoleName);
                                    }
                                }
                                serviceDescription.PortTypes.Add(portType);
                            }
                        }
                            #endregion
                            #region  Responder

                        else
                        {
                            //according to the UMM spec there are always two and only two roles involved in a collaboration, therefor the next one has to be the responder
                            reportStatus("The role seems to be Responder.");

                            var visitedTransactions = new List<int>();
                            foreach (var connector in repo.GetElementByID(bcPartition.Id).Connectors)
                            {
                                if (!(connector is Connector)) continue;
                                var realConnector = (Connector) connector;
                                if (!realConnector.ClientID.Equals(bcPartition.Id))
                                {
                                    if (!visitedTransactions.Contains(realConnector.ClientID))
                                    {
                                        visitedTransactions.Add(realConnector.ClientID);
                                        var targetTransaktion = repo.GetElementByID(realConnector.ClientID);
                                        var realTransaktion =
                                            repo.GetElementByID(targetTransaktion.ClassfierID);
                                        reportStatus("portType: " + realTransaktion.Name);

                                        //every transaction is a portType in the WSDL
                                        var portType = new PortType
                                                           {Name = removeWhiteSpaces(realTransaktion.Name)};
                                        foreach (var child in getElementsByStereoType(realTransaktion.Elements,"bTPartition"))
                                        {
                                            if (isInitiator(child)) continue;

                                            foreach (var action in getElementsByStereoType(child.Elements,"ResAction"))
                                            {
                                                //this action is the operation name in the final WSDL
                                                reportStatus(
                                                    "operation name: " +
                                                    action.Name);
                                                var operation = new Operation
                                                                    {Name = removeWhiteSpaces(action.Name)};

                                                var pins =
                                                    new List<Element>(getElementsByStereoType(action.Elements,
                                                                                              "ResInfPin"));
                                                if (pins.Count.Equals(1))
                                                {
                                                    var outputMessageValue =
                                                        removeWhiteSpaces(repo.GetElementByID(pins[0].ClassfierID).Name);

                                                    //this is the output message
                                                    var outputMessage =
                                                        (OperationMessage) new OperationOutput();
                                                    outputMessage.Message =
                                                        new XmlQualifiedName(outputMessageValue,
                                                                             "http://" + collaborationRoleName);
                                                    operation.Messages.Add(outputMessage);
                                                    serviceDescription.Messages.Add(
                                                        assembleMessageWithPart(outputMessageValue));
                                                }
                                                else
                                                {
                                                    serviceDescription.Types.Schemas.Add(generateInnerXmlSchema(pins,
                                                                                                                action.
                                                                                                                    Name));
                                                    containsInnerXML = true;
                                                    var outputMessage =
                                                        (OperationMessage) new OperationOutput();
                                                    outputMessage.Message =
                                                        new XmlQualifiedName(
                                                            removeWhiteSpaces(action.Name) + "Response",
                                                            "http://" + collaborationRoleName);
                                                    operation.Messages.Add(outputMessage);
                                                    serviceDescription.Messages.Add(assembleMessageWithPartForInnerXml(action.Name));
                                                }
                                                pins =
                                                    new List<Element>(getElementsByStereoType(action.Elements,
                                                                                              "ReqInfPin"));
                                                if (pins.Count.Equals(1))
                                                {
                                                    var inputMessageValue =
                                                        removeWhiteSpaces(repo.GetElementByID(pins[0].ClassfierID).Name);

                                                    //this is the input message
                                                    var inputMessage =
                                                        assembleInputMessage(inputMessageValue);
                                                    operation.Messages.Add(inputMessage);
                                                    serviceDescription.Messages.Add(
                                                        assembleMessageWithPart(inputMessageValue));

                                                    portType.Operations.Add(operation);
                                                }
                                                else
                                                {
                                                    serviceDescription.Types.Schemas.Add(generateInnerXmlSchema(pins,
                                                                                                                action.
                                                                                                                    Name));
                                                    containsInnerXML = true;
                                                    var inputMessage =
                                                        assembleInputMessage(
                                                            removeWhiteSpaces(action.Name + "Response"));
                                                    operation.Messages.Add(inputMessage);
                                                    serviceDescription.Messages.Add(assembleMessageWithPartForInnerXml(action.Name));
                                                }
                                            }
                                        }
                                        //add default operations to portType
                                        portType = addDefaultOperations(portType, collaborationRoleName);
                                        serviceDescription.PortTypes.Add(portType);
                                    }
                                }
                            }
                        }

                        #endregion
                        finalizeAndWriteServiceDescription(collaborationRoleName);
                    }
        }

        /// <summary>
        /// Retrieve the bCPartitions from a Business CollaborationView.
        /// </summary>
        /// <returns>A enumerable List of bCPartitions in the CollaborationView.</returns>
        private IEnumerable<IUmlClass> getbcPartitions()
        {
            foreach (var bcollaborationUC in currentCollaborationView.GetClassesByStereotype("bCollaborationUC"))
            {
                foreach (
                    var bcollaborationprotocol in
                        bcollaborationUC.GetClassesByStereotype("bCollaborationProtocol"))
                {
                    foreach (var bcPartition in bcollaborationprotocol.GetClassesByStereotype("bCPartition"))
                    {
                        yield return bcPartition;
                    }
                }
            }
        }

        /// <summary>
        /// Retrieve the Names of the Messages exchanged in a Business Colaboration View.
        /// </summary>
        /// <returns>A dictionary containing the retrieved Messages and a blank placeholder for the target Namespace.</returns>
        private Dictionary<string,string> retrieveMessageNames()
        {
            var returnSet = new Dictionary<string,string>();
            foreach (var bcPartition in getbcPartitions())
            {
                var flow = (Connector) repo.GetElementByID(bcPartition.Id).Connectors.GetAt(0);

                if (flow.ClientID.Equals(bcPartition.Id))
                {
                    var visitedTransactions = new List<int>();
                    foreach (
                        Connector connector in repo.GetElementByID(bcPartition.Id).Connectors)
                    {
                        if (connector.ClientID.Equals(bcPartition.Id)) continue;
                        if (visitedTransactions.Contains(connector.ClientID)) continue;
                        visitedTransactions.Add(connector.ClientID);
                        var targetTransaktion = repo.GetElementByID(connector.ClientID);
                        var realTransaktion =
                            repo.GetElementByID(targetTransaktion.ClassfierID);

                        //every transaction is a portType in the WSDL
                        foreach (var child in getElementsByStereoType(realTransaktion.Elements, "bTPartition"))
                        {
                            if (!isInitiator(child)) continue;
                            foreach (var action in getElementsByStereoType(child.Elements, "ReqAction"))
                            {
                                var pins =
                                    new List<Element>(getElementsByStereoType(action.Elements, "ResInfPin"));
                                pins.AddRange(getElementsByStereoType(action.Elements,"ReqInfPin"));
                                foreach (var pin in pins)
                                {
                                    var message = repo.GetElementByID(pin.ClassfierID);
                                    returnSet.Add(removeWhiteSpaces(message.Name.Replace("Envelope", "")), "");
                                }
                            }
                        }
                    }
                }
            }
            return returnSet;
        }

        /// <summary>
        /// This method retrieves the elements with the provided stereotype from a given collection of elements.
        /// It has been implemented in order to circumvent the shortcomings of the EaUmlElement implementation.
        /// </summary>
        /// <param name="elements">The initial collection of input elements.</param>
        /// <param name="stereotype">The stereotype to retrieve elements from the initial collection.</param>
        /// <returns>A collection of elements with the defined stereotype</returns>
        private static IEnumerable<Element> getElementsByStereoType(Collection elements, string stereotype)
        {
            foreach (Element element in elements)
            {
                if (element.Stereotype.Equals(stereotype))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Removes whitespace characters from a given input string.
        /// </summary>
        /// <param name="input">The input String.</param>
        /// <returns>A string without whitespaces.</returns>
        private static string removeWhiteSpaces(string input)
        {
            return Regex.Replace(input, @"\s", "");
        }

        /// <summary>
        /// Assemble an input message for an operationMessage in the WSDL syntax
        /// </summary>
        /// <param name="name">The name of the resulting input message</param>
        /// <returns>An input message with the given name and namespace.</returns>
        private OperationMessage assembleInputMessage(string name)
        {
            //reportStatus("input messsage:" + inputMessageValue);
            var inputMessage = (OperationMessage) new OperationInput();
            foreach (RetrievedMessage message in targetXSDs.Items)
            {
                if(message.messageName.Equals(name))
                {
                    inputMessage.Message = new XmlQualifiedName(name, message.targetNamespace);
                }
            }
            return inputMessage;
        }

        /// <summary>
        /// Assemble a WSDL message containing a Part referring to the generated inner XML Schema.
        /// </summary>
        /// <param name="messageName">The name of the resulting message</param>
        /// <returns>A WSDL message containing a Part referring to the generated inner XML Schema.</returns>
        private static Message assembleMessageWithPartForInnerXml(string messageName)
        {
            string inputMessageValue = removeWhiteSpaces(messageName) + "Response";
            var concreteMessage = new Message {Name = inputMessageValue};
            var messagePart = new MessagePart
                                  {
                                      Name = "body",
                                      Element =
                                          new XmlQualifiedName(
                                          removeWhiteSpaces(messageName) +
                                          "Response","http://inline")
                                  };
            concreteMessage.Parts.Add(messagePart);

            return concreteMessage;
        }

        /// <summary>
        /// Assemble a WSDL message containing a Part referring to the imported XML Schema.
        /// </summary>
        /// <param name="messageName">The name of the resulting  message</param>
        /// <returns>A WSDL message containing a Part referring to the imported XML Schema.</returns>
        private Message assembleMessageWithPart(string messageName)
        {
            string inputMessageValue = removeWhiteSpaces(messageName);
            var concreteMessage = new Message { Name = inputMessageValue };
            var nameSpace = "";
            foreach (RetrievedMessage message in targetXSDs.Items)
            {
                if(message.messageName.Equals(inputMessageValue.Replace("Envelope","")))
                {
                    nameSpace = message.targetNamespace;
                }
            }
            var messagePart = new MessagePart
                                  {
                                      Name = "body",
                                      Element =
                                          new XmlQualifiedName(inputMessageValue,nameSpace)
            };
            concreteMessage.Parts.Add(messagePart);

            return concreteMessage;
        }

        /// <summary>
        /// Check if a given bCollaborationPartition is the initiator of a collaboration.
        /// </summary>
        /// <param name="element">The element to check if it is the initiator.</param>
        /// <returns>True if the element is the initiator, False otherwise.</returns>
        private static bool isInitiator(IDualElement element)
        {
            foreach (Element states in element.Elements)
            {
                if (states.Name.Contains("Initial"))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Generates an XML Schema inside the WSDL containing a XmlChoice with the defined pins.
        /// This method was implemented in order to comply to the WSDL 1.1 Spec where only single input/output messages are allowed.
        /// </summary>
        /// <param name="pins">The incoming or outgoing pins of an action Element.</param>
        /// <param name="actionName">The name of the action Element.</param>
        /// <returns>An XML Schema to be used inside the final WSDL containing a XmlChoice element.</returns>
        private XmlSchema generateInnerXmlSchema(IEnumerable<Element> pins, string actionName)
        {
            var schema = new XmlSchema {TargetNamespace = "http://inline"};
            var i = 1;

            foreach (RetrievedMessage message in targetXSDs.Items)
            {
                if (!message.filePath.Equals(string.Empty))
                {
                    schema.Includes.Add(new XmlSchemaImport { SchemaLocation = message.filePath, Namespace = message.targetNamespace });
                    if (message.prefix.Equals(string.Empty))
                    {
                        message.prefix = "xsd" + i;
                    }
                    schema.Namespaces.Add(message.prefix, message.targetNamespace);

                    i++;
                }
            }

            var xmlSchemaElement = new XmlSchemaElement
                                       {
                                           Name =
                                               removeWhiteSpaces(actionName) +
                                               "Response"
                                       };
            var xmlSchemaComplexType = new XmlSchemaComplexType();
            var xmlSchemaChoice = new XmlSchemaChoice();
            foreach (var pin in pins)
            {
                XmlSchemaElement xmlSchemaChoiceElement = null;
                foreach (RetrievedMessage message in targetXSDs.Items)
                {
                    if(message.messageName.Equals(removeWhiteSpaces(repo.GetElementByID(
                                                                                  pin.ClassfierID).Name).Replace("Envelope","")))
                    {
                        xmlSchemaChoiceElement = new XmlSchemaElement
                        {
                            RefName = new XmlQualifiedName(message.messageName, message.targetNamespace)
                        };
                    }
                }
                
                xmlSchemaChoice.Items.Add(xmlSchemaChoiceElement);
            }


            xmlSchemaComplexType.Particle = xmlSchemaChoice;
            xmlSchemaElement.SchemaType = xmlSchemaComplexType;
            schema.Items.Add(xmlSchemaElement);
            return schema;
        }

        /// <summary>
        /// Adds default messages and namespaces as well as the default targetNamespace to a WSDL ServiceDescription and writes it to target location.
        /// </summary>
        /// <param name="collaborationRoleName">The collaborationRolename retrieved from the UMM model.</param>
        private void finalizeAndWriteServiceDescription(string collaborationRoleName)
        { 
            //Imports
            var defaultMessagesImport = new Import {Namespace = "http://bas", Location = "bas.xsd"};
            serviceDescription.Imports.Add(defaultMessagesImport);

            if (!containsInnerXML)
            {
                foreach (RetrievedMessage message in targetXSDs.Items)
                {
                    if (!message.filePath.Equals(string.Empty))
                    {
                        serviceDescription.Imports.Add(new Import
                                                           {
                                                               Namespace = message.targetNamespace,
                                                               Location = message.filePath
                                                           });
                    }
                }
            }

            //Messages
            var ackReceiptMessagePart = new MessagePart
                                  {
                                      Name = "parameters",
                                      Element = new XmlQualifiedName("Acknowledgement","http://bas")
                                  };
            var ackReceiptMessage = new Message {Name = "Acknowledgement"};
            ackReceiptMessage.Parts.Add(ackReceiptMessagePart);
            serviceDescription.Messages.Add(ackReceiptMessage);


            var controlFailureMessagePart = new MessagePart
            {
                Name = "parameters",
                Element = new XmlQualifiedName("ControlFailure", "http://bas")
            };
            var controlFailureMessage = new Message { Name = "BusinessSignalControlFailure" };
            controlFailureMessage.Parts.Add(controlFailureMessagePart);
            serviceDescription.Messages.Add(controlFailureMessage);

            //TargetNamespace
            serviceDescription.TargetNamespace = "http://" + collaborationRoleName;

            //Namespaces
            serviceDescription.Namespaces.Add("wsdl", "http://schemas.xmlsoap.org/wsdl/");
            serviceDescription.Namespaces.Add("xsd", "http://www.w3.org/2001/XMLSchema");
            serviceDescription.Namespaces.Add("soap", "http://schemas.xmlsoap.org/wsdl/soap/");
            serviceDescription.Namespaces.Add("bas","http://bas");
            serviceDescription.Namespaces.Add("tns", "http://" + collaborationRoleName);

            if(!containsInnerXML)
            {
                var i = 1;
                foreach (RetrievedMessage message in targetXSDs.Items)
                {
                    if (!message.filePath.Equals(string.Empty))
                    {
                        if(message.prefix.Equals(string.Empty))
                        {
                            message.prefix = "xsd" + i;
                        }
                        serviceDescription.Namespaces.Add(message.prefix, message.targetNamespace);
                        i++;
                    }
                }
            }
            else
            {
                serviceDescription.Namespaces.Add("il","http://inline");
            }

            serviceDescription.Write(outputDirectory + "\\" + collaborationRoleName + ".wsdl");
        }

        /// <summary>
        /// Adds the default Ack, Processing and ControlFailure operations to a given portType and returns it.
        /// </summary>
        /// <param name="inputPortType">The portType to which the default operations should be added.</param>
        /// <param name="nameSpace">The targetNamespace without the http:// prefix.</param>
        /// <returns>A portType with the default Ack, Processing and ControlFailure operations added.</returns>
        private static PortType addDefaultOperations(PortType inputPortType, string nameSpace)
        {
            //Create AckReceipt operation
            var ackReceiptOperation = new Operation {Name = "receiveAck"};
            var ackReceiptInputMessage = (OperationMessage) new OperationInput();
            ackReceiptInputMessage.Message = new XmlQualifiedName("Acknowledgement", "http://" + nameSpace);
            ackReceiptOperation.Messages.Add(ackReceiptInputMessage);

            inputPortType.Operations.Add(ackReceiptOperation);

            //Create ControlFailure operation

            var controlFailureOperation = new Operation {Name = "ControlFailure"};
            var controlFailureInputMessage = (OperationMessage) new OperationInput();
            controlFailureInputMessage.Message = new XmlQualifiedName("BusinessSignalControlFailure",
                                                                      "http://" + nameSpace);
            controlFailureOperation.Messages.Add(controlFailureInputMessage);

            inputPortType.Operations.Add(controlFailureOperation);

            return inputPortType;
        }

        /// <summary>
        /// This method is called whenever a Directory Name in the Fileselecter has changed.
        /// It also adds the filepath and the target Namespace of the chosen XML File to the corresponding message.
        /// </summary>
        /// <param name="sender">The initiator of the method call.</param>
        /// <param name="e">Arguments which may be processed in other usage scenarios.</param>
        private void FileSelector_DirectoryNameChanged(object sender, RoutedEventArgs e)
        {
            var senderCast = (FileSelector) sender;
            var stackpanel = (StackPanel) senderCast.Parent;
            var textblock = (TextBlock) stackpanel.Children[0];

            foreach (RetrievedMessage item in targetXSDs.Items)
            {
                if(item.messageName.Equals(textblock.Text))
                {
                    item.filePath = senderCast.FileName;
                    item.targetNamespace = retrieveTargetNamespace(senderCast.FileName);
                }
            }

            targetXSDs.Items.Refresh();
        }

        /// <summary>
        /// Help method to retrieve the Target Namespace from a XML Schema file.
        /// </summary>
        /// <param name="fileName">The file path of the target XML Schema file.</param>
        /// <returns>The value of the target Namespace of the XML Schema File in String format.</returns>
        private static string retrieveTargetNamespace(string fileName)
        {
            var schema = XmlSchema.Read(XmlReader.Create(fileName),null);
            return schema.TargetNamespace;
        }

        /// <summary>
        /// This is a inner Class which helps capsulating information retrieved from the graphical user interface.
        /// </summary>
        public class RetrievedMessage
        {
            public string messageName
            {
                get; set;
            }
            public string targetNamespace
            {
                get; set;
            }
            public string filePath
            {
                get; set;
            }
            public string prefix
            {
                get; set;
            }
            public RetrievedMessage(string _messageName, string _targetNamespace, string _filePath)
            {
                messageName = _messageName;
                targetNamespace = _targetNamespace;
                filePath = _filePath;
                prefix = "";
            }
        }
    }
}