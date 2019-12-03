using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.strandgenomics.cube.dataset;

namespace com.strandgenomics.cube.dataset.util
{
    public sealed class ColumnConverter
    {
        /// <summary>
        /// Converts the original column from its type to the specified newType
        /// and returns the converted column.
        /// <para>
        /// newType can be one of (case-insensitive):<br>
        /// <pre>string, integer, float, date</pre>
        /// If the column is of the same type as newType, then the same column is
        /// returned.
        /// </para>
        /// </summary>
        /// <param name="newType"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static IColumn Convert(string newType, IColumn column)
        {
            string oldType = column.GetDatatype();
            if (oldType.Equals(newType))
                return column;

            object o;
            int size = column.GetSize();
            string columnName = column.GetName();
            if (newType.Equals("string"))
            {
                string[] newData = new string[size];
                for (int i = 0; i < size; i++)
                {
                    o = column.Get(i);
                    if (o != null)
                        newData[i] = o.ToString();
                }
                return ColumnFactory.CreateStringColumn(columnName, newData);
            }

            else if (newType.Equals("float"))
            {
                float[] newData = new float[size];
                for (int i = 0; i < size; i++)
                {
                    newData[i] = column.GetFloat(i);
                }
                return ColumnFactory.CreateFloatColumn(columnName, newData);
            }

            else if (newType.Equals("integer"))
            {
                int[] newData = new int[size];
                for (int i = 0; i < size; i++)
                {
                    newData[i] = column.GetInt(i);
                }
                return ColumnFactory.CreateIntColumn(columnName, newData);
            }

            else if (newType.Equals("decimal"))
            {
                decimal[] newData = new decimal[size];
                for (int i = 0; i < size; i++)
                {
                    newData[i] = column.GetDecimal(i);
                }
                return ColumnFactory.CreateDecimalColumn(columnName, newData);
            }
            else//if (newType.Equals("date"))
            { 
                long[] newData = new long[size];
                for (int i = 0; i < size; i++)
                {
                    newData[i] = (long)column.GetInt(i);
                }
                return ColumnFactory.CreateDateColumn(columnName, newData);
            }
        }


    }
}
