using DripChip.Models.Entities;

namespace DripChip.Models.DataTransferObjects;

public static class UserMapper
{
    public static User FromDto(UserCreationDto userCreationDto)
    {
        return new User
        {
            FirstName = userCreationDto.FirstName,
            LastName = userCreationDto.LastName,
            Email = userCreationDto.Email,
            Password = userCreationDto.Password
        };
    }

    public static UserDto ToDto(User user)
    {
        return new UserDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Id = user.Id
        };
    }
}