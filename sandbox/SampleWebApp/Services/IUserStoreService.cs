using SampleWebApp.Dto;

namespace SampleWebApp.Services;

public interface IUserStoreService
{
    void Register(string name, UserRole role = UserRole.Default);

    UserDto[] GetAll();

    UserDto? GetById(int id);

    UserDto? UpdateById(int id, UserDto user);

    bool RemoveById(int id);
}
