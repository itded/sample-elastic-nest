namespace Elnes.Exceptions;

/// <summary>
/// Represents an exception when the given Elastic index already exists.
/// </summary>
public class IndexExistsException : Exception
{
    public IndexExistsException(string name) : base(name)
    {
        // nothing
    }
}