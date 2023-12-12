using SampleWebApp.Dto;

namespace SampleWebApp.Services;

public interface IUserStoreService
{
    void Register(string name);

    UserDto[] GetAll();

    UserDto? GetById(int id);
}
