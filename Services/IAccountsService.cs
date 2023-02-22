using DripChip.Models;
using DripChip.Models.Entities;
using DripChip.Models.SearchInformation;

namespace DripChip.Services;

public interface IAccountsService
{
    public User? GetUserInformation(uint id);

    public User[] SearchUsers(UsersSearchInformation searchInformation, int from, int size);

    public User CreateNew(User user);

    public User? FindBy(Func<User, bool> pred);
}