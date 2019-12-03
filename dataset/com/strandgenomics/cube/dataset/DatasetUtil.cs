using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using com.strandgenomics.cube.dataset.util;
using framework;

namespace com.strandgenomics.cube.dataset
{
    public class DatasetUtil
    {
        /// <summary>
        /// Returns indices of continous columns in the specified dataset.
        /// </summary>
        /// <param name="dataset"></param>
        /// <returns></returns>
        public static /*IntArray*/int[] GetContinuousColumnIndices(IDataset dataset) //#framework
        {
            int colCount = dataset.GetColumnCount();
            
            int[] indices = new int[colCount]; //#framework

            for (int i = 0; i < colCount; i++)
            {
                if (!dataset.GetColumn(i).IsCategorical())
                    indices[i] = 1;
            }
            
            return indices;
        }

        /// <summary>
        /// Returns indices of categorical columns in the specified dataset.
        /// </summary>
        /// <param name="dataset"></param>
        /// <returns></returns>
        public static /*IntArray*/ int[] GetCategoricalColumnIndices(IDataset dataset)
        {
            int colCount = dataset.GetColumnCount();

            int[] indices = new int[colCount];

            for (int i = 0; i < colCount; i++)
            {
                if (dataset.GetColumn(i).IsCategorical())
                    indices[i] = 1;
            }

            return indices;
        }

        /// <summary>
        /// This method returns a dataset that contains a subset of columns of the initial
        /// dataset. The columns to subset out of the original dataset are specified in the
        /// colIndices argument.
        /// 
        /// The name of the new dataset has "(subset)" prepended to the original dataset name.
        /// </summary>
        /// <param name="dataset"></param>
        /// <param name="colIndices"></param>
        /// <returns></returns>
        public static IDataset GetColumnSubsetDataset(IDataset dataset, int[] colIndices)
        {
            int numCols = colIndices.Length;
            IColumn[] cols = new IColumn[numCols];
            for (int i = 0; i < numCols; i++)
            {
                cols[i] = dataset.GetColumn(colIndices[i]);
            }
            string name = "(subset)(" + dataset.GetName() + ")";
            return DatasetFactory.CreateDataset(name, cols);
        }

        public static IDataset GetRowSubsetDataset(IDataset dataset, /*IndexedIntArray*/int[] rowIndices)
        {
            int[] colIndices = Enumerable.Range(0, dataset.GetColumnCount()).ToArray();
            return GetSubsetDataset(dataset, rowIndices, colIndices);
        }

        /// <summary>
        /// This methods returns a dataset that contains a subset of rows and columns
        /// of the original dataset. The subset is specified by the intersection of
        /// the rowIndices and colIndices arguments. Thus every cell of the form
        /// rowIndices.get(i), colIndices.get(j)
        /// gets into the new subset dataset.
        /// 
        /// The name of the new dataset has "(subset)" prepended to the original dataset
        /// name.
        /// </summary>
        /// <param name="dataset"></param>
        /// <param name="rowIndices"></param>
        /// <param name="colIndices"></param>
        /// <returns></returns>
        public static IDataset GetSubsetDataset(IDataset dataset, /*IndexedIntArray*/int[] rowIndices, /*IntArray*/int[] colIndices)
        {
            int numCols = colIndices.Length;
            IColumn[] cols = new IColumn[numCols];
            for (int i = 0; i < numCols; i++)
            {
                IColumn c = dataset.GetColumn(colIndices[i]);
                cols[i] = ColumnFactory.CreateSubsetColumn(c, rowIndices);
            }
            string name = "(subset)(" + dataset.GetName() + ")";
            return DatasetFactory.CreateDataset(name, cols, rowIndices.Length);
        }

        /// <summary>
        /// Returns minimum value of the column at the specified position in the dataset.
        /// </summary>
        /// <param name="dataset"></param>
        /// <param name="colIndex"></param>
        /// <returns></returns>
        public static float GetMin(IDataset dataset, int colIndex)
        {
            IColumn c = dataset.GetColumn(colIndex);

            int minIndex = c.GetMinIndex();
            return c.GetFloat(minIndex);
        }

        /// <summary>
        /// Returns minimum value among all columns specified by the colIndices in the dataset.
        /// </summary>
        /// <param name="dataset"></param>
        /// <param name="colIndices"></param>
        /// <returns></returns>
        public static float GetMin(IDataset dataset, int[] colIndices)
        {
            int n = colIndices.Length;

            if (n == 0)
                return DatasetConstants.FLOAT_MV;

            float min = float.MaxValue;

            for (int i = 0; i < n; i++)
            {
                int colIndex = colIndices[i];
                float cmin = GetMin(dataset, colIndex);
                min = Math.Min(min, cmin);
            }

            return min;
        }

        /// <summary>
        /// Returns maximum value of the column at the specified position in the dataset.
        /// </summary>
        /// <param name="dataset"></param>
        /// <param name="colIndex"></param>
        /// <returns></returns>
        public static float GetMax(IDataset dataset, int colIndex)
        {
            IColumn c = dataset.GetColumn(colIndex);

            int maxIndex = c.GetMaxIndex();
            return c.GetFloat(maxIndex);
        }


        /// <summary>
        /// Returns maximum value among all columns specified by the colIndices in the dataset.
        /// </summary>
        /// <param name="dataset"></param>
        /// <param name="colIndices"></param>
        /// <returns></returns>
        public static float GetMax(IDataset dataset, int[] colIndices)
        {
            int n = colIndices.Length;

            if (n == 0)
                return DatasetConstants.FLOAT_MV;

            float max = -float.MaxValue;

            for (int i = 0; i < n; i++)
            {
                int colIndex = colIndices[i];
                float cmax = GetMax(dataset, colIndex);

                max = Math.Max(max, cmax);
            }

            return max;
        }


        /// <summary>
        /// Returns sum of the column at the specified position in the dataset.
        /// </summary>
        /// <param name="dataset"></param>
        /// <param name="colIndex"></param>
        /// <returns></returns>
        public static float GetSum(IDataset dataset, int colIndex)
        {
            return dataset.GetColumn(colIndex).GetSum();
        }



        /// <summary>
        /// Returns sum of the column at the specified position in the dataset considering 
        /// only the rows specified in the rowIndices.
        /// </summary>
        /// <param name="dataset"></param>
        /// <param name="colIndex"></param>
        /// <param name="rowIndices"></param>
        /// <returns></returns>
        public static float GetSum(IDataset dataset, int colIndex, int[] rowIndices)
        {
            IColumn c = dataset.GetColumn(colIndex);
            int size = rowIndices.Length;

            float sum = 0.0f;
            for (int i = 0; i < size; i++)
            {
                int rowIndex = rowIndices[i];
                sum += c.GetFloat(rowIndex);
            }

            return sum;
        }


        /// <summary>
        /// Following private 'copy' methods copy the contents of given column to the array,
        /// starting from the beginIndex location
        /// </summary>
        /// <param name="c"></param>
        /// <param name="array"></param>
        /// <param name="beginIndex"></param>
        public static void Copy(IColumn c, int[] array, int beginIndex)
        {
            int size = c.GetSize();
            for (int i = 0; i < size; i++)
                array[i + beginIndex] = (int)c.GetInt(i);
        }

        public static void Copy(IColumn c, float[] array, int beginIndex)
        {
            int size = c.GetSize();
            for (int i = 0; i < size; i++)
                array[i + beginIndex] = (float)c.GetFloat(i);
        }


        public static void Copy(IColumn c, string[] array, int beginIndex)
        {
            int size = c.GetSize();
            for (int i = 0; i < size; i++)
                array[i + beginIndex] = (string)c.Get(i);
        }

        public static void Copy(IColumn c, object[] array, int beginIndex)
        {
            int size = c.GetSize();
            for (int i = 0; i < size; i++)
                array[i + beginIndex] = c.Get(i);
        }
        public static void Copy(IColumn c, long[] array, int beginIndex)
        {
            int size = c.GetSize();
            for (int i = 0; i < size; i++)
                array[i + beginIndex] = ((long)c.Get(i));
        }

        /// <summary>
        /// Returns a column after combining the contents of column 'a' followed by column 'b'
        /// throws exception if they are not of same type
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static IColumn CombineColumns(IColumn a, IColumn b)
        {
            string dataType1 = a.GetDatatype();
            string dataType2 = b.GetDatatype();

            if (!dataType1.Equals(dataType2))
                throw new Exception("Both columns should have same type to be combined");

            if (dataType1.Equals(ColumnFactory.DATATYPE_INT))
            {
                int sizeA = a.GetSize();
                int sizeB = b.GetSize();
                int[] array = new int[sizeA + sizeB];
                Copy(a, array, 0);
                Copy(b, array, sizeA);
                return ColumnFactory.CreateIntColumn(a.GetName(), array);
            }
            else if (dataType1.Equals(ColumnFactory.DATATYPE_FLOAT))
            {
                int sizeA = a.GetSize();
                int sizeB = b.GetSize();
                float[] array = new float[sizeA + sizeB];
                Copy(a, array, 0);
                Copy(b, array, sizeA);
                return ColumnFactory.CreateFloatColumn(a.GetName(), array);
            }
            else if (dataType1.Equals(ColumnFactory.DATATYPE_STRING))
            {
                int sizeA = a.GetSize();
                int sizeB = b.GetSize();
                string[] array = new string[sizeA + sizeB];
                Copy(a, array, 0);
                Copy(b, array, sizeA);
                return ColumnFactory.CreateStringColumn(a.GetName(), array);
            }
            else
            {
                int sizeA = a.GetSize();
                int sizeB = b.GetSize();
                object[] array = new object[sizeA + sizeB];
                Copy(a, array, 0);
                Copy(b, array, sizeA);
                return ColumnFactory.CreateObjectColumn(a.GetName(), array);
            }
        }


        //#framework
        /// <summary>
        /// Returns the contents of the column as a FloatArray.
        /// data = [ c.getNumericalValue(i) for i in range(c.getSize()) ].
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static float[] GetColumnData(IColumn c)
        {
            return null;
            //#framework
            //return new ColumnFloatArray(c);
        }

        /// <summary>
        /// Returns the contents pf the column as a String[].
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string[] GetStringArray(IColumn c)
        {
            int size = c.GetSize();
            string[] retVal = new string[size];
            for (int i = 0; i < size; i++)
            {
                object o = c.Get(i);
                if (o != null)
                    retVal[i] = o.ToString();
            }
            return retVal;
        }

        /// <summary>
        /// Returns the contents pf the column as a int[].
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static int[] GetIntegerArray(IColumn c)
        {
            int size = c.GetSize();
            int[] retVal = new int[size];
            for (int i = 0; i < size; i++)
            {
                retVal[i] = c.GetInt(i);
            }
            return retVal;
        }


        /// <summary>
        /// Returns the contents of the column as a List.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static ArrayList GetColumnDataAsList(IColumn c)
        {
            int size = c.GetSize();
            ArrayList l = new ArrayList(size);
            for (int i = 0; i < size; i++)
            {
                l.Add(c.Get(i));
            }
            return l;
        }

        /// <summary>
        /// Appends the specified column to the specified dataset.
        /// If the dataset contains a column with same name as that of the 
        /// specified column, some suitable prefix is added to the name of 
        /// the column to make it unique.
        /// <see cref="IDataset.AddColumn(IColumn)"/>
        /// </summary>
        /// <param name="dataset"></param>
        /// <param name="column"></param>
        public static void AddColumn(IDataset dataset, IColumn column)
        {
            MakeColumnNameUnique(dataset, column, null);
            dataset.AddColumn(column);
        }

        /// <summary>
        /// Appends the specified columns to the specified dataset.
        /// If the dataset contains a column with same name as that of a
        /// specified column, some suitable prefix is added to the name of 
        /// the column to make it unique.
        /// <see cref="AddColumn(IDataset,IColumn) "/>
        /// <see cref="IDataset.AddColumn(IColumn)"/>
        /// </summary>
        /// <param name="dataset"></param>
        /// <param name="columns"></param>
        public static void AddColumns(IDataset dataset, IColumn[] columns)
        {
            HashSet<string> blacklist = new HashSet<string>();

            //TODO-Anand: explain why blacklist is required. 
            for (int i = 0; i < columns.Length; i++)
            {
                MakeColumnNameUnique(dataset, columns[i], blacklist);
                blacklist.Add(columns[i].GetName());
            }

            dataset.AddColumns(columns);
        }

        private static HashSet<string> EMPTY_SET = new HashSet<string>();


        private static void MakeColumnNameUnique(IDataset dataset, IColumn column, HashSet<string> blacklist)
        {
            string name = column.GetName();
            int count = 1;

            if (blacklist == null)
                blacklist = EMPTY_SET;

            while (dataset.HasColumn(name) || blacklist.Contains(name))
                name = column.GetName() + "_" + count++;

            if (!column.GetName().Equals(name))
                column.SetName(name);
        }

        /// <summary>
        /// Returns true if the specfied column is an float column.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsFloatColumn(IColumn c)
        {
            return c.GetDatatype().Equals(ColumnFactory.DATATYPE_FLOAT);
        }

        /// <summary>
        /// Returns true if the specfied column is an integer column.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsIntegerColumn(IColumn c)
        {
            return c.GetDatatype().Equals(ColumnFactory.DATATYPE_INT);
        }


        /// <summary>
        /// Returns true if the specfied column is an double column.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsDoubleColumn(IColumn c)
        {
            return c.GetDatatype().Equals(ColumnFactory.DATATYPE_DOUBLE);
        }


        /// <summary>
        /// Returns true if the specfied column is an decimal column.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsDecimalColumn(IColumn c)
        {
            return c.GetDatatype().Equals(ColumnFactory.DATATYPE_DECIMAL);
        }

        /// <summary>
        /// Returns true if the specfied column is an string column.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsStringColumn(IColumn c)
        {
            return c.GetDatatype().Equals(ColumnFactory.DATATYPE_STRING);
        }

        /// <summary>
        /// Returns true if the specfied column is an date column. 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsDateColumn(IColumn c)
        {
            return c.GetDatatype().Equals(ColumnFactory.DATATYPE_DATE);
        }

        /// <summary>
        /// Returns true if the specfied column is an object column.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsObjectColumn(IColumn c)
        {
            return c.GetDatatype().Equals(ColumnFactory.DATATYPE_OBJECT);
        }

        /// <summary>
        /// Returns true if the specfied column is an object column.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsMutableColumn(IColumn c)
        {
            return (c is MutableColumn);
            // return c.getDatatype().equals(ColumnFactory.DATATYPE_MUTABLE);
        }



        public static bool IsRegularColumn(IColumn c)
        {
            return (c is AbstractRegularColumn || c is MultisetColumn);
        }

        public static bool IsSubsetColumn(IColumn c)
        {
            return c is SubsetColumn;
        }

        public static bool IsSupersetColumn(IColumn c)
        {
            return c is SupersetColumn;
        }

        private static float MAX_THRESHOLD = 1.0e20f;
        private static float MIN_THRESHOLD = -1.0e20f;

        /// <summary>
        /// checks whether the specified float column has 'valid' values 
        /// within specified bounds
        /// </summary>
        /// <param name="dataset"></param>
        /// <param name="colIndex"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool IsWithinBounds(IDataset dataset,
                     int colIndex,
                     float min, float max)
        {
            IColumn c = dataset.GetColumn(colIndex);
            return IsWithinBounds(c, min, max);
        }

        private static bool IsWithinBounds(IColumn c, float min, float max)
        {
            if (!c.GetDatatype().Equals("float"))
                return true;

            float colMin = c.GetFloat(c.GetMinIndex());
            float colMax = c.GetFloat(c.GetMaxIndex());
            if (float.IsNaN(colMin) || float.IsNaN(colMax))
                return false;

            if (colMin < min || colMax > max)
                return false;

            return true;
        }


        public static bool IsWithinBounds(IColumn c)
        {
            return IsWithinBounds(c, MIN_THRESHOLD, MAX_THRESHOLD);
        }

        public static bool IsWithinBounds(IDataset dataset,
                         int colIndex)
        {
            return IsWithinBounds(dataset, colIndex,
                          MIN_THRESHOLD, MAX_THRESHOLD);
        }

        public static bool IsWithinBounds(IDataset dataset,
                         int[] colIndices,
                         float min, float max)
        {
            for (int i = 0; i < colIndices.Length; i++)
            {
                if (!IsWithinBounds(dataset, colIndices[i], min, max))
                    return false;
            }
            return true;
        }

        public static bool IsWithinBounds(IDataset dataset,
                         int[] colIndices)
        {
            return IsWithinBounds(dataset, colIndices,
                          MIN_THRESHOLD, MAX_THRESHOLD);
        }

        /// <summary>
        /// Returns an array of marked columns of the dataset. Returns an array
        /// of size 0 if the dataset has no marked columns.
        /// </summary>
        /// <param name="dataset"></param>
        /// <returns></returns>
        public static IColumn[] GetMarkedColumns(IDataset dataset)
        {

            ArrayList columns = new ArrayList();
            int colCount = dataset.GetColumnCount();
            for (int i = 0; i < colCount; i++)
            {
                IColumn column = dataset.GetColumn(i);
                if (column.GetMetaData().IsMarked())
                    columns.Add(column);
            }
            int size = columns.Count;
            IColumn[] retVal = new IColumn[size];
            for (int i = 0; i < size; i++)
            {
                retVal[i] = (IColumn)columns[i];
            }

            return retVal;
        }


        /// <summary>
        /// Returns an array of names of all columns of the dataset. Returns an array
        /// of size 0 if the dataset has no marked columns.
        /// </summary>
        /// <param name="dataset"></param>
        /// <returns></returns>
        public static string[] GetColumnNames(IDataset dataset)
        {
            List<string> columnNames = new List<string>();
            int colCount = dataset.GetColumnCount();

            for (int i = 0; i < colCount; i++)
            {
                columnNames.Add(dataset.GetColumnName(i));
            }
            return columnNames.ToArray();
        }

        /// <summary>
        /// Returns an array of name of marked columns of the dataset. Returns an array
        /// of size 0 if the dataset has no marked columns.
        /// </summary>
        /// <param name="dataset"></param>
        /// <returns></returns>
        public static string[] GetMarkedColumnNames(IDataset dataset)
        {

            List<string> columnNames = new List<string>();
            int colCount = dataset.GetColumnCount();

            for (int i = 0; i < colCount; i++)
            {
                string columnName = dataset.GetColumnName(i);
                if (dataset.GetMetaData(columnName).IsMarked())
                    columnNames.Add(columnName);
            }
            string[] retVal = columnNames.ToArray();

            return retVal;
        }
        public static IntSet GetMarkedColumnIndices(IDataset dataset)
        {
            return GetMarkedColumnIndices(dataset, null);
        }

        public static IntSet GetMarkedColumnIndices(IDataset dataset, string markId)
        {

            return new MarkedColumnIndicesIntSet(dataset, markId);
        }

        /// <summary>
        /// Returns an array of column names of the dataset whose mark identifier matches 
        /// the specified regular expression. Returns an array of size 0 if the 
        /// dataset has no marked columns, or has no column with a mark that matches
        /// the specified regex.
        /// </summary>
        /// <param name="dataset"></param>
        /// <param name="regex"></param>
        /// <returns></returns>
        public static IColumn[] GetMarkedColumns(IDataset dataset, string regex)
        {

            ArrayList columns = new ArrayList();
            int colCount = dataset.GetColumnCount();
            for (int i = 0; i < colCount; i++)
            {
                IColumn column = dataset.GetColumn(i);
                string mark = column.GetMetaData().GetMark();
                if (mark != null)
                {
                    if (Regex.IsMatch(mark, regex))
                    {
                        columns.Add(column);
                    }
                }
            }

            int size = columns.Count;
            IColumn[] retVal = new IColumn[size];
            for (int i = 0; i < size; i++)
            {
                retVal[i] = (IColumn)columns[i];
            }

            return retVal;
        }

        /// <summary>
        /// This utility method returns a candidate new column index for the specified
        /// oldColumnIndex, after the specified removedIndices have been removed. Thus
        /// if the maxValue is 5, i.e. the original indices were<br>
        /// 0, 1, 2, 3, 4<br>
        /// and the removed indices were<br>
        /// 1, 2, 4<br>
        /// then <br>
        /// oldColumnIndex = 0, will give newColumnIndex = 0<br>
        /// oldColumnIndex = 1, will give newColumnIndex = 0<br>
        /// oldColumnIndex = 3, will give newColumnIndex = 1<br>
        /// </summary>
        /// <param name="oldColumnIndex"></param>
        /// <param name="removedIndices"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static int GetNewColumnIndex(int oldColumnIndex, IntSet removedIndices, int maxValue)
        {

            int count = 0;
            while (removedIndices.Contains(oldColumnIndex))
            {
                oldColumnIndex = (oldColumnIndex - 1) < 0 ?
                         (maxValue - oldColumnIndex - 1) :
                         (oldColumnIndex - 1);
                count++;
                if (count == maxValue)
                    throw new DataException("Cannot find a suitable new index.");
            }

            //#framework
            IntSet remainingIndices = null;
            //ArrayUtil.difference(
            //ArrayUtil.range(0, maxValue),
            //removedIndices);

            IEnumerator iter = remainingIndices.GetEnumerator();
            count = 0;
            do
            {
                if ((int)iter.Current == oldColumnIndex)
                    return count;
                count++;
            }
            while (iter.MoveNext());


            return -1;
        }
        //#framework


        /// <summary>
        /// Returns an IndexedIntArray of newColumnIndices from oldColumnIndices, after 
        /// removedIndices have been removed. 
        /// </summary>
        /// <param name="oldColumnIndices"></param>
        /// <param name="removedIndices"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static int[] GetNewColumnIndices(int[] oldColumnIndices,
        IntSet removedIndices, int maxValue)
        {

            IntSet remainingIndices = null; /*ArrayUtil.difference(oldColumnIndices, removedIndices);*/

            IEnumerator<int> iter = remainingIndices.GetEnumerator();
            int[] retVal = new int[maxValue];
            do
            {

                int newIndex = GetNewColumnIndex(iter.Current, removedIndices, maxValue);
                retVal.Append(newIndex);
            }
            while (iter.MoveNext());

            return null;/*ArrayUtil.createIndexedIntArray(retVal, maxValue);*/ //#framework
        }

        public static IDataset Transpose(IDataset dataset)
        {
            return Transpose("transpose", dataset);
        }


        public static IDataset Transpose(string name, IDataset dataset)
        {

            IColumn[] idColumns = GetMarkedColumns(dataset, "identifier");
            IColumn idColumn = null;
            if (idColumns != null && idColumns.Length > 0)
                idColumn = idColumns[0];
            if (idColumn == null)
            {
                int[] categoryColumns = GetCategoricalColumnIndices(dataset);
                int size = categoryColumns.Length;
                for (int i = 0; i < size; i++)
                {
                    string idname = dataset.GetColumn(categoryColumns[i]).GetName();
                    if (idname.Equals("Identifier"))
                        idColumn = dataset.GetColumn(categoryColumns[i]);
                }
            }

            return Transpose(name, dataset, idColumn);
        }

        public static IDataset Transpose(IDataset dataset, IColumn idColumn)
        {
            return Transpose("transpose", dataset, idColumn);
        }

        public static IDataset Transpose(string name, IDataset dataset, IColumn idColumn)
        {

            int[] datacolumns = GetContinuousColumnIndices(dataset);
            int rowCount = datacolumns.Length;
            int colCount = dataset.GetRowCount() + 1;
            if (rowCount <= 0)      // no data columns
                return null;

            // HashMap duplicateMap = new HashMap();

            // create id-column first from column-headers
            string[] ids = new string[rowCount];
            for (int i = 0; i < rowCount; i++)
                ids[i] = dataset.GetColumn(datacolumns[i]).GetName();
            IColumn newIdColumn = ColumnFactory.CreateStringColumn("Identifier", ids);
            newIdColumn.GetMetaData().SetMark("identifier");

            string[] columnNames = new string[colCount];
            for (int i = 1; i < colCount; i++)
            {
                string columnName;
                if (idColumn != null)
                {
                    object o = idColumn.Get(i - 1);
                    if (o == null)
                        columnName = "UNNAMED";
                    else
                        columnName = o.ToString();
                }
                else
                    columnName = "COL" + i;

                // TODO - check if column-name exists (duplicate ID's possible)
                // columnName = chooseUniqueColumnName(duplicateMap, columnName);
                columnNames[i] = columnName;
            }

            float[][] newData = new float[colCount][];
            const int BLOCK = 32;
            for (int i = 1; i < colCount; i += BLOCK)
            {
                int ri = i + BLOCK;
                if (colCount < ri)
                    ri = colCount;
                for (int j = 0; j < rowCount; j += BLOCK)
                {
                    int rj = j + BLOCK;
                    if (rowCount < rj)
                        rj = rowCount;
                    for (int i2 = i; i2 < ri; i2++)
                    {
                        float[] data = newData[i2];
                        for (int j2 = j; j2 < rj; j2++)
                        {
                            data[j2] = dataset.GetColumn(datacolumns[j2]).GetFloat(i2 - 1);
                        }
                    }
                }
            }

            IColumn[] newCols = new IColumn[colCount];
            newCols[0] = newIdColumn;
            for (int i = 1; i < colCount; i++)
            {
                newCols[i] = ColumnFactory.CreateFloatColumn(columnNames[i], newData[i]);
            }
            return DatasetFactory.CreateDataset(name, newCols);
        }


        public static IColumn GetParentColumn(IColumn subsetColumn)
        {
            if (!(subsetColumn is SubsetColumn))
	            return null;
            return ((SubsetColumn)subsetColumn).GetParentColumn();
        }

        private static string UNIQUE = "_*#AVADIS#*_";

        public static IColumn[] MergeById(IDataset dataset, int idDataset, IDataset fileDataset, int idFile)
        {
            return MergeWithIdentifiers(dataset, idDataset, fileDataset, idFile, false, false);
        }

        public static IColumn[] MergeByIdWithReplicates(IDataset dataset, int idDataset, IDataset fileDataset, int idFile)
        {
            return MergeWithIdentifiers(dataset, idDataset, fileDataset, idFile, true, false);
        }

        public static IColumn[] MergeByIdIgnoringCase(IDataset dataset, int idDataset, IDataset fileDataset, int idFile)
        {
            return MergeWithIdentifiers(dataset, idDataset, fileDataset, idFile, false, true);
        }

        private static IColumn[] MergeWithIdentifiers(IDataset dataset,
                           int idDataset,
                           IDataset fileDataset,
                           int idFile,
                           bool allowReplicates,
                           bool ignoreCase)
        {
            IColumn dsIdColumn = dataset.GetColumn(idDataset);
            int rowCount = dataset.GetRowCount();

            string[] values = new string[rowCount];
            HashSet<string> set = new HashSet<string>();

            for (int i = 0; i < rowCount; i++)
            {
                string str = GetUniqueid(dsIdColumn.Get(i), set);
                if (str != null && ignoreCase)
                    values[i] = str.ToLower();
                else
                    values[i] = str;
            }
            //#framework
            //IndexedArray colArray = ArrayUtil.createIndexedArray(ArrayUtil.createArray(values));
            int[] colArray = new int[0];
            IColumn fileIdColumn = fileDataset.GetColumn(idFile);

            ArrayList list = CreateMappingArrays(colArray, fileIdColumn,
                                 allowReplicates, ignoreCase);
            //IndexedIntArray tmp1 = ArrayUtil.createIndexedIntArray(((DefaultIntArray)list.get(1)));
            //IndexedIntArray tmp2 = ArrayUtil.createIndexedIntArray(((DefaultIntArray)list.get(0)));
            int[] tmp1 = Array.Empty<int>();
            int[] tmp2 = Array.Empty<int>();

            int numCols = fileDataset.GetColumnCount();
            IColumn[] cols = new IColumn[numCols - 1];
            //IndexedIntArray indices = ArrayUtil.range(0, rowCount);
            int[] indices = Enumerable.Range(0, rowCount).ToArray();
            int index = 0;
            for (int i = 0; i < numCols; i++)
            {
                if (i != idFile)
                {
                    IColumn c = fileDataset.GetColumn(i);

                    IColumn col =
                        ColumnFactory.CreateSupersetColumn(
                        ColumnFactory.CreateSubsetColumn(c, tmp1),
                        tmp2,
                        rowCount);

                    cols[index] = ColumnFactory.CloneColumn(col, indices);
                    cols[index].SetName(c.GetName());
                    cols[index].SetCategorical(c.IsCategorical());
                    cols[index].GetMetaData().SetMark(c.GetMetaData().GetMark());
                    index++;
                }
            }
            return cols;
        }


        public static IColumn[] Merge(IDataset dataset, IDataset fileDataset)
        {
            int fileRowCount = fileDataset.GetRowCount();
            int rowCount = dataset.GetRowCount();

            int numCols = fileDataset.GetColumnCount();
            IColumn[] cols = new IColumn[numCols];
            //IndexedIntArray indices = ArrayUtil.range(0, rowCount);
            int[] indices = Enumerable.Range(0,rowCount).ToArray();
            if (fileRowCount < rowCount)
            {
                //IndexedIntArray fileIndices = ArrayUtil.range(0, fileRowCount);
                int[] fileIndices = Enumerable.Range(0,fileRowCount).ToArray();
                for (int i = 0; i < numCols; i++)
                {
                    IColumn c = fileDataset.GetColumn(i);

                    IColumn col = ColumnFactory.CreateSupersetColumn(c, fileIndices, rowCount);
                    cols[i] = ColumnFactory.CloneColumn(col, indices);
                    cols[i].SetName(c.GetName());
                    cols[i].SetCategorical(c.IsCategorical());
                    cols[i].GetMetaData().SetMark(c.GetMetaData().GetMark());
                }
            }
            else
            {
                for (int i = 0; i < numCols; i++)
                {
                    IColumn c = fileDataset.GetColumn(i);
                    IColumn col = ColumnFactory.CreateSubsetColumn(c, indices);
                    cols[i] = ColumnFactory.CloneColumn(col, indices);
                    cols[i].SetName(c.GetName());
                    cols[i].SetCategorical(c.IsCategorical());
                    cols[i].GetMetaData().SetMark(c.GetMetaData().GetMark());
                }
            }
            return cols;
        }


        private static string GetUniqueid(object o, HashSet<string> s)
        {
            string str = (o == null) ? null : o.ToString();
            string name = str;

            if (name != null)
            {
                int count = 1;
                while (s.Contains(name))
                {
                    name = str + UNIQUE + count;
                    count++;
                }
                s.Add(name);
            }
            return name;
        }

        //#framework
        private static ArrayList CreateMappingArrays(/*IndexedArray*/int[] colArray,
                         IColumn fileIdColumn,
                         bool allowReplicates,
                         bool ignoreCase)
        {
            int size = fileIdColumn.GetSize();
            int[] mapArray = new int[size];
            int[] presenceArray = new int[size];

            HashSet<string> set = new HashSet<string>();

            for (int i = 0; i < size; i++)
            {
                string name = GetUniqueid(fileIdColumn.Get(i), set);
                if (name != null && ignoreCase)
                    name = name.ToLower();

                int index = Array.IndexOf(colArray, name);

                if (index < 0)
                {
                    if (allowReplicates)
                    {
                        name = StripUniqueKey(name);
                        index = Array.IndexOf(colArray, name);
                    }
                }

                if (index >= 0)
                {
                    presenceArray.Append(i);
                    mapArray.Append(index);
                }
            }
            ArrayList list = new ArrayList(2);
            list.Add(mapArray);
            list.Add(presenceArray);
            return list;
        }

        private static string StripUniqueKey(string name)
        {
            if (name != null)
            {
                int index = name.IndexOf(UNIQUE);
                if (index > 0)
                    name = name.Substring(0, index);
            }
            return name;
        }


        public static string GetString(IDataset dataset, int row, int col)
        {
            IColumn c = dataset.GetColumn(col);
            if (c.IsMissingValue(row))
                return "";
            else
                return c.Get(row).ToString();
        }

        public static void WriteDataset(IDataset dataset, FileStream file, string separator, string header)
        {

            int colCount = dataset.GetColumnCount();
            string[] columnNames = new string[colCount];

            for (int i = 0; i < colCount; i++)
                columnNames[i] = dataset.GetColumn(i).GetName();
            WriteDataset(dataset, file, separator, header, columnNames);
        }


        public static void WriteDataset(IDataset dataset, FileStream file, String separator, String header, String[] columnNames)
        {
            StreamWriter w =

        new StreamWriter(

        new BufferedStream(

            file));

            int rowCount = dataset.GetRowCount();
            int colCount = dataset.GetColumnCount();

            string buf = "";



            if (header != null)
            {
                buf.Concat(header.ToCharArray());
                w.WriteLine(buf);
                buf = "";
            }

            if (columnNames != null)
            {
                for (int i = 0; i < colCount; i++)
                {
                    if (i != 0)
                        buf.Concat(separator);

                    buf.Concat(columnNames[i]);
                }
            }
            /*	for(int i=0; i<colCount; i++){
                    if (i != 0)
                    buf.append(separator);

                    buf.append(dataset.getColumn(i).getName());
                }*/
            w.WriteLine(buf);
            for (int i = 0; i < rowCount; i++)
            {
                buf = "";
                for (int j = 0; j < colCount; j++)
                {
                    if (j != 0)
                        buf.Concat(separator);

                    buf.Concat(GetString(dataset, i, j));
                }
                w.WriteLine(buf);
            }
            w.Close();
        }

        public static void WriteDataset(IDataset dataset, FileStream file, string separator)

        {
            WriteDataset(dataset, file, separator, null);
        }

        public static void WriteDataset(IDataset dataset, FileStream file)

        {
            string filename = file.Name;
            string extension = filename.Substring(filename.LastIndexOf(".") + 1).ToLower();
            string separator = "\t";
            if (extension.Equals("csv"))
                separator = ",";
            WriteDataset(dataset, file, separator);
        }

        public static void WriteDataset(IDataset dataset, StreamWriter stream, string separator)

        {
            WriteDataset(dataset, stream, separator, null);
        }

        public static void WriteDataset(IDataset dataset, StreamWriter stream, string separator, string header)

        {
            WriteDataset(dataset, stream, separator, header, true);
        }


        public static void WriteDataset(IDataset dataset, StreamWriter stream, string separator, string header, bool haveColNames) //{{{

        {

            int rowCount = dataset.GetRowCount();
            int colCount = dataset.GetColumnCount();

            StreamWriter pw = new StreamWriter
                    (new BufferedStream
                    (stream.BaseStream, 16 * 1024));
            if (header != null)
                pw.WriteLine(header);

            if (haveColNames)
            {
                for (int i = 0; i < colCount; i++)
                {
                    if (i != 0)
                        pw.Write(separator);

                    pw.Write(dataset.GetColumn(i).GetName());
                }
                pw.WriteLine();
            }

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    if (j != 0)
                        pw.Write(separator);

                    pw.Write(GetString(dataset, i, j));
                }

                pw.WriteLine();
            }

            pw.Flush();
        }

        //#framework
        ///// <summary>
        ///// Creates a MultiIndexedIntArray using the category information of 
        ///// the specified column.
        ///// </summary>
        ///// <param name="column"></param>
        ///// <returns></returns>
        //public static IndexedBintArray GetCategoricalState(IColumn column)
        //{
        //    return new CategoricalState(column);
        //}

        //#framework
        ///// <summary>
        ///// searches for multiple occurences of ids from searchFor in searchIn. 
        ///// Typical use case is expanding compressed transcriptID column to exon level 
        ///// transcript id column
        ///// </summary>
        ///// <param name="searchFor"></param>
        ///// <param name="searchIn"></param>
        ///// <returns></returns>
        //public static IndexedIntArray getMatchingRowIndices(IColumn searchFor, IColumn searchIn)
        //{
        //    DefaultIntArray indices = new DefaultIntArray(searchFor.getSize());


        //    if (searchIn == null || searchIn.getSize() == 0 || searchFor == null || searchFor.getSize() == 0)
        //        return null;
        //    int forSize = searchFor.getSize();
        //    int inSize = searchIn.getSize();


        //    IntIterator forIterator = searchFor.getRowIndicesInSortedOrder(true).iterator();
        //    IntIterator inIterator = searchIn.getRowIndicesInSortedOrder(true).iterator();

        //    int forIndex = forIterator.next();
        //    int inIndex = inIterator.next();
        //    Comparable forComparable = searchFor.getComparable(forIndex);
        //    Comparable inComparable = searchIn.getComparable(inIndex);

        //    while (true)
        //    {

        //        if (forComparable == null || inComparable == null)
        //            break;

        //        int value = forComparable.compareTo(inComparable);
        //        if (value == 0)
        //        {
        //            indices.add(inIndex);
        //        }
        //        if (value < 0)
        //        {
        //            if (!forIterator.hasNext())
        //                break;
        //            forIndex = forIterator.next();
        //            forComparable = searchFor.getComparable(forIndex);
        //        }
        //        else
        //        {
        //            if (!inIterator.hasNext())
        //                break;
        //            inIndex = inIterator.next();
        //            inComparable = searchIn.getComparable(inIndex);
        //        }

        //    }
        //    if (indices.getSize() == 0)
        //        return null;

        //    return ArrayUtil.createIndexedIntArray(indices);
        //}

        /// <summary>
        /// Collapses given categorical column in a categorical column with unique category values.
        /// </summary>
        /// <param name="c"></param>
        /// <returns>SubsetColumn</returns>
        public static IColumn GetUniqueCategoryColumn(IColumn c)
        {
            if (!c.IsCategorical())
                throw new Exception("Given column is not categorical");
            //#framework
            //DefaultIntArray indices = new DefaultIntArray();
            int[] indices = new int[0];
            int size = c.GetCategoryCount();
            for (int i = 0; i < size; i++)
            {
                indices.Append(c.GetRowIndicesOfCategory(i).GetEnumerator().Current);
                c.GetRowIndicesOfCategory(i).GetEnumerator().MoveNext();
            }

            //IndexedIntArray rowIndices = ArrayUtil.createIndexedIntArray(indices);
            int[] rowIndices = indices;
            return ColumnFactory.CreateSubsetColumn(c, rowIndices);
        }


        public static bool ApproxEquals(IDataset d1, IDataset d2, float delta)
        { // {{{

            return true
                && d1.GetColumnCount() == d1.GetColumnCount()
                && d1.GetRowCount() == d2.GetRowCount()
                && d1.GetName().Equals(d2.GetName())
                && CompareColumns(d1, d2, delta)
                ;
        }

        private static bool CompareColumns(IDataset d1, IDataset d2, float delta)
        {
            int colCount = d1.GetColumnCount();

            if (delta < 0)
                delta = -delta;

            for (int i = 0; i < colCount; i++)
            {
                IColumn c1 = d1.GetColumn(i);
                IColumn c2 = d2.GetColumn(i);

                if (!ColumnOperations.IsNumericColumn(c1) || !ColumnOperations.IsNumericColumn(c2))
                {
                    if (!c1.Equals(c2))
                        return false;
                }
                else
                {
                    IColumn diff = ColumnOperations.Sub(c1, c2, "diff");

                    if (c1.GetMissingValueCount() != diff.GetMissingValueCount() ||
                        c2.GetMissingValueCount() != diff.GetMissingValueCount())
                        return false;

                    // check min value is not less than -delta
                    IntSet rowIndices = diff.GetRowIndicesInSortedOrder(true);
                    //#framework
                    //IntIterator iter = rowIndices.iterator();
                    IEnumerator<int> iter = rowIndices.GetEnumerator();
                    do
                    {
                        int index = iter.Current;
                        if (diff.IsMissingValue(index))
                            break;
                        if (diff.GetFloat(index) < -delta)
                            return false;
                        break;
                    }
                    while (iter.MoveNext()) ;

                        // check max value is not greater than delta
                        rowIndices = diff.GetRowIndicesInSortedOrder(false);
                    //iter = rowIndices.iterator();
                    iter = rowIndices.GetEnumerator();
                    do
                    {
                        int index = iter.Current;
                        if (diff.IsMissingValue(index))
                            continue;
                        if (diff.GetFloat(index) > delta)
                            return false;
                        break;
                    }
                    while (iter.MoveNext()) ;
                }
            }

            return true;
        }


        public static IDataset CloneDataset(IDataset d, bool deep)
        {
            int columnCount = d.GetColumnCount();
            IColumn[] columns = new IColumn[columnCount];
            for (int i = 0; i < columnCount; i++)
                columns[i] = deep ?
                             ColumnFactory.CloneColumn(d.GetColumn(i)) :
                             d.GetColumn(i);
            return DatasetFactory.CreateDataset(d.GetName(), columns);
        }


    }

    //#framework

    public class CategoricalState /*: IndexedBintArray*/
    {
        public static string FORMAT_ID = "cube.dataset.DatasetUtil$CategoricalState";

        int[] data;
        int[] offset;
        int[] lookup;

        public CategoricalState(IColumn c)
        {
            int catcount = c.GetCategoryCount();
            int size = c.GetSize();

            lookup = new int[size];
            data = new int[size];
            offset = new int[catcount + 1];

            int index = 0;
            for (int i = 0; i < catcount; i++)
            {
                offset[i] = index;

                //IntIterator iterator = c.getRowIndicesOfCategory(i).iterator();
                IEnumerator<int> iterator = c.GetRowIndicesOfCategory(i).GetEnumerator();
                do
                {
                    int value = iterator.Current;
                    data[index++] = value;
                    lookup[value] = i;
                }
                while (iterator.MoveNext()) ;
            }
            offset[catcount] = size;
        }

        /// <summary>
        /// for hexff
        /// </summary>
        private CategoricalState()
        {
        }

        // for hexff
        private void Initialize()
        {
            /*
            int index = 0;
            lookup = new int[data.length];

            for (int i=0; i < lookup.length; i++)
            lookup[i] = -1;

            for (int i=1; i<offset.length; i++) {
            for (int j=offset[i-1]; j < offset[i]; j++)
                lookup[data[j]] = i;
            }
            */
        }

        public int GetSize()
        {
            return data.Length;
        }

        public int Get(int i)
        {
            return data[i];
        }

        public /*IntIterator*/ void Iterator()
        {
            //return ArrayUtil.createIntIterator(data);
        }

        public int GetBinCount()
        {
            return offset.Length - 1;
        }

        public /*IntArray*/ void GetBin(int binIndex)
        {
            //return ArrayUtil.createIntArray(data, offset[binIndex], offset[binIndex + 1]);
        }

        public int GetBinIndex(int value)
        {
            if (0 <= value && value < data.Length)
                return lookup[value];
            return -1;
        }

        public bool Contains(int value)
        {
            return 0 <= value && value < data.Length;
        }

    }


    //#framework
    internal class ColumnFloatArray /*:AbstractFloatArray*/
    {

        private IColumn c;

        public ColumnFloatArray(IColumn c)
        {
            this.c = c;
        }

        public int GetSize()
        {
            return c.GetSize();
        }

        public float Get(int index)
        {
            return c.GetNumericValue(index);
        }
    }
    internal class MarkedColumnIndicesIntSet : IntSet
    {
        private IDataset dataset;
        private string markId;
        public MarkedColumnIndicesIntSet(IDataset dataset,string markId)
        {
            this.dataset = dataset;
            this.markId = markId;
        }

        public bool Contains(int element)
        {
            string markId2 = dataset.GetColumn(element).GetMetaData().GetMark();

            if (markId2 == null)
                return false;

            return (markId == null || markId.Equals(markId2));
        }

        public IEnumerator<int> GetEnumerator()
        {
            //return ArrayUtil.createIntIterator(this, dataset.getColumnCount());  //#framework
            return null;
        }
    }


}




