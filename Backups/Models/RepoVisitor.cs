using System.IO.Compression;
using Backups.Interfaces;
using Backups.Interfaces.ArchiveInterfaces;
using Backups.Interfaces.RepoInterfaces;

namespace Backups.Models;

public class RepoVisitor : IRepoVisitor
{
    private readonly Stack<ZipArchive> _zipArchives = new Stack<ZipArchive>();
    private readonly Stack<List<IArchiveComponent>> _components = new Stack<List<IArchiveComponent>>();
    public RepoVisitor(ZipArchive archive)
    {
        _zipArchives.Push(archive);
        _components.Push(new List<IArchiveComponent>());
    }

    public void Visit(IRepoFile component)
    {
        ZipArchive destArchive = _zipArchives.Peek();
        ZipArchiveEntry newEntry = destArchive.CreateEntry(component.Identificator.Last().GetIdentificator());
        using Stream destStream = newEntry.Open();
        using (Stream sourceStream = component.OpenRead())
        {
            sourceStream.CopyTo(destStream);
        }

        var newZipFile = new ZipFile(component.Identificator.Last());
        _components.Peek().Add(newZipFile);
    }

    public void Visit(IRepoFolder component)
    {
        ZipArchive destArchive = _zipArchives.Peek();
        IStreamIdentificator archiveIdentificator = component.Identificator.Empty().Append(component.Identificator.Last().GetIdentificator() + ".zip");
        ZipArchiveEntry newEntry = destArchive.CreateEntry(archiveIdentificator.Last().GetIdentificator());
        using Stream destStream = newEntry.Open();
        using var newZipArchive = new ZipArchive(destStream, ZipArchiveMode.Create);
        _zipArchives.Push(newZipArchive);
        _components.Push(new List<IArchiveComponent>());
        foreach (IRepoComponent innerComponent in component.GetComponents())
        {
            innerComponent.Accept(this);
        }

        _zipArchives.Pop();
        List<IArchiveComponent> addedComponents = _components.Pop();
        var newFolder = new ZipFolder(archiveIdentificator.Last(), addedComponents);
        _components.Peek().Add(newFolder);
    }

    public IReadOnlyCollection<IArchiveComponent> GetArchiveComponent()
    {
        return _components.Peek();
    }
}
