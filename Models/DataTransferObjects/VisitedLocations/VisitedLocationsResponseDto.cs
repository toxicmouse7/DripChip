﻿namespace DripChip.Models.DataTransferObjects.VisitedLocations;

public class VisitedLocationsResponseDto
{
    public uint Id { get; set; }
    public DateTimeOffset DateTimeOfVisitLocationPoint { get; set; }
    public uint LocationPointId { get; set; }
}