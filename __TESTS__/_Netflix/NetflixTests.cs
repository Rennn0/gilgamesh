using EducativeIo.Projects.Netflix;

namespace __TESTS__._Netflix
{
    [TestClass]
    public class NetflixTests
    {
        [TestMethod]
        public void MedianAge()
        {
            Netflix netflix = new Netflix();
            netflix.InsertAge(10);
            netflix.InsertAge(22);
            netflix.InsertAge(25);
            netflix.InsertAge(30);

            float median = netflix.MedianAge();
            Assert.AreEqual(23.5, median);

            netflix.InsertAge(35);
            median = netflix.MedianAge();
            Assert.AreEqual(25, median);
        }

    }
}