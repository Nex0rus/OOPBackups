using Backups.Interfaces;
using Backups.Interfaces.RepoInterfaces;
using Backups.Models;

namespace Backups.Entities;

public class FileSystemRepository : IRepository
{
    private readonly IStreamIdentificator _identificator;

    public FileSystemRepository(IStreamIdentificator identificator)
    {
        _identificator = identificator;
    }

    public IStreamIdentificator Identificator => _identificator;

    public IRepoComponent GetComponent(IStreamIdentificator identificator)
    {
        string path = identificator.GetIdentificator();
        if (File.Exists(path))
        {
            return new RepoFile(() => GetFileStream(identificator), identificator);
        }
        else if (Directory.Exists(path))
        {
            return new RepoFolder(() => TraverseFolder(identificator), identificator);
        }

        throw new ArgumentException();
    }

    public Stream OpenWrite(IStreamIdentificator identificatorToOpen)
    {
        Directory.CreateDirectory(identificatorToOpen.Prev().GetIdentificator());
        return new FileStream(identificatorToOpen.GetIdentificator(), FileMode.Create, FileAccess.Write);
    }

    public DirectoryInfo CreateDirectory(IStreamIdentificator identificator)
    {
        return Directory.CreateDirectory(identificator.GetIdentificator());
    }

    public void CreateFile(IStreamIdentificator identificator)
    {
        File.Create(identificator.GetIdentificator());
    }

    private Stream GetFileStream(IStreamIdentificator fileIdentificator)
    {
        return new FileStream(fileIdentificator.GetIdentificator(), FileMode.Open, FileAccess.Read);
    }

    private IReadOnlyCollection<IRepoComponent> TraverseFolder(IStreamIdentificator folderIdentificator)
    {
        string folderPath = folderIdentificator.GetIdentificator();
        string[] filePaths = Directory.GetFiles(folderPath, "*", SearchOption.TopDirectoryOnly);
        IEnumerable<IRepoComponent> fileComponents = filePaths
            .Select(filePath =>
            new StreamIdentificator(filePath))
            .Select(fileIdentificator =>
            new RepoFile(
                () => GetFileStream(fileIdentificator), fileIdentificator));

        string[] directoryPaths = Directory.GetDirectories(folderPath);
        IEnumerable<IRepoComponent> directoryComponents = directoryPaths
            .Select(dirPath =>
            new StreamIdentificator(dirPath))
            .Select(dirIdentificator =>
            new RepoFolder(
                () => TraverseFolder(dirIdentificator), dirIdentificator));

        return directoryComponents.Concat(fileComponents).ToList();
    }
}
