using LoadBalancer.Interfaces;
using LoadBalancer.RoundRobin;
using LoadBalancer.Services;
using Moq;

namespace LoadBalancer.Tests.Services.Tests;
public class LoadBalancerTests
{
    private Mock<ILoadBalancerClientHandler> mockCLientHandler;
    private Mock<IServer> mockServer1;
    private Mock<IServer> mockServer2;
    private Mock<IServer> mockServer3;
    private LoadBalancerService loadBalancer;
    private readonly string ip = "127.0.0.1";
    private readonly int port = 8080;

    [SetUp]
    public void Setup()
    {
        mockCLientHandler = new Mock<ILoadBalancerClientHandler>();

        mockServer1 = new Mock<IServer>();
        mockServer1.Setup(s => s.ToString()).Returns("\nServer1");
        mockServer1.Setup(s => s.Weight).Returns(1);

        mockServer2 = new Mock<IServer>();
        mockServer2.Setup(s => s.ToString()).Returns("\nServer2");
        mockServer2.Setup(s => s.Weight).Returns(2);

        mockServer3 = new Mock<IServer>();
        mockServer3.Setup(s => s.ToString()).Returns("\nServer3");
        mockServer3.Setup(s => s.Weight).Returns(1);

        loadBalancer = new([mockServer1.Object, mockServer2.Object, mockServer3.Object], ip, port, mockCLientHandler.Object);
    }

    [Test]
    public void TestAddServer()
    {
        // Arrange
        var mockServer4 = new Mock<IServer>();
        mockServer4.Setup(s => s.ToString()).Returns("\nServer4");
        var expected = $"{mockServer1.Object.ToString()}{mockServer2.Object.ToString()}{mockServer3.Object.ToString()}{mockServer4.Object.ToString()}";
        // Act
        loadBalancer.AddServer(mockServer4.Object);
        var actual = loadBalancer.PrintServers();
        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void TestNextServerWithDifferingWeights()
    {
        // Arrange
        var firstExpectedNode = mockServer1.Object;
        var secondAndThirdExpectedNode = mockServer2.Object;
        var fourthExpectedNode = mockServer3.Object;
        // Act - Running multiple times to ensure it gets the correct node bearing the weights in mind
        var firstActualNode = loadBalancer.NextServer();
        var secondActualNode = loadBalancer.NextServer();
        var thirdActualNode = loadBalancer.NextServer();
        var fourthActualNode = loadBalancer.NextServer();
        // Assert
        Assert.That(firstActualNode, Is.EqualTo(firstExpectedNode));
        Assert.That(secondActualNode, Is.EqualTo(secondAndThirdExpectedNode));
        Assert.That(thirdActualNode, Is.EqualTo(secondAndThirdExpectedNode));
        Assert.That(fourthActualNode, Is.EqualTo(fourthExpectedNode));
    }

    [Test]
    public void TestNextServerConcurrentAccess()
    {
        // Arrange
        var expectedServers = new[] { mockServer1.Object, mockServer2.Object, mockServer3.Object };
        var concurrentCalls = 100;
        var tasks = new List<Task<IServer>>();
        // Act
        for (int i = 0; i < concurrentCalls; i++)
        {
            tasks.Add(Task.Run(() =>
            {
                return loadBalancer.NextServer();
            }));
        }
        Task.WaitAll(tasks.ToArray());
        var results = tasks.Select(t => t.Result).ToList();
        // Assert
        Assert.That(results, Has.Count.EqualTo(concurrentCalls));
        Assert.That(results.All(r => expectedServers.Contains(r)), Is.True);
    }

    [Test]
    public void TestPrintServers()
    {
        // Arrange
        var expected = $"{mockServer1.Object.ToString()}{mockServer2.Object.ToString()}{mockServer3.Object.ToString()}";
        // Act
        var actual = loadBalancer.PrintServers();
        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void TestRemoveServerWithEmptyList()
    {
        // Arrange
        var expected = false;
        var expectedMessage = "List is empty";
        // Act
        loadBalancer.RemoveServer(mockServer1.Object);
        loadBalancer.RemoveServer(mockServer2.Object);
        loadBalancer.RemoveServer(mockServer3.Object);
        var actual = loadBalancer.RemoveServer(mockServer1.Object);
        var actualMessage = loadBalancer.PrintServers();
        // Assert
        Assert.That(actual, Is.EqualTo(expected));
        Assert.That(actualMessage, Is.EqualTo(expectedMessage));
    }


    [Test]
    public void TestRemoveServerWhenServereIsInTheMiddle()
    {
        // Arrange
        var expected = true;
        var expectedMessage = mockServer1.Object.ToString() + mockServer3.Object.ToString();
        // Act
        var actual = loadBalancer.RemoveServer(mockServer2.Object);
        var actualMessage = loadBalancer.PrintServers();
        // Assert
        Assert.That(actual, Is.EqualTo(expected));
        Assert.That(actualMessage, Is.EqualTo(expectedMessage));
    }

    [Test]
    public void TestRemoveServerWhenServerIsAtTheEnd()
    {
        // Arrange
        var expected = true;
        var expectedMessage = mockServer1.Object.ToString() + mockServer2.Object.ToString();
        // Act
        var actual = loadBalancer.RemoveServer(mockServer3.Object);
        var actualMessage = loadBalancer.PrintServers();
        // Assert
        Assert.That(actual, Is.EqualTo(expected));
        Assert.That(actualMessage, Is.EqualTo(expectedMessage));
    }
}
