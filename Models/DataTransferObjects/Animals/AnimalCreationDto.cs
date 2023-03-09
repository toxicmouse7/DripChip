using DripChip.Models.Entities;

namespace DripChip.Models.DataTransferObjects.Animals;

public class AnimalCreationDto
{
    public IEnumerable<uint> AnimalTypes { get; set; } = null!;
    public float Weight { get; set; }
    public float Length { get; set; }
    public float Height { get; set; }
    public Animal.Gender Gender { get; set; }
    public uint ChipperId { get; set; }
    public uint ChippingLocationId { get; set; }
}