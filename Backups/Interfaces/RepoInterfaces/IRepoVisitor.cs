namespace Backups.Interfaces.RepoInterfaces;

public interface IRepoVisitor
{
    void Visit(IRepoFile component);

    void Visit(IRepoFolder component);
}
