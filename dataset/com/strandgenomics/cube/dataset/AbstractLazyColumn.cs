using framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.strandgenomics.cube.dataset
{
    public abstract class AbstractLazyColumn : AbstractColumn
    {
        protected IColumn column;
        protected string name;
        protected int size;
        protected ColumnMetaData metaData;


        protected abstract void CreateColumn();
        protected AbstractLazyColumn()
        {
        }

        public AbstractLazyColumn(string name, int size)
        {
            Init(name, size);
        }

        protected void Init(string name, int size)
        {
            this.name = name;
            this.size = size;
            this.metaData = new ColumnMetaData();
            this.ColumnChanged += ColumnChangedListener; //we will listen to columchanged events here.

        }

        protected IColumn GetColumn()
        {
            if (column == null)
            {
                CreateColumn();
            }
            return column;
        }

        override public void SetName(string name)
        {
            this.name = name;
            if (column != null)
                column.SetName(name);
        }



        /// <summary>
        /// this method handles waht to do when column is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected abstract void ColumnChangedListener(object sender, ColumnChangedEventArgs e);


        override public string GetName()
        {
            return name;
        }

        override public int GetSize()
        {
            if (size < 0)
                size = GetColumn().GetSize();

            return size;
        }

        override public void SetMetaData(ColumnMetaData metaData)
        {
            this.metaData = metaData;
            if (column != null)
                column.SetMetaData(metaData);
        }

        override public ColumnMetaData GetMetaData()
        {
            return metaData;
        }

        // {{{ IColumn implementation

        /**
         * return datatype
         */
        override public string GetDatatype()
        {
            IColumn c = GetColumn();
            // Arjun: hack to return datatype.
            // Needed when recreating column in session restore
            return (c == null) ? "string" : c.GetDatatype();
        }

        override public float GetNumericValue(int index)
        {
            return GetColumn().GetNumericValue(index);
        }

        override public int GetInt(int index)
        {
            return GetColumn().GetInt(index);
        }

        override public float GetFloat(int index)
        {
            return GetColumn().GetFloat(index);
        }

        override public object Get(int index)
        {
            return GetColumn().Get(index);
        }
        override public IComparable GetComparable(int index)
        {
            return GetColumn().GetComparable(index);
        }

        override public void SetCategorical(bool b)
        {
            GetColumn().SetCategorical(b);
        }

        override public bool IsCategorical()
        {
            return GetColumn().IsCategorical();
        }

        override public float GetSum()
        {
            return GetColumn().GetSum();
        }

        override public int GetCategoryCount()
        {
            return GetColumn().GetCategoryCount();
        }

        override public int GetCategorySize(int categoryIndex)
        {
            return GetColumn().GetCategorySize(categoryIndex);
        }

        override public object GetCategoryValue(int categoryIndex)
        {
            return GetColumn().GetCategoryValue(categoryIndex);
        }

        override public IntSet GetRowIndicesOfCategory(int categoryIndex)
        {
            return GetColumn().GetRowIndicesOfCategory(categoryIndex);
        }

        override public int GetCategoryIndex(int rowIndex)
        {
            return GetColumn().GetCategoryIndex(rowIndex);
        }

        override public bool IsMissingValue(int index)
        {
            return GetColumn().IsMissingValue(index);
        }

        override public int GetMissingValueCount()
        {
            return GetColumn().GetMissingValueCount();
        }

        override public IntSet GetMissingValueIndices()
        {
            return GetColumn().GetMissingValueIndices();
        }

        override public int GetMinIndex()
        {
            return GetColumn().GetMinIndex();
        }

        override public int GetMaxIndex()
        {
            return GetColumn().GetMaxIndex();
        }

        override public IntSet GetRowIndicesInSortedOrder(bool ascending)
        {
            return GetColumn().GetRowIndicesInSortedOrder(ascending);
        }

        override public IntSet GetRowIndicesInRange(float min, float max)
        {
            return GetColumn().GetRowIndicesInRange(min, max);
        }

        override public IntSet GetRowIndicesInRange(float min, bool minOpen, float max, bool maxOpen)
        {
            return GetColumn().GetRowIndicesInRange(min, minOpen, max, maxOpen);
        }

        public override void SetName0(string name)
        {
            this.name = name;
        }
        // }}}

    }
}

