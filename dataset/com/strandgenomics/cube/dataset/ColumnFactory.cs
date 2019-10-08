using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.strandgenomics.cube.dataset
{
    
    public class ColumnFactory
    {
        public static string DATATYPE_STRING = StringColumn.DATATYPE;
        public static string DATATYPE_FLOAT = FloatColumn.DATATYPE;
        public static string DATATYPE_INT = IntColumn.DATATYPE;
        //public const string DATATYPE_DATE = DateColumn.DATATYPE;
        //public const string DATATYPE_BIT = BitColumn.DATATYPE;
        public static string DATATYPE_OBJECT = ObjectColumn.DATATYPE;
        //public const string DATATYPE_ENUM = EnumColumn.DATATYPE;
        internal static object GetComparableMin(AbstractRegularColumn abstractRegularColumn, float min)
        {
            throw new NotImplementedException();
        }

        internal static object GetComparableMax(AbstractRegularColumn abstractRegularColumn, float max)
        {
            throw new NotImplementedException();
        }
    }
}
