using Backups.Interfaces.DomainInterfaces;

namespace Backups.Entities;

public class Backup : IBackup, IEquatable<Backup>
{
    private readonly List<IRestorePoint> _restorePoints;
    public Backup()
    {
        _restorePoints = new List<IRestorePoint>();
        Id = Guid.NewGuid();
    }

    public Guid Id { get; }

    public IReadOnlyCollection<IRestorePoint> RestorePoints => _restorePoints;

    public void AddRestorePoint(IRestorePoint restorePoint)
    {
        IRestorePoint? restorePointInCollection = RestorePoints.FirstOrDefault(x => x.Id.Equals(restorePoint.Id));
        if (restorePointInCollection is not null)
        {
            // NotUniqueRestorePoint
            throw new ArgumentException(nameof(restorePointInCollection));
        }

        _restorePoints.Add(restorePoint);
    }

    public bool Equals(Backup? other)
    {
        return other is not null && other.Id.Equals(Id);
    }

    public void RemoveRestorePoint(IRestorePoint restorePoint)
    {
        IRestorePoint? restorePointInCollection = RestorePoints.FirstOrDefault(x => x.Id.Equals(restorePoint.Id));
        if (restorePoint is null)
        {
            // NoRestorePointExists
            throw new ArgumentNullException(nameof(restorePoint));
        }
    }
}
