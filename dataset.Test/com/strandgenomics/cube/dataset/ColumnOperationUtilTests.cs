using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace com.strandgenomics.cube.dataset.Tests
{
    [TestClass()]
    public class ColumnOperationUtilTests
    {
        public TestContext TestContext
        {
            get;
            set;
        }

        [TestMethod]
        public void TestSafeEquals()
        {
            PrivateType p = new PrivateType(typeof(ColumnOperationUtil));
            Type[] types = new Type[] { typeof(object), typeof(object) };
            object[] args = new object[] { 4, 4f };
            p.InvokeStatic("SafeEquals", parameterTypes: types, args);
        }

        [TestMethod()]
        public void UpdateMetaDataTest()
        {

        }

        [TestMethod()]
        public void UpdateMetaDataTest1()
        {

        }

        [TestCleanup]
        public void TestCleanUp()
        {
            TestContext.WriteLine(TestContext.TestName + " has " + TestContext.CurrentTestOutcome.ToString());
        }
    }
}