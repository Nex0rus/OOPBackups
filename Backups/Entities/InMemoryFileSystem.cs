using Backups.Interfaces;
using Backups.Interfaces.RepoInterfaces;
using Backups.Models;
using Zio;
using Zio.FileSystems;

namespace Backups.Entities;

public class InMemoryFileSystem : IRepository, IDisposable
{
    private readonly MemoryFileSystem _inMemoryFS;
    public InMemoryFileSystem(MemoryFileSystem memoryFileSystem, IStreamIdentificator identificator)
    {
        _inMemoryFS = memoryFileSystem;
        Identificator = identificator;
    }

    public IStreamIdentificator Identificator { get; }

    public bool DirectoryExists(IStreamIdentificator identificator)
    {
        return _inMemoryFS.DirectoryExists(identificator.GetIdentificator());
    }

    public bool FileExists(IStreamIdentificator identificator)
    {
        return _inMemoryFS.FileExists(identificator.GetIdentificator());
    }

    public IRepoComponent GetComponent(IStreamIdentificator identificator)
    {
        string path = identificator.GetIdentificator();
        if (_inMemoryFS.FileExists(path))
        {
            return new RepoFile(() => GetFileStream(identificator), identificator);
        }
        else if (_inMemoryFS.DirectoryExists(path))
        {
            return new RepoFolder(() => TraverseFolder(identificator), identificator);
        }

        throw new ArgumentException();
    }

    public Stream OpenWrite(IStreamIdentificator identificatorToOpen)
    {
        _inMemoryFS.CreateDirectory(identificatorToOpen.Prev().GetIdentificator());
        return _inMemoryFS.OpenFile(identificatorToOpen.GetIdentificator(), FileMode.OpenOrCreate, FileAccess.Write);
    }

    public void CreateFile(IStreamIdentificator fileIdentificator)
    {
        Stream file = _inMemoryFS.CreateFile(fileIdentificator.GetIdentificator());
        file.Close();
    }

    public void CreateFolder(IStreamIdentificator folderIdentificator)
    {
        _inMemoryFS.CreateDirectory(folderIdentificator.GetIdentificator());
    }

    public void Dispose()
    {
        _inMemoryFS.Dispose();
    }

    private Stream GetFileStream(IStreamIdentificator fileIdentificator)
    {
        return _inMemoryFS.OpenFile(fileIdentificator.GetIdentificator(), FileMode.Open, FileAccess.Read);
    }

    private IReadOnlyCollection<IRepoComponent> TraverseFolder(IStreamIdentificator folderIdentificator)
    {
        var folderComponents = new List<IRepoComponent>();
        string folderPath = folderIdentificator.GetIdentificator();
        IEnumerable<UPath> filePaths = _inMemoryFS.EnumerateFiles(folderPath, "*", SearchOption.TopDirectoryOnly);
        foreach (UPath filePath in filePaths)
        {
            var fileIdentificator = new StreamIdentificator(filePath.FullName);
            var newFile = new RepoFile(() => GetFileStream(fileIdentificator), fileIdentificator);
            folderComponents.Add(newFile);
        }

        Func<StreamIdentificator, IReadOnlyCollection<IRepoComponent>> getComponentsMethod = TraverseFolder;
        IEnumerable<UPath> directoryPaths = _inMemoryFS.EnumerateDirectories(folderPath, "*", SearchOption.TopDirectoryOnly);
        foreach (UPath directoryPath in directoryPaths)
        {
            var dirIdentificator = new StreamIdentificator(directoryPath.FullName);
            var newFolder = new RepoFolder(() => TraverseFolder(dirIdentificator), dirIdentificator);
            folderComponents.Add(newFolder);
        }

        return folderComponents;
    }
}
