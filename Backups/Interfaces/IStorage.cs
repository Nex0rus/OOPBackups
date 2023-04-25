using Backups.Interfaces.ArchiveInterfaces;
using Backups.Interfaces.RepoInterfaces;

namespace Backups.Interfaces;

public interface IStorage
{
    public IStreamIdentificator Path { get; }
    public IReadOnlyCollection<IArchiveComponent> GetArchive();

    public IReadOnlyCollection<IRepoComponent> GetRepoComponents();
}