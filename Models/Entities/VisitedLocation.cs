namespace DripChip.Models.Entities;

public class VisitedLocation
{
    public uint Id { get; set; }
    public Location Location { get; set; } = null!;
    public DateTime Time { get; set; }
};