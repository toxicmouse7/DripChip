using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Models.DataTransferObjects;

public class EntityIdDto
{
    [Range(1, uint.MaxValue)]
    public uint Id { get; set; }
}