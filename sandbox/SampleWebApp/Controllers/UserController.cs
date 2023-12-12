using System.Collections.Generic;
using System.Web.Http;
using SampleWebApp.Dto;
using SampleWebApp.Services;

namespace SampleWebApp.Controllers;

[RoutePrefix("api/users")]
public class UserController : ApiController
{
    private readonly IUserStoreService userStoreService;

    public UserController(IUserStoreService userStoreService)
    {
        this.userStoreService = userStoreService;
    }

    [HttpGet]
    [Route]
    public IEnumerable<UserDto> Get()
    {
        return this.userStoreService.GetAll();
    }

    [HttpGet]
    [Route("{id}")]
    public IHttpActionResult Get(int id)
    {
        var user = this.userStoreService.GetById(id);

        return user is null ? this.NotFound() : this.Ok(user);
    }

    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    [HttpPut]
    public void Put(int id, [FromBody] string value)
    {
    }

    [HttpDelete]
    public void Delete(int id)
    {
    }
}
