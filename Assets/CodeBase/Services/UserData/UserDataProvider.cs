namespace CodeBase.Services.UserData
{
    public class UserDataProvider : IUserDataProvider
    {
        public UserData UserData { get; }

        public UserDataProvider() => 
            UserData = new UserData();
    }
}