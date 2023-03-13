using DripChip.Exceptions;
using DripChip.Models.DataTransferObjects.Accounts;
using DripChip.Models.Entities;
using DripChip.Services;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Models.Mappers;

public class UserMapper : IMapper<User, UserCreationDto, UserUpdateDto, UserRepsonseDto>
{
    private readonly IRepository<User> _userRepository;

    public UserMapper(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public User Create(UserCreationDto dto)
    {
        try
        {
            _userRepository.Get(existingUser => existingUser.Email == dto.Email);
        }
        catch (EntityNotFoundException)
        {
            return new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = dto.Password
            };
        }

        throw new DuplicateEntityException();
    }

    public User Update(User entity, [FromBody] UserUpdateDto dto)
    {
        if (entity is null)
            throw new EntityNotFoundException("User with this id was not found");

        if (_userRepository.Get(user => user.Email == entity.Email) != entity)
            throw new DuplicateEntityException("User with this email already exists");

        entity.Email = dto.Email;
        entity.FirstName = dto.FirstName;
        entity.LastName = dto.LastName;
        entity.Password = dto.Password;

        return entity;
    }

    public UserRepsonseDto ToResponse(User entity)
    {
        return new UserRepsonseDto
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email
        };
    }
}