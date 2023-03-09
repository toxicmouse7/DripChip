using DripChip.Models.Entities;
using DripChip.Services;

namespace DripChip.Models.DataTransferObjects.Animals;

public class TestDto : IDtoMapper<User>
{
    public User FromDto<TDto>(TDto dto)
    {
        throw new NotImplementedException();
    }

    public TDto ToDto<TDto>(User entity)
    {
        throw new NotImplementedException();
    }
}