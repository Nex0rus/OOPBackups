using Backups.Interfaces.ArchiveInterfaces;
using Backups.Interfaces.RepoInterfaces;

namespace Backups.Interfaces;

public interface IStorageAlgorithm
{
    public IStorage CreateStorage(IStreamIdentificator archiveName, IArchivator archivator, IReadOnlyCollection<IRepoComponent> components, IRepository destRepository);
}
