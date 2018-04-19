using CctsRepository;
using EA;
using VIENNAAddIn.upcc3.ea;
using VIENNAAddIn.upcc3.repo;

namespace VIENNAAddIn.upcc3
{
    public static class CctsRepositoryFactory
    {
        public static ICctsRepository CreateCctsRepository(Repository eaRepository)
        {
            return new UpccRepository(EaUmlRepository.getNewRepository(eaRepository));
        }
    }
}
