using System.Collections.Generic;
using System.Threading;
using Microsoft.Azure.Cosmos;
using Moq;

namespace Restaurant.KitchenManager.UnitTests.Helpers
{
    public static class TestExtensions
    {
        public static Mock<ItemResponse<T>> SetupCreateItemAsync<T>(this Mock<Container> containerMock)
        {
            var itemResponseMock = new Mock<ItemResponse<T>>();

            containerMock
                .Setup(x => x.CreateItemAsync(
                    It.IsAny<T>(),
                    It.IsAny<PartitionKey>(),
                    It.IsAny<ItemRequestOptions>(),
                    It.IsAny<CancellationToken>()))
                .Callback((T item, PartitionKey? pk, ItemRequestOptions opt, CancellationToken ct) => itemResponseMock.Setup(x => x.Resource).Returns(null))
                .ReturnsAsync((T item, PartitionKey? pk, ItemRequestOptions opt, CancellationToken ct) => itemResponseMock.Object);

            return itemResponseMock;
        }

        public static Mock<ItemResponse<T>> SetupDeleteItemAsync<T>(this Mock<Container> containerMock)
        {
            var itemResponseMock = new Mock<ItemResponse<T>>();

            containerMock
                .Setup(x => x.DeleteItemAsync<T>(
                    It.IsAny<string>(),
                    It.IsAny<PartitionKey>(),
                    It.IsAny<ItemRequestOptions>(),
                    It.IsAny<CancellationToken>()))
                .Callback((string id, PartitionKey? pk, ItemRequestOptions opt, CancellationToken ct) => itemResponseMock.Setup(x => x.Resource).Returns(null))
                .ReturnsAsync((string id, PartitionKey? pk, ItemRequestOptions opt, CancellationToken ct) => itemResponseMock.Object);

            return itemResponseMock;
        }

        public static (Mock<FeedResponse<T>> feedResponseMock, Mock<FeedIterator<T>> feedIterator) SetupItemQueryIteratorMock<T>(this Mock<Container> containerMock, IEnumerable<T> itemsToReturn)
        {
            var feedResponseMock = new Mock<FeedResponse<T>>();
            feedResponseMock.Setup(x => x.Resource).Returns(itemsToReturn);
            var iteratorMock = new Mock<FeedIterator<T>>();
            iteratorMock.SetupSequence(x => x.HasMoreResults).Returns(true).Returns(false);
            iteratorMock.Setup(x => x.ReadNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(feedResponseMock.Object);
            containerMock.Setup(x => x.GetItemQueryIterator<T>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<QueryRequestOptions>()))
                .Returns(iteratorMock.Object);
            containerMock.Setup(x => x.GetItemQueryIterator<T>(It.IsAny<QueryDefinition>(), It.IsAny<string>(), It.IsAny<QueryRequestOptions>()))
                .Returns(iteratorMock.Object);

            return (feedResponseMock, iteratorMock);
        }

        public static Mock<ItemResponse<T>> SetupReadItemAsync<T>(this Mock<Container> containerMock, T objectToReturn)
        {
            var itemResponseMock = new Mock<ItemResponse<T>>();
            itemResponseMock.Setup(x => x.Resource).Returns(objectToReturn);

            containerMock
                .Setup(x => x.ReadItemAsync<T>(
                    It.IsAny<string>(),
                    It.IsAny<PartitionKey>(),
                    It.IsAny<ItemRequestOptions>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(itemResponseMock.Object);

            return itemResponseMock;
        }

        public static Mock<ItemResponse<T>> SetupUpsertItemAsync<T>(this Mock<Container> containerMock)
        {
            var itemResponseMock = new Mock<ItemResponse<T>>();

            containerMock
                .Setup(x => x.UpsertItemAsync(
                    It.IsAny<T>(),
                    It.IsAny<PartitionKey>(),
                    It.IsAny<ItemRequestOptions>(),
                    It.IsAny<CancellationToken>()))
                .Callback((T item, PartitionKey? pk, ItemRequestOptions opt, CancellationToken ct) => itemResponseMock.Setup(x => x.Resource).Returns(null))
                .ReturnsAsync((T item, PartitionKey? pk, ItemRequestOptions opt, CancellationToken ct) => itemResponseMock.Object);

            return itemResponseMock;
        }
    }
}
