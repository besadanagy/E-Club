using E_Club.DTOs.Auth.Requests;
using E_Club.DTOs.Auth.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace E_Club.Tests.Unit.Validators;

public class RegisterRequestValidatorTests
{
    private readonly RegisterRequestValidator _validator;

    public RegisterRequestValidatorTests()
    {
        _validator = new RegisterRequestValidator();
    }

    private RegisterRequest CreateValidRequest()
    {
        return new RegisterRequest("Ahmed Ali", "test@example.com", "StrongPassword123!", "01000000000", "29901010101010", false);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("Ahmed")] // Missing last name
    public void Validate_WhenFullNameIsInvalid_ShouldHaveError(string fullName)
    {
        // Arrange
        var request = CreateValidRequest() with { FullName = fullName ?? "" }; // Prevent null for record constructor if needed, but it accepts string

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void Validate_WhenFullNameIsValid_ShouldNotHaveError()
    {
        // Arrange
        var request = CreateValidRequest();

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.FullName);
    }

    [Theory]
    [InlineData("")]
    [InlineData("invalid-email")]
    public void Validate_WhenEmailIsInvalid_ShouldHaveError(string email)
    {
        // Arrange
        var request = CreateValidRequest() with { Email = email };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Validate_WhenEmailIsValid_ShouldNotHaveError()
    {
        // Arrange
        var request = CreateValidRequest();

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    [Theory]
    [InlineData("weakpassword")] // no uppercase, digit, special char
    [InlineData("Short1!")] // < 8 chars
    public void Validate_WhenPasswordIsInvalid_ShouldHaveError(string password)
    {
        // Arrange
        var request = CreateValidRequest() with { Password = password };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Validate_WhenPasswordIsValid_ShouldNotHaveError()
    {
        // Arrange
        var request = CreateValidRequest();

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Validate_WhenNationalIdIsValid_ShouldNotHaveError()
    {
        // Arrange
        var request = CreateValidRequest();

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.NationalId);
    }

    [Theory]
    [InlineData("1234567890123")] // 13 digits
    [InlineData("123456789012345")] // 15 digits
    [InlineData("abcdefghijklmn")] // Not numbers
    public void Validate_WhenNationalIdIsInvalid_ShouldHaveError(string nationalId)
    {
        // Arrange
        var request = CreateValidRequest() with { NationalId = nationalId };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.NationalId);
    }
}
