using Backups.Interfaces;
using Backups.Interfaces.DomainInterfaces;
using Backups.Interfaces.RepoInterfaces;

namespace Backups.Entities;

public class BackupObject : IBackupObject, IEquatable<BackupObject>
{
    public BackupObject(IStreamIdentificator identificator, IRepository repository)
    {
        StreamIdentificator = identificator;
        Id = Guid.NewGuid();
        Repository = repository;
    }

    public Guid Id { get; }

    public IStreamIdentificator StreamIdentificator { get; }

    public IRepository Repository { get; }

    public bool Equals(BackupObject? other)
    {
        return other is not null && other.Id.Equals(Id);
    }

    public IRepoComponent GetRepoComponent()
    {
        return Repository.GetComponent(StreamIdentificator);
    }
}
