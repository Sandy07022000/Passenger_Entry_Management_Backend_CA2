
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class PassengersController : ControllerBase
{
    private readonly MyDbContext _context;

    public PassengersController(MyDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _context.Passengers.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var passenger = await _context.Passengers.FindAsync(id);
        return Ok(passenger); // Vulnerability: verbose errors if null (#7)
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Passenger passenger)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Prevent overposting
        var newPassenger = new Passenger
        {
            FullName = passenger.FullName?.Trim(),
            PassportNumber = passenger.PassportNumber?.Trim(),
            VisaType = passenger.VisaType?.Trim(),
            Nationality = passenger.Nationality?.Trim(),
            ArrivalDate = passenger.ArrivalDate, // DateOnly type
            ArrivalYear = passenger.ArrivalYear,
            PurposeOfVisit = passenger.PurposeOfVisit?.Trim(),
            OfficerId = passenger.OfficerId
        };

        _context.Passengers.Add(newPassenger);
        await _context.SaveChangesAsync();

        return Ok(newPassenger);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Passenger passenger)
    {
        passenger.PassengerId = id; // Overwriting ID / sensitive fields (#5)
        _context.Passengers.Update(passenger);
        await _context.SaveChangesAsync();
        return Ok(passenger);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var passenger = await _context.Passengers.FindAsync(id);
        _context.Passengers.Remove(passenger);
        await _context.SaveChangesAsync();
        return Ok();
    }
}
