using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using SampleWebApp.Dto;

namespace SampleWebApp.Controllers;

[RoutePrefix("api/company/departments")]
public class DepartmentController : ApiController
{
    private readonly DepartmentDto[] departments = new[]
    {
        new DepartmentDto
        {
            Id = 1,
            Name = "営業部",
            Members = new[] { new UserDto { Id = 1, Name = "佐藤太郎", }, new UserDto { Id = 2, Name = "鈴木花子", } },
        },
        new DepartmentDto
        {
            Id = 2,
            Name = "開発部",
            Members = new[] { new UserDto { Id = 3, Name = "山田次郎", }, new UserDto { Id = 4, Name = "田中三郎", } },
        },
    };

    [HttpGet]
    [Route]
    public IEnumerable<DepartmentDto> Get()
    {
        return this.departments;
    }

    [HttpGet]
    [Route("{id}")]
    public IHttpActionResult Get(int id)
    {
        var department = this.departments.FirstOrDefault(department => department.Id == id);

        return department is null ? this.NotFound() : this.Ok(department);
    }
}
