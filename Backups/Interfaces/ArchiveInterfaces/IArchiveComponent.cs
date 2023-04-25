using System.IO.Compression;
using Backups.Interfaces.RepoInterfaces;

namespace Backups.Interfaces.ArchiveInterfaces;

public interface IArchiveComponent
{
    public IStreamIdentificator Name { get; }
    public IRepoComponent GetRepoComponent(ZipArchiveEntry entry);
}