using DripChip.Models.Entities;

namespace DripChip.Services;

public interface IRepository<T> where T : Entity
{
    public T? Get(uint id);
    public T? Get(Func<T, bool> predicate);
    public T Update(T entity);
    public T Create(T entity);

    public void Delete(uint id);
}