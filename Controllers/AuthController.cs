using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly MyDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(MyDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        if (await _context.Users.AnyAsync(u => u.Username == user.Username))
        {
            Log.Warning("Registration attempt failed: Username {Username} already exists", user.Username);
            return BadRequest("Username already exists.");
        }

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
        user.PasswordHash = hashedPassword;
        user.Role = string.IsNullOrEmpty(user.Role) ? "User" : user.Role;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        Log.Information("New user registered: {Username}, Role: {Role}", user.Username, user.Role);
        return Ok("User registered successfully.");
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest login)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == login.Username);
        if (user == null)
        {
            Log.Warning("Login failed: Invalid username {Username}", login.Username);
            return BadRequest("Invalid username or password.");
        }

        bool valid = BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash);
        if (!valid)
        {
            Log.Warning("Login failed: Incorrect password for username {Username}", login.Username);
            return BadRequest("Invalid username or password.");
        }

        var token = GenerateJwtToken(user);
        Log.Information("User logged in successfully: {Username}", login.Username);

        return Ok(new { token });
    }

    [HttpPost("reauth")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ReAuthenticate([FromBody] LoginRequest request)
    {
        var username = User.Identity?.Name;
        if (string.IsNullOrEmpty(username))
            return Unauthorized();

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.Role == "Admin");
        if (user == null)
            return Unauthorized();

        bool valid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!valid)
            return Unauthorized();

        return Ok(true);
    }


    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        // --- Cryptographic Hardening ---
        var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY")
                     ?? throw new InvalidOperationException("JWT Key missing!");
        var keyBytes = Encoding.UTF8.GetBytes(jwtKey);
        if (keyBytes.Length < 32)
            throw new InvalidOperationException("JWT key must be at least 256 bits (32 bytes).");

        var key = new SymmetricSecurityKey(keyBytes);
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Secure token expiration: 20 minutes
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(20),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
