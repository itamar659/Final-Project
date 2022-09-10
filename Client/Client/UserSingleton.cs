using IdentityModel.OidcClient;

namespace Client;

public class UserSingleton
{
    public string username { get; private set; }
    public string token { get; private set; }
    public int age { get; private set; }

    private Boolean isLoggedIn = false;

    public void fillUser(LoginResult result)
    {
        token = result.AccessToken;
        username = result.User.Identity.Name;

        Console.WriteLine(result.User);
    }


}

