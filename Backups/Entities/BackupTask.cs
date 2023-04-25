using Backups.Interfaces;
using Backups.Interfaces.ArchiveInterfaces;
using Backups.Interfaces.DomainInterfaces;
using Backups.Interfaces.RepoInterfaces;

namespace Backups.Entities;

public class BackupTask : IBackupTask, IEquatable<BackupTask>
{
    private readonly List<IBackupObject> _backupObjects = new List<IBackupObject>();
    private readonly IBackup _backup;
    private readonly IStreamIdentificator _restorePointName;
    private int _version = 1;
    private IStorageAlgorithm _storageAlgorithm;

    public BackupTask(
        IBackup backup,
        IStorageAlgorithm storageAlgorithm,
        IStreamIdentificator baseRestorePointName)
    {
        _backup = backup;
        _storageAlgorithm = storageAlgorithm;
        _restorePointName = baseRestorePointName;
        Id = Guid.NewGuid();
    }

    public Guid Id { get; }

    public IReadOnlyCollection<IRestorePoint> RestorePoints => _backup.RestorePoints;

    public void AddBackupObject(IBackupObject backupObject)
    {
        IBackupObject? backupObjectInCollection = _backupObjects.Find(x => x.StreamIdentificator.Equals(backupObject.StreamIdentificator));
        if (backupObjectInCollection is not null)
        {
            // NotUniqueBackupObject
            throw new ArgumentException(nameof(backupObjectInCollection));
        }

        _backupObjects.Add(backupObject);
    }

    public bool Equals(BackupTask? other)
    {
        return other is not null && other.Id.Equals(Id);
    }

    public void ExecuteTask(IArchivator archivator, DateTime time, IRepository destinationRepository)
    {
        IReadOnlyCollection<IRepoComponent> componentsToBackup = _backupObjects
            .Select(backupObject =>
            destinationRepository.GetComponent(backupObject.StreamIdentificator))
            .ToList();
        IStorage newStorage = _storageAlgorithm.CreateStorage(GetNextRestorePointIdentifiactor(), archivator, componentsToBackup, destinationRepository);
        IRestorePoint restorePoint = new RestorePoint(_backupObjects, newStorage, time);
        _backup.AddRestorePoint(restorePoint);
    }

    public void ChangeStorageAlgorithm(IStorageAlgorithm newAlgo)
    {
        _storageAlgorithm = newAlgo;
    }

    public void RemoveBackupObject(IStreamIdentificator otherIdentificator)
    {
        IBackupObject? backupObjectInCollection = _backupObjects.Find(x => x.StreamIdentificator.Equals(otherIdentificator));
        if (backupObjectInCollection is null)
        {
            throw new ArgumentNullException(nameof(backupObjectInCollection));
        }

        _backupObjects.Remove(backupObjectInCollection);
    }

    private IStreamIdentificator GetNextRestorePointIdentifiactor()
    {
        return _restorePointName.Empty().Append(_restorePointName.GetIdentificator() + $"({_version++})");
    }
}
