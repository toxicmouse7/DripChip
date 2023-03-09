using System.Data;
using DripChip.Exceptions;
using DripChip.Models;
using DripChip.Models.Entities;
using DripChip.Models.FilterData;

namespace DripChip.Services;

public class AccountsRepository : IRepository<User>, IFilterable<User, UsersFilterData>
{
    private readonly ApplicationContext _applicationContext;

    public AccountsRepository(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public User Get(uint id)
    {
        var foundUser = _applicationContext.Users.Find(id);
        if (foundUser is null)
            throw new EntityNotFoundException();
        
        return foundUser;
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

    public IEnumerable<User> Search(UsersFilterData filterData, int from, int size)
    {
        var users = _applicationContext.Users.AsQueryable();

        return users
            .WhereIf(filterData.FirstName != null,
                user => user.FirstName.ToLower().Contains(filterData.FirstName!.ToLower()))
            .WhereIf(filterData.LastName != null,
                user => user.LastName.ToLower().Contains(filterData.LastName!.ToLower()))
            .WhereIf(filterData.Email != null,
                user => user.Email.ToLower().Contains(filterData.Email!.ToLower()))
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

    public void Delete(uint id)
    {
        var foundUser = _applicationContext.Users.Find(id);
        if (foundUser is null)
            throw new ArgumentException($"User with id = {id} not found");
        if (foundUser.ChippedAnimals.Any())
            throw new LinkedWithAnimalException();
        _applicationContext.Users.Remove(foundUser);
    }

    public User Get(Func<User, bool> pred)
    {
        var foundUser = _applicationContext.Users.FirstOrDefault(pred);
        if (foundUser is null)
            throw new EntityNotFoundException();
        return foundUser;
    }
}