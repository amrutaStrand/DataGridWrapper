using framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.strandgenomics.cube.dataset
{
    /// <summary>
    /// Operations on columns.
    /// </summary>
    sealed public class ColumnOperations:BasicColumnOperations
    {
        public static IColumn Log(IColumn c, float baseNum, string name)
        {

            if (DatasetUtil.IsStringColumn(c))
                return null;

            if (baseNum <= 0)
                return null;

            if (name == null)
                name = "log_ " + c.GetName();

            float scale = (float)Math.Log(baseNum);
            int size = c.GetSize();
            float[] data = new float[size];
            for (int i = 0; i < size; i++)
            {
                float f = c.GetFloat(i);
                if (f <= 0 || c.IsMissingValue(i))
                    data[i] = DatasetConstants.FLOAT_MV;
                else
                    data[i] = (float)Math.Log(f) / scale;
            }

            return CreateFloatColumn(name, data, c);
        }

        public static IColumn GeometricMean(IColumn[] columns)
        {
            if (columns == null)
                return null;

            try
            {
                IColumn[] logCols = new IColumn[columns.Length];
                int i = 0;
                foreach (IColumn col in columns)
                {
                    IColumn logCol = Log(col, 2.0f, col.GetName());
                    logCols[i++] = logCol;
                }

                IColumn average = Average(logCols);

                return Pow(2.0f, average, average.GetName());

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static IColumn Exponent(IColumn c, float baseNum, string name)
        {

            if (DatasetUtil.IsStringColumn(c))
                return null;

            if (name == null)
                name = "exp_" + c.GetName();

            int size = c.GetSize();
            float[] data = new float[size];
            for (int i = 0; i < size; i++)
            {
                float f = c.GetFloat(i);
                if (c.IsMissingValue(i))
                    data[i] = DatasetConstants.FLOAT_MV;
                else
                    data[i] = (float)Math.Pow(baseNum, f);
            }

            return CreateFloatColumn(name, data, c);
        }

        public static IColumn Absolute(IColumn c, string name)
        {

            if (DatasetUtil.IsStringColumn(c))
                return null;

            if (name == null)
                name = "abs_" + c.GetName();

            int size = c.GetSize();
            float[] data = new float[size];
            for (int i = 0; i < size; i++)
            {
                float f = c.GetFloat(i);
                if (c.IsMissingValue(i))
                    data[i] = DatasetConstants.FLOAT_MV;
                else
                    data[i] = Math.Abs(f);
            }

            return CreateFloatColumn(name, data, c);
        }

        public static IColumn Scale(IColumn c, string scaleType, float scaleFactor, string name)
        {

            if (DatasetUtil.IsStringColumn(c))
                return null;

            if (name == null)
                name = "scale_" + c.GetName();

            int size = c.GetSize();
            float[] data = new float[size];
            if (scaleType.Equals("Up"))
            {
                for (int i = 0; i < size; i++)
                {
                    float f = c.GetFloat(i);
                    if (c.IsMissingValue(i))
                        data[i] = DatasetConstants.FLOAT_MV;
                    else
                        data[i] = f * scaleFactor;
                }
            }
            else
            {
                for (int i = 0; i < size; i++)
                {
                    float f = c.GetFloat(i);
                    if (c.IsMissingValue(i))
                        data[i] = DatasetConstants.FLOAT_MV;
                    else
                        data[i] = f / scaleFactor;
                }
            }

            return CreateFloatColumn(name, data, c);
        }


        public static IColumn Shift(IColumn c, string shiftType, float shiftOffset, string name)
        {

            if (DatasetUtil.IsStringColumn(c))
                return null;

            if (name == null)
                name = "shift_" + c.GetName();

            int size = c.GetSize();
            float[] data = new float[size];
            if (shiftType.Equals("Up"))
            {
                for (int i = 0; i < size; i++)
                {
                    float f = c.GetFloat(i);
                    if (c.IsMissingValue(i))
                        data[i] = DatasetConstants.FLOAT_MV;
                    else
                        data[i] = f + shiftOffset;
                }
            }
            else
            {
                for (int i = 0; i < size; i++)
                {
                    float f = c.GetFloat(i);
                    if (c.IsMissingValue(i))
                        data[i] = DatasetConstants.FLOAT_MV;
                    else
                        data[i] = f - shiftOffset;
                }
            }
            return CreateFloatColumn(name, data, c);
        }

        public static IColumn Threshold(IColumn c, float min, float max, string name)
        {
            float MAX_VAL = float.MaxValue;
            float MIN_VAL = (-1.0f) * MAX_VAL;

            if (DatasetUtil.IsStringColumn(c))
                return null;

            if (name == null)
                name = "threshold_" + c.GetName();

            int size = c.GetSize();
            float[] data = new float[size];
            for (int i = 0; i < size; i++)
            {
                float f = c.GetFloat(i);

                if (min > MIN_VAL && f < min)
                    f = min;
                else if (max < MAX_VAL && f > max)
                    f = max;

                data[i] = f;
            }
            return CreateFloatColumn(name, data, c);
        }

        public static IColumn Pow(IColumn c, float a, string name)
        {

            if (DatasetUtil.IsStringColumn(c))
                return null;

            if (name == null)
                name = c.GetName() + " ** " + a;

            int size = c.GetSize();
            float[] data = new float[size];
            for (int i = 0; i < size; i++)
            {
                float f = c.GetFloat(i);

                if (c.IsMissingValue(i))
                    data[i] = DatasetConstants.FLOAT_MV;
                else
                    data[i] = (float)Math.Pow(f, a);
            }

            return CreateFloatColumn(name, data, c);
        }

        public static IColumn Pow(float baseNum, IColumn c, string name)
        {

            if (DatasetUtil.IsStringColumn(c))
                return null;

            if (baseNum <= 0)
                return null;

            if (name == null)
                name = baseNum + " ** " + c.GetName();

            int size = c.GetSize();
            float[] data = new float[size];
            for (int i = 0; i < size; i++)
            {
                float f = c.GetFloat(i);
                if (c.IsMissingValue(i))
                    data[i] = DatasetConstants.FLOAT_MV;
                else
                    data[i] = (float)Math.Pow((double)baseNum, (double)f);
            }

            return CreateFloatColumn(name, data, c);
        }

        /// <summary>
        /// String concatination of two columns.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IColumn Concat(IColumn a, IColumn b, string name)
        {
            return Concat(a, b, name, "");
        }


        /// <summary>
        /// String concatination of a column with a string.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="name"></param>
        /// <param name="sep"></param>
        /// <returns></returns>
        public static IColumn Concat(IColumn a, IColumn b, string name, string sep)
        {

            if (name == null)
                name = a.GetName() + b.GetName();

            int size = a.GetSize();
            char[][] data = new char[size][];

            for (int i = 0; i < size; i++)
            {
                Object x = a.Get(i);
                Object y = b.Get(i);

                if (x != null && y != null)
                {
                    String s = x.ToString() + sep + y.ToString();
                    data[i] = s.ToCharArray();
                }
            }

            return ColumnFactory.CreateStringColumn(name, data);
        }

        /// <summary>
        /// String concatination of a string with a column.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="a"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IColumn Concat(object b, IColumn a, string name)
        {
            if (b == null)
                return a;

            b = b.ToString();

            if (name == null)
                name = a.GetName() + b;

            int size = a.GetSize();
            char[][] data = new char[size][];

            for (int i = 0; i < size; i++)
            {
                Object x = a.Get(i);

                if (x != null)
                {
                    String s = b + x.ToString();
                    data[i] = s.ToCharArray();
                }
            }

            IColumn c1 = ColumnFactory.CreateStringColumn(name, data);
            ColumnOperationUtil.UpdateMetaData(c1, a);
            return c1;
        }

        /// <summary>
        /// Size of binValues is one less than size of binLabels
        /// bin ([0, 50, 100], ["label0", "label1", "label2", "label3"]):
        /// To the specified column, rows whose value < 0 are labeled "label0", 
        /// [0, 50) labeled "label1", [50, 100) labeled "label2" and >= 100 labeled "label3"
        /// </summary>
        /// <param name="c"></param>
        /// <param name="binValues"></param>
        /// <param name="binLabels"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IColumn Bin(IColumn c, float[] binValues, string[] binLabels, string name)
        {

            if (binValues.Length != binLabels.Length - 1)
                return null;

            if (!DatasetUtil.IsIntegerColumn(c) && !DatasetUtil.IsFloatColumn(c))
                return null;

            string[] labels = new string[c.GetSize()];
            IntSet rowIndices = c.GetRowIndicesInSortedOrder(true);  //#framework
            IEnumerator<int> iter = rowIndices.GetEnumerator();

            do
            {
                int index = iter.Current;

                if (c.IsMissingValue(index))
                {
                    labels[index] = null;
                    continue;
                }

                float value = c.GetNumericValue(index);
                if (value < binValues[0])
                    labels[index] = binLabels[0];

                else if (value >= binValues[binValues.Length - 1])
                    labels[index] = binLabels[binLabels.Length - 1];

                else
                {
                    for (int j = 0; j < binValues.Length - 1; j++)
                    {
                        if (value >= binValues[j] && value < binValues[j + 1])
                        {
                            labels[index] = binLabels[j + 1];
                            break;
                        }
                    }
                }
            }
            while (iter.MoveNext());

            if (name == null)
                name = "bin_" + c.GetName();
            return ColumnFactory.CreateStringColumn(name, labels);
        }

        /// <summary>
        /// Returns mean of the specified column.
        /// This works only for numeric column. 
        /// For other colums missing value is returned.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static float Mean(IColumn c)
        {
            int n = c.GetSize() - c.GetMissingValueCount();

            if (!IsNumericColumn(c) || n == 0)
                return DatasetConstants.FLOAT_MV;

            return c.GetSum() / n;
        }

        /// <summary>
        /// Returns median of the specified column.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static float Median(IColumn c)
        {
            return ComputeNthPercentile(c, 50);
        }


        /// <summary>
        /// Returns the standard deviation for the data in this column. Returns 0 if there is only single value in column.
        /// </summary>
        /// <param name="c">for which standard deviation is required </param>
        /// <returns>standard deviation of the input column</returns>
        public static float StandardDeviation(IColumn c)
        {

            if (!IsNumericColumn(c))
                return DatasetConstants.FLOAT_MV;

            float mean = Mean(c);

            IColumn c1 = Sub(c, mean, c.GetName());
            IColumn c2 = Mul(c1, c1, c1.GetName());
            int n = c2.GetSize() - c2.GetMissingValueCount();

            if (n == 0)
                throw new Exception("All entries in column are missing values.");

            float columnSum = c2.GetSum();

            if (columnSum == 0.0f || n == 1)
                return 0.0f;

            return (float)Math.Sqrt(1.0 * columnSum / (n - 1));

        }

        /// <summary>
        /// Returns a column with standard deviation across each row in input columns
        /// </summary>
        /// <param name="cols">for which across row standard deviation is required</param>
        /// <returns>IColumn with value in row i corresponding to standard deviation of values in row i of argument columns</returns>
        public static IColumn StandardDeviation(IColumn[] cols)
        {

            IDataset transposedDataset = DatasetUtil.Transpose(DatasetFactory.CreateDataset("TransposeDataset", cols));

            int columnCount = transposedDataset.GetColumnCount();
            float[] stdDev = new float[columnCount];
            for (int i = 0; i < columnCount; i++)
            {
                IColumn col = transposedDataset.GetColumn(i);
                stdDev[i] = StandardDeviation(col);
            }

            return ColumnFactory.CreateFloatColumn("Standard Deviation", stdDev);
        }

        public static float ComputeNthPercentile(IColumn c, float N)
        {

            int n = c.GetSize() - c.GetMissingValueCount();

            if (!IsNumericColumn(c) || n == 0)
                return DatasetConstants.FLOAT_MV;

            //float[] colData = ArrayUtil.getContents(DatasetUtil.GetColumnData(c)); //#framework
            //Arrays.computeSortOrder(colData);
            float[] colData = { };
            if (N == 100.0)
                return colData[colData.Length - 1];
            else
            {
                float position = ((n + 1) * N / 100) - 1;
                int index = (int)Math.Floor(position);
                float remainder = position - index;
                int nextIndex = index + 1;
                if (index == -1)
                    index = 0;
                if (nextIndex == n)
                    nextIndex = n - 1;
                float result = ((1 - remainder) * colData[index]) + (remainder * colData[nextIndex]);
                return result;
            }
        }

        public static IColumn MedianOfColumns(IColumn[] cols)
        {
            int n = cols[0].GetSize();
            float[] median = new float[n];
            for (int i = 0; i < n; i++)
            {
                float[] row = new float[cols.Length];
                int missingCount = 0;
                for (int j = 0; j < cols.Length; j++)
                {
                    row[j] = cols[j].GetFloat(i);
                    if (cols[j].IsMissingValue(i))
                        missingCount = missingCount + 1;
                }
                median[i] = GetMedian(row, missingCount);
            }
            IColumn c = ColumnFactory.CreateFloatColumn("Median(" + cols.Length + ")", median);
            c.GetMetaData().SetMark(cols[0].GetMetaData().GetMark());
            return c;
        }

        public static IColumn MeanOfColumns(IColumn[] cols)
        {
            int n = cols[0].GetSize();
            float[] mean = new float[n];
            for (int i = 0; i < n; i++)
            {
                float sum = 0;
                int missingCount = 0;
                for (int j = 0; j < cols.Length; j++)
                {
                    if (cols[j].IsMissingValue(i))
                        missingCount = missingCount + 1;
                    else
                        sum = sum + cols[j].GetFloat(i);

                }
                if (missingCount >= n)
                    mean[i] = FLOAT_MV;
                else
                    mean[i] = sum / (cols.Length - missingCount);
            }
            IColumn c = ColumnFactory.CreateFloatColumn("Mean(" + cols.Length + ")", mean);
            c.GetMetaData().SetMark(cols[0].GetMetaData().GetMark());
            return c;
        }

        public static IColumn MaxOfColumns(IColumn[] cols)
        {
            int n = cols[0].GetSize();
            float[] max = new float[n];
            for (int i = 0; i < n; i++)
            {
                float[] row = new float[cols.Length];
                int missingCount = 0;
                for (int j = 0; j < cols.Length; j++)
                {
                    row[j] = cols[j].GetFloat(i);
                    if (cols[j].IsMissingValue(i))
                        missingCount = missingCount + 1;
                }
                max[i] = GetMax(row, missingCount);
            }
            IColumn c = ColumnFactory.CreateFloatColumn("Max(" + cols.Length + ")", max);
            c.GetMetaData().SetMark(cols[0].GetMetaData().GetMark());
            return c;
        }

        private static float GetMax(float[] data, int missingCount)
        {
            int[] order = { }; /*Arrays.computeSortOrder(data);*/ //#framework
            int n = data.Length - missingCount;
            if (n == 0)
                throw new Exception();
            else
                return data[order[n - 1]];
        }


        private static float GetMedian(float[] data, int missingCount)
        {
            //#framework
            //Arrays.computeSortOrder(data);
            int n = data.Length - missingCount;
            if (n <= 0)
                return FLOAT_MV;
            if (n % 2 == 0)
            {
                float x = data[n / 2 - 1];
                float y = data[n / 2];
                return (x + y) / 2;
            }
            else
                return data[n / 2];
        }

        /// <summary>
        /// skips first n items from the specified iterator.
        /// </summary>
        /// <param name="iter"></param>
        /// <param name="n"></param>
        private static void SkipItems(IEnumerator<int> iter, int n) //#framework
        {

            for (int i = 0; i < n && iter.MoveNext(); i++)
            {
                //dont need to do anything i think since every time condition is checked 
                //it automatically moves the iterator to the next step.
            }
        }

        public static IColumn Average(IColumn[] columns)
        {

            if (columns == null || columns.Length == 0)
                return null;
            int rowCount = columns[0].GetSize();
            int columnCount = columns.Length;
            float[] newColumnArray = new float[rowCount];
            string name = "avg_";
            for (int j = 0; j < columnCount; j++)
            {
                name = name + "+" + columns[j].GetName();
            }

            // decide if mark has to be updated
            ColumnMetaData p = columns[0].GetMetaData();
            bool updateMark = true, updateSource = true;
            if (p.GetSource() == null)
                updateSource = false;
            if (p.GetMark() == null)
                updateMark = false;
            for (int i = 1; i < columnCount; i++)
            {
                ColumnMetaData q = columns[i].GetMetaData();
                if (updateSource)
                    if (!p.GetSource().Equals(q.GetSource()))
                        updateSource = false;
                if (updateMark)
                    if (!p.GetMark().Equals(q.GetMark()))
                        updateMark = false;
            }

            for (int i = 0; i < rowCount; i++)
            {
                int denominator = 0;
                float sum = 0;
                for (int j = 0; j < columnCount; j++)
                {
                    if (columns[j].IsMissingValue(i))
                    {
                        continue; // ignore missing values while computing average
                    }
                    float value = (float)columns[j].GetNumericValue(i);
                    denominator++; // takes only non missing values for averaging
                    sum += value;
                }
                if (denominator > 0)
                    newColumnArray[i] = sum / denominator;
                else
                    newColumnArray[i] = DatasetConstants.FLOAT_MV;
            }
            IColumn retColumn = ColumnFactory.CreateFloatColumn(name, newColumnArray);
            if (updateSource)
                retColumn.GetMetaData().SetSource(p.GetSource());
            else
                retColumn.GetMetaData().SetSource(null);
            if (updateMark)
                retColumn.GetMetaData().SetMark(p.GetMark());
            else
                retColumn.GetMetaData().SetMark(null);
            return retColumn;
            /*
        IColumn average = columns[0];
            for (int i=1; i<columns.length; i++)
            average = add (average, columns[i], null);

            IColumn retval = scale (average, "down", columns.length, null);
        retval.getMetaData().setSource (null);
        return retval;
             */
        }


        public static int[] Lt(IColumn a, IColumn b) { return Compare(a, b, SafeComparator.LT); }
        public static int[] Le(IColumn a, IColumn b) { return Compare(a, b, SafeComparator.LE); }
        public static int[] Eq(IColumn a, IColumn b) { return Compare(a, b, SafeComparator.EQ); }
        public static int[] Ne(IColumn a, IColumn b) { return Compare(a, b, SafeComparator.NE); }
        public static int[] Gt(IColumn a, IColumn b) { return Compare(a, b, SafeComparator.GT); }
        public static int[] Ge(IColumn a, IColumn b) { return Compare(a, b, SafeComparator.GE); }

        public static int[] Lt(IColumn a, object b) { return Compare(a, b, SafeComparator.LT); }
        public static int[] Le(IColumn a, object b) { return Compare(a, b, SafeComparator.LE); }
        public static int[] Eq(IColumn a, object b) { return Compare(a, b, SafeComparator.EQ); }
        public static int[] Ne(IColumn a, object b) { return Compare(a, b, SafeComparator.NE); }
        public static int[] Gt(IColumn a, object b) { return Compare(a, b, SafeComparator.GT); }
        public static int[] Ge(IColumn a, object b) { return Compare(a, b, SafeComparator.GE); }

        private static object CoerceObject(IColumn column, object value)
        {
            if (DatasetUtil.IsFloatColumn(column) && (value is int || value is float || value is double)) {
                float n = (float)value;
                return n;
            }
        else if (DatasetUtil.IsIntegerColumn(column) && (value is int || value is float || value is double)) {
                int n = (int)value;
                return n;
            }
            return value;
        }

        private static int[] Compare(IColumn c, object value, SafeComparator comparator)
        {
            int[] result = new int[c.GetSize()];

            // float column can be compared with int and vice-versa.
            value = CoerceObject(c, value);

            for (int i = 0; i < result.Length; i++)
                result[i] = comparator.Compare(c.Get(i), value) ? 1 : 0;

            return result;
        }

        private static int[] Compare(IColumn a, IColumn b, SafeComparator comparator)
        {
            // assert a.getSize() == b.getSize()

            int[] result = new int[a.GetSize()];

            //XXX: what if one is int column and other is float column??
            for (int i = 0; i < result.Length; i++)
                result[i] = comparator.Compare(a.Get(i), b.Get(i)) ? 1 : 0;

            return result;
        }

    }

    internal  class SafeComparator
    {
        private const int TYPE_LT = 0;
        private const int TYPE_LE = 1;
        private const int TYPE_EQ = 2;
        private const int TYPE_NE = 3;
        private const int TYPE_GT = 4;
        private const int TYPE_GE = 5;

        public static  SafeComparator LT = new SafeComparator(TYPE_LT);
        public static  SafeComparator LE = new SafeComparator(TYPE_LE);
        public static  SafeComparator EQ = new SafeComparator(TYPE_EQ);
        public static  SafeComparator NE = new SafeComparator(TYPE_NE);
        public static  SafeComparator GT = new SafeComparator(TYPE_GT);
        public static  SafeComparator GE = new SafeComparator(TYPE_GE);

        int type;

        public SafeComparator(int type)
        {
            this.type = type;
        }

        public bool Compare(Object a, Object b)
        {
            int diff = GetDiff(a, b);

            switch (type)
            {
                case TYPE_LT: return diff < 0;
                case TYPE_LE: return diff <= 0;
                case TYPE_EQ: return diff == 0;
                case TYPE_NE: return diff != 0;
                case TYPE_GT: return diff > 0;
                case TYPE_GE: return diff >= 0;
            }
            return false;
        }

        private int GetDiff(Object a, Object b)
        {
            if (a == null && b == null)
                return 0;

            if (a == null)
                return 1;

            if (b == null)
                return -1;

            IComparable ca = (IComparable)a;
            IComparable cb = (IComparable)b;

            return ca.CompareTo(cb);
        }
    }
}
