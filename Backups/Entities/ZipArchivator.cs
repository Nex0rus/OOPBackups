using System.IO.Compression;
using Backups.Interfaces;
using Backups.Interfaces.ArchiveInterfaces;
using Backups.Interfaces.RepoInterfaces;
using Backups.Models;

namespace Backups.Entities;

public class ZipArchivator : IArchivator
{
    public ZipArchivator()
    {
    }

    public IStorage CreateStorage(IReadOnlyCollection<IRepoComponent> components, IStreamIdentificator archiveName, IRepository destRepository)
    {
        var newStorage = new Storage(ArchiveComponents(archiveName, components, destRepository), GetZipArchiveName(archiveName, destRepository.Identificator), destRepository);
        return newStorage;
    }

    private IReadOnlyCollection<IArchiveComponent> ArchiveComponents(IStreamIdentificator archiveName, IReadOnlyCollection<IRepoComponent> components, IRepository destRepository)
    {
        using Stream repositoryStream = destRepository.OpenWrite(GetZipArchiveName(archiveName, destRepository.Identificator));
        using var archive = new ZipArchive(repositoryStream, ZipArchiveMode.Create);
        var visitor = new RepoVisitor(archive);
        foreach (IRepoComponent component in components)
        {
            component.Accept(visitor);
        }

        IReadOnlyCollection<IArchiveComponent> zipComponents = visitor.GetArchiveComponent();
        return zipComponents;
    }

    private IStreamIdentificator GetZipArchiveName(IStreamIdentificator archiveName, IStreamIdentificator destRepositoryIdentificator)
    {
        return destRepositoryIdentificator.Append(archiveName.GetIdentificator() + ".zip");
    }
}
