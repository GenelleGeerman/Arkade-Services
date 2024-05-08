// namespace TestSuite.Services;
//
// [TestClass]
// public class ProfileServiceTest
// {
//     private ProfileService profileService = null!;
//
//     [TestInitialize]
//     public void BeforeEachTest()
//     {
//         UserData correctUser = new()
//         {
//             Id = 1,
//             Email = "correct@email.com",
//             FirstName = "John",
//             LastName = "Doe",
//             Password = "password",
//             Salt = new byte[2]
//         };
//         correctUser.Salt[0] = 1;
//         correctUser.Salt[1] = 1;
//         Mock<IProfileRepository> repo = new();
//         repo.Setup(r => r.Get(It.IsAny<long>())).Returns(new UserData());
//         repo.Setup(r => r.Get(1)).Returns(correctUser);
//         profileService = new(repo.Object);
//     }
//
//     [TestMethod]
//     public void GetInfo_ValidId_ReturnsData()
//     {
//         UserData data = profileService.Get(1);
//
//         Assert.AreEqual("correct@email.com", data.Email);
//     }
//
//     [TestMethod]
//     public void GetInfo_InvalidId_ReturnsEmpty()
//     {
//         UserData data = profileService.Get(0);
//
//         Assert.AreEqual(string.Empty, data.Email);
//     }
//
//     [TestMethod]
//     public void GetInfo_ValidUser_PasswordFieldAndSaltShouldBeEmpty()
//     {
//         UserData data = profileService.Get(1);
//         Assert.AreEqual(string.Empty, data.Password);
//         Assert.AreEqual(Array.Empty<byte>(), data.Salt);
//     }
// }