using Microsoft.VisualStudio.TestTools.UnitTesting;
using dataset.Test.com.strandgenomics.cube.dataset;
using System;

namespace com.strandgenomics.cube.dataset.Tests
{
    [TestClass()]
    public class BasicColumnOperationsTests
    {
        private static IntColumn intCol1;
        private static IntColumn intCol2;
        private static FloatColumn floatCol1;
        private static FloatColumn floatCol2;
        private IColumn actual = null;
        private IColumn expected = null;
        public TestContext TestContext
        {
            get;
            set;
        }

        [ClassInitialize]
        public static void SetUp(TestContext testContext)
        {
            int[] intData = new int[] { 1, 2, 3, 4};
            intCol1 = new IntColumn("Col1", intData);
            intCol2 = new IntColumn("Col1", intData);

            float[] floatData = new float[] { 1f, 2f, 3f, 4f};
            floatCol1 = new FloatColumn("Col2", floatData);
            floatCol2 = new FloatColumn("Col3", floatData);
        }
        
        [TestMethod()]
        public void AddTest_IntCols()
        {
            actual = BasicColumnOperations.Add(intCol1, intCol2, null);
            expected = new IntColumn("expectedRes", new int[] { 2, 4, 6, 8 });
            
            Assert.IsTrue(TestUtils.IsDeepEqual(actual, expected));
        }
        [TestMethod()]
        public void AddTest_FloatCols()
        {
            actual = BasicColumnOperations.Add(floatCol1, floatCol2, null);
            expected = new FloatColumn("expectedRes", new float[] { 2f, 4f, 6f, 8f });

            Assert.IsTrue(TestUtils.IsDeepEqual(actual, expected));
        }

        [TestMethod()]
        public void AddTest_ICols_WithName()
        {
            actual = BasicColumnOperations.Add(intCol1, floatCol2, "ResCol");
            expected = new FloatColumn("expectedRes", new float[] { 2f, 4f, 6f, 8f });

            Assert.IsTrue(TestUtils.IsDeepEqual(actual, expected));
        }

        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void AddTest_NotNUmericCols()
        {
            char[][] data = new char[2][];
            data[0] = new char[] { 'h', 'b' };
            data[1] = new char[] { 's', 'k' };
            try
            {
                actual = BasicColumnOperations.Add(new StringColumn("strCol", data), floatCol2, null); //_sortOrder Initialization in AbstractRegularColumn.cs
            }catch(Exception ex)
            {
                Assert.AreEqual("operation can be performed only on int and float columns.", ex.Message);
                throw;
            }
            
        }
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void AddTest_ColAndObject_WithName()
        {
            try
            {
                actual = BasicColumnOperations.Add(intCol1, 's', "col3");
            }catch(Exception ex)
            {
                Assert.AreEqual("+ operation can be performed only with numbers.", ex.Message);
                throw;
            }
            
        }

        [TestMethod()]
        public void AddTest_IntColAndIntVal()
        {
            actual = BasicColumnOperations.Add(intCol1, 1, "col3");
            expected = new IntColumn("expectedRes", new int[] { 2, 3, 4, 5 });
            Assert.IsTrue(TestUtils.IsDeepEqual(actual, expected));
        }

        [TestMethod()]
        public void AddTest_FloatColAndFloatVal()
        {
            actual = BasicColumnOperations.Add(floatCol1, 1f, "col3");
            expected = new FloatColumn("expectedRes", new float[] { 2f, 3f, 4f, 5f });
            Assert.IsTrue(TestUtils.IsDeepEqual(actual, expected));
        }

        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void AddTest_IrrelevantParams()
        {
            try
            {
                int x = 1;
                actual = BasicColumnOperations.Add(floatCol1, x, "col3");
            }
            catch (Exception ex)
            {
                Assert.AreEqual("huh?", ex.Message);
                throw;
            }
        }

        [TestMethod()]
        public void SubTest_IntCols()
        {
            actual = BasicColumnOperations.Sub(intCol1, intCol2, null);
            expected = new IntColumn("expectedRes", new int[] { 0, 0, 0, 0 });

            Assert.IsTrue(TestUtils.IsDeepEqual(actual, expected));
        }
        [TestMethod()]
        public void SubTest_FloatCols()
        {
            actual = BasicColumnOperations.Sub(floatCol1, floatCol2, null);
            expected = new FloatColumn("expectedRes", new float[] { 0f, 0f, 0f, 0f });

            Assert.IsTrue(TestUtils.IsDeepEqual(actual, expected));
        }

        [TestMethod()]
        public void SubTest_ICols_WithName()
        {
            actual = BasicColumnOperations.Sub(intCol1, floatCol2, "ResCol");
            expected = new FloatColumn("expectedRes", new float[] { 0f, 0f, 0f, 0f });

            Assert.IsTrue(TestUtils.IsDeepEqual(actual, expected));
        }

        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void SubTest_NotNUmericCols()
        {
            char[][] data = new char[2][];
            data[0] = new char[] { 'h', 'b' };
            data[1] = new char[] { 's', 'k' };
            try
            {
                actual = BasicColumnOperations.Sub(new StringColumn("strCol", data), floatCol2, null); //_sortOrder Initialization in AbstractRegularColumn.cs
            }
            catch (Exception ex)
            {
                Assert.AreEqual("- operation can be performed only on int and float columns.", ex.Message);
                throw;
            }

        }
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void SubTest_ColAndObject_WithName()
        {
            try
            {
                actual = BasicColumnOperations.Sub(intCol1, 's', "col3");
            }
            catch (Exception ex)
            {
                Assert.AreEqual("- operation can be performed only with numbers.", ex.Message);
                throw;
            }

        }

        [TestMethod()]
        public void SubTest_IntColAndIntVal()
        {
            actual = BasicColumnOperations.Sub(intCol1, 1, "col3");
            expected = new IntColumn("expectedRes", new int[] { 0, 1, 2, 3 });
            Assert.IsTrue(TestUtils.IsDeepEqual(actual, expected));
        }

        [TestMethod()]
        public void SubTest_FloatColAndFloatVal()
        {
            actual = BasicColumnOperations.Sub(floatCol1, 1f, "col3");
            expected = new FloatColumn("expectedRes", new float[] { 0f, 1f, 2f, 3f });
            Assert.IsTrue(TestUtils.IsDeepEqual(actual, expected));
        }

        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void SubTest_IrrelevantParams()
        {
            try
            {
                int x = 1;
                actual = BasicColumnOperations.Sub(floatCol1, x, "col3");
            }
            catch (Exception ex)
            {
                Assert.AreEqual("huh?", ex.Message);
                throw;
            }
        }

        [TestMethod()]
        public void MulTest_IntCols()
        {
            actual = BasicColumnOperations.Mul(intCol1, intCol2, null);
            expected = new IntColumn("expectedRes", new int[] { 1, 4, 9, 16 });

            Assert.IsTrue(TestUtils.IsDeepEqual(actual, expected));
        }
        [TestMethod()]
        public void MulTest_FloatCols()
        {
            actual = BasicColumnOperations.Mul(floatCol1, floatCol2, null);
            expected = new FloatColumn("expectedRes", new float[] { 1f, 4f, 9f, 16f });

            Assert.IsTrue(TestUtils.IsDeepEqual(actual, expected));
        }

        [TestMethod()]
        public void MulTest_ICols_WithName()
        {
            actual = BasicColumnOperations.Mul(intCol1, floatCol2, "ResCol");
            expected = new FloatColumn("expectedRes", new float[] { 1f, 4f, 9f, 16f });

            Assert.IsTrue(TestUtils.IsDeepEqual(actual, expected));
        }

        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void MulTest_NotNUmericCols()
        {
            char[][] data = new char[2][];
            data[0] = new char[] { 'h', 'b' };
            data[1] = new char[] { 's', 'k' };
            try
            {
                actual = BasicColumnOperations.Mul(new StringColumn("strCol", data), floatCol2, null); //_sortOrder Initialization in AbstractRegularColumn.cs
            }
            catch (Exception ex)
            {
                Assert.AreEqual("* operation can be performed only on int and float columns.", ex.Message);
                throw;
            }

        }
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void MulTest_ColAndObject_WithName()
        {
            try
            {
                actual = BasicColumnOperations.Mul(intCol1, 's', "col3");
            }
            catch (Exception ex)
            {
                Assert.AreEqual("* operation can be performed only with numbers.", ex.Message);
                throw;
            }

        }

        [TestMethod()]
        public void MulTest_IntColAndIntVal()
        {
            actual = BasicColumnOperations.Mul(intCol1, 1, "col3");
            expected = new IntColumn("expectedRes", new int[] { 1, 2, 3, 4 });
            Assert.IsTrue(TestUtils.IsDeepEqual(actual, expected));
        }

        [TestMethod()]
        public void MulTest_FloatColAndFloatVal()
        {
            actual = BasicColumnOperations.Mul(floatCol1, 1f, "col3");
            expected = new FloatColumn("expectedRes", new float[] { 1f, 2f, 3f, 4f });
            Assert.IsTrue(TestUtils.IsDeepEqual(actual, expected));
        }

        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void MulTest_IrrelevantParams()
        {
            try
            {
                int x = 1;
                actual = BasicColumnOperations.Mul(floatCol1, x, "col3");
            }
            catch (Exception ex)
            {
                Assert.AreEqual("huh?", ex.Message);
                throw;
            }
        }

        [TestMethod()]
        public void DivTest_IntCols()
        {
            actual = BasicColumnOperations.Div(intCol1, intCol2, null);
            expected = new IntColumn("expectedRes", new int[] { 1, 1, 1, 1 });

            Assert.IsTrue(TestUtils.IsDeepEqual(actual, expected));
        }
        [TestMethod()]
        public void DivTest_FloatCols()
        {
            actual = BasicColumnOperations.Div(floatCol1, floatCol2, null);
            expected = new FloatColumn("expectedRes", new float[] { 1f, 1f, 1f, 1f });

            Assert.IsTrue(TestUtils.IsDeepEqual(actual, expected));
        }

        [TestMethod()]
        public void DivTest_ICols_WithName()
        {
            actual = BasicColumnOperations.Div(intCol1, floatCol2, "ResCol");
            expected = new FloatColumn("expectedRes", new float[] { 1f, 1f, 1f, 1f });

            Assert.IsTrue(TestUtils.IsDeepEqual(actual, expected));
        }

        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void DivTest_NotNUmericCols()
        {
            char[][] data = new char[2][];
            data[0] = new char[] { 'h', 'b' };
            data[1] = new char[] { 's', 'k' };
            try
            {
                actual = BasicColumnOperations.Div(new StringColumn("strCol", data), floatCol2, null); //_sortOrder Initialization in AbstractRegularColumn.cs
            }
            catch (Exception ex)
            {
                Assert.AreEqual("/ operation can be performed only on int and float columns.", ex.Message);
                throw;
            }

        }
        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void DivTest_ColAndObject_WithName()
        {
            try
            {
                actual = BasicColumnOperations.Div(intCol1, 's', "col3");
            }
            catch (Exception ex)
            {
                Assert.AreEqual("/ operation can be performed only with numbers.", ex.Message);
                throw;
            }

        }

        [TestMethod()]
        public void DivTest_IntColAndIntVal()
        {
            actual = BasicColumnOperations.Div(intCol1, 1, "col3");
            expected = new IntColumn("expectedRes", new int[] { 1, 2, 3, 4 });
            Assert.IsTrue(TestUtils.IsDeepEqual(actual, expected));
        }

        [TestMethod()]
        public void DivTest_FloatColAndFloatVal()
        {
            actual = BasicColumnOperations.Div(floatCol1, 1f, "col3");
            expected = new FloatColumn("expectedRes", new float[] { 1f, 2f, 3f, 4f });
            Assert.IsTrue(TestUtils.IsDeepEqual(actual, expected));
        }

        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void DivTest_IrrelevantParams()
        {
            try
            {
                int x = 1;
                actual = BasicColumnOperations.Div(floatCol1, x, "col3");
            }
            catch (Exception ex)
            {
                Assert.AreEqual("huh?", ex.Message);
                throw;
            }
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            TestContext.WriteLine(TestContext.TestName + " has " + TestContext.CurrentTestOutcome.ToString());
            actual = null;
            expected = null;
        }

        [ClassCleanup]
        public static void CleanUp()
        {
            intCol1 = null;
            intCol2 = null;
            floatCol1 = null;
            floatCol2 = null;
        }
    }
}