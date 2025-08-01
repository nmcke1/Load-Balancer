using LoadBalancer.Interfaces;
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
        roundRobinList.AppendList(testServer1);
        roundRobinList.AppendList(testServer2);
        roundRobinList.AppendList(testServer3);
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

    [Test]
    public void TestNextNodeConcurrentAccess()
    {
        // Arrange
        roundRobinList.AppendList(testServer1);
        roundRobinList.AppendList(testServer2);
        roundRobinList.AppendList(testServer3);
        var expectedServers = new[] { testServer1, testServer2, testServer3 };
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
        Assert.That(results.Count, Is.EqualTo(concurrentCalls));
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
        roundRobinList.AppendList(testServer1);
        roundRobinList.AppendList(testServer2);
        roundRobinList.AppendList(testServer3);
        var expected = $"{testServer1}{testServer2}{testServer3}";
        // Act
        var actual = roundRobinList.ToString();
        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void TestRemoveNodeConcurrentAccess()
    {
        // Arrange
        roundRobinList.AppendList(testServer1);
        roundRobinList.AppendList(testServer2);
        roundRobinList.AppendList(testServer3);
        var expectedToString = testServer1.ToString() + testServer2.ToString() + testServer3.ToString();
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
        Assert.That(results.Count, Is.EqualTo(concurrentCalls));
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
        var actual = roundRobinList.RemoveNode(testServer1);
        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void TestRemoveNodeWithItemNotInList()
    {
        // Arrange
        roundRobinList.AppendList(testServer1);
        roundRobinList.AppendList(testServer2);
        var expected = false;
        // Act
        var actual = roundRobinList.RemoveNode(testServer3);
        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void TestRemoveNodeWithOneItemInList()
    {
        // Arrange
        roundRobinList.AppendList(testServer1);
        var expected = true;
        var expectedMessage = "List is empty";
        // Act
        var actual = roundRobinList.RemoveNode(testServer1);
        // Assert
        Assert.That(actual, Is.EqualTo(expected));
        Assert.That(roundRobinList.ToString, Is.EqualTo(expectedMessage));
    }

    [Test]
    public void TestRemoveNodeWhenNodeIsInTheMiddle()
    {
        // Arrange
        roundRobinList.AppendList(testServer1);
        roundRobinList.AppendList(testServer2);
        roundRobinList.AppendList(testServer3);
        var expected = true;
        var expectedMessage = testServer1.ToString() + testServer3.ToString();
        // Act
        var actual = roundRobinList.RemoveNode(testServer2);
        // Assert
        Assert.That(actual, Is.EqualTo(expected));
        Assert.That(roundRobinList.ToString, Is.EqualTo(expectedMessage));
    }

    [Test]
    public void TestRemoveNodeWhenNodeIsAtTheEnd()
    {
        // Arrange
        roundRobinList.AppendList(testServer1);
        roundRobinList.AppendList(testServer2);
        roundRobinList.AppendList(testServer3);
        var expected = true;
        var expectedMessage = testServer1.ToString() + testServer2.ToString();
        // Act
        var actual = roundRobinList.RemoveNode(testServer3);
        // Assert
        Assert.That(actual, Is.EqualTo(expected));
        Assert.That(roundRobinList.ToString, Is.EqualTo(expectedMessage));
    }

    [Test]
    public void TestPrintNodeConcurrentAccess()
    {
        // Arrange
        roundRobinList.AppendList(testServer1);
        roundRobinList.AppendList(testServer2);
        var expectedToString = testServer1.ToString() + testServer2.ToString();
        var concurrentCalls = 100;

        for (int i = 0; i < concurrentCalls; i++)
        {
            roundRobinList.AppendList(testServer3);
        }
        var tasks = new List<Task<bool>>();
        // Act
        for (int i = 0; i < concurrentCalls; i++)
        {
            tasks.Add(Task.Run(() =>
            {
                return roundRobinList.RemoveNode(testServer3);
            }));
        }
        Task.WaitAll(tasks.ToArray());
        var results = tasks.Select(t => t.Result).ToList();
        var actualToString = roundRobinList.ToString();
        // Assert
        Assert.That(results.Count, Is.EqualTo(concurrentCalls));
        Assert.That(results.All(r => r == true), Is.True);
        Assert.That(actualToString, Is.EqualTo(expectedToString));
    }
    #endregion
}


