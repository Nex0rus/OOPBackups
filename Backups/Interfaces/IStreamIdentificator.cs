namespace Backups.Interfaces;

public interface IStreamIdentificator
{
    public IReadOnlyCollection<string> IdentificatorParts { get; }
    public string GetIdentificator();
    public IStreamIdentificator Append(string partitialIdentificator);

    public IStreamIdentificator Last();

    public IStreamIdentificator Empty();

    public IStreamIdentificator Prev();
}
