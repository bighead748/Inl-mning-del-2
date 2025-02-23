using dagnyr.api.Entities;
using dagnyr.api.Services;
using dagnyr.api.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dagnyr.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
        private readonly UserManager<User> _userManager;
        private readonly TokenService _tokenService;
        private readonly RoleManager<IdentityRole> _roleManager;
    public AccountsController(UserManager<User> userManager,RoleManager<IdentityRole> roleManager,TokenService tokenService)
    {
            _roleManager = roleManager;
            _tokenService = tokenService;
            _userManager = userManager;
            
    }


    [HttpPost("register")]
    public async Task<ActionResult> RegisterUser(RegisterUserViewModel model)
    {
        try
        {
            var user = await AddUser(model);
            await _userManager.AddToRoleAsync(user, "User");
            return StatusCode(201, "Användaren är skapad");
        }
        catch (Exception e)
        {
            return BadRequest(new {sucess = false, message = e.Message});
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult> LoginUser(LoginViewModel model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName);
        if(user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
        {
            return Unauthorized(new {sucess=false, message="Fel Information"});
        }
        return Ok(new {sucess = true, email = user.Email, message="Du är inloggad", token = await _tokenService.CreateToken(user)});
    }

    [HttpPost("changepassword")]
    public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName);
        if(user is null)
        {
            return BadRequest(new {sucess = false});
        }
        await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

        return StatusCode(201, "Lösenordet är ändrat");
    }


    [HttpPost("registerwithRole")]
    //[Authorize(Roles = "Admin")]
    public async Task<ActionResult> RegisterUserWithRole(RegisterUserWithRoleViewModel model)
    {
        try
        {
            var role = await _roleManager.FindByNameAsync(model.RoleName);

            if(role is null)
            {
                return BadRequest(new {sucess = false, message = "Rollen finns inte"});
            }
            
            var user = await AddUser(model);
            await _userManager.AddToRoleAsync(user, model.RoleName);
            return StatusCode(201, "Användaren är skapad");
        }
        catch (Exception e)
        {
            return BadRequest(new {sucess = false, message = e.Message});
        }

    }

    private async Task<User>AddUser(RegisterUserViewModel model)
    {
        model.UserName = model.Email;
        var  user  = new User
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if(result.Succeeded)
        {
            return user;
        }
        throw new Exception($"Det gick inte att skapa användaren, {result.Errors}");
    }



}
