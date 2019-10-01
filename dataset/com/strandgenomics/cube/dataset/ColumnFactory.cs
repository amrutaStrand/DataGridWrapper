using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.strandgenomics.cube.dataset
{
    
    public class ColumnFactory
    {
        public static String DATATYPE_STRING = StringColumn.DATATYPE;
        public static String DATATYPE_FLOAT = FloatColumn.DATATYPE;
        public static String DATATYPE_INT = IntColumn.DATATYPE;
        //public const String DATATYPE_DATE = DateColumn.DATATYPE;
        //public const String DATATYPE_BIT	= BitColumn.DATATYPE;
        //public const String DATATYPE_OBJECT 	= ObjectColumn.DATATYPE;
        //public const String DATATYPE_ENUM    = EnumColumn.DATATYPE;
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
