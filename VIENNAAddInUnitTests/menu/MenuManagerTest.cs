using System.Collections.Generic;
using EA;
using Moq;
using NUnit.Framework;
using VIENNAAddIn.menu;
using Stereotype=VIENNAAddIn.upcc3.Stereotype;

namespace VIENNAAddInUnitTests.menu
{
    [TestFixture]
    public class MenuManagerTest
    {
        private static AddInContext CreatePackageContext(string stereotype)
        {
            Mock<Package> packageMock = CreatePackageMock(stereotype);
            var repositoryMock = new Mock<Repository>();
            repositoryMock.Setup(r => r.GetTreeSelectedObject()).Returns(packageMock.Object);
            object outContextObject = packageMock.Object;
            repositoryMock.Setup(r => r.GetContextItem(out outContextObject)).Returns(ObjectType.otPackage);
            return new AddInContext(repositoryMock.Object, MenuLocation.TreeView.ToString());
        }

        private static AddInContext CreateElementContext()
        {
            var elementMock = new Mock<Element>();
            var repositoryMock = new Mock<Repository>();
            repositoryMock.Setup(r => r.GetContextItemType()).Returns(ObjectType.otElement);
            repositoryMock.Setup(r => r.GetContextObject()).Returns(elementMock.Object);
            return new AddInContext(repositoryMock.Object, MenuLocation.TreeView.ToString());
        }

        private static Mock<Package> CreatePackageMock(string stereotype)
        {
            var packageElementMock = new Mock<Element>();
            packageElementMock.Setup(e => e.Stereotype).Returns(stereotype);
            var packageMock = new Mock<Package>();
            packageMock.Setup(p => p.Element).Returns(packageElementMock.Object);
            return packageMock;
        }

        private static AddInContext CreateDiagramContext()
        {
            var diagramMock = new Mock<Diagram>();
            var repositoryMock = new Mock<Repository>();
            repositoryMock.Setup(r => r.GetContextItemType()).Returns(ObjectType.otDiagram);
            repositoryMock.Setup(r => r.GetContextObject()).Returns(diagramMock.Object);
            return new AddInContext(repositoryMock.Object, MenuLocation.TreeView.ToString());
        }

        private static void DoNothing(AddInContext context)
        {
        }

        private static void AssertMenuState(TestPredicate testPredicate, MenuManager menuManager, string menuName, string menuItem)
        {
            var context = CreateMainMenuContext();
            bool isEnabled = false;
            bool isChecked = false;

            testPredicate.Enable();
            testPredicate.Check();
            menuManager.GetMenuState(context, menuName, menuItem, ref isEnabled, ref isChecked);
            Assert.IsTrue(isEnabled);
            Assert.IsTrue(isChecked);

            testPredicate.Disable();
            testPredicate.Uncheck();
            menuManager.GetMenuState(context, menuName, menuItem, ref isEnabled, ref isChecked);
            Assert.IsFalse(isEnabled);
            Assert.IsFalse(isChecked);
        }

        private static AddInContext CreateMainMenuContext()
        {
            var eaRepositoryMock = new Mock<Repository>();
            return new AddInContext(eaRepositoryMock.Object, MenuLocation.MainMenu.ToString());
        }

        private static bool ContextIsBDTLibrary(AddInContext context)
        {
            return context.MenuLocation.IsContextMenu() && context.SelectedItemIsLibraryOfType(Stereotype.BDTLibrary);
        }

        [Test]
        public void TestContextMenuClick()
        {
            var action = new AssertableMenuAction();
            var menuManager = new MenuManager();
            menuManager.AddMenu(MenuLocation.TreeView +
                                ("VIENNAAddIn"
                                 + action.Named("Validate BDT Library")
                                 + action.Named("Create new BDT")))
                .ShowIf(ContextIsBDTLibrary);
            var context = CreatePackageContext(Stereotype.BDTLibrary);
            menuManager.GetMenuItems(context, null);
            menuManager.MenuClick(context, "-VIENNAAddIn", "Validate BDT Library");
            menuManager.MenuClick(context, "-VIENNAAddIn", "Create new BDT");
            Assert.AreEqual(new[] {"Validate BDT Library", "Create new BDT"}, action.ExecutedActions);
        }

        [Test]
        public void TestGetContextMenuItems()
        {
            var menuManager = new MenuManager
                              {
                                  DefaultMenuItems = new[] {"default"}
                              };
            menuManager.AddMenu(MenuLocation.TreeView +
                                ("VIENNAAddIn"
                                 + "Validate BDT Library".OnClick(DoNothing)
                                 + "Create new BDT".OnClick(DoNothing)))
                .ShowIf(context => context.SelectedItemIsLibraryOfType(Stereotype.BDTLibrary));
            Assert.AreEqual(new[] {"default"}, menuManager.GetMenuItems(CreateMainMenuContext(), null));
            Assert.AreEqual(new[] {"-VIENNAAddIn"}, menuManager.GetMenuItems(CreatePackageContext(Stereotype.BDTLibrary), null));
            Assert.AreEqual(new[] {"Validate BDT Library", "Create new BDT"}, menuManager.GetMenuItems(CreatePackageContext(Stereotype.BDTLibrary), "-VIENNAAddIn"));
            Assert.AreEqual(new[] {"default"}, menuManager.GetMenuItems(CreateElementContext(), null));
            Assert.AreEqual(new[] {"default"}, menuManager.GetMenuItems(CreateDiagramContext(), null));
        }

        [Test]
        public void TestGetMainMenuItems()
        {
            var menuManager = new MenuManager
                              {
                                  DefaultMenuItems = new[] {"default"}
                              };
            menuManager.AddMenu(MenuLocation.MainMenu
                                + ("menu"
                                   + "action1".OnClick(DoNothing)
                                   + "action2".OnClick(DoNothing)
                                   + MenuItem.Separator
                                   + ("sub-menu"
                                      + "sub-menu-action1".OnClick(DoNothing)
                                      + "sub-menu-action2".OnClick(DoNothing)
                                     )
                                   + "action3".OnClick(DoNothing)));
            var context = CreateMainMenuContext();
            Assert.AreEqual(new[] {"default"}, menuManager.GetMenuItems(context, "unknown menu name"));
            Assert.AreEqual(new[] {"-menu"}, menuManager.GetMenuItems(context, null));
            Assert.AreEqual(new[] {"-menu"}, menuManager.GetMenuItems(context, string.Empty));
            Assert.AreEqual(new[] {"action1", "action2", "-", "-sub-menu", "action3"}, menuManager.GetMenuItems(context, "-menu"));
            Assert.AreEqual(new[] {"sub-menu-action1", "sub-menu-action2"}, menuManager.GetMenuItems(context, "-sub-menu"));
        }

        [Test]
        public void TestGetMainMenuState()
        {
            var testPredicate = new TestPredicate();
            var menuManager = new MenuManager();
            menuManager.AddMenu(MenuLocation.MainMenu
                                + ("menu"
                                   + "action1".OnClick(DoNothing).Checked(testPredicate.IsChecked).Enabled(testPredicate.IsEnabled)
                                   + "action2".OnClick(DoNothing).Checked(testPredicate.IsChecked).Enabled(testPredicate.IsEnabled)
                                   + MenuItem.Separator
                                   + ("sub-menu"
                                      + "sub-menu-action1".OnClick(DoNothing).Checked(testPredicate.IsChecked).Enabled(testPredicate.IsEnabled)
                                      + "sub-menu-action2".OnClick(DoNothing).Checked(testPredicate.IsChecked).Enabled(testPredicate.IsEnabled)
                                     )
                                   + "action3".OnClick(DoNothing).Checked(testPredicate.IsChecked).Enabled(testPredicate.IsEnabled)));
            menuManager.GetMenuItems(CreateMainMenuContext(), null);
            AssertMenuState(testPredicate, menuManager, "-menu", "action1");
            AssertMenuState(testPredicate, menuManager, "-menu", "action2");
            AssertMenuState(testPredicate, menuManager, "-menu", "action3");
            AssertMenuState(testPredicate, menuManager, "-sub-menu", "sub-menu-action1");
            AssertMenuState(testPredicate, menuManager, "-sub-menu", "sub-menu-action2");
        }

        [Test]
        public void TestMainMenuClick()
        {
            var assertableMenuAction = new AssertableMenuAction();
            var menuManager = new MenuManager();
            menuManager.AddMenu(MenuLocation.MainMenu
                                + ("menu"
                                   + assertableMenuAction.Named("action1")
                                   + assertableMenuAction.Named("action2")
                                   + MenuItem.Separator
                                   + ("sub-menu"
                                      + assertableMenuAction.Named("sub-menu-action1")
                                      + assertableMenuAction.Named("sub-menu-action2")
                                     )
                                   + assertableMenuAction.Named("action3")));
            AddInContext context = CreateMainMenuContext();
            menuManager.GetMenuItems(context, null);
            menuManager.MenuClick(context, "-menu", "action1");
            menuManager.MenuClick(context, "-menu", "action2");
            menuManager.MenuClick(context, "-sub-menu", "sub-menu-action1");
            menuManager.MenuClick(context, "-sub-menu", "sub-menu-action2");
            menuManager.MenuClick(context, "-menu", "action3");
            Assert.AreEqual(new[] {"action1", "action2", "sub-menu-action1", "sub-menu-action2", "action3"}, assertableMenuAction.ExecutedActions);
        }

        [Test]
        public void TestNestedSubMenu()
        {
            SubMenu nestedMenu = ("menu"
                                  + "action1".OnClick(DoNothing)
                                  + "action2".OnClick(DoNothing)
                                  + MenuItem.Separator
                                  + ("sub-menu"
                                     + "sub-menu-action1".OnClick(DoNothing)
                                     + "sub-menu-action2".OnClick(DoNothing)
                                    )
                                  + "action3".OnClick(DoNothing)
                                 );
            Assert.AreEqual("-menu", nestedMenu.Name);
            Assert.AreEqual(5, nestedMenu.Items.Count);
            Assert.AreEqual("action1", nestedMenu.Items[0].Name);
            Assert.AreEqual("action2", nestedMenu.Items[1].Name);
            Assert.AreEqual("-", nestedMenu.Items[2].Name);
            Assert.AreEqual("action3", nestedMenu.Items[4].Name);
            Assert.IsTrue(nestedMenu.Items[3] is SubMenu);
            var subMenu = (SubMenu) nestedMenu.Items[3];
            Assert.AreEqual("-sub-menu", subMenu.Name);
            Assert.AreEqual(2, subMenu.Items.Count);
            Assert.AreEqual("sub-menu-action1", subMenu.Items[0].Name);
            Assert.AreEqual("sub-menu-action2", subMenu.Items[1].Name);
        }

        [Test]
        public void TestSimpleSubMenu()
        {
            SubMenu simpleSubMenu = "subMenu"
                                    + "action1".OnClick(DoNothing)
                                    + "action2".OnClick(DoNothing)
                                    + MenuItem.Separator
                                    + "action3".OnClick(DoNothing);
            Assert.AreEqual("-subMenu", simpleSubMenu.Name);
            Assert.AreEqual(4, simpleSubMenu.Items.Count);
            Assert.AreEqual("action1", simpleSubMenu.Items[0].Name);
            Assert.AreEqual("action2", simpleSubMenu.Items[1].Name);
            Assert.AreEqual("-", simpleSubMenu.Items[2].Name);
            Assert.AreEqual("action3", simpleSubMenu.Items[3].Name);
        }

        [Test]
        public void TestSingleActionMenu()
        {
            MenuAction singleActionMenu = "action".OnClick(DoNothing);
            Assert.AreEqual("action", singleActionMenu.Name);
        }
    }

    public class TestPredicate
    {
        private bool isChecked;
        private bool isEnabled;

        public bool IsEnabled(AddInContext obj)
        {
            return isEnabled;
        }

        public void Enable()
        {
            isEnabled = true;
        }

        public void Disable()
        {
            isEnabled = false;
        }

        public bool IsChecked(AddInContext obj)
        {
            return isChecked;
        }

        public void Check()
        {
            isChecked = true;
        }

        public void Uncheck()
        {
            isChecked = false;
        }
    }

    public class AssertableMenuAction
    {
        private readonly List<string> executedActions = new List<string>();

        public string[] ExecutedActions
        {
            get { return executedActions.ToArray(); }
        }

        public MenuAction Named(string name)
        {
            return name.OnClick(context => executedActions.Add(name));
        }
    }
}