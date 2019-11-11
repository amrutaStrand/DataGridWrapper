using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.strandgenomics.cube.dataset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.strandgenomics.cube.dataset.Tests
{
    [TestClass()]
    public class DateColumnTests
    {
        private static DateColumn dateCol;
        public TestContext TestContext
        {
            get;
            set;
        }

        [ClassInitialize]
        public static void SetUp(TestContext testContext)
        {
            long[] data = new long[] { 1, 2, 5 };
            dateCol = new DateColumn("DateCol", data);
        }

        [TestMethod()]
        public void GetTest()
        {
            Assert.AreEqual(dateCol.Get(0), new DateTime(1));
        }

        [TestMethod()]
        public void GetDatatypeTest()
        {
            Assert.AreEqual(dateCol.GetDatatype(), DateColumn.DATATYPE);
        }

        [TestMethod()]
        [ExpectedException(typeof(DataException))]
        public void GetSumTest()
        {
            Boolean isCategorical = dateCol.IsCategorical();
            
            try
            {
                dateCol.GetSum();
            }catch(Exception ex)
            {
                string actual = ex.Message;
                string expected = "Invalid Student Name: Method invoked on the column is not compatible with its state. Categorical column = True";
                StringAssert.Equals(expected, actual);
                throw;
            }
        }

        [TestMethod()]
        public void GetFloatTest()
        {
            float actual = dateCol.GetFloat(1);
            Assert.AreEqual(2, actual);
        }

        [TestMethod()]
        public void GetIntTest()
        {
            int actual = dateCol.GetInt(1);
            Assert.AreEqual(2, actual);
        }

        [TestMethod()]
        public void GetNumericValueTest()
        {
            float actual = dateCol.GetNumericValue(1);
            Assert.AreEqual(1, actual);
        }
        [TestCleanup]
        public void TestCleanUp()
        {
            TestContext.WriteLine(TestContext.TestName + " has " + TestContext.CurrentTestOutcome.ToString());
        }

        [ClassCleanup]
        public static void CleanUp()
        {
            dateCol = null;
        }
    }
}