using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.strandgenomics.cube.dataset;
//#framework
//using com.strandgenomics.cube.framework.data.DefaultIntArray;
//using com.strandgenomics.cube.framework.data.DefaultFloatArray;
//using com.strandgenomics.cube.framework.data.DefaultLongArray;


namespace com.strandgenomics.cube.dataset.util
{
    public sealed class UpgradeableMutableColumn
    {
        private string name;
        private IColumnData columnData;
        private bool started;
        private int maxDataIndex;


        public UpgradeableMutableColumn(string colName)
        {
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

        }


        public void Add(string val)
        {
            val.Trim();
            if (!started)
            {
                CreateColumnData(val);
                started = true;
            }

            SetData(maxDataIndex, val);
            maxDataIndex++;
        }


        private void CreateColumnData(string token)
        {
            object value = GetDataType(token);

            if (value is float)
            {
                columnData = new FloatColumnData();
            }
            else if (value is int)
            {
                columnData = new IntColumnData();
            }
            else if (value is DateTime)
            {
                columnData = new DateColumnData();
            }
            else
            {
                columnData = new StringColumnData();
            }
        }

        private void SetData(int index, string val)
        {
            try
            {
                columnData.Set(index, val);
            }
            catch (DataException ne)
            {

                // convert from integer to float
                if ((columnData is IntColumnData) && (GetDataType(val) is float))
                {

                    IntColumnData oldColumnData = (IntColumnData)columnData;
                    FloatColumnData newColumnD = new FloatColumnData(maxDataIndex + 1);

                    for (int i = 0; i < maxDataIndex; i++)
                    {
                        int value = oldColumnData.GetInt(i);
                        if (value == DatasetConstants.INTEGER_MV)
                            newColumnD.AddFloat(DatasetConstants.FLOAT_MV);
                        else
                            newColumnD.AddFloat(value);
                    }
                    columnData = newColumnD;
                    columnData.Set(index, val);
                    return;
                }

                // convert from integer to string
                // convert from float to string
                // convert from date to string
                StringColumnData newColumnData = new StringColumnData(maxDataIndex + 1);
                for (int i = 0; i < maxDataIndex; i++)
                {
                    newColumnData.Add(columnData.Get(i));
                }
                columnData = newColumnData;
                columnData.Set(index, val);
                return;
            }
        }


        public IColumn GetColumn()
        {
            IColumn c = null;
            if (columnData is IntColumnData)
            {
                c = ColumnFactory.CreateIntColumn(name, (int[])columnData.GetData(), maxDataIndex);
            }


            else if (columnData is FloatColumnData)
            {
                c = ColumnFactory.CreateFloatColumn(name, (float[])columnData.GetData(), maxDataIndex);
            }


            else if (columnData is DateColumnData)
            {
                c = ColumnFactory.CreateDateColumn(name, (long[])columnData.GetData(), maxDataIndex);
            }


            else if (columnData is StringColumnData)
            {
                c = ColumnFactory.CreateStringColumn(name, (char[][])columnData.GetData(), maxDataIndex);
            }

            else if (columnData is DoubleColumnData)
            {
                c = ColumnFactory.CreateDoubleColumn(name, (double[])columnData.GetData(), maxDataIndex);
            }

            else if (columnData is DecimalColumnData)
            {
                c = ColumnFactory.CreateDecimalColumn(name, (decimal[])columnData.GetData(), maxDataIndex);
            }


            return c;
        }


        private object GetDataType(string tok)
        {
            if (tok == null || tok.Trim().Length == 0)
                return (DatasetConstants.INTEGER_MV);

            tok = tok.Trim();
            try
            {
                
                return int.TryParse(tok,out int x);
            }
            catch (DataException e)
            {
            }

            try
            {
                return float.TryParse(tok,out float x);
            }
            catch (DataException e)
            {
            }

            try
            {
                return decimal.TryParse(tok, out decimal x);
            }
            catch (DataException e)
            {
            }

            try
            {
                return double.TryParse(tok, out double x);
            }
            catch (DataException e)
            {
            }

            try
            {
                return DateTime.TryParse(tok, out DateTime result);
            }
            catch (DataException e)
            {
            }

            return tok;
        }




    }

    internal interface IColumnData
    {

        void Set(int index, string value);

        void Add(string value);

        string Get(int index);

        object GetData();
    }

    internal class IntColumnData : IColumnData
    {


        private int[] data;

        public IntColumnData()
        {
            data = new int[0];
            InitializeData();
        }

        public IntColumnData(int size)
        {
            data = new int[size];
            InitializeData();
        }

        private void InitializeData()
        {
            int size = data.Length;
            for (int i = 0; i < size; i++)
                data.Append(DatasetConstants.INTEGER_MV);
        }

        public void Set(int index, string value)
        {
            int size = data.Length;
            for (int i = size; i <= index; i++)
                data.Append(DatasetConstants.INTEGER_MV);
            if (value != null && value.Trim().Length != 0)
            {
                data[index] = int.Parse(value.Trim());
            }
        }

        public void Add(string value)
        {
            if (value != null && value.Trim().Length != 0)
            {
                data.Append(int.Parse(value));
            }
            else
            {
                data.Append(DatasetConstants.INTEGER_MV);
            }
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
            int[] column = new int[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                column[i] = data[i];
            }
            return column;
        }
    }

    internal class DoubleColumnData : IColumnData
    {

        //#framework
        private double[] data;

        public DoubleColumnData()
        {
            data = new double[0];
            InitializeData();
        }

        public DoubleColumnData(int size)
        {
            data = new double[size];
            InitializeData();
        }

        private void InitializeData()
        {
            int size = data.Length;
            for (int i = 0; i < size; i++)
                data.Append(DatasetConstants.DOUBLE_MV);
        }

        public void Set(int index, string value)
        {
            int size = data.Length;
            for (int i = size; i <= index; i++)
                data.Append(DatasetConstants.DOUBLE_MV);

            if (value != null && value.Trim().Length != 0)
            {
                data[index] = double.Parse(value);
            }
        }

        public void Add(string value)
        {
            if (value != null && value.Trim().Length != 0)
            {
                data.Append(double.Parse(value));
            }
            else
            {
                data.Append(DatasetConstants.DOUBLE_MV);
            }
        }

        public double GetDouble(int index)
        {
            return data[index];
        }

        public string Get(int index)
        {
            if (data[index] == DatasetConstants.FLOAT_MV)
                return null;
            return data[index].ToString();
        }

        public void AddDouble(double f)
        {
            data.Append(f);
        }
        public object GetData()
        {
            double[] column = new double[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                column[i] = data[i];
            }
            return column;
        }
    }


    internal class DecimalColumnData : IColumnData
    {

        //#framework
        private decimal[] data;

        public DecimalColumnData()
        {
            data = new decimal[0];
            InitializeData();
        }

        public DecimalColumnData(int size)
        {
            data = new decimal[size];
            InitializeData();
        }

        private void InitializeData()
        {
            int size = data.Length;
            for (int i = 0; i < size; i++)
                data.Append(DatasetConstants.DECIMAL_MV);
        }

        public void Set(int index, string value)
        {
            int size = data.Length;
            for (int i = size; i <= index; i++)
                data.Append(DatasetConstants.DECIMAL_MV);

            if (value != null && value.Trim().Length != 0)
            {
                data[index] = decimal.Parse(value);
            }
        }

        public void Add(string value)
        {
            if (value != null && value.Trim().Length != 0)
            {
                data.Append(decimal.Parse(value));
            }
            else
            {
                data.Append(DatasetConstants.DECIMAL_MV);
            }
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

        public void AddDecimal(decimal f)
        {
            data.Append(f);
        }
        public object GetData()
        {
            decimal[] column = new decimal[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                column[i] = data[i];
            }
            return column;
        }
    }



    internal class FloatColumnData : IColumnData
    {

        //#framework
        private float[] data;

        public FloatColumnData()
        {
            data = new float[0];
            InitializeData();
        }

        public FloatColumnData(int size)
        {
            data = new float[size];
            InitializeData();
        }

        private void InitializeData()
        {
            int size = data.Length;
            for (int i = 0; i < size; i++)
                data.Append(DatasetConstants.FLOAT_MV);
        }

        public void Set(int index, string value)
        {
            int size = data.Length;
            for (int i = size; i <= index; i++)
                data.Append(DatasetConstants.FLOAT_MV);

            if (value != null && value.Trim().Length != 0)
            {
                data[index] = float.Parse(value);
            }
        }

        public void Add(string value)
        {
            if (value != null && value.Trim().Length != 0)
            {
                data.Append(float.Parse(value));
            }
            else
            {
                data.Append(DatasetConstants.FLOAT_MV);
            }
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

        public void AddFloat(float f)
        {
            data.Append(f);
        }
        public object GetData()
        {
            float[] column = new float[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                column[i] = data[i];
            }
            return column;
        }
    }

    internal class DateColumnData : IColumnData
    {


        private long[] data;

        public DateColumnData()
        {
            data = new long[0];
            InitializeData();
        }

        public DateColumnData(int size)
        {
            data = new long[size];
            InitializeData();
        }

        private void InitializeData()
        {
            int size = data.Length;
            for (int i = 0; i < size; i++)
                data.Append(DatasetConstants.LONG_MV);
        }

        public void Set(int index, string value)
        {
            int size = data.Length;
            for (int i = size; i < index; i++)
                data.Append(DatasetConstants.LONG_MV);
            if (value != null && value.Trim().Length != 0)
            {
                data[index] = DateTime.Parse(value).Ticks;
            }
        }

        public void Add(string value)
        {
            if (value != null && value.Trim().Length != 0)
            {
                data.Append(DateTime.Parse(value).Ticks);
            }
            else
            {
                data.Append(DatasetConstants.LONG_MV);
            }
        }

        public string Get(int index)
        {
            if (data[index] == DatasetConstants.LONG_MV)
                return null;
            return new DateTime(data[index]).ToString();
        }

        public object GetData()
        {
            long[] column = new long[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                column[i] = data[i];
            }
            return column;
        }
    }


    internal class StringColumnData : IColumnData
    {


        private ArrayList data;

        public StringColumnData()
        {
            data = new ArrayList();
            //initializeData();
        }

        public StringColumnData(int size)
        {
            data = new ArrayList(size);
            //initializeData();
        }

        private void InitializeData()
        {
            int size = data.Count;
            for (int i = 0; i < size; i++)
                data.Add(null);
        }

        public void Set(int index, string value)
        {
            int size = data.Count;

            //list needs to be resized before adding values
            for (int i = size; i <= index; i++)
                data.Add(null);
            if (value != null && value.Trim().Length != 0)
            {
                data.Insert(index, value.Trim());
            }

        }

        public void Add(string value)
        {
            if (value != null && value.Trim().Length != 0)
            {
                data.Add(value);
            }
            else
            {
                data.Add(null);
            }
        }

        public string Get(int index)
        {
            return new string((char[])data[index]);
        }

        public object GetData()
        {
            char[][] column = new char[data.Count][];
            for (int i = 0; i < data.Count; i++)
            {
                column[i] = (char[])data[i];
            }
            return column;
        }
    }


}
