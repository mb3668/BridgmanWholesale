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
        string sql = "SELECT FurnitureId, FurnitureName, FurnitureSetPieceId, FurniturePriceGroupId FROM furniture_piece;";

        IEnumerable<FurniturePieceModel> AllPieces = _dapper.LoadData<FurniturePieceModel>(sql);

        return AllPieces;
    }

    [HttpGet("GetPieceByType/{id}")]
    public IEnumerable<FurniturePieceModel> GetPieceByType(int id)
    {
        string sql = "SELECT FurnitureId, FurnitureName, FurnitureSetPieceId, FurniturePriceGroupId FROM furniture_piece WHERE FurniturePriceGroupId = " + id.ToString() + ";";

        IEnumerable<FurniturePieceModel> PieceByType = _dapper.LoadData<FurniturePieceModel>(sql);

        return PieceByType;
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
