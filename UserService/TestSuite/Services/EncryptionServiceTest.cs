namespace TestSuite.Services;

[TestClass]
public class EncryptionServiceTest
{
    [TestMethod]
    public void EncryptPassword_SaltIsDifferent()
    {
        EncryptionService.EncryptPassword("password", out byte[] salt);
        EncryptionService.EncryptPassword("password", out byte[] salt2);

        Assert.AreEqual(salt, salt);
        Assert.AreEqual(salt2, salt2);
        Assert.AreNotEqual(salt, salt2);
    }

    [TestMethod]
    public void IsPasswordMatching_MismatchingPassword_ShouldReturnFalse()
    {
        string password = EncryptionService.EncryptPassword("Password", out byte[] salt);
        bool result = EncryptionService.IsMatching(password, "password", salt);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsPasswordMatching_CorrectPassword_ShouldReturnTrue()
    {
        string password = EncryptionService.EncryptPassword("Password", out byte[] salt);
        bool result = EncryptionService.IsMatching(password, "Password", salt);
        Assert.IsTrue(result);
    }
}
