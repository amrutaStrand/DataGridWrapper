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
