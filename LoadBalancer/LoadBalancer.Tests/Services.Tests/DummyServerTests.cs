using LoadBalancer.Services;
namespace LoadBalancer.Tests.Services.Tests;

public class DummyServerTests
{
    private DummyServer testServer;
    private readonly string address = "127.0.0.1";
    private readonly int port = 8080;
    private readonly int weight = 5;

    [SetUp]
    public void Setup() => testServer = new DummyServer(address, port, weight);

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

