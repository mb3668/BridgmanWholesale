namespace Catalog.API.Features.FurniturePiece;

public class FurniturePieceDto
{
    public string FurnitureName { get; set; } = "";
    public byte[] FurnitureImage { get; set; } = new byte[0];
    public int FurnitureSetPieceId { get; set; }
    public int FurniturePriceGroupId { get; set; }
}
