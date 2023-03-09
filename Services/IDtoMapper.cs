using DripChip.Models.Entities;

namespace DripChip.Services;

public interface IDtoMapper<TEntity> where TEntity : Entity
{
    public TEntity FromDto<TDto>(TDto dto);
    public TDto ToDto<TDto>(TEntity entity);
}