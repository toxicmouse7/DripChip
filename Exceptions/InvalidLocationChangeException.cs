namespace DripChip.Exceptions;

public class InvalidLocationChangeException : Exception
{
    public InvalidLocationChangeException(string message)
        : base(message)
    {
    }

    public InvalidLocationChangeException()
        : base("Invalid location change")
    {
    }
}