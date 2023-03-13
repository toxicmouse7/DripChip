namespace DripChip.Models.DataTransferObjects.VisitedLocations;

public class VisitedLocationsResponseDto
{
    public uint Id { get; set; }
    public DateTime DateTimeOfVisitLocationPoint { get; set; }
    public uint LocationPointId { get; set; }
}