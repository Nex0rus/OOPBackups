using Backups.Interfaces;

namespace Backups.Models;

public class StreamIdentificator : IStreamIdentificator, IEquatable<StreamIdentificator>
{
    private readonly List<string> _identificatorParts;
    private char _separator = Path.DirectorySeparatorChar;

    public StreamIdentificator()
    {
        _identificatorParts = new List<string>();
    }

    public StreamIdentificator(string identificator)
    {
        _identificatorParts = new List<string>();
        string[] splitted = identificator.Split(_separator);
        _identificatorParts.AddRange(splitted);
    }

    public StreamIdentificator(List<string> identificatorParts)
    {
        _identificatorParts = identificatorParts;
    }

    public IReadOnlyCollection<string> IdentificatorParts => _identificatorParts;
    public IStreamIdentificator Append(string partitialIdentificator)
    {
        string[] splitted = partitialIdentificator.Split(_separator);
        var newIdentificator = new List<string>();
        newIdentificator.AddRange(_identificatorParts);
        newIdentificator.AddRange(splitted);
        return new StreamIdentificator(newIdentificator);
    }

    public IStreamIdentificator Empty()
    {
        return new StreamIdentificator();
    }

    public bool Equals(StreamIdentificator? other)
    {
        return other is not null && _identificatorParts.SequenceEqual(other._identificatorParts);
    }

    public string GetIdentificator()
    {
        return string.Join(_separator, _identificatorParts);
    }

    public IStreamIdentificator Last()
    {
        return new StreamIdentificator(_identificatorParts.Last());
    }

    public IStreamIdentificator Prev()
    {
        var prevParts = new List<string>();
        for (int i = 0; i < _identificatorParts.Count() - 1; ++i)
        {
            prevParts.Add(_identificatorParts[i]);
        }

        return new StreamIdentificator(prevParts);
    }
}
