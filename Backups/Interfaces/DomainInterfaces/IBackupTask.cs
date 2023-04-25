using Backups.Interfaces.ArchiveInterfaces;
using Backups.Interfaces.RepoInterfaces;

namespace Backups.Interfaces.DomainInterfaces;

public interface IBackupTask
{
    public Guid Id { get; }
    public void AddBackupObject(IBackupObject backupObject);
    public void RemoveBackupObject(IStreamIdentificator otherIdentificator);
    public void ExecuteTask(IArchivator archivator, DateTime time, IRepository destinationRepository);
    public void ChangeStorageAlgorithm(IStorageAlgorithm newAlgo);
}
