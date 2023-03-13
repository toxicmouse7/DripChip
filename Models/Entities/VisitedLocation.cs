namespace DripChip.Models.Entities;

public class VisitedLocation : Entity
{
    public Location Location { get; init; } = null!;
    public DateTimeOffset Time { get; init; }
};