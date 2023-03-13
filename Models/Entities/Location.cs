namespace DripChip.Models.Entities;

public class Location : Entity
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public ICollection<Animal> LinkedAnimals { get; set; } = null!;
}