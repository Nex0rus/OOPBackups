using System.IO.Compression;
using Backups.Interfaces;
using Backups.Interfaces.ArchiveInterfaces;
using Backups.Interfaces.RepoInterfaces;

namespace Backups.Models;

public class Storage : IStorage
{
    private readonly IReadOnlyCollection<IArchiveComponent> _archive;
    private readonly IRepository _repository;
    public Storage(IReadOnlyCollection<IArchiveComponent> component, IStreamIdentificator path, IRepository repository)
    {
        _repository = repository;
        _archive = component;
        Path = path;
        Id = Guid.NewGuid();
    }

    public Guid Id { get; }
    public IStreamIdentificator Path { get; }
    public IReadOnlyCollection<IArchiveComponent> GetArchive()
    {
        return _archive;
    }

    public IReadOnlyCollection<IRepoComponent> GetRepoComponents()
    {
        IRepoComponent baseRepoComponent = _repository.GetComponent(Path);
        if (baseRepoComponent is IRepoFile zipFile)
        {
            Stream zipStream = zipFile.OpenRead();
            var zipArchive = new ZipArchive(zipStream);
            var repoComponents = new List<IRepoComponent>();
            foreach (IArchiveComponent innerComp in _archive)
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
                    repoComponents.Add(repoObject);
                }
            }

            return repoComponents;
        }
        else
        {
            throw new InvalidCastException("That exception should never have occured. Filed to cast archive to file object, but archive is always a file for such implementation of Storage");
        }
    }
}
