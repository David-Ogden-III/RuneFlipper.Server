using DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DataTransferObjects;
using Models.Entities;

namespace Controllers;

[Route("[controller]")]
[ApiController]
[Authorize(Roles = "Owner")]
public class RoleController : ControllerBase
{
    private readonly RuneFlipperContext _context;
    private readonly UnitOfWork _unitOfWork;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<User> _userManager;

    public RoleController(RuneFlipperContext context, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
    {
        _context = context;
        _unitOfWork = new(_context);
        _roleManager = roleManager;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult<RoleResponse>> Get()
    {
        var roles = await _unitOfWork.RoleRepository.GetListAsync();
        var response = ObjectMapper.CreateFetchRoleResponses(roles);

        return Ok(response);
    }

    [HttpPost("CreateRole")]
    public async Task<ActionResult<IdentityRole>> Create([FromBody] NewRole newRole)
    {
        try
        {
            var roleName = newRole.Name;

            if (roleName == null) return BadRequest("Role Name is Invalid");

            IdentityRole roleToAdd = new(roleName)
            {
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            bool success = (await _roleManager.CreateAsync(roleToAdd)).Succeeded;

            if (success)
            {
                RoleResponse response = ObjectMapper.CreateFetchRoleResponse(roleToAdd);
                return CreatedAtAction(nameof(Create), response);
            }

            return BadRequest();
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
            IdentityRole roleToDelete = await _unitOfWork.RoleRepository.GetAsync(filters: [role => role.Id == roleId]);
            bool success = (await _roleManager.DeleteAsync(roleToDelete)).Succeeded;


            if (success)
            {
                RoleResponse response = ObjectMapper.CreateFetchRoleResponse(roleToDelete);
                return Ok(response);
            }

            return BadRequest();
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

            var user = await _userManager.FindByIdAsync(updateUserRole.UserId);
            if (user == null) return NotFound("User Not Found");

            List<string> roles = [];
            foreach (string roleName in updateUserRole.RoleNames)
            {
                var currentRole = await _roleManager.FindByNameAsync(roleName);

                if (currentRole != null && currentRole.Name != null)
                {
                    roles.Add(currentRole.Name);
                }
            }

            if (roles.Count <= 0) return BadRequest();

            bool success = (await _userManager.AddToRolesAsync(user, roles)).Succeeded;

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

            var user = await _userManager.FindByIdAsync(updateUserRole.UserId);
            if (user == null) return NotFound("User Not Found");

            List<string> roles = [];
            foreach (string roleName in updateUserRole.RoleNames)
            {
                var currentRole = await _roleManager.FindByNameAsync(roleName);

                if (currentRole != null && currentRole.Name != null)
                {
                    roles.Add(currentRole.Name);
                }
            }

            if (roles.Count <= 0) return BadRequest("Supplied role names did not match any existing roles");

            bool success = (await _userManager.RemoveFromRolesAsync(user, roles)).Succeeded;

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