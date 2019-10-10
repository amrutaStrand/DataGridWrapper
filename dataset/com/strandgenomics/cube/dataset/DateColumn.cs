using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.strandgenomics.cube.dataset
{
    sealed public class DateColumn : AbstractRegularColumn
    {

        public static string FORMAT_ID = "cube.dataset.DateColumn";

        public static string DATATYPE = "date";

        /// <summary>
        /// the raw data associated with the column.
        /// </summary>
        private long[] data;

        public DateColumn(String name, long[] data): this(name, data, data.Length)
        {
            
        }
        /// <summary>
        /// Invoked by ReflectionSerializer.
        /// </summary>
        private DateColumn()
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
        /// constructs a DateColumn with the specified name and with
        /// the data specified. The data is internally maintained as a
        /// array of long values.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="size"></param>
        public DateColumn(string name, long[] data, int size):base(name, size)
        {
            this.data = data;
            SetCategorical(true);
        }

        public override object Get(int index)
        {
            DateTime? nullDateTime = null;
            return (data[index] == DatasetConstants.LONG_MV) ? nullDateTime : new DateTime(data[index]);
        }

        public override IComparable GetComparable(int index)
        {
            return (IComparable)Get(index);
        }

        public override string GetDatatype()
        {
            return DATATYPE;
        }

        override public float GetSum()
        {
            InvalidMethodException();

            return 0;
        }

        public override float GetFloat(int index)
        {
            if (IsMissingValue(index))
                return DatasetConstants.FLOAT_MV;
            else
                return (float)data[index];
        }

        /// <summary>
        /// returns the integer value at specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
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
                return GetFloat(index);
        }
    }
}
