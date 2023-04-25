namespace Backups.Interfaces.RepoInterfaces;

public interface IRepository
{
    public IStreamIdentificator Identificator { get; }
    public Stream OpenWrite(IStreamIdentificator identificatorToOpen);
    public IRepoComponent GetComponent(IStreamIdentificator identificator);
}
