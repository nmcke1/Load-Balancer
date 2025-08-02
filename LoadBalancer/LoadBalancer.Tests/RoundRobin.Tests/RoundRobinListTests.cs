using Moq;
using LoadBalancer.Interfaces;
using LoadBalancer.RoundRobin;
using LoadBalancer.Services;
namespace LoadBalancer.Tests.RoundRobin.Tests;

public class RoundRobinListTests
{
    private Mock<IServer> mockServer1;
    private Mock<IServer> mockServer2;
    private Mock<IServer> mockServer3;
    private RoundRobinList roundRobinList;

    [SetUp]
    public void Setup()
    {
        roundRobinList = new RoundRobinList();

        mockServer1 = new Mock<IServer>();
        mockServer1.Setup(s => s.ToString()).Returns("\nServer1");
        mockServer1.Setup(s => s.Weight).Returns(1);

        mockServer2 = new Mock<IServer>();
        mockServer2.Setup(s => s.ToString()).Returns("\nServer2");
        mockServer2.Setup(s => s.Weight).Returns(2);

        mockServer3 = new Mock<IServer>();
        mockServer3.Setup(s => s.ToString()).Returns("\nServer3");
        mockServer3.Setup(s => s.Weight).Returns(1);
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
        roundRobinList.AppendList(mockServer1.Object);
        roundRobinList.AppendList(mockServer2.Object);
        roundRobinList.AppendList(mockServer3.Object);
        var firstExpectedNode = mockServer1.Object;
        var secondAndThirdExpectedNode = mockServer2.Object;
        var fourthExpectedNode = mockServer3.Object;
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

    [Test]
    public void TestNextNodeConcurrentAccess()
    {
        // Arrange
        roundRobinList.AppendList(mockServer1.Object);
        roundRobinList.AppendList(mockServer2.Object);
        roundRobinList.AppendList(mockServer3.Object);
        var expectedServers = new[] { mockServer1.Object, mockServer2.Object, mockServer3.Object };
        var concurrentCalls = 100;
        var tasks = new List<Task<IServer>>();
        // Act
        for (int i = 0; i < concurrentCalls; i++)
        {
            tasks.Add(Task.Run(() =>
            {
                return roundRobinList.NextNode();
            }));
        }
        Task.WaitAll(tasks.ToArray());
        var results = tasks.Select(t => t.Result).ToList();
        // Assert
        Assert.That(results, Has.Count.EqualTo(concurrentCalls));
        Assert.That(results.All(r => expectedServers.Contains(r)), Is.True);
    }

    #endregion

    #region PrintNodes Tests
    [Test]
    public void TestPrintNodesWithEmptyList()
    {
        // Arrange
        var expected = "List is empty";
        // Act
        var actual = roundRobinList.ToString();
        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void TestPrintNodesWithNonEmptyList()
    {
        // Arrange
        roundRobinList.AppendList(mockServer1.Object);
        roundRobinList.AppendList(mockServer2.Object);
        roundRobinList.AppendList(mockServer3.Object);
        var expected = $"{mockServer1.Object.ToString()}{mockServer2.Object.ToString()}{mockServer3.Object.ToString()}";
        // Act
        var actual = roundRobinList.ToString();
        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void TestRemoveNodeConcurrentAccess()
    {
        // Arrange
        roundRobinList.AppendList(mockServer1.Object);
        roundRobinList.AppendList(mockServer2.Object);
        roundRobinList.AppendList(mockServer3.Object);
        var expectedToString = mockServer1.Object.ToString() + mockServer2.Object.ToString() + mockServer3.Object.ToString();
        int concurrentCalls = 100;
        var tasks = new List<Task<String>>();
        // Act
        for (int i = 0; i < concurrentCalls; i++)
        {
            tasks.Add(Task.Run(() =>
            {
                return roundRobinList.ToString();
            }));
        }
        Task.WaitAll(tasks.ToArray());
        var results = tasks.Select(t => t.Result).ToList();
        // Assert
        Assert.That(results, Has.Count.EqualTo(concurrentCalls));
        Assert.That(results.All(r => r == expectedToString), Is.True);
    }
    #endregion

    #region RemoveNode Tests
    [Test]
    public void TestRemoveNodeWithEmptyList()
    {
        // Arrange
        var expected = false;
        // Act
        var actual = roundRobinList.RemoveNode(mockServer1.Object);
        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void TestRemoveNodeWithItemNotInList()
    {
        // Arrange
        roundRobinList.AppendList(mockServer1.Object);
        roundRobinList.AppendList(mockServer2.Object);
        var expected = false;
        // Act
        var actual = roundRobinList.RemoveNode(mockServer3.Object);
        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void TestRemoveNodeWithOneItemInList()
    {
        // Arrange
        roundRobinList.AppendList(mockServer1.Object);
        var expected = true;
        var expectedMessage = "List is empty";
        // Act
        var actual = roundRobinList.RemoveNode(mockServer1.Object);
        var actualMessage = roundRobinList.ToString();
        // Assert
        Assert.That(actual, Is.EqualTo(expected));
        Assert.That(actualMessage, Is.EqualTo(expectedMessage));
    }

    [Test]
    public void TestRemoveNodeWhenNodeIsInTheMiddle()
    {
        // Arrange
        roundRobinList.AppendList(mockServer1.Object);
        roundRobinList.AppendList(mockServer2.Object);
        roundRobinList.AppendList(mockServer3.Object);
        var expected = true;
        var expectedMessage = mockServer1.Object.ToString() + mockServer3.Object.ToString();
        // Act
        var actual = roundRobinList.RemoveNode(mockServer2.Object);
        var actualMessage = roundRobinList.ToString();
        // Assert
        Assert.That(actual, Is.EqualTo(expected));
        Assert.That(actualMessage, Is.EqualTo(expectedMessage));
    }

    [Test]
    public void TestRemoveNodeWhenNodeIsAtTheEnd()
    {
        // Arrange
        roundRobinList.AppendList(mockServer1.Object);
        roundRobinList.AppendList(mockServer2.Object);
        roundRobinList.AppendList(mockServer3.Object);
        var expected = true;
        var expectedMessage = mockServer1.Object.ToString() + mockServer2.Object.ToString();
        // Act
        var actual = roundRobinList.RemoveNode(mockServer3.Object);
        var actualMessage = roundRobinList.ToString();
        // Assert
        Assert.That(actual, Is.EqualTo(expected));
        Assert.That(actualMessage, Is.EqualTo(expectedMessage));
    }

    [Test]
    public void TestPrintNodeConcurrentAccess()
    {
        // Arrange
        roundRobinList.AppendList(mockServer1.Object);
        roundRobinList.AppendList(mockServer2.Object);
        var expectedToString = mockServer1.Object.ToString() + mockServer2.Object.ToString();
        var concurrentCalls = 100;

        for (int i = 0; i < concurrentCalls; i++)
        {
            roundRobinList.AppendList(mockServer3.Object);
        }
        var tasks = new List<Task<bool>>();
        // Act
        for (int i = 0; i < concurrentCalls; i++)
        {
            tasks.Add(Task.Run(() =>
            {
                return roundRobinList.RemoveNode(mockServer3.Object);
            }));
        }
        Task.WaitAll(tasks.ToArray());
        var results = tasks.Select(t => t.Result).ToList();
        var actualToString = roundRobinList.ToString();
        // Assert
        Assert.That(results, Has.Count.EqualTo(concurrentCalls));
        Assert.That(results.All(r => r == true), Is.True);
        Assert.That(actualToString, Is.EqualTo(expectedToString));
    }
    #endregion
}


