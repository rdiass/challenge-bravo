using CurrencyConverter.Api.Controllers;
using CurrencyConverter.Api.Services;
using FluentAssertions;
using Moq;
using System.Net;

namespace CurrencyConverter.Api.Tests.Controllers
{
    public class CurrencyControllerTests
    {
        private readonly Mock<ICurrencyService> _currencyServiceMock;

        public CurrencyControllerTests()
        {
            _currencyServiceMock = new Mock<ICurrencyService>();
        }

        [Test]
        public void When_Invalid_Currency_ConvertCurrency_Should_Throw_Exception()
        {
            // Arrange
            _currencyServiceMock.Setup(x => x.ConvertCurrency("", "", 1))
                .Throws(new Exception("Currency not found"));

            var controller = new CurrencyController(_currencyServiceMock.Object);

            // Act
            var response = controller.ConvertCurrency("", "", 1);

            // Assert
            response.Should().NotBeNull();
            response.Result.ResponseResult.Status.Should().Be((int)HttpStatusCode.BadRequest);
            response.Result.ResponseResult.Errors.Should().NotBeNull();
            response.Result.ResponseResult.Errors.Messages.First().Should().Contain("Currency not found");
        }

        [Test]
        public void When_Valid_Currency_ConvertCurrency_Should_Response_Ok_And_Result()
        {
            // Arrange
            _currencyServiceMock.Setup(x => x.ConvertCurrency("USD", "BRL", 1)).ReturnsAsync(5);

            var controller = new CurrencyController(_currencyServiceMock.Object);

            // Act
            var response = controller.ConvertCurrency("USD", "BRL", 1);

            // Assert
            response.Should().NotBeNull();
            response.Result.ResponseResult.Status.Should().Be((int)HttpStatusCode.OK);
            response.Result.Result.Should().Be((double)5.0);
            response.Result.ResponseResult.Errors.Should().BeNull();
        }
    }
}