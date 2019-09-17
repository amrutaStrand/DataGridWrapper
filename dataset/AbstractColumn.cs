using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using framework;

namespace dataset
{
    //omitted the event multicasters and add and remove listeners since they are not needed.

    public abstract class AbstractColumn : IColumn, IColumnChanged
    {
       
        public event ColumnChangedEventHandler ColumnChanged;

        //fireColumnEvent name changed to NotifyColumnChanged.
        protected void NotifyColumnChanged(ColumnChangedEventArgs e)
        {
            ColumnChanged?.Invoke(this, e);
        }

        protected bool CompareData(IColumn c)
        {
            int size = GetSize();

            for (int i = 0; i < size; i++)
            {
                object a = Get(i);
                object b = c.Get(i);

                if (a == null && b == null)
                    continue;

                if (a == null || b == null || !a.Equals(b))
                    return false;
            }

            return true;
        }

        public abstract object Get(int index);


        public abstract int GetCategoryCount();


        public abstract int GetCategoryIndex(int rowIndex);


        public abstract int GetCategorySize(int categoryIndex);


        public abstract object GetCategoryValue(int categoryIndex);


        public abstract IComparable GetComparable(int index);


        public abstract string GetDatatype();


        public abstract float GetFloat(int index);


        public abstract int GetInt(int index);


        public abstract int GetMaxIndex();


        public abstract ColumnMetaData GetMetaData();


        public abstract int GetMinIndex();


        public abstract int GetMissingValueCount();


        public abstract IntSet GetMissingValueIndices();


        public abstract string GetName();
       

        public abstract float GetNumericValue(int index);

        public abstract IntSet GetRowIndicesInRange(float min, float max);

        public abstract IntSet GetRowIndicesInRange(float min, bool minOpen, float max, bool maxOpen);

        public abstract IntSet GetRowIndicesInSortedOrder(bool ascending);

        public abstract IntSet GetRowIndicesOfCategory(int categoryIndex);

        public abstract int GetSize();

        public abstract float GetSum();

        public abstract bool IsCategorical();

        public abstract bool IsMissingValue(int index);

        /// <summary>
        /// Use base.SetCategorical() at the start of your hild class implementation.
        /// </summary>
        /// <param name="b"></param>
        public virtual void SetCategorical(bool b)
        {
            ColumnChangedEventArgs e = new ColumnChangedEventArgs(DatasetConstants.STATE_CHANGED);
            e.Column = this;
            try
            {
                NotifyColumnChanged(e);
            }
            catch (DataException)
            {
                throw;
            }
        }

        public abstract void SetMetaData(ColumnMetaData metaData);

        /// <summary>
        /// function to change only property name of the column.
        /// </summary>
        /// <param name="name"></param>
        public abstract void SetName0(string name);
        public virtual void SetName(string name)
        {
            ColumnChangedEventArgs e = new ColumnChangedEventArgs(DatasetConstants.NAME_CHANGED);
            string oldName = GetName();
            string newName = name;

            e.OldName = oldName;
            e.NewName = newName;

            
            NotifyColumnChanged(e);
            SetName0(e.NewName);
            if(e.Cancelled)
            {
                SetName0(e.OldName);
                throw new DataException("duplicate column name: " + newName);
            }



        }
    }
}
