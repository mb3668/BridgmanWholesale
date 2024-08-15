namespace Catalog.API.Features.FurniturePiece;

public class FurniturePieceModel
{
    public int FurnitureId { get; set; }
    public string FurnitureName { get; set; } = "";
    public int FurnitureSetPieceId { get; set; }
    public int FurniturePriceGroupId { get; set; }
}
