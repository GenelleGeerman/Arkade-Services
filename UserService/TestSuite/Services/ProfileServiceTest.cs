namespace TestSuite.Services;

[TestClass]
public class ProfileServiceTest
{
    private Mock<IAuthorizationService> authorizationServiceMock;
    private Mock<IMessageService> messageServiceMock;
    private Mock<IProfileRepository> profileRepositoryMock;
    private ProfileService profileService;

    [TestInitialize]
    public void BeforeEach()
    {
        profileRepositoryMock = new();
        authorizationServiceMock = new();
        messageServiceMock = new();

        profileService = new(
            profileRepositoryMock.Object,
            authorizationServiceMock.Object,
            messageServiceMock.Object
        );
    }

    [TestMethod]
    public async Task Get_ValidToken_ReturnsUserData()
    {
        // Arrange
        string token = "valid_token";
        long userId = 1;
        var expectedUserData = new UserData { Id = userId, Username = "testuser" };

        authorizationServiceMock.Setup(auth => auth.GetId(token)).Returns(userId);
        profileRepositoryMock.Setup(repo => repo.Get(userId)).ReturnsAsync(expectedUserData);

        // Act
        var result = await profileService.Get(token);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedUserData.Id, result.Id);
        Assert.AreEqual(expectedUserData.Username, result.Username);

        // Verify interactions
        authorizationServiceMock.Verify(auth => auth.GetId(token), Times.Once);
        profileRepositoryMock.Verify(repo => repo.Get(userId), Times.Once);
    }

    [TestMethod]
    public async Task GetProfile_ValidId_ReturnsUserData()
    {
        // Arrange
        long userId = 1;
        var expectedUserData = new UserData { Id = userId, Username = "testuser" };

        profileRepositoryMock.Setup(repo => repo.Get(userId)).ReturnsAsync(expectedUserData);

        // Act
        var result = await profileService.GetProfile(userId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedUserData.Id, result.Id);
        Assert.AreEqual(expectedUserData.Username, result.Username);

        // Verify interactions
        profileRepositoryMock.Verify(repo => repo.Get(userId), Times.Once);
    }

    [TestMethod]
    public async Task Update_ValidRequestAndToken_UpdatesUserData()
    {
        string token = "valid_token";
        long userId = 1;
        var updatedUserData = new UserData { Id = userId, Username = "updateduser", Email = "updated@example.com" };

        authorizationServiceMock.Setup(auth => auth.GetId(token)).Returns(userId);
        profileRepositoryMock.Setup(repo => repo.Update(updatedUserData)).ReturnsAsync(updatedUserData);

        var result = await profileService.Update(updatedUserData, token);

        Assert.IsNotNull(result);
        Assert.AreEqual(updatedUserData.Id, result.Id);
        Assert.AreEqual(updatedUserData.Username, result.Username);
        Assert.AreEqual(updatedUserData.Email, result.Email);

        authorizationServiceMock.Verify(auth => auth.GetId(token), Times.Once);
        profileRepositoryMock.Verify(repo => repo.Update(updatedUserData), Times.Once);
    }

    [TestMethod]
    public async Task Delete_ValidToken_DeletesUserDataAndPublishesMessage()
    {
        string token = "valid_token";
        long userId = 1;

        authorizationServiceMock.Setup(auth => auth.GetId(token)).Returns(userId);
        profileRepositoryMock.Setup(repo => repo.Delete(userId)).ReturnsAsync(true);

        var result = await profileService.Delete(token);

        Assert.IsTrue(result);

        authorizationServiceMock.Verify(auth => auth.GetId(token), Times.Once);
        profileRepositoryMock.Verify(repo => repo.Delete(userId), Times.Once);
        messageServiceMock.Verify(msg => msg.Publish(It.IsAny<MessageData>()), Times.Once);
    }
}
