using Microsoft.AspNetCore.Mvc;
using UserService.Controllers;
using UserService.DTO;

namespace TestSuite.Controllers;

[TestClass]
public class RegisterControllerTest
{
    private const int HTTP_OK = 200;
    private const int HTTP_UNAUTHORIZED = 401;
    private RegisterController controller = null!;

    [TestInitialize]
    public void BeforeEachTest()
    {
        EncryptionService encrypt = new();
        UserData user = new()
        {
            Email = "test@email.com",
            Password = encrypt.EncryptPassword("password", out byte[] salt),
            Salt = salt
        };
        Mock<IRegisterRepository> repository = new();
        repository.Setup(p => p.IsEmailExisting(It.IsAny<UserData>())).Returns(false);
        repository.Setup(p => p.IsEmailExisting(It.Is<UserData>(u => u.Email.Equals("existing@email.com")))).Returns(true);
        repository.Setup(p => p.Register(It.Is<UserData>(u => u.Email.Equals("existing@email.com")))).Returns(false);
        repository.Setup(p => p.Register(It.IsAny<UserData>())).Returns(true);
        Mock<IAuthorizationService> auth = new();
        auth.Setup(p => p.GenerateToken(It.IsAny<UserData>())).Returns("Token");
        
        IRegisterService service = new RegisterService(repository.Object, auth.Object);
        controller = new(service);
    }

    [TestMethod]
    public void Register_NewEmail_ShouldReturnOk()
    {
        UserRequest request = new()
        {
            Email = "test@gmail.com",
            Password = "Password123!"
        };

        OkObjectResult response = (OkObjectResult)controller.Register(request);
        Assert.IsNotNull(response);
        Assert.AreEqual(HTTP_OK, response.StatusCode);
    }
    
    [TestMethod]
    public void Register_ExistingEmail_ShouldReturnUnauthorized()
    {
        UserRequest request = new()
        {
            Email = "existing@email.com",
            Password = "password"
        };

        UnauthorizedObjectResult response = (UnauthorizedObjectResult)controller.Register(request);
        Assert.IsNotNull(response);
        Assert.AreEqual(HTTP_UNAUTHORIZED, response.StatusCode);
    }
}
