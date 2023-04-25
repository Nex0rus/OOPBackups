using Backups.Interfaces;
using Backups.Interfaces.ArchiveInterfaces;
using Backups.Interfaces.RepoInterfaces;

namespace Backups.Models;

public class SplitStorage : IStorage
{
    private readonly List<IStorage> _storages;
    private readonly IStreamIdentificator _path;
    private readonly IRepository _repository;

    public SplitStorage(List<IStorage> storages, IStreamIdentificator path, IRepository repository)
    {
        _storages = storages;
        _path = path;
        _repository = repository;
    }

    public IStreamIdentificator Path => _path;

    public IReadOnlyCollection<IArchiveComponent> GetArchive()
    {
        var components = new List<IArchiveComponent>();
        foreach (IStorage singleStorage in _storages)
        {
            components.AddRange(singleStorage.GetArchive());
        }

        return components;
    }

    public IReadOnlyCollection<IRepoComponent> GetRepoComponents()
    {
        var components = new List<IRepoComponent>();
        foreach (IStorage singleStorage in _storages)
        {
            IReadOnlyCollection<IRepoComponent> singleStorageComponents = singleStorage.GetRepoComponents();
            components.AddRange(singleStorageComponents);
        }

        return components;
    }
}
