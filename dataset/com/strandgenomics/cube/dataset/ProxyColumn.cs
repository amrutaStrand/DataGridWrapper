using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using framework;

namespace com.strandgenomics.cube.dataset
{
    public abstract class ProxyColumn: AbstractColumn
    {
        protected IColumn master;

        public ProxyColumn(IColumn master)
        {
            this.master = master;
        }

        protected ProxyColumn()
        {
        }

        override public string GetName()
        {
            return master.GetName();
        }

        override public void SetName0(string name)
        {
            master.SetName(name);
        }

        override public String GetDatatype()
        {
            return master.GetDatatype();
        }

        override public float GetNumericValue(int index)
        {
            index = GetMasterIndex(index);
            return master.GetNumericValue(index);
        }

        override public int GetInt(int index)
        {
            index = GetMasterIndex(index);
            return master.GetInt(index);
        }

        override public float GetFloat(int index)
        {
            index = GetMasterIndex(index);
            return master.GetFloat(index);
        }

        override public Object Get(int index)
        {
            index = GetMasterIndex(index);
            return master.Get(index);
        }

        override public IComparable GetComparable(int index)
        {
            index = GetMasterIndex(index);
            return master.GetComparable(index);
        }

        override public ColumnMetaData GetMetaData()
        {
            return master.GetMetaData();
        }

        override public void SetMetaData(ColumnMetaData metadata)
        {
            master.SetMetaData(metadata);
        }

        override public void SetCategorical(bool b)
        {
            master.SetCategorical(b);
            base.SetCategorical(b);
        }

        override public bool IsCategorical()
        {
            return master.IsCategorical();
        }

        override public bool IsMissingValue(int index)
        {
            index = GetMasterIndex(index);
            return master.IsMissingValue(index);
        }

        override public int GetCategoryCount()
        {
            return master.GetCategoryCount();
        }

        override public int GetCategorySize(int categoryIndex)
        {
            return master.GetCategorySize(categoryIndex);
        }

        override public object GetCategoryValue(int categoryIndex)
        {
            var index = GetRowIndicesOfCategory(categoryIndex).GetEnumerator();

            if (index.MoveNext())
                return Get(index.Current);
            else
                return null;
        }

        override  public IntSet GetRowIndicesOfCategory(int categoryIndex)
        {
            return Map(master.GetRowIndicesOfCategory(categoryIndex));
        }

        // ?
        override public int GetCategoryIndex(int rowIndex)
        {
            int masterIndex = GetMasterIndex(rowIndex);
            return master.GetCategoryIndex(masterIndex);
        }

        // ?
        override  public int GetMissingValueCount()
        {
            return master.GetMissingValueCount();
        }

        override  public IntSet GetMissingValueIndices()
        {
            return Map(master.GetMissingValueIndices());
        }

        // ?
        override  public float GetSum()
        {
            return master.GetSum();
        }

        override public int GetMinIndex()
        {
            return GetReverseIndex(master.GetMinIndex());
        }

        override public int GetMaxIndex()
        {
            return GetReverseIndex(master.GetMaxIndex());
        }

        override public IntSet GetRowIndicesInSortedOrder(bool ascending)
        {
            return Map(master.GetRowIndicesInSortedOrder(ascending));
        }

        override public IntSet GetRowIndicesInRange(float min, float max)
        {
            return Map(master.GetRowIndicesInRange(min, max));
        }

        override public IntSet GetRowIndicesInRange(float min, bool minOpen, float max, bool maxOpen)
        {
            return Map(master.GetRowIndicesInRange(min, minOpen, max, maxOpen));
        }

        /**
         * Converts from rowIndex to masterRowIndex. 
         * Tells where this[index] maps in the master.
         */
        protected int GetMasterIndex(int index)
        {
            return index;
        }

        /**
         * Converts from masterRowIndex to rowIndex. 
         * Tells where master[masterIndex] maps in this column.
         */
        protected int GetReverseIndex(int masterIndex)
        {
            return masterIndex;
        }

        /** Converts masterRowIndices to rowIndices. */
        protected IntSet Map(IntSet masterIndices)
        {
            return masterIndices;
        }
    }
}
