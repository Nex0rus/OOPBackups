namespace Backups.Interfaces.RepoInterfaces;

public interface IRepoFile : IRepoComponent
{
    public Stream OpenRead();
}
