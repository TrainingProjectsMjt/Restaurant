using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Restaurant.KitchenManager.API.Functions.Toppings;
using Restaurant.KitchenManager.API.Repositories.Toppings;
using Restaurant.KitchenManager.UnitTests.Helpers;
using Xunit;

namespace Restaurant.KitchenManager.UnitTests.FunctionTests.Toppings
{
    public class GetToppingByNameShould
    {
        private Mock<ILogger<GetToppingByName>> _loggerMock;
        private Mock<IConfiguration> _configMock;
        private Mock<IToppingRepository> _toppingRepositoryMock;
        private Mock<HttpRequest> _httpRequestMock;

        private GetToppingByName _func;

        public GetToppingByNameShould()
        {
            _loggerMock = new Mock<ILogger<GetToppingByName>>();
            _configMock = new Mock<IConfiguration>();
            _toppingRepositoryMock = new Mock<IToppingRepository>();
            _httpRequestMock = new Mock<HttpRequest>();

            _func = new GetToppingByName(
                _loggerMock.Object,
                _configMock.Object,
                _toppingRepositoryMock.Object);
        }

        [Fact]
        public async Task Return200OnOk()
        {
            // Arrange
            var topping = TestDataGenerator.GenerateBaconTopping();
            var byteArray = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(topping));
            var memoryStream = new MemoryStream(byteArray);
            _httpRequestMock
                .Setup(r => r.Body)
                .Returns(memoryStream);

            _toppingRepositoryMock
                .Setup(s => s.GetToppingByName(It.IsAny<string>()))
                .ReturnsAsync(() => topping);

            // Act
            var response = await _func.Run(_httpRequestMock.Object, topping.Name);

            // Assert
            Assert.Equal(typeof(OkObjectResult), response.GetType());
            var okObjectResult = response as OkObjectResult;
            Assert.Equal(200, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task Throw400OnBadRequest()
        {
            // Arrange
            var toppingName = "";

            // Act
            var response = await _func.Run(_httpRequestMock.Object, toppingName);

            // Assert
            Assert.Equal(typeof(StatusCodeResult), response.GetType());
            var responseAsStatusCode = response as StatusCodeResult;
            Assert.Equal(400, responseAsStatusCode.StatusCode);
        }

        [Fact]
        public async Task Throw404OnNotFound()
        {
            // Arrange
            var topping = TestDataGenerator.GenerateBaconTopping();
            var byteArray = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(topping));
            var memoryStream = new MemoryStream(byteArray);
            _httpRequestMock
                .Setup(r => r.Body)
                .Returns(memoryStream);

            _toppingRepositoryMock
                .Setup(s => s.GetToppingByName(It.IsAny<string>()))
                .Throws(new CosmosException("Not found", HttpStatusCode.NotFound, 404, "someActivity", 0.0));

            // Act
            var response = await _func.Run(_httpRequestMock.Object, topping.Id);

            // Assert
            Assert.Equal(typeof(NotFoundResult), response.GetType());
            var notFoundResult = (NotFoundResult)response;
            Assert.Equal(404, notFoundResult.StatusCode);
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

            _toppingRepositoryMock
                .Setup(s => s.GetToppingByName(It.IsAny<string>()))
                .Throws(new Exception("Some error!"));

            // Act
            var response = await _func.Run(_httpRequestMock.Object, topping.Name);

            // Assert
            Assert.Equal(typeof(StatusCodeResult), response.GetType());
            var responseAsStatusCode = (StatusCodeResult)response;
            Assert.Equal(500, responseAsStatusCode.StatusCode);
        }
    }
}
