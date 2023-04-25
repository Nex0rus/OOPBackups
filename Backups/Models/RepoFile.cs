using Backups.Interfaces;
using Backups.Interfaces.RepoInterfaces;

namespace Backups.Models;

public class RepoFile : IRepoFile
{
    private readonly IStreamIdentificator _path;
    private readonly Func<Stream> _getStream;
    public RepoFile(Func<Stream> getStream, IStreamIdentificator path)
    {
        _path = path;
        _getStream = getStream;
        Id = Guid.NewGuid();
    }

    public Guid Id { get; }

    public IStreamIdentificator Identificator => _path;

    public void Accept(IRepoVisitor visitor)
    {
        visitor.Visit(this);
    }

    public Stream OpenRead()
    {
        return _getStream();
    }
}
