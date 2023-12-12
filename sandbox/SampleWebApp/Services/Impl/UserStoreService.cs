using System.Collections.Concurrent;
using System.Linq;
using SampleWebApp.Dto;

namespace SampleWebApp.Services.Impl;

public sealed class UserStoreService : IUserStoreService
{
    private readonly ConcurrentBag<UserDto> users;

    public UserStoreService()
    {
        this.users = new ConcurrentBag<UserDto>();
        this.users.Add(new UserDto
        {
            Id = 1,
            Name = "佐藤太郎",
        });
        this.users.Add(new UserDto
        {
            Id = 2,
            Name = "鈴木花子",
        });
    }

    public void Register(string name)
    {
        var max = this.users.Max(user => user.Id);
        var newUser = new UserDto
        {
            Id = max + 1,
            Name = name,
        };

        this.users.Add(newUser);
    }

    public UserDto[] GetAll()
    {
        return this.users.ToArray();
    }

    public UserDto? GetById(int id)
    {
        return this.users.FirstOrDefault(user => user.Id == id);
    }
}
