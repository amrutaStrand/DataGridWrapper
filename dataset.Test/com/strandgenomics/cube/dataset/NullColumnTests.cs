using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace com.strandgenomics.cube.dataset.Tests
{
    [TestClass()]
    public class NullColumnTests
    {
        private static NullColumn nullCol;
        public TestContext TestContext
        {
            get;
            set;
        }

        [ClassInitialize]
        public static void SetUp(TestContext testContext)
        {
            nullCol = new NullColumn("nullCol");
        }

        [TestMethod()]
        public void GetTest()
        {
            object actual = nullCol.Get(45);
            string expected = String.Empty;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod()]
        public void GetCategoryCountTest()
        {
            int actual = nullCol.GetCategoryCount();
            int expected = int.MaxValue;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod()]
        public void GetCategoryIndexTest()
        {
            int actual = nullCol.GetCategoryIndex(45);
            int expected = int.MaxValue;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod()]
        public void GetCategorySizeTest()
        {
            int actual = nullCol.GetCategorySize(45);
            int expected = int.MaxValue;
            Assert.AreEqual(actual, expected);
        }

        [TestMethod()]
        public void GetCategoryValueTest()
        {
            object actual = nullCol.GetCategoryValue(45);
            Assert.IsNull(actual);
        }

        [TestMethod()]
        public void GetComparableTest()
        {
            object actual = nullCol.GetComparable(45);
            Assert.IsNull(actual);
        }

        [TestMethod()]
        public void GetDatatypeTest()
        {
            object actual = nullCol.GetDatatype();
            string expected = "string";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetFloatTest()
        {
            float actual = nullCol.GetFloat(45);
            float expected = float.MaxValue;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetIntTest()
        {
            int actual = nullCol.GetInt(45);
            int expected = int.MaxValue;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetMaxIndexTest()
        {
            int actual = nullCol.GetMaxIndex();
            int expected = int.MaxValue;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetMetaDataTest()
        {
            Assert.IsNull(null);
        }

        [TestMethod()]
        public void GetMinIndexTest()
        {
            int actual = nullCol.GetMinIndex();
            int expected = -1;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetMissingValueCountTest()
        {
            int actual = nullCol.GetMissingValueCount();
            int expected = int.MaxValue;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetMissingValueIndicesTest()
        {
            //IntSet actual = nullCol.GetMissingValueIndices();
            Assert.IsNull(null);
        }

        [TestMethod()]
        public void GetNameTest()
        {
            string actual = nullCol.GetName();
            string expected = "nullCol";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetNumericValueTest()
        {
            float actual = nullCol.GetNumericValue(45);
            float expected = float.MaxValue;
            Assert.AreEqual(expected, actual);
        }

        //Intset suppose to be coming from framework, No implementation in the class files
        [TestMethod()]
        public void GetRowIndicesInRangeTest()
        {
            Assert.IsNull(null);
        }

        [TestMethod()]
        public void GetRowIndicesInRangeTest1()
        {
            Assert.IsNull(null);
        }

        [TestMethod()]
        public void GetRowIndicesInSortedOrderTest()
        {
            Assert.IsNull(null);
        }

        [TestMethod()]
        public void GetRowIndicesOfCategoryTest()
        {
            Assert.IsNull(null);
        }

        [TestMethod()]
        public void GetSizeTest()
        {
            int actual = nullCol.GetSize();
            int expected = -1;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetSumTest()
        {
            float actual = nullCol.GetSum();
            float expected = float.MaxValue;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void IsCategoricalTest()
        {
            bool actual = nullCol.IsCategorical();
            Assert.IsFalse(actual);
        }

        [TestMethod()]
        public void IsMissingValueTest()
        {
            bool actual = nullCol.IsMissingValue(45);
            Assert.IsTrue(actual);
        }

        [TestMethod()]
        public void SetCategoricalTest()
        {
            //Assert.Fail();
        }

        [TestMethod()]
        public void SetMetaDataTest()
        {
            //Assert.Fail();
        }

        [TestMethod()]
        public void SetNameTest()
        {
            nullCol.SetName("nCol");
            string expected = "nCol";
            Assert.AreEqual(expected, nullCol.GetName());
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            TestContext.WriteLine(TestContext.TestName + " has " + TestContext.CurrentTestOutcome.ToString());
        }

        [ClassCleanup]
        public static void CleanUp()
        {
            nullCol = null;
        }
    }
}