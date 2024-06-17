using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TestSuite.Controllers;

[TestClass]
public class ReviewControllerTest
{
    private const int TERRARIA_ID = 105600;
    private ReviewController controller = null!;
    private Review review = null!;
    private ReviewService service = null!;

    [TestInitialize]
    public void BeforeEach()
    {
        review = new()
        {
            Title = "Terraria: A classic for ages",
            Description = "Terraria is a game that everyone knows as an amazing game",
            UserId = 1,
            GameId = TERRARIA_ID
        };

        // Mock the repository or data access layer
        Mock<IReviewRepository> mockRepository = new Mock<IReviewRepository>();
        mockRepository.Setup(r => r.Create(It.IsAny<Review>())).ReturnsAsync(review);
        mockRepository.Setup(r => r.Get(It.IsAny<int>())).ReturnsAsync(review);
        mockRepository.Setup(r => r.Update(It.IsAny<Review>())).ReturnsAsync(review);
        mockRepository.Setup(r => r.Delete(It.IsAny<int>())).ReturnsAsync(true);
        mockRepository.Setup(r => r.GetByUserId(It.IsAny<int>())).ReturnsAsync(new[] { review, review });
        mockRepository.Setup(r => r.GetByGameId(It.IsAny<int>())).ReturnsAsync(new[] { review, review });

        // Create the ReviewService instance with mocked repository
        service = new(mockRepository.Object, new Mock<MessageService>().Object);
        controller = new(service);
    }

    [TestMethod]
    public async Task Create()
    {
        IActionResult result = await controller.Create(review);

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        OkObjectResult? okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
    }

    [TestMethod]
    public async Task Get()
    {
        int reviewId = 1;
        IActionResult result = await controller.Get(reviewId);

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        OkObjectResult? okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.AreEqual(review, okResult.Value);
    }

    [TestMethod]
    public async Task Update()
    {
        IActionResult result = await controller.Update(review);

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        Assert.AreEqual(StatusCodes.Status200OK, ((OkObjectResult)result).StatusCode);
    }

    [TestMethod]
    public async Task Delete()
    {
        int reviewId = 1;
        IActionResult result = await controller.Delete(reviewId);

        Assert.IsInstanceOfType(result, typeof(OkResult));
        Assert.AreEqual(StatusCodes.Status200OK, ((OkResult)result).StatusCode);
    }

    [TestMethod]
    public async Task GetByUserId()
    {
        int userId = 1;
        IActionResult result = await controller.GetByUserId(userId);

        OkObjectResult? okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.IsInstanceOfType(okResult.Value, typeof(Review[]));
        Assert.AreEqual(2, ((Review[])okResult.Value).Length);
    }

    [TestMethod]
    public async Task GetByGameId()
    {
        IActionResult result = await controller.GetByGameId(TERRARIA_ID);

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        OkObjectResult? okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.IsInstanceOfType(okResult.Value, typeof(Review[]));
        Assert.AreEqual(2, ((Review[])okResult.Value).Length);
    }
}
