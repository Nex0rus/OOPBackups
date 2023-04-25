namespace Backups.Interfaces.RepoInterfaces;

public interface IRepoFolder : IRepoComponent
{
    public IReadOnlyCollection<IRepoComponent> GetComponents();
}
