using DripChip.Models.Entities;

namespace DripChip.Services;

public interface IMapper<TEntity, in TCreationDto, in TUpdateDto, out TResponseDto> where TEntity : Entity
{
    public TEntity Create(TCreationDto dto);
    public TEntity Update(TEntity entity, TUpdateDto dto);
    public TResponseDto ToResponse(TEntity entity);
}