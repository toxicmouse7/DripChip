namespace DripChip.Exceptions;

public class DuplicateEntityException : Exception
{
    public DuplicateEntityException(string message)
        : base(message)
    {
    }

    public DuplicateEntityException()
        : base("This entity already exists")
    {
    }
}