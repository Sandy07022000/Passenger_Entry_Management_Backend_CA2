using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Require valid token for all actions
public class PassengersController : ControllerBase
{
    private readonly MyDbContext _context;

    public PassengersController(MyDbContext context)
    {
        _context = context;
    }

    //(Admin + User) can read
    [HttpGet]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _context.Passengers.ToListAsync());
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> Get(int id)
    {
        var passenger = await _context.Passengers.FindAsync(id);
        if (passenger == null)
            return NotFound();
        return Ok(passenger);
    }

    //Admin only: Create
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] Passenger passenger)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.Passengers.Add(passenger);
        await _context.SaveChangesAsync();

        Log.Information("Passenger created: {PassengerId}, FullName: {FullName}", passenger.PassengerId, passenger.FullName);
        return Ok(passenger);
    }

    // Admin only: Update
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] Passenger passenger)
    {
        passenger.PassengerId = id;
        _context.Passengers.Update(passenger);
        await _context.SaveChangesAsync();

        Log.Information("Passenger updated: {PassengerId}, FullName: {FullName}", passenger.PassengerId, passenger.FullName);
        return Ok(passenger);
    }

    // Admin only: Delete
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var passenger = await _context.Passengers.FindAsync(id);
        if (passenger == null)
            return NotFound();

        _context.Passengers.Remove(passenger);
        await _context.SaveChangesAsync();
        Log.Information("Passenger deleted: {PassengerId}, FullName: {FullName}", passenger.PassengerId, passenger.FullName);
        return Ok();
    }
}
