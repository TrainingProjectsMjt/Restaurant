using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public class GetAllToppingsShould
    {
        private Mock<ILogger<GetAllToppings>> _loggerMock;
        private Mock<IConfiguration> _configMock;
        private Mock<IToppingRepository> _toppingRepositoryMock;
        private Mock<HttpRequest> _httpRequestMock;

        private GetAllToppings _func;

        public GetAllToppingsShould()
        {
            _loggerMock = new Mock<ILogger<GetAllToppings>>();
            _configMock = new Mock<IConfiguration>();
            _toppingRepositoryMock = new Mock<IToppingRepository>();
            _httpRequestMock = new Mock<HttpRequest>();

            _func = new GetAllToppings(
                _loggerMock.Object,
                _configMock.Object,
                _toppingRepositoryMock.Object);
        }

        [Fact]
        public async Task Return200OnOk()
        {
            // Arrange
            var allToppings = TestDataGenerator.GenerateAllToppings();
            var byteArray = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(allToppings));
            var memoryStream = new MemoryStream(byteArray);
            _httpRequestMock
                .Setup(r => r.Body)
                .Returns(memoryStream);

            _toppingRepositoryMock
                .Setup(s => s.GetAllToppings())
                .ReturnsAsync(() => allToppings);

            // Act
            var response = await _func.Run(_httpRequestMock.Object);

            // Assert
            Assert.Equal(typeof(OkObjectResult), response.GetType());
            var okObjectResult = response as OkObjectResult;
            Assert.Equal(200, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task Throw500OnInternalServerError()
        {
            // Arrange
            var topping = TestDataGenerator.GenerateAllToppings();
            var byteArray = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(topping));
            var memoryStream = new MemoryStream(byteArray);
            _httpRequestMock
                .Setup(r => r.Body)
                .Returns(memoryStream);

            _toppingRepositoryMock
                .Setup(s => s.GetAllToppings())
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
