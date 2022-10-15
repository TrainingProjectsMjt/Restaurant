using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Moq;
using Restaurant.KitchenManager.API.Models;
using Restaurant.KitchenManager.API.Repositories.Pizzas;
using Restaurant.KitchenManager.UnitTests.Helpers;
using Xunit;

namespace Restaurant.KitchenManager.UnitTests.RepositoryTests.Pizzas
{
    public class PizzaRepositoryShould
    {
        private Mock<Container> _containerMock;
        private Mock<CosmosClient> _cosmosClientMock;
        private Mock<IConfiguration> _configMock;

        private PizzaRepository _sut;

        public PizzaRepositoryShould()
        {
            _containerMock = new Mock<Container>();
            _cosmosClientMock = new Mock<CosmosClient>();
            _cosmosClientMock.Setup(x => x.GetContainer(It.IsAny<string>(), It.IsAny<string>())).Returns(_containerMock.Object);
            _configMock = new Mock<IConfiguration>();

            _sut = new PizzaRepository(
                _cosmosClientMock.Object,
                _configMock.Object);
        }

        [Fact]
        public async Task FireCreatePizzaAsync()
        {
            // Arrange
            var testPizza = TestDataGenerator.GenerateHawaiianPizza();

            _containerMock.SetupCreateItemAsync<Pizza>();

            // Act
            await _sut.CreatePizza(testPizza);

            // Assert
            _containerMock.Verify(o => o.CreateItemAsync(
                It.IsAny<Pizza>(),
                It.IsAny<PartitionKey>(),
                It.IsAny<ItemRequestOptions>(),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task FireDeletePizzaAsync()
        {
            // Arrange
            var testPizza = TestDataGenerator.GenerateHawaiianPizza();

            _containerMock.SetupDeleteItemAsync<Pizza>();

            // Act
            await _sut.DeletePizza(testPizza.Id, testPizza.PizzaId);

            // Assert
            _containerMock.Verify(c => c.DeleteItemAsync<Pizza>(
                It.IsAny<string>(),
                It.IsAny<PartitionKey>(),
                It.IsAny<ItemRequestOptions>(),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task FireGetAllPizzasAsync()
        {
            // Arrange
            var testPizzas = TestDataGenerator.GenerateAllPizzas();

            _containerMock.SetupItemQueryIteratorMock(testPizzas);

            // Act
            var response = await _sut.GetAllPizzas();

            // Assert
            _containerMock.Verify(c => c.GetItemQueryIterator<Pizza>(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<QueryRequestOptions>()),
                Times.Once);

            Assert.Equal(testPizzas, response);
        }

        [Fact]
        public async Task FireGetPizzaByIdAsync()
        {
            // Arrange
            var testPizza = TestDataGenerator.GenerateHawaiianPizza();

            _containerMock.SetupItemQueryIteratorMock(new List<Pizza> { testPizza });

            // Act
            var response = await _sut.GetPizzaById(testPizza.Id);

            // Assert
            _containerMock.Verify(c => c.GetItemQueryIterator<Pizza>(
                It.IsAny<QueryDefinition>(),
                It.IsAny<string>(),
                It.IsAny<QueryRequestOptions>()),
                Times.Once);

            Assert.Equal(testPizza, response);
        }

        [Fact]
        public async Task FireGetPizzaByNameAsync()
        {
            // Arrange
            var testPizza = TestDataGenerator.GenerateHawaiianPizza();

            _containerMock.SetupItemQueryIteratorMock(new List<Pizza> { testPizza });

            // Act
            var response = await _sut.GetPizzaByName(testPizza.Name);

            // Assert
            _containerMock.Verify(c => c.GetItemQueryIterator<Pizza>(
                It.IsAny<QueryDefinition>(),
                It.IsAny<string>(),
                It.IsAny<QueryRequestOptions>()), Times.Once);

            Assert.Equal(testPizza, response);
        }

        [Fact]
        public async Task FireUpdatePizzaAsync()
        {
            // Arrange
            var testPizza = TestDataGenerator.GenerateHawaiianPizza();

            _containerMock.SetupUpsertItemAsync<Pizza>();

            // Act
            await _sut.UpdatePizza(testPizza);

            // Assert
            _containerMock.Verify(c => c.UpsertItemAsync(
                It.IsAny<Pizza>(),
                It.IsAny<PartitionKey>(),
                It.IsAny<ItemRequestOptions>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
