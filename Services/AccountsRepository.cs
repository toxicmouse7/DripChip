using System.Data;
using DripChip.Exceptions;
using DripChip.Models;
using DripChip.Models.Entities;
using DripChip.Models.FilterData;
using Microsoft.EntityFrameworkCore;

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

    public void Update(User entity)
    {
        _applicationContext.Users.Update(entity);
        _applicationContext.SaveChanges();
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

    public void Create(User user)
    {
        _applicationContext.Users.Add(user);
        _applicationContext.SaveChanges();
    }

    public void Delete(uint id)
    {
        var foundUser = _applicationContext.Users
            .Include(x => x.ChippedAnimals)
            .FirstOrDefault(x => x.Id == id);
        if (foundUser is null)
            throw new ArgumentException($"User with id = {id} not found");
        if (foundUser.ChippedAnimals.Any())
            throw new LinkedWithAnimalException();
        _applicationContext.Users.Remove(foundUser);
        _applicationContext.SaveChanges();
    }

    public User Get(Func<User, bool> pred)
    {
        var foundUser = _applicationContext.Users.FirstOrDefault(pred);
        if (foundUser is null)
            throw new EntityNotFoundException();
        return foundUser;
    }
}