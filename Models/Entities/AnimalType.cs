namespace DripChip.Models.Entities;

public class AnimalType : Entity
{
    public string Type { get; set; } = null!;

    public virtual ICollection<Animal> Animals { get; set; } = null!;
}