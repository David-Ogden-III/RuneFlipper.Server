using DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;

namespace Controllers;

[Route("[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
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
    public async Task<ActionResult<IdentityRole>> Get()
    {
        var roles = await _unitOfWork.RoleRepository.Get();

        return Ok(roles);
    }

    [HttpPost("CreateRole")]
    public async Task<ActionResult<IdentityRole>> Create([FromBody] string roleName)
    {
        try
        {
            IdentityRole roleToAdd = new(roleName)
            {
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            bool success = (await _roleManager.CreateAsync(roleToAdd)).Succeeded;

            if (success)
            {
                return CreatedAtAction(nameof(Get), new { roleToAdd.Id }, roleToAdd);
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
    public async Task<ActionResult<IdentityRole>> AddRoleToUser([FromBody] IEnumerable<string> roleNames, string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound("User Not Found");

            List<string> roles = [];
            foreach (string roleName in roleNames)
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
}
