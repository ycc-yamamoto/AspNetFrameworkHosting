using System.Collections.Generic;
using System.Web.Http;
using Microsoft.Extensions.Logging;
using SampleWebApp.Dto;
using SampleWebApp.Services;

namespace SampleWebApp.Controllers;

[RoutePrefix("api/users")]
public class UserController : ApiController
{
    private readonly ILogger<UserController> logger;

    private readonly IUserStoreService userStoreService;

    public UserController(ILogger<UserController> logger, IUserStoreService userStoreService)
    {
        this.logger = logger;
        this.userStoreService = userStoreService;
    }

    [HttpGet]
    [Route]
    public IEnumerable<UserDto> Get()
    {
        this.logger.LogInformation("GET /api/users");
        return this.userStoreService.GetAll();
    }

    [HttpGet]
    [Route("{id}")]
    public IHttpActionResult Get(int id)
    {
        this.logger.LogInformation($"GET /api/users/{id}");
        var user = this.userStoreService.GetById(id);

        return user is null ? this.NotFound() : this.Ok(user);
    }

    [HttpPost]
    [Route]
    public void Post([FromBody] UserDto user)
    {
        this.logger.LogInformation($"POST /api/users: {user.Name}");
        this.userStoreService.Register(user.Name);
    }

    [HttpPut]
    [Route("{id}")]
    public void Put(int id, [FromBody] string value)
    {
        this.logger.LogInformation($"PUT /api/users/{id}: {value}");
    }

    [HttpDelete]
    [Route("{id}")]
    public void Delete(int id)
    {
        this.logger.LogInformation($"DELETE /api/users/{id}");
    }
}
