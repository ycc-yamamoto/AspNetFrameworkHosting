using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.CompilerServices;
using SampleWebApp.Dto;

namespace SampleWebApp.Services.Impl;

public sealed class UserStoreService : IUserStoreService
{
    private readonly ConcurrentDictionary<int, UserDto> users;

    public UserStoreService()
    {
        this.users = new ConcurrentDictionary<int, UserDto>();
        this.users.TryAdd(1, new UserDto
        {
            Id = 1,
            Name = "佐藤太郎",
            Role = UserRole.Default,
        });
        this.users.TryAdd(2, new UserDto
        {
            Id = 2,
            Name = "鈴木花子",
            Role = UserRole.Administrator,
        });
    }

    public void Register(string name, UserRole role = UserRole.Default)
    {
        var max = this.users.Max(user => user.Key);
        var id = max + 1;
        var newUser = new UserDto
        {
            Id = id,
            Name = name,
            Role = role,
        };

        this.users.TryAdd(id, newUser);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public UserDto[] GetAll()
    {
        return this.users.Values.ToArray();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public UserDto? GetById(int id)
    {
        return this.users.TryGetValue(id, out var user) ? user : null;
    }

    public UserDto? UpdateById(int id, UserDto user)
    {
        if (this.users.TryGetValue(id, out var oldUser))
        {
            this.users[id] = user;
            return user;
        }

        return null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool RemoveById(int id)
    {
        return this.users.TryRemove(id, out _);
    }
}
