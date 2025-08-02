using Moq;
using LoadBalancer.Services;
using LoadBalancer.Interfaces;
namespace LoadBalancer.Tests.Services.Tests;

public class DummyServerTests
{
    private Mock<IServerClientHandler> mockClientHandler;
    private DummyServer testServer;
    private readonly string address = "127.0.0.1";
    private readonly int port = 8080;
    private readonly int weight = 5;

    [SetUp]
    public void Setup()
    {
        mockClientHandler = new Mock<IServerClientHandler>();
        testServer = new DummyServer(address, port, weight, mockClientHandler.Object);
    }

    [Test]
    public void TestToString()
    {
        // Arrange
        var expectedToString = $"\nAddress: {address} \t Port: {port} \t Weight: {weight}";
        // Act
        var actualToString = testServer.ToString();
        // Arrange
        Assert.That(actualToString, Is.EqualTo(expectedToString));
    }
}

