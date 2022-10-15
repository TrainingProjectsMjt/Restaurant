using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Restaurant.KitchenManager.API.Functions.Toppings;
using Restaurant.KitchenManager.API.Models;
using Restaurant.KitchenManager.API.Repositories.Toppings;
using Restaurant.KitchenManager.UnitTests.Helpers;
using Xunit;

namespace Restaurant.KitchenManager.UnitTests.FunctionTests.Toppings
{
    public class CreateToppingShould
    {
        private Mock<ILogger<CreateTopping>> _loggerMock;
        private Mock<IConfiguration> _configMock;
        private Mock<IToppingRepository> _toppingRepository;
        private Mock<HttpRequest> _httpRequestMock;

        private CreateTopping _func;

        public CreateToppingShould()
        {
            _loggerMock = new Mock<ILogger<CreateTopping>>();
            _configMock = new Mock<IConfiguration>();
            _toppingRepository = new Mock<IToppingRepository>();
            _httpRequestMock = new Mock<HttpRequest>();

            _func = new CreateTopping(
                _loggerMock.Object,
                _configMock.Object,
                _toppingRepository.Object);
        }

        [Fact]
        public async Task Return201OnCreated()
        {
            // Arrange
            var topping = TestDataGenerator.GenerateBaconTopping();
            var byteArray = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(topping));
            var memoryStream = new MemoryStream(byteArray);
            _httpRequestMock
                .Setup(r => r.Body)
                .Returns(memoryStream);

            _toppingRepository
                .Setup(s => s.GetToppingByName(It.IsAny<string>()))
                .ReturnsAsync(() => null);
            _toppingRepository
                .Setup(s => s.CreateTopping(It.IsAny<Topping>()))
                .Returns(Task.CompletedTask);

            // Act
            var response = await _func.Run(_httpRequestMock.Object);

            // Assert
            Assert.Equal(typeof(CreatedResult), response.GetType());
            var createdResult = response as CreatedResult;
            Assert.Equal(201, createdResult.StatusCode);
        }

        [Fact]
        public async Task Throw409OnConflict()
        {
            // Arrange
            var topping = TestDataGenerator.GenerateBaconTopping();
            var byteArray = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(topping));
            var memoryStream = new MemoryStream(byteArray);
            _httpRequestMock
                .Setup(r => r.Body)
                .Returns(memoryStream);

            _toppingRepository
                .Setup(s => s.GetToppingByName(It.IsAny<string>()))
                .ReturnsAsync(() => topping);
            _toppingRepository
                .Setup(s => s.CreateTopping(It.IsAny<Topping>()))
                .Returns(Task.CompletedTask);

            // Act
            var response = await _func.Run(_httpRequestMock.Object);

            // Assert
            Assert.Equal(typeof(ConflictResult), response.GetType());
            var responseAsStatusCode = (ConflictResult)response;
            Assert.Equal(409, responseAsStatusCode.StatusCode);
        }

        [Fact]
        public async Task Throw500OnInternalServerError()
        {
            // Arrange
            var topping = TestDataGenerator.GenerateBaconTopping();
            var byteArray = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(topping));
            var memoryStream = new MemoryStream(byteArray);
            _httpRequestMock
                .Setup(r => r.Body)
                .Returns(memoryStream);

            _toppingRepository
                .Setup(s => s.CreateTopping(It.IsAny<Topping>()))
                .Throws(new Exception("Some error!"));

            // Act
            var response = await _func.Run(_httpRequestMock.Object);

            // Assert
            Assert.Equal(typeof(StatusCodeResult), response.GetType());
            var responseAsStatusCode = (StatusCodeResult)response;
            Assert.Equal(500, responseAsStatusCode.StatusCode);
        }
    }
}
