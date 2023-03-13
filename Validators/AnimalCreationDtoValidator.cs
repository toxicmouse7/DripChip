using DripChip.Models.DataTransferObjects.Animals;
using FluentValidation;

namespace DripChip.Validators;

public class AnimalCreationDtoValidator : AbstractValidator<AnimalCreationDto>
{
    public AnimalCreationDtoValidator()
    {
        RuleForEach(x => x.AnimalTypes).NotEqual((uint)0);
        RuleFor(x => x.Height).GreaterThan(0);
        RuleFor(x => x.Weight).GreaterThan(0);
        RuleFor(x => x.Length).GreaterThan(0);
        RuleFor(x => x.ChipperId).NotEqual((uint)0);
        RuleFor(x => x.ChippingLocationId).NotEqual((uint)0);
    }
}