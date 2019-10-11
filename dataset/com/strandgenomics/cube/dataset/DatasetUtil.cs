using System;
using System.Collections;

namespace com.strandgenomics.cube.dataset
{
    public class DatasetUtil
    {
        public static bool IsFloatColumn(IColumn c)
        {
            return c.GetDatatype().Equals(ColumnFactory.DATATYPE_FLOAT);
        }
        public static bool IsIntegerColumn(IColumn c)
        {
            return c.GetDatatype().Equals(ColumnFactory.DATATYPE_INT);
        }

        internal static bool IsStringColumn(IColumn c)
        {
            throw new NotImplementedException();
        }

        internal static ArrayList GetColumnDataAsList(IColumn column)
        {
            throw new NotImplementedException();
        }

        internal static IDataset Transpose(object v)
        {
            throw new NotImplementedException();
        }

        internal static object GetColumnData(IColumn c)
        {
            throw new NotImplementedException();
        }
    }
}




