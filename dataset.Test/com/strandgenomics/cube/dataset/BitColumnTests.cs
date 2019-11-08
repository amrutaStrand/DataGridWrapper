using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;

namespace com.strandgenomics.cube.dataset.Tests
{
    [TestClass()]
    public class BitColumnTests
    {
        public TestContext TestContext
        {
            get;
            set;
        }

        private static BitColumn col;
        [ClassInitialize]
        public static void SetUp(TestContext testContext)
        {
            BitArray ar = new BitArray(2);
            ar[0] = false;
            ar[1] = true;
            col = new BitColumn("bitColumn", ar, 2);
        }

        [TestMethod]
        [Owner("Harika")]
        public void GetTest_Equal()
        {
            Assert.AreEqual(true, col.Get(1), "The value at index 1 {0}", true);
            TestContext.WriteLine("Test1 : Completed");
        }

        [TestMethod]
        [Owner("Harika")]
        public void GetTest_NotEqual()
        {
            Assert.AreNotEqual(true, col.Get(0));
            System.Diagnostics.Debug.WriteLine("Test2 : Completed");
        }
        [TestMethod]
        [Owner("Harika")]
        [ExpectedException(typeof(DataException))]
        public void GetExceptionTest()
        {
            BitArray ar = new BitArray(2);
            try
            {
                BitColumn col = new BitColumn("bitColumn", ar, 3);
            }
            catch (DataException e)
            {
                Assert.AreEqual("Invalid Student Name: Invalid arguments. Cannot create a column with size larger than the provided data", e.Message);
                throw;
            }

        }

        [TestMethod]
        [Owner("Harika")]
        [Ignore]
        public void Test_Init_privateMethod()
        {
            PrivateObject p = new PrivateObject(typeof(BitColumn));
            p.Invoke("Initialize");
        }

        [TestMethod()]
        [Ignore]
        public void GetComparableTest()
        {

        }

        [TestMethod]
        [Owner("Harika")]
        public void GetDatatypeTest()
        {
            string expected = col.GetDatatype();
            string actual = "bitset";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [Owner("Harika")]
        public void GetFloatTest()
        {
            Assert.AreEqual(0.0f, col.GetFloat(0));
        }

        [TestMethod]
        [Owner("Harika")]
        public void GetIntTest()
        {
            Assert.AreEqual(0, col.GetInt(0));
        }

        [TestMethod]
        [Owner("Harika")]
        public void IsCategorical()
        {
            Assert.AreEqual(false, col.IsCategorical());
        }

        [TestMethod()]
        [Owner("Harika")]
        public void GetNumericValueTest()
        {
            Assert.AreEqual(0, col.GetNumericValue(0));
        }

        [TestMethod()]
        [Owner("Harika")]
        public void GetSumTest()
        {
            Assert.AreEqual(1, col.GetSum());
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            TestContext.WriteLine(TestContext.TestName + " has " + TestContext.CurrentTestOutcome.ToString());
        }

        [ClassCleanup]
        public static void CleanUp()
        {
            col = null;
        }
    }
}