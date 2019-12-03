using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using framework;

namespace com.strandgenomics.cube.dataset
{
    /// <summary>
    ///  Dataset implementation. IMPORTANT : To create a dataset
    ///  use the <code>DatasetFactory</code> class.
    /// </summary>
    public class Dataset : IDataset, IColumnChanged, IDataChanged
    {
        /// <summary>
        /// Hexff format id.
        /// </summary>
        public static string FORMAT_ID = "cube.dataset.Dataset";

        /// <summary>
        /// The name of the dataset.
        /// </summary>
        protected string name;

        /// <summary>
        /// The number of rows in the dataset.
        /// </summary>
        protected int rowCount;

        /// <summary>
        /// List of columns of the dataset.
        /// </summary>
        protected ArrayList columns;

        /// <summary>
        /// Mapping from column names to columns. 
        /// Used to get column from column name.
        /// </summary>
        protected Dictionary<string,IColumn> columnMap;

        /// <summary>
        /// Flag to indicate whether the dataset is locked or not.
        /// </summary>
        protected bool lockedFlag;



        public Dataset()
        {
        }


        public Dataset(string name, int rowCount)
        {
            this.name = name;
            this.rowCount = rowCount;
            lockedFlag = false;

            columns = new ArrayList();
            columnMap = new Dictionary<string, IColumn>();
        }


        public event ColumnChangedEventHandler ColumnChanged;
        public event DataChangedEventHandler DataChanged;

        private void Initialize()
        {
            columnMap = new Dictionary<string, IColumn>(GetColumnCount());

            UpdateColumnMap();
            UpdateColumnListners();

        }



        protected void FireDataChanged(DataChangedEventArgs e)
        {
            DataChanged?.Invoke(this, e);
        }



        protected void UpdateColumnMap()
        {
            columnMap.Clear();
            int columnCount = GetColumnCount();
            for (int i = 0; i < columnCount; i++)
            {
                IColumn c = GetColumn(i);
                columnMap.Add(c.GetName(), c);
            }
        }


        protected void UpdateColumnListners()
        {
            int columnCount = GetColumnCount();
            for (int i = 0; i < columnCount; i++)
            {
                IColumn c = GetColumn(i);
                c.AddColumnListener(this);
            }
        }

        public void AddColumn(IColumn column)
        {
            AddColumns(new IColumn[] { column });
        }

        public void AddColumns(IColumn[] cols)
        {
            ValidateNewColumns(cols);

            int columnCount = GetColumnCount();
            for (int i = 0; i < cols.Length; i++)
            {
                IColumn c = cols[i];

                columns.Add(c);
                columnMap.Add(c.GetName(), c);
                c.AddColumnListener(this);
            }
            FireColumnsAdded(Enumerable.Range(columnCount, columnCount + cols.Length).ToArray(), cols);
        }


        //#framework
        protected void FireColumnsAdded(int[] indices, IColumn[] columns)
        {
            DataChangedEventArgs args = new DataChangedEventArgs(this, DataChangedEventArgs.COLUMNS_ADDED);
            //args.columnIndices = indices;

            args.columns = columns;

            args.colCountBeforeChange = GetColumnCount()-columns.Length;
	        FireDataChanged(args);
        }

        protected void AssertUnlocked()
        {

            if (IsLocked())
                throw new DataException("Modifications are not allowed when the dataset is locked.");
        }


        public void RemoveColumns(IColumn[] cols)

        {

            AssertUnlocked();

            // Why should we worry whether columns are present or not.
            // isn't it an error to give columns like that??
            ArrayList removedColumns = new ArrayList();

            //#framework                        
            //BitSetIntSet indices = new BitSetIntSet(getColumnCount());

            int[] indices = new int[GetColumnCount()];

            for (int i = 0; i < cols.Length; i++)
            {
                int index = IndexOf(cols[i]);
                if (index != -1)
                {
                    indices.Append(index);
                    removedColumns.Add(cols[i]);
                }
            }
            int numColumnsRemoved = removedColumns.Count;
            if (numColumnsRemoved == 0)
                return;

            if (numColumnsRemoved != cols.Length)
                cols = (IColumn[])removedColumns.ToArray();

            for (int i = 0; i < cols.Length; i++)
            {
                columns.Remove(cols[i]);
                columnMap.Remove(cols[i].GetName());

                cols[i].RemoveColumnListener(this);
            }
            FireColumnsRemoved(indices, cols);
        }

        public void FireColumnsRemoved(int[] indices, IColumn[] columns)
        {
            DataChangedEventArgs args = new DataChangedEventArgs(this, DataChangedEventArgs.COLUMNS_REMOVED);
            //#framework
            //args.columnIndices = indices;

            args.columns = columns;

            args.colCountBeforeChange = GetColumnCount()+columns.Length;
	        FireDataChanged(args);
        }


        private void RemoveAllColumns()

        {
            AssertUnlocked();

            IColumn[] cols = (IColumn[])columns.ToArray();
            //#framework
            //BitSetIntSet indices = new BitSetIntSet(getColumnCount());
            int[] indices = new int[GetColumnCount()];

            columns.Clear();
            columnMap.Clear();

            for (int i = 0; i < cols.Length; i++)
            {
                indices.Append(i);
                cols[i].RemoveColumnListener(this);
            }
            FireColumnsRemoved(indices, cols);
        }



        public void Cleanup()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the column at the specified index.
        /// </summary>
        /// <param name="index">index of the column required.</param>
        /// <returns></returns>
        public IColumn GetColumn(int index)
        {
            if (index < 0 || index >= columns.Count)
                return null;

            return (IColumn)columns[index];
        }

        public IColumn GetColumn(string name)
        {
            return (IColumn)columnMap[name];
        }

        /// <summary>
        /// Returns the number of columns in the dataset.
        /// </summary>
        /// <returns></returns>
        public int GetColumnCount()
        {
            return columns.Count;
        }


        /// <summary>
        /// Returns name of the specified column.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetColumnName(int index)
        {
            return GetColumn(index).GetName();
        }



        /// <summary>
        /// Returns type of specified column
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetColumnType(int index)
        {
            return GetColumn(index).GetDatatype();
        }

        /// <summary>
        /// Returns the columnMetaData of the column with specified name.
        /// name of the column required
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ColumnMetaData GetMetaData(string name)
        {
            IColumn column = GetColumn(name);
            return column.GetMetaData();
        }

        public string GetName()
        {
            return name;
        }

        public object[] GetRow(int index)
        {
            int colCount = GetColumnCount();
            object[] objArray = new object[colCount];

            for (int i = 0; i < colCount; i++)
                objArray[i] = GetColumn(i).Get(index);

            return objArray;
        }

        /// <summary>
        /// Returns the number of rows in the dataset. Returns 0
        /// if there are no rows or columns.
        /// </summary>
        /// <returns></returns>
        public int GetRowCount()
        {
            return rowCount;
        }

        public IntSet GetRowIndicesInSortedOrder(string columnName, bool isAscending)
        {
            IColumn column = GetColumn(columnName);
            return column.GetRowIndicesInSortedOrder(isAscending);
        }

        //removed since it returns only null
        //public Map getGroupInfo()
        //{
        //    return null;
        //}

        public bool HasColumn(string name)
        {
            return columnMap.ContainsKey(name);
        }


        public int IndexOf(IColumn column)
        {
            // columns.indexOf method calls equals method, which is not desirable.
            // we need exact instance. so checking ==
            int colCount = GetColumnCount();

            for (int i = 0; i < colCount; i++)
                if (columns[i] == column)
                    return i;

            return -1;
        }

        /// <summary>
        /// Returns index of the specified column.
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public int IndexOf(string columnName)
        {
            return IndexOf(GetColumn(columnName));
        }

        public bool IsLocked()
        {
            return lockedFlag;
        }

        protected void AssertUniqueName(string name)
        {
            if (columnMap.ContainsKey(name))
            {
                throw new DataException("Duplicate column name: " + name);
            }
        }

        protected void AssertColumnSize(IColumn c)
        {
            if (rowCount != c.GetSize())
                throw new DataException("Invalid column. Sizes do not match.");
        }

        protected void ValidateNewColumn(IColumn c)
        {
            AssertUniqueName(c.GetName());
            AssertColumnSize(c);
        }

        protected void ValidateNewColumns(IColumn[] cols)
        {
            HashSet<string> set = new HashSet<string>(cols.Length);

            for (int i = 0; i < cols.Length; i++)
            {
                string name = cols[i].GetName();

                if (set.Contains(name) || columnMap.ContainsKey(name))
                    throw new DataException("Duplicate column name: " + name);

                if (rowCount != cols[i].GetSize())
                    throw new DataException("Invalid column. Sizes do not match.");

                set.Add(name);
            }
        }





        public void RemoveColumn(IColumn column)
        {
            RemoveColumns(new IColumn[] { column });
        }

        public void ReplaceColumn(int columnIndex, IColumn newColumn)
        {
            AssertUnlocked();

            IColumn oldColumn = GetColumn(columnIndex);

            if (!newColumn.GetName().Equals(oldColumn.GetName()))
                AssertUniqueName(newColumn.GetName());

            AssertColumnSize(newColumn);

            ColumnOperationUtil.UpdateMetaData(newColumn, oldColumn);

            columns[columnIndex]= newColumn;
            columnMap.Remove(oldColumn.GetName());
            columnMap.Add(newColumn.GetName(), newColumn);

            oldColumn.RemoveColumnListener(this);
            newColumn.AddColumnListener(this);

            FireColumnsReplaced(Enumerable.Range(columnIndex, columnIndex + 1).ToArray(),
                new IColumn[] { oldColumn }, new IColumn[] { newColumn });
        }

        //#framework
        protected void FireColumnsReplaced(int[] indices,
        IColumn[] oldColumns, IColumn[] newColumns)
        {

            DataChangedEventArgs args = new DataChangedEventArgs(this, DataChangedEventArgs.COLUMNS_REPLACED);
            //#framework
            //args.columnIndices = indices;

            args.oldColumns = oldColumns;

            args.newColumns = newColumns;

            args.colCountBeforeChange = GetColumnCount();
            FireDataChanged(args);
        }


        public void ColumnChangedEvent(ColumnChangedEventArgs e)
        {

            int type = e.GetChangeType();
            IColumn column = e.GetColumn();
            
            switch (type)
            {

                case ColumnChangedEventArgs.NAME_CHANGED:
                    if (!e.OldName.Equals(e.NewName) && HasColumn(e.NewName))
                    {
                        e.Cancel();
                        return;
                    }
                    goto case ColumnChangedEventArgs.STATE_CHANGED;

                // fall-through
                //
                case ColumnChangedEventArgs.STATE_CHANGED:
                    int index = IndexOf(column);

                    // fake a column replaced event.
                    FireColumnsReplaced(Enumerable.Range(index, index + 1).ToArray(),
                            new IColumn[] { column }, new IColumn[] { column });

                    //XXX: optimize
                    UpdateColumnMap();
                    break;
                default:
                    break;
            }
        }





        public void SetLock(bool b)
        {
            if (b && lockedFlag == b)
                throw new DataException("The dataset is already locked. Cannot lock the dataset again.");
            lockedFlag = b;
        }

        /// <summary>
        /// Sets the column meta data of column with the specified name.
        /// </summary>
        /// <param name="columnName">of the column required.</param>
        /// <param name="columnMetaData">for the column required</param>
        public void SetMetaData(string columnName, ColumnMetaData columnMetaData)
        {
            IColumn column = GetColumn(columnName);
            column.SetMetaData(columnMetaData);
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Still need to figure what this will do. after all the dependencies are built.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DataChangedHandler(object sender, DataChangedEventArgs e)
        {
           //does nothing for now 

        }

        public void AddDataListener(IDataChanged l)
        {
            l.DataChanged += DataChangedHandler;
        }

        public void RemoveDataListener(IDataChanged l)
        {
            l.DataChanged += DataChangedHandler;

        }
    }


    //not sure if we need the event dispatcher class. so holding it.


}
