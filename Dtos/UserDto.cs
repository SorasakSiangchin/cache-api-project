namespace CacheProject.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public bool IsCache { get; set; } = false;   
    public bool IsDb { get; set; }= false;  
}