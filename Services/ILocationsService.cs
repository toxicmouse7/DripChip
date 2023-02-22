using DripChip.Models;
using DripChip.Models.Entities;

namespace DripChip.Services;

public interface ILocationsService
{
    public Location? GetLocationInformation(uint id);
}