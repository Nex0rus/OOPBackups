namespace Backups.Interfaces.RepoInterfaces;

public interface IRepoComponent
{
    public Guid Id { get; }

    public IStreamIdentificator Identificator { get; }
    void Accept(IRepoVisitor visitor);
}
