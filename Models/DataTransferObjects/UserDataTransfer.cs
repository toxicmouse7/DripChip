using DripChip.Models.Entities;

namespace DripChip.Models.DataTransferObjects;

public class UserDataTransfer
{
    public uint Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    
    public UserDataTransfer(User user)
    {
        Id = user.Id;
        FirstName = user.FirstName;
        LastName = user.LastName;
        Email = user.Email;
    }
}