using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.strandgenomics.cube.dataset
{
    public sealed class FloatColumn : AbstractRegularColumn
    {
        private static string fORMAT_ID = "cube.dataset.FloatColumn";
    
        private static string dATATYPE = "float";

        /// <summary>
        /// raw data associtated with column.
        /// </summary>
        private float[] data;
        public FloatColumn(string name, float[] data):base(name,data.Length)
        {
            this.data = data;
        }

        public FloatColumn(string name, float[] data, int size):base(name, size)
        {
            if (size > data.Length)
            {
                throw new DataException("Invalid arguments. Cannot create a column with size larger than the provided data");
            }
            this.data = data;
        }

        public static string FORMAT_ID => fORMAT_ID;

        public static string DATATYPE => dATATYPE;

        public override object Get(int index)
        {
            return (data[index] == DatasetConstants.FLOAT_MV) ? null : (object) data[index];
        }

        public override IComparable GetComparable(int index)
        {
            return (IComparable)Get(index);
        }

        public override string GetDatatype()
        {
            return DATATYPE;
        }

        public override decimal GetDecimal(int index)
        {
            if (IsMissingValue(index))
                return DatasetConstants.DECIMAL_MV;
            else
                return (decimal)data[index];
        }

        public override double GetDouble(int index)
        {
            if (IsMissingValue(index))
                return DatasetConstants.DOUBLE_MV;
            else
                return (double)data[index];
        }

        public override float GetFloat(int index)
        {
            return data[index];
        }

        public override int GetInt(int index)
        {
            if (IsMissingValue(index))
                return DatasetConstants.INTEGER_MV;
            else
                return (int)data[index];
        }

        public override float GetNumericValue(int index)
        {
            if (IsCategorical())
                return GetCategoryIndex(index);
            else
                return data[index];
        }
    }
}
