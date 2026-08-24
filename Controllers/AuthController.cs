using Microsoft.AspNetCore.Mvc;
using auth.Models;
using auth.Data;
using auth.DTOs;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
namespace auth.Controllers;


[ApiController]
[Route("auth")]
public class AuthUser:ControllerBase
{
    private readonly IHttpClientFactory _IClient;
    private readonly AppDbContext _context;

    public AuthUser(IHttpClientFactory iclientFactory,AppDbContext context)
    {
        _IClient = iclientFactory;
        _context = context;
    }  

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUser dto)
    {
        if(await _context.Users.AnyAsync(u=>u.UserName.ToLower() == dto.Name.ToLower()))
        {
            return BadRequest("User Name already in use");
        }
        var user = new User
        {
            UserName=dto.Name,
            HashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password),
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("user Registered");
    }

    [HttpPost("log")]
    public async Task<IActionResult> LogIn(LoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u=> u.UserName.ToLower() == dto.Name.ToLower());

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.HashedPassword))
        {
            return BadRequest("invalid user name or password");
        }
        return Ok("user logged");
    }


    
}