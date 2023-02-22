using DripChip.Models;
using DripChip.Models.Entities;
using DripChip.Models.SearchInformation;

namespace DripChip.Services;

public class AccountsService : IAccountsService
{
    private readonly ApplicationContext _applicationContext;

    public AccountsService(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public User? GetUserInformation(uint id)
    {
        return _applicationContext.Users.Find(id);
    }

    public User[] SearchUsers(UsersSearchInformation searchInformation, int from, int size)
    {
        var users = _applicationContext.Users.AsQueryable();

        return users
            .WhereIf(searchInformation.FirstName != null,
                user => user.FirstName.ToLower().Contains(searchInformation.FirstName!.ToLower()))
            .WhereIf(searchInformation.LastName != null,
                user => user.LastName.ToLower().Contains(searchInformation.LastName!.ToLower()))
            .WhereIf(searchInformation.Email != null,
                user => user.Email.ToLower().Contains(searchInformation.Email!.ToLower()))
            .OrderBy(user => user.Id).Skip(from).Take(size).ToArray();
    }

    public User CreateNew(User user)
    {
        if (_applicationContext.Users.FirstOrDefault(existingUser => existingUser.Email == user.Email) is not null)
            throw new ArgumentException($"User with email {user.Email} already exists");
        
        var createdUser = _applicationContext.Users.Add(user);
        _applicationContext.SaveChanges();
        return createdUser.Entity;
    }

    public User? FindBy(Func<User, bool> pred)
    {
        return _applicationContext.Users.FirstOrDefault(pred);
    }
}