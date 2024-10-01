namespace api.Dto;

public class LoginModel
{
    public required string username { get; set; }
    public required string password { get; set; }
}

public class ResponseLoginModel
{
    public required string token { get; set; }
    public required bool firstLogin { get; set; }
}

public class RegisterModel
{
    public required string email { get; set; }
    public required string username { get; set; }
}

public class TokenModel
{
    public required string token { get; set; }
}

public class ResetPasswordModel
{
    public required string username { get; set; }
}

public class ChangePasswordModel
{
    public required string password { get; set; }
}