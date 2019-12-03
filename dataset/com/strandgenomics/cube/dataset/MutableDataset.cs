using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using framework;

namespace com.strandgenomics.cube.dataset
{
    public class MutableDataset :Dataset, IMutableDataset, IColumnChanged, IDataChanged
    {
        /// <summary>
        /// Hexff format id.
        /// </summary>
        public new  static  string FORMAT_ID = "cube.dataset.MutableDataset";
        public static  int EMPTY = 0;

        public new  event ColumnChangedEventHandler ColumnChanged;

        public MutableDataset()
        {

        }

        public MutableDataset(string name):base(name,EMPTY)
        {
            
        }

        /// <summary>
        /// called by ReflectionSerializer.
        /// </summary>
        protected void Initialize()
        {
            columnMap = new Dictionary<string, IColumn>(GetColumnCount());

            UpdateColumnMap();
        }

        ///// <summary>
        ///// removed since this returns null
        ///// </summary>
        ///// <returns></returns>
        //public Dictionary<object,object> GetGroupInfo()
        //{
        //    return null;
        //}

        protected new void AssertColumnSize(IColumn c)
        {
            if (rowCount == EMPTY)
                rowCount = c.GetSize();
            if (rowCount != c.GetSize())
                throw new DataException("Invalid column. Sizes do not match.");
        }


        protected new void ValidateNewColumns(IColumn[] cols)
        {
            HashSet<string> set = new HashSet<string>(cols.Length);
            int count = rowCount;

            for (int i = 0; i < cols.Length; i++)
            {
                string name = cols[i].GetName();

                if (set.Contains(name) || columnMap.ContainsKey(name))
                    throw new DataException("Duplicate column name: " + name);
                if (count == EMPTY)
                    count = cols[i].GetSize();
                else if (count != cols[i].GetSize())
                    throw new DataException("Invalid column. Sizes do not match.");

                set.Add(name);
            }
        }

        public void addColumns(IColumn[] cols)
        {

            // all error check should be done before starting the append process.
            // it won't be nice to apped some of the columns and then fail.
            ValidateNewColumns(cols);

            bool isEmpty = (rowCount == EMPTY);

            int columnCount = GetColumnCount();
            for (int i = 0; i < cols.Length; i++)
            {
                IColumn c = cols[i];

                columns.Add(c);
                columnMap.Add(c.GetName(), c);
                c.AddColumnListener(this);
                if (rowCount == EMPTY)
                    rowCount = c.GetSize();
            }
            FireColumnsAdded(Enumerable.Range(columnCount, columnCount + cols.Length).ToArray(), cols);
            if (isEmpty)
            {
                //BitSetIntSet indices = new BitSetIntSet(rowCount);
                BitArray indices = new BitArray(rowCount);
                for (int i = 0; i < rowCount; i++)
                    indices.Set(i,true);
                //#framework
                //FireRowsAdded(indices);
            }
        }

        protected void FireRowsAdded(IntSet rowIndices)
        {
            DataChangedEventArgs e = new DataChangedEventArgs(this, DataChangedEventArgs.ROWS_ADDED);
            e.rowIndices = rowIndices;
            FireDataChanged(e);
        }

        protected void FireRowsAdded()
        {
            DataChangedEventArgs e = new DataChangedEventArgs(this, DataChangedEventArgs.ROWS_ADDED);
            FireDataChanged(e);
        }



        /// <summary>
        /// These are the only two functions that assume columns to be mutable .. 
        /// Columns can be converted to mutable columns when one of these operations is called.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="o"></param>
        public void SetValue(int row, int column, object o)
        {
            IMutableColumn col = null;
            if (row < GetRowCount() && column < GetColumnCount())
            {
                col = GetMutableColumn(column);
                col.SetValue(row, o);
            }
            else
                throw new DataException("Invalid row or column index.");
        }

        public void AddRow(List<object> data)
        {
            if (data.Count > GetColumnCount())
                throw new DataException("Mismatch in row size");
            for (int i = 0; i < data.Count; i++)
            {
                IMutableColumn col = GetMutableColumn(i);
                col.AddValue(data[i]);
            }
            //	    BitSetIntSet indices = new BitSetIntSet (getColumnCount());
            //	    for (int i=0; i<getColumnCount(); i++) 
            //		indices.add (i);
            //	    fireColumnsReplaced(indices, (IColumn[]) columns.toArray(), (IColumn[]) columns.toArray());
            //#framework
            //BitSetIntSet indices = new BitSetIntSet(1);
            BitArray indices = new BitArray(1);
            indices.Set(rowCount,true);

            rowCount++;
            //framework
            //FireRowsAdded(indices);
        }

        public void AddRow(Dictionary<object, object> data)
        {
            for (int i = 0; i < GetColumnCount(); i++)
            {
                IMutableColumn col = GetMutableColumn(i);
                col.AddValue(null);
            }
            IEnumerator it = data.Keys.GetEnumerator();
            while (it.MoveNext())
            {
                String key = (String)it.Current;
                IMutableColumn col = GetMutableColumn(key);
                if (col != null)
                    col.SetValue(rowCount, data[key]);
            }

            //	    BitSetIntSet indices = new BitSetIntSet (getColumnCount());
            //	    for (int i=0; i<getColumnCount(); i++) 
            //		indices.add (i);
            //	    fireColumnsReplaced(indices, (IColumn[]) columns.toArray(), (IColumn[]) columns.toArray());
            
            //#framework
            //BitSetIntSet indices = new BitSetIntSet(1);
            //indices.add(rowCount);

            //rowCount++;

            //FireRowsAdded(indices);
        }

        public void IncrementRowCount()
        {
            //#framework
            //BitSetIntSet indices = new BitSetIntSet(1);
            //indices.add(rowCount);

            //rowCount++;
            //FireRowsAdded(indices);
        }

        public IMutableColumn GetMutableColumn(int index)
        {
            IColumn col = (IColumn)columns[index];
            if (col is IMutableColumn)
                return (IMutableColumn)col;

            else
            {
                //MutableColumn mCol = new MutableColumn(col.getName(), !col.isCategorical());
                MutableColumn mCol = new MutableColumn(col);
                for (int i = 0; i < col.GetSize(); i++)
                    mCol.AddValue(col.Get(i));
                ReplaceColumn(index, mCol);
                return mCol;
            }
        }

        public IMutableColumn GetMutableColumn(String name)
        {
            IColumn col = GetColumn(name);
            if (col != null)
                return GetMutableColumn(IndexOf(col));
            else
                return null;
        }

    }
}
