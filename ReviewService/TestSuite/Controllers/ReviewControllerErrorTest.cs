using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TestSuite.Controllers;

[TestClass]
public class ReviewControllerErrorTest
{
    private ReviewController controller = null!;
    private Mock<IReviewService> mockService = null!;

    [TestInitialize]
    public void BeforeEach()
    {
        mockService = new();
        controller = new(mockService.Object);
    }

    [TestMethod]
    public async Task Create_ThrowsException_Returns500InternalServerError()
    {
        // Arrange
        mockService.Setup(s => s.Create(It.IsAny<Review>())).ThrowsAsync(new("Service exception"));

        // Act
        IActionResult result = await controller.Create(new());

        // Assert
        Assert.IsInstanceOfType(result, typeof(ObjectResult));
        ObjectResult? objectResult = result as ObjectResult;
        Assert.AreEqual(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.AreEqual("Internal server error: Service exception", objectResult.Value);
    }

    [TestMethod]
    public async Task Get_ThrowsException_Returns500InternalServerError()
    {
        // Arrange
        int reviewId = 1;
        mockService.Setup(s => s.Get(reviewId)).ThrowsAsync(new("Service exception"));

        // Act
        IActionResult result = await controller.Get(reviewId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(ObjectResult));
        ObjectResult? objectResult = result as ObjectResult;
        Assert.AreEqual(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.AreEqual("Internal server error: Service exception", objectResult.Value);
    }

    [TestMethod]
    public async Task Update_ThrowsException_Returns500InternalServerError()
    {
        // Arrange
        mockService.Setup(s => s.Update(It.IsAny<Review>())).ThrowsAsync(new("Service exception"));

        // Act
        IActionResult result = await controller.Update(new());

        // Assert
        Assert.IsInstanceOfType(result, typeof(ObjectResult));
        ObjectResult? objectResult = result as ObjectResult;
        Assert.AreEqual(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.AreEqual("Internal server error: Service exception", objectResult.Value);
    }

    [TestMethod]
    public async Task Delete_ThrowsException_Returns500InternalServerError()
    {
        // Arrange
        int reviewId = 1;
        mockService.Setup(s => s.Delete(reviewId)).ThrowsAsync(new("Service exception"));

        // Act
        IActionResult result = await controller.Delete(reviewId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(ObjectResult));
        ObjectResult? objectResult = result as ObjectResult;
        Assert.AreEqual(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.AreEqual("Internal server error: Service exception", objectResult.Value);
    }

    [TestMethod]
    public async Task GetByUserId_ThrowsException_Returns500InternalServerError()
    {
        // Arrange
        int userId = 1;
        mockService.Setup(s => s.GetByUserId(userId)).ThrowsAsync(new("Service exception"));

        // Act
        IActionResult result = await controller.GetByUserId(userId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(ObjectResult));
        ObjectResult? objectResult = result as ObjectResult;
        Assert.AreEqual(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.AreEqual("Internal server error: Service exception", objectResult.Value);
    }

    [TestMethod]
    public async Task GetByGameId_ThrowsException_Returns500InternalServerError()
    {
        // Arrange
        int gameId = 105600;
        mockService.Setup(s => s.GetByGameId(gameId)).ThrowsAsync(new("Service exception"));

        // Act
        IActionResult result = await controller.GetByGameId(gameId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(ObjectResult));
        ObjectResult? objectResult = result as ObjectResult;
        Assert.AreEqual(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        Assert.AreEqual("Internal server error: Service exception", objectResult.Value);
    }

    private Mock<IMessageService> SetupMockMessage()
    {
        var mock = new Mock<IMessageService>();

        // Setup for Publish
        mock.Setup(m => m.Publish(It.IsAny<MessageData>()))
            .Verifiable();

        // Setup for Subscribe
        mock.Setup(m => m.Subscribe(It.IsAny<MessageData>(),
                It.IsAny<Action<MessageData>>()))
            .Returns((string exchangeName, string queueName, string routingKey, Action<MessageData> handler) =>
            {
                // Simulate message data and invoke the handler instantly
                var messageData = new MessageData
                {
                    ExchangeName = "ProfileResponse",
                    RoutingKey = "ProfileResponse",
                    Data = "Test Data"
                };
                handler(messageData);
                return "test-tag";
            });
        // Setup for UnSubscribe
        mock.Setup(m => m.UnSubscribe(It.IsAny<string>()))
            .Verifiable();

        return mock;
    }
}
