using OperatingSystem = EducativeIo.Diy.OperatingSystem;

namespace __TESTS__;

[TestClass]
public class OperatingSystemTests
{
    [TestMethod]
    public void PossibleSpaceAllocation()
    {
        Assert.AreEqual(4, new OperatingSystem().PossibleSpaceAllocation([1, 2, 1, 2, 1], 3));
        Assert.AreEqual(4,
            new OperatingSystem().PossibleSpaceAllocation([1, 1, 2, 1, 1, 4, 4, 2], 2));
    }

    [TestMethod]
    public void GetMissingProcessId()
    {
        Assert.AreEqual(11, new OperatingSystem().GetMissingProcessId([5, 7, 9, 10, 13], 3));
        Assert.AreEqual(12, new OperatingSystem().GetMissingProcessId([5, 7, 9, 10, 13], 4));
        Assert.AreEqual(6, new OperatingSystem().GetMissingProcessId([5, 7, 9, 10, 13], 1));
    }
}