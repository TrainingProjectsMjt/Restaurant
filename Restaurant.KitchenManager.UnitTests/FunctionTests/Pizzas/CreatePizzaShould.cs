using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Restaurant.KitchenManager.API.Functions.Pizzas;
using Restaurant.KitchenManager.API.Models;
using Restaurant.KitchenManager.API.Repositories.Pizzas;
using Restaurant.KitchenManager.UnitTests.Helpers;
using Xunit;

namespace Restaurant.KitchenManager.UnitTests.FunctionTests.Pizzas
{
    public class CreatePizzaShould
    {
        private Mock<ILogger<CreatePizza>> _loggerMock;
        private Mock<IConfiguration> _configMock;
        private Mock<IPizzaRepository> _pizzaRepositoryMock;
        private Mock<HttpRequest> _httpRequestMock;

        private CreatePizza _func;

        public CreatePizzaShould()
        {
            _loggerMock = new Mock<ILogger<CreatePizza>>();
            _configMock = new Mock<IConfiguration>();
            _pizzaRepositoryMock = new Mock<IPizzaRepository>();
            _httpRequestMock = new Mock<HttpRequest>();

            _func = new CreatePizza(
                _loggerMock.Object,
                _configMock.Object,
                _pizzaRepositoryMock.Object);
        }

        [Fact]
        public async Task Return201OnCreatedAsync()
        {
            // Arrange
            var pizza = TestDataGenerator.GenerateHawaiianPizza();
            var byteArray = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(pizza));
            var memoryStream = new MemoryStream(byteArray);
            _httpRequestMock
                .Setup(r => r.Body)
                .Returns(memoryStream);

            _pizzaRepositoryMock
                .Setup(s => s.GetPizzaByName(It.IsAny<string>()))
                .ReturnsAsync(() => null);
            _pizzaRepositoryMock
                .Setup(s => s.CreatePizza(It.IsAny<Pizza>()))
                .Returns(Task.CompletedTask);

            // Act
            var response = await _func.Run(_httpRequestMock.Object);

            // Assert
            Assert.Equal(typeof(CreatedResult), response.GetType());
            var createdResult = response as CreatedResult;
            Assert.Equal(201, createdResult.StatusCode);
        }

        [Fact]
        public async Task Throw409OnConflictAsync()
        {
            // Arrange
            var pizza = TestDataGenerator.GenerateHawaiianPizza();
            var byteArray = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(pizza));
            var memoryStream = new MemoryStream(byteArray);
            _httpRequestMock
                .Setup(r => r.Body)
                .Returns(memoryStream);

            _pizzaRepositoryMock
                .Setup(s => s.GetPizzaByName(It.IsAny<string>()))
                .ReturnsAsync(() => pizza);
            _pizzaRepositoryMock
                .Setup(s => s.CreatePizza(It.IsAny<Pizza>()))
                .Returns(Task.CompletedTask);

            // Act
            var response = await _func.Run(_httpRequestMock.Object);

            // Assert
            Assert.Equal(typeof(ConflictResult), response.GetType());
            var responseAsStatusCode = (ConflictResult)response;
            Assert.Equal(409, responseAsStatusCode.StatusCode);
        }

        [Fact]
        public async Task Throw500OnInternalServerErrorAsync()
        {
            // Arrange
            var pizza = TestDataGenerator.GenerateHawaiianPizza();
            var byteArray = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(pizza));
            var memoryStream = new MemoryStream(byteArray);
            _httpRequestMock
                .Setup(r => r.Body)
                .Returns(memoryStream);

            _pizzaRepositoryMock
                .Setup(s => s.CreatePizza(It.IsAny<Pizza>()))
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
