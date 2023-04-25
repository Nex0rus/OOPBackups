namespace Backups.Interfaces.DomainInterfaces;

public interface IBackup
{
    public Guid Id { get; }
    public IReadOnlyCollection<IRestorePoint> RestorePoints { get; }
    public void AddRestorePoint(IRestorePoint restorePoint);
    public void RemoveRestorePoint(IRestorePoint restorePoint);
}
