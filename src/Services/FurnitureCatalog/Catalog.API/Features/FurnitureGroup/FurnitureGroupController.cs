using Catalog.API.Data;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Features.Furniture;


[ApiController]
[Route("[controller]")]
public class FurnitureGroupController : ControllerBase
{
    readonly DataContextDapper _dapper;

    public FurnitureGroupController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("GetAllGroups")]
    public IEnumerable<FurnitureGroupModel> GetAllGroups()
    {
        string sql = "SELECT FurnitureGroupId, FurnitureGroupPrice FROM furniture_group;";

        IEnumerable<FurnitureGroupModel> AllGroups = _dapper.LoadData<FurnitureGroupModel>(sql);

        return AllGroups;
    }

    [HttpPost("CreateGroup")]
    public IActionResult CreateGroup(FurnitureGroupDto groupToAdd)
    {
        string sql = $"INSERT INTO furniture_group (FurnitureGroupPrice) VALUES ({groupToAdd.FurnitureGroupPrice});";

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to create group.");
    }

    [HttpPut("EditGroup")]
    public IActionResult EditGroup(FurnitureGroupModel groupToEdit)
    {
        string sql = $"UPDATE furniture_group SET FurnitureGroupPrice = {groupToEdit.FurnitureGroupPrice} WHERE FurnitureGroupId = {groupToEdit.FurnitureGroupId};";

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to edit group.");
    }

    [HttpDelete("DeleteGroup/{id}")]
    public IActionResult DeleteGroup(int id)
    {
        string sql = $"DELETE FROM furniture_group WHERE FurnitureGroupId = {id};";

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to delete group.");
    }
}
