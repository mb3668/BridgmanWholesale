namespace Catalog.API.Features.Furniture;

public class FurnitureGroupModel
{
    public int FurnitureGroupId { get; set; }
    public decimal FurnitureGroupPrice { get; set; }
    public bool FurnitureGroupIsDelivered { get; set; }
    public byte[] FurnitureImage { get; set; } = new byte[0];
}
