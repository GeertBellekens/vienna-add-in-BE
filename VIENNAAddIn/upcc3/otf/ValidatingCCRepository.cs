using System;
using System.Collections.Generic;
using System.Linq;
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.BLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using CctsRepository.DocLibrary;
using CctsRepository.EnumLibrary;
using CctsRepository.PrimLibrary;
using EA;
using VIENNAAddIn.upcc3.otf.validators;
using VIENNAAddInUtils;

namespace VIENNAAddIn.upcc3.otf
{
    public class ValidatingCCRepository : ICctsRepository
    {
        private readonly RepositoryContentLoader contentLoader;
        private readonly HierarchicalRepository repository;
        private readonly ValidationService validationService;

        public ValidatingCCRepository(Repository eaRepository)
        {
            validationService = new ValidationService();
            validationService.AddValidator(new BLibraryValidator());
            validationService.AddValidator(new ElementLibaryValidator(Stereotype.PRIMLibrary, Stereotype.PRIM));
            validationService.AddValidator(new ElementLibaryValidator(Stereotype.ENUMLibrary, Stereotype.ENUM));
            validationService.AddValidator(new ElementLibaryValidator(Stereotype.CDTLibrary, Stereotype.CDT));
            validationService.AddValidator(new ElementLibaryValidator(Stereotype.CCLibrary, Stereotype.ACC));
            validationService.AddValidator(new ElementLibaryValidator(Stereotype.BDTLibrary, Stereotype.BDT));
            validationService.AddValidator(new ElementLibaryValidator(Stereotype.BIELibrary, Stereotype.ABIE));
            validationService.AddValidator(new ElementLibaryValidator(Stereotype.DOCLibrary, Stereotype.ABIE));
            validationService.AddValidator(new PRIMValidator());

            repository = new HierarchicalRepository();
            repository.OnItemCreatedOrModified += validationService.ItemCreatedOrModified;
            repository.OnItemDeleted += validationService.ItemDeleted;

            contentLoader = new RepositoryContentLoader(eaRepository);
            contentLoader.ItemLoaded += repository.ItemLoaded;
        }

		public IEnumerable<IDocLibrary> GetDocLibraries(int packageID)
		{
			throw new NotImplementedException();
		}
        public IEnumerable<ValidationIssue> ValidationIssues
        {
            get { return validationService.ValidationIssues; }
        }

        #region ICctsRepository Members

        public IEnumerable<IBLibrary> GetBLibraries()
        {
            return from item in repository.AllItems()
                   where item.Stereotype == Stereotype.bLibrary
                   select WrapItem(item) as IBLibrary;
        }

        public IEnumerable<IPrimLibrary> GetPrimLibraries()
        {
            return from item in repository.AllItems()
                   where item.Stereotype == Stereotype.PRIMLibrary
                   select WrapItem(item) as IPrimLibrary;
        }

        public IEnumerable<IEnumLibrary> GetEnumLibraries()
        {
            return from item in repository.AllItems()
                   where item.Stereotype == Stereotype.ENUMLibrary
                   select WrapItem(item) as IEnumLibrary;
        }

        public IEnumerable<ICdtLibrary> GetCdtLibraries()
        {
            return from item in repository.AllItems()
                   where item.Stereotype == Stereotype.CDTLibrary
                   select WrapItem(item) as ICdtLibrary;
        }

        public IEnumerable<ICcLibrary> GetCcLibraries()
        {
            return from item in repository.AllItems()
                   where item.Stereotype == Stereotype.CCLibrary
                   select WrapItem(item) as ICcLibrary;
        }

        public IEnumerable<object> GetAllLibraries()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IBdtLibrary> GetBdtLibraries()
        {
            return from item in repository.AllItems()
                   where item.Stereotype == Stereotype.BDTLibrary
                   select WrapItem(item) as IBdtLibrary;
        }

        public IEnumerable<IBieLibrary> GetBieLibraries()
        {
            return from item in repository.AllItems()
                   where item.Stereotype == Stereotype.BIELibrary
                   select WrapItem(item) as IBieLibrary;
        }

        public IEnumerable<IDocLibrary> GetDocLibraries()
        {
            return from item in repository.AllItems()
                   where item.Stereotype == Stereotype.DOCLibrary
                   select WrapItem(item) as IDocLibrary;
        }

        public IBLibrary GetBLibraryById(int id)
        {
            return GetLibraryById(id) as IBLibrary;
        }

        public IPrimLibrary GetPrimLibraryById(int id)
        {
            return GetLibraryById(id) as IPrimLibrary;
        }

        public IEnumLibrary GetEnumLibraryById(int id)
        {
            return GetLibraryById(id) as IEnumLibrary;
        }

        public ICdtLibrary GetCdtLibraryById(int id)
        {
            return GetLibraryById(id) as ICdtLibrary;
        }

        public ICcLibrary GetCcLibraryById(int id)
        {
            return GetLibraryById(id) as ICcLibrary;
        }

        public IBdtLibrary GetBdtLibraryById(int id)
        {
            return GetLibraryById(id) as IBdtLibrary;
        }

        public IBieLibrary GetBieLibraryById(int id)
        {
            return GetLibraryById(id) as IBieLibrary;
        }

        public IDocLibrary GetDocLibraryById(int id)
        {
            return GetLibraryById(id) as IDocLibrary;
        }

        public IIdScheme GetIdSchemeByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IPrim GetPrimById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnum GetEnumById(int id)
        {
            throw new NotImplementedException();
        }

        public IAbie GetAbieByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IMa GetMaByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Path> GetRootLocations()
        {
            throw new NotImplementedException();
        }

        public IBLibrary CreateRootBLibrary(Path rootLocation, BLibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        public ICdt GetCdtById(int id)
        {
            throw new NotImplementedException();
        }

        public IAcc GetAccById(int id)
        {
            throw new NotImplementedException();
        }

        public IBdt GetBdtById(int id)
        {
            throw new NotImplementedException();
        }

        public IAbie GetAbieById(int id)
        {
            throw new NotImplementedException();
        }

        public IBLibrary GetBLibraryByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IPrimLibrary GetPrimLibraryByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IIdScheme GetIdSchemeById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumLibrary GetEnumLibraryByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public ICdtLibrary GetCdtLibraryByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public ICcLibrary GetCcLibraryByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IBdtLibrary GetBdtLibraryByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IBieLibrary GetBieLibraryByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IDocLibrary GetDocLibraryByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IPrim GetPrimByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IEnum GetEnumByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public ICdt GetCdtByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IMa GetMaById(int id)
        {
            throw new NotImplementedException();
        }

        public IAcc GetAccByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IBdt GetBdtByPath(Path path)
        {
            throw new NotImplementedException();
        }

        #endregion

        private object GetLibraryById(int id)
        {
            return WrapItem(repository.GetItemById(ItemId.ForPackage(id)));
        }

        public event Action<IEnumerable<ValidationIssue>> ValidationIssuesUpdated
        {
            add { validationService.ValidationIssuesUpdated += value; }
            remove { validationService.ValidationIssuesUpdated -= value; }
        }

        public ValidationIssue GetValidationIssue(int issueId)
        {
            return validationService.GetIssueById(issueId);
        }

        private static object WrapItem(RepositoryItem item)
        {
            return CCItemWrapper.Wrap(item);
        }

        public void ItemDeleted(ItemId id)
        {
            contentLoader.ItemDeleted(id);
            repository.ItemDeleted(id);
            validationService.Validate();
        }

        public void LoadRepositoryContent()
        {
            contentLoader.LoadRepositoryContent();
            validationService.Validate();
        }

        public void LoadItemByGUID(ObjectType objectType, string guid)
        {
            contentLoader.LoadItemByGUID(objectType, guid);
            validationService.Validate();
        }

        public void LoadElementByID(int id)
        {
            contentLoader.LoadElementByID(id);
            validationService.Validate();
        }

        public void LoadPackageByID(int id)
        {
            contentLoader.LoadPackageByID(id);
            validationService.Validate();
        }
    }
}