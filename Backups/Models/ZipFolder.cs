using System.IO.Compression;
using Backups.Interfaces;
using Backups.Interfaces.ArchiveInterfaces;
using Backups.Interfaces.RepoInterfaces;

namespace Backups.Models;

internal class ZipFolder : IZipFolder
{
    private readonly IStreamIdentificator _name;
    private readonly List<IArchiveComponent> _components;
    public ZipFolder(IStreamIdentificator name, List<IArchiveComponent> components)
    {
        _name = name;
        _components = components;
    }

    public IStreamIdentificator Name => _name;

    public IReadOnlyCollection<IArchiveComponent> Components => _components;

    public IRepoComponent GetRepoComponent(ZipArchiveEntry entry)
    {
        return new RepoFolder(() => TraverseFolder(entry), Name);
    }

    private IReadOnlyCollection<IRepoComponent> TraverseFolder(ZipArchiveEntry entry)
    {
        var components = new List<IRepoComponent>();
        Stream baseStream = entry.Open();
        var zipArchive = new ZipArchive(baseStream);
        foreach (IArchiveComponent innerComp in _components)
        {
            string innerEntryName = innerComp.Name.GetIdentificator();
            ZipArchiveEntry? innerEntry = zipArchive.GetEntry(innerEntryName);
            if (innerEntry is null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                IRepoComponent repoObject = innerComp.GetRepoComponent(innerEntry);
                components.Add(repoObject);
            }
        }

        return components;
    }
}
