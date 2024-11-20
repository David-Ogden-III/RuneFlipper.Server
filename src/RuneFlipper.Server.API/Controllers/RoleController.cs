using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RuneFlipper.Server.Application;
using RuneFlipper.Server.Application.Roles.TransferObjects;
using RuneFlipper.Server.Domain.Entities;
using RuneFlipper.Server.Infrastructure.Persistence;

namespace RuneFlipper.Server.API.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize(Roles = "Owner")]
public class RoleController(RuneFlipperContext context, RoleManager<IdentityRole> roleManager, UserManager<User> userManager) : ControllerBase
{
    private readonly UnitOfWork _unitOfWork = new(context);

    [HttpGet]
    public async Task<ActionResult<RoleResponse>> Get()
    {
        var roles = await _unitOfWork.RoleRepository.GetListAsync();
        var response = ObjectMapper.CreateRoleResponses(roles);

        return Ok(response);
    }

    [HttpPost("CreateRole")]
    public async Task<ActionResult<IdentityRole>> Create([FromBody] NewRole newRole)
    {
        try
        {
            string roleName = newRole.Name;

            IdentityRole roleToAdd = new(roleName)
            {
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            bool success = (await roleManager.CreateAsync(roleToAdd)).Succeeded;

            if (!success) return BadRequest();

            RoleResponse response = new(roleToAdd.Id, roleToAdd.Name ?? string.Empty);
            return CreatedAtAction(nameof(Create), response);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return BadRequest("Unable to save changes. Try again.");
        }
    }

    [HttpDelete("{roleId}")]
    public async Task<ActionResult<RoleResponse>> Delete(string roleId)
    {
        try
        {
            var roleToDelete = await _unitOfWork.RoleRepository.GetAsync(filters: [role => role.Id == roleId]);
            if (roleToDelete == null) return BadRequest();

            bool success = (await roleManager.DeleteAsync(roleToDelete)).Succeeded;


            if (!success) return BadRequest();

            RoleResponse response = new(roleToDelete.Id, roleToDelete.Name ?? string.Empty);
            return Ok(response);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return BadRequest("Unable to save changes. Try again.");
        }
    }

    [HttpPost("AddRoleToUser/{userId}")]
    public async Task<ActionResult<IdentityRole>> AddRoleToUser([FromBody] UpdateUserRole updateUserRole, string userId)
    {
        try
        {
            if (updateUserRole.UserId != userId) return BadRequest("Supplied User Id's do not match");

            var user = await userManager.FindByIdAsync(updateUserRole.UserId);
            if (user == null) return NotFound("User Not Found");

            List<string> roles = [];
            foreach (string roleName in updateUserRole.RoleNames)
            {
                var currentRole = await roleManager.FindByNameAsync(roleName);

                if (currentRole is { Name: not null })
                {
                    roles.Add(currentRole.Name);
                }
            }

            if (roles.Count <= 0) return BadRequest();

            bool success = (await userManager.AddToRolesAsync(user, roles)).Succeeded;

            if (success)
            {
                return Ok();
            }

            return BadRequest();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return BadRequest("Unable to save changes. Try again.");
        }
    }

    [HttpPost("RemoveUserRole/{userId}")]
    public async Task<ActionResult<IdentityRole>> RemoveUserRole([FromBody] UpdateUserRole updateUserRole, string userId)
    {
        try
        {
            if (updateUserRole.UserId != userId) return BadRequest("Supplied User Id's do not match");

            var user = await userManager.FindByIdAsync(updateUserRole.UserId);
            if (user == null) return NotFound("User Not Found");

            List<string> roles = [];
            foreach (string roleName in updateUserRole.RoleNames)
            {
                var currentRole = await roleManager.FindByNameAsync(roleName);

                if (currentRole is { Name: not null })
                {
                    roles.Add(currentRole.Name);
                }
            }

            if (roles.Count <= 0) return BadRequest("Supplied role names did not match any existing roles");

            bool success = (await userManager.RemoveFromRolesAsync(user, roles)).Succeeded;

            if (success)
            {
                return Ok();
            }

            return BadRequest();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return BadRequest("Unable to save changes. Try again.");
        }
    }
}