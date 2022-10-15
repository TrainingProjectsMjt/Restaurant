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
using Restaurant.KitchenManager.API.Repositories.Toppings;
using Restaurant.KitchenManager.UnitTests.Helpers;
using Xunit;

namespace Restaurant.KitchenManager.UnitTests.RepositoryTests.Toppings
{
    public class ToppingRepositoryShould
    {
        private Mock<Container> _containerMock;
        private Mock<CosmosClient> _cosmosClientMock;
        private Mock<IConfiguration> _configMock;

        private ToppingRepository _sut;

        public ToppingRepositoryShould()
        {
            _containerMock = new Mock<Container>();
            _cosmosClientMock = new Mock<CosmosClient>();
            _cosmosClientMock.Setup(x => x.GetContainer(It.IsAny<string>(), It.IsAny<string>())).Returns(_containerMock.Object);
            _configMock = new Mock<IConfiguration>();

            _sut = new ToppingRepository(
                _cosmosClientMock.Object,
                _configMock.Object);
        }

        [Fact]
        public async Task FireCreateToppingAsync()
        {
            // Arrange
            var testTopping = TestDataGenerator.GenerateBaconTopping();

            _containerMock.SetupCreateItemAsync<Topping>();

            // Act
            await _sut.CreateTopping(testTopping);

            // Assert
            _containerMock.Verify(o => o.CreateItemAsync(
                It.IsAny<Topping>(),
                It.IsAny<PartitionKey>(),
                It.IsAny<ItemRequestOptions>(),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task FireDeleteToppingAsync()
        {
            // Arrange
            var testTopping = TestDataGenerator.GenerateBaconTopping();

            _containerMock.SetupDeleteItemAsync<Topping>();

            // Act
            await _sut.DeleteTopping(testTopping.Id, testTopping.ToppingId);

            // Assert
            _containerMock.Verify(c => c.DeleteItemAsync<Topping>(
                It.IsAny<string>(),
                It.IsAny<PartitionKey>(),
                It.IsAny<ItemRequestOptions>(),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task FireGetAllToppingsAsync()
        {
            // Arrange
            var testToppings = TestDataGenerator.GenerateAllToppings();

            _containerMock.SetupItemQueryIteratorMock(testToppings);

            // Act
            var response = await _sut.GetAllToppings();

            // Assert
            _containerMock.Verify(c => c.GetItemQueryIterator<Topping>(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<QueryRequestOptions>()),
                Times.Once);

            Assert.Equal(testToppings, response);
        }

        [Fact]
        public async Task FireGetToppingByIdAsync()
        {
            // Arrange
            var testTopping = TestDataGenerator.GenerateBaconTopping();

            _containerMock.SetupItemQueryIteratorMock(new List<Topping> { testTopping });

            // Act
            var response = await _sut.GetToppingById(testTopping.Id);

            // Assert
            _containerMock.Verify(c => c.GetItemQueryIterator<Topping>(
                It.IsAny<QueryDefinition>(),
                It.IsAny<string>(),
                It.IsAny<QueryRequestOptions>()),
                Times.Once);

            Assert.Equal(testTopping, response);
        }

        [Fact]
        public async Task FireGetToppingByNameAsync()
        {
            // Arrange
            var testTopping = TestDataGenerator.GenerateBaconTopping();

            _containerMock.SetupItemQueryIteratorMock(new List<Topping> { testTopping });

            // Act
            var response = await _sut.GetToppingByName(testTopping.Name);

            // Assert
            _containerMock.Verify(c => c.GetItemQueryIterator<Topping>(
                It.IsAny<QueryDefinition>(),
                It.IsAny<string>(),
                It.IsAny<QueryRequestOptions>()), Times.Once);

            Assert.Equal(testTopping, response);
        }

        [Fact]
        public async Task FireUpdateToppingAsync()
        {
            // Arrange
            var testTopping = TestDataGenerator.GenerateBaconTopping();

            _containerMock.SetupUpsertItemAsync<Topping>();

            // Act
            await _sut.UpdateTopping(testTopping);

            // Assert
            _containerMock.Verify(c => c.UpsertItemAsync(
                It.IsAny<Topping>(),
                It.IsAny<PartitionKey>(),
                It.IsAny<ItemRequestOptions>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
