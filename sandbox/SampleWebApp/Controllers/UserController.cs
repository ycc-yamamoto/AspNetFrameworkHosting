using System.Collections.Generic;
using System.Web.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SampleWebApp.Dto;
using SampleWebApp.Extensions;
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
        this.logger.ReceivedHttpRequest("GET", "/api/users");
        return this.userStoreService.GetAll();
    }

    [HttpGet]
    [Route("{id}")]
    public IHttpActionResult Get(int id)
    {
        this.logger.ReceivedHttpRequest("GET", $"/api/users/{id}");
        var user = this.userStoreService.GetById(id);

        return user is null ? this.NotFound() : this.Ok(user);
    }

    [HttpPost]
    [Route]
    public void Post([FromBody] UserDto user)
    {
        this.logger.ReceivedHttpRequest("POST", $"/api/users: {user?.Name}");

        if (user is null)
        {
            return;
        }

        this.userStoreService.Register(user.Name);
    }

    [HttpPut]
    [Route("{id}")]
    public IHttpActionResult Put(int id, [FromBody] UserDto user)
    {
        this.logger.ReceivedHttpRequest("PUT", $"/api/users/{id}: {JsonConvert.SerializeObject(user)}");

        var updatedUser = this.userStoreService.UpdateById(id, user);

        return updatedUser is null ? this.NotFound() : this.Ok(updatedUser);
    }

    [HttpDelete]
    [Route("{id}")]
    public IHttpActionResult Delete(int id)
    {
        this.logger.ReceivedHttpRequest( "DELETE", $"/api/users/{id}");

        return this.userStoreService.RemoveById(id) ? this.Ok() : this.NotFound();
    }
}
