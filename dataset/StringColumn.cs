using framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataset
{
    public sealed class StringColumn : AbstractRegularColumn
    {
        private const string fORMAT_ID = "cube.dataset.StringColumn";
    
        private const string dATATYPE = "string";

        public static string FORMAT_ID => fORMAT_ID;

        public static string DATATYPE => dATATYPE;

        public Comparator Comparator { get; private set; }

        private CharArrayData data;
        private char[][] inpData;
        private bool deleteOriginalData;

        /**
  * calls StringColumn with size = data.length
 */
        public StringColumn(String name, char[][] data): this(name, data, data.Length)
        {
            
        }

        public StringColumn(String name, char[][] data, Comparator comparator): this(name, data, data.Length, comparator)
        {
            
        }


        /**
         * Constructs a new <code>StringColumn</code> with the specified name
         * holding the specified data. The data is a two-dimensional character
         * array, where each column value is stored a char array. The size is
         * the size of the data array that actually constitutes the data of
         * the column.
         * @param name the name of the column.
         * @param inpData the data of the column.
         * @param size the size of the data to use in the column.
         */
        public StringColumn(String name, char[][] data, int size): this(name, data, size, false)
        {
            
        }

        public StringColumn(String name, char[][] data, int size, Comparator comparator): this(name, data, size, false, comparator)
        {
            
        }

        public StringColumn(String name, char[][] data, int size, bool deleteOriginalData): this(name, data, size, deleteOriginalData, null)
        {
            
        }

        public StringColumn(String name, char[][] data, int size, bool deleteOriginalData, Comparator comparator):base(name, size)
        {
            

            if (size > data.Length)
                throw new DataException("Invalid arguments. Cannot create a column with size larger than the provided data");

            if (comparator != null)
                this.Comparator = comparator;
            inpData = data;
            this.deleteOriginalData = deleteOriginalData;
            base.SetCategorical(true);
            this.UpdateState();
        }


        new private void UpdateState()
        {
            // if _sortOrder has already been computed, then nothing needs
            // to be done. State of a string column once computed will not
            // be altered. because string columns are always categorical,
            // unlinke other columns that can alternate between continuous
            // and categorical.
            if (sortOrder != null)
            {
                return;
            }

            // first use the original input data to update the state of the
            // column - categoryIndexArray, etc.
            base.UpdateState();

            // now create CharArrayData from the input data, and discard the
            // original input data.
            data = new CharArrayData(this, inpData, deleteOriginalData);
            inpData = null;
        }

        /** Invoked by ReflectionSerializer. */
        private StringColumn()
        {
            base.SetCategorical(true);
        }

        new public void SetCategorical(bool b)
        {
            // String column does not support continuous type.
            if (!b)
            {
                InvalidMethodException();
            }
            base.SetCategorical(b);
        }

        /// <summary>
        /// Invoked by ReflectionSerializer.
        /// </summary>
        private void Initialize()
        {
            base.Init(name, data.GetSize());

            // XXX: categoryIndexArray is computed unnecesarily.
            base.UpdateState();
        }

        public override object Get(int index)
        {
            return data.Get(index);
        }

        public override IComparable GetComparable(int index)
        {
            return (IComparable)data.Get(index);
        }

        public override string GetDatatype()
        {
            return DATATYPE;
        }

        public override float GetFloat(int index)
        {
            throw new NotImplementedException();
        }

        public override int GetInt(int index)
        {
            throw new NotImplementedException();
        }

        public override float GetNumericValue(int index)
        {
            throw new NotImplementedException();
        }

        //makeComparavle is not yet implemented.

    }

    internal class CharArrayData
    {
        public const string FORMAT_ID = "cube.dataset.StringColumnData";
        public string data;
        int[] categoryIndexArray;

        int[] catoffset;

        public CharArrayData(StringColumn column, char[][] inpData, bool deleteOriginalData)
        {
            //this.column = column;
            try
            {
                this.categoryIndexArray = column.GetCategoryIndexArray();
            }
            catch (DataException e)
            {
                // should not happen. string column is always categorical.
            }

            Init(column, inpData, deleteOriginalData);
        }

        /// <summary>
        /// Invoked by ReflectionSerializer. 
        /// </summary>
        public CharArrayData()
        {
        }
        private void Init(StringColumn column, char[][] inpData, bool deleteOriginalData)
        {
            int catcount = column.GetCategoryCount();

            // ignore missing-values category
            if (column.GetMissingValueCount() > 0)
                catcount--;

            // initialize offsets for each category

            // size of catoffset array is one more than category count because 
            // catoffset[n+1] - catoffset[n] can be used to get length of n'th category value.
            catoffset = new int[catcount + 1];

            int totalsize = 0;
            for (int i = 0; i < catcount; i++)
            {
                catoffset[i] = totalsize;

                IntSet intset = column.GetRowIndicesOfCategory(i);
                var indexlList = intset.GetEnumerator();
                indexlList.MoveNext();
                char[] value = inpData[indexlList.Current];
                totalsize += value.Length;
            }
            catoffset[catcount] = totalsize;

            // initialize the data
            char[] cData = new char[totalsize];
            for (int i = 0; i < catcount; i++)
            {
                IntSet intset = column.GetRowIndicesOfCategory(i);
                var indexlList = intset.GetEnumerator();
                indexlList.MoveNext();
                char[] d = inpData[indexlList.Current];

                // copy chars from inpData to char[]data
                Array.Copy(d, 0, cData, catoffset[i], d.Length);
                if (deleteOriginalData)
                    inpData[indexlList.Current] = null;
            }
            data = new String(cData);
        }

        public String Get(int index)
        {
            int catIndex = categoryIndexArray[index];
            if (catIndex >= (catoffset.Length - 1))
                return null;

            // begining and length of the string in the big data array
            return data.Substring(catoffset[catIndex], catoffset[catIndex + 1]);
        }
        public int GetSize()
        {
            return categoryIndexArray.Length;
        }
    }
}
