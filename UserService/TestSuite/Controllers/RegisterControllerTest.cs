using Microsoft.AspNetCore.Mvc;
using UserService.Controllers;
using UserService.DTO;

namespace TestSuite.Controllers;

[TestClass]
public class RegisterControllerTest
{
    private const string NEW_EMAIL = "new@email.com";
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
            Password = EncryptionService.EncryptPassword("password", out byte[] salt),
            Salt = salt
        };
        Mock<IRegisterRepository> repository = new();
        repository.Setup(p => p.IsEmailExisting(It.IsAny<UserData>())).Returns(false);
        repository.Setup(p => p.IsEmailExisting(It.Is<UserData>(u => u.Email.Equals("existing@email.com")))).Returns(true);
        repository.Setup(p => p.Register(It.Is<UserData>(u => u.Email.Equals("existing@email.com")))).Returns(false);
        repository.Setup(p => p.Register(It.IsAny<UserData>())).Returns(true);
        Mock<ILoginService> loginService = new();//TODO: Fix this
        loginService.Setup(service => service.Login(It.IsAny<UserData>())).ReturnsAsync(new UserData());
        loginService.Setup(service => service.Login(It.Is<UserData>(data => data.Email.Equals(NEW_EMAIL)))).ReturnsAsync(new UserData(){Token ="Token"});
        
        IRegisterService service = new RegisterService(repository.Object, loginService.Object);
        controller = new(service);
    }

    [TestMethod]
    public async void Register_NewEmail_ShouldReturnOk()
    {
        UserRequest request = new()
        {
            Email = NEW_EMAIL,
            Password = "Password123!"
        };

        OkObjectResult response = (OkObjectResult)await controller.Register(request);
        Assert.IsNotNull(response);
        Assert.AreEqual(HTTP_OK, response.StatusCode);
    }
    
    [TestMethod]
    public async void Register_ExistingEmail_ShouldReturnUnauthorized()
    {
        UserRequest request = new()
        {
            Email = "existing@email.com",
            Password = "password"
        };

        UnauthorizedObjectResult response = (UnauthorizedObjectResult)await controller.Register(request);
        Assert.IsNotNull(response);
        Assert.AreEqual(HTTP_UNAUTHORIZED, response.StatusCode);
    }
}
