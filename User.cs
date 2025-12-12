using System.ComponentModel.DataAnnotations;

public class User
{
    public int Id { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    [Required]
    public string Role { get; set; } // "Admin" or "User"

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
