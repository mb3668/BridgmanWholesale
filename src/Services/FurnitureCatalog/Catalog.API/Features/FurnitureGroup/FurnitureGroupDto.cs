namespace Catalog.API.Features.Furniture;

public class FurnitureGroupDto
{
    public decimal FurnitureGroupPrice { get; set; }
    public bool FurnitureGroupIsDelivered { get; set; }
    public byte[] FurnitureImage { get; set; } = new byte[0];
}
