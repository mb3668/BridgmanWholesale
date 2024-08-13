using Catalog.API.Data;
using Catalog.API.Features.Furniture;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace Catalog.API.Features.FurniturePiece;

[ApiController]
[Route("[controller]")]

public class FurniturePieceController : ControllerBase
{
    readonly DataContextDapper _dapper;

    public FurniturePieceController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("GetAllPieces")]
    public IEnumerable<FurniturePieceModel> GetAllPieces()
    {
        string sql = "SELECT FurnitureId, FurnitureName, FurnitureImage, FurnitureSetPieceId, FurniturePriceGroupId5 FROM furniture_piece;";

        IEnumerable<FurniturePieceModel> AllPieces = _dapper.LoadData<FurniturePieceModel>(sql);

        return AllPieces;
    }

    [HttpPost("CreatePiece")]
    public IActionResult CreatePiece(FurniturePieceDto pieceToAdd)
    {
        string sql = $"INSERT INTO furniture_piece (FurnitureName, FurnitureSetPieceId, FurniturePriceGroupId) VALUES ('{pieceToAdd.FurnitureName}', {pieceToAdd.FurnitureSetPieceId}, {pieceToAdd.FurniturePriceGroupId});";

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to create piece.");
    }

    [HttpPut("EditPiece")]
    public IActionResult EditPiece(FurniturePieceModel pieceToEdit)
    {
        string sql = $"UPDATE furniture_piece SET FurnitureName = '{pieceToEdit.FurnitureName}', FurnitureSetPieceId = {pieceToEdit.FurnitureSetPieceId}, FurniturePriceGroupId = {pieceToEdit.FurniturePriceGroupId} WHERE FurnitureId = {pieceToEdit.FurnitureId};";

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to edit piece.");
    }

    [HttpPut("EditImage/{furnitureId}")]
    public IActionResult EditImage(int furnitureId, [FromBody] byte[] img)
    {
        if (img == null || img.Length == 0)
        {
            return BadRequest("No image provided");
        }

        var parameters = new Dictionary<string, object>
        {
            { "Image", img },
            { "FurnitureId", furnitureId }
        };

        string sql = $"UPDATE furniture_piece SET FurnitureImage = @Image WHERE FurnitureId = @furnitureId;";

        if (_dapper.ExecuteSqlWithParameters(sql, parameters))
        {
            return Ok();
        }
        throw new Exception("Failed to add image.");
    }

    [HttpDelete("DeletePiece/{id}")]
    public IActionResult DeletePiece(int id)
    {
        string sql = $"DELETE FROM furniture_piece WHERE FurnitureId = {id};";

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to delete piece.");
    }
}
