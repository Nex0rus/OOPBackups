using Backups.Interfaces;
using Backups.Interfaces.ArchiveInterfaces;
using Backups.Interfaces.RepoInterfaces;
using Backups.Models;

namespace Backups.Entities;

public class SplitStorageAlgorithm : IStorageAlgorithm
{
    public SplitStorageAlgorithm()
    {
    }

    public IStorage CreateStorage(IStreamIdentificator archiveName, IArchivator archivator, IReadOnlyCollection<IRepoComponent> components, IRepository destRepository)
    {
        var storages = components
            .Select(component =>
            new
            {
                Component = component,
                Name = archiveName.Append(Guid.NewGuid().ToString()),
            })
            .Select(x =>
            archivator.CreateStorage(new List<IRepoComponent>() { x.Component }, x.Name, destRepository))
            .ToList();

        var newStorage = new SplitStorage(storages, archiveName, destRepository);
        return newStorage;
    }
}
