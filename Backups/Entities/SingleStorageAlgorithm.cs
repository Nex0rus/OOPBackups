using Backups.Interfaces;
using Backups.Interfaces.ArchiveInterfaces;
using Backups.Interfaces.RepoInterfaces;

namespace Backups.Entities;

public class SingleStorageAlgorithm : IStorageAlgorithm
{
    public SingleStorageAlgorithm()
    {
    }

    public IStorage CreateStorage(IStreamIdentificator archiveName, IArchivator archivator, IReadOnlyCollection<IRepoComponent> components, IRepository destRepository)
    {
        IStorage storage = archivator.CreateStorage(components, archiveName, destRepository);
        return storage;
    }
}
