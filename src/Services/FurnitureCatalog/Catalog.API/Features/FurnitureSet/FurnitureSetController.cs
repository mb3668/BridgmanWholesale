using Catalog.API.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Catalog.API.Features.FurnitureSet;

[ApiController]
[Route("[controller]")]
public class FurnitureSetController : ControllerBase
{
    readonly DataContextDapper _dapper;

    public FurnitureSetController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("GetAllSets")]
    public IEnumerable<FurnitureSetModel> GetAllSets()
    {
        string sql = "SELECT SetPieceId, PieceName FROM furniture_set;";

        IEnumerable<FurnitureSetModel> AllSets = _dapper.LoadData<FurnitureSetModel>(sql);

        return AllSets;
    }

    [HttpPost("CreateSet")]
    public IActionResult CreateSet(FurnitureSetDto setToAdd)
    {
        string sql = $"INSERT INTO furniture_set (PieceName) VALUES ('{setToAdd.PieceName}');";

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to add set.");
    }

    [HttpPut("EditSet")]
    public IActionResult EditSet(FurnitureSetModel setToEdit)
    {
        string sql = $"UPDATE furniture_set SET PieceName = '{ setToEdit.PieceName }' WHERE SetPieceId = { setToEdit.SetPieceId }; ";

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to edit set.");
    }

    [HttpDelete("DeleteSet/{id}")]
    public IActionResult DeleteSet(int id)
    {
        string sql = $"DELETE FROM furniture_set WHERE SetPieceId = {id};";

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to delete set.");
    }
}
