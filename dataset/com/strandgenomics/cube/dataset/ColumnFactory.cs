using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using framework;

namespace com.strandgenomics.cube.dataset
{
    /// <summary>
    /// ColumnFactory class provides various factory methods to create
    /// columns of different types. None of the column classes are
    /// themselves exposed outside the data package. Hence ColumnFactory
    /// is the only way to create columns from anywhere in the tool.
    /// 
    /// TODO: Apart from factory methods for basic columns like string, int,
    /// float, and date types, it also provides factory methods for creating
    /// subset columns, meta columns, etc.
    /// </summary>
    public class ColumnFactory
    {
        public static string DATATYPE_STRING = StringColumn.DATATYPE;
        public static string DATATYPE_FLOAT = FloatColumn.DATATYPE;
        public static string DATATYPE_INT = IntColumn.DATATYPE;
        public static string DATATYPE_DOUBLE = DoubleColumn.DATATYPE;
        public static string DATATYPE_DECIMAL = DecimalColumn.DATATYPE;
        public static string DATATYPE_DATE = DateColumn.DATATYPE;
        public static string DATATYPE_BIT = BitColumn.DATATYPE;
        public static string DATATYPE_OBJECT = ObjectColumn.DATATYPE;
        public static string DATATYPE_ENUM = EnumColumn<Enum>.DATATYPE;

        public static int COLUMNSIZE = 500000 * 4;
        public static string[] DATATYPES = new string[] {DATATYPE_STRING, DATATYPE_FLOAT, DATATYPE_INT, DATATYPE_DATE, DATATYPE_BIT, DATATYPE_OBJECT};

        /// <summary>
        /// Factory method to create a string column.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IColumn CreateStringColumn(string name, char[][] data, int size)
        {
            IColumn c = new StringColumn(name, data, size);
            return c;
        }

        public static IColumn CreateStringColumn(string name, char[][] data, int size, Comparator comparator)
        {
            IColumn c = new StringColumn(name, data, size, comparator);
            return c;
        }

        /// <summary>
        /// Factory method to create a string column.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="size"></param>
        /// <param name="deleteOriginalData"></param>
        /// <returns></returns>
        public static IColumn CreateStringColumn(string name, char[][] data, int size, bool deleteOriginalData)
        {
            IColumn c = new StringColumn(name, data, size, deleteOriginalData);
            return c;
        }

        /// <summary>
        /// Factory method to create a string column.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IColumn CreateStringColumn(string name, char[][] data)
        {
            return CreateStringColumn(name, data, data.Length);
        }

        /// <summary>
        /// Factory method to create a string column.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="deleteOriginalData"></param>
        /// <returns></returns>
        public static IColumn CreateStringColumn(string name, char[][] data, bool deleteOriginalData)
        {
            return CreateStringColumn(name, data, data.Length, deleteOriginalData);
        }

        /// <summary>
        /// Factory method to create a string column.
        /// This method calls <code>createStringColumn(String,char[][],int)</code>
        /// method after creating char[][] using String[][] data.
        /// <see cref="CreateStringColumn(string,char[][],int)"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IColumn CreateStringColumn(string name, string[] data, int size)
        {
            char[][] cdata = new char[size][];

            for (int i = 0; i < size; i++)
                cdata[i] = (data[i] == null) ? null : data[i].ToCharArray();

            return CreateStringColumn(name, cdata, size);
        }

        public static IColumn CreateStringColumn(string name, string[] data, int size, Comparator comparator)
        {
            char[][] cdata = new char[size][];

            for (int i = 0; i < size; i++)
                cdata[i] = (data[i] == null) ? null : data[i].ToCharArray();

            return CreateStringColumn(name, cdata, size, comparator);
        }


        public static IColumn CreateStringColumn(string name, string[] data)
        {
            return CreateStringColumn(name, data, data.Length);
        }


        public static IColumn CreateStringColumn(string name, string[] data, Comparator comparator)
        {
            return CreateStringColumn(name, data, data.Length, comparator);
        }

        /// <summary>
        /// Factory method to create a int column.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IColumn CreateIntColumn(string name, int[] data, int size)
        {
            IColumn c = new IntColumn(name, data, size);
            return c;
        }

        public static IColumn CreateIntColumn(string name, int[] data)
        {
            return CreateIntColumn(name, data, data.Length);
        }

        /// <summary>
        /// Factory method to create a int column.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IColumn CreateEnumColumn<T>(string name, T[] data, int size) where T:Enum
        {
            IColumn c = new EnumColumn<T>(name, size, data);
            return c;
        }

        public static  IColumn CreateEnumColumn<T>(string name, T[] data) where T : Enum
        {
            return CreateEnumColumn(name, data, data.Length);
        }

        /// <summary>
        /// Modifies the contents of floatarray by replacing Nans and Infinities with MISSING_VALUE (it is Float.MAX_VAVLUE)
        /// </summary>
        /// <param name="dataorg">float[]</param>
        private static void RemoveNanAndInf(float[] dataorg)
        {
            for (int i = 0; i < dataorg.Length; i++)
            {
                float f = dataorg[i];
                if (float.IsNaN(f) || float.IsInfinity(f))
                {
                    dataorg[i] = float.MaxValue;
                }
            }
        }


        /// <summary>
        /// Modifies the contents of double array by replacing Nans and Infinities with MISSING_VALUE (it is Float.MAX_VAVLUE)
        /// </summary>
        /// <param name="dataorg">float[]</param>
        private static void RemoveNanAndInf(double[] dataorg)
        {
            for (int i = 0; i < dataorg.Length; i++)
            {
                double f = dataorg[i];
                if (double.IsNaN(f) || double.IsInfinity(f))
                {
                    dataorg[i] = double.MaxValue;
                }
            }
        }

        /// <summary>
        /// Modifies the contents of double array by replacing Nans and Infinities with MISSING_VALUE (it is Float.MAX_VAVLUE)
        /// </summary>
        /// <param name="dataorg">float[]</param>
        private static void RemoveNanAndInf(decimal[] dataorg)
        {
            for (int i = 0; i < dataorg.Length; i++)
            {
                double f = (double)dataorg[i];
                if (double.IsNaN(f) || double.IsInfinity(f))
                {
                    dataorg[i] = decimal.MaxValue;
                }
            }
        }

        /// <summary>
        /// Factory method to create a float column.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IColumn CreateFloatColumn(string name, float[] data, int size)
        {
            RemoveNanAndInf(data);
            IColumn c = new FloatColumn(name, data, size);
            return c;
        }

        public static IColumn CreateFloatColumn(string name, float[] data)
        {
            return CreateFloatColumn(name, data, data.Length);
        }

        /// <summary>
        /// Factory method to create a double column.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IColumn CreateDoubleColumn(string name, double[] data, int size)
        {
            RemoveNanAndInf(data);
            IColumn c = new DoubleColumn(name, data, size);
            return c;
        }

        public static IColumn CreateDoubleColumn(string name, double[] data)
        {
            return CreateDoubleColumn(name, data, data.Length);
        }


        /// <summary>
        /// Factory method to create a decimal column.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IColumn CreateDecimalColumn(string name, decimal[] data, int size)
        {
            RemoveNanAndInf(data);
            IColumn c = new DecimalColumn(name, data, size);
            return c;
        }

        public static IColumn CreateDecimalColumn(string name, decimal[] data)
        {
            return CreateDecimalColumn(name, data, data.Length);
        }


        /// <summary>
        /// Factory method to create a date column.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IColumn CreateDateColumn(string name, long[] data, int size)
        {
            DateColumn c = new DateColumn(name, data, size);
            return c;
        }


        public static IColumn CreateDateColumn(string name, long[] data)
        {
            return CreateDateColumn(name, data, data.Length);
        }

        /// <summary>
        /// Factory method to create a bit column.
        /// XXX: Does not create a disk based column
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IColumn CreateBitColumn(string name, BitArray data, int size) //#framework.
        {
            IColumn c = new BitColumn(name, data, size);
            return c;
        }

        /// <summary>
        /// Factory method to create a bit column.
        /// XXX: Does not create a disk based column
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IColumn CreateBitColumn(string name, BitArray data)
        {
            return CreateBitColumn(name, data, data.Count);
        }

        /// <summary>
        /// Factory method to create an Object column.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IColumn CreateObjectColumn(string name, object[] data, int size)
        {
            ObjectColumn c = new ObjectColumn(name, data, size);
            return c;
        }
        
     
        public static IColumn CreateObjectColumn(string name, object[] data)
        {
            return CreateObjectColumn(name, data, data.Length);
        }

        public static IMutableColumn CreateMutableColumn(string name, IColumn column)
        {
            return new MutableColumn(name, column);
        }

        /// <summary>
        ///  Factory method to create a subset of column. The subset indices are given by
        ///  the indices argument.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="indices"></param>
        /// <returns></returns>
        public static IColumn CreateSubsetColumn(IColumn column, int[] indices)
        {
            if (column is MutableColumn)
	            return SubsetMutableColumn(column, indices);

            else
                return new SubsetColumn(column, indices);
        }

        private static IColumn SubsetMutableColumn(IColumn column, int[] indices) //#framework
        {
            ArrayList newList = new ArrayList();
            int size = indices.Length;
            for (int i = 0; i < size; i++)
                newList.Add(column.Get(indices[i]));
            IMutableColumn c = new MutableColumn(column.GetName(), column.GetDatatype(), newList);
            c.GetMetaData().SetMark(column.GetMetaData().GetMark());
            c.SetCategorical(column.IsCategorical());
            return c;
        }

        public static IColumn CreateSupersetColumn(IColumn column, int[] indices, int size) //#framework
        {
            if (column is MutableColumn) {
                return SupersetMutableColumn(column, indices, size);
            }
            else
                return new SupersetColumn(column, indices, size);
        }


        public static IColumn CloneColumn(IColumn column)
        {
            return CloneColumn(column, Enumerable.Range(0, column.GetSize()).ToArray()); 
        }

        /// <summary>
        /// common method to clone given column.
        /// </summary>
        /// <param name="column">The column which needs to be cloned</param>
        /// <param name="indices">the subset row indices</param>
        /// <returns>clone of the input column based on given row indices, return null if type is not recognized</returns>
        public static IColumn CloneColumn(IColumn column, int[] indices) //#framework
        {
            String dataType = column.GetDatatype();
            IColumn clone = null;
            if (column is MutableColumn)
                clone = CloneMutableColumn(column);


            else if (dataType.Equals(DATATYPE_INT))
                clone = CloneIntColumn(column, indices);
            else if (dataType.Equals(DATATYPE_FLOAT))
                clone = CloneFloatColumn(column, indices);
            else if (dataType.Equals(DATATYPE_STRING))
                clone = CloneStringColumn(column, indices);
            else if (dataType.Equals(DATATYPE_DATE))
                clone = CloneDateColumn(column, indices);
            else if (dataType.Equals(DATATYPE_BIT))
                clone = CloneBitColumn(column, indices);
            else if (dataType.Equals(DATATYPE_OBJECT))
                clone = CloneObjectColumn(column, indices);



            if (clone != null)
                ColumnOperationUtil.UpdateMetaData(clone, column);

            return clone;
        }

        private static IColumn CloneIntColumn(IColumn column, int[] indices) //#framework
        {
            int length = indices.Length;
            int[] data = new int[length];
            for (int i = 0; i < length; i++)
                data[i] = column.GetInt(indices[i]);

            IColumn c = CreateIntColumn(column.GetName(), data);
            if (column.IsCategorical())
                c.SetCategorical(true);
            return c;
        }


        private static IColumn CloneFloatColumn(IColumn column, int[] indices)  //#framwork
        {
            int length = indices.Length;
            float[] data = new float[length];
            for (int i = 0; i < length; i++)
                data[i] = column.GetFloat(indices[i]);

            IColumn c = CreateFloatColumn(column.GetName(), data);
            if (column.IsCategorical())
                c.SetCategorical(true);
            return c;
        }

        private static IColumn CloneStringColumn(IColumn column, int[] indices) //#framework
        {
            int length = indices.Length;
            string[] data = new string[length];
            for (int i = 0; i < length; i++)
                data[i] = (string)column.Get(indices[i]);

            return CreateStringColumn(column.GetName(), data);
        }

        private static IColumn CloneDateColumn(IColumn column, int[] indices) //#framework
        {
            int length = indices.Length;
            long[] data = new long[length];
            for (int i = 0; i < length; i++)
            {
                DateTime date = (DateTime)column.Get(indices[i]);
                data[i] = (date != null ? date.Ticks : DatasetConstants.LONG_MV);
            }

            IColumn c = CreateDateColumn(column.GetName(), data);
            if (column.IsCategorical())
                c.SetCategorical(true);
            return c;
        }


        private static IColumn CloneBitColumn(IColumn column, int[] indices) //#framework
        {
            int length = indices.Length;
            BitArray data = new BitArray(length);
            for (int i = 0; i < length; i++)
                data.Set(i, bool.Parse(indices[i].ToString())); //Littel doubtful.

            return CreateBitColumn(column.GetName(), data);
        }

        private static IColumn CloneObjectColumn(IColumn column, int[] indices) //#framework
        {
            int length = indices.Length;
            object[] data = new object[length];
            for (int i = 0; i < length; i++)
                data[i] = column.Get(indices[i]);

            IColumn c = CreateObjectColumn(column.GetName(), data);
            if (column.IsCategorical())
                c.SetCategorical(true);
            return c;
        }

        private static IColumn CloneMutableColumn(IColumn column)
        {
            ArrayList data = DatasetUtil.GetColumnDataAsList(column);
            IMutableColumn c = new MutableColumn(column.GetName(), column.GetDatatype(), data);
            c.SetCategorical(column.IsCategorical());
            return c;
        }

        private static IColumn SupersetMutableColumn(IColumn column, int[] indices, int size)  //#framework
        {
            object[] newArray1 = new object[size];
            int listsize = indices.Length;
            for (int i = 0; i < listsize; i++)
            {
                newArray1[indices[i]] = column.Get(i);
            }
            IMutableColumn c = new MutableColumn(column.GetName(), column.GetDatatype(), new ArrayList(newArray1));
            c.GetMetaData().SetMark(column.GetMetaData().GetMark());
            c.SetCategorical(column.IsCategorical());
            return c;
        }


        public static object GetComparableMin(IColumn c, float min)
        {
            object omin;

            if (c.IsCategorical())
            {
                int minIndex = (int)(min + 0.5f);
                if (minIndex > c.GetCategoryCount() - 1)
                    minIndex = c.GetCategoryCount() - 1;
                omin = c.GetCategoryValue(minIndex);
            }
            else
            {
                if (c.GetDatatype().Equals(ColumnFactory.DATATYPE_INT))
                {
                    omin = int.Parse( Math.Round(min).ToString());
                }
                else if (c.GetDatatype().Equals(ColumnFactory.DATATYPE_FLOAT))
                {
                    omin = float.Parse(min.ToString());
                }
                else if (c.GetDatatype().Equals(ColumnFactory.DATATYPE_DATE))
                {
                    //XXX Anand: This might cause trucation errors
                    omin = new DateTime((long)Math.Round(min));
                    // } else if(c.getDatatype().equals(ColumnFactory.DATATYPE_MUTABLE)){
                }
                else if (c.GetDatatype().Equals(ColumnFactory.DATATYPE_BIT))
                {
                    if (min == 0.0f)
                        omin = false;
                    else
                        omin = true;
                }
                else if (c is MutableColumn) {
                    omin = ((IMutableColumn)c).GetDataObject(min);
                }

                else
                {
                    throw new Exception("huh??");
                }
            }

            return omin;
        }

        public static object GetComparableMax(IColumn c, float max)
        {
            object omax;

            if (c.IsCategorical())
            {
                int maxIndex = (int)(max);
                if (maxIndex > c.GetCategoryCount() - 1)
                    maxIndex = c.GetCategoryCount() - 1;
                omax = c.GetCategoryValue(maxIndex);
            }
            else
            {
                if (c.GetDatatype().Equals(ColumnFactory.DATATYPE_INT))
                {
                    omax = int.Parse(max.ToString());
                }
                else if (c.GetDatatype().Equals(ColumnFactory.DATATYPE_FLOAT))
                {
                    omax =  float.Parse(max.ToString());
                }
                else if (c.GetDatatype().Equals(ColumnFactory.DATATYPE_DATE))
                {
                    //XXX Anand: This might cause trucation errors
                    omax = new DateTime((long)max);
                    // }else if (c.getDatatype().equals(ColumnFactory.DATATYPE_MUTABLE)) {
                }
                else if (c.GetDatatype().Equals(ColumnFactory.DATATYPE_BIT))
                {
                    if (max == 0.0f)
                        omax = false;
                    else
                        omax = true;

                }
                else if (c is MutableColumn) {
                    omax = ((IMutableColumn)c).GetDataObject(max);
                }

                else
                {
                    throw new Exception("huh??");
                }
            }

            return omax;

        }

        /// <summary>
        /// Creates a union of data in the input columns. Size of output
        /// column is sum of size of input columns. MetaData
        /// of the column returned is same as first column.
        /// </summary>
        /// <param name="columns">array of columns for union</param>
        /// <returns>column with data of all the input column. </returns>
        public static IColumn UnionAll(IColumn[] columns)
        {

            if (columns == null)
            {
                return null;
            }

            int numCols = columns.Length;

            if (numCols <= 0)
            {
                return null;

            }
            else if (numCols == 1)
            {
                return CloneMutableColumn(columns[0]);
            }

            String datatype = columns[0].GetDatatype();
            IMutableColumn unionCol = new MutableColumn(columns[0]);

            for (int i = 1; i < numCols; i++)
            {
                IColumn col = columns[i];
                AppendCol(unionCol, col);
            }

            unionCol.SetMetaData(columns[0].GetMetaData());
            return unionCol;
        }

        private static void AppendCol(IMutableColumn mutCol, IColumn dataCol)
        {
            int size = dataCol.GetSize();
            for (int i = 0; i < size; i++)
            {
                mutCol.AddValue(dataCol.Get(i));
            }
        }

    }
}
