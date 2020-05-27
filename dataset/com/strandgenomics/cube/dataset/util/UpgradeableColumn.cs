using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.strandgenomics.cube.dataset;

namespace com.strandgenomics.cube.dataset.util
{
    public sealed class UpgradeableColumn
    {
        private int size;
        private int resize;
        private string name;
        private IColumnDataUC columnData;
        private bool started;
        private int maxDataIndex;


        public UpgradeableColumn(int colSize, string colName)
        {
            size = colSize;
            name = colName;
            started = false;
            maxDataIndex = 0;
        }

        public void Set(int index, string val)
        {

            if (!started)
            {
                CreateColumnData(val);
                started = true;
            }

            SetData(index, val);

            if (index >= maxDataIndex)
                maxDataIndex = index + 1;

            //maxDataIndex++;
        }

        public void Add(string val)
        {
            Set(maxDataIndex, val);
        }

        private void CreateColumnData(string token)
        {
            object value = GetDataType(token);
            if (value is float)
            {
                columnData = new FloatColumnDataUC(size);
            }
            else if (value is int)
            {
                columnData = new IntColumnDataUC(size);
            }
            else if (value is DateTime)
            {
                columnData = new DateColumnDataUC(size);
            }
            else
            {
                columnData = new StringColumnDataUC(size);
            }
        }

        private void SetData(int index, String val)
        {
            try
            {
                /* While deciding the columnData type, if the first value is a float, a float columnData is created and all subsequent
                    values are parsed as float. This case works fine.
                    But this is the issue : if the first value is an integer, an integer columnData is created. If say, any of the subsequent values 
                    is a float e.g - 6.5, then parsing that value as an integer in german locale considers the dot(.) as a grouping character
                    and gives the result as 65. The following check avoids this by throwing a Data Exception and converting all values to float.
                */
                if ((columnData is IntColumnDataUC) && (GetDataType(val) is float))
                    throw new DataException("Unable to parse value \"" + val + "\" as an integer.");
                else
                    columnData.Set(index, val);
            }
            catch (DataException ne)
            {

                // convert from integer to float
                if ((columnData is IntColumnDataUC) && (GetDataType(val) is float))
                {

                    IntColumnDataUC oldColumnData = (IntColumnDataUC)columnData;
                    FloatColumnDataUC newColumnData = new FloatColumnDataUC(size);

                    for (int i = 0; i < maxDataIndex; i++)
                    {
                        int value = oldColumnData.GetInt(i);
                        if (value == DatasetConstants.INTEGER_MV)
                            newColumnData.SetFloat(i, DatasetConstants.FLOAT_MV);
                        else
                            newColumnData.SetFloat(i, value);
                    }
                    columnData = newColumnData;
                    columnData.Set(index, val);
                    return;
                }

                // convert from integer to string
                // convert from float to string
                // convert from date to string
                {
                    StringColumnDataUC newColumnData = new StringColumnDataUC(size);
                    for (int i = 0; i < maxDataIndex; i++)
                    {
                        newColumnData.Set(i, columnData.Get(i));
                    }
                    columnData = newColumnData;
                    columnData.Set(index, val);
                    return;
                }
            }
        }

        public IColumn GetColumn()
        {
            IColumn c = null;
            if (columnData is IntColumnDataUC)
            {
                c = ColumnFactory.CreateIntColumn(name, (int[])columnData.GetData(), maxDataIndex);
            }


            else if (columnData is FloatColumnDataUC)
            {
                c = ColumnFactory.CreateFloatColumn(name, (float[])columnData.GetData(), maxDataIndex);
            }

            else if (columnData is DoubleColumnDataUC)
            {
                c = ColumnFactory.CreateDoubleColumn(name, (double[])columnData.GetData(), maxDataIndex);
            }
            else if (columnData is DecimalColumnDataUC)
            {
                c = ColumnFactory.CreateDecimalColumn(name, (decimal[])columnData.GetData(), maxDataIndex);
            }


            else if (columnData is DateColumnDataUC)
            {
                c = ColumnFactory.CreateDateColumn(name, (long[])columnData.GetData(), maxDataIndex);
            }


            else if (columnData is StringColumnDataUC)
            {
                c = ColumnFactory.CreateStringColumn(name, (char[][])columnData.GetData(), maxDataIndex);
            }

            return c;


        }

        private object GetDataType(string tok)
        {
            if (tok == null)
                return DatasetConstants.INTEGER_MV;
            try
            {
                // this check is for xx.00 values which should be parsed as float not as integer.
                if (!tok.Contains("."))
                    return int.Parse(tok);
            }
            catch (DataException e)
            {
            }

            try
            {
                return float.Parse(tok);
            }
            catch (DataException e)
            {
            }

            try
            {
                return DateTime.Parse(tok);
            }
            catch (DataException e)
            {
            }

            return tok;
        }


    }

    internal interface IColumnDataUC
    {

        void Set(int index, string value);

        string Get(int index);

        object GetData();
    }

    internal class IntColumnDataUC : IColumnDataUC
    {


        private int[] data;

        public IntColumnDataUC(int size)
        {
            data = new int[size];
            InitializeData();
        }

        private void InitializeData()
        {
            for (int i = 0; i < data.Length; i++)
                data[i] = DatasetConstants.INTEGER_MV;
        }

        public void Set(int index, string value)
        {
            data[index] = int.Parse(value);
        }

        public int GetInt(int index)
        {
            return data[index];
        }

        public string Get(int index)
        {
            if (data[index] == DatasetConstants.INTEGER_MV)
                return null;
            return data[index].ToString();
        }

        public object GetData()
        {
            return data;
        }

    }

    internal class FloatColumnDataUC : IColumnDataUC
    {


        private float[] data;

        public FloatColumnDataUC(int size)
        {
            data = new float[size];
            InitializeData();
        }

        private void InitializeData()
        {
            for (int i = 0; i < data.Length; i++)
                data[i] = DatasetConstants.FLOAT_MV;
        }

        public void Set(int index, string value)
        {
            data[index] = float.Parse(value);
        }

        public float GetFloat(int index)
        {
            return data[index];
        }

        public string Get(int index)
        {
            if (data[index] == DatasetConstants.FLOAT_MV)
                return null;
            return data[index].ToString();
        }

        public void SetFloat(int index, float f)
        {
            data[index] = f;
        }

        public object GetData()
        {
            return data;
        }
    }



    internal class DoubleColumnDataUC : IColumnDataUC
    {


        private double[] data;

        public DoubleColumnDataUC(int size)
        {
            data = new double[size];
            InitializeData();
        }

        private void InitializeData()
        {
            for (int i = 0; i < data.Length; i++)
                data[i] = DatasetConstants.DOUBLE_MV;
        }

        public void Set(int index, string value)
        {
            data[index] = double.Parse(value);
        }

        public double GetDouble(int index)
        {
            return data[index];
        }

        public string Get(int index)
        {
            if (data[index] == DatasetConstants.DOUBLE_MV)
                return null;
            return data[index].ToString();
        }

        public void SetDouble(int index, double f)
        {
            data[index] = f;
        }

        public object GetData()
        {
            return data;
        }
    }


    internal class DecimalColumnDataUC : IColumnDataUC
    {


        private decimal[] data;

        public DecimalColumnDataUC(int size)
        {
            data = new decimal[size];
            InitializeData();
        }

        private void InitializeData()
        {
            for (int i = 0; i < data.Length; i++)
                data[i] = DatasetConstants.DECIMAL_MV;
        }

        public void Set(int index, string value)
        {
            data[index] = decimal.Parse(value);
        }

        public decimal GetDecimal(int index)
        {
            return data[index];
        }

        public string Get(int index)
        {
            if (data[index] == DatasetConstants.DECIMAL_MV)
                return null;
            return data[index].ToString();
        }

        public void SetDecimal(int index, decimal f)
        {
            data[index] = f;
        }

        public object GetData()
        {
            return data;
        }
    }



    internal class DateColumnDataUC : IColumnDataUC
    {


        private long[] data;

        public DateColumnDataUC(int size)
        {
            data = new long[size];
            InitializeData();
        }

        private void InitializeData()
        {
            for (int i = 0; i < data.Length; i++)
                data[i] = DatasetConstants.LONG_MV;
        }

        public void Set(int index, string value)
        {
            data[index] = DateTime.Parse(value).Ticks;
        }

        public string Get(int index)
        {
            if (data[index] == DatasetConstants.LONG_MV)
                return null;
            return new DateTime(data[index]).ToString();
        }

        public object GetData()
        {
            return data;
        }
    }

    internal class StringColumnDataUC : IColumnDataUC
    {


        private char[][] data;

        public StringColumnDataUC(int size)
        {
            data = new char[size][];
        }

        public void Set(int index, string value)
        {
            data[index] = value.ToCharArray();
        }

        public string Get(int index)
        {
            return new string(data[index]);
        }

        public object GetData()
        {
            return data;
        }
    }

}
