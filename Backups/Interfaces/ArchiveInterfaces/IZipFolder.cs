namespace Backups.Interfaces.ArchiveInterfaces;

public interface IZipFolder : IArchiveComponent
{
    public IReadOnlyCollection<IArchiveComponent> Components { get; }
}
