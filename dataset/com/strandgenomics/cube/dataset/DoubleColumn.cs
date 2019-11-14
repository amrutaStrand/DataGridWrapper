using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.strandgenomics.cube.dataset
{
    public class DoubleColumn : AbstractRegularColumn
    {
        private static string fORMAT_ID = "cube.dataset.DoubleColumn";

        private static string dATATYPE = "double";

        /// <summary>
        /// raw data associtated with column.
        /// </summary>
        private double[] data;
        public DoubleColumn(string name, double[] data) : base(name, data.Length)
        {
            this.data = data;
        }

        public DoubleColumn(string name, double[] data, int size) : base(name, size)
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
            return (data[index] == DatasetConstants.Double_MV) ? null : (object)data[index];
        }

        public override IComparable GetComparable(int index)
        {
            return (IComparable)Get(index);
        }

        public override string GetDatatype()
        {
            return DATATYPE;
        }

        public override float GetFloat(int index)
        {
            return (float)data[index];
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
                return (float)data[index];
        }

        public double GetDouble(int index)
        {
            return data[index];
        }
    }
}
