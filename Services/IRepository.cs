using DripChip.Models.Entities;

namespace DripChip.Services;

public interface IRepository<T> where T : Entity
{
    public T Get(uint id);
    public T Get(Func<T, bool> predicate);
    public void Update(T entity);
    public void Create(T entity);

    public void Delete(uint id);
}