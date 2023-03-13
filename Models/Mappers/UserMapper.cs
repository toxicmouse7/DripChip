using DripChip.Models.DataTransferObjects.Accounts;
using DripChip.Models.Entities;
using DripChip.Services;

namespace DripChip.Models.Mappers;

public class UserMapper : IMapper<User, UserCreationDto, UserUpdateDto, UserRepsonseDto>
{
    public User Create(UserCreationDto dto)
    {
        return new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Password = dto.Password
        };
    }

    public User Update(User entity, UserUpdateDto dto)
    {
        return new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Password = dto.Password
        };
    }

    public UserRepsonseDto ToResponse(User entity)
    {
        return new UserRepsonseDto
        {
            Id = entity.Id,
            FirstName = entity.LastName,
            LastName = entity.LastName,
            Email = entity.Email
        };
    }
}