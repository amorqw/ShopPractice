using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Infrastructure.Services.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Infrastructure.Tests;

    [TestFixture]
    public class AuthServiceTest
    {
        private AuthService _authService;
        private Mock<IUser> _userMock;
        private Mock<IPasswordHasher> _passwordHasherMock;
        private Mock<IJwtProvider> _jwtProviderMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Mock<HttpContext> _httpContextMock;
        private Mock<IResponseCookies> _responseCookiesMock;

        [SetUp]
        public void Setup()
        {
            _userMock = new Mock<IUser>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _jwtProviderMock = new Mock<IJwtProvider>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _httpContextMock = new Mock<HttpContext>();
            _responseCookiesMock = new Mock<IResponseCookies>();

            var responseMock = new Mock<HttpResponse>();
            responseMock.Setup(r => r.Cookies).Returns(_responseCookiesMock.Object);

            _httpContextMock.Setup(c => c.Response).Returns(responseMock.Object);
            _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(_httpContextMock.Object);

            _authService = new AuthService(
                _userMock.Object,
                _passwordHasherMock.Object,
                _jwtProviderMock.Object,
                _httpContextAccessorMock.Object
            );
        }

        [Test]
        public async Task Register_ShouldCallCreateUser()
        {
            // Arrange
            var userName = "John";
            var email = "john@example.com";
            var password = "securepassword";
            var phoneNumber = "1234567890";
            var hashedPassword = "hashedPassword";
            _passwordHasherMock.Setup(p => p.Generate(password)).Returns(hashedPassword);
            _userMock.Setup(u => u.CreateUser(It.IsAny<User>())).ReturnsAsync(1);

            // Act
            var result = await _authService.Register(userName, email, password, phoneNumber);

            // Assert
            _passwordHasherMock.Verify(p => p.Generate(password), Times.Once);
            _userMock.Verify(u => u.CreateUser(It.Is<User>(u =>
                u.FirstName == userName &&
                u.Email == email &&
                u.Password == hashedPassword &&
                u.PhoneNumber == phoneNumber)), Times.Once);

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public async Task Login_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var email = "john@example.com";
            var password = "securepassword";
            var hashedPassword = "hashedPassword";
            var user = new User { Email = email, Password = hashedPassword };
            var token = "jwtToken";

            _userMock.Setup(u => u.GetUserByEmail(email)).ReturnsAsync(user);
            _passwordHasherMock.Setup(p => p.Verify(password, hashedPassword)).Returns(true);
            _jwtProviderMock.Setup(j => j.GenerateToken(user)).Returns(token);

            // Act
            var result = await _authService.Login(email, password);

            // Assert
            _userMock.Verify(u => u.GetUserByEmail(email), Times.Once);
            _passwordHasherMock.Verify(p => p.Verify(password, hashedPassword), Times.Once);
            _jwtProviderMock.Verify(j => j.GenerateToken(user), Times.Once);
            Assert.That(result, Is.EqualTo(token));
        }

        [Test]
        public void Login_ShouldThrowUnauthorizedAccessException_WhenUserNotFound()
        {
            // Arrange
            var email = "nonexistent@example.com";
            var password = "securepassword";

            _userMock.Setup(u => u.GetUserByEmail(email)).ReturnsAsync((User)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
                await _authService.Login(email, password));

            Assert.That(ex.Message, Is.EqualTo("Invalid credentials."));
        }

        [Test]
        public void Login_ShouldThrowUnauthorizedAccessException_WhenPasswordIsIncorrect()
        {
            // Arrange
            var email = "john@example.com";
            var password = "securepassword";
            var hashedPassword = "hashedPassword";
            var user = new User { Email = email, Password = hashedPassword };

            _userMock.Setup(u => u.GetUserByEmail(email)).ReturnsAsync(user);
            _passwordHasherMock.Setup(p => p.Verify(password, hashedPassword)).Returns(false);

            // Act & Assert
            var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
                await _authService.Login(email, password));

            Assert.That(ex.Message, Is.EqualTo("Invalid credentials."));
        }

        [Test]
        public async Task Login_ShouldSetCookie_WhenUserLogsIn()
        {
            // Arrange
            var email = "john@example.com";
            var password = "securepassword";
            var hashedPassword = "hashedPassword";
            var user = new User { Email = email, Password = hashedPassword };
            var token = "jwtToken";

            _userMock.Setup(u => u.GetUserByEmail(email)).ReturnsAsync(user);
            _passwordHasherMock.Setup(p => p.Verify(password, hashedPassword)).Returns(true);
            _jwtProviderMock.Setup(j => j.GenerateToken(user)).Returns(token);

            // Act
            var result = await _authService.Login(email, password);

            // Assert
            _responseCookiesMock
                .Setup(c => c.Append(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CookieOptions>()))
                .Verifiable();
            Assert.That(result, Is.EqualTo(token));
        }

        [Test]
        public async Task CreateUser_ShouldCallCreateUser()
        {
            // Arrange
            var user = new User { FirstName = "John", Email = "john@example.com" };
            _userMock.Setup(u => u.CreateUser(user)).ReturnsAsync(1);

            // Act
            var result = await _authService.CreateUser(user);

            // Assert
            _userMock.Verify(u => u.CreateUser(user), Times.Once);
            Assert.That(result, Is.EqualTo(1));
        }
    }

