using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace com.strandgenomics.cube.dataset.Tests
{
    [TestClass()]
    public class ColumnMetaDataTests
    {
        private static ColumnMetaData colMetaData;
        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void SetUp(TestContext testContext)
        {
            colMetaData = new ColumnMetaData();
            colMetaData.SetSource("Strand");
            colMetaData.SetMark("Dataset");
        }

        [TestMethod()]
        public void GetSourceTest() => Assert.AreEqual("Strand", colMetaData.GetSource());

        [TestMethod()]
        public void GetMarkTest() => Assert.AreEqual("Dataset", colMetaData.GetMark());

        [TestMethod()]
        public void IsMarkedTest() => Assert.IsTrue(colMetaData.IsMarked());

        [TestMethod()]
        public void GetStateTest() => Assert.IsTrue(colMetaData.GetState().Count() == 0);

        [TestCleanup]
        public void TestCleanUp() => TestContext.WriteLine(TestContext.TestName + " has " + TestContext.CurrentTestOutcome.ToString());

        [ClassCleanup]
        public static void CleanUp() => colMetaData = null;
    }
}