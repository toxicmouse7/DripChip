using System.ComponentModel.DataAnnotations;
using DripChip.Models.Entities;

namespace DripChip.Models.SearchInformation;

public record AnimalsSearchInformation(
    DateTime? StartDateTime,
    DateTime? EndDateTime,
    [Range(1, uint.MaxValue)] uint? ChipperId,
    [Range(1, uint.MaxValue)] uint? ChippingLocationId,
    Animal.LifeStatus? LifeStatus,
    Animal.Gender? Gender
);