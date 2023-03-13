using DripChip.Models.Entities;

namespace DripChip.Models.DataTransferObjects.Animals;

public class AnimalUpdateDto
{
    public float Weight { get; set; }
    public float Length { get; set; }
    public float Height { get; set; }
    public Animal.Gender Gender { get; set; }
    public Animal.LifeStatus LifeStatus { get; set; }
    public uint ChipperId { get; set; }
    public uint ChippingLocationId { get; set; }
}