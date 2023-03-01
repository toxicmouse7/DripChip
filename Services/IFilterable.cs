using DripChip.Models.Entities;

namespace DripChip.Services;

public interface IFilterable<out T, in TS> where T : Entity
{
    public IEnumerable<T> Search(TS searchInformation, int from, int size);
}