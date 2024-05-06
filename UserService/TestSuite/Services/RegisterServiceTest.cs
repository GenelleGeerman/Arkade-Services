using Microsoft.Extensions.Configuration;

namespace TestSuite.Services;

[TestClass]
public class RegisterServiceTest
{
    private RegisterService service = null!;
    private const string CORRECT_EMAIL = "test@email.com";
    private const string EXISTING_EMAIL = "existing@email.com";

    [TestInitialize]
    public void BeforeEachTest()
    {
        EncryptionService encrypt = new();

        Mock<IRegisterRepository> repository = new();
        repository.Setup(p => p.Register(It.IsAny<UserData>())).Returns(false);
        repository.Setup(p => p.Register(It.Is<UserData>(u => u.Email.Equals(CORRECT_EMAIL)))).Returns(true);
        repository.Setup(p => p.IsEmailExisting(It.Is<UserData>(u => u.Email.Equals(EXISTING_EMAIL)))).Returns(true);
        Mock<IAuthorizationService> auth = new();
        auth.Setup(p => p.GenerateToken(It.IsAny<UserData>())).Returns("Token");
        service = new(repository.Object, auth.Object);
    }

    [TestMethod]
    public void Register_CorrectCredentials_ReturnsToken()
    {
        //arrange
        UserData user = new()
        {
            Email = CORRECT_EMAIL,
            Password = "password"
        };
        //act
        UserData response = service.Register(user);
        //assert
        Assert.IsTrue(response.Token == "Token");
    }

    [TestMethod]
    public void Register_WrongCredentials_ReturnsEmptyTokenField()
    {
        //arrange
        UserData user = new()
        {
            Email = "Incorrect@email.com",
            Password = "password"
        };

        //act
        UserData response = service.Register(user);
        //assert
        Assert.IsTrue(string.IsNullOrEmpty(response.Token));
    }

    [TestMethod]
    public void Register_PasswordEqualsOrGreaterThan8Char_ReturnsToken()
    {
        //arrange
        UserData user = new()
        {
            Email = CORRECT_EMAIL,
            Password = "password"
        };

        //act
        UserData response = service.Register(user);
        //assert
        Assert.IsTrue(response.Token.Equals("Token"));
    }

    [TestMethod]
    public void Register_PasswordBelow8Char_ReturnsEmptyTokenField()
    {
        //arrange
        UserData user = new()
        {
            Email = "Incorrect@email.com",
            Password = "1234567"
        };

        //act
        UserData response = service.Register(user);
        //assert
        Assert.IsTrue(string.IsNullOrEmpty(response.Token));
    }

    [TestMethod]
    [DataRow("existing@email.com")]
    [DataRow("Existing@Email.com")]
    [DataRow("ExiSting@EmaiL.coM")]
    public void IsEmailExisting_AlreadyExists_ReturnsTrue(string email)
    {
        //arrange
        UserData user = new()
        {
            Email = email,
            Password = "1234567"
        };
        //act
        bool result = service.IsEmailExisting(user);
        //assert
        Assert.IsTrue(result);
    } 
    [TestMethod]
    public void IsEmailExisting_DoesNotExist_ReturnsFalse()
    {
        //arrange
        UserData user = new()
        {
            Email = CORRECT_EMAIL,
            Password = "1234567"
        };
        //act
        bool result = service.IsEmailExisting(user);
        //assert
        Assert.IsFalse(result);
    }
}
