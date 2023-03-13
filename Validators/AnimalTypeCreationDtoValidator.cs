using System.Text.RegularExpressions;
using DripChip.Models.DataTransferObjects.AnimalTypes;
using FluentValidation;

namespace DripChip.Validators;

public partial class AnimalTypeCreationDtoValidator : AbstractValidator<AnimalTypeCreationDto>
{
    public AnimalTypeCreationDtoValidator()
    {
        RuleFor(x => x.Type).NotEmpty().Matches(AsciiOnlyRegex());
    }

    [GeneratedRegex("[0-9a-zA-Z\\-]*")]
    private static partial Regex AsciiOnlyRegex();
}