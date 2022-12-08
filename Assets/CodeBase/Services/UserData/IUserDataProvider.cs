using CodeBase.Infrastructure.ServiceLocator;

namespace CodeBase.Services.UserData
{
    public interface IUserDataProvider : IService
    {
        UserData UserData { get; }
    }
}