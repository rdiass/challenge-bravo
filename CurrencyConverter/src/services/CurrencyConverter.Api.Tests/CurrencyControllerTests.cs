using CurrencyConverter.Api.Controllers;
using CurrencyConverter.Api.Services;
using FluentAssertions;
using Moq;

namespace CurrencyConverter.Api.Tests
{
    public class CurrencyControllerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void When_Invalid_Currency_ConvertCurrency_Should_Throw_Exception()
        {
            // Arrange
            var mockCurrencyService = new Mock<ICurrencyService>();
            mockCurrencyService.Setup(x => x.ConvertCurrency("", "", 1))
                .Throws(new Exception("Not found"));

            var controller = new CurrencyController(mockCurrencyService.Object);

            // Act
            var response = controller.ConvertCurrency("", "", 1);

            // Assert
            response.Should().NotBeNull();
            response.Result.ResponseResult.Errors.Should().NotBeNull();
            response.Result.ResponseResult.Errors.Messages.First().Should().Contain("Not found");
        }
    }
}