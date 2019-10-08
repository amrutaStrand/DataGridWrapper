using System;

namespace com.strandgenomics.cube.dataset
{
    public class DatasetUtil
    {
        public static bool isFloatColumn(IColumn c)
        {
            return c.GetDatatype().Equals(ColumnFactory.DATATYPE_FLOAT);
        }
        public static bool isIntegerColumn(IColumn c)
        {
            return c.GetDatatype().Equals(ColumnFactory.DATATYPE_INT);
        }

        internal static System.Collections.Generic.List<object> GetColumnDataAsList(IColumn column)
        {
            throw new NotImplementedException();
        }
    }
}




