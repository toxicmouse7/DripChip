using System.Data;
using DripChip.Models;
using DripChip.Models.Entities;
using DripChip.Models.SearchInformation;

namespace DripChip.Services;

public class AccountsRepository : IRepository<User>, IFilterable<User, UsersSearchInformation>
{
    private readonly ApplicationContext _applicationContext;

    public AccountsRepository(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public User? Get(uint id)
    {
        return _applicationContext.Users.Find(id);
    }

    public User Update(User entity)
    {
        var foundUser = _applicationContext.Users.Find(entity.Id);
        if (foundUser is null)
            throw new ArgumentOutOfRangeException(nameof(entity.Id),"User with this id was not found");
        
        if (_applicationContext.Users.SingleOrDefault(user => user.Email == entity.Email) != foundUser)
            throw new DuplicateNameException("User with this email already exists");

        foundUser.Email = entity.Email;
        foundUser.FirstName = entity.FirstName;
        foundUser.LastName = entity.LastName;
        foundUser.Password = entity.Password;

        _applicationContext.SaveChanges();
        return foundUser;
    }

    public IEnumerable<User> Search(UsersSearchInformation searchInformation, int from, int size)
    {
        var users = _applicationContext.Users.AsQueryable();

        return users
            .WhereIf(searchInformation.FirstName != null,
                user => user.FirstName.ToLower().Contains(searchInformation.FirstName!.ToLower()))
            .WhereIf(searchInformation.LastName != null,
                user => user.LastName.ToLower().Contains(searchInformation.LastName!.ToLower()))
            .WhereIf(searchInformation.Email != null,
                user => user.Email.ToLower().Contains(searchInformation.Email!.ToLower()))
            .OrderBy(user => user.Id).Skip(from).Take(size);
    }

    public User Create(User user)
    {
        if (_applicationContext.Users.FirstOrDefault(existingUser => existingUser.Email == user.Email) is not null)
            throw new ArgumentException($"User with email {user.Email} already exists");
        
        var createdUser = _applicationContext.Users.Add(user);
        _applicationContext.SaveChanges();
        return createdUser.Entity;
    }

    public void Delete(User entity)
    {
        throw new NotImplementedException();
    }

    public User? Get(Func<User, bool> pred)
    {
        return _applicationContext.Users.FirstOrDefault(pred);
    }
}