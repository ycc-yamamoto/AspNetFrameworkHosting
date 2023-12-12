using System;

namespace SampleWebApp.Dto;

public class DepartmentDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public UserDto[] Members { get; set; } = Array.Empty<UserDto>();
}
