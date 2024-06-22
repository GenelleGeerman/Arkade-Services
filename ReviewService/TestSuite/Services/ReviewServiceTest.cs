namespace TestSuite.Services;

[TestClass]
public class ReviewServiceTest
{
    private const int TERRARIA_ID = 105600;
    private Mock<IReviewRepository> mockRepo = null!;
    private Review review = null!;
    private int reviewIdCounter = 1;

    private ReviewService service = null!;

    [TestInitialize]
    public void BeforeEach()
    {
        review = new()
        {
            Title = "My new Review",
            Description = "This Game is really cool because it just is",
            GameId = TERRARIA_ID,
            UserId = 1
        };
        mockRepo = new();

        // Common mock setup for Create method
        mockRepo.Setup(r => r.Create(It.IsAny<Review>())).ReturnsAsync((Review r) =>
        {
            r.Id = reviewIdCounter++;
            return r;
        });
        Mock<IMessageService> mockmessage = SetupMockMessage();
        service = new(mockRepo.Object, mockmessage.Object);
    }

    private Mock<IMessageService> SetupMockMessage()
    {
        var mock = new Mock<IMessageService>();

        // Setup for Publish
        mock.Setup(m => m.Publish(It.IsAny<MessageData>()))
            .Verifiable();

        // Setup for Subscribe
        mock.Setup(m => m.Subscribe(It.IsAny<MessageData>(), It.IsAny<Action<MessageData>>()))
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

    [TestMethod]
    public async Task CreateReview_ReturnsWithID()
    {
        Review firstReview = await service.Create(review);
        Review secondReview = await service.Create(review);

        Assert.AreEqual(1, firstReview.Id);
        Assert.AreEqual(2, secondReview.Id);
    }

    [TestMethod]
    public async Task GetReviewsByGameId_ReturnsReview()
    {
        await CreateReviewAsync();
        await CreateReviewAsync();
        await CreateReviewAsync();

        review.Title = "Should not show review";
        review.GameId = 1;
        await CreateReviewAsync();

        mockRepo.Setup(r => r.GetByGameId(TERRARIA_ID)).ReturnsAsync(new[] { review, review, review });

        var reviews = await service.GetByGameId(TERRARIA_ID);
        Assert.AreEqual(3, reviews.Length);
    }

    [TestMethod]
    public async Task GetByReviewId()
    {
        Review createdReview = await CreateReviewAsync();

        mockRepo.Setup(r => r.Get(createdReview.Id)).ReturnsAsync(createdReview);

        Review result = await service.Get(createdReview.Id);

        Assert.AreEqual(review.Title, result.Title);
        Assert.AreEqual(review.Description, result.Description);
    }

    [TestMethod]
    public async Task GetReviewsByUserId()
    {
        await CreateReviewAsync(); // user id is automatically 1
        await CreateReviewAsync(); // user id is automatically 1
        await CreateReviewAsync(); // user id is automatically 1

        review.UserId = 2;
        await CreateReviewAsync();
        await CreateReviewAsync();

        mockRepo.Setup(r => r.GetByUserId(1)).ReturnsAsync(new[] { review, review, review });
        mockRepo.Setup(r => r.GetByUserId(2)).ReturnsAsync(new[] { review, review });

        var results = await service.GetByUserId(1);
        Assert.AreEqual(3, results.Length);

        var secondResults = await service.GetByUserId(2);
        Assert.AreEqual(2, secondResults.Length);
    }

    [TestMethod]
    public async Task UpdateReviews()
    {
        const string newTitle = "I Changed It!!";
        Review createdReview = await CreateReviewAsync();
        review.Id = createdReview.Id;
        review.Title = newTitle;

        mockRepo.Setup(r => r.Update(review)).ReturnsAsync(review);

        Review result = await service.Update(review);

        Assert.AreEqual(newTitle, result.Title);
    }

    [TestMethod]
    public async Task DeleteReview()
    {
        Review createdReview = await CreateReviewAsync();

        mockRepo.Setup(r => r.Delete(createdReview.Id)).ReturnsAsync(true);

        Assert.IsTrue(await service.Delete(createdReview.Id));
        mockRepo.Setup(r => r.Delete(createdReview.Id)).ReturnsAsync(false);
        Assert.IsFalse(await service.Delete(createdReview.Id));
    }

    private async Task<Review> CreateReviewAsync()
    {
        return await service.Create(review);
    }
}
