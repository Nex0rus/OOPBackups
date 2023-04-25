namespace Backups.Interfaces.DomainInterfaces;

public interface IRestorePoint
{
    public Guid Id { get; }
    public IReadOnlyCollection<IBackupObject> BackupObjects { get; }

    public IStorage Storage { get; }
    public DateTime DateTime { get; }
}
