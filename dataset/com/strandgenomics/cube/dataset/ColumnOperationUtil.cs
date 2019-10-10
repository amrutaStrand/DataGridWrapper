using System;
namespace com.strandgenomics.cube.dataset

{
    public class ColumnOperationUtil 
    {
        protected const float FLOAT_MV = DatasetConstants.FLOAT_MV;
        protected const int INT_MV = DatasetConstants.INTEGER_MV;
        
        protected static bool IsNumericColumn(IColumn c)
        {
            return DatasetUtil.IsFloatColumn(c) || DatasetUtil.isIntegerColumn(c);
        }

        protected static float ToFloat(Object o)
        {
            return (float) o;
        }

        protected static int ToInt(Object o)
        {
            return (int) o;
        }

        internal static void UpdateMetaData(IColumn clone, IColumn column)
        {
            throw new NotImplementedException();
        }


        //protected static IColumn createFloatColumn(String name, float[] data, IColumn source)
        //{
        //    IColumn c = ColumnFactory.createFloatColumn(name, data);

        //    updateMetaData(c, source);
        //    return c;
        //}
    }
}

//public class ColumnOperationUtil
//{

//    protected static final float FLOAT_MV = IColumn.FLOAT_MV;
//    protected static final int INT_MV = IColumn.INTEGER_MV;

//    protected static boolean isNumericColumn(IColumn c)
//    {
//        return DatasetUtil.isFloatColumn(c) || DatasetUtil.isIntegerColumn(c);
//    }

//    protected static float toFloat(Object o)
//    {
//        return ((Number)o).floatValue();
//    }

//    protected static int toInt(Object o)
//    {
//        return ((Number)o).intValue();
//    }


//    // {{{ createXXXColumn 

//    /**
//     * Creates a float column with the specified name and data.
//     * mark and groupInfo of the source are propagated to the new column.
//     */
//    protected static IColumn createFloatColumn(String name, float[] data, IColumn source)
//    {
//        IColumn c = ColumnFactory.createFloatColumn(name, data);

//        updateMetaData(c, source);
//        return c;
//    }

//    /**
//     * Creates a float column with the specified name and data.
//     * mark and groupInfo of the source are propagated to the new column
//     * if both the sources have mark/groupInfo.
//     */
//    protected static IColumn createFloatColumn(
//        String name, float[] data,
//        IColumn source1, IColumn source2)
//    {

//        IColumn c = ColumnFactory.createFloatColumn(name, data);
//        updateMetaData(c, source1, source2);
//        return c;
//    }

//    /**
//     * Creates a int column with the specified name and data.
//     * mark and groupInfo of the source are propagated to the new column.
//     */
//    protected static IColumn createIntColumn(String name, int[] data, IColumn source)
//    {
//        IColumn c = ColumnFactory.createIntColumn(name, data);

//        updateMetaData(c, source);
//        return c;
//    }

//    /**
//     * Creates a int column with the specified name and data.
//     * mark and groupInfo of the source are propagated to the new column
//     * if both the sources have mark/groupInfo.
//     */
//    protected static IColumn createIntColumn(
//        String name, int[] data,
//        IColumn source1, IColumn source2)
//    {

//        IColumn c = ColumnFactory.createIntColumn(name, data);
//        updateMetaData(c, source1, source2);
//        return c;
//    }

//    // }}}

//    // {{{ updateMetaData
//    public static void updateMetaData(IColumn result, IColumn source)
//    {
//        ColumnMetaData p = source.getMetaData();
//        ColumnMetaData x = result.getMetaData();

//        x.setSource(p.getSource());
//        x.setMark(p.getMark());

//        Map ms = p.getState();
//        Map mr = x.getState();
//        mr.putAll(ms);
//    }

//    /**
//     * @param result result column
//     * @param a		first source column
//     * @param b		second source column
//     */
//    public static void updateMetaData(IColumn result, IColumn source1, IColumn source2)
//    {

//        ColumnMetaData p = source1.getMetaData();
//        ColumnMetaData q = source2.getMetaData();

//        ColumnMetaData x = result.getMetaData();

//        if (p.getSource() != null && p.getSource().equals(q.getSource()))
//            x.setSource(p.getSource());

//        if (p.getMark() != null && p.getMark().equals(q.getMark()))
//            x.setMark(p.getMark());
//    }

//    private static boolean safeEquals(Object a, Object b)
//    {
//        return (a == null) ? (b == null) : a.equals(b);
//    }
//    // }}}
//}

