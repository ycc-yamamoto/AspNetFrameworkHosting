namespace SampleWebApp.Dto;

public class UserDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public UserRole Role { get; set; }
}
