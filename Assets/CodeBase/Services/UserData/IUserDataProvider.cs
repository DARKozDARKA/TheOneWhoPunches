using CodeBase.Infrastructure;

namespace CodeBase.Services.UserData
{
    public interface IUserDataProvider : IService
    {
        UserData UserData { get; }
    }
}