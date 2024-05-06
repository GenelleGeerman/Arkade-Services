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
        EncryptionService encrypt = new();
        UserData user = new()
        {
            Email = "test@email.com",
            Password = encrypt.EncryptPassword("password", out byte[] salt),
            Salt = salt
        };
        Mock<ILoginRepository> repository = new();
        repository.Setup(p => p.GetUser(It.IsAny<UserData>())).Returns(new UserData());
        repository.Setup(p => p.GetUser(It.Is<UserData>(u => u.Email.Equals("test@email.com")))).Returns(user);
        Mock<IAuthorizationService> auth = new();
        auth.Setup(p => p.GenerateToken(It.IsAny<UserData>())).Returns("Token");
        
        ILoginService service = new LoginService(repository.Object, auth.Object);
        controller = new(service);
    }

    [TestMethod]
    public void Login_IncorrectEmail_ShouldFail()
    {
        LoginRequest request = new()
        {
            Email = "bad@email.com",
            Password = "password"
        };

        UnauthorizedResult response = (UnauthorizedResult)controller.Login(request);
        Assert.IsNotNull(response);
        Assert.AreEqual(HTTP_UNAUTHORIZED, response.StatusCode);
    }

    [TestMethod]
    public void Login_UsingCorrectCredentials_ShouldPass()
    {
        LoginRequest request = new()
        {
            Email = "test@email.com",
            Password = "password"
        };

        OkObjectResult response = (OkObjectResult)controller.Login(request);

        Assert.AreEqual(HTTP_OK, response.StatusCode);
    }

    [TestMethod]
    public void Login_UsingCorrectCredentialsTwice_ShouldPass()
    {
        LoginRequest request = new()
        {
            Email = "test@email.com",
            Password = "password"
        };

        ObjectResult response = (ObjectResult)controller.Login(request);
        Assert.AreEqual(HTTP_OK, response.StatusCode);
        response = (ObjectResult)controller.Login(request);
        Assert.AreEqual(HTTP_OK, response.StatusCode);
    }
}
