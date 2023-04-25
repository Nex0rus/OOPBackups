using Backups.Interfaces.RepoInterfaces;

namespace Backups.Interfaces.ArchiveInterfaces;

public interface IArchivator
{
    public IStorage CreateStorage(IReadOnlyCollection<IRepoComponent> components, IStreamIdentificator archiveName, IRepository destRepository);
}
