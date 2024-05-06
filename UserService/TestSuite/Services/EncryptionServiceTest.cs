namespace TestSuite.Services;

[TestClass]
public class EncryptionServiceTest
{
    private EncryptionService service = new();

    [TestInitialize]
    public void BeforeEachTest()
    {
        service = new();
    }

    [TestMethod]
    public void EncryptPassword_SaltIsDifferent()
    {
        service.EncryptPassword("password", out byte[] salt);
        service.EncryptPassword("password", out byte[] salt2);

        Assert.AreEqual(salt, salt);
        Assert.AreEqual(salt2, salt2);
        Assert.AreNotEqual(salt, salt2);
    }

    [TestMethod]
    public void IsPasswordMatching_MismatchingPassword_ShouldReturnFalse()
    {
        string password = service.EncryptPassword("Password", out byte[] salt);
        bool result = service.IsMatching(password, "password", salt);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsPasswordMatching_CorrectPassword_ShouldReturnTrue()
    {
        string password = service.EncryptPassword("Password", out byte[] salt);
        bool result = service.IsMatching(password, "Password", salt);
        Assert.IsTrue(result);
    }
}
