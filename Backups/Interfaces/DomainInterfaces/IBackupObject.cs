using Backups.Interfaces.RepoInterfaces;

namespace Backups.Interfaces.DomainInterfaces;

public interface IBackupObject
{
    public Guid Id { get; }
    public IStreamIdentificator StreamIdentificator { get; }
    public IRepository Repository { get; }
    public IRepoComponent GetRepoComponent();
}
