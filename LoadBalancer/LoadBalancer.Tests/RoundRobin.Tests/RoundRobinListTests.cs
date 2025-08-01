using LoadBalancer.RoundRobin;
using LoadBalancer.Services;
namespace LoadBalancer.Tests.RoundRobin.Tests;

    public class RoundRobinListTests
    {
        private TestServer testServer1;
        private TestServer testServer2;
        private TestServer testServer3;
        private RoundRobinList roundRobinList;

        [SetUp]
        public void Setup()
        {
            roundRobinList = new RoundRobinList();
            testServer1 = new TestServer("127.0.0.1", 8080, 1);
            testServer2 = new TestServer("127.0.0.2", 9090, 2);
            testServer3 = new TestServer("127.0.0.3", 1010, 5);
        }

        #region NextNode Tests
        [Test]
        public void TestNextNodeWithEmptyList()
        {
            // Arrange
            var expectedMessage = "No Servers Available";
            // Act
            var actual = Assert.Throws<ArgumentOutOfRangeException>(() => roundRobinList.NextNode());
            // Assert
            Assert.That(actual.Message, Does.Contain(expectedMessage));
        }

        [Test]
        public void TestNextNodeWithNonEmptyList()
        {
            // Arrange
            roundRobinList.Append(testServer1);
            roundRobinList.Append(testServer2);
            roundRobinList.Append(testServer3);
            var firstExpectedNode = testServer1;
            var secondAndThirdExpectedNode = testServer2;
            var fourthExpectedNode = testServer3;
            // Act - Running multiple times to ensure it gets the correct node bearing the weights in mind
            var firstActualNode = roundRobinList.NextNode();
            var secondActualNode = roundRobinList.NextNode();
            var thirdActualNode = roundRobinList.NextNode();
            var fourthActualNode = roundRobinList.NextNode();
            // Assert
            Assert.That(firstActualNode, Is.EqualTo(firstExpectedNode));
            Assert.That(secondActualNode, Is.EqualTo(secondAndThirdExpectedNode));
            Assert.That(thirdActualNode, Is.EqualTo(secondAndThirdExpectedNode));
            Assert.That(fourthActualNode, Is.EqualTo(fourthExpectedNode));
        }
        #endregion

        #region PrintNodes Tests
        [Test]
        public void TestPrintNodesWithEmptyList()
        {
            // Arrange
            var expected = "List is empty";
            // Act
            var actual = roundRobinList.PrintNodes();
            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void TestPrintNodesWithNonEmptyList()
        {
            // Arrange
            roundRobinList.Append(testServer1);
            roundRobinList.Append(testServer2);
            roundRobinList.Append(testServer3);
            var expected = $"{testServer1}{testServer2}{testServer3}";
            // Act
            var actual = roundRobinList.PrintNodes();
            // Assert
            Assert.That(actual, Is.EqualTo(expected));
            #endregion
        }
    }

