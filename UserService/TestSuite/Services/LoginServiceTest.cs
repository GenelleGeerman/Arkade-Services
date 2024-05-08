namespace TestSuite.Services;

[TestClass]
public class LoginServiceTest
{
    private Mock<ILoginRepository> repository = null!;
    private LoginService service = null!;

    [TestInitialize]
    public void BeforeEachTest()
    {
        UserData user = new();
        user.Email = "test@email.com";
        user.Password = EncryptionService.EncryptPassword("password", out byte[] salt);
        user.Salt = salt;

        repository = new();
        repository.Setup(p => p.GetUser(It.IsAny<UserData>())).Returns(new UserData());
        repository.Setup(p => p.GetUser(It.Is<UserData>(u => u.Email.Equals("test@email.com")))).Returns(user);
        Mock<IAuthorizationService> auth = new();
        auth.Setup(p => p.GenerateToken(It.IsAny<UserData>())).Returns("Token");

        service = new(repository.Object, auth.Object);
    }

    [TestMethod]
    public void Login_WithValidCredentials_ShouldReturnTrue()
    {
        //arrange
        UserData request = new();
        request.Email = "test@email.com";
        request.Password = "password";

        //act
        UserData userData = service.Login(request);

        //assert

        Assert.IsNotNull(userData);
    }

    [TestMethod]
    public void Login_WithInvalidCredentials_ShouldReturnEmptyToken()
    {
        //arrange
        UserData request = new();
        request.Email = "test@email.com";
        request.Password = "badPassword";

        //act
        service.Login(request);
        //assert
        Assert.IsTrue(string.IsNullOrEmpty(request.Token));
    }

    [TestMethod]
    public void LoginWithValidCredentials_ShouldHaveEmptyPasswordField()
    {
        //arrange
        UserData request = new();
        request.Email = "test@email.com";
        request.Password = "password";

        //act
        UserData userData = service.Login(request);

        //assert

        Assert.IsTrue(string.IsNullOrEmpty(userData.Password));
    }

    [TestMethod]
    public void LoginWithInvalidEmail_ShouldReturnEmptyToken()
    {
        //arrange
        UserData request = new();
        request.Email = "bad@email.com";
        request.Password = "password";

        //act
        UserData userData = service.Login(request);

        //assert

        Assert.IsTrue(string.IsNullOrEmpty(userData.Token));
    }
    
    
    [TestMethod]
    public void LoginWithCapitalEmail_ShouldReturnToken()
    {
        //arrange
        UserData request = new()
        {
            Email = "Test@email.com",
            Password = "password"
        };

        //act
        UserData userData = service.Login(request);

        //assert

        Assert.IsTrue(userData.Token == "Token");
    }
}
