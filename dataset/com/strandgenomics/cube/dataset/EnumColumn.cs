using System;
using System.Collections.Generic;

namespace com.strandgenomics.cube.dataset
{
    sealed public class EnumColumn<T>: AbstractRegularColumn where T : Enum
    {

        public static string FORMAT_ID = "cube.dataset.EnumColumn";
    
        static string DATATYPE = "enum";

        float totalSum = -1.0f;

        // the raw data associated with the column.
        private T[] data;

        public EnumColumn(string name, T[] data): this(name, data.Length, data)
        {
            
        }

        public EnumColumn(string name, int size, T[] data):base(name, size)
        {
            if (size > data.Length)
                throw new DataException("Invalid arguments. Cannot create a column with size larger than the provided data");

            this.data = data;
            SetCategorical(true);
        }

        /// <summary>
        ///  Invoked by ReflectionSerializer.
        /// </summary>
        private EnumColumn()
        {
            SetCategorical(true);
        }

        /// <summary>
        ///  Invoked by ReflectionSerializer.
        /// </summary>
        private void Initialize()
        {
            base.Init();
        }

        /// <summary>
        ///  returns an object representing column value
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override object Get(int index)
        {
            Enum val = data[index];
            return val;
        }

        public override IComparable GetComparable(int index)
        {
            return (IComparable)Get(index).ToString();
        }

        public override string GetDatatype()
        {
            return DATATYPE;
        }

        /// <summary>
        /// returns the float value at specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override float GetFloat(int index)
        {
            Enum val = data[index];
            float value = /*(float)val.ordinal();*/ 0;
            return value;
        }

        /// <summary>
        /// returns the integer value at specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override int GetInt(int index)
        {
            Enum val = data[index];
            //return val.ordinal();     need to figure how to type cast Enum to int or how to get the val.ordinal() alternative.
            return 0;
        }

        public override float GetNumericValue(int index)
        {
            return GetInt(index);
        }

        override public float GetSum()
        {
            if (IsCategorical())
                InvalidMethodException();

            if (totalSum == -1.0f)
                totalSum = ComputeSum();

            return totalSum;
        }

        private float ComputeSum()
        {
            int sum = 0;
            for (int i = 0; i < size; i++)
                sum += GetInt(i);
            return (float)sum;
        }

        /// <summary>
        /// Throws <code>DataException</code> when invoked with value false. 
        ///  A <code>StringColumn</code> cannot be made continuous, and is by
        ///  default always categorical.
        /// </summary>
        /// <param name="b">true to set the column as categorical.</param>
        override public void SetCategorical(bool b)
        {
            // Enum column does not support continuous type.
            if (!b)
                InvalidMethodException();
            base.SetCategorical(b);
        }
    }
}

