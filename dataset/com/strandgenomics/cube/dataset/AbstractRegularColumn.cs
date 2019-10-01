using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using framework;

namespace com.strandgenomics.cube.dataset
{
    public abstract class AbstractRegularColumn : AbstractColumn
    {
        
        /// <summary>
        /// The name of the column.
        /// </summary>
        protected string name;

        /// <summary>
        ///  Number of values in the column.
        /// </summary>
        protected int size;

        /// <summary>
        /// number of missing values in the column.   
        /// </summary>
        protected int mvCount;

        /// <summary>
        /// sum of values in the column.
        /// </summary>
        protected float sum;

        /// <summary>
        ///  Array to store the row indices in the sorted order.
        /// </summary>
        protected int[] sortOrder;

        //removed the sortedOrderReference since we canf simply use ref sortedOrder


        /// <summary>
        ///  Map from category index to rank of the category, when all
        /// values in the column are sorted - in the sortOrder array.
        /// Size of this array is number of categories + 1. 
        /// The last entry is always equal to size. 
        /// Because of this extra entry, size of a category can be found just by
        /// _categoryRank[catIndex+1] - _categoryRank[catIndex].
        /// This field should not be used directly. <code>GetCategoryRank()</code>
        /// method should be used instead.
        /// </summary>
        protected int[] categoryRank;

        /// <summary>
        ///  Array to store category index for each element.
        ///  here is change from avadis java there is a optimizedintArray used here simple Array is used. we can use list as well which ever is optimal.
        /// </summary>
        protected int[] categoryIndexArray;

        /// <summary>
        /// Meta data for the column.
        /// </summary>
        protected ColumnMetaData metaData;

        /// <summary>
        /// flag for maintaining whether the state of the column is valid or not.
        /// </summary>
        protected bool isCategorical;

        /// <summary>
        /// flag for maintaining whether the state of the column is valid or not.
        /// </summary>
        protected bool stateNeedsUpdate = true;

        public AbstractRegularColumn(string name, int size)
        {
            Init(name, size);
        }

        /// <summary>
        ///  Default constructor. 
        /// Its is the responsibility of the caller to ensure<code> init</code>
        /// method is called later.
        /// </summary>
        protected AbstractRegularColumn()
        {
        }

        protected void Init(string name, int size)
        {
            this.name = name;
            this.size = size;
            Init();
        }

        /// <summary>
        /// invoked through hexff.
        /// </summary>
        protected void Init()
        {
            if (metaData == null)
                metaData = new ColumnMetaData();
        }



      
        public override int GetCategoryCount()
        {
            int[] categoryRank = GetCategoryRank();
            return categoryRank.Length - 1;
        }

        public override int GetCategoryIndex(int rowIndex)
        {
            int[] categoryIndexArray = GetCategoryIndexArray();
            return categoryIndexArray[rowIndex];
        }

        public override int GetCategorySize(int categoryIndex)
        {
            int[] categoryRank = GetCategoryRank();
            return categoryRank[categoryIndex + 1] - categoryRank[categoryIndex];
        }

        public override object GetCategoryValue(int categoryIndex)
        {
            int[] categoryRank = GetCategoryRank();
            int[] _sortOrder = GetSortOrder();

            return Get(_sortOrder[categoryRank[categoryIndex]]);
        }

        protected int[] GetCategoryRank()
        {
            if (!IsCategorical())
                InvalidMethodException();

            if (stateNeedsUpdate)
                UpdateState();

            return categoryRank;
        }

        public int[] GetCategoryIndexArray()
        {
            if (!IsCategorical())
                InvalidMethodException();

            if (stateNeedsUpdate)
                UpdateState();

            return categoryIndexArray;
        }

      
        //Commeneted these sice the they were not implemented here in java avadis  

        //public override IComparable GetComparable(int index)
        //{
        //    throw new NotImplementedException();
        //}

        //public override string GetDatatype()
        //{
        //    throw new NotImplementedException();
        //}

        //public override float GetFloat(int index)
        //{
        //    throw new NotImplementedException();
        //}

        //public override int GetInt(int index)
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// returns the index of the element with maximum value
        /// </summary>
        /// <returns></returns>
        public override int GetMaxIndex()
        {
            int maxSortIndex = GetMaxSortIndex();

            // case of all missing values
            if (maxSortIndex < 0)
                return 0;

            return GetSortOrder()[maxSortIndex];
        }

        /// <summary>
        /// Returns the sort-index of the maximum DEFINED value. 
        ///  Returns -1 if all values in the column are missing values.
        /// </summary>
        /// <returns></returns>
        private int GetMaxSortIndex()
        {
            int maxIndex = GetSize() - 1;

            // for categorical columns, maxSortIndex is same as maxIndex.
            if (IsCategorical())
                return maxIndex;

            // for continuous columns
            int mvcount = GetMissingValueCount();
            return maxIndex - mvcount;
        }

        /// <summary>
        ///  Finds the smallest entry greater than or equal to value, and returns
        ///  the index of the element in the sortOrder array. If the value is larger 
        ///  than any of the column entries, then size of the column is returned.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int GetCeilIndex(Object value)
        {
            int[] order = GetSortOrder();

            IComparable key = (IComparable)value;

            int m = 0;
            int left = 0;
            // Start with right as 1 more than the rightmost index
            // of the column which is getSize()-1. So if the value 
            // is larger than any of the column entries, then getSize()
            // will be returned.
            int right = GetSize();

            while (right > left)
            {
                m = (left + right) / 2;

                IComparable v = GetComparable(order[m]);
                int ccode = SafeCompare(key, v);
                if (m == left)  // terminate 
                    return (ccode > 0) ? right : left;

                if (ccode > 0)
                    left = m;
                else
                    right = m;
            }

            return 0;
        }

        /// <summary>
        /// Finds the largest entry less than or equal to value, and returns
        /// the index of the element in the sortOrder array. If the value is smaller
        /// than all the entries in the column, then -1 is returned.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int GetFloorIndex(object value)
        {
            int[] order = GetSortOrder();

            IComparable key = (IComparable)value;

            int m = 0;
            // Start with left as -1 more than the leftmost index
            // of the column which is 0. So if the value 
            // is smaller than any of the column entries, then -1
            // will be returned.
            int left = -1;
            int right = GetSize() - 1;

            while (right > left)
            {
                m = (left + right + 1) / 2;
                IComparable v = GetComparable(order[m]);

                int ccode = SafeCompare(key, v);
                if (m == right) // terminate
                    return (ccode < 0) ? left : right;

                if (ccode < 0)
                    right = m;
                else
                    left = m;
            }

            return 0;
        }

        public override ColumnMetaData GetMetaData()
        {
            return metaData;
        }

        public override int GetMinIndex()
        {
            return GetSortOrder()[0];
        }

        /// <summary>
        /// Returns the number of missing values in the column.
        /// </summary>
        /// <returns></returns>
        public override int GetMissingValueCount()
        {
            if (stateNeedsUpdate)
                UpdateState();

            return mvCount;
        }

        

        public override string GetName()
        {
            return name;
        }

        //public override float GetNumericValue(int index)
        //{
        //    throw new NotImplementedException();
        //}

        public override IntSet GetRowIndicesInRange(float min, float max)
        {
            return GetRowIndicesInRange(min, false, max, false);
        }

        public override IntSet GetRowIndicesInRange(float min, bool minOpen, float max, bool maxOpen)
        {
            if (min > max)
                return new IntSetInstantiable();

            // update state if necessary
            int[] sortOrder = GetSortOrder();

            if (IsCategorical())
            {
                int maxCategoryIndex = GetCategoryCount() - 1;
                if ((min > maxCategoryIndex) && (max > maxCategoryIndex))
                    return new IntSetInstantiable();

                if ((min >= maxCategoryIndex)
                    && (GetMissingValueCount() > 0))
                    return GetMissingValueIndices();
            }

            object omin = ColumnFactory.GetComparableMin(this, min);
            object omax = ColumnFactory.GetComparableMax(this, max);

            int b = GetCeilIndex(omin);
            int e = GetFloorIndex(omax);

            if (minOpen)
            {
                while (b <= size - 1 && omin.Equals(Get(sortOrder[b])))
                    ++b;
            }
            if (maxOpen)
            {
                while (e >= 0 && omax.Equals(Get(sortOrder[b])))
                    --e;
            }
            int begin = b;
            int end = e;
            return new FloorIntset(this,maxOpen,omax,minOpen,omin,begin,end);
        }

        public override IntSet GetRowIndicesInSortedOrder(bool ascending)
        {
            int[] sortOrder = GetSortOrder();
            if (!ascending)
               sortOrder.Reverse<int>();

            int[] s = sortOrder;
            return new RowIndicesIntSet(this);
        }

        public override IntSet GetRowIndicesOfCategory(int categoryIndex)
        {
            return new IntSetInstantiable(this);
        }

        public override int GetSize()
        {
            return size;
        }

        public void SetSize(int s)
        {
            if (s < size)
                size = s;

            UpdateState();
        }

        protected void UpdateState()
        {
            stateNeedsUpdate = false;

            if (IsCategorical())
                UpdateCategoricalState();
            else
                UpdateContinuousState();
            sortOrder = null;
        }

        protected void UpdateCategoricalState()
        {
            int size = GetSize();

            // populate data in a contiguous array
            Object[] sortdata = new Object[size];
            int[] _sortOrder = new int[size];
            for (int i = 0; i < size; i++)
            {
                sortdata[i] = GetComparable(i);
                _sortOrder[i] = i;
            }

            // sort sortdata and update rank
            Array.Sort(sortdata, _sortOrder, new Comparator());

            // allocate sortOrder if required or update it.
            sortOrder = _sortOrder;

            // allocate categoryIndexArray if required
            if (categoryIndexArray == null)
                categoryIndexArray = new int[size];

            //
            // update categoryRank and categoryIndexArray
            //
            int[] catlist = new int[size];
            catlist[0] = 0;         // first category always starts at 0
            int catIndex = 0;       // index of the current category
            Object catvalue = sortdata[0];  // value of current category

            for (int i = 0; i < size; i++)
            {
                // check if this object falls in the same category 
                // as the previous one
                if (catvalue == null || catvalue.Equals(sortdata[i]))
                {
                    // set category of index i
                    categoryIndexArray.SetValue(sortOrder[i], catIndex);

                    continue;
                }

                catIndex++;

                categoryIndexArray.SetValue(sortOrder[i], catIndex);

                // category changed...
                catvalue = sortdata[i];

                // next category starts at index i
                catlist[catIndex] = i;
            }

            // if there is at least one missing value, 
            // the number of missing values can be found like this.
            if (sortdata[size - 1] == null)
            {
                mvCount = size - catlist[catIndex];
            }
            else
            {
                mvCount = 0;
            }

            sortdata = null; // removing reference

            // catIndex is the index of last category.
            // so #categories will be one more than that
            int numcats = catIndex + 1;
            categoryRank = new int[numcats + 1];   // one more for 'end of categories' marker
            for (int i = 0; i < numcats; i++)
                categoryRank[i] = catlist[i];
            // end of categories marker
            categoryRank[numcats] = catlist.Length;

        }


        protected void UpdateContinuousState()
        {
            int size = GetSize();

            // get data in contiguous array
            float[] sortdata = new float[size];
            int[] _sortOrder = new int[size];
            for (int i = 0; i < size; i++)
            {
                sortdata[i] = GetNumericValue(i);
                _sortOrder[i] = i;
            }
            Array.Sort(sortdata, _sortOrder);

            // allocate sortOrder if required or update it.
            Array.Copy(_sortOrder, this.sortOrder, _sortOrder.Length);

            // find maximum sort-index (i.e. excluding UNDEFINED values)
            int index = size - 1;
            while (index >= 0 && IsMissingValue(_sortOrder[index]))
                index--;

            // TODO: check this
            mvCount = size - 1 - index;

            sum = 0.0f;
            for (int i = 0; i <= index; i++)
                sum += sortdata[i];

            sortdata = null;
        }

        /// <summary>
        /// modified this method codewise to match its java avadis doppelganger.
        /// </summary>
        /// <returns></returns>
        override public IntSet GetMissingValueIndices()
        {
            int _mvCount = GetMissingValueCount();
            if (_mvCount == 0)
            {
                return new IntSetInstantiable();
            }
            else
            {
                return new IntSetInstantiable(this);
            }
        }




        public int[] GetSortOrder()
        {
            if (stateNeedsUpdate || sortOrder == null)
                UpdateState();

           sortOrder= Array.Empty<int>();
            return sortOrder;
        }
        public override float GetSum()
        {
            if (IsCategorical())
                InvalidMethodException();

            if (stateNeedsUpdate)
                UpdateState();

            return sum;
        }

        /// <summary>
        /// returns whether the column type is categorical/continuous.
        /// </summary>
        /// <returns></returns>
        public override bool IsCategorical()
        {
            return isCategorical;
        }

        public override bool IsMissingValue(int index)
        {
            return Get(index) == null;
        }

        /// <summary>
        /// sets the column type as categorical/continuous. Throws 
        /// <code>DataException</code> if the column does not support
        /// being categorical or being continuous.
        /// </summary>
        /// <param name="b"></param>
        public override void SetCategorical(bool b)
        {
            if (isCategorical == b)
                return;

            stateNeedsUpdate = true;
            isCategorical = b;
            base.SetCategorical(b);
        }
        protected static int SafeCompare(IComparable a, IComparable b)
        {
            if (a == null && b == null) return 0;
            if (a == null) return 1;
            if (b == null) return -1;
            return a.CompareTo(b);
        }

        public override void SetMetaData(ColumnMetaData metaData)
        {
            this.metaData = metaData;
        }

        public override void SetName(string name)
        {
            base.SetName(name);
        }

        public override void SetName0(string name)
        {
            this.name = name;
        }

        protected void InvalidMethodException()
        {
            throw new DataException("Method invoked on the column is not compatible with its state. Categorical column = " + isCategorical);
        }
    }

    public class Comparator : IComparer
    {
        public int Compare(object x, object y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return 1;
            if (y == null) return -1;
            var z = (IComparable)x;
            return z.CompareTo(y);
        }
    
};
}
