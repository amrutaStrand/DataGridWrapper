using Microsoft.VisualStudio.TestTools.UnitTesting;
using DeepEqual.Syntax;

namespace com.strandgenomics.cube.dataset.Tests
{
    [TestClass()]
    public class ColumnOperationsTests
    {
        public TestContext TestContext
        {
            get;
            set;
        }
        [TestMethod()]
        public void LogTest()
        {
            float[] data = new float[] { 1, 2, 4, 8, 16 };
            FloatColumn iCol = new FloatColumn("Column", data);
            IColumn actual = new FloatColumn("actual", new float[] { 0, 1, 2, 3, 4 });
            //Act
            IColumn expected = ColumnOperations.Log(iCol, 2f, "logCol");
            //Assert
            bool expectedFlag = actual.IsDeepEqual(expected);
            Assert.IsTrue(expectedFlag);
        }

        [TestMethod()]
        public void GeometricMeanTest()
        {

        }

        [TestMethod()]
        public void ExponentTest()
        {
            float[] data = new float[] { 0, 1, 2, 3, 4 };
            FloatColumn iCol = new FloatColumn("Column", data);
            IColumn actual = new FloatColumn("actual", new float[] { 1, 2, 4, 8, 16 });
            //Act
            IColumn expected = ColumnOperations.Exponent(iCol, 2f, "expCol");
            //Assert
            bool expectedFlag = actual.IsDeepEqual(expected);
            Assert.IsTrue(expectedFlag);
        }

        [TestMethod()]
        public void AbsoluteTest()
        {

        }

        [TestMethod()]
        public void ScaleTest()
        {

        }

        [TestMethod()]
        public void ShiftTest()
        {

        }

        [TestMethod()]
        public void ThresholdTest()
        {

        }

        [TestMethod()]
        public void PowTest()
        {

        }

        [TestMethod()]
        public void PowTest1()
        {

        }

        [TestMethod()]
        public void ConcatTest()
        {

        }

        [TestMethod()]
        public void ConcatTest1()
        {

        }

        [TestMethod()]
        public void ConcatTest2()
        {

        }

        [TestMethod()]
        public void BinTest()
        {

        }

        [TestMethod()]
        public void MeanTest()
        {
            //Arrange
            float[] data = new float[] { 1, 2, 3, 4, 5 };
            FloatColumn iCol = new FloatColumn("Column", data);
            float actual = 3.0f;
            //Act
            float expected = ColumnOperations.Mean(iCol);
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void MedianTest()
        {
            //Arrange
            float[] data = new float[] { 1, 2, 3, 4, 5 };
            FloatColumn iCol = new FloatColumn("Column", data);
            float actual = 3.0f;
            //Act
            float expected = ColumnOperations.Median(iCol);
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void StandardDeviationTest()
        {

        }

        [TestMethod()]
        public void StandardDeviationTest1()
        {

        }

        [TestMethod()]
        public void ComputeNthPercentileTest()
        {

        }

        [TestMethod()]
        public void MedianOfColumnsTest()
        {

        }

        [TestMethod()]
        public void MeanOfColumnsTest()
        {

        }

        [TestMethod()]
        public void MaxOfColumnsTest()
        {

        }

        [TestMethod()]
        public void AverageTest()
        {

        }

        [TestMethod()]
        public void LtTest()
        {

        }

        [TestMethod()]
        public void LeTest()
        {

        }

        [TestMethod()]
        public void EqTest()
        {

        }

        [TestMethod()]
        public void NeTest()
        {

        }

        [TestMethod()]
        public void GtTest()
        {

        }

        [TestMethod()]
        public void GeTest()
        {

        }

        [TestMethod()]
        public void LtTest1()
        {

        }

        [TestMethod()]
        public void LeTest1()
        {

        }

        [TestMethod()]
        public void EqTest1()
        {

        }

        [TestMethod()]
        public void NeTest1()
        {

        }

        [TestMethod()]
        public void GtTest1()
        {

        }

        [TestMethod()]
        public void GeTest1()
        {

        }

        //private bool IsDeepEqual(IColumn actual, IColumn expected)
        //{
        //    for(int i=0; i<actual.GetSize(); i++)
        //    {
        //        var at = actual.Get(i);
        //        var et = expected.Get(i);
        //        if (!at.ToString().Equals(et.ToString()))
        //            return false;
        //    }
        //    return true;
        //}

        //[TestMethod]
        //public void GetMedian_PrivateMethod()
        //{
        //    PrivateType p = new PrivateType(typeof(ColumnOperations));
        //    Type[] types = new Type[] { typeof(float[]), typeof(int) };
        //    object[] args = new object[] { new float[]{ 1f, 2f,3f}, 3 };
        //    p.InvokeStatic("GetMedian", parameterTypes:types, args);
        //}

        [TestCleanup]
        public void TestCleanUp()
        {
            TestContext.WriteLine(TestContext.TestName + " has " + TestContext.CurrentTestOutcome.ToString());
        }
    }
}