using Backups.Interfaces;
using Backups.Interfaces.RepoInterfaces;

namespace Backups.Models;

public class RepoFolder : IRepoFolder
{
    private readonly IStreamIdentificator _identificator;
    private readonly Func<IReadOnlyCollection<IRepoComponent>> _getComponents;
    public RepoFolder(Func<IReadOnlyCollection<IRepoComponent>> getComponents, IStreamIdentificator path)
    {
        _identificator = path;
        _getComponents = getComponents;
        Id = Guid.NewGuid();
    }

    public Guid Id { get; }

    public IStreamIdentificator Identificator => _identificator;

    public IReadOnlyCollection<IRepoComponent> GetComponents()
    {
        return _getComponents();
    }

    public void Accept(IRepoVisitor visitor)
    {
        visitor.Visit(this);
    }
}
