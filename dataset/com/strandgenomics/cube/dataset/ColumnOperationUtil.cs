using System;
using System.Collections.Generic;
namespace com.strandgenomics.cube.dataset

{
    public class ColumnOperationUtil 
    {
        protected static float FLOAT_MV = DatasetConstants.FLOAT_MV;
        protected static int INT_MV = DatasetConstants.INTEGER_MV;
        
        public static bool IsNumericColumn(IColumn c)
        {
            return DatasetUtil.IsFloatColumn(c) || DatasetUtil.IsIntegerColumn(c);
        }

        public static float ToFloat(object o)
        {
            return (float) o;
        }

        public static int ToInt(object o)
        {
            return (int) o;
        }

        /// <summary>
        /// Creates a float column with the specified name and data.
        /// mark and groupInfo of the source are propagated to the new column.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        protected static IColumn CreateFloatColumn(string name, float[] data, IColumn source)
        {
            IColumn c = ColumnFactory.CreateFloatColumn(name, data);

            UpdateMetaData(c, source);
            return c;
        }

        /// <summary>
        /// Creates a float column with the specified name and data.
        /// mark and groupInfo of the source are propagated to the new column
        /// if both the sources have mark/groupInfo.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="source1"></param>
        /// <param name="source2"></param>
        /// <returns></returns>
        protected static IColumn CreateFloatColumn(
        string name, float[] data,
        IColumn source1, IColumn source2)
        {

            IColumn c = ColumnFactory.CreateFloatColumn(name, data);
            UpdateMetaData(c, source1, source2);
            return c;
        }

        /// <summary>
        /// Creates a int column with the specified name and data.
        /// mark and groupInfo of the source are propagated to the new column.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        protected static IColumn CreateIntColumn(string name, int[] data, IColumn source)
        {
            IColumn c = ColumnFactory.CreateIntColumn(name, data);

            UpdateMetaData(c, source);
            return c;
        }

        /// <summary>
        /// Creates a int column with the specified name and data.
        /// mark and groupInfo of the source are propagated to the new column
        /// if both the sources have mark/groupInfo.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="source1"></param>
        /// <param name="source2"></param>
        /// <returns></returns>
        protected static IColumn CreateIntColumn(
        string name, int[] data,
        IColumn source1, IColumn source2)
        {

            IColumn c = ColumnFactory.CreateIntColumn(name, data);
            UpdateMetaData(c, source1, source2);
            return c;
        }


        public static void UpdateMetaData(IColumn result, IColumn source)
        {
            ColumnMetaData p = source.GetMetaData();
            ColumnMetaData x = result.GetMetaData();

            x.SetSource(p.GetSource());
            x.SetMark(p.GetMark());

            Dictionary<object,object> ms = p.GetState();
            Dictionary<object, object> mr = x.GetState();
            foreach (var item in ms)
            {
                mr.Add(item.Key, item.Value);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="result">result column</param>
        /// <param name="source1">first source column</param>
        /// <param name="source2">second source column</param>
        public static void UpdateMetaData(IColumn result, IColumn source1, IColumn source2)
        {

            ColumnMetaData p = source1.GetMetaData();
            ColumnMetaData q = source2.GetMetaData();

            ColumnMetaData x = result.GetMetaData();

            if (p.GetSource() != null && p.GetSource().Equals(q.GetSource()))
                x.SetSource(p.GetSource());

            if (p.GetMark() != null && p.GetMark().Equals(q.GetMark()))
                x.SetMark(p.GetMark());
        }

        private static bool SafeEquals(object a, object b)
        {
            return (a == null) ? (b == null) : a.Equals(b);
        }



    }
}