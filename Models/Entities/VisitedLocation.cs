namespace DripChip.Models.Entities;

public class VisitedLocation : Entity
{
    public Location Location { get; set; } = null!;
    public DateTime Time { get; set; }
};