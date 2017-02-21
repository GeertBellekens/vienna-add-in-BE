using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn;
using VIENNAAddIn.upcc3;
using VIENNAAddIn.upcc3.otf;
using VIENNAAddInUnitTests.TestRepository;
using VIENNAAddInUtils;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    [TestFixture]
    public class CCRepositoryTests
    {
        #region Setup/Teardown

        [SetUp]
        public void Context()
        {
            var eaRepository = new EARepository();
            eaRepository.AddModel("Model", model =>
                                           {
                                               model.AddPackage("Package 1", package_1 =>
                                                                             {
                                                                                 package_1.Element.Stereotype = Stereotype.bLibrary;
                                                                                 package_1.AddPackage("Package 1.1", package_1_1 =>
                                                                                                                     {
                                                                                                                         package_1_1.Element.Stereotype = Stereotype.bLibrary;
                                                                                                                         package_1_1.AddPackage("Package 1.1.1", p => { p.Element.Stereotype = Stereotype.PRIMLibrary; });
                                                                                                                         package_1_1.AddPackage("Package 1.1.2", p => { p.Element.Stereotype = Stereotype.ENUMLibrary; });
                                                                                                                         package_1_1.AddPackage("Package 1.1.3", p => { p.Element.Stereotype = Stereotype.CDTLibrary; });
                                                                                                                         package_1_1.AddPackage("Package 1.1.4", p => { p.Element.Stereotype = Stereotype.CCLibrary; });
                                                                                                                         package_1_1.AddPackage("Package 1.1.5", p => { p.Element.Stereotype = Stereotype.BDTLibrary; });
                                                                                                                         package_1_1.AddPackage("Package 1.1.6", p => { p.Element.Stereotype = Stereotype.BIELibrary; });
                                                                                                                         package_1_1.AddPackage("Package 1.1.7", p => { p.Element.Stereotype = Stereotype.DOCLibrary; });
                                                                                                                     });
                                                                                 package_1.AddPackage("Package 1.2", package_1_2 => { package_1_2.Element.Stereotype = "Foo bar 1"; });
                                                                             });
                                               model.AddPackage("Package 2", package_2 => { package_2.Element.Stereotype = "Foo bar 2"; });
                                               model.AddPackage("Package 3", package_3 =>
                                                                             {
                                                                                 package_3.Element.Stereotype = Stereotype.bInformationV;
                                                                                 package_3.AddPackage("Package 3.1", package_3_1 =>
                                                                                                                     {
                                                                                                                         package_3_1.Element.Stereotype = Stereotype.bLibrary;
                                                                                                                         package_3_1.AddPackage("Package 3.1.1", package_3_1_1 => { package_3_1_1.Element.Stereotype = Stereotype.bLibrary; });
                                                                                                                         package_3_1.AddPackage("Package 3.1.2", package_3_1_2 => { package_3_1_2.Element.Stereotype = "Foo bar 3"; });
                                                                                                                     });
                                                                             });
                                           });
            validatingCcRepository = new ValidatingCCRepository(eaRepository);
        }

        #endregion

        private ValidatingCCRepository validatingCcRepository;

        private void WaitUntilRepositoryIsReady()
        {
        }

        [Test]
        public void When_it_is_created_Then_it_should_load_all_EA_repository_contents_and_build_an_internal_model()
        {
            validatingCcRepository.LoadRepositoryContent();
            WaitUntilRepositoryIsReady();
            Assert.That(validatingCcRepository.GetBLibraries().Convert(l => l.Name), Is.EquivalentTo(new[]{"Package 1", "Package 1.1", "Package 3.1", "Package 3.1.1"}));
            Assert.That(validatingCcRepository.GetPrimLibraries().Convert(l => l.Name), Is.EquivalentTo(new[]{"Package 1.1.1"}));
            Assert.That(validatingCcRepository.GetEnumLibraries().Convert(l => l.Name), Is.EquivalentTo(new[]{"Package 1.1.2"}));
            Assert.That(validatingCcRepository.GetCdtLibraries().Convert(l => l.Name), Is.EquivalentTo(new[]{"Package 1.1.3"}));
            Assert.That(validatingCcRepository.GetCcLibraries().Convert(l => l.Name), Is.EquivalentTo(new[]{"Package 1.1.4"}));
            Assert.That(validatingCcRepository.GetBdtLibraries().Convert(l => l.Name), Is.EquivalentTo(new[]{"Package 1.1.5"}));
            Assert.That(validatingCcRepository.GetBieLibraries().Convert(l => l.Name), Is.EquivalentTo(new[]{"Package 1.1.6"}));
            Assert.That(validatingCcRepository.GetDocLibraries().Convert(l => l.Name), Is.EquivalentTo(new[]{"Package 1.1.7"}));
        }
    }
}