namespace DripChip.Exceptions;

public class LinkedWithAnimalException : Exception
{
    public LinkedWithAnimalException() :
        base("User has chipped animals")
    {
    }
}