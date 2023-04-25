using Backups.Interfaces;
using Backups.Interfaces.DomainInterfaces;

namespace Backups.Entities;

public class RestorePoint : IRestorePoint, IEquatable<RestorePoint>
{
    public RestorePoint(IReadOnlyCollection<IBackupObject> backupObjects, IStorage storage, DateTime time)
    {
        BackupObjects = backupObjects;
        Storage = storage;
        DateTime = time;
        Id = Guid.NewGuid();
    }

    public Guid Id { get; }

    public IReadOnlyCollection<IBackupObject> BackupObjects { get; }

    public IStorage Storage { get; }

    public DateTime DateTime { get; }

    public bool Equals(RestorePoint? other)
    {
        return other is not null && other.Id.Equals(Id);
    }
}
