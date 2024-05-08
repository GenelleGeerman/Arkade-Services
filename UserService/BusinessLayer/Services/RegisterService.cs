using BusinessLayer.Interfaces;
using BusinessLayer.Models;

namespace BusinessLayer.Services;

public class RegisterService(IRegisterRepository repo, ILoginService login) : IRegisterService
{
    //Best practise for password length
    private const int MIN_PASSWORD_LENGTH = 8;

    public UserData Register(UserData request)
    {
        if (request.Password.Length < MIN_PASSWORD_LENGTH) return new();
        string encryptedPass = EncryptionService.EncryptPassword(request.Password, out byte[] salt);
        
        UserData user = request.Copy();
        user.Email = user.Email.ToLower();
        user.Password = encryptedPass;
        user.Salt = salt;
        return repo.Register(user) ? login.Login(request) : new();
    }

    public bool IsEmailExisting(UserData request)
    {
        request.Email = request.Email.ToLower();
        return repo.IsEmailExisting(request);
    }
}
