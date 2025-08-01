using LoadBalancer.Services;
namespace LoadBalancer.Tests.Services.Tests;

public class DummyServerTests
{
    private DummyServer dummyServer;
    private readonly string address = "127.0.0.1";
    private readonly int port = 8080;

    [SetUp]
    public void Setup() => dummyServer = new DummyServer(address, port);

    [Test]
    public void TestToString()
    {
        // Arrange
        var expectedToString = $"\nAddress: {address} \t Port: {port}";
        // Act
        var actualToString = dummyServer.ToString();
        // Arrange
        Assert.That(actualToString, Is.EqualTo(expectedToString));
    }
}

