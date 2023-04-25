using System.IO.Compression;
using Backups.Interfaces;
using Backups.Interfaces.ArchiveInterfaces;
using Backups.Interfaces.RepoInterfaces;

namespace Backups.Models;

public class ZipFile : IZipFile
{
    private readonly IStreamIdentificator _name;
    public ZipFile(IStreamIdentificator name)
    {
        _name = name;
    }

    public IStreamIdentificator Name => _name;
    public IRepoComponent GetRepoComponent(ZipArchiveEntry entry)
    {
        return new RepoFile(() => entry.Open(), Name);
    }
}
