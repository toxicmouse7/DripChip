using System.ComponentModel.DataAnnotations;

namespace DripChip.Models.DataTransferObjects.AnimalTypes;

public class AnimalTypeUpdateDto
{
    [Range(1, uint.MaxValue)]
    public uint OldTypeId { get; set; }
    [Range(1, uint.MaxValue)]
    public uint NewTypeId { get; set; }
}