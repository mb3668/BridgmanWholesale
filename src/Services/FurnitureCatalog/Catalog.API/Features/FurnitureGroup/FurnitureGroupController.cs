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
        string sql = "SELECT FurnitureGroupId, FurnitureGroupPrice, FurnitureGroupIsDelivered FROM furniture_group;";

        IEnumerable<FurnitureGroupModel> AllGroups = _dapper.LoadData<FurnitureGroupModel>(sql);

        return AllGroups;
    }

    [HttpPost("CreateGroup")]
    public IActionResult CreateGroup(FurnitureGroupDto groupToAdd)
    {
        string sql = $"INSERT INTO furniture_group (FurnitureGroupPrice, FurnitureGroupIsDelivered) VALUES ({groupToAdd.FurnitureGroupPrice}, {groupToAdd.FurnitureGroupIsDelivered});";

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to create group.");
    }

    [HttpPut("EditGroup")]
    public IActionResult EditGroup(FurnitureGroupModel groupToEdit)
    {
        string sql = $"UPDATE furniture_group SET FurnitureGroupPrice = {groupToEdit.FurnitureGroupPrice}, FurnitureGroupIsDelivered = {groupToEdit.FurnitureGroupIsDelivered} WHERE FurnitureGroupId = {groupToEdit.FurnitureGroupId};";

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to edit group.");
    }

    [HttpPut("EditImage/{groupId}")]
    public IActionResult EditImage(int groupId, [FromBody] byte[] img)
    {
        if (img == null || img.Length == 0)
        {
            return BadRequest("No image provided");
        }

        var parameters = new Dictionary<string, object>
        {
            { "Image", img },
            { "GroupId", groupId }
        };

        string sql = $"UPDATE furniture_group SET FurnitureImage = @Image WHERE FurnitureGroupId = @GroupId;";

        if (_dapper.ExecuteSqlWithParameters(sql, parameters))
        {
            return Ok();
        }
        throw new Exception("Failed to add image.");
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
