using BusinessLayer.Interfaces;
using BusinessLayer.Models;

namespace BusinessLayer.Services;

public class RegisterService(IRegisterRepository repo, IAuthorizationService authService) : IRegisterService
{
    //Best practise for password length
    private const int MIN_PASSWORD_LENGTH = 8;
    private readonly EncryptionService encryptionService = new();

    public UserData Register(UserData request)
    {
        if (request.Password.Length < MIN_PASSWORD_LENGTH) return new();

        UserData user = request.Copy();
        user.Email = user.Email.ToLower();
        user.Password = encryptionService.EncryptPassword(user.Password, out byte[] salt);
        user.Salt = salt;
        if (!repo.Register(user)) return new();
        user.Token = authService.GenerateToken(user);
        return user;
    }

    public bool IsEmailExisting(UserData request)
    {
        request.Email = request.Email.ToLower();
        return repo.IsEmailExisting(request);
    }
}
