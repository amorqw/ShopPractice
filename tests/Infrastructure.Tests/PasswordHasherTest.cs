using Domain.Interfaces;
using Infrastructure.Services;
using NUnit.Framework;

namespace Infrastructure.Tests;

[TestFixture]
public class PasswordHasherTest
{
    private IPasswordHasher _passwordHasher;

    [SetUp]
    public void Setup()
    {
        _passwordHasher = new PasswordHasher();
    }

    [Test]
    public void Generate_ShouldReturnHashedPassword()
    {
        string password = "123";
        string hashedPassword = _passwordHasher.Generate(password);
        Assert.That(hashedPassword, Is.Not.Null);
        Assert.That(hashedPassword, Is.Not.Empty);
        Assert.That(hashedPassword, Is.Not.EqualTo(password));
    }

    [Test]
    public void Verify_ShouldReturnTrue_WhenPasswordIsCorrect()
    {
        string password = Guid.NewGuid().ToString();
        string hashedPassword = _passwordHasher.Generate(password);
        bool isValid = _passwordHasher.Verify(password, hashedPassword);
        Assert.That(isValid, Is.True);
    }

    [Test]
    public void Verify_ShouldReturnFalse_WhenPasswordIsIncorrect()
    {
        string password = Guid.NewGuid().ToString();
        string wrongPassword = Guid.NewGuid().ToString();
        string hashedPassword = _passwordHasher.Generate(password);

        bool isValid = _passwordHasher.Verify(wrongPassword, hashedPassword);

        Assert.That(isValid, Is.False);
    }
}