using BusinessLayer.Interfaces;
using BusinessLayer.Models;

namespace BusinessLayer.Services;

public class LoginService(ILoginRepository data, IAuthorizationService authService) : ILoginService
{
    private readonly EncryptionService encrypt = new();

    public UserData Login(UserData request)
    {
        request.Email  = request.Email.ToLower();
        UserData user = data.GetUser(request);
        if (IsNotValidUser(user)) return new();
        if (!encrypt.IsMatching(user.Password, request.Password, user.Salt)) return new();
        UserData response = user.Copy();
        response.Token = authService.GenerateToken(response);
        return response;
    }

    private bool IsNotValidUser(UserData response)
    {
        return string.IsNullOrEmpty(response.Email)
               & string.IsNullOrEmpty(response.Password);
    }
}
