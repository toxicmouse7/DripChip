using DripChip.Models.DataTransferObjects.Locations;
using FluentValidation;

namespace DripChip.Validators;

public class LocationCreationDtoValidator : AbstractValidator<LocationCreationDto>
{
    public LocationCreationDtoValidator()
    {
        RuleFor(x => x.Latitude).InclusiveBetween(-90, 90);
        RuleFor(x => x.Longitude).InclusiveBetween(-180, 180);
    }
}