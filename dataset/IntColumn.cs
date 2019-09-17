using framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataset
{
    public sealed class IntColumn:AbstractRegularColumn
    {
        private  readonly string fORMAT_ID = "cube.dataset.IntColumn";
    
        private readonly string dATATYPE = "integer";

        /// <summary>
        /// the raw data associated with the column.
        /// </summary>
        private int[] data;

        public string FORMAT_ID => fORMAT_ID;

        public string DATATYPE => dATATYPE;

        public IntColumn(string name, int[] data): base(name,data.Length)
        {
            Check(name,data,data.Length);
        }

        public IntColumn(string name, int[] data, int size) : base(name, size)
        {
            Check(name, data, size);
        }

        /// <summary>
        /// this method is generate to check for data validation
        ///  and also to generate same implementation as avadis java.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="size"></param>
        private void Check(string name, int[] data, int size)
        {
            if (size > data.Length)
                throw new DataException("Invalid arguments. Cannot create a column with size larger than the provided data");

            this.data = data;
        }

        /// <summary>
        /// invoked by ReflectionSerializer.
        /// </summary>
        private IntColumn()
        {
        }

        /// <summary>
        /// Invoked by ReflectionSerializer.
        /// </summary>
        private void Initialize()
        {
            base.Init();
        }


        public override object Get(int index)
        {
            return (data[index] == DatasetConstants.INTEGER_MV) ? null :  (object) data[index];
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
        /// returns float value at given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override float GetFloat(int index)
        {
            if (IsMissingValue(index))
                return DatasetConstants.FLOAT_MV;
            else
                return data[index];
        }

        /// <summary>
        /// returns inter value at given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override int GetInt(int index)
        {
            return data[index];
        }

        public override float GetNumericValue(int index)
        {
            if (IsCategorical())
                return GetCategoryIndex(index);
            else
                return data[index];
        }
        public override IntSet GetRowIndicesInRange(float min, bool minOpen, float max, bool maxOpen)
        {
            if (IsCategorical())
                return base.GetRowIndicesInRange(min, minOpen, max, maxOpen);

            // There is a problem with getRowIndicesInRange when one of the 
            // sides is open. 
            // for example range [4.0, 4.1) becomes [4, 4) after truncation and results 
            // empty indices.
            //
            // similarly (3.9, 4.9] becomes (4, 4] after truncation and results 
            // empty indices.
            //
            // This is a workaround for this problem.

            if (minOpen)
                min = (float)Math.Floor(min);
            else
                min = (float)Math.Ceiling(min);

            if (maxOpen)
                max = (float)Math.Ceiling(max);
            else
                max = (float)Math.Floor(max);

            return base.GetRowIndicesInRange(min, minOpen, max, maxOpen);
        }

        private new  void UpdateContinuousState()
        {
            int size = GetSize();

            // get data in contiguous array
            int[] sortdata = new int[size];
            int[] _sortOrder = new int[size];
            for (int i = 0; i < size; i++)
            {
                if (IsCategorical())
                    sortdata[i] = GetCategoryIndex(i);
                else
                    sortdata[i] = data[i];
                sortOrder[i] = i;
            }
            Array.Sort(sortdata, _sortOrder);

            // allocate sortOrder if required or update it.
            sortOrder = _sortOrder;

            // find maximum sort-index (i.e. excluding UNDEFINED values)
            int index = size - 1;
            while (index >= 0 && IsMissingValue(sortOrder[index]))
                index--;

            // TODO: check this
            mvCount = size - 1 - index;

            sum = 0.0f;
            for (int i = 0; i <= index; i++)
                sum += sortdata[i];

            sortdata = null;
        }
    }
}
