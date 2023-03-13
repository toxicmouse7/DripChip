namespace DripChip.Exceptions;

public class IncorrectLifeStatusException : Exception
{
    public IncorrectLifeStatusException(string message)
        : base(message)
    {
    }

    public IncorrectLifeStatusException()
        : base("Attempt to change life status from DEAD to ALIVE made")
    {
    }
}