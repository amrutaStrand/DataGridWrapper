﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataset
{
    public sealed class FloatColumn : AbstractRegularColumn
    {
        private readonly string fORMAT_ID = "cube.dataset.FloatColumn";
    
        private readonly string dATATYPE = "float";

        /// <summary>
        /// raw data associtated with column.
        /// </summary>
        private float[] data;
        public FloatColumn(String name, float[] data):base(name,data.Length)
        {
            this.data = data;
        }

        public FloatColumn(String name, float[] data, int size):base(name, size)
        {
            if (size > data.Length)
            {
                throw new DataException("Invalid arguments. Cannot create a column with size larger than the provided data");
            }
            this.data = data;
        }

        public string FORMAT_ID => fORMAT_ID;

        public string DATATYPE => dATATYPE;

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
