using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace IrelandEntryApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PassengerController : ControllerBase
    {
        private readonly string connString =
            "Server=localhost;Database=irelandentrydb;User ID=root;Password=admin;";

        // GET all passengers
        [HttpGet]
        public IEnumerable<Passenger> GetPassengers()
        {
            var list = new List<Passenger>();

            using var conn = new MySqlConnection(connString);
            conn.Open();

            string sql = "SELECT * FROM passengers";

            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Passenger
                {
                    PassengerId = reader.GetInt32("PassengerId"),
                    FullName = reader.GetString("FullName"),
                    PassportNumber = reader.GetString("PassportNumber"),
                    VisaType = reader.GetString("VisaType"),
                    Nationality = reader.GetString("Nationality"),
                    ArrivalDate = DateOnly.FromDateTime(reader.GetDateTime("ArrivalDate")),
                    ArrivalYear = reader.GetInt32("ArrivalYear"),
                    PurposeOfVisit = reader.GetString("PurposeOfVisit"),
                    OfficerId = reader.GetInt32("OfficerId")
                });
            }

            return list;
        }

        // POST - Insert a new passenger
        [HttpPost]
        public IActionResult Insert(Passenger p)
        {
            using var conn = new MySqlConnection(connString);
            conn.Open();

            string sql = @"INSERT INTO passengers 
                            (FullName, PassportNumber, VisaType, Nationality, ArrivalDate, ArrivalYear, PurposeOfVisit, OfficerId) 
                            VALUES 
                            (@name, @passport, @visa, @nation, @date, @year, @purpose, @officer)";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@name", p.FullName);
            cmd.Parameters.AddWithValue("@passport", p.PassportNumber);
            cmd.Parameters.AddWithValue("@visa", p.VisaType);
            cmd.Parameters.AddWithValue("@nation", p.Nationality);
            cmd.Parameters.AddWithValue("@date", p.ArrivalDate.ToDateTime(TimeOnly.MinValue));
            cmd.Parameters.AddWithValue("@year", p.ArrivalYear);
            cmd.Parameters.AddWithValue("@purpose", p.PurposeOfVisit);
            cmd.Parameters.AddWithValue("@officer", p.OfficerId);

            cmd.ExecuteNonQuery();

            return Ok("Passenger inserted successfully!");
        }

        // PUT - Update passenger
        [HttpPut("{id}")]
        public IActionResult Update(int id, Passenger p)
        {
            using var conn = new MySqlConnection(connString);
            conn.Open();

            string sql = @"UPDATE passengers SET 
                            FullName=@name, 
                            PassportNumber=@passport, 
                            VisaType=@visa, 
                            Nationality=@nation, 
                            ArrivalDate=@date, 
                            ArrivalYear=@year, 
                            PurposeOfVisit=@purpose,
                            OfficerId=@officer
                           WHERE PassengerId=@id";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@name", p.FullName);
            cmd.Parameters.AddWithValue("@passport", p.PassportNumber);
            cmd.Parameters.AddWithValue("@visa", p.VisaType);
            cmd.Parameters.AddWithValue("@nation", p.Nationality);
            cmd.Parameters.AddWithValue("@date", p.ArrivalDate.ToDateTime(TimeOnly.MinValue));
            cmd.Parameters.AddWithValue("@year", p.ArrivalYear);
            cmd.Parameters.AddWithValue("@purpose", p.PurposeOfVisit);
            cmd.Parameters.AddWithValue("@officer", p.OfficerId);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            return Ok("Passenger updated successfully!");
        }
    }
}
