using Microsoft.AspNetCore.Mvc;
using UserService.Controllers;
using UserService.DTO;

namespace TestSuite.Controllers;

[TestClass]
public class LoginControllerTest
{
    private const int HTTP_OK = 200;
    private const int HTTP_UNAUTHORIZED = 401;
    private LoginController controller = null!;

    [TestInitialize]
    public void BeforeEachTest()
    {
        UserData user = new()
        {
            Email = "test@email.com",
            Password = EncryptionService.EncryptPassword("password", out byte[] salt),
            Salt = salt
        };
        Mock<ILoginRepository> repository = new();
        repository.Setup(p => p.GetUser(It.IsAny<UserData>())).ReturnsAsync(new UserData());
        repository.Setup(p => p.GetUser(It.Is<UserData>(u => u.Email.Equals("test@email.com")))).ReturnsAsync(user);
        Mock<IAuthorizationService> auth = new();
        auth.Setup(p => p.GenerateToken(It.IsAny<UserData>())).Returns("Token");

        ILoginService service = new LoginService(repository.Object, auth.Object);
        controller = new(service);
    }

    [TestMethod]
    public async void Login_IncorrectEmail_ShouldFail()
    {
        LoginRequest request = new()
        {
            Email = "bad@email.com",
            Password = "password"
        };

        UnauthorizedResult response = (UnauthorizedResult)await controller.Login(request);
        Assert.IsNotNull(response);
        Assert.AreEqual(HTTP_UNAUTHORIZED, response.StatusCode);
    }

    [TestMethod]
    public async void Login_UsingCorrectCredentials_ShouldPass()
    {
        LoginRequest request = new()
        {
            Email = "test@email.com",
            Password = "password"
        };

        OkObjectResult response = (OkObjectResult)await controller.Login(request);

        Assert.AreEqual(HTTP_OK, response.StatusCode);
    }

    [TestMethod]
    public async void Login_UsingCorrectCredentialsTwice_ShouldPass()
    {
        LoginRequest request = new()
        {
            Email = "test@email.com",
            Password = "password"
        };

        ObjectResult response = (ObjectResult)await controller.Login(request);
        Assert.AreEqual(HTTP_OK, response.StatusCode);
        response = (ObjectResult)await controller.Login(request);
        Assert.AreEqual(HTTP_OK, response.StatusCode);
    }
}
