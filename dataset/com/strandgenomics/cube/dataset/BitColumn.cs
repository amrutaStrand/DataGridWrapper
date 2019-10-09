using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.strandgenomics.cube.dataset
{
    sealed public class BitColumn : AbstractRegularColumn
    {
        public static string FORMAT_ID = "cube.dataset.BitColumn";
    
        static string DATATYPE = "bitset";

        private BitArray data;


        public BitColumn(string name, BitArray data): this(name, data, data.Length)
        {
        }

        public BitColumn(string name, BitArray data, int size):base(name, size)
        {
            if (size > data.Length)
                throw new DataException("Invalid arguments. Cannot create a column with size larger than the provided data");

            this.data = data;
        }

        /// <summary>
        /// Invoked by ReflectionSerializer.
        /// </summary>
        private BitColumn()
        {
        }

        /// <summary>
        /// Invoked by ReflectionSerializer.
        /// </summary>
        private void Initialize()
        {
            base.Init();
        }

        /// <summary>
        /// returns an object representing column value
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override object Get(int index)
        {
            return data.Get(index);
        }

        public override IComparable GetComparable(int index)
        {
            return (IComparable)Get(index);
        }

        public override string GetDatatype()
        {
            return DATATYPE;
        }

        /// <summary>
        ///  returns the float value at specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override float GetFloat(int index)
        {
            if (data.Get(index))
                return 1.0f;
            else
                return 0.0f;
        }

        /// <summary>
        /// returns the integer value at specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override int GetInt(int index)
        {
            if (data.Get(index))
                return 1;
            else
                return 0;
        }

        public override float GetNumericValue(int index)
        {
            if (IsCategorical())
                return GetCategoryIndex(index);

            if (data.Get(index))
                return 1.0f;
            else
                return 0.0f;
        }
        override public float GetSum()
        {
            int sum = 0;
            for(int i = 0; i < data.Count; i++)
            {
                if (data.Get(i))
                {
                    sum++;
                }
            }
            return sum;
        }
    }
}
